import React from "react";

export function Badge({ children, tone = "neutral" }) {
  return <span className={`badge ${tone}`}>{children}</span>;
}

export function Button({ children, tone = "primary", className = "", ...props }) {
  return <button {...props} className={`${className} button ${tone}`}>{children}</button>;
}

export function Field({ label, hint, ...props }) {
  return <label className="field"><span>{label}</span><input {...props} />{hint && <small>{hint}</small>}</label>;
}

export function Toggle({ label, checked, onChange, detail }) {
  return <label className="toggle-row"><span><strong>{label}</strong>{detail && <small>{detail}</small>}</span><input type="checkbox" checked={checked} onChange={(event) => onChange(event.target.checked)} /></label>;
}

export function Alert({ tone = "info", title, children }) {
  return <div className={`alert ${tone}`}><strong>{title}</strong><p>{children}</p></div>;
}
