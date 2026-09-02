"""Loose, explainable M1 customer discovery and address safety checks."""
from __future__ import annotations

import re
import threading
import time
from collections import defaultdict
from difflib import SequenceMatcher
from typing import Any

from integrations.m1 import M1Client


def _text(value: Any) -> str:
    words = re.findall(r"[a-z0-9]+", str(value or "").lower().replace("&", " and "))
    aliases = {"street": "st", "road": "rd", "avenue": "ave", "boulevard": "blvd", "drive": "dr", "lane": "ln", "suite": "ste"}
    return " ".join(aliases.get(word, word) for word in words)


def _digits(value: Any) -> str:
    return re.sub(r"\D", "", str(value or ""))[-10:]


def _address(record: dict[str, Any], prefix: str) -> dict[str, str]:
    return {"name": str(record.get(f"{prefix}Name") or ""), "address1": str(record.get(f"{prefix}AddressLine1") or ""),
            "address2": str(record.get(f"{prefix}AddressLine2") or ""), "city": str(record.get(f"{prefix}City") or ""),
            "province": str(record.get(f"{prefix}State") or ""), "postal_code": str(record.get(f"{prefix}PostCode") or ""),
            "country": str(record.get(f"{prefix}CountryCode") or record.get(f"{prefix}Country") or "")}


def _address_key(value: dict[str, Any] | None) -> str:
    value = value or {}
    if not value.get("address1"):
        return ""
    return "|".join(_text(value.get(field)) for field in ("address1", "address2", "city", "province", "postal_code"))


def compare_addresses(shopify: dict[str, Any] | None, m1: dict[str, Any] | None) -> dict[str, Any]:
    shopify, m1 = shopify or {}, m1 or {}
    if not shopify.get("address1"):
        return {"status": "missing_shopify", "matches": False, "detail": "Shopify did not provide this address."}
    fields = {"address1": _text(shopify.get("address1")) == _text(m1.get("address1")),
              "address2": _text(shopify.get("address2")) == _text(m1.get("address2")),
              "city": _text(shopify.get("city")) == _text(m1.get("city")),
              "province": _text(shopify.get("province")) == _text(m1.get("province")),
              "postal_code": _text(shopify.get("postal_code")) == _text(m1.get("postal_code")),
              "country": not shopify.get("country") or not m1.get("country") or _text(shopify.get("country")) == _text(m1.get("country"))}
    matches = all(fields.values())
    return {"status": "match" if matches else "mismatch", "matches": matches, "fields": fields, "shopify": shopify, "m1": m1,
            "detail": "Address matches Shopify." if matches else "Selected M1 location differs from Shopify."}


class _Directory:
    lock = threading.Lock()
    loaded_at = 0.0
    data: dict[str, dict[str, Any]] = {}
    indexes: dict[str, dict[str, set[str]]] = {}
    source_key: tuple[Any, ...] | None = None

    @classmethod
    def load(cls, m1: M1Client, max_age: int = 300) -> dict[str, dict[str, Any]]:
        with cls.lock:
            source_key = (getattr(m1, "base_url", None), getattr(m1, "api_id", None))
            if not any(source_key):
                source_key = (id(m1),)
            if cls.source_key == source_key and cls.data and time.monotonic() - cls.loaded_at < max_age:
                return cls.data
            data: dict[str, dict[str, Any]] = {}
            for row in m1.get_all("Organizations"):
                oid = str(row.get("cmoOrganizationID") or "").strip()
                if oid:
                    data[oid] = {"organization": row, "locations": [], "contacts": []}
            for row in m1.get_all("OrganizationLocations"):
                oid = str(row.get("cmlOrganizationID") or "").strip()
                if oid in data:
                    data[oid]["locations"].append(row)
            for row in m1.get_all("OrganizationContacts"):
                oid = str(row.get("cmcOrganizationID") or "").strip()
                if oid in data:
                    data[oid]["contacts"].append(row)
            indexes: dict[str, dict[str, set[str]]] = {
                name: defaultdict(set) for name in ("email", "phone", "name", "address")
            }
            for oid, details in data.items():
                org = details["organization"]
                for value in (org.get("cmoEmailAddress"), *(row.get("cmlEmailAddress") for row in details["locations"]), *(row.get("cmcEmailAddress") for row in details["contacts"])):
                    if _text(value): indexes["email"][_text(value)].add(oid)
                for value in (org.get("cmoPhoneNumber"), *(row.get("cmlPhoneNumber") for row in details["locations"]), *(row.get("cmcPhoneNumber") for row in details["contacts"])):
                    if _digits(value): indexes["phone"][_digits(value)].add(oid)
                for value in (org.get("cmoName"), *(row.get("cmlName") for row in details["locations"]), *(row.get("cmcName") for row in details["contacts"])):
                    if _text(value): indexes["name"][_text(value)].add(oid)
                for value in (_address(org, "cmo"), *(_address(row, "cml") for row in details["locations"])):
                    if _address_key(value): indexes["address"][_address_key(value)].add(oid)
            cls.data, cls.indexes, cls.source_key, cls.loaded_at = data, indexes, source_key, time.monotonic()
            return data


def _shape(details: dict[str, Any]) -> dict[str, Any]:
    org = details["organization"]
    return {"organization_id": str(org.get("cmoOrganizationID") or "").strip(), "name": org.get("cmoName") or "",
            "email": org.get("cmoEmailAddress") or "", "phone": org.get("cmoPhoneNumber") or "", "address": _address(org, "cmo"),
            "locations": [{"location_id": str(row.get("cmlLocationID") or "").strip(), "name": row.get("cmlName") or "", "email": row.get("cmlEmailAddress") or "", "phone": row.get("cmlPhoneNumber") or "", "address": _address(row, "cml")} for row in details["locations"]],
            "contacts": [{"contact_id": str(row.get("cmcContactID") or "").strip(), "location_id": str(row.get("cmcLocationID") or "").strip(), "name": row.get("cmcName") or "", "email": row.get("cmcEmailAddress") or "", "phone": row.get("cmcPhoneNumber") or ""} for row in details["contacts"]]}


class CustomerMatcher:
    def __init__(self, m1: M1Client | None = None):
        self.m1 = m1 or M1Client()

    def organization(self, organization_id: str) -> dict[str, Any] | None:
        organization_id = organization_id.strip()
        # The synchronization pass loads the M1 customer directory once. Reuse
        # that snapshot for direct organization recommendations instead of
        # issuing three API calls per backlogged order.
        details = _Directory.load(self.m1).get(organization_id) if hasattr(self.m1, "get_all") else None
        if details is None:
            details = self.m1.organization_details(organization_id)
        return _shape(details) if details else None

    def candidates(self, order: dict[str, Any], query: str = "", *, include_fuzzy: bool = True) -> list[dict[str, Any]]:
        emails = {_text(value) for value in (order.get("email"),) if value}
        phones = {_digits(value) for value in (order.get("phone"), (order.get("shipping_address") or {}).get("phone"), (order.get("billing_address") or {}).get("phone")) if _digits(value)}
        names = {_text(value) for value in (order.get("customer_name"), (order.get("shipping_address") or {}).get("name"), (order.get("billing_address") or {}).get("name")) if value}
        companies = {_text(value) for value in ((order.get("shipping_address") or {}).get("company"), (order.get("billing_address") or {}).get("company")) if value}
        query_norm, results = _text(query), []
        directory = _Directory.load(self.m1)
        if not include_fuzzy and not query_norm:
            candidate_ids: set[str] = set()
            for value in emails: candidate_ids.update(_Directory.indexes["email"].get(value, set()))
            for value in phones: candidate_ids.update(_Directory.indexes["phone"].get(value, set()))
            for value in names | companies: candidate_ids.update(_Directory.indexes["name"].get(value, set()))
            for value in (_address_key(order.get("shipping_address")), _address_key(order.get("billing_address"))):
                if value: candidate_ids.update(_Directory.indexes["address"].get(value, set()))
            detail_rows = (directory[oid] for oid in candidate_ids)
        else:
            detail_rows = directory.values()
        for details in detail_rows:
            shaped, org = _shape(details), details["organization"]
            org_name = _text(org.get("cmoName"))
            record_emails = {_text(org.get("cmoEmailAddress")), *(_text(row.get("cmcEmailAddress")) for row in details["contacts"]), *(_text(row.get("cmlEmailAddress")) for row in details["locations"])} - {""}
            record_phones = {_digits(org.get("cmoPhoneNumber")), *(_digits(row.get("cmcPhoneNumber")) for row in details["contacts"]), *(_digits(row.get("cmlPhoneNumber")) for row in details["locations"])} - {""}
            record_names = {org_name, *(_text(row.get("cmcName")) for row in details["contacts"]), *(_text(row.get("cmlName")) for row in details["locations"])} - {""}
            evidence: list[dict[str, Any]] = []
            if emails & record_emails: evidence.append({"field": "email", "kind": "exact", "label": "Email exact", "score": 100})
            if phones & record_phones: evidence.append({"field": "phone", "kind": "exact", "label": "Phone exact", "score": 96})
            if names & record_names: evidence.append({"field": "name", "kind": "exact", "label": "Name exact", "score": 90})
            if companies & record_names: evidence.append({"field": "company", "kind": "exact", "label": "Business name exact", "score": 95})
            if include_fuzzy:
                best_name = max((SequenceMatcher(None, source, target).ratio() for source in names for target in record_names), default=0)
                best_company = max((SequenceMatcher(None, source, target).ratio() for source in companies for target in record_names), default=0)
                if best_name >= .68 and not any(item["field"] == "name" for item in evidence):
                    evidence.append({"field": "name", "kind": "fuzzy", "label": f"Name similar ({best_name:.0%})", "score": round(55 + best_name * 35)})
                if best_company >= .68 and not any(item["field"] == "company" for item in evidence):
                    evidence.append({"field": "company", "kind": "fuzzy", "label": f"Business name similar ({best_company:.0%})", "score": round(55 + best_company * 35)})
            location_matches = []
            address_match_labels = []
            address_records = [("organization", shaped["address"]), *((row["location_id"], row["address"]) for row in shaped["locations"])]
            for location in shaped["locations"]:
                shipping = compare_addresses(order.get("shipping_address"), location["address"])
                billing = compare_addresses(order.get("billing_address"), location["address"])
                if shipping["matches"] or billing["matches"]: location_matches.append({"location_id": location["location_id"], "shipping": shipping["matches"], "billing": billing["matches"]})
            if any(compare_addresses(order.get("shipping_address"), address)["matches"] for _, address in address_records):
                address_match_labels.append("Shipping address exact")
            billing_source = order.get("billing_address") or {}
            if billing_source.get("address1") and any(compare_addresses(billing_source, address)["matches"] for _, address in address_records):
                address_match_labels.append("Billing address exact")
            if address_match_labels:
                evidence.append({"field": "address", "kind": "exact", "label": " + ".join(address_match_labels), "score": 98})
            searchable = " ".join([shaped["organization_id"], shaped["name"], shaped["email"], shaped["phone"], *(row["name"] + " " + row["email"] + " " + row["phone"] for row in shaped["contacts"]), *(row["name"] + " " + " ".join(row["address"].values()) for row in shaped["locations"])])
            if query_norm and query_norm in _text(searchable): evidence.append({"field": "manual_search", "kind": "contains", "label": "Manual search text", "score": 65})
            if not evidence: continue
            score = min(100, max(item["score"] for item in evidence) + 2 * (len(evidence) - 1) + (4 if location_matches else 0))
            results.append({**shaped, "score": score, "evidence": evidence, "reasons": [item["label"] for item in evidence], "location_matches": location_matches})
        return sorted(results, key=lambda row: (-row["score"], row["organization_id"]))

    def search(self, order: dict[str, Any], query: str = "", *, include_fuzzy: bool = True) -> dict[str, Any]:
        candidates = self.candidates(order, query, include_fuzzy=include_fuzzy)
        if query.strip():
            candidates = [candidate for candidate in candidates if any(item["field"] == "manual_search" for item in candidate["evidence"])]
        sources = {
            "name": bool(order.get("customer_name") or (order.get("shipping_address") or {}).get("name") or (order.get("billing_address") or {}).get("name")),
            "company": bool((order.get("shipping_address") or {}).get("company") or (order.get("billing_address") or {}).get("company")),
            "email": bool(order.get("email")),
            "phone": bool(order.get("phone") or (order.get("shipping_address") or {}).get("phone") or (order.get("billing_address") or {}).get("phone")),
            "address": bool((order.get("shipping_address") or {}).get("address1") or (order.get("billing_address") or {}).get("address1")),
        }
        fields = {}
        for field, provided in sources.items():
            count = sum(any(item["field"] == field for item in candidate["evidence"]) for candidate in candidates)
            fields[field] = {"provided": provided, "match_count": count, "matched": bool(count)}
        return {
            "candidates": candidates,
            "candidate_count": len(candidates),
            "automatic_selection_id": candidates[0]["organization_id"] if len(candidates) == 1 else None,
            "fields": fields,
            "no_shopify_match": not any(any(item["field"] != "manual_search" for item in candidate["evidence"]) for candidate in candidates),
        }

    @staticmethod
    def _contact_for(candidate: dict[str, Any], location_id: str, order: dict[str, Any]) -> str:
        if location_id == "NEW": return "NEW"
        order_email = _text(order.get("email"))
        order_phones = {_digits(value) for value in (order.get("phone"), (order.get("shipping_address") or {}).get("phone"), (order.get("billing_address") or {}).get("phone")) if _digits(value)}
        order_names = {_text(value) for value in (order.get("customer_name"), (order.get("shipping_address") or {}).get("name"), (order.get("billing_address") or {}).get("name")) if value}
        scored = []
        for contact in candidate.get("contacts", []):
            if contact.get("location_id") and contact["location_id"] != location_id: continue
            score = (6 if order_email and _text(contact.get("email")) == order_email else 0) + (4 if _digits(contact.get("phone")) in order_phones and _digits(contact.get("phone")) else 0) + (2 if _text(contact.get("name")) in order_names else 0)
            if score: scored.append((score, contact["contact_id"]))
        scored.sort(reverse=True)
        return scored[0][1] if scored and (len(scored) == 1 or scored[0][0] > scored[1][0]) else "NEW"

    def _recommend_candidate(self, order: dict[str, Any], candidate: dict[str, Any], *, historical: bool = False) -> dict[str, Any]:
        if "location_matches" not in candidate:
            candidate = dict(candidate)
            candidate["location_matches"] = []
            for location in candidate.get("locations", []):
                shipping = compare_addresses(order.get("shipping_address"), location["address"])
                billing = compare_addresses(order.get("billing_address"), location["address"])
                if shipping["matches"] or billing["matches"]:
                    candidate["location_matches"].append({"location_id": location["location_id"], "shipping": shipping["matches"], "billing": billing["matches"]})
        ship_match = next((row for row in candidate.get("location_matches", []) if row.get("shipping") and row.get("location_id")), None)
        bill_source = order.get("billing_address") or {}
        same_address = not bill_source.get("address1") or compare_addresses(order.get("shipping_address"), bill_source)["matches"]
        bill_match = ship_match if same_address else next((row for row in candidate.get("location_matches", []) if row.get("billing") and row.get("location_id")), None)
        shipping_location = ship_match["location_id"] if ship_match else "NEW"
        billing_location = shipping_location if same_address else (bill_match["location_id"] if bill_match else "NEW")
        shipping_contact = self._contact_for(candidate, shipping_location, order)
        billing_contact = shipping_contact if same_address and billing_location == shipping_location else self._contact_for(candidate, billing_location, order)
        adding_locations = shipping_location == "NEW" or billing_location == "NEW"
        adding_contacts = shipping_contact == "NEW" or billing_contact == "NEW"
        if adding_locations:
            action, title = "add_location", f"Use {candidate['name']} and add the Shopify address"
        elif adding_contacts:
            action, title = "add_contact", f"Use {candidate['name']} and add the Shopify contact"
        else:
            action, title = "use_existing", f"Use existing customer {candidate['name']}"
        exact_reasons = [item["label"] for item in candidate.get("evidence", []) if item["kind"] == "exact"]
        why = "Previously used for this Shopify customer." if historical else ("Matched by " + ", ".join(exact_reasons).lower() + "." if exact_reasons else "Closest available name match.")
        location_by_id = {row["location_id"]: row for row in candidate.get("locations", [])}
        contact_by_id = {row["contact_id"]: row for row in candidate.get("contacts", [])}
        def route(label: str, location_id: str, contact_id: str) -> dict[str, Any]:
            location = location_by_id.get(location_id)
            contact = contact_by_id.get(contact_id)
            return {"label": label, "location_id": location_id, "location_text": "Add from Shopify" if location_id == "NEW" else f"{location_id} · {location.get('name') or location['address'].get('address1')}",
                    "contact_id": contact_id, "contact_text": "Add from Shopify" if contact_id == "NEW" else f"{contact_id} · {contact.get('name')}"}
        return {"status": "recommended", "action": action, "confidence": "high" if historical or exact_reasons else "medium", "title": title, "why": why,
                "organization": candidate, "selection": {"organization_id": candidate["organization_id"], "location_id": shipping_location, "contact_id": shipping_contact,
                "billing_location_id": billing_location, "billing_contact_id": billing_contact},
                "routes": [route("Ship to", shipping_location, shipping_contact), route("Bill to", billing_location, billing_contact)], "primary_label": "Use this customer setup"}

    def resolution(self, order: dict[str, Any], returning_match: dict[str, Any] | None = None, organization_id: str = "") -> dict[str, Any]:
        def new_customer(confidence: str, why: str, possible_match_count: int = 0) -> dict[str, Any]:
            return {"status": "recommended", "action": "create_new", "confidence": confidence, "title": f"Create {order.get('shipping_address', {}).get('company') or order.get('customer_name') or 'a new customer'} in M1",
                    "why": why, "organization": None,
                    "selection": {"organization_id": "__NEW__", "location_id": "NEW", "contact_id": "NEW", "billing_location_id": "NEW", "billing_contact_id": "NEW"},
                    "routes": [{"label": "Ship to", "location_text": "Create from Shopify", "contact_text": "Create from Shopify"}, {"label": "Bill to", "location_text": "Create from Shopify", "contact_text": "Create from Shopify"}],
                    "primary_label": "Create this customer setup", "possible_match_count": possible_match_count}
        if organization_id == "__NEW__":
            return new_customer("high", "The Shopify customer will be created with its billing and shipping details.")
        if organization_id:
            candidate = self.organization(organization_id)
            if not candidate: return {"status": "not_found", "title": "M1 customer not found"}
            return {**self._recommend_candidate(order, candidate), "possible_match_count": 1}
        if returning_match and returning_match.get("organization_id"):
            candidate = self.organization(returning_match["organization_id"])
            if candidate:
                return {**self._recommend_candidate(order, candidate, historical=True), "possible_match_count": 1}
        search = self.search(order, include_fuzzy=False)
        # A personal name alone is not enough to merge customers. It remains
        # available in the correction search, while email, phone, company, or
        # address can support a primary existing-customer recommendation.
        strong = [candidate for candidate in search["candidates"] if any(item["kind"] == "exact" and item["field"] in {"email", "phone", "company", "address"} for item in candidate["evidence"])]
        winner = None
        if len(strong) == 1:
            winner = strong[0]
        elif len(strong) > 1:
            first, second = strong[0], strong[1]
            first_fields = {item["field"] for item in first["evidence"] if item["kind"] == "exact"}
            if len(first_fields) >= 2 and first["score"] - second["score"] >= 6: winner = first
        if winner:
            return {**self._recommend_candidate(order, winner), "possible_match_count": search["candidate_count"]}
        if strong:
            return {"status": "ambiguous", "action": "choose_existing", "confidence": "low", "title": "Choose between likely existing customers",
                    "why": "More than one M1 customer has strong Shopify evidence.", "choices": strong, "possible_match_count": search["candidate_count"]}
        return new_customer("high", "No existing M1 customer has an exact Shopify name, business, email, phone, or address match.", search["candidate_count"])

    def validate_selection(self, order: dict[str, Any], organization_id: str, location_id: str | None, contact_id: str | None,
                           billing_location_id: str | None = None, billing_contact_id: str | None = None) -> dict[str, Any]:
        if organization_id == "__NEW__": return {"safe": True, "status": "new_customer", "detail": "New M1 records will use the Shopify address."}
        details = self.organization(organization_id)
        if not details: return {"safe": False, "status": "organization_missing", "detail": "Organization ID was not found in M1."}
        billing_location_id = billing_location_id or location_id
        billing_contact_id = billing_contact_id or contact_id
        if not location_id or not contact_id or not billing_location_id or not billing_contact_id:
            return {"safe": False, "status": "selection_incomplete", "detail": "Select both locations and both contacts. Choose NEW when Shopify data should create one.", "organization": details}
        shipping_location = next((row for row in details["locations"] if row["location_id"] == location_id), None)
        billing_location = next((row for row in details["locations"] if row["location_id"] == billing_location_id), None)
        if (not shipping_location and location_id != "NEW") or (not billing_location and billing_location_id != "NEW"): return {"safe": False, "status": "location_missing", "detail": "Select billing and shipping locations belonging to this organization, or choose NEW.", "organization": details}
        shipping_contact = next((row for row in details["contacts"] if row["contact_id"] == contact_id and (not row["location_id"] or row["location_id"] == location_id)), None)
        billing_contact = next((row for row in details["contacts"] if row["contact_id"] == billing_contact_id and (not row["location_id"] or row["location_id"] == billing_location_id)), None)
        if (contact_id and contact_id != "NEW" and not shipping_contact) or (billing_contact_id and billing_contact_id != "NEW" and not billing_contact): return {"safe": False, "status": "contact_missing", "detail": "Each contact must belong to its selected location, or choose NEW.", "organization": details}
        shipping = ({"status": "new", "matches": True, "detail": "A new shipping location will use Shopify."} if location_id == "NEW" else compare_addresses(order.get("shipping_address"), shipping_location["address"]))
        bill_source = order.get("billing_address") or {}
        billing = ({"status": "new", "matches": True, "detail": "A new billing location will use Shopify."} if billing_location_id == "NEW" else compare_addresses(bill_source, billing_location["address"])) if bill_source.get("address1") else {"status": "not_provided", "matches": True, "detail": "No separate Shopify billing address."}
        safe = shipping["matches"] and billing["matches"]
        return {"safe": safe, "status": "match" if safe else "address_mismatch", "detail": "Selected M1 location matches Shopify." if safe else "M1 location must match both Shopify shipping and billing addresses.", "shipping": shipping, "billing": billing, "organization": details}
