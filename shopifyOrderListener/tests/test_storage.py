import unittest
from decimal import Decimal

from storage import MemoryStore
from tests.test_domain import sample_node
from domain import normalize_shopify_order


class MemoryStoreTests(unittest.TestCase):
    def test_upsert_is_idempotent(self):
        store = MemoryStore()
        order = normalize_shopify_order(sample_node())
        first = store.upsert_order(order)
        second = store.upsert_order(order)
        self.assertEqual(first["order_id"], second["order_id"])
        self.assertEqual(1, store.dashboard()["total"])

    def test_match_review_blocks_commit(self):
        store = MemoryStore()
        row = store.upsert_order(normalize_shopify_order(sample_node()))
        row = store.set_match(row["order_id"], "C100", "100", "1", True)
        self.assertEqual("customer_review", row["state"])
        self.assertTrue(row["blocks_commit"])

    def test_returning_shopify_customer_reuses_confirmed_m1_identity(self):
        store = MemoryStore()
        first = store.upsert_order(normalize_shopify_order(sample_node()))
        store.set_match(first["order_id"], "C100", "100", "1", False)
        match = store.find_shopify_customer_match("gid://shopify/Customer/1")
        self.assertEqual({"organization_id": "C100", "location_id": "100", "contact_id": "1", "billing_location_id": "100", "billing_contact_id": "1"}, match)

    def test_line_edits_survive_shopify_reconciliation_and_recalculate_totals(self):
        store = MemoryStore()
        order = normalize_shopify_order(sample_node())
        row = store.upsert_order(order)
        edited = [{**order["lines"][0], "current_quantity": Decimal("2"), "unit_price": Decimal("30"), "line_total": Decimal("60") }]
        row = store.update_lines(row["order_id"], edited, "tester")
        self.assertEqual(Decimal("83.59"), row["total"])
        row = store.upsert_order(order)
        self.assertEqual(Decimal("2"), row["lines"][0]["current_quantity"])

    def test_line_edit_does_not_double_apply_shipping_discount(self):
        node = sample_node()
        node["lineItems"]["nodes"][0]["originalUnitPriceSet"]["shopMoney"]["amount"] = "0.00"
        node["currentSubtotalPriceSet"]["shopMoney"]["amount"] = "0.00"
        node["currentTotalDiscountsSet"]["shopMoney"]["amount"] = "16.78"
        node["currentShippingPriceSet"]["shopMoney"]["amount"] = "0.00"
        node["currentTotalPriceSet"]["shopMoney"]["amount"] = "0.00"
        store = MemoryStore()
        order = normalize_shopify_order(node)
        row = store.upsert_order(order)

        row = store.update_lines(row["order_id"], order["lines"], "tester")

        self.assertEqual(Decimal("0.00"), row["total"])

    def test_address_override_cannot_bypass_non_address_review(self):
        store = MemoryStore()
        row = store.upsert_order(normalize_shopify_order(sample_node()))
        with self.assertRaises(ValueError):
            store.set_address_override(row["order_id"], "not applicable", "tester")

    def test_queue_is_ordered_by_shopify_order_date_not_update_date(self):
        store = MemoryStore()
        older = {**sample_node(), "id": "older", "legacyResourceId": 1, "name": "S-1", "createdAt": "2026-08-01T12:00:00Z", "updatedAt": "2026-09-01T12:00:00Z"}
        newer = {**sample_node(), "id": "newer", "legacyResourceId": 2, "name": "S-2", "createdAt": "2026-08-31T12:00:00Z", "updatedAt": "2026-08-31T12:00:00Z"}
        store.upsert_order(normalize_shopify_order(older))
        store.upsert_order(normalize_shopify_order(newer))

        self.assertEqual(["S-2", "S-1"], [row["order_name"] for row in store.list_orders()])


if __name__ == "__main__":
    unittest.main()
