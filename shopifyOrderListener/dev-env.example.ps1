<#
    dev-env.example.ps1

    Local development configuration. Copy to dev-env.ps1, fill in the values, and
    DOT-SOURCE it before starting the server so the variables land in your shell:

        Copy-Item dev-env.example.ps1 dev-env.ps1
        . .\dev-env.ps1
        .\.venv\Scripts\python.exe -m uvicorn api:app --reload --port 8010

    Note the leading "." and space -- running .\dev-env.ps1 without it sets the
    variables in a child shell that exits immediately.

    There is no .env loading in this app: app_config.setting() reads os.getenv
    directly, so anything not set here is simply absent. Every NAME below also
    accepts NAME_FILE pointing at a file containing the value, which is how the
    systemd deployment keeps secrets out of the environment (see
    deployment/runtime.env.example).

    dev-env.ps1 is gitignored. Keep real secrets out of this example file.
#>

# --- Local development conveniences -----------------------------------------
# Without this, Basic auth is required (it defaults to true) and there is no
# built-in account, so every request 401s.
$env:SALES_ORDER_AUTH_REQUIRED = "false"

# Leave the background poller off while developing; run a sync by hand with
# POST /sales-orders/api/sync?full=true when you want one.
$env:SALES_ORDER_BACKGROUND_SYNC = "false"

# The startup sync blocks the server until Shopify and M1 have both been swept,
# on every --reload. Turn it on once the rest is configured and working.
$env:SALES_ORDER_STARTUP_SYNC = "false"

# --- Database ----------------------------------------------------------------
# WITHOUT THESE THE APP SILENTLY USES AN IN-MEMORY STORE. get_store() falls back
# to mode "memory" when no connection is configured, so you get an empty queue
# that disappears on restart, with no error. Setting STORAGE_MODE=sql makes a
# missing connection raise instead of degrading quietly.
$env:SALES_ORDER_STORAGE_MODE = "sql"
$env:SALES_ORDER_DB_SERVER    = "localhost\MEZIEREDB22"     # or "HOST,1433"
$env:SALES_ORDER_DB_USERNAME  = "cost_app_access"
$env:SALES_ORDER_DB_PASSWORD  = "abc123"
$env:SALES_ORDER_DB_DRIVER    = "ODBC Driver 18 for SQL Server"
# Driver 18 encrypts by default; "true" here is usually needed against a local
# instance using a self-signed certificate.
$env:SALES_ORDER_DB_TRUST_SERVER_CERTIFICATE = "true"
# The database name is hardcoded to M1_ME in app_config.database_url().
# To point somewhere else, set the full URL instead and leave the parts above unset:
# $env:SALES_ORDER_DATABASE_URL = "mssql+pyodbc:///?odbc_connect=..."

# --- Shopify -----------------------------------------------------------------
# Subdomain only. ShopifyClient strips ".myshopify.com" and re-appends it, so
# "my-shop" and "my-shop.myshopify.com" both work. All three must be set or
# `configured` is false and BOTH syncs are skipped without an error.
$env:SHOPIFY_SHOP          = "your-shop"
$env:SHOPIFY_CLIENT_ID     = "replace-with-client-id"
$env:SHOPIFY_CLIENT_SECRET = "replace-with-client-secret"
$env:SHOPIFY_API_VERSION   = "2026-07"

# --- M1 Public API -----------------------------------------------------------
# Same rule: all three or `configured` is false.
$env:M1_API_BASE_URL = "http://<m1 host>:1937"
$env:M1_API_ID       = "replace-with-api-id"
$env:M1_API_KEY      = "replace-with-api-key"

# Writes stay off until you mean it. Committing an order needs this true.
$env:M1_WRITES_ENABLED = "false"

# Defaults stamped onto new sales order deliveries.
$env:M1_DEFAULT_WAREHOUSE = "142"
$env:M1_DEFAULT_BIN       = "BIN1"
$env:M1_DEFAULT_UOM       = "EA"
# Blank is correct for the home currency; the app reads it from DatasetProperties.
# $env:M1_CURRENCY_RATE_ID = ""

# --- Optional ----------------------------------------------------------------
# Username attributed to actions while auth is disabled (default "developer").
# $env:SALES_ORDER_DEV_USERNAME = "cole"
# Only needed for the embedded Shopify iframe route (/sales-orders/shopify).
# $env:SALES_ORDER_SESSION_SECRET = "D94F96366C9D416897BF7130D15E48D6"
# With auth enabled, either a shared user directory...
# $env:SALES_ORDER_USERS_JSON_FILE = "C:\path\to\users.json"
# ...or a single account:
# $env:SALES_ORDER_USERNAME = "administrator"
# $env:SALES_ORDER_PASSWORD = "<app password>"

Write-Host "Sales order dev environment loaded." -ForegroundColor Green
Write-Host "  storage : $env:SALES_ORDER_STORAGE_MODE on $env:SALES_ORDER_DB_SERVER"
Write-Host "  shopify : $env:SHOPIFY_SHOP"
Write-Host "  m1      : $env:M1_API_BASE_URL (writes=$env:M1_WRITES_ENABLED)"

