"""Sales-order queue HTTP application mounted at ``/sales-orders``."""

from __future__ import annotations

import asyncio
import json
import logging
from decimal import Decimal
from contextlib import asynccontextmanager
from pathlib import Path
from typing import Any

from fastapi import FastAPI, HTTPException, Request
from fastapi.responses import HTMLResponse, JSONResponse
from fastapi.staticfiles import StaticFiles
from pydantic import BaseModel, Field

from app_config import BASE_PATH, boolean_setting, integer_setting, setting
from authentication import AuthenticationError, authenticate_request, embed_cookie, verify_shopify_query
from commit_service import CommitError, CommitService
from customer_matching import CustomerMatcher
from integrations.m1 import M1Client
from integrations.shopify import ShopifyClient
from storage import get_store
from sync_service import SyncService


logger = logging.getLogger(__name__)
BASE_DIR = Path(__file__).resolve().parents[1]
STATIC_DIR = BASE_DIR / "static"


async def _shopify_worker(*, startup_sync: bool, background_sync: bool) -> None:
    """Run startup reconciliation and polling serially outside app readiness."""
    if startup_sync:
        shopify = ShopifyClient()
        if shopify.configured:
            try:
                await asyncio.to_thread(SyncService(get_store(), shopify=shopify).run, full=True)
            except Exception:
                logger.exception("Startup Shopify/M1 reconciliation failed")

    if not background_sync:
        return

    while True:
        store = get_store()
        settings = store.get_settings()
        delay = max(30, int(settings.get("sync_interval_seconds", 60)))
        await asyncio.sleep(delay)
        if settings.get("shopify_polling_enabled", True) and ShopifyClient().configured:
            try:
                await asyncio.to_thread(SyncService(store).run)
            except Exception:
                logger.exception("Scheduled Shopify synchronization failed")


@asynccontextmanager
async def lifespan(_: FastAPI):
    task = None
    startup_sync = boolean_setting("SALES_ORDER_STARTUP_SYNC", True)
    background_sync = boolean_setting("SALES_ORDER_BACKGROUND_SYNC", True)
    if startup_sync or background_sync:
        # The full pass runs first in this single worker, so it cannot overlap
        # periodic polling. Scheduling it instead of awaiting it lets health
        # checks and the review UI come online independently of backlog size.
        task = asyncio.create_task(
            _shopify_worker(startup_sync=startup_sync, background_sync=background_sync)
        )
    yield
    if task:
        task.cancel()
        try:
            await task
        except asyncio.CancelledError:
            pass


app = FastAPI(title="Shopify Sales Order Queue", version="0.1.0", lifespan=lifespan)
app.mount(f"{BASE_PATH}/static", StaticFiles(directory=STATIC_DIR), name="sales-order-static")


PUBLIC_PATHS = {f"{BASE_PATH}/api/health", f"{BASE_PATH}/shopify"}


def _customer_matcher() -> CustomerMatcher:
    return CustomerMatcher(directory_source=get_store())


@app.middleware("http")
async def authentication_and_headers(request: Request, call_next):
    path = request.url.path
    if path.startswith(f"{BASE_PATH}/static/") or path in PUBLIC_PATHS:
        response = await call_next(request)
    else:
        try:
            request.state.principal = authenticate_request(request)
        except AuthenticationError:
            return JSONResponse(
                {"detail": "Authentication required"},
                status_code=401,
                headers={"WWW-Authenticate": 'Basic realm="Sales Orders", charset="UTF-8"'},
            )
        except RuntimeError as exc:
            return JSONResponse({"detail": str(exc)}, status_code=503)
        response = await call_next(request)
    response.headers["Cache-Control"] = "no-store"
    response.headers["X-Content-Type-Options"] = "nosniff"
    if path == f"{BASE_PATH}/shopify":
        response.headers["Content-Security-Policy"] = "frame-ancestors https://admin.shopify.com https://*.myshopify.com"
    else:
        response.headers["X-Frame-Options"] = "DENY"
    return response


def _shell(mode: str, principal: dict[str, Any]) -> HTMLResponse:
    html = (STATIC_DIR / "index.html").read_text(encoding="utf-8")
    bootstrap = json.dumps({"mode": mode, "basePath": BASE_PATH, "principal": principal}).replace("<", "\\u003c")
    return HTMLResponse(html.replace("__BOOTSTRAP_JSON__", bootstrap))


@app.get(f"{BASE_PATH}")
@app.get(f"{BASE_PATH}/")
def index(request: Request):
    principal = request.state.principal
    return _shell("queue", {"username": principal.username, "groups": principal.groups})


@app.get(f"{BASE_PATH}/shopify")
def shopify_embed(request: Request):
    try:
        shop = verify_shopify_query(request.url.query)
    except AuthenticationError:
        raise HTTPException(status_code=401, detail="Invalid or expired Shopify app launch") from None
    response = _shell("shopify", {"username": f"shopify:{shop}", "groups": ["administrators"]})
    response.set_cookie("sales_order_embed", embed_cookie(shop), max_age=3600, secure=True, httponly=True, samesite="none", path=BASE_PATH)
    return response


@app.get(f"{BASE_PATH}/api/health")
def health():
    return {"ok": True, "version": setting("APP_VERSION", "development")}


@app.get(f"{BASE_PATH}/api/ready")
def ready():
    try:
        database_ready = get_store().ready()
    except Exception:
        logger.exception("Sales-order database readiness failed")
        database_ready = False
    if not database_ready:
        raise HTTPException(status_code=503, detail="SalesOrders schema is unavailable")
    return {"ok": True, "database": "M1_ME.SalesOrders"}


@app.get(f"{BASE_PATH}/api/dashboard")
def dashboard():
    return get_store().dashboard()


@app.get(f"{BASE_PATH}/api/orders")
def orders(state: str = "all", search: str = ""):
    return {"orders": get_store().list_orders(state, search)}


@app.get(f"{BASE_PATH}/api/orders/{{order_id}}")
def order(order_id: str):
    row = get_store().get_order(order_id)
    if not row:
        raise HTTPException(status_code=404, detail="Order not found")
    return row


class MatchRequest(BaseModel):
    organization_id: str = Field(min_length=1, max_length=10)
    location_id: str | None = Field(default=None, max_length=5)
    contact_id: str | None = Field(default=None, max_length=5)
    billing_location_id: str | None = Field(default=None, max_length=5)
    billing_contact_id: str | None = Field(default=None, max_length=5)
    requires_review: bool = False


@app.get(f"{BASE_PATH}/api/orders/{{order_id}}/customer-candidates")
def customer_candidates(order_id: str, query: str = ""):
    row = get_store().get_order(order_id)
    if not row:
        raise HTTPException(status_code=404, detail="Order not found")
    try:
        return _customer_matcher().search(row, query, include_fuzzy=not bool(query.strip()))
    except Exception as exc:
        raise HTTPException(status_code=502, detail=f"M1 customer search failed: {exc}") from exc


@app.get(f"{BASE_PATH}/api/orders/{{order_id}}/customer-resolution")
def customer_resolution(order_id: str, organization_id: str = ""):
    store = get_store()
    row = store.get_order(order_id)
    if not row: raise HTTPException(status_code=404, detail="Order not found")
    if not organization_id and row.get("customer_resolution"):
        return row["customer_resolution"]
    returning_match = None
    if row.get("matched_organization_id") and row.get("matched_organization_id") != "__NEW__":
        returning_match = {"organization_id": row["matched_organization_id"]}
    else:
        returning_match = store.find_shopify_customer_match(row.get("shopify_customer_id"))
    try:
        return _customer_matcher().resolution(row, returning_match, organization_id)
    except Exception as exc:
        raise HTTPException(status_code=502, detail=f"M1 customer recommendation failed: {exc}") from exc


@app.put(f"{BASE_PATH}/api/orders/{{order_id}}/customer-match")
def select_customer(order_id: str, selection: MatchRequest, request: Request):
    store = get_store()
    row = store.get_order(order_id)
    if not row: raise HTTPException(status_code=404, detail="Order not found")
    try:
        matcher = _customer_matcher()
        validation = matcher.validate_selection(row, selection.organization_id, selection.location_id, selection.contact_id,
                                                selection.billing_location_id, selection.billing_contact_id)
        row = store.set_match(order_id, selection.organization_id, selection.location_id, selection.contact_id, not validation["safe"],
                              selection.billing_location_id, selection.billing_contact_id, validation)
        resolution = matcher.resolution(row, organization_id=selection.organization_id)
        resolution["selection"] = selection.model_dump()
        row = store.set_customer_resolution(order_id, resolution)
    except KeyError:
        raise HTTPException(status_code=404, detail="Order not found") from None
    store.add_event(order_id, "customer_match_confirmed", selection.model_dump(), request.state.principal.username)
    _refresh_preview(order_id)
    return row


@app.get(f"{BASE_PATH}/api/m1/organizations/{{organization_id}}")
def m1_organization(organization_id: str):
    result = _customer_matcher().organization(organization_id)
    if not result: raise HTTPException(status_code=404, detail="M1 organization not found")
    return result


class OverrideRequest(BaseModel):
    confirmation: str
    reason: str = Field(min_length=10, max_length=500)


@app.post(f"{BASE_PATH}/api/orders/{{order_id}}/address-override")
def address_override(order_id: str, body: OverrideRequest, request: Request):
    if body.confirmation != "USE M1 ADDRESSES":
        raise HTTPException(status_code=400, detail='Type "USE M1 ADDRESSES" exactly to override address validation')
    try: row = get_store().set_address_override(order_id, body.reason, request.state.principal.username)
    except KeyError: raise HTTPException(status_code=404, detail="Order not found") from None
    except ValueError as exc: raise HTTPException(status_code=409, detail=str(exc)) from exc
    _refresh_preview(order_id)
    return row


class LineRequest(BaseModel):
    shopify_line_id: str
    sku: str = Field(min_length=1, max_length=80)
    description: str = Field(min_length=1, max_length=500)
    variant: str = Field(default="", max_length=250)
    current_quantity: Decimal = Field(ge=0)
    unit_price: Decimal = Field(ge=0)


@app.put(f"{BASE_PATH}/api/orders/{{order_id}}/lines")
def edit_lines(order_id: str, lines: list[LineRequest], request: Request):
    if not lines: raise HTTPException(status_code=400, detail="At least one line is required")
    normalized = [{**line.model_dump(), "line_number": index, "original_quantity": line.current_quantity,
                   "line_total": line.current_quantity * line.unit_price} for index, line in enumerate(lines, start=1)]
    try: row = get_store().update_lines(order_id, normalized, request.state.principal.username)
    except KeyError: raise HTTPException(status_code=404, detail="Order not found") from None
    _refresh_preview(order_id)
    return row


def _refresh_preview(order_id: str) -> None:
    """Rebuild the stored preview after an edit, without failing the edit itself."""
    try:
        CommitService(get_store()).refresh_preview(order_id)
    except Exception:
        # The next read rebuilds it: cached_preview only serves a preview whose
        # fingerprint still matches the order.
        logger.exception("Could not refresh the stored M1 preview for order %s", order_id)


@app.get(f"{BASE_PATH}/api/orders/{{order_id}}/m1-preview")
def m1_preview(order_id: str):
    try:
        envelope = CommitService(get_store()).cached_preview(order_id)
    except KeyError:
        raise HTTPException(status_code=404, detail="Order not found") from None
    except RuntimeError as exc:
        raise HTTPException(status_code=409, detail=str(exc)) from exc
    if not envelope.get("ok"):
        raise HTTPException(status_code=409, detail=envelope.get("detail") or "This order cannot be committed yet")
    return envelope["preview"]


class CommitRequest(BaseModel):
    confirmed: bool


@app.post(f"{BASE_PATH}/api/orders/{{order_id}}/commit")
def commit(order_id: str, body: CommitRequest, request: Request):
    if not body.confirmed:
        raise HTTPException(status_code=400, detail="Explicit confirmation is required")
    try:
        return CommitService(get_store()).commit(order_id, request.state.principal.username)
    except KeyError:
        raise HTTPException(status_code=404, detail="Order not found") from None
    except (CommitError, RuntimeError) as exc:
        raise HTTPException(status_code=409, detail=str(exc)) from exc


@app.post(f"{BASE_PATH}/api/sync")
async def sync(full: bool = False):
    try:
        return await asyncio.to_thread(SyncService(get_store()).run, full=full)
    except Exception as exc:
        raise HTTPException(status_code=502, detail=f"Shopify synchronization failed: {exc}") from exc


@app.get(f"{BASE_PATH}/api/settings")
def get_settings():
    return get_store().get_settings()


class SettingsRequest(BaseModel):
    sync_interval_seconds: int = Field(ge=30, le=86400)
    sync_lookback_minutes: int = Field(ge=1, le=1440)
    default_warehouse: str = Field(min_length=1, max_length=30)
    default_bin: str = Field(min_length=1, max_length=30)
    default_uom: str = Field(min_length=1, max_length=10)
    auto_match_threshold: int = Field(ge=0, le=100)
    shopify_polling_enabled: bool


@app.put(f"{BASE_PATH}/api/settings")
def save_settings(body: SettingsRequest, request: Request):
    if "administrators" not in request.state.principal.groups:
        raise HTTPException(status_code=403, detail="Administrator access is required")
    return get_store().save_settings(body.model_dump(), request.state.principal.username)


@app.get(f"{BASE_PATH}/api/diagnostics")
def diagnostics():
    store = get_store()
    return {
        "database": {"ok": store.ready(), "target": "M1_ME.SalesOrders"},
        "shopify": {"configured": ShopifyClient().configured, "api_version": setting("SHOPIFY_API_VERSION", "2026-07")},
        "m1": {"configured": M1Client().configured, "writes_enabled": M1Client().writes_enabled, "transport": "M1 Public API"},
        "sync": store.last_sync(),
        "ingestion": "outbound_polling",
        "public_webhooks_required": False,
    }
