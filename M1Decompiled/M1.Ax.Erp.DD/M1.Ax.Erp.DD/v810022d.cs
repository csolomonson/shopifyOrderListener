using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.022", "Add Fields to ARInvoices tables", "2013-03-21")]
public class v810022d
{
	public v810022d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpIncludeTaxInRetention"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpIncludeTaxInRetention", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoices Set arpIncludeTaxInRetention = cmoARIncludeTaxInRetention From ARInvoices Inner Join Organizations On arpCustomerOrganizationID = cmoOrganizationID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlIncludeTaxInRetention"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlIncludeTaxInRetention", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoiceLines Set arlIncludeTaxInRetention = arpIncludeTaxInRetention From ARInvoiceLines Inner Join ARInvoices On arpARInvoiceID = arlARInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpRetentionPaidBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpRetentionPaidBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoices Set arpRetentionPaidBase = arpRetentionTotalBase - arpRetentionBalanceBase");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpRetentionPaidForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpRetentionPaidForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoices Set arpRetentionPaidForeign = arpRetentionTotalForeign - arpRetentionBalanceForeign");
		}
	}
}
