import json
import os
import tempfile
import unittest
from pathlib import Path
from unittest.mock import patch

from authentication import password_hash, password_matches_record
from user_management import change_own_password


class UserManagementTests(unittest.TestCase):
    def setUp(self):
        self.directory = tempfile.TemporaryDirectory()
        self.path = Path(self.directory.name, "users.json")
        self.path.write_text(json.dumps({
            "shipper": {
                "password_hash": password_hash("old-password", iterations=10),
                "groups": ["sales-orders"],
            }
        }))
        self.environment = patch.dict(
            os.environ, {"SALES_ORDER_USERS_JSON_FILE": str(self.path)}, clear=True
        )
        self.environment.start()

    def tearDown(self):
        self.environment.stop()
        self.directory.cleanup()

    def test_user_changes_password_without_changing_groups(self):
        change_own_password("shipper", "old-password", "new-password")

        changed = json.loads(self.path.read_text())
        self.assertTrue(password_matches_record("new-password", changed["shipper"]))
        self.assertEqual(changed["shipper"]["groups"], ["sales-orders"])

    def test_current_password_is_required(self):
        with self.assertRaisesRegex(ValueError, "Current password is incorrect"):
            change_own_password("shipper", "wrong-password", "new-password")


if __name__ == "__main__":
    unittest.main()
