# Ubuntu Deployment Guide

This guide installs Shopify Sales Order Queue on the Ubuntu host that already
runs the Product Cost Calculator. The resulting application is available at
`https://costing.meziere.net/sales-orders/` and uses the existing authentication,
SQL connection, Caddy instance, hostname, certificate, and private certificate
authority.

## Deployment architecture

| Component | Configuration |
| --- | --- |
| Reverse proxy and TLS | Existing Caddy service |
| Product Cost Calculator | `127.0.0.1:8000` |
| Shopify Sales Order Queue | `127.0.0.1:8010` |
| Public application path | `/sales-orders*` |
| Runtime service account | `cost-calculator` |
| Application database | `M1_ME.SalesOrders` |
| User directory | `/var/lib/cost-calculator/users/app_users.json` |
| Application configuration | `/etc/shopify-order-listener/runtime.env` |
| Installed release | `/opt/shopify-order-listener/current` |

Caddy remains the only process listening for HTTPS traffic. The installer adds
the `/sales-orders*` route ahead of the existing costing-application route and
does not replace Caddy's certificate or CA state.

## Prerequisites

Confirm the following before installation:

- The Product Cost Calculator is installed and operational on the target host.
- Caddy serves the costing application with its existing internal certificate.
- The `cost-calculator` Linux account and group exist.
- The following costing-application files are present and readable:
  - `/etc/cost-calculator/runtime.env`
  - `/etc/cost-calculator/caddy.env`
  - `/var/lib/cost-calculator/users/app_users.json`
- Python 3, `venv`, Git, Caddy, Node.js, Corepack, pnpm, unixODBC, and Microsoft
  ODBC Driver 18 for SQL Server are installed.
- The host can make outbound HTTPS connections to Shopify.
- The host can reach SQL Server and the M1 Public API.
- The operator has `sudo` access.

Prepare these application credentials:

- Shopify shop subdomain
- Shopify client ID and client secret
- M1 Public API base URL, API ID, and API key

The Shopify application requires `read_orders` and `read_all_orders`.
`read_all_orders` is required to reconcile orders older than Shopify's standard
order-history window.

## Installation

### 1. Clone the repository

```bash
git clone https://github.com/csolomonson/shopifyOrderListener.git
cd shopifyOrderListener/shopifyOrderListener
```

### 2. Run the installer in preview-only mode

```bash
sudo bash deployment/ubuntu/deploy-on-costing-vm.sh
```

The installer prompts for Shopify and M1 credentials without displaying secret
values. Existing values are retained when the installer is run again and the
secret prompt is left blank.

The installer performs the following operations:

1. Reads the SQL connection from `/etc/cost-calculator/runtime.env`.
2. Writes protected application configuration and secret files.
3. Creates a versioned release and Python virtual environment.
4. Installs Python and frontend dependencies and builds the React application.
5. Installs and starts `shopify-order-listener.service` on `127.0.0.1:8010`.
6. Adds a persistent `/sales-orders*` route to the existing Caddy configuration.
7. Validates SQL Server, Shopify, and M1 connectivity.
8. Runs the startup reconciliation and verifies the HTTPS application path.

M1 writes remain disabled unless `--enable-m1-writes` is supplied.

### 3. Complete the database bootstrap if required

The production runtime login is expected to be unable to create schemas or
tables. If the `SalesOrders` schema has not been installed, the deployment exits
with status `2` and creates this DBA script:

```text
/var/lib/shopify-order-listener/database/bootstrap-sales-orders.sql
```

A database administrator must execute the complete script against `M1_ME`. The
script creates the application schema, applies all migrations, and grants the
runtime principal the required permissions on that schema. It does not grant
direct access to operational M1 ERP tables.

After the script completes, run the installer again:

```bash
sudo bash deployment/ubuntu/deploy-on-costing-vm.sh
```

### 4. Verify the installation

Check the service and recent logs:

```bash
sudo systemctl status shopify-order-listener.service
sudo journalctl -u shopify-order-listener.service -n 100 --no-pager
```

Check the local application endpoint:

```bash
curl --fail http://127.0.0.1:8010/sales-orders/api/health
```

Open the application from a domain-managed workstation:

```text
https://costing.meziere.net/sales-orders/
```

Authentication is shared with the Product Cost Calculator. A user added,
removed, or updated in the costing application's user directory receives the
same change in the sales-order application.

## Enabling and disabling M1 writes

Enable writes only after preview output has been validated against a test M1
environment:

```bash
sudo bash deployment/ubuntu/deploy-on-costing-vm.sh --enable-m1-writes
```

Disable writes and return the application to preview-only mode:

```bash
sudo bash deployment/ubuntu/deploy-on-costing-vm.sh --disable-m1-writes
```

Running the installer without either flag preserves the current M1 write
setting.

## Synchronization modes

Only one automatic synchronization mode should be enabled at a time.

### Built-in synchronization

The default deployment uses the FastAPI process for synchronization:

- `SALES_ORDER_STARTUP_SYNC=true` performs a full reconciliation before the web
  application begins serving requests.
- `SALES_ORDER_BACKGROUND_SYNC=true` starts the incremental polling loop.
- The default polling interval is 60 seconds.
- Incremental runs include a 10-minute overlap to prevent gaps.

This mode requires no cron entry or systemd timer.

### External synchronization command

The installer publishes the scheduler-safe command at:

```text
/usr/local/bin/shopify-order-sync
```

Run an incremental synchronization:

```bash
sudo -u cost-calculator /usr/local/bin/shopify-order-sync
```

Run a full reconciliation:

```bash
sudo -u cost-calculator /usr/local/bin/shopify-order-sync --full
```

The command loads `/etc/shopify-order-listener/runtime.env`, prints a JSON
summary, and returns a nonzero exit status on failure. A non-blocking file lock
causes overlapping scheduled invocations to exit successfully without starting
a second reconciliation.

Before configuring cron or a systemd timer, edit
`/etc/shopify-order-listener/runtime.env`:

```text
SALES_ORDER_STARTUP_SYNC=false
SALES_ORDER_BACKGROUND_SYNC=false
```

Apply the change:

```bash
sudo systemctl restart shopify-order-listener.service
```

The web interface and its manual **Sync now** action remain available.

### Cron configuration

Edit the runtime account's crontab:

```bash
sudo crontab -u cost-calculator -e
```

Add an incremental synchronization every minute:

```cron
* * * * * /usr/local/bin/shopify-order-sync >/dev/null
```

Successful JSON output is discarded. Failures remain on stderr and are handled
according to the host's configured cron mail policy.

### systemd timer configuration

Create `/etc/systemd/system/shopify-order-sync.service`:

```ini
[Unit]
Description=Synchronize Shopify sales orders
Wants=network-online.target
After=network-online.target

[Service]
Type=oneshot
User=cost-calculator
Group=cost-calculator
ExecStart=/usr/local/bin/shopify-order-sync
```

Create `/etc/systemd/system/shopify-order-sync.timer`:

```ini
[Unit]
Description=Run Shopify sales-order synchronization every minute

[Timer]
OnBootSec=1min
OnUnitInactiveSec=1min
Unit=shopify-order-sync.service

[Install]
WantedBy=timers.target
```

Enable the timer:

```bash
sudo systemctl daemon-reload
sudo systemctl enable --now shopify-order-sync.timer
sudo systemctl list-timers shopify-order-sync.timer
```

View synchronization logs:

```bash
sudo journalctl -u shopify-order-sync.service -n 100 --no-pager
```

Do not configure both cron and the systemd timer.

## Service operations

```bash
sudo systemctl start shopify-order-listener.service
sudo systemctl stop shopify-order-listener.service
sudo systemctl restart shopify-order-listener.service
sudo systemctl status shopify-order-listener.service
sudo journalctl -u shopify-order-listener.service -f
```

The service is configured with `Restart=on-failure` and runs one Uvicorn worker.
A single worker prevents duplicate in-process polling loops.

## Upgrades

Pull the required revision and rerun the installer from the application
directory:

```bash
git pull --ff-only
sudo bash deployment/ubuntu/deploy-on-costing-vm.sh
```

The installer creates a new versioned release and atomically updates the
`/opt/shopify-order-listener/current` symlink. Stored credentials, database data,
and the current M1 write setting are retained.

If cron or a systemd timer is used, verify that both built-in synchronization
variables remain disabled after an upgrade.

## Certificate and webhook constraints

The application reuses Caddy's internal certificate. Domain-managed clients
trust that certificate through the organization's certificate policy. Shopify's
servers do not share that trust and therefore cannot deliver HTTPS webhooks
directly to this endpoint.

The supported ingestion method is outbound Shopify Admin GraphQL polling. It
requires no public inbound firewall rule, public certificate, or Gmail mailbox.
Webhook delivery requires a separate publicly trusted ingress or supported cloud
event relay.

## Troubleshooting

| Symptom | Diagnostic action |
| --- | --- |
| Service does not start | Run `journalctl -u shopify-order-listener.service -n 100 --no-pager`. |
| Local health check fails | Confirm that port 8010 is not in use and inspect the service logs. |
| Public path returns a Caddy error | Run `caddy validate --config /etc/caddy/Caddyfile` and inspect the Caddy service. |
| Browser reports an invalid certificate | Confirm that the workstation trusts the existing Caddy internal CA. |
| Database schema is missing | Execute the generated bootstrap script against `M1_ME` and rerun the installer. |
| Shopify synchronization fails | Run `/usr/local/bin/shopify-order-sync` as `cost-calculator` and inspect stderr. |
| M1 submission is unavailable | Confirm `M1_WRITES_ENABLED`, API connectivity, and the selected M1 target database. |

## Installed files

| Path | Purpose |
| --- | --- |
| `/opt/shopify-order-listener/releases/` | Versioned application releases |
| `/opt/shopify-order-listener/current` | Active release symlink |
| `/etc/shopify-order-listener/runtime.env` | Runtime configuration |
| `/var/lib/shopify-order-listener/secrets/` | Shopify, M1, and session secrets |
| `/var/lib/shopify-order-listener/database/` | Generated database bootstrap artifacts |
| `/usr/local/bin/shopify-order-sync` | External synchronization command |
| `/usr/local/lib/shopify-order-listener/ensure-caddy-route.py` | Persistent Caddy route guard |
| `/etc/systemd/system/shopify-order-listener.service` | Web application service |
