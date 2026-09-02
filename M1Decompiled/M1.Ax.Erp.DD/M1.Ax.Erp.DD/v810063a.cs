using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.063", "Add fields to QuoteQuantities table", "2013-12-23")]
public class v810063a
{
	public v810063a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqAdditionalCostPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqAdditionalCostPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqPurchaseToOrderPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqPurchaseToOrderPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTotalMarkupPercent"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqTotalMarkupPercent", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqSubcontractPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqSubcontractPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTotalCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqTotalCost", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTotalPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqTotalPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqLaborPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqLaborPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTotalUnitPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqTotalUnitPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqPurchaseUnitCostBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqPurchaseUnitCostBase", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqSecondTaxCodeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqSecondTaxCodeID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTaxDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqTaxDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqQuoteMarkupType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqQuoteMarkupType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTaxCodeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqTaxCodeID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqCalculatedUnitPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqCalculatedUnitPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqQuotingPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqQuotingPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTotalUnitCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqTotalUnitCost", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqOverheadPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqOverheadPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqMaterialPrice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqMaterialPrice", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqPurchaseToOrder"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteQuantities", "qmqPurchaseToOrder", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqAddSecondTaxAmountBase");
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqPurchaseToOrderCost"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QuoteQuantities Set qmqPurchaseToOrderCost = qmqPurchaseUnitCostBase*qmqQuoteQuantity");
		}
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqAddTaxAmountBase");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTotalRunQuantity");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqUnitSecondTaxAmountBase");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqUnitTaxAmountBase");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqAdditionalCostPrice");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqPurchaseToOrderPrice");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqSubcontractPrice");
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTotalCost"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QuoteQuantities Set qmqTotalCost = qmqMaterialCost+qmqSubcontractCost+qmqLaborCost+qmqOverheadCost+qmqAdditionalCostAmount+qmqPurchaseToOrderCost");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTotalPrice"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QuoteQuantities Set qmqTotalPrice = qmqMaterialPrice+qmqSubcontractPrice+qmqQuotingPrice+qmqMaterialPrice+qmqSubcontractPrice+qmqLaborPrice+qmqOverheadPrice+qmqAdditionalCostPrice");
		}
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqLaborPrice");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTotalUnitPrice");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqCalculatedUnitPrice");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqQuotingPrice");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqTotalUnitCost");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqOverheadPrice");
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteQuantities", "qmqMaterialPrice");
	}
}
