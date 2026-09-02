# Browser test against the test ERP — findings

Run against `http://127.0.0.1:8010/sales-orders` with `M1_WRITES_ENABLED=true` pointing at the
test M1 dataset. 934 orders in the queue at the start.

**Sales orders created:** 112768 (S-1934), 112769 (S-1932), 112770 (S-1930), 112771 (S-1929),
112772 (S-1928).
**Customers created:** organizations `M15DE1`, `M15DE2`; contacts 3, 4 and 5 on `M1578L`.
**Order edited:** S-1928 line quantity 2 -> 4, plus an address override.

---

## Confirmed working

| Behaviour | Evidence |
|---|---|
| Delivery quantity total | `omlOrderQuantity` = `omlDeliveryQuantityTotal` = `omdDeliveryQuantity` = 4 after a line edit |
| NextID reservation | Five consecutive commits produced 112768-112772, no duplicate key |
| Organization ID allocation | `M15DDZ` -> `M15DE1` -> `M15DE2`, skipping `M15DE0` which already existed |
| New customer creation | No `cmoShipContactID ... not found` error; organization, both locations and both contacts created |
| Customer active date | Set on creation |
| Precomputed preview | Served in **4 ms** from SQL |
| Preview invalidation | Fingerprint changed on line edit; rebuilt preview showed the new quantity |
| Address mismatch guard | Blocked a mismatched customer; override required exact `USE M1 ADDRESSES` (lowercase -> 400) and a >=10 character reason (-> 422) |
| Cancelled-before-ERP reporting | Correct: no `erp_order_id`, removed from the creation queue |

---

## 1. FIXED: paged M1 reads silently dropped rows

**Severity: high.** Found and fixed during this test.

The M1 API paginates with `ORDER BY <orderBy> OFFSET n ROWS FETCH NEXT m ROWS ONLY`. When no
`orderBy` is supplied the repositories substitute the literal `"1"`
(`ERPOrganizationContactRepository.cs:50`), which SQL Server reads as an **ordinal reference to
the first column of the select list**:

| Table | Default sort column |
|---|---|
| OrganizationContacts | `cmcAlternatePhoneNumber` |
| OrganizationLocations | `cmlAddressLine1` |
| Organizations | `cmoAccountManagerEmployeeID` |
| SalesOrders | `ompApprovalDecisionDate` |

Each is a near-total tie, so the row order is free to differ between pages and OFFSET/FETCH
silently skips and duplicates rows. No error is raised.

**Observed:** `M1578L` had contacts 1-3 in M1. A live filtered read saw all of them
(`next_child_id` allocated 4), while `_Directory.load` — which uses `get_all` — reported only
contact 1. Across ten sampled organizations only two had any contacts at all.

**Downstream damage:** the customer directory drives matching. Because it believed organizations
had no contacts, the app kept recommending `contact_id: "NEW"` and creating duplicate contacts for
people who already existed — contacts 2, 3, 4 and 5 on `M1578L` are all the same person.
`shopify_sales_orders_by_po()` paged the same way, so the startup reconciliation could also
misclassify orders already in M1 as still needing creation. That did **not** cause duplicate sales
orders, because `preview()` re-checks with `find_sales_order_by_po` (an unpaged filtered read,
which is reliable) before every commit.

**Fix:** `M1Client.PAGED_ORDER_BY` supplies a primary-key sort for every paged read, and
`_page_order_by` raises for any resource without one — M1 silently ignores an unknown `orderBy`
field and reverts to `ORDER BY 1`, so failing loudly is the only safe default.

**Verified after the fix:** `M1578L` -> contacts `1@-, 2@100, 3@100, 4@100, 5@100`;
`M15DE1`/`M15DE2` -> `1@100, 2@200`.

### Follow-up required

Resolutions and previews computed before this fix are stale and were built on an incomplete
directory. `SyncService` only recomputes when `source_hash` changes or a resolution is missing, so
a full sync will **not** refresh them. Worth noting that the sync auto-applies a recommendation via
`set_match(..., "automatic_recommendation")`, so some existing matches were chosen from partial
data. Re-resolving S-1927 live after the fix changed its reason from "Matched by phone exact, name
exact" to "Closest available name match" — same organization, but the confidence was previously
overstated because rival candidates were missing from the directory.

There is currently no way to force a recompute. The preview fingerprint covers order inputs and
settings, not the matcher's own logic version.

---

## 2. Line edits destroy the original Shopify quantity

`web/app.py`, `edit_lines`: `"original_quantity": line.current_quantity`.

Editing a line overwrites `original_quantity` with the new value, so the record of what Shopify
actually sent is lost. After changing S-1928 from 2 to 4, the stored override read
`{q: 4, orig: 4}`. Anything comparing ordered against original — refund and partial-cancellation
review in particular — has nothing to compare against.

## 3. Line edits do not recompute tax

`build_m1_resource_plan` recomputes `subtotal` from the edited lines but passes `order["tax"]`
straight through to `ompOrderTaxAmountBase/Foreign`. Doubling a quantity leaves the original
Shopify tax on the M1 header, so the order total no longer agrees with its own lines. (S-1928 had
zero tax, so the discrepancy was not visible there, but the code path is unconditional.)

## 4. The preview leaks the raw `NEW` sentinel for billing

`CommitService.preview` masks the shipping side only:

```
"ompShipContactID": "<new contact>",
"ompArInvoiceContactID": "NEW",
```

`billing_location_id` and `billing_contact_id` are never substituted, and `customer_plan` reports
only `organization_id` / `location_id` / `contact_id`. A reviewer reading the preview cannot tell
what will happen to the billing contact.

## 5. Cancellation-after-shipment can never be reported

`domain.lifecycle_decision` branches on `erp_quantity_shipped`, but **nothing ever writes that
field** — it is read in `domain.py:127`, defaulted in `storage.py:74`, decoded in
`storage.py:296`, and has a SQL default of 0. All 934 orders have 0.

The `shipped_quantity == 0` branch is therefore always taken, and a Shopify cancellation on an
order that has already shipped in M1 will always be reported as *"Close the unshipped M1 order"*
(`cancellation_review`) instead of *"Cancellation after shipment — do not rewrite the shipped sales
order"* (`refund_review`). The three current `cancellation_review` orders are all genuinely
`UNFULFILLED`, so today's labels happen to be right; the guard is dead either way. Populating it
would mean reading `omlQuantityShipped` during reconciliation.

## 6. Refund guidance references an M1 order that may not exist

`refund_review` is reached before the `erp_order_id` check, so orders never sent to M1 still get
*"Historical practice uses a separate type-2 AR credit invoice with the original S- PO."* There is
no original S- PO for those. S-1904, S-1895, S-1804 and S-1567 are in this state, and they are also
blocked from ever being committed.

Refund capture itself looks correct. Shopify records refunds in pairs — one entry carrying
`total: 0` with the restocked lines, and a second carrying the money with no lines — and both are
preserved intact:

```
{ total: 0,     lines: [{ sku: "WN0070195", quantity: 1, subtotal: 12.09 }] }
{ total: 12.09, lines: [] }
```

Any future net-refund calculation must sum `total` and ignore line subtotals, or it will double
count.

## 7. `/api/orders` ships the entire raw Shopify payload

Each row includes `raw`, the full Shopify node. Listing all 934 orders returned over 400 KB and
overflowed a browser-side read twice during testing. `m1_preview` is already excluded from list
payloads; `raw` should be too.

## 8. Startup sync blocks the server, and stalls `--reload`

`lifespan` awaits a full reconciliation before yielding, so the app serves nothing until Shopify
and M1 have both been swept. Under `--reload` this happens on every code change. During this test
one reload sat in "Reloading..." for roughly ten minutes with the previous worker still answering
requests on stale code, which is confusing to debug — a route added to `web/app.py` kept returning
404 long after the file was saved. Setting `SALES_ORDER_STARTUP_SYNC=false` for local development
avoids it; the precomputed previews in SQL are served immediately either way.

## 9. Legacy M1 rows with blank IDs

`M1578L` has a location whose `cmlLocationID` is empty, with contact `1` attached to it. This is
pre-existing ERP data, not something the app created. The app tolerates it correctly —
`next_child_id` ignores non-numeric IDs and `validate_selection` cannot select a blank location —
but such rows do surface in the customer panel as blank entries.

## 10. Still unset on M1 records (previously identified, unchanged)

- `omdQuantityOnOrder` and `omdQuantityAllocated` on deliveries — calculated fields in the desktop
  DD, written verbatim by the API, so these orders do not appear in part availability or MRP.
- Line-level tax (`omlTaxAmountBase/Foreign`) is 0 while header tax is set directly.
- Organization-level default contacts cannot be set at all — M1 API defect, see
  `_write_customer_resources`.
- `cmoCustomerTaxable` defaults to false on every API-created customer; the desktop would set it
  from the `xapcmcustomertaxable` constant.
