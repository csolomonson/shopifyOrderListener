using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.022", "Add Commission Fields to ARInvoices tables", "2013-04-02")]
public class v810022e
{
	public v810022e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlCommissionRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlCommissionRate", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoiceLines Set arlCommissionRate = (Case When imuCommissionType = 2 Then imuCommissionRate Else 0 End) From ARInvoiceLines Inner Join PartGroups on arlPartGroupID = imuPartGroupID Where arlPayCommission <> 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlAmtForResellerCommission"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlAmtForResellerCommission", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoiceLines Set arlAmtForResellerCommission = arlExtendedPriceBase Where arlPayCommission <> 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlAmtForSalesCommission"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlAmtForSalesCommission", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoiceLines Set arlAmtForSalesCommission = arlExtendedPriceBase Where arlPayCommission <> 0 And arlCommissionRate = 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlCommissionAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlCommissionAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoiceLines Set arlCommissionAmount = arlExtendedPriceBase * (arlCommissionRate / 100) Where arlPayCommission <> 0 And arlCommissionRate <> 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpTotalForResellerCommission"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpTotalForResellerCommission", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARInvoices SET arpTotalForResellerCommission = arlAmtForResellerCommission From ARInvoices Inner Join (Select arlARInvoiceID,Sum(arlAmtForResellerCommission) As arlAmtForResellerCommission From ARInvoiceLines Group By arlARInvoiceID) As ARInvoiceLines On arpARInvoiceID = arlARInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpResellerCommissionRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpResellerCommissionRate", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoices Set arpResellerCommissionRate = cmoResellerCommissionRate From ARInvoices Inner Join Organizations on cmoOrganizationID = arpResellerOrganizationID Where arpResellerOrganizationID <> ''");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpResellerCommissionAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpResellerCommissionAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoices Set arpResellerCommissionAmount = arpTotalForResellerCommission * (arpResellerCommissionRate / 100) Where arpResellerCommissionRate <> 0 And arpTotalForResellerCommission <> 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpLineCommissionTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpLineCommissionTotal", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARInvoices SET arpLineCommissionTotal = arlCommissionAmount From ARInvoices Inner Join (Select arlARInvoiceID,Sum(arlCommissionAmount) As arlCommissionAmount From ARInvoiceLines Group By arlARInvoiceID) As ARInvoiceLines On arpARInvoiceID = arlARInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpTotalForSalesCommission"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpTotalForSalesCommission", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARInvoices SET arpTotalForSalesCommission = arlAmtForSalesCommission From ARInvoices Inner Join (Select arlARInvoiceID,Sum(arlAmtForSalesCommission) As arlAmtForSalesCommission From ARInvoiceLines Group By arlARInvoiceID) As ARInvoiceLines On arpARInvoiceID = arlARInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpSalesCommissionTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpSalesCommissionTotal", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARInvoices SET arpSalesCommissionTotal = arjAmount From ARInvoices Inner Join (Select arjARInvoiceID,Sum(arjAmount) As arjAmount From ARInvoiceSalesPeople Group By arjARInvoiceID) As ARInvoiceSalesPeople On arpARInvoiceID = arjARInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpTaxSubtotalBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpTaxSubtotalBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARInvoices SET arpTaxSubtotalBase = arlTaxAmount From ARInvoices Inner Join (Select arlARInvoiceID,Sum(arlTaxAmountBase + arlSecondTaxAmountBase) As arlTaxAmount From ARInvoiceLines Where arlDepositLine = 0 Group By arlARInvoiceID) As ARInvoiceLines On arpARInvoiceID = arlARInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpTaxSubtotalForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpTaxSubtotalForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARInvoices SET arpTaxSubtotalForeign = arlTaxAmount From ARInvoices Inner Join (Select arlARInvoiceID,Sum(arlTaxAmountForeign + arlSecondTaxAmountForeign) As arlTaxAmount From ARInvoiceLines Where arlDepositLine = 0 Group By arlARInvoiceID) As ARInvoiceLines On arpARInvoiceID = arlARInvoiceID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpSplitPercentTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpSplitPercentTotal", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ARInvoices SET arpSplitPercentTotal = arjPercent From ARInvoices Inner Join (Select arjARInvoiceID,Sum(arjPercent) As arjPercent From ARInvoiceSalesPeople Group By arjARInvoiceID) As ARInvoiceSalesPeople On arpARInvoiceID = arjARInvoiceID");
		}
	}
}
