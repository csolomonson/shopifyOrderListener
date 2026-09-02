#!/usr/bin/env bash
set -Eeuo pipefail

source_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
release_root="/opt/shopify-order-listener"
config_root="/etc/shopify-order-listener"
state_root="/var/lib/shopify-order-listener"
service_user="${SALES_ORDER_SERVICE_USER:-sales-order-listener}"
service_group="${SALES_ORDER_SERVICE_GROUP:-$service_user}"
version="${APP_VERSION:-$(date -u +%Y%m%d%H%M%S)}"
release="$release_root/releases/$version"

[[ "${EUID:-$(id -u)}" -eq 0 ]] || { echo "Run with sudo." >&2; exit 1; }
[[ -r "$source_root/requirements.txt" && -r "$source_root/package.json" ]] || { echo "Run from a complete release checkout." >&2; exit 1; }

if ! id "$service_user" >/dev/null 2>&1; then
    [[ "$service_user" == "sales-order-listener" ]] || {
        echo "Configured service user $service_user does not exist." >&2
        exit 1
    }
    useradd --system --user-group --home-dir "$state_root" --shell /usr/sbin/nologin "$service_user"
fi
getent group "$service_group" >/dev/null || { echo "Configured service group $service_group does not exist." >&2; exit 1; }
install -d -m 0755 -o root -g root "$release_root/releases"
install -d -m 0750 -o root -g "$service_group" "$config_root"
install -d -m 0750 -o "$service_user" -g "$service_group" "$state_root"
install -d -m 0750 -o root -g "$service_group" "$state_root/secrets"

install -d -m 0755 -o root -g root "$release"
install -d -m 0755 -o root -g root "$release/shopifyOrderListener"
tar --exclude=.venv --exclude=node_modules --exclude=__pycache__ --exclude=.pnpm-store \
    -C "$source_root" -cf - . | tar -C "$release/shopifyOrderListener" -xf -
python3 -m venv "$release_root/releases/$version/.venv"
"$release_root/releases/$version/.venv/bin/pip" install --requirement "$release/shopifyOrderListener/requirements.txt"

(cd "$release/shopifyOrderListener" && corepack pnpm install --frozen-lockfile && corepack pnpm run build)
rm -rf "$release/shopifyOrderListener/node_modules"

ln -sfn "$release_root/releases/$version" "$release_root/current.new"
mv -Tf "$release_root/current.new" "$release_root/current"
install -m 0644 "$release/shopifyOrderListener/deployment/ubuntu/shopify-order-listener.service" /etc/systemd/system/shopify-order-listener.service
install -m 0755 "$release/shopifyOrderListener/deployment/ubuntu/shopify-order-sync" /usr/local/bin/shopify-order-sync

override_dir="/etc/systemd/system/shopify-order-listener.service.d"
if [[ "$service_user" != "sales-order-listener" || "$service_group" != "sales-order-listener" ]]; then
    install -d -m 0755 -o root -g root "$override_dir"
    {
        printf '[Service]\n'
        printf 'User=%s\n' "$service_user"
        printf 'Group=%s\n' "$service_group"
    } > "$override_dir/runtime-identity.conf"
    chmod 0644 "$override_dir/runtime-identity.conf"
else
    rm -f "$override_dir/runtime-identity.conf"
fi

if [[ ! -r "$config_root/runtime.env" ]]; then
  echo "Missing $config_root/runtime.env; copy runtime.env.example, fill it in, and rerun." >&2
  exit 1
fi

systemctl daemon-reload
systemctl enable --now shopify-order-listener.service
echo "Installed release $version. Merge deployment/Caddyfile.same-host into the existing Caddy config and reload Caddy."
