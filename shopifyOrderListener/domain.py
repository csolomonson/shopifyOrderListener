"""Pure order normalization and lifecycle policy."""

from __future__ import annotations

from dataclasses import dataclass
from decimal import Decimal
from typing import Any


REVIEW_STATES = {
    "customer_review",
    "change_review",
    "cancellation_review",
    "refund_review",
    "commit_failed",
}


def money(value: Any) -> Decimal:
    if isinstance(value, dict):
        value = value.get("shopMoney", {}).get("amount", "0")
    return Decimal(str(value or "0"))


def _address(value: dict[str, Any] | None) -> dict[str, str]:
    value = value or {}
    return {
        "name": value.get("name") or " ".join(
            part for part in (value.get("firstName"), value.get("lastName")) if part
        ),
        "company": value.get("company") or "",
        "address1": value.get("address1") or "",
        "address2": value.get("address2") or "",
        "city": value.get("city") or "",
        "province": value.get("provinceCode") or value.get("province") or "",
        "postal_code": value.get("zip") or "",
        "country": value.get("countryCodeV2") or value.get("countryCode") or "",
        "phone": value.get("phone") or "",
    }


def normalize_shopify_order(node: dict[str, Any]) -> dict[str, Any]:
    customer = node.get("customer") or {}
    cancellation = node.get("cancellation") or {}
    lines = []
    for index, item in enumerate((node.get("lineItems") or {}).get("nodes", []), start=1):
        quantity = int(item.get("quantity") or 0)
        current_quantity = int(item.get("currentQuantity", quantity))
        unit = money(item.get("originalUnitPriceSet"))
        lines.append(
            {
                "shopify_line_id": item.get("id"),
                "line_number": index,
                "sku": (item.get("sku") or "").strip(),
                "description": item.get("name") or item.get("title") or "",
                "variant": item.get("variantTitle") or "",
                "original_quantity": quantity,
                "current_quantity": current_quantity,
                "unit_price": unit,
                "line_total": unit * current_quantity,
            }
        )
    refunds = []
    for refund in node.get("refunds") or []:
        refunds.append(
            {
                "shopify_refund_id": refund.get("id"),
                "created_at": refund.get("createdAt"),
                "updated_at": refund.get("updatedAt"),
                "note": refund.get("note") or "",
                "total": money(refund.get("totalRefundedSet")),
                "lines": [
                    {
                        "shopify_line_id": (row.get("lineItem") or {}).get("id"),
                        "sku": (row.get("lineItem") or {}).get("sku") or "",
                        "quantity": row.get("quantity") or 0,
                        "subtotal": money(row.get("subtotalSet")),
                    }
                    for row in (refund.get("refundLineItems") or {}).get("nodes", [])
                ],
            }
        )
    return {
        "shopify_order_id": node["id"],
        "legacy_order_id": str(node.get("legacyResourceId") or ""),
        "order_name": node.get("name") or "",
        "customer_po": node.get("name") or "",
        "created_at": node.get("createdAt"),
        "updated_at": node.get("updatedAt"),
        "cancelled_at": node.get("cancelledAt"),
        "cancel_reason": node.get("cancelReason") or "",
        "cancellation_note": cancellation.get("staffNote") or "",
        "financial_status": node.get("displayFinancialStatus") or "",
        "fulfillment_status": node.get("displayFulfillmentStatus") or "",
        "email": node.get("email") or customer.get("email") or "",
        "phone": node.get("phone") or customer.get("phone") or "",
        "customer_name": customer.get("displayName") or "",
        "shopify_customer_id": customer.get("id"),
        "billing_address": _address(node.get("billingAddress")),
        "shipping_address": _address(node.get("shippingAddress")),
        "shipping_method": (node.get("shippingLine") or {}).get("title") or "",
        "subtotal": money(node.get("currentSubtotalPriceSet")),
        "discount": money(node.get("currentTotalDiscountsSet")),
        "shipping": money(node.get("currentShippingPriceSet")),
        "tax": money(node.get("currentTotalTaxSet")),
        "total": money(node.get("currentTotalPriceSet")),
        "currency": node.get("currencyCode") or "USD",
        "note": node.get("note") or "",
        "lines": lines,
        "refunds": refunds,
        "raw": node,
    }


@dataclass(frozen=True)
class LifecycleDecision:
    state: str
    severity: str
    title: str
    detail: str
    blocks_commit: bool


def lifecycle_decision(order: dict[str, Any], existing: dict[str, Any] | None) -> LifecycleDecision:
    existing = existing or {}
    erp_order_id = existing.get("erp_order_id")
    shipped_quantity = Decimal(str(existing.get("erp_quantity_shipped") or 0))
    changed_after_commit = bool(erp_order_id and existing.get("source_hash") != existing.get("committed_hash"))

    if order.get("cancelled_at"):
        if not erp_order_id:
            return LifecycleDecision(
                "cancelled_before_erp", "neutral", "Cancelled before ERP creation",
                "Keep the audit record and remove it from the creation queue.", True,
            )
        if shipped_quantity == 0:
            return LifecycleDecision(
                "cancellation_review", "danger", "Close the unshipped M1 order",
                "Historical practice closes the header, lines, and deliveries and marks the S- PO as CANCELLED.", True,
            )
        return LifecycleDecision(
            "refund_review", "danger", "Cancellation after shipment",
            "Do not rewrite the shipped sales order. Review a separate AR credit action.", True,
        )

    if order.get("refunds"):
        return LifecycleDecision(
            "refund_review", "danger", "Refund or partial cancellation",
            "Historical practice uses a separate type-2 AR credit invoice with the original S- PO.", True,
        )
    if changed_after_commit:
        return LifecycleDecision(
            "change_review", "warning", "Shopify changed after M1 creation",
            "Compare the committed snapshot before changing any ERP record.", True,
        )
    if existing.get("match_requires_review"):
        return LifecycleDecision(
            "customer_review", "warning", "Existing customer match needs review",
            "Confirm the organization and create only the missing contact/location.", True,
        )
    if erp_order_id:
        return LifecycleDecision("committed", "success", "Added to M1", "No action required.", True)
    return LifecycleDecision("ready", "success", "Ready for review", "Validate the match and order totals, then add it to M1.", False)
