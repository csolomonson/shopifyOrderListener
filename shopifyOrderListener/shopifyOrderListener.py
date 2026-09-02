"""Visual Studio-compatible launcher for the sales-order web application.

Gmail/IMAP ingestion has been retired. Production starts ``api:app`` through the
systemd unit; this module remains a convenient local project startup file.
"""

import os

import uvicorn


if __name__ == "__main__":
    os.environ.setdefault("SALES_ORDER_AUTH_REQUIRED", "false")
    os.environ.setdefault("SALES_ORDER_BACKGROUND_SYNC", "false")
    uvicorn.run("api:app", host="127.0.0.1", port=8010, reload=True)
