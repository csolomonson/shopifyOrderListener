# Architecture and safety boundary

```text
Shopify Admin GraphQL (outbound poll)
                 |
                 v
       M1_ME.SalesOrders schema
       snapshots, matches, audit
                 |              M1 customer tables
                 |              read-only lookup
          human review/confirm
                 |
                 v
        M1 Public API writes only
```

The app writes only to its `SalesOrders` schema through SQL. Customer-directory
matching may directly `SELECT` the narrow organization, location, and contact
fields through the Product Cost Calculator's existing connection. The directory
snapshot is cached for five minutes, and the Public API is the automatic fallback
if those optional table reads are unavailable. Duplicate-order checks, ID
allocation, preflight validation outside the customer directory, and every ERP
mutation use the M1 Public API.

## Customer matching

Candidates are explainable and ranked. Organization, location, and contact data
is read from SQL when available, with a paged M1 Public API fallback, and compared
independently by company, customer or contact name, email, and phone. Fuzzy
name/company matches are labeled as such; a match on only one field remains
visible instead of disappearing behind a single strict search predicate.

Opening an order produces one operational recommendation instead of exposing the
underlying M1 tables. The app first reuses the exact organization recovered from
the same Shopify customer's prior `S-` order. It then decides whether the current
address/contact can be reused or should be added. Without that history, exact
email, phone, business, or address evidence can recommend an existing customer;
a personal name alone never authorizes a merge. If no operational evidence exists,
the primary action is to create a new customer. Loose searching and direct M1 IDs
remain behind the reviewer’s “this customer is wrong” correction flow.

The selected organization is not enough to permit a commit. Shipping and
billing locations are validated independently against their Shopify addresses,
and the selected shipping and accounting contacts must belong to the relevant
locations. A mismatch blocks the M1 preview and write. The only bypass requires
an audited reason plus the exact `USE M1 ADDRESSES` confirmation phrase, and the
addresses are checked again immediately before commit.

New customers use the historical defaults: base location `100`, contact `1`,
customer group `CG01`, prepaid terms, warehouse `142`, and `EA`. When Shopify
billing and shipping addresses differ, the accounting location/contact uses
`100`/`1` and a separate shipping location/contact uses `200`/`2`. For an
existing organization, newly allocated contacts are always linked to their
selected billing or shipping location. M1 resource creation remains behind the
same human and feature gates as sales-order creation.

## Cancellations and refunds

Historical M1 data shows two distinct treatments:

- Unshipped cancellations close the M1 order header, lines, and deliveries, and
  mark the Shopify customer PO as `CANCELLED`. They remain auditable rather than
  being deleted.
- Refunds after shipment are separate type-2 AR credit invoices. Their lines
  usually have negative quantities, carry the original `S-` customer PO, and are
  not linked back by changing the original sales order.

Accordingly, a cancellation before ERP creation simply leaves the creation queue.
Once an ERP order exists, cancellations, partial cancellations, order edits, and
refunds are blocking review events. Version 1 does not automatically issue credit
invoices; it presents the historical action and keeps the original sales order
unchanged until a reviewer authorizes a dedicated credit workflow.

## Idempotency and failure recovery

Shopify order IDs are unique in staging, polling overlaps its previous cursor, and
the normalized payload hash prevents duplicate work. Startup performs a full
reconciliation of every Shopify order visible to the app, and incremental polling
resumes from the previous sync time after any downtime. Both reconciliation and
commit check M1 for the exact `S-` customer PO. Orders found in M1 are marked as
reconciled and cannot enter the creation queue. Each attempt is audited. Since M1's generic ERP
resources do not provide a cross-resource transaction, any partial API failure is
treated as a reconciliation incident rather than retried blindly.
