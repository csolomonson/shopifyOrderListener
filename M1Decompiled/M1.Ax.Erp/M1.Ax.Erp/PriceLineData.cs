using System.Diagnostics;
using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
[DebuggerDisplay("{Sequence}: Qty = {Quantity}, Price = {UnitPrice}, Discount = {Discount}, Lead = {LeadTime}")]
public class PriceLineData
{
	public short Sequence;

	public decimal Quantity;

	public decimal UnitPrice;

	public decimal Discount;

	public short LeadTime;

	public PriceLineData(short sequence, decimal quantity, decimal unitPrice, decimal discount, short leadTime)
	{
		Sequence = sequence;
		Quantity = quantity;
		UnitPrice = unitPrice;
		Discount = discount;
		LeadTime = leadTime;
	}
}
