using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.036", "Add Tax Date to ARInvoices tables", "2013-09-12")]
public class v810036
{
	public v810036(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpTaxDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpTaxDate", "date", 8, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoices Set arpTaxDate = Case When arpInvoiceType = 2 Then arpCreditDate Else arpInvoiceDate End");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlTaxDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlTaxDate", "date", 8, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoiceLines Set arlTaxDate = arpTaxDate From ARInvoiceLines Inner Join ARInvoices On arlARInvoiceID = arpARInvoiceID");
		}
	}
}
