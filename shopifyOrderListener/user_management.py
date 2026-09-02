"""Atomic self-service updates for the shared application user directory."""

from __future__ import annotations

import json
import os
import time
from contextlib import contextmanager
from pathlib import Path
from threading import Lock

from app_config import setting
from authentication import password_hash, password_matches_record


_PROCESS_LOCK = Lock()


class UserStoreError(RuntimeError):
    pass


def _users_path() -> Path:
    filename = setting("SALES_ORDER_USERS_JSON_FILE") or setting("COST_APP_USERS_JSON_FILE")
    if not filename:
        raise UserStoreError("The shared application user directory is not configured")
    return Path(filename)


@contextmanager
def _file_lock(path: Path):
    lock_path = path.with_suffix(path.suffix + ".lock")
    deadline = time.monotonic() + 5
    descriptor = None
    while descriptor is None:
        try:
            descriptor = os.open(str(lock_path), os.O_CREAT | os.O_EXCL | os.O_WRONLY)
        except FileExistsError:
            try:
                if time.time() - lock_path.stat().st_mtime > 30:
                    lock_path.unlink()
                    continue
            except FileNotFoundError:
                continue
            if time.monotonic() >= deadline:
                raise UserStoreError("The user directory is busy; try again")
            time.sleep(0.05)
    try:
        yield
    finally:
        os.close(descriptor)
        try:
            lock_path.unlink()
        except FileNotFoundError:
            pass


def change_own_password(username: str, current_password: str, new_password: str) -> None:
    if len(new_password) < 8:
        raise ValueError("New password must be at least 8 characters")
    path = _users_path()
    with _PROCESS_LOCK, _file_lock(path):
        try:
            users = json.loads(path.read_text(encoding="utf-8"))
        except (OSError, json.JSONDecodeError) as exc:
            raise UserStoreError("Could not read the shared application user directory") from exc
        record = users.get(username)
        if isinstance(record, str):
            record = {"password_hash": record, "groups": []}
            users[username] = record
        if not isinstance(record, dict):
            raise KeyError(username)
        if not password_matches_record(current_password, record):
            raise ValueError("Current password is incorrect")
        record.pop("password", None)
        record["password_hash"] = password_hash(new_password)
        temporary = path.with_suffix(f".{os.getpid()}.tmp")
        try:
            temporary.write_text(json.dumps(users, indent=2) + "\n", encoding="utf-8")
            temporary.chmod(0o600)
            os.replace(temporary, path)
        except OSError as exc:
            try:
                temporary.unlink()
            except OSError:
                pass
            raise UserStoreError("Could not update the shared application user directory") from exc
