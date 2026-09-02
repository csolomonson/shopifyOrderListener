"""Repair SalesOrderLines.omlDeliveryQuantityTotal on orders already sent to M1.

M1 only maintains this running total inside the desktop DD engine, where
SalesOrderDeliveries.omdDeliveryQuantity is summed into the parent line. The
Public API writes the column verbatim, so lines created by this application
before that field was populated sit at 0 and M1 reports "Line order quantity
does not equal the sum of the deliveries".

Usage:  python tools/repair_delivery_quantity_total.py <SalesOrderID> [...]
        python tools/repair_delivery_quantity_total.py --dry-run <SalesOrderID>

Requires M1_WRITES_ENABLED unless --dry-run is passed.
"""

from __future__ import annotations

import sys
from decimal import Decimal
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
from integrations.m1 import M1Client


def delivered_totals(m1: M1Client, sales_order_id: str) -> dict[int, Decimal]:
    """Sum delivery quantities per line, mirroring M1's own bound-parent expression.

    Delivery type 3 is excluded from the total by the data dictionary.
    """
    totals: dict[int, Decimal] = {}
    for delivery in m1.get_all("SalesOrderDeliveries", [f"omdSalesOrderID[eq]{sales_order_id}"]):
        if int(delivery.get("omdDeliveryType") or 0) == 3:
            quantity = Decimal("0")
        else:
            quantity = Decimal(str(delivery.get("omdDeliveryQuantity") or 0))
        line_id = int(delivery["omdSalesOrderLineID"])
        totals[line_id] = totals.get(line_id, Decimal("0")) + quantity
    return totals


def repair(sales_order_id: str, *, dry_run: bool) -> int:
    m1 = M1Client()
    totals = delivered_totals(m1, sales_order_id)
    repaired = 0
    for line in m1.get_all("SalesOrderLines", [f"omlSalesOrderID[eq]{sales_order_id}"]):
        line_id = int(line["omlSalesOrderLineID"])
        current = Decimal(str(line.get("omlDeliveryQuantityTotal") or 0))
        expected = totals.get(line_id, Decimal("0"))
        ordered = Decimal(str(line.get("omlOrderQuantity") or 0))
        if current == expected:
            continue
        if expected != ordered:
            print(f"  line {line_id}: deliveries total {expected} but order quantity is {ordered}; skipping")
            continue
        print(f"  line {line_id}: {current} -> {expected}")
        repaired += 1
        if not dry_run:
            # PUT the row M1 returned so its row version and every other column
            # round-trip unchanged.
            m1.put("SalesOrderLines", {**line, "omlDeliveryQuantityTotal": str(expected)})
    return repaired


def main() -> None:
    arguments = sys.argv[1:]
    dry_run = "--dry-run" in arguments
    order_ids = [value for value in arguments if not value.startswith("-")]
    if not order_ids:
        raise SystemExit(__doc__)
    for sales_order_id in order_ids:
        print(f"SalesOrder {sales_order_id}")
        count = repair(sales_order_id, dry_run=dry_run)
        verb = "would update" if dry_run else "updated"
        print(f"  {verb} {count} line(s)")


if __name__ == "__main__":
    main()
