import json
import unittest
from datetime import date

from commit_service import CommitService, build_customer_resources, build_m1_resource_plan, preview_fingerprint
from domain import normalize_shopify_order
from integrations.m1 import M1Error
from storage import MemoryStore
from tests.test_domain import sample_node


class WritableFakeM1:
    writes_enabled = True

    def __init__(self):
        self.writes = []

    def find_sales_order_by_po(self, customer_po):
        return None

    def home_currency_id(self):
        return "HOME"

    def organizations_by_email(self, email):
        return []

    def next_id(self, table):
        return {"SalesOrders": "113000", "Organizations": "C101"}[table]

    def put(self, resource, payload):
        self.writes.append((resource, payload))
        return {}


class BlankHomeCurrencyFakeM1(WritableFakeM1):
    def home_currency_id(self):
        return ""


class ExistingCustomerFakeM1(WritableFakeM1):
    def organization_details(self, organization_id):
        return {"organization": {"cmoOrganizationID": organization_id, "cmoName": "Existing Customer"}, "locations": [], "contacts": []}

    def next_child_id(self, resource, organization_id, organization_field, id_field, *, start, step):
        return str(start)


class DependencyCheckingFakeM1(WritableFakeM1):
    def __init__(self):
        super().__init__()
        self.locations = set()
        self.contacts = set()

    def put(self, resource, payload):
        self.writes.append((resource, payload))
        if resource == "Organizations" and "cmoRowVersion" not in payload:
            assert payload.get("cmoDefaultShipLocationID") == ""
            assert payload.get("cmoDefaultArInvoiceLocationID") == ""
            assert payload.get("cmoShipContactID") == ""
            assert payload.get("cmoArInvoiceContactID") == ""
            return {"returnObject": {**payload, "cmoRowVersion": "organization-version"}}
        if resource == "OrganizationLocations" and "cmlRowVersion" not in payload:
            assert payload.get("cmlShipContactID") == ""
            assert payload.get("cmlArInvoiceContactID") == ""
            self.locations.add((payload["cmlOrganizationID"], payload["cmlLocationID"]))
            return {"returnObject": {**payload, "cmlRowVersion": "location-version"}}
        if resource == "OrganizationContacts":
            assert (payload["cmcOrganizationID"], payload["cmcLocationID"]) in self.locations
            self.contacts.add((payload["cmcOrganizationID"], payload["cmcLocationID"], payload["cmcContactID"]))
        if resource == "OrganizationLocations" and "cmlRowVersion" in payload:
            for contact_id in (payload.get("cmlShipContactID"), payload.get("cmlArInvoiceContactID")):
                if contact_id:
                    assert (payload["cmlOrganizationID"], payload["cmlLocationID"], contact_id) in self.contacts
        if resource == "Organizations" and "cmoRowVersion" in payload:
            assert (payload["cmoOrganizationID"], payload["cmoDefaultShipLocationID"]) in self.locations
            assert (payload["cmoOrganizationID"], payload["cmoDefaultArInvoiceLocationID"]) in self.locations
        return {}


class SalesOrderFailureFakeM1(DependencyCheckingFakeM1):
    def put(self, resource, payload):
        result = super().put(resource, payload)
        if resource == "SalesOrders":
            raise M1Error("test sales-order validation failure")
        return result


class CommitPayloadTests(unittest.TestCase):
    def test_new_customer_marker_is_replaced_after_successful_commit(self):
        store = MemoryStore()
        staged = store.upsert_order(normalize_shopify_order(sample_node()))
        store.set_match(staged["order_id"], "__NEW__", "100", "1", False)
        m1 = WritableFakeM1()

        result = CommitService(store, m1=m1).commit(staged["order_id"], "tester")

        self.assertEqual("C101", result["matched_organization_id"])
        self.assertEqual("100", result["matched_location_id"])
        self.assertEqual("1", result["matched_contact_id"])
        self.assertEqual("113000", result["erp_order_id"])
        self.assertNotIn("__NEW__", json.dumps(m1.writes))

    def test_new_customer_dependencies_are_created_before_their_references(self):
        store = MemoryStore()
        staged = store.upsert_order(normalize_shopify_order(sample_node()))
        store.set_match(staged["order_id"], "__NEW__", "NEW", "NEW", False, "NEW", "NEW")
        m1 = DependencyCheckingFakeM1()

        result = CommitService(store, m1=m1).commit(staged["order_id"], "tester")

        self.assertEqual("113000", result["erp_order_id"])
        self.assertEqual(
            ["Organizations", "OrganizationLocations", "OrganizationContacts", "OrganizationLocations", "Organizations"],
            [resource for resource, _ in m1.writes[:5]],
        )

    def test_created_customer_ids_are_saved_when_sales_order_validation_fails(self):
        store = MemoryStore()
        staged = store.upsert_order(normalize_shopify_order(sample_node()))
        store.set_match(staged["order_id"], "__NEW__", "NEW", "NEW", False, "NEW", "NEW")

        with self.assertRaisesRegex(M1Error, "validation failure"):
            CommitService(store, m1=SalesOrderFailureFakeM1()).commit(staged["order_id"], "tester")

        recovered = store.get_order(staged["order_id"])
        self.assertEqual("C101", recovered["matched_organization_id"])
        self.assertEqual("100", recovered["matched_location_id"])
        self.assertEqual("1", recovered["matched_contact_id"])
        self.assertEqual("m1_customer_resources_created", recovered["match_validation"]["status"])

    def test_uses_historical_shopify_defaults(self):
        order = normalize_shopify_order(sample_node())
        plan = build_m1_resource_plan(order, "113000", "C100", "100", "1")
        payload = plan["SalesOrders"][0]
        self.assertEqual("S-1918", payload["ompCustomerPo"])
        self.assertEqual("PREPA", payload["ompPaymentTermID"])
        self.assertEqual("FEDHD", payload["ompShippingMethodID"])
        self.assertEqual("SHIP", payload["ompShippingPaymentTypeID"])
        self.assertEqual(3, payload["ompStatus"])
        self.assertEqual("142", plan["SalesOrderDeliveries"][0]["omdPartWarehouseLocationID"])
        self.assertEqual("BIN1", plan["SalesOrderDeliveries"][0]["omdPartBinID"])

    def test_line_delivery_quantity_total_matches_the_sum_of_its_deliveries(self):
        order = normalize_shopify_order(sample_node())
        plan = build_m1_resource_plan(order, "113000", "C100", "100", "1")

        for line in plan["SalesOrderLines"]:
            delivered = sum(
                float(delivery["omdDeliveryQuantity"])
                for delivery in plan["SalesOrderDeliveries"]
                if delivery["omdSalesOrderLineID"] == line["omlSalesOrderLineID"]
            )
            self.assertEqual(float(line["omlOrderQuantity"]), delivered)
            self.assertEqual(float(line["omlDeliveryQuantityTotal"]), delivered)

    def test_m1_order_comments_are_limited_to_api_field_length(self):
        order = normalize_shopify_order({**sample_node(), "note": "x" * 200})

        payload = build_m1_resource_plan(order, "113000", "C100", "100", "1")["SalesOrders"][0]

        self.assertEqual(50, len(payload["ompOrderCommentsText"]))

    def test_commit_uses_m1_home_currency_id_not_shopify_currency_code(self):
        store = MemoryStore()
        staged = store.upsert_order(normalize_shopify_order(sample_node()))
        store.set_match(staged["order_id"], "__NEW__", "NEW", "NEW", False, "NEW", "NEW")
        m1 = WritableFakeM1()

        CommitService(store, m1=m1).commit(staged["order_id"], "tester")

        sales_order = next(payload for resource, payload in m1.writes if resource == "SalesOrders")
        self.assertEqual("HOME", sales_order["ompCurrencyRateID"])

    def test_commit_preserves_blank_m1_home_currency_id(self):
        store = MemoryStore()
        staged = store.upsert_order(normalize_shopify_order(sample_node()))
        store.set_match(staged["order_id"], "__NEW__", "NEW", "NEW", False, "NEW", "NEW")
        m1 = BlankHomeCurrencyFakeM1()

        CommitService(store, m1=m1).commit(staged["order_id"], "tester")

        sales_order = next(payload for resource, payload in m1.writes if resource == "SalesOrders")
        self.assertEqual("", sales_order["ompCurrencyRateID"])

    def test_new_customer_plan_uses_historical_defaults(self):
        order = normalize_shopify_order(sample_node())
        resources = build_customer_resources(order, "C101", "100", "1")
        organization = resources["Organizations"][0]
        self.assertEqual(2, organization["cmoCustomerStatus"])
        self.assertEqual("CG01", organization["cmoCustomerGroupID"])
        self.assertEqual("100", organization["cmoDefaultShipLocationID"])

    def test_different_billing_address_uses_accounting_contact_and_location(self):
        node = sample_node()
        node["billingAddress"] = {"name": "Accounts Payable", "address1": "9 Bill Rd", "city": "Carlsbad", "provinceCode": "CA", "zip": "92008", "countryCodeV2": "US"}
        order = normalize_shopify_order(node)
        resources = build_customer_resources(order, "C101", "200", "2", "100", "1")
        organization = resources["Organizations"][0]
        self.assertEqual("200", organization["cmoDefaultShipLocationID"])
        self.assertEqual("100", organization["cmoDefaultArInvoiceLocationID"])
        self.assertEqual({"100", "200"}, {row["cmlLocationID"] for row in resources["OrganizationLocations"]})
        billing_contact = next(row for row in resources["OrganizationContacts"] if row["cmcContactID"] == "1")
        self.assertEqual("100", billing_contact["cmcLocationID"])
        plan = build_m1_resource_plan(order, "113000", "C101", "200", "2", billing_location_id="100", billing_contact_id="1")
        header = plan["SalesOrders"][0]
        self.assertEqual("200", header["ompShipLocationID"])
        self.assertEqual("2", header["ompShipContactID"])
        self.assertEqual("100", header["ompArInvoiceLocationID"])
        self.assertEqual("1", header["ompArInvoiceContactID"])

    def test_same_shopify_address_creates_one_location_and_contact_for_existing_org(self):
        store = MemoryStore()
        staged = store.upsert_order(normalize_shopify_order(sample_node()))
        store.set_match(staged["order_id"], "C100", "NEW", "NEW", False, "NEW", "NEW")
        m1 = ExistingCustomerFakeM1()

        result = CommitService(store, m1=m1).commit(staged["order_id"], "tester")

        self.assertEqual(result["matched_location_id"], result["matched_billing_location_id"])
        self.assertEqual(result["matched_contact_id"], result["matched_billing_contact_id"])
        self.assertEqual(1, sum(resource == "OrganizationLocations" for resource, _ in m1.writes))
        self.assertEqual(1, sum(resource == "OrganizationContacts" for resource, _ in m1.writes))

    def test_different_shopify_addresses_create_separate_records_for_existing_org(self):
        node = sample_node()
        node["billingAddress"] = {"name": "Accounts Payable", "address1": "9 Bill Rd", "city": "Carlsbad", "provinceCode": "CA", "zip": "92008", "countryCodeV2": "US"}
        store = MemoryStore()
        staged = store.upsert_order(normalize_shopify_order(node))
        store.set_match(staged["order_id"], "C100", "NEW", "NEW", False, "NEW", "NEW")
        m1 = ExistingCustomerFakeM1()

        result = CommitService(store, m1=m1).commit(staged["order_id"], "tester")

        self.assertNotEqual(result["matched_location_id"], result["matched_billing_location_id"])
        self.assertNotEqual(result["matched_contact_id"], result["matched_billing_contact_id"])


class OrganizationContactRejectingFakeM1(DependencyCheckingFakeM1):
    """Reproduces M1's Public API: any non-blank organization-level contact is
    rejected, because ValidateRequest_PutOrganization compares 3 key columns
    against 2 values and its helper fails closed on the length mismatch."""

    ORGANIZATION_CONTACT_FIELDS = (
        "cmoShipContactID", "cmoArInvoiceContactID", "cmoQuoteContactID",
        "cmoPurchaseContactID", "cmoApInvoiceContactID",
    )

    def put(self, resource, payload):
        if resource == "Organizations":
            rejected = [field for field in self.ORGANIZATION_CONTACT_FIELDS if str(payload.get(field) or "").strip()]
            if rejected:
                raise M1Error("; ".join(f"{field} [{payload[field]}] not found." for field in rejected))
        return super().put(resource, payload)


class ExplodingM1:
    """Any M1 access at all is a failure: the read path must be pure SQL."""

    writes_enabled = True

    def __getattr__(self, name):
        raise AssertionError(f"M1 was contacted through {name}() while serving a stored preview")


class MatchingCustomerFakeM1(WritableFakeM1):
    """An M1 whose stored location matches the sample order's Shopify address."""

    def organization_details(self, organization_id):
        return {
            "organization": {"cmoOrganizationID": organization_id, "cmoName": "Existing Customer"},
            "locations": [{"cmlLocationID": "100", "cmlName": "Existing Customer", "cmlAddressLine1": "1 Main St",
                           "cmlCity": "Escondido", "cmlState": "CA", "cmlPostCode": "92025", "cmlCountryCode": "US"}],
            "contacts": [{"cmcContactID": "1", "cmcLocationID": "100", "cmcName": "Joe Turk"}],
        }


class NewCustomerOrganizationTests(unittest.TestCase):
    def _commit_new_customer(self, m1):
        store = MemoryStore()
        staged = store.upsert_order(normalize_shopify_order(sample_node()))
        store.set_match(staged["order_id"], "__NEW__", None, None, False)
        CommitService(store, m1).commit(staged["order_id"], "tester")
        return [payload for resource, payload in m1.writes if resource == "Organizations"]

    def test_new_customer_is_created_without_organization_level_contacts(self):
        organizations = self._commit_new_customer(OrganizationContactRejectingFakeM1())

        self.assertEqual(2, len(organizations))
        for payload in organizations:
            for field in OrganizationContactRejectingFakeM1.ORGANIZATION_CONTACT_FIELDS:
                self.assertEqual("", str(payload.get(field) or ""), f"{field} must never be sent to M1")

    def test_the_organization_still_gets_its_default_locations(self):
        organizations = self._commit_new_customer(OrganizationContactRejectingFakeM1())

        final = organizations[-1]
        self.assertEqual("100", final["cmoDefaultShipLocationID"])
        self.assertEqual("100", final["cmoDefaultArInvoiceLocationID"])

    def test_a_new_customer_is_active_from_today(self):
        m1 = OrganizationContactRejectingFakeM1()
        self._commit_new_customer(m1)

        organization = next(payload for resource, payload in m1.writes if resource == "Organizations")
        # Active status without an active date is the state the M1 data dictionary
        # treats as incomplete, and the API runs none of the desktop's field handlers.
        self.assertEqual(2, organization["cmoCustomerStatus"])
        self.assertEqual(date.today().isoformat(), organization["cmoCustomerActiveDate"])

    def test_the_contact_lives_on_the_location_instead(self):
        m1 = OrganizationContactRejectingFakeM1()
        self._commit_new_customer(m1)

        locations = [payload for resource, payload in m1.writes if resource == "OrganizationLocations"]
        self.assertEqual("1", locations[-1]["cmlShipContactID"])
        # And the order names its own contacts, so nothing relies on the
        # organization-level defaults M1 will not accept.
        header = next(payload for resource, payload in m1.writes if resource == "SalesOrders")
        self.assertEqual("1", header["ompShipContactID"])
        self.assertEqual("1", header["ompArInvoiceContactID"])


class PrecomputedPreviewTests(unittest.TestCase):
    def _staged_order(self):
        store = MemoryStore()
        order = store.upsert_order(normalize_shopify_order(sample_node()))
        store.set_match(order["order_id"], "C100", "100", "1", False, "100", "1",
                        {"safe": True, "status": "match", "detail": "ok"})
        return store, store.get_order(order["order_id"])

    def test_stored_preview_is_served_without_contacting_m1(self):
        store, order = self._staged_order()
        CommitService(store, MatchingCustomerFakeM1()).refresh_preview(order["order_id"])

        # A fresh process with a cold customer directory cache, as after a restart.
        envelope = CommitService(store, ExplodingM1()).cached_preview(order["order_id"])

        self.assertTrue(envelope["ok"])
        self.assertFalse(envelope["preview"]["already_exists"])
        # The sales-order ID is still allocated at commit, so nothing reserved
        # is baked into the stored preview.
        self.assertEqual("<allocated by M1 at commit>", envelope["preview"]["resources"]["SalesOrders"][0]["ompSalesOrderID"])
        self.assertEqual("C100", envelope["preview"]["resources"]["SalesOrders"][0]["ompCustomerOrganizationID"])

    def test_editing_lines_invalidates_the_stored_preview(self):
        store, order = self._staged_order()
        CommitService(store, MatchingCustomerFakeM1()).refresh_preview(order["order_id"])
        stale = preview_fingerprint(store.get_order(order["order_id"]), store.get_settings())

        store.update_lines(order["order_id"], [{
            "shopify_line_id": "1", "line_number": 1, "sku": "SKU-1", "description": "Widget",
            "variant": "", "current_quantity": 7, "original_quantity": 7, "unit_price": 10, "line_total": 70,
        }], "tester")

        refreshed = store.get_order(order["order_id"])
        self.assertNotEqual(stale, preview_fingerprint(refreshed, store.get_settings()))
        with self.assertRaises(AssertionError):
            CommitService(store, ExplodingM1()).cached_preview(order["order_id"])

    def test_rebuilt_preview_reflects_the_edited_lines(self):
        store, order = self._staged_order()
        service = CommitService(store, MatchingCustomerFakeM1())
        service.refresh_preview(order["order_id"])
        store.update_lines(order["order_id"], [{
            "shopify_line_id": "1", "line_number": 1, "sku": "SKU-1", "description": "Widget",
            "variant": "", "current_quantity": 7, "original_quantity": 7, "unit_price": 10, "line_total": 70,
        }], "tester")

        envelope = service.cached_preview(order["order_id"])

        self.assertEqual("7", envelope["preview"]["resources"]["SalesOrderLines"][0]["omlOrderQuantity"])
        self.assertEqual("7", envelope["preview"]["resources"]["SalesOrderDeliveries"][0]["omdDeliveryQuantity"])

    def test_a_blocked_order_stores_its_reason_instead_of_a_preview(self):
        store = MemoryStore()
        order = store.upsert_order(normalize_shopify_order(sample_node()))

        envelope = CommitService(store, MatchingCustomerFakeM1()).refresh_preview(order["order_id"])

        self.assertFalse(envelope["ok"])
        self.assertIn("customer", envelope["detail"].lower())
        # The blocking reason is served from storage too, so a blocked order is
        # just as cheap to open as a ready one.
        self.assertEqual(envelope, CommitService(store, ExplodingM1()).cached_preview(order["order_id"]))


if __name__ == "__main__":
    unittest.main()
