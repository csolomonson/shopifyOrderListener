import unittest
from unittest.mock import Mock

from integrations.m1 import M1Client, M1Error


class M1ReadTests(unittest.TestCase):
    def test_bulk_shopify_po_index_discards_nearby_customer_pos(self):
        client = M1Client("http://m1.test", "id", "key")
        client._request = Mock(return_value={
            "returnObject": [
                {"ompCustomerPo": "S-1917", "ompSalesOrderID": "112717"},
                {"ompCustomerPo": "S.O. 41999", "ompSalesOrderID": "29369"},
                {"ompCustomerPo": "S-1918", "ompSalesOrderID": "112718"},
            ]
        })

        result = client.shopify_sales_orders_by_po()

        self.assertEqual({"S-1917", "S-1918"}, set(result))
        client._request.assert_called_once()

    def test_home_currency_uses_dataset_property_case_insensitively(self):
        client = M1Client("http://dataset.test", "dataset-id", "key")
        client.get = Mock(return_value=[{"xadCurrencyRateId": "HOME"}])

        self.assertEqual("HOME", client.home_currency_id())

    def test_blank_dataset_currency_is_the_valid_home_currency(self):
        client = M1Client("http://blank-dataset.test", "blank-id", "key")
        client.get = Mock(return_value=[{"xadCurrencyRateID": ""}])
        client.shopify_sales_orders_by_po = Mock()

        self.assertEqual("", client.home_currency_id())
        client.shopify_sales_orders_by_po.assert_not_called()

    def test_home_currency_falls_back_to_historical_shopify_orders(self):
        client = M1Client("http://history.test", "history-id", "key")
        client.get = Mock(return_value=[{}])
        client.shopify_sales_orders_by_po = Mock(return_value={
            "S-1": {"ompCurrencyRateID": "BASE"},
            "S-2": {"ompCurrencyRateId": "BASE"},
            "S-3": {"ompCurrencyRateID": "CAD"},
        })

        self.assertEqual("BASE", client.home_currency_id())


class PaginationTests(unittest.TestCase):
    """M1 pages with OFFSET/FETCH under "ORDER BY 1" unless an orderBy is sent.

    "1" is an ordinal reference to the first column of the select list, which is a
    low-cardinality column on every table this app reads, so pages silently skip
    and duplicate rows.
    """

    def test_every_paged_read_sends_a_stable_sort(self):
        client = M1Client("http://page.test", "page-id", "key")
        client._request = Mock(return_value={"returnObject": []})

        client.get_all("OrganizationContacts")

        params = dict(client._request.call_args.kwargs["params"])
        self.assertEqual("cmcOrganizationID[Asc],cmcLocationID[Asc],cmcContactID[Asc]", params["orderBy"])

    def test_the_historical_sales_order_sweep_is_sorted_too(self):
        client = M1Client("http://page.test", "page-id", "key")
        client._request = Mock(return_value={"returnObject": []})

        client.shopify_sales_orders_by_po()

        params = client._request.call_args.kwargs["params"]
        self.assertIn(("orderBy", "ompSalesOrderID[Asc]"), params)

    def test_an_unlisted_resource_refuses_to_page(self):
        client = M1Client("http://page.test", "page-id", "key")
        client._request = Mock(return_value={"returnObject": []})

        # M1 ignores an unknown orderBy field and silently reverts to ORDER BY 1,
        # so paging a resource with no configured sort must fail instead.
        with self.assertRaisesRegex(M1Error, "stable pagination sort"):
            client.get_all("Parts")
        client._request.assert_not_called()

    def test_paging_continues_until_a_short_page(self):
        client = M1Client("http://page.test", "page-id", "key")
        client._request = Mock(side_effect=[
            {"returnObject": [{"cmcContactID": str(i)} for i in range(1000)]},
            {"returnObject": [{"cmcContactID": "extra"}]},
        ])

        self.assertEqual(1001, len(client.get_all("OrganizationContacts")))
        self.assertEqual([0, 1], [dict(call.kwargs["params"])["pageNumber"] for call in client._request.call_args_list])


class NextIDTests(unittest.TestCase):
    """M1 exposes NextIDs as plain CRUD; nothing advances it when a record is created."""

    def _client(self, next_id, existing_keys, *, numeric_only=2):
        client = M1Client("http://ids.test", "ids-id", "key")
        self.row = {
            "xanUniqueID": "next-id-guid", "xanTable": "SALESORDERS", "xanNextID": next_id,
            "xanNumericOnly": numeric_only, "xanIncrementAmount": 0, "xanRowVersion": "version-1",
        }

        def get(resource, filters=None):
            if resource == "NextIDs":
                return [dict(self.row)]
            key = (filters or [""])[0].split("]", 1)[1]
            return [{"ompSalesOrderID": key}] if key in existing_keys else []

        client.get = get
        client.put = Mock(return_value={})
        self.put = client.put
        return client

    def test_allocated_id_is_reserved_so_the_next_commit_cannot_reuse_it(self):
        client = self._client("112737", existing_keys=set())

        self.assertEqual("112737", client.next_id("SalesOrders"))
        self.put.assert_called_once()
        resource, payload = self.put.call_args[0]
        self.assertEqual("NextIDs", resource)
        self.assertEqual("112738", payload["xanNextID"])
        self.assertEqual("version-1", payload["xanRowVersion"])

    def test_ids_already_used_in_m1_are_skipped(self):
        client = self._client("112736", existing_keys={"112736", "112737"})

        self.assertEqual("112738", client.next_id("SalesOrders"))
        self.assertEqual("112739", self.put.call_args[0][1]["xanNextID"])

    def test_row_version_conflict_is_retried_against_the_reread_row(self):
        client = self._client("112737", existing_keys=set())
        client.put = Mock(side_effect=[M1Error("The row version of the NextID has changed."), {}])
        self.put = client.put

        self.assertEqual("112737", client.next_id("SalesOrders"))
        self.assertEqual(2, client.put.call_count)

    def test_non_numeric_tables_increment_the_way_the_m1_desktop_does(self):
        self.assertEqual("C11", M1Client.increment_id("C10", numeric_only=False))
        self.assertEqual("C1A", M1Client.increment_id("C19", numeric_only=False))
        self.assertEqual("D0", M1Client.increment_id("CZ", numeric_only=False))
        self.assertEqual("112737", M1Client.increment_id("112736", numeric_only=True))


if __name__ == "__main__":
    unittest.main()
