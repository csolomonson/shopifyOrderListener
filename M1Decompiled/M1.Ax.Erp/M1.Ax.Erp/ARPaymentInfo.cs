using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class ARPaymentInfo
{
	public decimal AmountClearedBase;

	public decimal AmountClearedForeign;

	public decimal ExchangeAmount;
}
