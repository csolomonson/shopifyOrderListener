using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.022", "Add Commission Fields to ARRecurringInvoices tables", "2013-03-14")]
public class v810022b
{
	public v810022b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoiceLines", "arqCommissionRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoiceLines", "arqCommissionRate", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARRecurringInvoiceLines Set arqCommissionRate = (Case When imuCommissionType = 2 Then imuCommissionRate Else 0 End) From ARRecurringInvoiceLines Inner Join PartGroups on arqPartGroupID = imuPartGroupID Where arqPayCommission <> 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoiceLines", "arqAmtForResellerCommission"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoiceLines", "arqAmtForResellerCommission", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARRecurringInvoiceLines Set arqAmtForResellerCommission = arqExtendedPriceBase Where arqPayCommission <> 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoiceLines", "arqAmtForSalesCommission"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoiceLines", "arqAmtForSalesCommission", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARRecurringInvoiceLines Set arqAmtForSalesCommission = arqExtendedPriceBase Where arqPayCommission <> 0 And arqCommissionRate = 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoiceLines", "arqCommissionAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoiceLines", "arqCommissionAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARRecurringInvoiceLines Set arqCommissionAmount = arqExtendedPriceBase * (arqCommissionRate / 100) Where arqPayCommission <> 0 And arqCommissionRate <> 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrTotalForResellerCommission"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrTotalForResellerCommission", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARRecurringInvoices SET arrTotalForResellerCommission = arqAmtForResellerCommission From ARRecurringInvoices Inner Join (Select arqARRecurringInvoiceID,Sum(arqAmtForResellerCommission) As arqAmtForResellerCommission From ARRecurringInvoiceLines Group By arqARRecurringInvoiceID) As ARRecurringInvoiceLines On arrARRecurringInvoiceID = arqARRecurringInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrResellerCommissionRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrResellerCommissionRate", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARRecurringInvoices Set arrResellerCommissionRate = cmoResellerCommissionRate From ARRecurringInvoices Inner Join Organizations on cmoOrganizationID = arrResellerOrganizationID Where arrResellerOrganizationID <> ''");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrResellerCommissionAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrResellerCommissionAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARRecurringInvoices Set arrResellerCommissionAmount = arrTotalForResellerCommission * (arrResellerCommissionRate / 100) Where arrResellerCommissionRate <> 0 And arrTotalForResellerCommission <> 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrLineCommissionTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrLineCommissionTotal", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARRecurringInvoices SET arrLineCommissionTotal = arqCommissionAmount From ARRecurringInvoices Inner Join (Select arqARRecurringInvoiceID,Sum(arqCommissionAmount) As arqCommissionAmount From ARRecurringInvoiceLines Group By arqARRecurringInvoiceID) As ARRecurringInvoiceLines On arrARRecurringInvoiceID = arqARRecurringInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrTotalForSalesCommission"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrTotalForSalesCommission", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARRecurringInvoices SET arrTotalForSalesCommission = arqAmtForSalesCommission From ARRecurringInvoices Inner Join (Select arqARRecurringInvoiceID,Sum(arqAmtForSalesCommission) As arqAmtForSalesCommission From ARRecurringInvoiceLines Group By arqARRecurringInvoiceID) As ARRecurringInvoiceLines On arrARRecurringInvoiceID = arqARRecurringInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrSalesCommissionTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrSalesCommissionTotal", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARRecurringInvoices SET arrSalesCommissionTotal = aroAmount From ARRecurringInvoices Inner Join (Select aroARRecurringInvoiceID,Sum(aroAmount) As aroAmount From ARRecurringInvoiceSalesPeople Group By aroARRecurringInvoiceID) As ARRecurringInvoiceSalesPeople On arrARRecurringInvoiceID = aroARRecurringInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrTaxSubtotalBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrTaxSubtotalBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARRecurringInvoices SET arrTaxSubtotalBase = arqTaxAmount From ARRecurringInvoices Inner Join (Select arqARRecurringInvoiceID,Sum(arqTaxAmountBase + arqSecondTaxAmountBase) As arqTaxAmount From ARRecurringInvoiceLines Group By arqARRecurringInvoiceID) As ARRecurringInvoiceLines On arrARRecurringInvoiceID = arqARRecurringInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrTaxSubtotalForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrTaxSubtotalForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARRecurringInvoices SET arrTaxSubtotalForeign = arqTaxAmount From ARRecurringInvoices Inner Join (Select arqARRecurringInvoiceID,Sum(arqTaxAmountForeign + arqSecondTaxAmountForeign) As arqTaxAmount From ARRecurringInvoiceLines Group By arqARRecurringInvoiceID) As ARRecurringInvoiceLines On arrARRecurringInvoiceID = arqARRecurringInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARRecurringInvoices", "arrSplitPercentTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoices", "arrSplitPercentTotal", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARRecurringInvoices SET arrSplitPercentTotal = aroPercent From ARRecurringInvoices Inner Join (Select aroARRecurringInvoiceID,Sum(aroPercent) As aroPercent From ARRecurringInvoiceSalesPeople Group By aroARRecurringInvoiceID) As ARRecurringInvoiceSalesPeople On arrARRecurringInvoiceID = aroARRecurringInvoiceID");
		}
	}
}
