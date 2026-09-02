"""Command-line entry point for Shopify-to-M1 reconciliation.

This module deliberately uses the same store and API clients as the web
application.  It can therefore be called interactively, by cron, or by a
systemd oneshot service without going through the HTTP API.
"""

from __future__ import annotations

import argparse
import json
import sys
from typing import Any, Sequence

from integrations.m1 import M1Client
from integrations.shopify import ShopifyClient
from storage import get_store
from sync_service import SyncService


def sync(*, full: bool = False) -> dict[str, Any]:
    """Run one reconciliation and return its summary."""
    shopify = ShopifyClient()
    if not shopify.configured:
        raise RuntimeError("Shopify Admin API is not configured")

    m1 = M1Client()
    if not m1.configured:
        raise RuntimeError("M1 Public API is not configured")

    return SyncService(get_store(), shopify=shopify, m1=m1).run(full=full)


def _parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        prog="shopify-order-sync",
        description="Synchronize Shopify orders into the SalesOrders staging queue.",
    )
    parser.add_argument(
        "--full",
        action="store_true",
        help="reconcile every Shopify order visible to the app instead of only recently updated orders",
    )
    return parser


def main(argv: Sequence[str] | None = None) -> int:
    args = _parser().parse_args(argv)
    try:
        result = sync(full=args.full)
    except Exception as exc:
        print(f"Shopify order synchronization failed: {exc}", file=sys.stderr)
        return 1

    print(json.dumps(result, sort_keys=True))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
