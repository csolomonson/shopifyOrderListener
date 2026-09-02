using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class ProjectTotals
{
	public decimal ProjectValue;

	public decimal InvoiceSubTotal;

	public decimal InvoiceTaxAmount;

	public decimal InvoiceTotal;

	public decimal FreightAmount;
}
