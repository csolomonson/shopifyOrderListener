import os
import unittest

os.environ["SALES_ORDER_BACKGROUND_SYNC"] = "false"
os.environ["SALES_ORDER_AUTH_REQUIRED"] = "false"

from web.app import app


class ApiContractTests(unittest.TestCase):
    def test_expected_subpath_routes_exist(self):
        routes = {(method, route.path) for route in app.routes for method in getattr(route, "methods", set())}
        expected = {
            ("GET", "/sales-orders"),
            ("GET", "/sales-orders/shopify"),
            ("GET", "/sales-orders/api/orders"),
            ("POST", "/sales-orders/api/sync"),
            ("PUT", "/sales-orders/api/settings"),
            ("POST", "/sales-orders/api/orders/{order_id}/commit"),
        }
        self.assertTrue(expected.issubset(routes), expected - routes)

    def test_no_root_level_app_routes_leak_into_costing_app(self):
        paths = [route.path for route in app.routes if not route.path.startswith(("/openapi", "/docs", "/redoc"))]
        self.assertTrue(all(path.startswith("/sales-orders") for path in paths))


if __name__ == "__main__":
    unittest.main()
