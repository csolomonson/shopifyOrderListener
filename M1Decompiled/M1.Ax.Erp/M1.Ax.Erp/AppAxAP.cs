using System;
using System.Data;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("AP")]
[ComVisible(true)]
public class AppAxAP
{
	private IServiceProvider provider;

	private M1Database _Database;

	public bool DisableTaxFields => _Database.Props("FN").Field<bool>("xafAPDisableTaxFields");

	public AppAxAP(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public int GetPaymentHeaderCount(object sessionID)
	{
		return new AP().GetPaymentHeaderCount(_Database, Convert.ToInt32(sessionID));
	}

	public int GetRecurringPaymentHeaderCount(object sessionID)
	{
		return new AP().GetRecurringPaymentHeaderCount(_Database, Convert.ToInt32(sessionID));
	}

	public bool ExportSepaNLCreditTransfer(object sessionID, string fileName)
	{
		return new AP().ExportSepaNLCreditTransfer(_Database, Convert.ToInt32(sessionID), fileName);
	}

	public bool CalculateAPTaxablePayment(object taxablePaymentID)
	{
		return new AP().CalculateAPTaxablePayment(_Database, Convert.ToInt32(taxablePaymentID));
	}

	public bool ExportAusTaxablePaymentToFile(object taxablePaymentID)
	{
		return new AP().ExportAusTaxablePaymentToFile(_Database, Convert.ToInt32(taxablePaymentID));
	}

	public bool CloseTaxablePayment(object taxablePaymentID)
	{
		return new AP().CloseTaxablePayment(_Database, Convert.ToInt32(taxablePaymentID));
	}

	public bool ReopenTaxablePayment(object taxablePaymentID)
	{
		return new AP().ReopenTaxablePayment(_Database, Convert.ToInt32(taxablePaymentID));
	}
}
