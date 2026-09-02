using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add Landed Costs Tables / Fields", "2008-06-03")]
public class v710000u
{
	public v710000u(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "LandedCosts"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCosts");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "LandedCostCharges"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCharges");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "LandedCostCategories"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCategories");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Tariffs"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Tariffs");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Receipts", "rmpLandedCostID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Receipts", "rmpLandedCostID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlDutyUnitCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlDutyUnitCost", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlFreightUnitCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlFreightUnitCost", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlMiscUnitCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlMiscUnitCost", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		parms.Dmo.AddMultipleColumns(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", true, true, parms.Messages, new object[4] { "imrStandardDutyCost", "numeric", 15, 5 }, new object[4] { "imrStandardFreightCost", "numeric", 15, 5 }, new object[4] { "imrStandardMiscCost", "numeric", 15, 5 }, new object[4] { "imrAverageDutyCost", "numeric", 15, 5 }, new object[4] { "imrAverageFreightCost", "numeric", 15, 5 }, new object[4] { "imrAverageMiscCost", "numeric", 15, 5 }, new object[4] { "imrLastDutyCost", "numeric", 15, 5 }, new object[4] { "imrLastFreightCost", "numeric", 15, 5 }, new object[4] { "imrLastMiscCost", "numeric", 15, 5 }, new object[4] { "imrTariffID", "char", 5, 0 });
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoiceLines", "aplLandedCostID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceLines", "aplLandedCostID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoiceLines", "aplLandedCostChargeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceLines", "aplLandedCostChargeID", "numeric", 3, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ExpenseAccountSplits", "xazLandedCostCategoryID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ExpenseAccountSplits", "xazLandedCostCategoryID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		parms.Dmo.AddMultipleColumns(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", true, true, parms.Messages, new object[4] { "imtUnitDutyCost", "numeric", 15, 5 }, new object[4] { "imtEstUnitDutyCost", "numeric", 15, 5 }, new object[4] { "imtEstTotalDutyCost", "money", 12, 2 }, new object[4] { "imtActualUnitDutyCost", "numeric", 15, 5 }, new object[4] { "imtActualTotalDutyCost", "money", 12, 2 }, new object[4] { "imtPrevUnitDutyCost", "numeric", 15, 5 }, new object[4] { "imtUnitFreightCost", "numeric", 15, 5 }, new object[4] { "imtEstUnitFreightCost", "numeric", 15, 5 }, new object[4] { "imtEstTotalFreightCost", "money", 12, 2 }, new object[4] { "imtActualUnitFreightCost", "numeric", 15, 5 }, new object[4] { "imtActualTotalFreightCost", "money", 12, 2 }, new object[4] { "imtPrevUnitFreightCost", "numeric", 15, 5 }, new object[4] { "imtUnitMiscCost", "numeric", 15, 5 }, new object[4] { "imtEstUnitMiscCost", "numeric", 15, 5 }, new object[4] { "imtEstTotalMiscCost", "money", 12, 2 }, new object[4] { "imtActualUnitMiscCost", "numeric", 15, 5 }, new object[4] { "imtActualTotalMiscCost", "money", 12, 2 }, new object[4] { "imtPrevUnitMiscCost", "numeric", 15, 5 });
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrders", "pmpLandedCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrders", "pmpLandedCost", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Receipts", "rmpLandedCostPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Receipts", "rmpLandedCostPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumberTransactions", "sntLandedCostID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumberTransactions", "sntLandedCostID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumberTransactions", "abtLandedCostID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumberTransactions", "abtLandedCostID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
