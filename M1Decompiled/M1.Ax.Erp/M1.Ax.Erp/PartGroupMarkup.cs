using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class PartGroupMarkup
{
	public string PartGroupID;

	public decimal MaterialMarkup;

	public decimal SubcontractMarkup;

	public decimal LaborMarkup;

	public decimal OverheadMarkup;

	public decimal QuoteMarkup;

	public decimal PurchaseToOrderMarkup;

	public byte MarkupType;

	public byte MarkupOption;
}
