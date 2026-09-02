"""Persistence confined to the app-owned ``SalesOrders`` schema."""

from __future__ import annotations

import hashlib
import json
import threading
import uuid
from datetime import datetime, timezone
from decimal import Decimal
from typing import Any

try:
    from sqlalchemy import create_engine, text
except ModuleNotFoundError:  # Pure domain tests can run before production deps are installed.
    create_engine = None

    def text(statement: str) -> str:
        return statement

from app_config import database_url, setting
from domain import lifecycle_decision


def _json_default(value: Any) -> Any:
    if isinstance(value, Decimal):
        return str(value)
    if isinstance(value, (datetime,)):
        return value.isoformat()
    raise TypeError(f"Cannot serialize {type(value).__name__}")


def canonical_json(value: Any) -> str:
    return json.dumps(value, default=_json_default, sort_keys=True, separators=(",", ":"))


def source_hash(value: Any) -> str:
    return hashlib.sha256(canonical_json(value).encode("utf-8")).hexdigest()


class MemoryStore:
    """Development/test store with the same behavior as SQL storage."""

    def __init__(self) -> None:
        self.orders: dict[str, dict[str, Any]] = {}
        self.settings: dict[str, Any] = {
            "sync_interval_seconds": 60,
            "sync_lookback_minutes": 10,
            "default_warehouse": "142",
            "default_bin": "BIN1",
            "default_uom": "EA",
            "auto_match_threshold": 95,
            "shopify_polling_enabled": True,
        }
        self.events: list[dict[str, Any]] = []
        self.sync_runs: list[dict[str, Any]] = []
        self._lock = threading.Lock()

    def ready(self) -> bool:
        return True

    def upsert_order(self, order: dict[str, Any]) -> dict[str, Any]:
        with self._lock:
            key = order["shopify_order_id"]
            current = self.orders.get(key, {})
            digest = source_hash(order)
            candidate = {
                **current,
                **order,
                "order_id": current.get("order_id", str(uuid.uuid4())),
                "source_hash": digest,
                "committed_hash": current.get("committed_hash"),
                "erp_order_id": current.get("erp_order_id"),
                "erp_quantity_shipped": current.get("erp_quantity_shipped", 0),
                "match_requires_review": current.get("match_requires_review", False),
                "matched_organization_id": current.get("matched_organization_id"),
                "matched_location_id": current.get("matched_location_id"),
                "matched_contact_id": current.get("matched_contact_id"),
                "matched_billing_location_id": current.get("matched_billing_location_id"),
                "matched_billing_contact_id": current.get("matched_billing_contact_id"),
                "match_validation": current.get("match_validation"),
                "customer_resolution": current.get("customer_resolution"),
                "customer_resolution_at": current.get("customer_resolution_at"),
                "m1_preview": current.get("m1_preview"),
                "m1_preview_fingerprint": current.get("m1_preview_fingerprint"),
                "m1_preview_at": current.get("m1_preview_at"),
                "address_override": current.get("address_override"),
                "line_overrides": current.get("line_overrides"),
            }
            if candidate.get("line_overrides") is not None:
                candidate["lines"] = candidate["line_overrides"]
            decision = lifecycle_decision(candidate, candidate)
            previous_state = current.get("state")
            candidate.update(
                state=decision.state,
                severity=decision.severity,
                action_title=decision.title,
                action_detail=decision.detail,
                blocks_commit=decision.blocks_commit,
            )
            self.orders[key] = candidate
            if previous_state != decision.state:
                self.add_event(candidate["order_id"], "state_changed", {"from": previous_state, "to": decision.state})
            return dict(candidate)

    def list_orders(self, state: str | None = None, search: str = "") -> list[dict[str, Any]]:
        rows = list(self.orders.values())
        if state and state != "all":
            rows = [row for row in rows if row.get("state") == state]
        needle = search.lower().strip()
        if needle:
            rows = [
                row for row in rows
                if needle in " ".join(
                    str(row.get(name, ""))
                    for name in ("order_name", "customer_name", "email", "matched_organization_id", "erp_order_id")
                ).lower()
            ]
        rows = sorted(rows, key=lambda row: (row.get("created_at") or "", row.get("order_name") or ""), reverse=True)
        # Mirror SQL storage: the stored preview is omitted from list payloads.
        return [{**row, "m1_preview": None} for row in rows]

    def get_order(self, order_id: str) -> dict[str, Any] | None:
        return next(
            (dict(row) for row in self.orders.values() if row["order_id"] == order_id or row["shopify_order_id"] == order_id),
            None,
        )

    def set_match(self, order_id: str, organization_id: str, location_id: str | None, contact_id: str | None, requires_review: bool,
                  billing_location_id: str | None = None, billing_contact_id: str | None = None, validation: dict[str, Any] | None = None) -> dict[str, Any]:
        with self._lock:
            row = self.get_order(order_id)
            if not row:
                raise KeyError(order_id)
            stored = self.orders[row["shopify_order_id"]]
            stored.update(
                matched_organization_id=organization_id,
                matched_location_id=location_id,
                matched_contact_id=contact_id,
                matched_billing_location_id=billing_location_id or location_id,
                matched_billing_contact_id=billing_contact_id or contact_id,
                match_requires_review=requires_review,
                match_validation=validation,
                address_override=None,
            )
            decision = lifecycle_decision(stored, stored)
            stored.update(state=decision.state, severity=decision.severity, action_title=decision.title, action_detail=decision.detail, blocks_commit=decision.blocks_commit)
            self.add_event(stored["order_id"], "customer_match_selected", {"organization_id": organization_id, "requires_review": requires_review})
            return dict(stored)

    def set_customer_resolution(self, order_id: str, resolution: dict[str, Any] | None) -> dict[str, Any]:
        with self._lock:
            row = self.get_order(order_id)
            if not row:
                raise KeyError(order_id)
            stored = self.orders[row["shopify_order_id"]]
            stored["customer_resolution"] = resolution
            stored["customer_resolution_at"] = datetime.now(timezone.utc).isoformat() if resolution else None
            return dict(stored)

    def set_m1_preview(self, order_id: str, preview: dict[str, Any] | None, fingerprint: str | None) -> dict[str, Any]:
        with self._lock:
            row = self.get_order(order_id)
            if not row:
                raise KeyError(order_id)
            stored = self.orders[row["shopify_order_id"]]
            stored["m1_preview"] = preview
            stored["m1_preview_fingerprint"] = fingerprint if preview else None
            stored["m1_preview_at"] = datetime.now(timezone.utc).isoformat() if preview else None
            return dict(stored)

    def set_address_override(self, order_id: str, reason: str, actor: str) -> dict[str, Any]:
        row = self.get_order(order_id)
        if not row: raise KeyError(order_id)
        if (row.get("match_validation") or {}).get("status") != "address_mismatch":
            raise ValueError("Only a verified Shopify/M1 address mismatch can be overridden")
        stored = self.orders[row["shopify_order_id"]]
        stored["address_override"] = {"reason": reason, "actor": actor, "created_at": datetime.now(timezone.utc).isoformat()}
        stored["match_requires_review"] = False
        decision = lifecycle_decision(stored, stored)
        stored.update(state=decision.state, severity=decision.severity, action_title=decision.title, action_detail=decision.detail, blocks_commit=decision.blocks_commit)
        self.add_event(order_id, "address_mismatch_overridden", stored["address_override"], actor)
        return dict(stored)

    def update_lines(self, order_id: str, lines: list[dict[str, Any]], actor: str) -> dict[str, Any]:
        row = self.get_order(order_id)
        if not row: raise KeyError(order_id)
        stored = self.orders[row["shopify_order_id"]]
        stored["line_overrides"] = lines
        stored["lines"] = lines
        stored["subtotal"] = sum((Decimal(str(line["line_total"])) for line in lines), Decimal("0"))
        stored["total"] = stored["subtotal"] - Decimal(str(stored.get("discount") or 0)) + Decimal(str(stored.get("shipping") or 0)) + Decimal(str(stored.get("tax") or 0))
        self.add_event(order_id, "order_lines_edited", {"lines": lines}, actor)
        return dict(stored)

    def find_shopify_customer_match(self, shopify_customer_id: str | None) -> dict[str, Any] | None:
        if not shopify_customer_id:
            return None
        rows = [row for row in self.orders.values() if row.get("shopify_customer_id") == shopify_customer_id and row.get("matched_organization_id") not in {None, "", "__NEW__"}]
        if not rows:
            return None
        row = sorted(rows, key=lambda item: (bool(item.get("erp_order_id")), item.get("updated_at") or ""), reverse=True)[0]
        return {
            "organization_id": row.get("matched_organization_id"),
            "location_id": row.get("matched_location_id"),
            "contact_id": row.get("matched_contact_id"),
            "billing_location_id": row.get("matched_billing_location_id"),
            "billing_contact_id": row.get("matched_billing_contact_id"),
        }

    def mark_committed(self, order_id: str, erp_order_id: str) -> dict[str, Any]:
        with self._lock:
            row = self.get_order(order_id)
            if not row:
                raise KeyError(order_id)
            stored = self.orders[row["shopify_order_id"]]
            stored.update(erp_order_id=erp_order_id, committed_hash=stored["source_hash"], state="committed", severity="success", action_title="Added to M1", action_detail="No action required.", blocks_commit=True)
            self.add_event(stored["order_id"], "m1_order_created", {"erp_order_id": erp_order_id})
            return dict(stored)

    def mark_existing_in_m1(self, order_id: str, erp_order_id: str) -> dict[str, Any]:
        with self._lock:
            row = self.get_order(order_id)
            if not row:
                raise KeyError(order_id)
            stored = self.orders[row["shopify_order_id"]]
            stored.update(erp_order_id=erp_order_id, committed_hash=stored["source_hash"], state="committed", severity="success", action_title="Already in M1", action_detail="Matched by Shopify customer PO during reconciliation.", blocks_commit=True)
            self.add_event(stored["order_id"], "m1_order_reconciled", {"erp_order_id": erp_order_id})
            return dict(stored)

    def add_event(self, order_id: str | None, event_type: str, detail: dict[str, Any], actor: str = "system") -> None:
        self.events.append({"event_id": str(uuid.uuid4()), "order_id": order_id, "event_type": event_type, "detail": detail, "actor": actor, "created_at": datetime.now(timezone.utc).isoformat()})

    def get_settings(self) -> dict[str, Any]:
        return dict(self.settings)

    def save_settings(self, updates: dict[str, Any], actor: str) -> dict[str, Any]:
        allowed = set(self.settings)
        self.settings.update({key: value for key, value in updates.items() if key in allowed})
        self.add_event(None, "settings_changed", {"keys": sorted(updates)}, actor)
        return self.get_settings()

    def dashboard(self) -> dict[str, Any]:
        rows = list(self.orders.values())
        return {
            "total": len(rows),
            "ready": sum(row.get("state") == "ready" for row in rows),
            "review": sum(row.get("state", "").endswith("_review") for row in rows),
            "committed": sum(row.get("state") == "committed" for row in rows),
            "cancelled": sum(row.get("state") == "cancelled_before_erp" for row in rows),
        }

    def begin_sync(self) -> str:
        sync_id = str(uuid.uuid4())
        self.sync_runs.append({"sync_id": sync_id, "started_at": datetime.now(timezone.utc), "status": "running", "orders_seen": 0, "orders_changed": 0, "error": ""})
        return sync_id

    def finish_sync(self, sync_id: str, *, seen: int, changed: int, error: str = "") -> None:
        run = next(item for item in self.sync_runs if item["sync_id"] == sync_id)
        run.update(status="failed" if error else "succeeded", finished_at=datetime.now(timezone.utc), orders_seen=seen, orders_changed=changed, error=error)

    def last_sync(self) -> dict[str, Any] | None:
        return dict(self.sync_runs[-1]) if self.sync_runs else None


class SqlServerStore:
    """SQL Server repository restricted to ``SalesOrders.*`` objects."""

    def __init__(self, url: str):
        if create_engine is None:
            raise RuntimeError("SQLAlchemy is required for SQL storage; install requirements.txt")
        self.engine = create_engine(url, pool_pre_ping=True, future=True)

    def ready(self) -> bool:
        with self.engine.connect() as connection:
            row = connection.execute(text("SELECT CASE WHEN SCHEMA_ID(N'SalesOrders') IS NULL THEN 0 ELSE 1 END")).scalar_one()
            return bool(row)

    @staticmethod
    def _decode(row: Any, *, include_preview: bool = True) -> dict[str, Any]:
        """``include_preview`` is off for list payloads: the stored resource plan
        is large, is only needed when a single order is opened, and parsing one
        per row would make the queue slower rather than faster."""
        value = dict(row._mapping)
        normalized = json.loads(value.pop("NormalizedJson"))
        normalized.update(
            order_id=str(value.pop("OrderID")),
            state=value.pop("State"),
            severity=value.pop("Severity"),
            action_title=value.pop("ActionTitle"),
            action_detail=value.pop("ActionDetail"),
            blocks_commit=value.pop("BlocksCommit"),
            source_hash=value.pop("SourceHash"),
            committed_hash=value.pop("CommittedHash"),
            erp_order_id=value.pop("M1SalesOrderID"),
            erp_quantity_shipped=value.pop("M1QuantityShipped"),
            matched_organization_id=value.pop("M1OrganizationID"),
            matched_location_id=value.pop("M1LocationID"),
            matched_contact_id=value.pop("M1ContactID"),
            matched_billing_location_id=value.pop("M1BillingLocationID", None),
            matched_billing_contact_id=value.pop("M1BillingContactID", None),
            match_requires_review=value.pop("MatchRequiresReview"),
            match_validation=json.loads(value.pop("MatchValidationJson", "null") or "null"),
            customer_resolution=json.loads(value.pop("CustomerResolutionJson", "null") or "null"),
            customer_resolution_at=value.pop("CustomerResolutionAt", None),
            m1_preview=json.loads(value.pop("M1PreviewJson", "null") or "null") if include_preview else None,
            m1_preview_fingerprint=value.pop("M1PreviewFingerprint", None),
            m1_preview_at=value.pop("M1PreviewAt", None),
            address_override=json.loads(value.pop("AddressOverrideJson", "null") or "null"),
            line_overrides=json.loads(value.pop("LineOverridesJson", "null") or "null"),
        )
        if normalized.get("line_overrides") is not None:
            normalized["lines"] = normalized["line_overrides"]
            normalized["subtotal"] = sum((Decimal(str(line["line_total"])) for line in normalized["lines"]), Decimal("0"))
            normalized["total"] = normalized["subtotal"] - Decimal(str(normalized.get("discount") or 0)) + Decimal(str(normalized.get("shipping") or 0)) + Decimal(str(normalized.get("tax") or 0))
        return normalized

    def upsert_order(self, order: dict[str, Any]) -> dict[str, Any]:
        digest = source_hash(order)
        normalized_json = canonical_json(order)
        now = datetime.now(timezone.utc)
        with self.engine.begin() as connection:
            current_row = connection.execute(
                text("SELECT * FROM SalesOrders.Orders WITH (UPDLOCK, HOLDLOCK) WHERE ShopifyOrderID=:shopify_id"),
                {"shopify_id": order["shopify_order_id"]},
            ).first()
            current = self._decode(current_row) if current_row else {}
            candidate = {**current, **order, "source_hash": digest}
            decision = lifecycle_decision(candidate, candidate)
            params = {
                "shopify_id": order["shopify_order_id"], "legacy_id": order["legacy_order_id"],
                "order_name": order["order_name"], "updated_at": order["updated_at"], "source_hash": digest,
                "state": decision.state, "severity": decision.severity, "title": decision.title,
                "detail": decision.detail, "blocks": decision.blocks_commit, "normalized": normalized_json,
                "raw": canonical_json(order.get("raw", {})), "now": now,
            }
            if current_row:
                connection.execute(text("""
                    UPDATE SalesOrders.Orders SET LegacyOrderID=:legacy_id, OrderName=:order_name,
                      ShopifyUpdatedAt=:updated_at, SourceHash=:source_hash, State=:state,
                      Severity=:severity, ActionTitle=:title, ActionDetail=:detail,
                      BlocksCommit=:blocks, NormalizedJson=:normalized, RawJson=:raw, UpdatedAt=:now
                    WHERE ShopifyOrderID=:shopify_id
                """), params)
                order_id = current["order_id"]
            else:
                order_id = str(uuid.uuid4())
                params["order_id"] = order_id
                connection.execute(text("""
                    INSERT SalesOrders.Orders
                      (OrderID,ShopifyOrderID,LegacyOrderID,OrderName,ShopifyUpdatedAt,SourceHash,
                       State,Severity,ActionTitle,ActionDetail,BlocksCommit,NormalizedJson,RawJson,CreatedAt,UpdatedAt)
                    VALUES (:order_id,:shopify_id,:legacy_id,:order_name,:updated_at,:source_hash,
                            :state,:severity,:title,:detail,:blocks,:normalized,:raw,:now,:now)
                """), params)
            connection.execute(text("DELETE FROM SalesOrders.OrderLines WHERE OrderID=:order_id"), {"order_id": order_id})
            for line in order["lines"]:
                connection.execute(text("""
                    INSERT SalesOrders.OrderLines
                      (OrderLineID,OrderID,ShopifyLineID,LineNumber,SKU,Description,Variant,
                       OriginalQuantity,CurrentQuantity,UnitPrice,LineTotal)
                    VALUES (:id,:order_id,:shopify_line_id,:line_number,:sku,:description,:variant,
                            :original_quantity,:current_quantity,:unit_price,:line_total)
                """), {"id": str(uuid.uuid4()), "order_id": order_id, **line})
            connection.execute(text("DELETE FROM SalesOrders.Refunds WHERE OrderID=:order_id"), {"order_id": order_id})
            for refund in order["refunds"]:
                connection.execute(text("""
                    INSERT SalesOrders.Refunds
                      (RefundID,OrderID,ShopifyRefundID,ShopifyCreatedAt,ShopifyUpdatedAt,Amount,Note,PayloadJson)
                    VALUES (:id,:order_id,:shopify_refund_id,:created_at,:updated_at,:total,:note,:payload)
                """), {"id": str(uuid.uuid4()), "order_id": order_id, **refund, "payload": canonical_json(refund)})
        return self.get_order(order_id)  # type: ignore[return-value]

    def find_shopify_customer_match(self, shopify_customer_id: str | None) -> dict[str, Any] | None:
        if not shopify_customer_id:
            return None
        with self.engine.connect() as connection:
            row = connection.execute(text("""
              SELECT TOP (1) M1OrganizationID organization_id,M1LocationID location_id,M1ContactID contact_id,
                M1BillingLocationID billing_location_id,M1BillingContactID billing_contact_id
              FROM SalesOrders.Orders
              WHERE JSON_VALUE(NormalizedJson,'$.shopify_customer_id')=:customer_id
                AND M1OrganizationID IS NOT NULL
                AND M1OrganizationID<>'__NEW__'
              ORDER BY CASE WHEN M1SalesOrderID IS NULL THEN 1 ELSE 0 END,UpdatedAt DESC
            """), {"customer_id": shopify_customer_id}).mappings().first()
            return dict(row) if row else None

    def list_orders(self, state: str | None = None, search: str = "") -> list[dict[str, Any]]:
        clauses = []
        params: dict[str, Any] = {}
        if state and state != "all":
            clauses.append("State=:state")
            params["state"] = state
        if search.strip():
            clauses.append("(OrderName LIKE :search OR NormalizedJson LIKE :search OR M1SalesOrderID LIKE :search OR M1OrganizationID LIKE :search)")
            params["search"] = f"%{search.strip()}%"
        where = " WHERE " + " AND ".join(clauses) if clauses else ""
        with self.engine.connect() as connection:
            rows = connection.execute(text(f"""SELECT TOP (500) * FROM SalesOrders.Orders{where}
              ORDER BY TRY_CONVERT(datetimeoffset, JSON_VALUE(NormalizedJson,'$.created_at')) DESC, OrderName DESC"""), params)
            return [self._decode(row, include_preview=False) for row in rows]

    def get_order(self, order_id: str) -> dict[str, Any] | None:
        with self.engine.connect() as connection:
            row = connection.execute(text("SELECT * FROM SalesOrders.Orders WHERE CONVERT(nvarchar(36),OrderID)=:id OR ShopifyOrderID=:id"), {"id": order_id}).first()
            return self._decode(row) if row else None

    def set_match(self, order_id: str, organization_id: str, location_id: str | None, contact_id: str | None, requires_review: bool,
                  billing_location_id: str | None = None, billing_contact_id: str | None = None, validation: dict[str, Any] | None = None) -> dict[str, Any]:
        current = self.get_order(order_id)
        if not current: raise KeyError(order_id)
        candidate = {**current, "matched_organization_id": organization_id, "matched_location_id": location_id,
                     "matched_contact_id": contact_id, "matched_billing_location_id": billing_location_id or location_id,
                     "matched_billing_contact_id": billing_contact_id or contact_id, "match_requires_review": requires_review}
        decision = lifecycle_decision(candidate, candidate)
        with self.engine.begin() as connection:
            connection.execute(text("""
                UPDATE SalesOrders.Orders SET M1OrganizationID=:organization_id,M1LocationID=:location_id,
                  M1ContactID=:contact_id,M1BillingLocationID=:billing_location_id,M1BillingContactID=:billing_contact_id,
                  MatchValidationJson=:validation,AddressOverrideJson=NULL,MatchRequiresReview=:review,State=:state,
                  Severity=:severity,ActionTitle=:title,ActionDetail=:detail,BlocksCommit=:blocks,UpdatedAt=SYSUTCDATETIME()
                WHERE CONVERT(nvarchar(36),OrderID)=:order_id
            """), {"order_id": order_id, "organization_id": organization_id, "location_id": location_id, "contact_id": contact_id,
                     "billing_location_id": billing_location_id or location_id, "billing_contact_id": billing_contact_id or contact_id,
                     "validation": canonical_json(validation) if validation else None, "review": requires_review,
                     "state": decision.state, "severity": decision.severity, "title": decision.title,
                     "detail": decision.detail, "blocks": decision.blocks_commit})
        return self.get_order(order_id)  # type: ignore[return-value]

    def set_customer_resolution(self, order_id: str, resolution: dict[str, Any] | None) -> dict[str, Any]:
        with self.engine.begin() as connection:
            connection.execute(text("""
              UPDATE SalesOrders.Orders
              SET CustomerResolutionJson=:resolution,CustomerResolutionAt=CASE WHEN :resolution IS NULL THEN NULL ELSE SYSUTCDATETIME() END,
                  UpdatedAt=SYSUTCDATETIME()
              WHERE CONVERT(nvarchar(36),OrderID)=:order_id
            """), {"order_id": order_id, "resolution": canonical_json(resolution) if resolution else None})
        row = self.get_order(order_id)
        if not row:
            raise KeyError(order_id)
        return row

    def set_m1_preview(self, order_id: str, preview: dict[str, Any] | None, fingerprint: str | None) -> dict[str, Any]:
        with self.engine.begin() as connection:
            connection.execute(text("""
              UPDATE SalesOrders.Orders
              SET M1PreviewJson=:preview,M1PreviewFingerprint=:fingerprint,
                  M1PreviewAt=CASE WHEN :preview IS NULL THEN NULL ELSE SYSUTCDATETIME() END,
                  UpdatedAt=SYSUTCDATETIME()
              WHERE CONVERT(nvarchar(36),OrderID)=:order_id
            """), {"order_id": order_id, "preview": canonical_json(preview) if preview else None,
                   "fingerprint": fingerprint if preview else None})
        row = self.get_order(order_id)
        if not row:
            raise KeyError(order_id)
        return row

    def set_address_override(self, order_id: str, reason: str, actor: str) -> dict[str, Any]:
        current = self.get_order(order_id)
        if not current: raise KeyError(order_id)
        if (current.get("match_validation") or {}).get("status") != "address_mismatch":
            raise ValueError("Only a verified Shopify/M1 address mismatch can be overridden")
        detail = {"reason": reason, "actor": actor, "created_at": datetime.now(timezone.utc).isoformat()}
        candidate = {**current, "match_requires_review": False, "address_override": detail}
        decision = lifecycle_decision(candidate, candidate)
        with self.engine.begin() as connection:
            connection.execute(text("""UPDATE SalesOrders.Orders SET AddressOverrideJson=:detail,MatchRequiresReview=0,
              State=:state,Severity=:severity,ActionTitle=:title,ActionDetail=:action_detail,BlocksCommit=:blocks,UpdatedAt=SYSUTCDATETIME()
              WHERE CONVERT(nvarchar(36),OrderID)=:order_id"""), {"order_id": order_id, "detail": canonical_json(detail),
              "state": decision.state, "severity": decision.severity, "title": decision.title,
              "action_detail": decision.detail, "blocks": decision.blocks_commit})
        self.add_event(order_id, "address_mismatch_overridden", detail, actor)
        return self.get_order(order_id)  # type: ignore[return-value]

    def update_lines(self, order_id: str, lines: list[dict[str, Any]], actor: str) -> dict[str, Any]:
        with self.engine.begin() as connection:
            connection.execute(text("UPDATE SalesOrders.Orders SET LineOverridesJson=:lines,UpdatedAt=SYSUTCDATETIME() WHERE CONVERT(nvarchar(36),OrderID)=:order_id"), {"order_id": order_id, "lines": canonical_json(lines)})
        self.add_event(order_id, "order_lines_edited", {"lines": lines}, actor)
        return self.get_order(order_id)  # type: ignore[return-value]

    def mark_committed(self, order_id: str, erp_order_id: str) -> dict[str, Any]:
        with self.engine.begin() as connection:
            connection.execute(text("""
                UPDATE SalesOrders.Orders SET M1SalesOrderID=:erp_order_id,CommittedHash=SourceHash,
                  State='committed',Severity='success',ActionTitle='Added to M1',ActionDetail='No action required.',
                  BlocksCommit=1,CommittedAt=SYSUTCDATETIME(),UpdatedAt=SYSUTCDATETIME()
                WHERE CONVERT(nvarchar(36),OrderID)=:order_id
            """), {"order_id": order_id, "erp_order_id": erp_order_id})
        return self.get_order(order_id)  # type: ignore[return-value]

    def mark_existing_in_m1(self, order_id: str, erp_order_id: str) -> dict[str, Any]:
        with self.engine.begin() as connection:
            connection.execute(text("""
                UPDATE SalesOrders.Orders SET M1SalesOrderID=:erp_order_id,CommittedHash=SourceHash,
                  State='committed',Severity='success',ActionTitle='Already in M1',
                  ActionDetail='Matched by Shopify customer PO during reconciliation.',
                  BlocksCommit=1,CommittedAt=COALESCE(CommittedAt,SYSUTCDATETIME()),UpdatedAt=SYSUTCDATETIME()
                WHERE CONVERT(nvarchar(36),OrderID)=:order_id
            """), {"order_id": order_id, "erp_order_id": erp_order_id})
        self.add_event(order_id, "m1_order_reconciled", {"erp_order_id": erp_order_id})
        return self.get_order(order_id)  # type: ignore[return-value]

    def add_event(self, order_id: str | None, event_type: str, detail: dict[str, Any], actor: str = "system") -> None:
        with self.engine.begin() as connection:
            connection.execute(text("""
              INSERT SalesOrders.AuditEvents (EventID,OrderID,EventType,Actor,DetailJson,CreatedAt)
              VALUES (:id,:order_id,:event_type,:actor,:detail,SYSUTCDATETIME())
            """), {"id": str(uuid.uuid4()), "order_id": order_id, "event_type": event_type, "actor": actor, "detail": canonical_json(detail)})

    def get_settings(self) -> dict[str, Any]:
        defaults = MemoryStore().settings
        with self.engine.connect() as connection:
            rows = connection.execute(text("SELECT SettingKey,ValueJson FROM SalesOrders.Settings"))
            for row in rows:
                defaults[row.SettingKey] = json.loads(row.ValueJson)
        return defaults

    def save_settings(self, updates: dict[str, Any], actor: str) -> dict[str, Any]:
        allowed = set(MemoryStore().settings)
        with self.engine.begin() as connection:
            for key, value in updates.items():
                if key not in allowed:
                    continue
                result = connection.execute(text("UPDATE SalesOrders.Settings SET ValueJson=:value,UpdatedAt=SYSUTCDATETIME(),UpdatedBy=:actor WHERE SettingKey=:key"), {"key": key, "value": canonical_json(value), "actor": actor})
                if not result.rowcount:
                    connection.execute(text("INSERT SalesOrders.Settings (SettingKey,ValueJson,UpdatedAt,UpdatedBy) VALUES (:key,:value,SYSUTCDATETIME(),:actor)"), {"key": key, "value": canonical_json(value), "actor": actor})
        self.add_event(None, "settings_changed", {"keys": sorted(updates)}, actor)
        return self.get_settings()

    def dashboard(self) -> dict[str, Any]:
        with self.engine.connect() as connection:
            row = connection.execute(text("""
              SELECT COUNT(*) total,
                SUM(CASE WHEN State='ready' THEN 1 ELSE 0 END) ready,
                SUM(CASE WHEN State LIKE '%[_]review' THEN 1 ELSE 0 END) review,
                SUM(CASE WHEN State='committed' THEN 1 ELSE 0 END) committed,
                SUM(CASE WHEN State='cancelled_before_erp' THEN 1 ELSE 0 END) cancelled
              FROM SalesOrders.Orders
            """)).mappings().one()
            return {key: int(value or 0) for key, value in row.items()}

    def begin_sync(self) -> str:
        sync_id = str(uuid.uuid4())
        with self.engine.begin() as connection:
            connection.execute(text("INSERT SalesOrders.SyncRuns (SyncRunID,StartedAt,Status,OrdersSeen,OrdersChanged) VALUES (:id,SYSUTCDATETIME(),'running',0,0)"), {"id": sync_id})
        return sync_id

    def finish_sync(self, sync_id: str, *, seen: int, changed: int, error: str = "") -> None:
        with self.engine.begin() as connection:
            connection.execute(text("UPDATE SalesOrders.SyncRuns SET FinishedAt=SYSUTCDATETIME(),Status=:status,OrdersSeen=:seen,OrdersChanged=:changed,ErrorMessage=:error WHERE SyncRunID=:id"), {"id": sync_id, "status": "failed" if error else "succeeded", "seen": seen, "changed": changed, "error": error})

    def last_sync(self) -> dict[str, Any] | None:
        with self.engine.connect() as connection:
            row = connection.execute(text("SELECT TOP (1) * FROM SalesOrders.SyncRuns ORDER BY StartedAt DESC")).mappings().first()
            return dict(row) if row else None


_store: MemoryStore | SqlServerStore | None = None


def get_store() -> MemoryStore | SqlServerStore:
    global _store
    if _store is None:
        url = database_url()
        mode = setting("SALES_ORDER_STORAGE_MODE", "sql" if url else "memory")
        if mode == "sql":
            if not url:
                raise RuntimeError("SQL storage selected but no database connection is configured")
            _store = SqlServerStore(url)
        else:
            _store = MemoryStore()
    return _store


def set_store(store: MemoryStore | SqlServerStore | None) -> None:
    global _store
    _store = store
