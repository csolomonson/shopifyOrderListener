using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class JobAssemblyTimeInfo
{
	public bool HasTime;

	public int Timecards;

	public int PurchaseOrders;

	public int RFQs;
}
