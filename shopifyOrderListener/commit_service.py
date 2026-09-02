"""Human-confirmed M1 creation boundary."""

from __future__ import annotations

from datetime import datetime, timezone
from decimal import Decimal
from typing import Any
import uuid

from app_config import setting
from customer_matching import CustomerMatcher
from integrations.m1 import M1Client, M1Error
from storage import source_hash


class CommitError(RuntimeError):
    pass


def build_m1_resource_plan(order: dict[str, Any], sales_order_id: str, organization_id: str, location_id: str, contact_id: str, defaults: dict[str, Any] | None = None,
                           billing_location_id: str | None = None, billing_contact_id: str | None = None,
                           currency_rate_id: str | None = None) -> dict[str, Any]:
    defaults = defaults or {}
    order_date = (order.get("created_at") or datetime.now(timezone.utc).isoformat())[:10]
    shipping_method = "FEDHD" if "home" in order.get("shipping_method", "").lower() else "FDXGR"
    lines = []
    deliveries = []
    for index, line in enumerate(order.get("lines", []), start=1):
        quantity = Decimal(str(line["current_quantity"]))
        unit_price = Decimal(str(line["unit_price"]))
        extended = Decimal(str(line["line_total"]))
        lines.append({
            "omlUniqueID": str(uuid.uuid4()),
            "omlSalesOrderID": sales_order_id,
            "omlSalesOrderLineID": index,
            "omlPartID": line["sku"],
            "omlPartRevisionID": "",
            "omlUnitOfMeasure": defaults.get("default_uom") or setting("M1_DEFAULT_UOM", "EA"),
            "omlPartShortDescription": line["description"][:50],
            "omlOrderQuantity": str(quantity),
            # M1 only maintains this running total in the desktop DD engine (the
            # omdDeliveryQuantity -> omlDeliveryQuantityTotal bound-parent sum).
            # The Public API writes the column verbatim, so the line must carry the
            # sum of its own deliveries or M1 raises "Line order quantity does not
            # equal the sum of the deliveries". One delivery per line is created below.
            "omlDeliveryQuantityTotal": str(quantity),
            "omlFullUnitPriceBase": str(unit_price),
            "omlFullUnitPriceForeign": str(unit_price),
            "omlUnitPriceBase": str(unit_price),
            "omlUnitPriceForeign": str(unit_price),
            "omlFullExtendedPriceBase": str(extended),
            "omlFullExtendedPriceForeign": str(extended),
            "omlExtendedPriceBase": str(extended),
            "omlExtendedPriceForeign": str(extended),
        })
        deliveries.append({
            "omdUniqueID": str(uuid.uuid4()),
            "omdSalesOrderID": sales_order_id,
            "omdSalesOrderLineID": index,
            "omdSalesOrderDeliveryID": 1,
            "omdCustomerOrganizationID": organization_id,
            "omdPartID": line["sku"],
            "omdPartRevisionID": "",
            "omdPartWarehouseLocationID": defaults.get("default_warehouse") or setting("M1_DEFAULT_WAREHOUSE", "142"),
            "omdPartBinID": defaults.get("default_bin") or setting("M1_DEFAULT_BIN", "BIN1"),
            "omdDeliveryQuantity": str(quantity),
            "omdDeliveryDate": order_date,
            "omdDeliveryType": 2,
            "omdFirm": True,
            "omdShipLocationID": location_id,
            "omdShipContactID": contact_id,
            "omdShippingMethodID": shipping_method,
            "omdShippingPaymentTypeID": "SHIP",
        })
    subtotal = sum((Decimal(str(line["line_total"])) for line in order.get("lines", [])), Decimal("0"))
    shipping = Decimal(str(order.get("shipping") or 0))
    tax = Decimal(str(order.get("tax") or 0))
    # currentTotalPriceSet is Shopify's authoritative, already-discounted value.
    # Derive the discount represented by the M1 components instead of blindly
    # applying currentTotalDiscountsSet, which may include a shipping discount
    # already reflected in currentShippingPriceSet.
    total = Decimal(str(order.get("total") or 0))
    discount = max(Decimal("0"), subtotal + shipping + tax - total)
    billing_location_id, billing_contact_id = billing_location_id or location_id, billing_contact_id or contact_id
    header = {
        "ompUniqueID": str(uuid.uuid4()),
        "ompSalesOrderID": sales_order_id,
        "ompCustomerPo": order["customer_po"],
        "ompRequestedShipDate": order_date,
        "ompOrderDate": order_date,
        "ompCustomerOrganizationID": organization_id,
        "ompShipOrganizationID": organization_id,
        "ompShipLocationID": location_id,
        "ompShipContactID": contact_id,
        "ompArInvoiceLocationID": billing_location_id,
        "ompArInvoiceContactID": billing_contact_id,
        "ompPaymentTermID": "PREPA",
        "ompCurrencyRateID": (
            currency_rate_id
            if currency_rate_id is not None
            else defaults.get("currency_rate_id") or setting("M1_CURRENCY_RATE_ID", "")
        ),
        "ompExchangeRate": "1",
        "ompFullOrderSubtotalBase": str(subtotal),
        "ompFullOrderSubtotalForeign": str(subtotal),
        "ompDiscountTotalBase": str(discount),
        "ompDiscountTotalForeign": str(discount),
        "ompFreightAmountBase": str(shipping),
        "ompFreightAmountForeign": str(shipping),
        "ompFreightTotalBase": str(shipping),
        "ompFreightTotalForeign": str(shipping),
        "ompOrderSubtotalBase": str(subtotal),
        "ompOrderSubTotalForeign": str(subtotal),
        "ompOrderTaxAmountBase": str(tax),
        "ompOrderTaxAmountForeign": str(tax),
        "ompOrderTotalBase": str(total),
        "ompOrderTotalForeign": str(total),
        "ompStatus": 3,
        "ompOrderCommentsText": order.get("note", "")[:50],
        "ompShippingMethodID": shipping_method,
        "ompShippingPaymentTypeID": "SHIP",
    }
    return {"SalesOrders": [header], "SalesOrderLines": lines, "SalesOrderDeliveries": deliveries}


def build_customer_resources(order: dict[str, Any], organization_id: str, location_id: str, contact_id: str,
                             billing_location_id: str | None = None, billing_contact_id: str | None = None) -> dict[str, list[dict[str, Any]]]:
    address = order.get("shipping_address") or {}
    supplied_billing = order.get("billing_address") or {}
    billing = supplied_billing if supplied_billing.get("address1") else address
    billing_location_id, billing_contact_id = billing_location_id or location_id, billing_contact_id or contact_id
    organization_name = (address.get("company") or order.get("customer_name") or address.get("name") or organization_id)[:50]
    contact_name = (address.get("name") or order.get("customer_name") or organization_name)[:50]
    def common(source): return {
        "AddressLine1": source.get("address1", "")[:50], "AddressLine2": source.get("address2", "")[:50],
        "City": source.get("city", "")[:30], "State": source.get("province", "")[:3], "PostCode": source.get("postal_code", "")[:10],
        "CountryCode": source.get("country", "")[:2], "Phone": (source.get("phone") or order.get("phone") or "")[:20],
    }
    common_address, billing_address = common(address), common(billing)
    organization = {
        "cmoUniqueID": str(uuid.uuid4()), "cmoOrganizationID": organization_id,
        "cmoName": organization_name, "cmoAddressLine1": common_address["AddressLine1"],
        "cmoAddressLine2": common_address["AddressLine2"], "cmoCity": common_address["City"],
        "cmoState": common_address["State"], "cmoPostCode": common_address["PostCode"],
        "cmoCountryCode": common_address["CountryCode"], "cmoPhoneNumber": common_address["Phone"],
        "cmoEmailAddress": order.get("email", "")[:50], "cmoCustomerStatus": 2,
        # cmoCustomerStatus 2 is Active. In the M1 client, cmoCustomerStatus_ValueChanged
        # stamps cmoCustomerActiveDate = Date whenever the status becomes 2, and the
        # data dictionary makes the field required while it is (dfRequiredExpression
        # "CInt(Fields(\"cmocustomerstatus\").Value) = 2"). The Public API runs no such
        # handler, so an API-created customer would be Active with no active date.
        # Local rather than UTC: this mirrors the VBScript Date the desktop would
        # have written, which is the M1 server's own day.
        "cmoCustomerActiveDate": datetime.now().date().isoformat(),
        "cmoCustomerGroupID": "CG01", "cmoCustomerPaymentTermsID": "PREPA",
        "cmoCustomerShippingMethodID": "FEDHD" if "home" in order.get("shipping_method", "").lower() else "FDXGR",
        "cmoCustomerShipPaymentTypeID": "SHIP",
        # The organization-level default contacts (cmoShipContactID and
        # cmoArInvoiceContactID) are deliberately absent. See
        # _write_customer_resources: M1's Public API rejects them unconditionally.
        # The location-level equivalents below are set, and the sales order names
        # its own ompShipContactID / ompArInvoiceContactID, so nothing depends on
        # the organization-level defaults.
        "cmoDefaultShipLocationID": location_id,
        "cmoDefaultArInvoiceLocationID": billing_location_id,
    }
    def make_location(location_value, source, common_value, invoice, ship, invoice_contact, ship_contact): return {
        "cmlUniqueID": str(uuid.uuid4()), "cmlOrganizationID": organization_id,
        "cmlLocationID": location_value, "cmlName": (source.get("company") or source.get("name") or organization_name)[:50],
        "cmlAddressLine1": common_value["AddressLine1"], "cmlAddressLine2": common_value["AddressLine2"], "cmlCity": common_value["City"], "cmlState": common_value["State"],
        "cmlPostCode": common_value["PostCode"], "cmlCountryCode": common_value["CountryCode"], "cmlPhoneNumber": common_value["Phone"], "cmlEmailAddress": order.get("email", "")[:50],
        "cmlArInvoiceLocation": invoice, "cmlShipLocation": ship, "cmlArInvoiceContactID": invoice_contact, "cmlShipContactID": ship_contact, "cmlCustomerPaymentTermID": "PREPA",
        "cmlCustomerShipPaymentTypeID": "SHIP",
    }
    def make_contact(contact_value, location_value, source, phone): return {
        "cmcUniqueID": str(uuid.uuid4()), "cmcOrganizationID": organization_id,
        "cmcLocationID": location_value, "cmcContactID": contact_value, "cmcName": (source.get("name") or contact_name)[:50],
        "cmcEmailAddress": order.get("email", "")[:50], "cmcPhoneNumber": phone,
        "cmcInactive": False,
    }
    same_location = location_id == billing_location_id
    locations = [make_location(location_id, address, common_address, same_location, True, billing_contact_id if same_location else "", contact_id)]
    contacts = [make_contact(contact_id, location_id, address, common_address["Phone"])]
    if not same_location:
        locations.append(make_location(billing_location_id, billing, billing_address, True, False, billing_contact_id, ""))
    if billing_contact_id != contact_id or billing_location_id != location_id:
        contacts.append(make_contact(billing_contact_id, billing_location_id, billing, billing_address["Phone"]))
    return {"Organizations": [organization], "OrganizationLocations": locations, "OrganizationContacts": contacts}


def preview_fingerprint(order: dict[str, Any], settings: dict[str, Any]) -> str:
    """Hash every input ``CommitService.preview`` reads out of local storage.

    A stored preview is only served while this matches, so a mutation that
    forgets to refresh the preview degrades to a rebuild rather than to a stale
    answer.
    """
    return source_hash({
        "source": order.get("source_hash"),
        "state": order.get("state"),
        "erp_order_id": order.get("erp_order_id"),
        "blocks_commit": order.get("blocks_commit"),
        "action_detail": order.get("action_detail"),
        "organization_id": order.get("matched_organization_id"),
        "location_id": order.get("matched_location_id"),
        "contact_id": order.get("matched_contact_id"),
        "billing_location_id": order.get("matched_billing_location_id"),
        "billing_contact_id": order.get("matched_billing_contact_id"),
        "address_override": bool(order.get("address_override")),
        "line_overrides": order.get("line_overrides"),
        "settings": {key: settings.get(key) for key in ("default_uom", "default_warehouse", "default_bin", "currency_rate_id")},
    })


class CommitService:
    def __init__(self, store: Any, m1: M1Client | None = None):
        self.store = store
        self.m1 = m1 or M1Client()

    @staticmethod
    def _returned_record(response: dict[str, Any]) -> dict[str, Any] | None:
        value = response.get("returnObject") or response.get("ReturnObject")
        if isinstance(value, list):
            value = value[0] if value else None
        return dict(value) if isinstance(value, dict) else None

    def _write_customer_resources(self, resources: dict[str, list[dict[str, Any]]]) -> None:
        """Create CRM records without sending references to children too early.

        M1 validates contact and location references before each PUT. Creating a
        fully-populated organization first therefore fails because its new
        locations and contacts do not exist yet. Locations have the same
        dependency on contacts. Create the parent shells first, create contacts,
        then update the returned full DTOs (including their row versions).

        The organization's own contact references are never sent. M1's
        ValidateRequest_PutOrganization checks each of them with
        DoesRecordExistInTableUsingKeys("OrganizationContacts", <3 key columns>,
        <2 values>), and that helper returns false whenever the two arrays differ
        in length. Any non-blank cmoShipContactID / cmoArInvoiceContactID is
        therefore rejected with "not found" no matter what exists in M1, and the
        API offers no PATCH to set them later. The same defect affects
        cmoQuoteContactID, cmoPurchaseContactID and cmoApInvoiceContactID.
        OrganizationLocations passes 3 values for its 3 key columns, so the
        location-level contacts this method does send validate correctly.
        """
        blank_organization_contacts = {
            "cmoArInvoiceContactID": "", "cmoShipContactID": "", "cmoQuoteContactID": "",
            "cmoPurchaseContactID": "", "cmoApInvoiceContactID": "",
        }
        organization_updates: list[tuple[dict[str, Any], dict[str, Any]]] = []
        location_updates: list[tuple[dict[str, Any], dict[str, Any]]] = []

        for intended in resources.get("Organizations", []):
            shell = {
                **intended,
                **blank_organization_contacts,
                "cmoDefaultArInvoiceLocationID": "",
                "cmoDefaultShipLocationID": "",
            }
            created = self._returned_record(self.m1.put("Organizations", shell))
            if created:
                organization_updates.append((created, intended))

        for intended in resources.get("OrganizationLocations", []):
            shell = {**intended, "cmlArInvoiceContactID": "", "cmlShipContactID": ""}
            created = self._returned_record(self.m1.put("OrganizationLocations", shell))
            if created:
                location_updates.append((created, intended))

        for payload in resources.get("OrganizationContacts", []):
            self.m1.put("OrganizationContacts", payload)

        for created, intended in location_updates:
            self.m1.put("OrganizationLocations", {
                **created,
                "cmlArInvoiceContactID": intended.get("cmlArInvoiceContactID", ""),
                "cmlShipContactID": intended.get("cmlShipContactID", ""),
            })

        for created, intended in organization_updates:
            self.m1.put("Organizations", {
                **created,
                **blank_organization_contacts,
                "cmoDefaultArInvoiceLocationID": intended.get("cmoDefaultArInvoiceLocationID", ""),
                "cmoDefaultShipLocationID": intended.get("cmoDefaultShipLocationID", ""),
            })

    def _recover_partial_customer(self, order: dict[str, Any]) -> dict[str, Any] | None:
        """Find CRM records created by an earlier attempt that failed at SalesOrders."""
        email = str(order.get("email") or "").strip()
        if not email or not hasattr(self.m1, "organizations_by_email"):
            return None
        matches: list[dict[str, Any]] = []
        for organization in self.m1.organizations_by_email(email):
            organization_id = str(organization.get("cmoOrganizationID") or "").strip()
            if not organization_id:
                continue
            resolution = CustomerMatcher(self.m1).resolution(order, organization_id=organization_id)
            selection = resolution.get("selection") or {}
            identifiers = (
                selection.get("location_id"), selection.get("contact_id"),
                selection.get("billing_location_id"), selection.get("billing_contact_id"),
            )
            if resolution.get("action") == "use_existing" and all(value and value != "NEW" for value in identifiers):
                matches.append(resolution)
        return matches[0] if len(matches) == 1 else None

    def preview(self, order_id: str) -> dict[str, Any]:
        order = self.store.get_order(order_id)
        if not order:
            raise KeyError(order_id)
        if order.get("blocks_commit"):
            raise CommitError(order.get("action_detail") or "This order is blocked for review")
        organization_id = order.get("matched_organization_id")
        location_id = order.get("matched_location_id") or ""
        contact_id = order.get("matched_contact_id") or ""
        billing_location_id = order.get("matched_billing_location_id") or location_id
        billing_contact_id = order.get("matched_billing_contact_id") or contact_id
        if not organization_id:
            raise CommitError("Select or create a customer organization before committing")
        if organization_id != "__NEW__":
            validation = CustomerMatcher(self.m1).validate_selection(
                order, organization_id, location_id, contact_id, billing_location_id, billing_contact_id
            )
            if not validation["safe"] and not order.get("address_override"):
                raise CommitError(validation["detail"])
        existing = self.m1.find_sales_order_by_po(order["customer_po"])
        if existing:
            return {"already_exists": True, "erp_order_id": existing.get("ompSalesOrderID") or existing.get("SalesOrderID")}
        placeholder = "<allocated by M1 at commit>"
        customer_plan = None
        if organization_id == "__NEW__":
            customer_plan = {"mode": "create_organization_location_contact", "organization_id": "<allocated by M1>", "location_id": "100", "contact_id": "1"}
            organization_id, location_id, contact_id = "<new organization>", "100", "1"
        elif location_id == "NEW" or contact_id == "NEW":
            customer_plan = {"mode": "add_missing_location_or_contact", "organization_id": organization_id, "location_id": location_id, "contact_id": contact_id}
            location_id = "<new location>" if location_id == "NEW" else location_id
            contact_id = "<new contact>" if contact_id == "NEW" else contact_id
        currency_rate_id = self.m1.home_currency_id()
        return {"already_exists": False, "customer_plan": customer_plan, "resources": build_m1_resource_plan(order, placeholder, organization_id, location_id, contact_id, self.store.get_settings(), billing_location_id, billing_contact_id, currency_rate_id)}

    def preview_envelope(self, order_id: str) -> dict[str, Any]:
        """``preview`` plus the blocking reasons, so both outcomes can be stored."""
        try:
            return {"ok": True, "preview": self.preview(order_id)}
        except CommitError as exc:
            return {"ok": False, "detail": str(exc)}

    def refresh_preview(self, order_id: str) -> dict[str, Any]:
        """Rebuild the preview from M1 and store it against the current inputs."""
        order = self.store.get_order(order_id)
        if not order:
            raise KeyError(order_id)
        envelope = self.preview_envelope(order_id)
        self.store.set_m1_preview(order_id, envelope, preview_fingerprint(order, self.store.get_settings()))
        return envelope

    def cached_preview(self, order_id: str) -> dict[str, Any]:
        """Serve the stored preview, rebuilding only when its inputs have moved.

        On the happy path this is a single SQL read, so opening an order never
        waits on M1 -- including on the first request after a restart, when the
        in-process customer directory cache is empty.
        """
        order = self.store.get_order(order_id)
        if not order:
            raise KeyError(order_id)
        stored = order.get("m1_preview")
        if stored and order.get("m1_preview_fingerprint") == preview_fingerprint(order, self.store.get_settings()):
            return stored
        return self.refresh_preview(order_id)

    def commit(self, order_id: str, actor: str) -> dict[str, Any]:
        preview = self.preview(order_id)
        if preview["already_exists"]:
            return self.store.mark_committed(order_id, str(preview["erp_order_id"]).strip())
        if not self.m1.writes_enabled:
            raise CommitError("M1 writes are disabled until API preflight is approved")
        order = self.store.get_order(order_id)
        sales_order_id = self.m1.next_id("SalesOrders")
        organization_id = order["matched_organization_id"]
        location_id = order.get("matched_location_id") or ""
        contact_id = order.get("matched_contact_id") or ""
        billing_location_id = order.get("matched_billing_location_id") or location_id
        billing_contact_id = order.get("matched_billing_contact_id") or contact_id
        if organization_id == "__NEW__":
            recovered = self._recover_partial_customer(order)
            if recovered:
                selection = recovered["selection"]
                order = self.store.set_match(
                    order_id, selection["organization_id"], selection["location_id"], selection["contact_id"], False,
                    selection["billing_location_id"], selection["billing_contact_id"],
                    {"safe": True, "status": "partial_commit_recovered", "detail": "Reused customer records created by an earlier M1 attempt."},
                )
                self.store.set_customer_resolution(order_id, recovered)
                organization_id = selection["organization_id"]
                location_id = selection["location_id"]
                contact_id = selection["contact_id"]
                billing_location_id = selection["billing_location_id"]
                billing_contact_id = selection["billing_contact_id"]
        customer_resources: dict[str, list[dict[str, Any]]] = {}
        if organization_id == "__NEW__":
            organization_id = self.m1.next_id("Organizations")
            ship = order.get("shipping_address") or {}
            supplied_bill = order.get("billing_address") or {}
            bill = supplied_bill if supplied_bill.get("address1") else ship
            same = all(str(ship.get(key) or "").strip().lower() == str(bill.get(key) or "").strip().lower() for key in ("address1", "address2", "city", "province", "postal_code", "country"))
            billing_location_id, billing_contact_id = "100", "1"
            location_id, contact_id = ("100", "1") if same else ("200", "2")
            customer_resources = build_customer_resources(order, organization_id, location_id, contact_id, billing_location_id, billing_contact_id)
        else:
            shipping_source = order.get("shipping_address") or {}
            billing_source = order.get("billing_address") or {}
            same_order_address = not billing_source.get("address1") or all(
                str(shipping_source.get(key) or "").strip().lower() == str(billing_source.get(key) or "").strip().lower()
                for key in ("address1", "address2", "city", "province", "postal_code", "country")
            )
            same_new_contact = (
                order.get("matched_contact_id") == "NEW"
                and order.get("matched_billing_contact_id") == "NEW"
                and order.get("matched_location_id") == order.get("matched_billing_location_id")
                and same_order_address
            )
            same_new_location = (
                order.get("matched_location_id") == "NEW"
                and order.get("matched_billing_location_id") == "NEW"
                and same_order_address
            )
            if location_id == "NEW":
                location_id = self.m1.next_child_id("OrganizationLocations", organization_id, "cmlOrganizationID", "cmlLocationID", start=100, step=100)
            if contact_id == "NEW":
                contact_id = self.m1.next_child_id("OrganizationContacts", organization_id, "cmcOrganizationID", "cmcContactID", start=1, step=1)
            if same_new_location:
                billing_location_id = location_id
            elif billing_location_id == "NEW":
                billing_location_id = self.m1.next_child_id("OrganizationLocations", organization_id, "cmlOrganizationID", "cmlLocationID", start=100, step=100)
                if billing_location_id == location_id: billing_location_id = str(int(billing_location_id) + 100)
            if same_new_contact:
                billing_contact_id = contact_id
            elif billing_contact_id == "NEW":
                billing_contact_id = self.m1.next_child_id("OrganizationContacts", organization_id, "cmcOrganizationID", "cmcContactID", start=1, step=1)
                if billing_contact_id == contact_id: billing_contact_id = str(int(billing_contact_id) + 1)
            if any(order.get(key) == "NEW" for key in ("matched_location_id", "matched_contact_id", "matched_billing_location_id", "matched_billing_contact_id")):
                generated = build_customer_resources(order, organization_id, location_id, contact_id, billing_location_id, billing_contact_id)
                def append_unique(resource: str, row: dict[str, Any], key: str) -> None:
                    target = customer_resources.setdefault(resource, [])
                    if not any(existing[key] == row[key] for existing in target):
                        target.append(row)
                if order.get("matched_location_id") == "NEW":
                    append_unique("OrganizationLocations", next(row for row in generated["OrganizationLocations"] if row["cmlLocationID"] == location_id), "cmlLocationID")
                if order.get("matched_contact_id") == "NEW":
                    append_unique("OrganizationContacts", next(row for row in generated["OrganizationContacts"] if row["cmcContactID"] == contact_id), "cmcContactID")
                if order.get("matched_billing_location_id") == "NEW":
                    append_unique("OrganizationLocations", next(row for row in generated["OrganizationLocations"] if row["cmlLocationID"] == billing_location_id), "cmlLocationID")
                if order.get("matched_billing_contact_id") == "NEW":
                    append_unique("OrganizationContacts", next(row for row in generated["OrganizationContacts"] if row["cmcContactID"] == billing_contact_id), "cmcContactID")
        currency_rate_id = preview["resources"]["SalesOrders"][0]["ompCurrencyRateID"]
        resources = {**customer_resources, **build_m1_resource_plan(order, sales_order_id, organization_id, location_id, contact_id, self.store.get_settings(), billing_location_id, billing_contact_id, currency_rate_id)}
        self.store.add_event(order_id, "m1_commit_started", {"sales_order_id": sales_order_id}, actor)
        try:
            self._write_customer_resources(customer_resources)
            if customer_resources:
                order = self.store.set_match(
                    order_id, organization_id, location_id, contact_id, False, billing_location_id, billing_contact_id,
                    {"safe": True, "status": "m1_customer_resources_created", "detail": "M1 customer records were created before the sales order."},
                )
                self.store.set_customer_resolution(order_id, None)
            for resource in ("SalesOrders", "SalesOrderLines", "SalesOrderDeliveries"):
                for payload in resources.get(resource, []):
                    self.m1.put(resource, payload)
        except M1Error:
            self.store.add_event(order_id, "m1_commit_failed", {"sales_order_id": sales_order_id}, actor)
            raise
        if (
            order.get("matched_organization_id") != organization_id
            or order.get("matched_location_id") != location_id
            or order.get("matched_contact_id") != contact_id
            or order.get("matched_billing_location_id") != billing_location_id
            or order.get("matched_billing_contact_id") != billing_contact_id
        ):
            self.store.set_match(order_id, organization_id, location_id, contact_id, False, billing_location_id, billing_contact_id, order.get("match_validation"))
        return self.store.mark_committed(order_id, sales_order_id)
