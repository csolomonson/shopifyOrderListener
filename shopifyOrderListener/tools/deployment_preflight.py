"""Read-only production dependency checks for the same-VM installer."""

from __future__ import annotations

import sys

from integrations.m1 import M1Client
from integrations.shopify import ShopifyClient
from storage import get_store


def main() -> int:
    checks = (
        ("SalesOrders database", lambda: get_store().ready()),
        ("M1 Public API", lambda: M1Client().probe()),
        ("Shopify Admin API", lambda: ShopifyClient().probe()),
    )
    failed = False
    for label, action in checks:
        try:
            result = action()
            if result is False:
                raise RuntimeError("readiness returned false")
            print(f"OK: {label}")
        except Exception as exc:
            failed = True
            print(f"FAILED: {label}: {exc}", file=sys.stderr)
    return 1 if failed else 0


if __name__ == "__main__":
    raise SystemExit(main())
