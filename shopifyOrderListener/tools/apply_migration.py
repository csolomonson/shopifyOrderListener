"""Apply one reviewed, idempotent SQL Server migration file."""

from __future__ import annotations

import re
import sys
from pathlib import Path

from sqlalchemy import create_engine

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
from app_config import database_url


def main() -> None:
    if len(sys.argv) != 2:
        raise SystemExit("usage: apply_migration.py <migration.sql>")
    migration = Path(sys.argv[1]).resolve()
    if migration.parent != (Path(__file__).resolve().parents[1] / "database") or migration.suffix.lower() != ".sql":
        raise SystemExit("migration must be a .sql file in this application's database directory")
    url = database_url()
    if not url:
        raise SystemExit("SQL Server is not configured")
    batches = [batch.strip() for batch in re.split(r"^\s*GO\s*$", migration.read_text(encoding="utf-8"), flags=re.MULTILINE | re.IGNORECASE) if batch.strip()]
    engine = create_engine(url, pool_pre_ping=True, future=True)
    connection = engine.raw_connection()
    try:
        cursor = connection.cursor()
        for batch in batches:
            cursor.execute(batch)
        connection.commit()
    except Exception:
        connection.rollback()
        raise
    finally:
        connection.close()
        engine.dispose()
    print(f"Applied {migration.name}")


if __name__ == "__main__":
    main()
