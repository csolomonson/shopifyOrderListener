"""Print the effective database configuration and what the app's login can do.

The password is never printed. Run from the same shell that starts the server so
it sees the same environment:

    .\\.venv\\Scripts\\python.exe tools\\show_db_config.py
"""

from __future__ import annotations

import os
import sys
from pathlib import Path
from urllib.parse import unquote_plus

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
from app_config import database_url, setting


def masked(odbc: str) -> str:
    parts = []
    for pair in odbc.split(";"):
        key, sep, value = pair.partition("=")
        if key.strip().upper() in {"PWD", "PASSWORD"} and value:
            value = f"<{len(value)} chars hidden>"
        parts.append(f"{key}{sep}{value}")
    return ";".join(parts)


def main() -> None:
    print("=== Environment the app sees ===")
    for name in ("SALES_ORDER_STORAGE_MODE", "SALES_ORDER_DATABASE_URL", "SALES_ORDER_DB_SERVER",
                 "SALES_ORDER_DB_USERNAME", "SALES_ORDER_DB_DRIVER",
                 "SALES_ORDER_DB_TRUST_SERVER_CERTIFICATE"):
        print(f"  {name:<44} {setting(name) or '<unset>'}")
    for name in ("SALES_ORDER_DB_PASSWORD",):
        value = setting(name)
        source = f"{name}_FILE" if os.getenv(f"{name}_FILE") else name
        print(f"  {name:<44} {'set via ' + source if value else '<unset>'}")

    url = database_url()
    if not url:
        print("\nNo database is configured -- the app falls back to the in-memory store.")
        return
    print("\n=== Connection string (password masked) ===")
    print("  " + masked(unquote_plus(url.split("odbc_connect=", 1)[-1])))
    print("\n  Note: the database name is hardcoded to M1_ME in app_config.database_url().")

    try:
        from sqlalchemy import create_engine, text
    except ModuleNotFoundError:
        print("\nInstall requirements.txt to run the permission checks.")
        return

    print("\n=== Identity and permissions, as the app connects ===")
    engine = create_engine(url, pool_pre_ping=True, future=True)
    checks = {
        "server login (SUSER_SNAME)": "SELECT SUSER_SNAME()",
        "database user (USER_NAME)": "SELECT USER_NAME()",
        "database (DB_NAME)": "SELECT DB_NAME()",
        "SalesOrders schema exists": "SELECT CASE WHEN SCHEMA_ID(N'SalesOrders') IS NULL THEN 'no' ELSE 'yes' END",
        "is db_owner": "SELECT CASE WHEN IS_ROLEMEMBER('db_owner') = 1 THEN 'yes' ELSE 'no' END",
        "is db_datareader": "SELECT CASE WHEN IS_ROLEMEMBER('db_datareader') = 1 THEN 'yes' ELSE 'no' END",
        "is db_datawriter": "SELECT CASE WHEN IS_ROLEMEMBER('db_datawriter') = 1 THEN 'yes' ELSE 'no' END",
    }
    try:
        with engine.connect() as connection:
            for label, statement in checks.items():
                print(f"  {label:<30} {connection.execute(text(statement)).scalar_one()}")

            rows = connection.execute(text(
                "SELECT permission_name FROM fn_my_permissions('SalesOrders', 'SCHEMA') ORDER BY permission_name"
            )).scalars().all()
            print(f"  {'rights ON SCHEMA::SalesOrders':<30} {', '.join(rows) if rows else '<none>'}")

            can_read = connection.execute(text(
                "SELECT CASE WHEN OBJECT_ID(N'SalesOrders.Orders') IS NULL THEN 'table missing'"
                "            ELSE 'visible' END"
            )).scalar_one()
            print(f"  {'SalesOrders.Orders':<30} {can_read}")
    except Exception as exc:
        print(f"  connection failed: {type(exc).__name__}: {str(exc)[:300]}")
    finally:
        engine.dispose()


if __name__ == "__main__":
    main()
