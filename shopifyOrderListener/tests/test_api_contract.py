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
            ("GET", "/sales-orders/logout"),
            ("GET", "/sales-orders/api/orders"),
            ("POST", "/sales-orders/api/account/password"),
            ("POST", "/sales-orders/api/sync"),
            ("PUT", "/sales-orders/api/settings"),
            ("POST", "/sales-orders/api/orders/{order_id}/commit"),
        }
        self.assertTrue(expected.issubset(routes), expected - routes)

    def test_logout_clears_browser_authentication_state(self):
        from web.app import logout

        response = logout()

        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.headers["clear-site-data"], '"cache", "cookies", "storage"')
        self.assertIn("Signed out", response.body.decode())

    def test_no_root_level_app_routes_leak_into_costing_app(self):
        paths = [route.path for route in app.routes if not route.path.startswith(("/openapi", "/docs", "/redoc"))]
        self.assertTrue(all(path.startswith("/sales-orders") for path in paths))


if __name__ == "__main__":
    unittest.main()
