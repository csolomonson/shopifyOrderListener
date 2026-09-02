"""Environment-backed runtime configuration.

Secrets may be supplied directly or through a ``*_FILE`` variable. Production
deployment uses the file form so credentials never live in a release bundle.
"""

from __future__ import annotations

import os
from pathlib import Path


BASE_PATH = "/sales-orders"


def setting(name: str, default: str | None = None, *, required: bool = False) -> str | None:
    filename = os.getenv(f"{name}_FILE")
    if filename:
        try:
            value = Path(filename).read_text(encoding="utf-8").strip()
        except OSError as exc:
            raise RuntimeError(f"Could not read secret file configured by {name}_FILE") from exc
    else:
        value = os.getenv(name, default)
    if required and not value:
        raise RuntimeError(f"{name} or {name}_FILE must be configured")
    return value


def boolean_setting(name: str, default: bool = False) -> bool:
    value = setting(name)
    return default if value is None else value.lower() in {"1", "true", "yes", "on"}


def integer_setting(name: str, default: int) -> int:
    value = setting(name)
    return default if value is None else int(value)


def database_url() -> str | None:
    explicit = setting("SALES_ORDER_DATABASE_URL")
    if explicit:
        return explicit
    server = setting("SALES_ORDER_DB_SERVER")
    username = setting("SALES_ORDER_DB_USERNAME")
    password = setting("SALES_ORDER_DB_PASSWORD")
    if not server or not username or not password:
        return None
    from urllib.parse import quote_plus

    driver = setting("SALES_ORDER_DB_DRIVER", "ODBC Driver 18 for SQL Server")
    trust = "yes" if boolean_setting("SALES_ORDER_DB_TRUST_SERVER_CERTIFICATE") else "no"
    odbc = (
        f"DRIVER={{{driver}}};SERVER={server};DATABASE=M1_ME;UID={username};PWD={password};"
        f"Encrypt=yes;TrustServerCertificate={trust};Connection Timeout=5"
    )
    return "mssql+pyodbc:///?odbc_connect=" + quote_plus(odbc)
