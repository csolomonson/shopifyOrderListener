import subprocess
import sys
import tempfile
from pathlib import Path
import unittest


ROOT = Path(__file__).resolve().parents[1]
CADDY_PATCHER = ROOT / "deployment" / "ubuntu" / "ensure-caddy-route.py"
SAME_VM_INSTALLER = ROOT / "deployment" / "ubuntu" / "deploy-on-costing-vm.sh"


class CaddyRouteTests(unittest.TestCase):
    def test_route_is_inserted_once_ahead_of_costing_fallback(self):
        source = """https://{$APP_HOSTNAME} {
  tls internal
  reverse_proxy 127.0.0.1:8000
}
"""
        with tempfile.TemporaryDirectory() as directory:
            path = Path(directory) / "Caddyfile"
            path.write_text(source, encoding="utf-8")

            first = subprocess.run([sys.executable, str(CADDY_PATCHER), str(path)], capture_output=True, text=True)
            second = subprocess.run([sys.executable, str(CADDY_PATCHER), str(path)], capture_output=True, text=True)

            self.assertEqual(0, first.returncode, first.stderr)
            self.assertEqual(0, second.returncode, second.stderr)
            updated = path.read_text(encoding="utf-8")
            self.assertEqual(1, updated.count("BEGIN shopify-order-listener managed route"))
            self.assertLess(updated.index("handle /sales-orders*"), updated.index("handle {"))
            self.assertEqual(1, updated.count("reverse_proxy 127.0.0.1:8000"))

    def test_unknown_caddy_shape_is_not_modified(self):
        source = "https://example.test {\n  respond \"ok\"\n}\n"
        with tempfile.TemporaryDirectory() as directory:
            path = Path(directory) / "Caddyfile"
            path.write_text(source, encoding="utf-8")

            result = subprocess.run([sys.executable, str(CADDY_PATCHER), str(path)], capture_output=True, text=True)

            self.assertNotEqual(0, result.returncode)
            self.assertEqual(source, path.read_text(encoding="utf-8"))


class InstallerRegressionTests(unittest.TestCase):
    def test_retaining_an_existing_secret_returns_success(self):
        installer = SAME_VM_INSTALLER.read_text(encoding="utf-8")

        self.assertIn('[[ -n "$first" ]] || return 0', installer)


if __name__ == "__main__":
    unittest.main()
