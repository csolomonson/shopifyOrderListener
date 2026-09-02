"""Outbound Shopify GraphQL synchronization client."""

from __future__ import annotations

from datetime import datetime, timezone
from typing import Any, Iterator

from app_config import setting


API_VERSION = setting("SHOPIFY_API_VERSION", "2026-07")

ORDER_QUERY = """
query UpdatedOrders($first: Int!, $after: String, $query: String!) {
  orders(first: $first, after: $after, query: $query, sortKey: UPDATED_AT) {
    pageInfo { hasNextPage endCursor }
    nodes {
      id legacyResourceId name createdAt updatedAt cancelledAt cancelReason
      cancellation { staffNote }
      displayFinancialStatus displayFulfillmentStatus currencyCode email phone note
      currentSubtotalPriceSet { shopMoney { amount currencyCode } }
      currentTotalDiscountsSet { shopMoney { amount currencyCode } }
      currentShippingPriceSet { shopMoney { amount currencyCode } }
      currentTotalTaxSet { shopMoney { amount currencyCode } }
      currentTotalPriceSet { shopMoney { amount currencyCode } }
      customer { id legacyResourceId displayName firstName lastName email phone }
      billingAddress { name firstName lastName company address1 address2 city province provinceCode zip countryCodeV2 phone }
      shippingAddress { name firstName lastName company address1 address2 city province provinceCode zip countryCodeV2 phone }
      shippingLine { title code }
      lineItems(first: 100) {
        nodes { id sku name title variantTitle quantity currentQuantity originalUnitPriceSet { shopMoney { amount currencyCode } } }
      }
      refunds {
        id legacyResourceId createdAt updatedAt note totalRefundedSet { shopMoney { amount currencyCode } }
        refundLineItems(first: 100) {
          nodes { quantity subtotalSet { shopMoney { amount currencyCode } } lineItem { id sku } }
        }
      }
    }
  }
}
"""


class ShopifyError(RuntimeError):
    pass


class ShopifyClient:
    def __init__(self, shop: str | None = None, client_id: str | None = None, client_secret: str | None = None):
        self.shop = (shop or setting("SHOPIFY_SHOP", "")).replace(".myshopify.com", "")
        self.client_id = client_id or setting("SHOPIFY_CLIENT_ID", "")
        self.client_secret = client_secret or setting("SHOPIFY_CLIENT_SECRET", "")
        self._token: str | None = None

    @property
    def configured(self) -> bool:
        return bool(self.shop and self.client_id and self.client_secret)

    def access_token(self) -> str:
        try:
            import requests
        except ModuleNotFoundError as exc:
            raise ShopifyError("requests is required; install requirements.txt") from exc
        if self._token:
            return self._token
        if not self.configured:
            raise ShopifyError("Shopify shop and client credentials are not configured")
        response = requests.post(
            f"https://{self.shop}.myshopify.com/admin/oauth/access_token",
            data={
                "grant_type": "client_credentials",
                "client_id": self.client_id,
                "client_secret": self.client_secret,
            },
            timeout=15,
        )
        response.raise_for_status()
        self._token = response.json()["access_token"]
        return self._token

    def graphql(self, query: str, variables: dict[str, Any]) -> dict[str, Any]:
        try:
            import requests
        except ModuleNotFoundError as exc:
            raise ShopifyError("requests is required; install requirements.txt") from exc
        response = requests.post(
            f"https://{self.shop}.myshopify.com/admin/api/{API_VERSION}/graphql.json",
            headers={"X-Shopify-Access-Token": self.access_token(), "Content-Type": "application/json"},
            json={"query": query, "variables": variables},
            timeout=30,
        )
        response.raise_for_status()
        payload = response.json()
        if payload.get("errors"):
            raise ShopifyError("; ".join(error.get("message", "Shopify GraphQL error") for error in payload["errors"]))
        return payload["data"]

    def _orders(self, search: str) -> Iterator[dict[str, Any]]:
        cursor = None
        while True:
            connection = self.graphql(
                ORDER_QUERY,
                {"first": 50, "after": cursor, "query": search},
            )["orders"]
            yield from connection["nodes"]
            if not connection["pageInfo"]["hasNextPage"]:
                break
            cursor = connection["pageInfo"]["endCursor"]

    def all_orders(self) -> Iterator[dict[str, Any]]:
        """Return every order visible to the app, including closed/cancelled orders.

        Shopify limits orders older than 60 days unless the app has the
        ``read_all_orders`` scope. The iterator still reconciles every order the
        installed app is permitted to read.
        """
        yield from self._orders("status:any")

    def updated_orders(self, since: datetime) -> Iterator[dict[str, Any]]:
        if since.tzinfo is None:
            since = since.replace(tzinfo=timezone.utc)
        timestamp = since.astimezone(timezone.utc).isoformat().replace("+00:00", "Z")
        yield from self._orders(f"status:any updated_at:>'{timestamp}'")

    def probe(self) -> dict[str, Any]:
        data = self.graphql("query { shop { name myshopifyDomain } }", {})
        return {"ok": True, **data["shop"], "api_version": API_VERSION}
