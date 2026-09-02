"""Check or apply the app-owned SalesOrders schema using runtime SQL settings."""

from __future__ import annotations

import argparse
from pathlib import Path
import re
import sys

from app_config import boolean_setting, setting


ROOT = Path(__file__).resolve().parents[1]
MIGRATION = ROOT / "database" / "apply_all_migrations.sql"
REQUIRED_TABLES = {
    "AuditEvents",
    "CustomerCandidates",
    "OrderLines",
    "Orders",
    "Refunds",
    "SchemaVersions",
    "Settings",
    "SyncRuns",
}


def connection():
    import pyodbc

    server = setting("SALES_ORDER_DB_SERVER", "") or ""
    username = setting("SALES_ORDER_DB_USERNAME", "") or ""
    password = setting("SALES_ORDER_DB_PASSWORD", "") or ""
    driver = setting("SALES_ORDER_DB_DRIVER", "ODBC Driver 18 for SQL Server") or ""
    if not server or not username or not password:
        raise RuntimeError("The shared SQL server, username, and password are not configured")
    trust = "yes" if boolean_setting("SALES_ORDER_DB_TRUST_SERVER_CERTIFICATE") else "no"
    return pyodbc.connect(
        f"DRIVER={{{driver}}};SERVER={server};DATABASE=M1_ME;UID={username};PWD={password};"
        f"Encrypt=yes;TrustServerCertificate={trust};Connection Timeout=10",
        autocommit=True,
    )


def apply(cursor) -> None:
    sql = MIGRATION.read_text(encoding="utf-8-sig")
    for batch in re.split(r"^\s*GO\s*(?:--.*)?$", sql, flags=re.IGNORECASE | re.MULTILINE):
        if not batch.strip():
            continue
        cursor.execute(batch)
        while cursor.nextset():
            pass


def check(cursor) -> None:
    tables = {
        row[0]
        for row in cursor.execute(
            "SELECT name FROM sys.tables WHERE schema_id=SCHEMA_ID(N'SalesOrders')"
        ).fetchall()
    }
    missing = sorted(REQUIRED_TABLES - tables)
    if missing:
        raise RuntimeError("SalesOrders is missing tables: " + ", ".join(missing))

    versions = {
        int(row[0])
        for row in cursor.execute(
            "SELECT VersionNumber FROM SalesOrders.SchemaVersions"
        ).fetchall()
    }
    missing_versions = sorted({1, 2, 3, 4} - versions)
    if missing_versions:
        raise RuntimeError(
            "SalesOrders is missing schema versions: "
            + ", ".join(str(value) for value in missing_versions)
        )

    permissions = cursor.execute(
        """
        SELECT
          HAS_PERMS_BY_NAME(N'SalesOrders', N'SCHEMA', N'SELECT'),
          HAS_PERMS_BY_NAME(N'SalesOrders', N'SCHEMA', N'INSERT'),
          HAS_PERMS_BY_NAME(N'SalesOrders', N'SCHEMA', N'UPDATE'),
          HAS_PERMS_BY_NAME(N'SalesOrders', N'SCHEMA', N'DELETE')
        """
    ).fetchone()
    names = ("SELECT", "INSERT", "UPDATE", "DELETE")
    missing_permissions = [name for name, allowed in zip(names, permissions) if allowed != 1]
    if missing_permissions:
        raise RuntimeError(
            "The shared SQL login lacks SalesOrders permissions: "
            + ", ".join(missing_permissions)
        )
    print("SalesOrders schema versions 1-4 and CRUD permissions are ready.")


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--apply", action="store_true", help="Apply the idempotent migrations before checking")
    args = parser.parse_args()
    try:
        with connection() as sql_connection:
            cursor = sql_connection.cursor()
            if args.apply:
                apply(cursor)
            check(cursor)
    except Exception as exc:
        print(f"SalesOrders database setup failed: {exc}", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
