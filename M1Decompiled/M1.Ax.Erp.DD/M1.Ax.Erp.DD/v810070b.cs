using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.071", "Add fields to ARInvoices table", "2014-04-30")]
public class v810070b
{
	public v810070b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpInvoicePaidBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpInvoicePaidBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpInvoicePaidForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpInvoicePaidForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoices Set arpInvoicePaidBase = arpInvoiceTotalBase - arpDepositAppliedBase - arpInvoiceBalanceBase, arpInvoicePaidForeign = arpInvoiceTotalForeign - arpDepositAppliedForeign - arpInvoiceBalanceForeign");
	}
}
