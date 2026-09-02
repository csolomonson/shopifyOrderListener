import unittest

from customer_matching import CustomerMatcher, _Directory, compare_addresses
from domain import normalize_shopify_order
from tests.test_domain import sample_node


class FakeM1:
    def __init__(self):
        self.details = {
            "organization": {"cmoOrganizationID": "C100", "cmoName": "Turk Racing", "cmoEmailAddress": "office@example.com"},
            "locations": [
                {"cmlOrganizationID": "C100", "cmlLocationID": "100", "cmlName": "Shipping", "cmlAddressLine1": "1 Main Street", "cmlCity": "Escondido", "cmlState": "CA", "cmlPostCode": "92025", "cmlCountryCode": "US"},
                {"cmlOrganizationID": "C100", "cmlLocationID": "200", "cmlName": "Accounting", "cmlAddressLine1": "9 Bill Rd", "cmlCity": "Carlsbad", "cmlState": "CA", "cmlPostCode": "92008", "cmlCountryCode": "US"},
            ],
            "contacts": [
                {"cmcOrganizationID": "C100", "cmcContactID": "1", "cmcLocationID": "100", "cmcName": "Joe Turk", "cmcEmailAddress": "joe@example.com"},
                {"cmcOrganizationID": "C100", "cmcContactID": "2", "cmcLocationID": "200", "cmcName": "Accounts Payable", "cmcEmailAddress": "billing@example.com"},
            ],
        }

    def organization_details(self, organization_id):
        return self.details if organization_id == "C100" else None

    def get_all(self, resource):
        return {"Organizations": [self.details["organization"]], "OrganizationLocations": self.details["locations"], "OrganizationContacts": self.details["contacts"]}[resource]


class CustomerMatchingTests(unittest.TestCase):
    def setUp(self):
        _Directory.data = {}
        _Directory.loaded_at = 0

    def test_normalizes_common_street_suffixes(self):
        result = compare_addresses(
            {"address1": "1 Main St.", "city": "Escondido", "province": "CA", "postal_code": "92025", "country": "US"},
            {"address1": "1 MAIN STREET", "city": "ESCONDIDO", "province": "CA", "postal_code": "92025", "country": "US"},
        )
        self.assertTrue(result["matches"])

    def test_loose_candidates_explain_independent_email_match(self):
        order = normalize_shopify_order(sample_node())
        candidates = CustomerMatcher(FakeM1()).candidates(order)
        self.assertEqual("C100", candidates[0]["organization_id"])
        self.assertIn("Email exact", candidates[0]["reasons"])
        self.assertIn("Shipping address exact", candidates[0]["reasons"])

    def test_single_candidate_is_declared_for_automatic_selection(self):
        report = CustomerMatcher(FakeM1()).search(normalize_shopify_order(sample_node()))
        self.assertEqual(1, report["candidate_count"])
        self.assertEqual("C100", report["automatic_selection_id"])
        self.assertTrue(report["fields"]["name"]["matched"])
        self.assertTrue(report["fields"]["email"]["matched"])
        self.assertTrue(report["fields"]["address"]["matched"])

    def test_no_match_report_names_every_checked_shopify_field(self):
        node = sample_node()
        node["email"], node["phone"] = "nobody@elsewhere.invalid", "555-9999"
        node["customer"]["displayName"] = "Completely Different Person"
        node["shippingAddress"].update(name="Completely Different Person", address1="999 Other Avenue", city="Elsewhere", zip="99999")
        report = CustomerMatcher(FakeM1()).search(normalize_shopify_order(node))
        self.assertTrue(report["no_shopify_match"])
        self.assertEqual(0, report["candidate_count"])
        self.assertEqual({"name", "company", "email", "phone", "address"}, set(report["fields"]))

    def test_all_matching_organizations_are_returned_without_result_cap(self):
        class ManyFakeM1(FakeM1):
            def get_all(self, resource):
                if resource == "Organizations":
                    return [{"cmoOrganizationID": f"C{index:03}", "cmoName": f"Organization {index}", "cmoEmailAddress": "joe@example.com"} for index in range(30)]
                return []

        report = CustomerMatcher(ManyFakeM1()).search(normalize_shopify_order(sample_node()))
        self.assertEqual(30, report["candidate_count"])
        self.assertIsNone(report["automatic_selection_id"])

    def test_billing_and_shipping_contacts_are_location_scoped(self):
        node = sample_node()
        node["billingAddress"] = {"name": "Accounts Payable", "address1": "9 Bill Road", "city": "Carlsbad", "provinceCode": "CA", "zip": "92008", "countryCodeV2": "US"}
        order = normalize_shopify_order(node)
        result = CustomerMatcher(FakeM1()).validate_selection(order, "C100", "100", "1", "200", "2")
        self.assertTrue(result["safe"])
        wrong = CustomerMatcher(FakeM1()).validate_selection(order, "C100", "100", "2", "200", "1")
        self.assertEqual("contact_missing", wrong["status"])

    def test_exact_identity_and_address_recommends_existing_setup(self):
        recommendation = CustomerMatcher(FakeM1()).resolution(normalize_shopify_order(sample_node()))
        self.assertEqual("use_existing", recommendation["action"])
        self.assertEqual("C100", recommendation["selection"]["organization_id"])
        self.assertEqual("100", recommendation["selection"]["location_id"])
        self.assertEqual("1", recommendation["selection"]["contact_id"])

    def test_billing_address_is_reused_when_shopify_has_no_shipping_address(self):
        node = sample_node()
        node["billingAddress"] = node["shippingAddress"]
        node["shippingAddress"] = None

        order = normalize_shopify_order(node)
        recommendation = CustomerMatcher(FakeM1()).resolution(order)
        selection = recommendation["selection"]

        self.assertEqual("100", selection["location_id"])
        self.assertEqual("100", selection["billing_location_id"])
        self.assertTrue(CustomerMatcher(FakeM1()).validate_selection(
            order, "C100", selection["location_id"], selection["contact_id"],
            selection["billing_location_id"], selection["billing_contact_id"],
        )["safe"])

    def test_exact_identity_with_new_address_recommends_new_location(self):
        node = sample_node()
        node["shippingAddress"].update(address1="500 New Road", city="Vista", zip="92081")
        recommendation = CustomerMatcher(FakeM1()).resolution(normalize_shopify_order(node))
        self.assertEqual("add_location", recommendation["action"])
        self.assertEqual("C100", recommendation["selection"]["organization_id"])
        self.assertEqual("NEW", recommendation["selection"]["location_id"])

    def test_fuzzy_name_only_recommends_new_customer(self):
        node = sample_node()
        node["email"], node["phone"] = "new.person@example.invalid", "555-9999"
        node["shippingAddress"].update(address1="500 New Road", city="Vista", zip="92081")
        recommendation = CustomerMatcher(FakeM1()).resolution(normalize_shopify_order(node))
        self.assertEqual("create_new", recommendation["action"])
        self.assertEqual("__NEW__", recommendation["selection"]["organization_id"])

    def test_prior_shopify_identity_takes_precedence(self):
        node = sample_node()
        node["email"], node["phone"] = "changed@example.invalid", "555-9999"
        node["shippingAddress"].update(address1="500 New Road", city="Vista", zip="92081")
        recommendation = CustomerMatcher(FakeM1()).resolution(normalize_shopify_order(node), {"organization_id": "C100"})
        self.assertEqual("add_location", recommendation["action"])
        self.assertIn("Previously used", recommendation["why"])

    def test_blank_legacy_location_is_not_recommended_for_new_order(self):
        fake = FakeM1()
        fake.details["locations"][0]["cmlLocationID"] = ""
        recommendation = CustomerMatcher(fake).resolution(normalize_shopify_order(sample_node()), {"organization_id": "C100"})
        self.assertEqual("NEW", recommendation["selection"]["location_id"])


if __name__ == "__main__":
    unittest.main()
