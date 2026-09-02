import unittest
from datetime import datetime, timezone

from storage import MemoryStore
from sync_service import SyncService
from tests.test_domain import sample_node


class FakeShopify:
    def __init__(self, orders):
        self.orders = orders
        self.full_calls = 0
        self.updated_since = None

    def all_orders(self):
        self.full_calls += 1
        yield from self.orders

    def updated_orders(self, since):
        self.updated_since = since
        yield from self.orders


class FakeM1:
    configured = True

    def __init__(self, existing=None):
        self.existing = existing or {}

    def find_sales_order_by_po(self, customer_po):
        return self.existing.get(customer_po)

    def shopify_sales_orders_by_po(self):
        return dict(self.existing)

    def organization_details(self, organization_id):
        return {
            "organization": {"cmoOrganizationID": organization_id, "cmoName": "Historical customer"},
            "locations": [],
            "contacts": [],
        }

    def get_all(self, resource, filters=None):
        return []

    def home_currency_id(self):
        return ""


class UnconfiguredM1(FakeM1):
    configured = False


class SyncTests(unittest.TestCase):
    def test_reconciliation_fails_closed_without_m1(self):
        store = MemoryStore()

        with self.assertRaisesRegex(RuntimeError, "M1 Public API"):
            SyncService(store, shopify=FakeShopify([sample_node()]), m1=UnconfiguredM1()).run(full=True)

        self.assertEqual([], store.list_orders())
        self.assertEqual("failed", store.last_sync()["status"])

    def test_full_reconciliation_only_queues_orders_missing_from_m1(self):
        new_order = sample_node()
        old_order = {**sample_node(), "id": "gid://shopify/Order/1917", "legacyResourceId": 1917, "name": "S-1917"}
        store = MemoryStore()
        shopify = FakeShopify([old_order, new_order])
        m1 = FakeM1({"S-1917": {"ompSalesOrderID": "112717", "ompCustomerOrganizationID": "C100", "ompShipLocationID": "100", "ompShipContactID": "1", "ompArInvoiceLocationID": "200", "ompArInvoiceContactID": "2"}})

        result = SyncService(store, shopify=shopify, m1=m1).run(full=True)

        rows = {row["order_name"]: row for row in store.list_orders()}
        self.assertEqual("committed", rows["S-1917"]["state"])
        self.assertEqual("112717", rows["S-1917"]["erp_order_id"])
        self.assertEqual("C100", rows["S-1917"]["matched_organization_id"])
        self.assertEqual("200", rows["S-1917"]["matched_billing_location_id"])
        self.assertEqual("ready", rows["S-1918"]["state"])
        self.assertEqual("C100", rows["S-1918"]["matched_organization_id"])
        self.assertEqual("automatic_recommendation", rows["S-1918"]["match_validation"]["status"])
        self.assertEqual("add_location", rows["S-1918"]["customer_resolution"]["action"])
        self.assertEqual(1, result["orders_already_in_m1"])
        self.assertEqual(1, result["orders_not_in_m1"])
        self.assertEqual(1, result["customer_resolutions_prepared"])
        # The commit preview is built during reconciliation too, so opening the
        # order later is a pure SQL read.
        self.assertEqual(1, result["m1_previews_prepared"])
        # Omitted from list payloads; present when the order itself is fetched.
        self.assertIsNone(rows["S-1918"]["m1_preview"])
        self.assertTrue(store.get_order(rows["S-1918"]["order_id"])["m1_preview"]["ok"])
        self.assertEqual(1, shopify.full_calls)

    def test_incremental_sync_starts_from_previous_run_after_downtime(self):
        store = MemoryStore()
        previous = store.begin_sync()
        store.sync_runs[-1]["started_at"] = datetime(2026, 8, 30, tzinfo=timezone.utc)
        store.finish_sync(previous, seen=0, changed=0)
        shopify = FakeShopify([])

        SyncService(store, shopify=shopify, m1=FakeM1()).run()

        self.assertEqual(datetime(2026, 8, 29, 23, 50, tzinfo=timezone.utc), shopify.updated_since)


if __name__ == "__main__":
    unittest.main()
