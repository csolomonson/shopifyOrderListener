"""Shared Basic authentication plus signed Shopify iframe sessions."""

from __future__ import annotations

import base64
import binascii
import hashlib
import hmac
import json
import secrets
import time
from dataclasses import dataclass
from pathlib import Path
from urllib.parse import parse_qsl

from fastapi import Request

from app_config import boolean_setting, setting


@dataclass(frozen=True)
class Principal:
    username: str
    groups: tuple[str, ...] = ()
    embedded: bool = False


class AuthenticationError(Exception):
    pass


class AuthorizationError(Exception):
    pass


def authorize_sales_orders(principal: Principal) -> None:
    required = setting("SALES_ORDER_ACCESS_GROUP", "sales-orders") or "sales-orders"
    if required not in principal.groups and "administrators" not in principal.groups:
        raise AuthorizationError(f"Membership in {required} is required")


def _users() -> dict[str, dict]:
    filename = setting("SALES_ORDER_USERS_JSON_FILE") or setting("COST_APP_USERS_JSON_FILE")
    if filename:
        try:
            payload = json.loads(Path(filename).read_text(encoding="utf-8"))
        except (OSError, json.JSONDecodeError) as exc:
            raise RuntimeError("Could not read the shared application user directory") from exc
        return {name: ({"password_hash": value, "groups": []} if isinstance(value, str) else value) for name, value in payload.items()}
    username = setting("SALES_ORDER_USERNAME")
    password = setting("SALES_ORDER_PASSWORD")
    return {username: {"password": password, "groups": ["administrators"]}} if username and password else {}


def _verify_password(password: str, user: dict) -> bool:
    encoded = user.get("password_hash")
    if encoded:
        try:
            scheme, iterations, salt, expected = encoded.split("$", 3)
            actual = hashlib.pbkdf2_hmac("sha256", password.encode(), bytes.fromhex(salt), int(iterations)).hex()
            return scheme == "pbkdf2_sha256" and secrets.compare_digest(actual, expected)
        except (TypeError, ValueError):
            return False
    configured = user.get("password")
    return configured is not None and secrets.compare_digest(password, str(configured))


def _session_secret() -> bytes:
    value = setting("SALES_ORDER_SESSION_SECRET")
    if not value:
        raise RuntimeError("SALES_ORDER_SESSION_SECRET is required for Shopify embedding")
    return value.encode()


def verify_shopify_query(raw_query: str) -> str:
    values = dict(parse_qsl(raw_query, keep_blank_values=True))
    supplied = values.pop("hmac", "")
    shop = values.get("shop", "")
    expected_shop = (setting("SHOPIFY_SHOP", "") or "").replace(".myshopify.com", "")
    if not supplied or not shop.endswith(".myshopify.com") or (expected_shop and shop != f"{expected_shop}.myshopify.com"):
        raise AuthenticationError
    message = "&".join(f"{key}={value}" for key, value in sorted(values.items()))
    expected = hmac.new((setting("SHOPIFY_CLIENT_SECRET", "") or "").encode(), message.encode(), hashlib.sha256).hexdigest()
    if not secrets.compare_digest(supplied, expected):
        raise AuthenticationError
    timestamp = int(values.get("timestamp", "0"))
    if abs(time.time() - timestamp) > 300:
        raise AuthenticationError
    return shop


def embed_cookie(shop: str) -> str:
    expires = int(time.time()) + 3600
    payload = f"{shop}|{expires}"
    signature = hmac.new(_session_secret(), payload.encode(), hashlib.sha256).hexdigest()
    return f"{payload}|{signature}"


def _verify_embed_cookie(value: str) -> Principal | None:
    try:
        shop, expires, signature = value.split("|", 2)
        payload = f"{shop}|{expires}"
        expected = hmac.new(_session_secret(), payload.encode(), hashlib.sha256).hexdigest()
        if int(expires) < time.time() or not secrets.compare_digest(signature, expected):
            return None
        return Principal(f"shopify:{shop}", ("administrators",), True)
    except (ValueError, RuntimeError):
        return None


def authenticate_request(request: Request) -> Principal:
    embedded = _verify_embed_cookie(request.cookies.get("sales_order_embed", ""))
    if embedded:
        return embedded
    if not boolean_setting("SALES_ORDER_AUTH_REQUIRED", True):
        return Principal(setting("SALES_ORDER_DEV_USERNAME", "developer") or "developer", ("administrators",))
    header = request.headers.get("Authorization", "")
    if not header.startswith("Basic "):
        raise AuthenticationError
    try:
        username, password = base64.b64decode(header[6:], validate=True).decode().split(":", 1)
    except (binascii.Error, UnicodeDecodeError, ValueError):
        raise AuthenticationError from None
    user = _users().get(username)
    if not user or not _verify_password(password, user):
        raise AuthenticationError
    return Principal(username, tuple(user.get("groups", ())))
