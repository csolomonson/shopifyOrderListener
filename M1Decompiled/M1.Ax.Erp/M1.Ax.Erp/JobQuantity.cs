using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class JobQuantity
{
	public decimal QtyReceivedToInventory;

	public decimal QtyToInspect;

	public decimal QtyShipped;

	public decimal QtyCompleted;

	public bool IsEqualToQtyCompleted;
}
