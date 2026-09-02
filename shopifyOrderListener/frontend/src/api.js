const bootstrap = window.SALES_ORDER_BOOTSTRAP || { basePath: "/sales-orders" };

export async function api(path, options = {}) {
  const response = await fetch(`${bootstrap.basePath}/api${path}`, {
    credentials: "same-origin",
    ...options,
    headers: { "Content-Type": "application/json", ...(options.headers || {}) },
  });
  let payload = null;
  try { payload = await response.json(); } catch { payload = {}; }
  if (!response.ok) throw new Error(payload.detail || `Request failed (${response.status})`);
  return payload;
}

export { bootstrap };
