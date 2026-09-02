using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.022", "Add Tax Subtotal to ARRecurringInvoices", "2013-03-12")]
public class v810022a
{
	public v810022a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrTaxSubtotalBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrTaxSubtotalBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARRecurringInvoices Set arrTaxSubtotalBase = arrInvoiceTaxAmountBase - arrFreightTaxAmountBase - arrSecondFreightTaxAmtBase");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrTaxSubtotalForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrTaxSubtotalForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARRecurringInvoices Set arrTaxSubtotalForeign = arrInvoiceTaxAmountForeign - arrFreightTaxAmountForeign - arrSecondFreightTaxAmtForeign");
		}
	}
}
