import contextlib
import io
import json
import unittest
from unittest.mock import Mock, patch

import sync_orders


class SyncCommandTests(unittest.TestCase):
    @patch("sync_orders.SyncService")
    @patch("sync_orders.get_store")
    @patch("sync_orders.M1Client")
    @patch("sync_orders.ShopifyClient")
    def test_sync_uses_shared_clients_and_honors_full(self, shopify_class, m1_class, get_store, service_class):
        shopify = shopify_class.return_value
        shopify.configured = True
        m1 = m1_class.return_value
        m1.configured = True
        store = get_store.return_value
        service = service_class.return_value
        service.run.return_value = {"ok": True, "orders_seen": 12}

        result = sync_orders.sync(full=True)

        service_class.assert_called_once_with(store, shopify=shopify, m1=m1)
        service.run.assert_called_once_with(full=True)
        self.assertEqual(12, result["orders_seen"])

    @patch("sync_orders.sync", return_value={"ok": True, "orders_changed": 3})
    def test_main_prints_machine_readable_summary(self, run_sync):
        output = io.StringIO()
        with contextlib.redirect_stdout(output):
            exit_code = sync_orders.main(["--full"])

        self.assertEqual(0, exit_code)
        run_sync.assert_called_once_with(full=True)
        self.assertEqual({"ok": True, "orders_changed": 3}, json.loads(output.getvalue()))

    @patch("sync_orders.sync", side_effect=RuntimeError("connection unavailable"))
    def test_main_returns_failure_exit_code(self, _run_sync):
        error = io.StringIO()
        with contextlib.redirect_stderr(error):
            exit_code = sync_orders.main([])

        self.assertEqual(1, exit_code)
        self.assertIn("connection unavailable", error.getvalue())


if __name__ == "__main__":
    unittest.main()
