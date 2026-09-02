import unittest

from authentication import AuthorizationError, Principal, authorize_sales_orders


class AuthorizationTests(unittest.TestCase):
    def test_sales_order_group_is_allowed(self):
        authorize_sales_orders(Principal("shipper", ("sales-orders",)))

    def test_administrators_are_allowed(self):
        authorize_sales_orders(Principal("admin", ("administrators",)))

    def test_costing_only_user_is_denied(self):
        with self.assertRaises(AuthorizationError):
            authorize_sales_orders(Principal("costing", ("users",)))


if __name__ == "__main__":
    unittest.main()
