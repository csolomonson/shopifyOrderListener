"""Idempotently add the sales-order route ahead of the costing fallback."""

from __future__ import annotations

import os
from pathlib import Path
import shutil
import sys
import tempfile


MARKER = "# BEGIN shopify-order-listener managed route"
FALLBACK = "  reverse_proxy 127.0.0.1:8000"
ROUTES = """  # BEGIN shopify-order-listener managed route
  handle /sales-orders* {
    reverse_proxy 127.0.0.1:8010
  }

  handle {
    reverse_proxy 127.0.0.1:8000
  }
  # END shopify-order-listener managed route"""


def main() -> int:
    path = Path(sys.argv[1] if len(sys.argv) > 1 else "/etc/caddy/Caddyfile")
    source = path.read_text(encoding="utf-8")
    if MARKER in source:
        return 0
    if source.count(FALLBACK) != 1:
        print(
            f"Refusing to modify {path}: expected exactly one costing fallback {FALLBACK!r}.",
            file=sys.stderr,
        )
        return 1
    updated = source.replace(FALLBACK, ROUTES, 1)
    descriptor, temporary_name = tempfile.mkstemp(prefix=".Caddyfile.sales-orders.", dir=path.parent)
    try:
        with os.fdopen(descriptor, "w", encoding="utf-8", newline="\n") as temporary:
            temporary.write(updated)
        shutil.copystat(path, temporary_name)
        os.replace(temporary_name, path)
    finally:
        if os.path.exists(temporary_name):
            os.unlink(temporary_name)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
