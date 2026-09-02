"""M1 Public API adapter.

No SQL endpoint is used here. All ERP reads and writes are sent to the M1 Public
API, and writes stay disabled unless explicitly enabled in production settings.
"""

from __future__ import annotations

from collections import Counter
from typing import Any

from app_config import boolean_setting, integer_setting, setting


class M1Error(RuntimeError):
    pass


class M1Client:
    _home_currency_cache: dict[tuple[str, str], str] = {}

    def __init__(self, base_url: str | None = None, api_id: str | None = None, api_key: str | None = None,
                 timeout_seconds: int | None = None, page_size: int | None = None):
        configured_url = (base_url or setting("M1_API_BASE_URL", "")).strip()
        if configured_url and "://" not in configured_url:
            configured_url = f"http://{configured_url}"
        self.base_url = configured_url.rstrip("/")
        self.api_id = api_id or setting("M1_API_ID", "")
        self.api_key = api_key or setting("M1_API_KEY", "")
        self.timeout_seconds = max(1, timeout_seconds or integer_setting("M1_API_TIMEOUT_SECONDS", 120))
        self.page_size = min(1000, max(1, page_size or integer_setting("M1_API_PAGE_SIZE", 250)))

    @property
    def configured(self) -> bool:
        return bool(self.base_url and self.api_id and self.api_key)

    @property
    def writes_enabled(self) -> bool:
        return boolean_setting("M1_WRITES_ENABLED", False)

    def _request(self, method: str, resource: str, **kwargs: Any) -> dict[str, Any]:
        try:
            import requests
        except ModuleNotFoundError as exc:
            raise M1Error("requests is required; install requirements.txt") from exc
        if not self.configured:
            raise M1Error("M1 Public API is not configured")
        try:
            response = requests.request(
                method,
                f"{self.base_url}/api/ERP/{resource}",
                headers={
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": f"apikey {self.api_id}:{self.api_key}",
                },
                # Fail fast when the host is unreachable, but allow M1 enough
                # time to materialize and serialize its larger ERP resources.
                timeout=(10, self.timeout_seconds),
                **kwargs,
            )
            response.raise_for_status()
        except requests.RequestException as exc:
            response = getattr(exc, "response", None)
            status = f"HTTP {response.status_code}" if response is not None else "connection error"
            detail = "" if response is not None else f"{type(exc).__name__}: {exc}"
            if response is not None:
                try:
                    body = response.json()
                    validation = body.get("validationInfo") or body.get("ValidationInfo") or {}
                    errors = validation.get("errorsList") or validation.get("ErrorsList") or body.get("errors") or body.get("message")
                    if isinstance(errors, list):
                        detail = "; ".join(str(value) for value in errors if value)
                    elif errors:
                        detail = str(errors)
                    else:
                        detail = str(body)
                except ValueError:
                    detail = response.text.strip()
            suffix = f": {detail[:1000]}" if detail else ""
            raise M1Error(f"M1 {resource} request failed ({status}){suffix}") from exc
        return response.json() if response.content else {}

    def get(self, resource: str, filters: list[str] | None = None) -> list[dict[str, Any]]:
        payload = self._request("GET", resource, params=[("filter", value) for value in (filters or [])])
        return payload.get("returnObject") or []

    # A stable sort for every paged read. M1 paginates with
    # "ORDER BY <orderBy> OFFSET n ROWS FETCH NEXT m ROWS ONLY", and when no
    # orderBy is supplied the repositories fall back to the literal "1" -- an
    # ordinal reference to the FIRST COLUMN OF THE SELECT LIST, which is
    # cmcAlternatePhoneNumber for contacts, cmlAddressLine1 for locations,
    # cmoAccountManagerEmployeeID for organizations and ompApprovalDecisionDate
    # for sales orders. Those are near-total ties, so SQL Server is free to order
    # the rows differently for each page and OFFSET/FETCH then silently skips and
    # duplicates rows. Paging any table larger than one page without this returns
    # an incomplete result with no error.
    PAGED_ORDER_BY = {
        "Organizations": "cmoOrganizationID[Asc]",
        "OrganizationLocations": "cmlOrganizationID[Asc],cmlLocationID[Asc]",
        "OrganizationContacts": "cmcOrganizationID[Asc],cmcLocationID[Asc],cmcContactID[Asc]",
        "SalesOrders": "ompSalesOrderID[Asc]",
        "SalesOrderLines": "omlSalesOrderID[Asc],omlSalesOrderLineID[Asc]",
        "SalesOrderDeliveries": "omdSalesOrderID[Asc],omdSalesOrderLineID[Asc],omdSalesOrderDeliveryID[Asc]",
    }

    def _page_order_by(self, resource: str) -> str:
        # M1 silently ignores an unknown orderBy field and reverts to "ORDER BY 1",
        # so an unlisted resource must fail loudly rather than page unreliably.
        try:
            return self.PAGED_ORDER_BY[resource]
        except KeyError:
            raise M1Error(f"No stable pagination sort is configured for {resource}") from None

    def get_all(self, resource: str, filters: list[str] | None = None) -> list[dict[str, Any]]:
        rows: list[dict[str, Any]] = []
        page_size = self.page_size
        page_number = 0
        while True:
            params: list[tuple[str, Any]] = [
                ("pageSize", page_size), ("pageNumber", page_number),
                ("orderBy", self._page_order_by(resource)),
            ]
            params.extend(("filter", value) for value in (filters or []))
            page = self._request("GET", resource, params=params).get("returnObject") or []
            rows.extend(page)
            if len(page) < page_size:
                return rows
            page_number += 1

    def organization_details(self, organization_id: str) -> dict[str, Any] | None:
        organizations = self.get("Organizations", [f"cmoOrganizationID[eq]{organization_id}"])
        if not organizations:
            return None
        return {
            "organization": organizations[0],
            "locations": self.get("OrganizationLocations", [f"cmlOrganizationID[eq]{organization_id}"]),
            "contacts": self.get("OrganizationContacts", [f"cmcOrganizationID[eq]{organization_id}"]),
        }

    def organizations_by_email(self, email: str) -> list[dict[str, Any]]:
        email = email.strip()
        return self.get("Organizations", [f"cmoEmailAddress[eq]{email}"]) if email else []

    def home_currency_id(self) -> str:
        cache_key = (self.base_url, self.api_id)
        if cache_key in self._home_currency_cache:
            return self._home_currency_cache[cache_key]
        rows = self.get("DatasetProperties")
        dataset = rows[0] if rows else {}
        currency_fields = [value for key, value in dataset.items() if key.lower() == "xadcurrencyrateid"]
        if currency_fields:
            # M1 represents the dataset's home/base currency with a blank ID.
            # Only foreign currencies have a CurrencyRates foreign key.
            currency_id = str(currency_fields[0] or "").strip()
        else:
            historical = self.shopify_sales_orders_by_po().values()
            counts = Counter(
                str(next((value for key, value in row.items() if key.lower() == "ompcurrencyrateid"), "") or "").strip()
                for row in historical
            )
            currency_id = counts.most_common(1)[0][0] if counts else ""
        if not currency_fields and not historical:
            raise M1Error("M1 did not return currency metadata from DatasetProperties or historical Shopify sales orders")
        self._home_currency_cache[cache_key] = currency_id
        return currency_id

    def put(self, resource: str, payload: dict[str, Any]) -> dict[str, Any]:
        if not self.writes_enabled:
            raise M1Error("M1 writes are disabled; set M1_WRITES_ENABLED=true only after preflight approval")
        return self._request("PUT", resource, json=payload)

    def find_sales_order_by_po(self, customer_po: str) -> dict[str, Any] | None:
        rows = self.get("SalesOrders", [f"ompCustomerPo[eq]{customer_po}"])
        return rows[0] if rows else None

    def shopify_sales_orders_by_po(self) -> dict[str, dict[str, Any]]:
        """Load historical ``S-`` orders once for startup reconciliation.

        The M1 endpoint has no starts-with operator. The numeric Shopify order
        names fit inside this narrow range; a local prefix check removes nearby
        non-Shopify customer POs returned by the database collation.
        """
        page_size = self.page_size
        page_number = 0
        result: dict[str, dict[str, Any]] = {}
        while True:
            payload = self._request(
                "GET",
                "SalesOrders",
                params=[
                    ("pageSize", page_size),
                    ("pageNumber", page_number),
                    # Without this the reconciliation silently misses M1 orders and
                    # the app re-creates them. See PAGED_ORDER_BY.
                    ("orderBy", self._page_order_by("SalesOrders")),
                    ("filter", "ompCustomerPo[gt]S-0"),
                    ("filter", "ompCustomerPo[lt]S-999999999"),
                ],
            )
            rows = payload.get("returnObject") or []
            for row in rows:
                customer_po = str(row.get("ompCustomerPo") or row.get("ompCustomerPO") or "").strip()
                if customer_po.startswith("S-"):
                    result[customer_po] = row
            if len(rows) < page_size:
                break
            page_number += 1
        return result

    # Key field of each table this application allocates IDs for, so a candidate
    # can be probed for an existing record the way M1.Core.NextIDList does.
    NEXT_ID_KEY_FIELDS = {"SalesOrders": "ompSalesOrderID", "Organizations": "cmoOrganizationID"}

    @staticmethod
    def _format_at(mask: str, position: int) -> str:
        return "X" if not mask.strip() else mask[position]

    @classmethod
    def increment_id(cls, value: str, *, numeric_only: bool, increment_amount: int = 1) -> str:
        """Port of ``M1.Core.NextIDList.IncrementValue`` for nvarchar key fields.

        A numeric-only table gets a "999..." mask, which M1 short-circuits to plain
        arithmetic. Otherwise the mask is blank and M1 walks the ID from the right,
        carrying through 'Z' and rolling '9' over to 'A'.
        """
        increment_amount = max(increment_amount, 1)
        text = str(value).strip()
        if numeric_only:
            return str(int(text) + increment_amount) if text.lstrip("-").isdigit() else "1"
        head, tail = text.split("-", 1)[0], ""
        carrying = True
        while carrying:
            carrying = False
            if not head:
                tail = ("A" if cls._format_at("", 0) == "A" else "1") + tail
                continue
            position, character, head = len(head) - 1, head[-1], head[:-1]
            if ("A" <= character < "Z") or ("0" <= character < "9"):
                tail = chr(ord(character) + 1) + tail
                continue
            if character == "Z":
                tail = ("A" if cls._format_at("", position) == "A" else "0") + tail
                carrying = True
            elif character == "9":
                if cls._format_at("", position) == "9":
                    tail, carrying = "0" + tail, True
                else:
                    tail = "A" + tail
            elif character == "#":
                tail = "0" + tail
            else:
                raise M1Error(f"Invalid character {character!r} in M1 ID {value!r}")
        return head + tail

    def _next_id_row(self, table: str) -> dict[str, Any]:
        rows = self.get("NextIDs", [f"xanTable[eq]{table}"])
        if not rows:
            raise M1Error(f"M1 did not return a next ID for {table}")
        return rows[0]

    def next_id(self, table: str) -> str:
        """Allocate the next key for ``table`` and reserve it in M1's NextIDs row.

        The Public API exposes NextIDs as a plain CRUD table: nothing advances
        xanNextID when a record is created, so reading it alone hands out the same
        ID on every commit and the second one fails with a duplicate key. The M1
        desktop skips IDs that are already taken and then writes the incremented
        value back; both halves are reproduced here.
        """
        key_field = self.NEXT_ID_KEY_FIELDS.get(table)
        if not key_field:
            raise M1Error(f"No M1 key field is configured for {table}")
        for attempt in range(3):
            row = self._next_id_row(table)
            numeric_only = int(row.get("xanNumericOnly") or 0) != 0
            increment_amount = max(int(row.get("xanIncrementAmount") or 0), 1)
            candidate = str(row["xanNextID"]).strip()
            for _ in range(1000):
                if not self.get(table, [f"{key_field}[eq]{candidate}"]):
                    break
                candidate = self.increment_id(candidate, numeric_only=numeric_only, increment_amount=increment_amount)
            else:
                raise M1Error(f"M1 has no unused {table} ID near {row['xanNextID']}")
            reserved = self.increment_id(candidate, numeric_only=numeric_only, increment_amount=increment_amount)
            try:
                self.put("NextIDs", {**row, "xanNextID": reserved})
            except M1Error as exc:
                # Another writer advanced the row between the read and the write.
                # Re-read and re-probe rather than handing back a claimed ID.
                if "row version" not in str(exc).lower() or attempt == 2:
                    raise
                continue
            return candidate
        raise M1Error(f"M1 NextIDs for {table} kept changing; could not reserve an ID")

    def next_child_id(self, resource: str, organization_id: str, organization_field: str, id_field: str, *, start: int, step: int) -> str:
        rows = self.get(resource, [f"{organization_field}[eq]{organization_id}"])
        used = {int(str(row.get(id_field, "")).strip()) for row in rows if str(row.get(id_field, "")).strip().isdigit()}
        candidate = start
        while candidate in used:
            candidate += step
        return str(candidate)

    def probe(self) -> dict[str, Any]:
        rows = self.get("NextIDs", ["xanTable[eq]SalesOrders"])
        return {"ok": bool(rows), "writes_enabled": self.writes_enabled}
