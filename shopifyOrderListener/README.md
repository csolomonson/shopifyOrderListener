# Shopify Sales Order Queue

Shopify Sales Order Queue is an internal order-review application for staging
Shopify orders before they are created in ECI M1. It provides a FastAPI backend,
a React review interface, automated M1 customer matching, and an explicit
human-confirmed boundary for M1 writes.

Production data is stored in the `SalesOrders` schema of the `M1_ME` database.
Customer matching reads organizations, locations, and contacts through the
existing costing SQL connection when those tables are readable. All ERP writes
and all other ERP operations remain behind the M1 Public API.

## Key behavior

- Retrieves orders, cancellations, and refunds from the Shopify Admin GraphQL
  API without using Gmail.
- Reconciles Shopify customer purchase orders against existing M1 orders.
- Stages new or changed orders outside the operational M1 sales-order tables.
- Suggests an existing organization, contact, and location or proposes the
  creation of new customer records.
- Requires human confirmation before an order is submitted to M1.
- Supports incremental synchronization, full reconciliation, and an external
  scheduler command.
- Runs below `/sales-orders` when deployed behind the existing costing server.
- Restricts access to members of `sales-orders` and `administrators` in the
  shared application user directory.

## Project requirements

- Python 3 with the `venv` module
- Node.js with Corepack and pnpm
- Microsoft ODBC Driver 18 for SQL Server when SQL storage is enabled
- Network access to Shopify, SQL Server, and the M1 Public API

## Local installation

Run these commands from the application directory:

```powershell
py -3.13 -m venv .venv
.\.venv\Scripts\python.exe -m pip install -r requirements-dev.txt
corepack pnpm install --frozen-lockfile
corepack pnpm run build
Copy-Item dev-env.example.ps1 dev-env.ps1
```

Edit `dev-env.ps1` and configure the required Shopify, M1, and database values.
The file is excluded from version control and must not be committed.

Load the environment and start the development server:

```powershell
. .\dev-env.ps1
.\.venv\Scripts\python.exe -m uvicorn api:app --reload --host 127.0.0.1 --port 8010
```

Open `http://127.0.0.1:8010/sales-orders/`.

If SQL settings are omitted, the application uses an in-memory store intended
only for development and automated tests. Production installations must set
`SALES_ORDER_STORAGE_MODE=sql`.

## Synchronization command

Run one incremental synchronization:

```powershell
. .\dev-env.ps1
.\.venv\Scripts\python.exe -m sync_orders
```

Run a complete reconciliation of all Shopify orders visible to the application:

```powershell
.\.venv\Scripts\python.exe -m sync_orders --full
```

The command prints a JSON summary to stdout. It returns exit code `0` on success
and `1` on failure. The underlying function is also available to Python callers
as `sync_orders.sync(full=False)`.

## Verification

Run the backend tests:

```powershell
.\.venv\Scripts\python.exe -m pytest -q tests
```

Run the frontend tests and production build:

```powershell
corepack pnpm test
corepack pnpm run build
```

## Production installation

The supported production topology installs the application beside the Product
Cost Calculator and publishes it at:

```text
https://costing.meziere.net/sales-orders/
```

See [deployment/README.md](deployment/README.md) for prerequisites,
installation, database bootstrap, scheduler configuration, verification, and
service-management procedures. Review [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)
before enabling M1 writes.

## Security requirements

- Store credentials in protected files referenced by `*_FILE` environment
  variables.
- Do not commit `dev-env.ps1`, API keys, client secrets, or database passwords.
- Keep `M1_WRITES_ENABLED=false` until the target M1 environment is validated.
- Grant SQL write access only to `M1_ME.SalesOrders`. Optional read-only access
  to `dbo.Organizations`, `dbo.OrganizationLocations`, and
  `dbo.OrganizationContacts` accelerates customer matching.
- Treat Shopify content as untrusted input and review every staged order before
  submission to M1.
