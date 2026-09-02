using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.300", "Add Credit Date to AR/AP Invoices table", "2009-03-20")]
public class v710300
{
	public v710300(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpCreditDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpCreditDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoices Set arpCreditDate = arpInvoiceDate ");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoices", "appCreditDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoices", "appCreditDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APInvoices Set appCreditDate = appInvoiceDate ");
		}
	}
}
