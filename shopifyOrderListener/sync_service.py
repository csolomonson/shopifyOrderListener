"""Idempotent outbound Shopify reconciliation."""

from __future__ import annotations

import logging
from datetime import datetime, timedelta, timezone
from typing import Any

from commit_service import CommitService
from customer_matching import CustomerMatcher
from domain import normalize_shopify_order
from integrations.m1 import M1Client
from integrations.shopify import ShopifyClient


logger = logging.getLogger(__name__)


class SyncService:
    def __init__(self, store: Any, shopify: ShopifyClient | None = None, m1: M1Client | None = None,
                 matcher: CustomerMatcher | None = None, commits: CommitService | None = None):
        self.store = store
        self.shopify = shopify or ShopifyClient()
        self.m1 = m1 or M1Client()
        self.matcher = matcher or CustomerMatcher(self.m1, store)
        self.commits = commits or CommitService(store, self.m1)

    def run(self, *, full: bool = False) -> dict[str, Any]:
        # Capture the previous run before recording this one. This is what lets
        # an incremental sync cover the entire interval while the app was down.
        last = self.store.last_sync()
        sync_id = self.store.begin_sync()
        seen = changed = already_in_m1 = not_in_m1 = resolutions_prepared = previews_prepared = 0
        resolution_order_ids: list[str] = []
        try:
            if not self.m1.configured:
                raise RuntimeError("M1 Public API must be configured before Shopify reconciliation")
            settings = self.store.get_settings()
            if full:
                nodes = self.shopify.all_orders()
                existing_m1_orders = self.m1.shopify_sales_orders_by_po()
            elif not last:
                since = datetime.now(timezone.utc) - timedelta(days=60)
                nodes = self.shopify.updated_orders(since)
                existing_m1_orders = None
            else:
                raw = last.get("started_at") or last.get("StartedAt")
                if isinstance(raw, str):
                    raw = datetime.fromisoformat(raw.replace("Z", "+00:00"))
                since = raw or datetime.now(timezone.utc) - timedelta(days=1)
                if since.tzinfo is None:
                    since = since.replace(tzinfo=timezone.utc)
                since -= timedelta(minutes=int(settings.get("sync_lookback_minutes", 10)))
                nodes = self.shopify.updated_orders(since)
                existing_m1_orders = None
            for node in nodes:
                seen += 1
                normalized = normalize_shopify_order(node)
                prior = self.store.get_order(normalized["shopify_order_id"])

                existing_m1 = (
                    existing_m1_orders.get(normalized["customer_po"])
                    if existing_m1_orders is not None
                    else self.m1.find_sales_order_by_po(normalized["customer_po"])
                )

                current = self.store.upsert_order(normalized)
                source_changed = not prior or prior.get("source_hash") != current.get("source_hash")

                if existing_m1:
                    already_in_m1 += 1
                    erp_order_id = str(
                        existing_m1.get("ompSalesOrderID")
                        or existing_m1.get("SalesOrderID")
                        or existing_m1.get("salesOrderId")
                        or ""
                    ).strip()
                    if not erp_order_id:
                        raise RuntimeError(f"M1 order for {normalized['customer_po']} did not include its sales-order ID")
                    if current.get("erp_order_id") != erp_order_id:
                        current = self.store.mark_existing_in_m1(current["order_id"], erp_order_id)
                        source_changed = True
                    historical_organization = str(existing_m1.get("ompCustomerOrganizationID") or "").strip()
                    if historical_organization and not current.get("matched_organization_id"):
                        shipping_location = str(existing_m1.get("ompShipLocationID") or "").strip()
                        shipping_contact = str(existing_m1.get("ompShipContactID") or "").strip()
                        billing_location = str(existing_m1.get("ompArInvoiceLocationID") or shipping_location).strip()
                        billing_contact = str(existing_m1.get("ompArInvoiceContactID") or shipping_contact).strip()
                        current = self.store.set_match(
                            current["order_id"], historical_organization, shipping_location, shipping_contact, False,
                            billing_location, billing_contact,
                            {"safe": True, "status": "historical_m1_order", "detail": f"Recovered from M1 sales order {erp_order_id}."},
                        )
                else:
                    not_in_m1 += 1

                if not current.get("matched_organization_id"):
                    prior_match = self.store.find_shopify_customer_match(normalized.get("shopify_customer_id"))
                    if prior_match and prior_match.get("organization_id"):
                        current = self.store.set_match(
                            current["order_id"], prior_match["organization_id"],
                            prior_match.get("location_id"), prior_match.get("contact_id"), False,
                            prior_match.get("billing_location_id"), prior_match.get("billing_contact_id"),
                        )
                if (
                    current.get("state") not in {"committed", "cancelled_before_erp"}
                    and (source_changed or not current.get("customer_resolution"))
                ):
                    resolution_order_ids.append(current["order_id"])
                if source_changed:
                    changed += 1

            # Run customer discovery after all orders have been reconciled so
            # a backlogged order can use history encountered later in the same
            # startup run. The M1 directory is cached by CustomerMatcher, so
            # this prepares the whole review queue without repeated downloads.
            for order_id in resolution_order_ids:
                current = self.store.get_order(order_id)
                if not current:
                    continue
                historical = None
                if current.get("matched_organization_id") not in {None, "", "__NEW__"}:
                    historical = {"organization_id": current["matched_organization_id"]}
                else:
                    historical = self.store.find_shopify_customer_match(current.get("shopify_customer_id"))
                resolution = self.matcher.resolution(current, historical)
                selection = resolution.get("selection") if resolution.get("status") == "recommended" else None
                validation_status = (current.get("match_validation") or {}).get("status")
                can_stage = not current.get("matched_organization_id") or validation_status in {None, "automatic_recommendation", "historical_m1_order"}
                if selection and can_stage:
                    current = self.store.set_match(
                        order_id,
                        selection["organization_id"], selection.get("location_id"), selection.get("contact_id"), False,
                        selection.get("billing_location_id"), selection.get("billing_contact_id"),
                        {
                            "safe": True,
                            "status": "automatic_recommendation",
                            "detail": resolution.get("why") or "Prepared automatically during Shopify synchronization.",
                        },
                    )
                self.store.set_customer_resolution(order_id, resolution)
                resolutions_prepared += 1
                # Build the commit preview here too. Both halves of opening an
                # order are then plain SQL reads, so the review queue stays
                # instant even when the M1 directory cache is cold.
                try:
                    self.commits.refresh_preview(order_id)
                    previews_prepared += 1
                except Exception:
                    # A preview that cannot be built now is rebuilt on demand;
                    # it must never abort reconciliation of the other orders.
                    logger.exception("Could not prepare the M1 commit preview for order %s", order_id)
            self.store.finish_sync(sync_id, seen=seen, changed=changed)
            return {
                "ok": True,
                "sync_id": sync_id,
                "orders_seen": seen,
                "orders_changed": changed,
                "orders_already_in_m1": already_in_m1,
                "orders_not_in_m1": not_in_m1,
                "customer_resolutions_prepared": resolutions_prepared,
                "m1_previews_prepared": previews_prepared,
                "full_reconciliation": full,
            }
        except Exception as exc:
            self.store.finish_sync(sync_id, seen=seen, changed=changed, error=str(exc))
            raise
