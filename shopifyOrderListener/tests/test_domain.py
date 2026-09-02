import unittest

from domain import lifecycle_decision, normalize_shopify_order


def sample_node():
    return {
        "id": "gid://shopify/Order/1918",
        "legacyResourceId": 1918,
        "name": "S-1918",
        "createdAt": "2026-08-30T10:00:00Z",
        "updatedAt": "2026-08-30T10:00:00Z",
        "cancelledAt": None,
        "cancelReason": None,
        "cancellation": None,
        "displayFinancialStatus": "PAID",
        "displayFulfillmentStatus": "UNFULFILLED",
        "currencyCode": "USD",
        "email": "joe@example.com",
        "phone": "555-0100",
        "customer": {"id": "gid://shopify/Customer/1", "displayName": "Joe Turk"},
        "shippingAddress": {"name": "Joe Turk", "address1": "1 Main St", "city": "Escondido", "provinceCode": "CA", "zip": "92025", "countryCodeV2": "US"},
        "billingAddress": None,
        "shippingLine": {"title": "FedEx Ground Home Delivery"},
        "currentSubtotalPriceSet": {"shopMoney": {"amount": "25.53"}},
        "currentTotalDiscountsSet": {"shopMoney": {"amount": "0.00"}},
        "currentShippingPriceSet": {"shopMoney": {"amount": "23.59"}},
        "currentTotalTaxSet": {"shopMoney": {"amount": "0.00"}},
        "currentTotalPriceSet": {"shopMoney": {"amount": "49.12"}},
        "lineItems": {"nodes": [{"id": "line-1", "sku": "WP805", "name": "Part", "quantity": 1, "currentQuantity": 1, "originalUnitPriceSet": {"shopMoney": {"amount": "25.53"}}}]},
        "refunds": [],
    }


class DomainTests(unittest.TestCase):
    def test_normalizes_order_and_totals(self):
        order = normalize_shopify_order(sample_node())
        self.assertEqual("S-1918", order["customer_po"])
        self.assertEqual("WP805", order["lines"][0]["sku"])
        self.assertEqual("49.12", str(order["total"]))
        self.assertEqual("Escondido", order["shipping_address"]["city"])

    def test_missing_shipping_address_uses_billing_address(self):
        node = sample_node()
        node["billingAddress"] = node["shippingAddress"]
        node["shippingAddress"] = None

        order = normalize_shopify_order(node)

        self.assertEqual(order["billing_address"], order["shipping_address"])
        self.assertTrue(order["shipping_address_from_billing"])

    def test_uncommitted_cancel_does_not_enter_erp(self):
        node = sample_node()
        node["cancelledAt"] = "2026-08-30T11:00:00Z"
        order = normalize_shopify_order(node)
        decision = lifecycle_decision(order, {})
        self.assertEqual("cancelled_before_erp", decision.state)
        self.assertTrue(decision.blocks_commit)

    def test_committed_unshipped_cancel_requires_close_review(self):
        node = sample_node()
        node["cancelledAt"] = "2026-08-30T11:00:00Z"
        order = normalize_shopify_order(node)
        decision = lifecycle_decision(order, {"erp_order_id": "112716", "erp_quantity_shipped": 0})
        self.assertEqual("cancellation_review", decision.state)
        self.assertIn("header, lines, and deliveries", decision.detail)

    def test_refund_is_never_silent_sales_order_edit(self):
        node = sample_node()
        node["refunds"] = [{"id": "refund-1", "createdAt": "2026-08-31T00:00:00Z", "updatedAt": "2026-08-31T00:00:00Z", "totalRefundedSet": {"shopMoney": {"amount": "25.53"}}, "refundLineItems": {"nodes": []}}]
        order = normalize_shopify_order(node)
        decision = lifecycle_decision(order, {"erp_order_id": "112999"})
        self.assertEqual("refund_review", decision.state)
        self.assertIn("type-2 AR credit", decision.detail)


if __name__ == "__main__":
    unittest.main()
