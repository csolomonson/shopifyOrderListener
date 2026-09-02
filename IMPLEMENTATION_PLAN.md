# Shopify Order Review Queue → M1

## Objective

Build an internal web application that receives Shopify order webhooks, stages every order outside M1, proposes the appropriate M1 organization/location/contact, and writes to M1 only after a human reviews and confirms the complete order.

The production application must use the M1 Public API for all runtime reads and writes. Direct SQL access is only for the one-time read-only analysis documented here and must not be included in application code.

## What the historical data says

Analysis used live M1 records where `SalesOrders.ompCustomerPO` starts with `S-`. At the time of analysis, M1 contained 857 such orders dated 2025-12-19 through 2026-08-18, covering 766 customer organizations. Six were still open.

Historical customer classification:

| Situation at order entry | Orders | Share |
| --- | ---: | ---: |
| No prior sales order in M1 | 592 | 69.08% |
| Existing M1 customer, first Shopify order | 174 | 20.30% |
| Returning Shopify customer | 91 | 10.62% |

Looking only at each organization's first Shopify order:

| Organization origin | Organizations | Share |
| --- | ---: | ---: |
| Created with/near the first Shopify order | 587 | 76.63% |
| Existing M1 customer with prior non-Shopify order | 174 | 22.72% |
| Pre-existing organization with no prior sales order | 5 | 0.65% |

This confirms that the ambiguous existing-customer case is common enough to be a first-class workflow, not an exception.

For the 174 existing-customer/first-Shopify orders:

- All 174 used the same organization for the customer and ship-to organization.
- 140 reused the base location; 34 used a named location.
- 17 shipping locations and 24 shipping contacts were created at about the time of the Shopify order.
- 172 had a shipping contact; only 2 did not.

For the 587 newly created organizations:

- 478 shipped to the base location and 109 used a named shipping location.
- 103 of those 109 named locations had an address different from the organization/base address.
- This is important because the notification email exposes the shipping address but not the billing address. Email alone cannot reproduce the historical base/billing and shipping split.

Dominant historical defaults include:

- Customer status `2`, group `CG01`, payment terms `PREPA` (Prepaid).
- Tax code `CAR` for most California customers and `OUTST` for most other states.
- `FDXGR` for FedEx Ground and `FEDHD` for FedEx Home Delivery.
- Shipping payment type `SHIP` in most orders.
- Warehouse `142`, unit `EA`, delivery type `2`, one firm delivery per product line.
- Sales-order status `3` (Approved).
- New base contacts normally use contact ID `1`; additional contacts use the next available ID.
- New alternate locations normally start at `100`, then increment within the organization.

Special line conventions also need to be preserved:

- 223 orders included zero-price part `FS01` when freight was zero (complimentary/free shipping).
- 24 orders included a zero-price `PROMO` line carrying the promotion description.
- 30 orders used non-zero line discount percentages/header discount totals.
- Cancelled/manual follow-up orders sometimes use `NOTE` or `HOLD`; those are not part of normal initial import and should remain a later workflow.

The attached S-1918 notification parses as one `WP805` line, quantity 1, merchandise subtotal $25.53, FedEx Home Delivery $23.59, total $49.12 USD, with no displayed tax. An exact check of email, name, normalized phone, and full address/postal code found no M1 organization/location/contact candidate, so it currently looks like a new customer. It is not yet present among the M1 Shopify orders examined. The notification has no billing address, so it is not safe to create the organization from email alone unless a reviewer explicitly confirms that billing equals shipping.

## Required architecture

### 1. Internal application

Use a Python web application that fits the existing repository:

- FastAPI service.
- SQLAlchemy plus Alembic migrations.
- Server-rendered Jinja/HTMX review UI; no separate JavaScript SPA is needed for the first version.
- SQLite in WAL mode for a single-server/small-team deployment, with automated backups. Use PostgreSQL instead if the application will run on multiple instances or needs high write concurrency.
- One background worker for mailbox polling, Shopify enrichment, M1 index refresh, and commit jobs.

The staging database is independent of M1. No queue, parser, candidate, or review state is stored in an M1 table.

### 2. Shopify webhook ingestion

Gmail is not required. Shopify webhooks are the production notification mechanism.

Ingestion behavior:

- Subscribe to `ORDERS_CREATE` as the initial queue trigger and to the relevant order lifecycle topics, including updated, edited, paid, cancelled, and refund events.
- Receive HTTPS webhook POSTs on a narrowly exposed endpoint. If the application otherwise remains internal, expose only this endpoint through a secure reverse proxy/tunnel, or use a supported event-bus destination such as Google Pub/Sub or AWS EventBridge.
- Capture the raw request body before JSON parsing and verify `X-Shopify-Hmac-SHA256` with a constant-time comparison.
- Reject an invalid HMAC and an unexpected shop domain/topic.
- Deduplicate individual deliveries by `X-Shopify-Webhook-Id`, correlate related deliveries by `X-Shopify-Event-Id`, and also enforce one staged order per shop + Shopify order ID.
- Persist the verified raw event, relevant headers, and a queued work item in one short database transaction, then return `200 OK` immediately. Perform Shopify/M1 lookups asynchronously.
- Reprocessing the same delivery or event must be harmless.
- Use event timestamps and the order's Shopify `updatedAt` value because webhook ordering is not guaranteed.
- Run a periodic GraphQL reconciliation query for orders updated since the last checkpoint so missed deliveries or downtime cannot silently lose an order.

### 3. Shopify authoritative order fetch

After a verified webhook is recorded, fetch the order's current state from the GraphQL Admin API. Treat the fetched order—not the webhook payload—as the authoritative staged snapshot. Request read-only order access.

Use Shopify data for:

- Stable order and customer IDs.
- Billing and shipping addresses.
- Customer/company/contact identity.
- Line SKU, quantity, unit price, discounts, tax lines, and currency.
- Shipping method and shipping charge.
- Payment state, cancellation/refund state, and order edits.

If the authoritative Shopify fetch fails, keep the webhook in a retryable Received state and do not construct an M1 proposal from an incomplete event payload.

### 4. M1 Public API adapter

Create one typed adapter around the M1 Public API. Runtime code must never connect to SQL Server.

Read resources:

- Organizations, OrganizationLocations, OrganizationContacts.
- SalesOrders (including lookup by Customer PO).
- PartRevisions/parts for SKU validation.
- ShippingMethods, ShippingPaymentTypes, PaymentTerms, TaxCodes, Warehouses.
- NextIDs only as part of a carefully serialized ID-allocation workflow.

Write resources:

- Organizations, OrganizationLocations, OrganizationContacts.
- SalesOrders, SalesOrderLines, SalesOrderDeliveries.

The decompiled ERP endpoints use independent `PUT` upserts keyed internally by GUID. Updates require the current row version. Therefore the adapter must:

- Assign and persist deterministic operation GUIDs before the first request.
- Read by GUID before retrying an ambiguous request.
- Preserve and use row versions for any update.
- Verify every response with a read-after-write comparison.
- Serialize NextID-dependent creates and retry unique-key collisions after refreshing the next ID.
- Never update the NextIDs row directly just to reserve an ID.
- Log response status and M1 identifiers without logging API keys or unnecessary customer data.

The decompiled EDI `POST /api/EDI/850/PostOrder` path creates the order header, lines, and deliveries inside one SQL transaction and asks M1 to allocate the sales-order ID. It is safer than independent ERP entity PUTs, but its request contract does not expose all Shopify freight/tax/payment fields. During the API contract-test phase, compare two approaches:

1. EDI atomic create followed by a verified header update for Shopify-specific totals.
2. ERP entity PUT workflow using a controlled incomplete/closed state until all children verify.

Do not enable production writes until a test environment proves that the selected approach cannot expose a shippable partial order. If neither standard endpoint can guarantee that property, add a small M1-side, vendor-supported transactional endpoint rather than accepting partial-order risk.

### 5. Local M1 customer index

Because the M1 API filter language supports simple comparisons rather than normalized/fuzzy searching, periodically synchronize a read-only customer search index into the staging database via the M1 Public API. Normalize locally:

- Email: trim and lowercase.
- Phone: digits plus country-code handling.
- Names/company: Unicode/case/punctuation normalization plus token form.
- Address: USPS-style normalization where practical; preserve original text.
- Postal code: country-aware normalization.

Refresh on startup, nightly, on demand from the review screen, and after the app creates or updates a CRM record.

## Staging data model

Suggested tables:

- `inbound_events`: shop, topic, webhook/event IDs, selected headers, verified raw JSON/blob reference, triggered/received time, API version, hash.
- `staged_orders`: Shopify IDs/name, state, currency, totals, source revision, reviewer, timestamps.
- `staged_order_lines`: SKU, description, quantity, source prices/discounts/taxes, proposed M1 part/revision.
- `staged_addresses`: billing/shipping address snapshots and normalized forms.
- `customer_snapshots`: source customer/company/contact details.
- `m1_organizations_cache`, `m1_locations_cache`, `m1_contacts_cache`: read-only synchronized search data.
- `match_candidates`: candidate organization/location/contact, per-signal evidence, score, conflicts, cache timestamp.
- `identity_links`: human-approved Shopify customer/email identity to M1 organization/contact/location links.
- `review_flags`: typed warnings with severity, resolution, and reviewer note.
- `commit_runs`: immutable approved snapshot hash, overall state, reviewer, start/end times, resulting M1 sales-order ID.
- `commit_operations`: ordered API actions, deterministic GUID, request hash, attempts, response status, verified result.
- `audit_events`: parsing, edits, match choices, approval, retries, success/failure.

Important uniqueness constraints:

- One staged order per Shopify store + Shopify order ID.
- One inbound event per shop + Shopify webhook ID.
- One successful commit per staged-order revision.
- One M1 Customer PO per Shopify order name, verified again immediately before write.

## Shopify mapping and validation rules

The current email prototype assumes every line occupies exactly eight text lines and assumes the presence of a variant/color row. S-1918 disproves that assumption. Remove email parsing from the production path and replace it with:

- Versioned GraphQL-to-staging mapping from Shopify's structured order model.
- Versioned webhook envelopes so an API-version change can be tested and replayed.
- Treat all Shopify strings, notes, and descriptions as untrusted data; never execute embedded instructions.
- Validate every SKU, positive quantity, decimal/currency format, and recognized shipping method.
- Reconcile merchandise subtotal, discounts, shipping, taxes, and grand total to the cent.
- Flag missing/duplicate SKUs, unknown parts, price differences, address gaps, non-USD currency, cancellation/refund, edits, multiple fulfillments, and any reconciliation failure.
- Allow a stored raw webhook to be replayed without overwriting a reviewed revision.

## Customer matching policy

The app proposes; the employee decides. No customer match writes to M1 without the final human Add action.

### Candidate generation

Generate candidates from all three M1 levels (organization, location, contact) using:

1. Existing approved Shopify customer identity link.
2. Exact normalized contact or organization email.
3. Exact normalized phone.
4. Exact normalized street + postal code.
5. Company name + postal code/address.
6. Personal name + postal code/address.
7. Fuzzy company/name/address similarity for suggestions only.

Never merge solely on a common personal name, shared domain such as Gmail, or phone/address fragments.

### Confidence/review classes

- **Green — known Shopify mapping:** A prior human-approved identity link still agrees with M1. Show the previous orders and current address differences.
- **Yellow — new customer:** No credible candidate. Propose a new organization/contact/location, but require billing-versus-shipping confirmation.
- **Red — existing M1 candidate, first Shopify order:** Any credible pre-existing M1 match with no approved Shopify mapping. Always require explicit selection and show why each candidate matched.
- **Red — conflict:** Strong signals point to different organizations, multiple organizations share an exact email/phone, inactive/credit-hold records are involved, or the order identity changed.

### Location/contact behavior after organization selection

- Reuse a location only when the normalized full address agrees.
- Otherwise propose the next unused location ID, normally beginning at `100`.
- Reuse a contact only when the exact email agrees and the name/phone do not conflict.
- Otherwise propose the next unused contact ID at the chosen location, normally beginning at `1`.
- For a new organization, create the base location (`""`) and contact `1`. Create location `100` only when the confirmed shipping address differs from the billing/base address.
- Store every human-approved identity link so the next Shopify order is deterministic.

## Review screen

The primary order screen should show, side by side:

- Shopify source: order ID/date/status, customer, billing and shipping addresses, lines, discounts, freight, tax, total, and a link to Shopify.
- Proposed M1 result: organization/location/contact, header defaults, mapped parts, delivery records, tax/freight representation, and final total.
- Candidate evidence: exact/fuzzy signals, prior Shopify orders, prior non-Shopify orders, existing contacts/locations, created dates, inactive/credit flags.
- Review flags with required resolution.
- Diff highlighting for every field that will differ from the source or existing M1 record.

Actions:

- Refresh from Shopify/M1.
- Select an existing organization/location/contact.
- Propose new CRM records without writing them yet.
- Edit staged values with an audit note.
- Mark ignored/cancelled.
- `Add to M1` with a confirmation summary.
- Retry/resume a failed commit without creating duplicates.

The button is enabled only when all blocking flags are resolved, totals reconcile, every SKU maps, a customer decision is explicit, and the approved snapshot has not changed since review.

## Commit workflow

1. Lock the staged order and calculate an immutable snapshot hash.
2. Refresh Shopify status; stop on edit, cancellation, refund, or payment problem.
3. Query M1 by Customer PO `S-####`; stop if an order already exists.
4. Refresh the selected organization/location/contact and re-run conflict checks.
5. Create missing organization/location/contact records, verifying each result.
6. Create the complete sales order with lines and deliveries using the validated transactional strategy.
7. Read the complete M1 order back and compare organization IDs, lines, quantities, prices, freight, tax, total, ship method, contacts, and locations.
8. Mark the queue item Created only after exact verification; otherwise mark Failed/Needs Intervention and preserve the resumable operation ledger.
9. Never automatically delete or overwrite a partially created M1 record. Surface it prominently for controlled recovery.

## Security requirements before implementation testing

The current prototype contains a Gmail credential and an M1 API credential in source, and Shopify/M1 secrets were also pasted into the task conversation. Treat every exposed secret as compromised:

- Rotate/revoke both credentials before any further use.
- Remove secrets from source and git history as appropriate.
- Use environment variables backed by Windows Credential Manager, DPAPI, or a deployment secret store.
- Create separate least-privilege M1 keys for development/test and production.
- Use HTTPS for M1 API traffic; do not transmit API credentials over plaintext LAN HTTP.
- Authenticate the internal UI and restrict M1 commit permission by role.
- Avoid customer PII in normal application logs and error telemetry.

## Delivery phases

### Phase 0 — security and fixtures

- Rotate leaked credentials and add secret-loading configuration.
- Commit sanitized Shopify GraphQL/webhook fixtures corresponding to S-1918 and synthetic variants.
- Establish linting, tests, migrations, and configuration profiles.

### Phase 1 — ingestion and staging

- Signed Shopify webhook ingestion, deduplication, raw-event retention, and asynchronous processing.
- Shopify read-only GraphQL fetch and downtime reconciliation.
- Queue persistence and reconciliation validation.
- No M1 writes.

Acceptance: repeated deliveries/restarts create no duplicates; invalid signatures are rejected; webhook processing acknowledges within Shopify's timeout; fixture suite covers create/update/edit/paid/cancel/refund events, discounts, free shipping, tax, business/residential formats, international addresses, and malformed payloads.

### Phase 2 — M1 reads and matching

- Typed M1 API client and lookup-cache synchronization.
- Part/shipping/tax/default mapping.
- Candidate generation, scoring evidence, prior-order history, identity links.
- Still no M1 writes.

Acceptance: replay a representative historical sample and measure whether the previously chosen organization is among the candidates. Target 100% recall for exact prior Shopify mappings and high recall for the 174 existing-customer first-Shopify cases; false auto-matches are unacceptable.

### Phase 3 — review UI

- Queue filters, order detail/diff, candidate selection, new CRM proposal, flags, audited edits, approval snapshot.
- Read-only shadow operation alongside the employee's current process.

Acceptance: the employee can reproduce a historical entry decision and all discrepancies are visible before commit.

### Phase 4 — M1 write contract tests

- Use a non-production company/database or clearly isolated test customer/order range.
- Validate payload defaults, ID allocation, row-version behavior, ambiguous timeouts, retries, tax/freight totals, and the atomic/incomplete-order strategy.
- Confirm what shipping can see at every intermediate step.

Acceptance: fault injection after every API call cannot create a duplicate and cannot leave a shippable incomplete order.

### Phase 5 — controlled pilot

- Enable production commits for one authorized employee.
- Require review for every order.
- Start with known returning Shopify customers, then new customers, and enable ambiguous existing-customer cases last.
- Compare every created M1 order to Shopify and the old manual process during the pilot.

## Credentials/access needed after planning

Before implementation can exercise integrations, provide:

- A rotated Shopify client secret and read-only Shopify Admin order access.
- A non-production M1 Public API key with the same resource permissions planned for production.
- Later, a least-privilege production M1 key only after contract tests pass.
- Confirmation of the preferred host, backup location, and internal users/roles.

No production M1 write key is needed for Phases 0–3.
