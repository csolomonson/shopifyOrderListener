export function money(value, currency = "USD") {
  return new Intl.NumberFormat("en-US", { style: "currency", currency }).format(Number(value || 0));
}

export function date(value) {
  if (!value) return "—";
  return new Intl.DateTimeFormat("en-US", { month: "short", day: "numeric", year: "numeric", hour: "numeric", minute: "2-digit" }).format(new Date(value));
}

export function statusLabel(value) {
  return String(value || "unknown").replaceAll("_", " ");
}

export function stateTone(row) {
  if (row?.state === "ready") return "blue";
  if (row?.severity === "danger") return "red";
  if (row?.severity === "warning") return "amber";
  if (row?.severity === "success") return "green";
  return "neutral";
}
