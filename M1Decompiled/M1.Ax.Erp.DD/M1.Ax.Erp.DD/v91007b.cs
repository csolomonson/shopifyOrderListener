using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.007", "Update PartTransactions to reverse issue types quantities and total costs", "2016-01-28")]
public class v91007b
{
	public v91007b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartTransactions"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PartTransactions SET imtActualTotalCostOfGoodsSold = imtActualTotalCostOfGoodsSold * -1, imtActualTotalDutyCost = imtActualTotalDutyCost * -1, imtActualTotalFreightCost = imtActualTotalFreightCost * -1, imtActualTotalLaborCost = imtActualTotalLaborCost * -1, imtActualTotalMaterialCost = imtActualTotalMaterialCost * -1, imtActualTotalMiscCost = imtActualTotalMiscCost * -1, imtActualTotalOverheadCost = imtActualTotalOverheadCost * -1, imtActualTotalSubcontractCost = imtActualTotalSubcontractCost * -1, imtEstTotalCostOfGoodsSold = imtEstTotalCostOfGoodsSold * -1, imtEstTotalDutyCost = imtEstTotalDutyCost * -1, imtEstTotalFreightCost = imtEstTotalFreightCost * -1, imtEstTotalLaborCost = imtEstTotalLaborCost * -1, imtEstTotalMaterialCost = imtEstTotalMaterialCost * -1, imtEstTotalMiscCost = imtEstTotalMiscCost * -1, imtEstTotalOverheadCost = imtEstTotalOverheadCost * -1, imtEstTotalSubcontractCost = imtEstTotalSubcontractCost * -1, imtInventoryQuantityReceived = imtInventoryQuantityReceived * -1, imtScrapQuantity = imtScrapQuantity * -1, imtPurchaseQuantityReceived = imtPurchaseQuantityReceived * -1 WHERE(imtTransactionType = 2 and imtIssueType <> 0) Or(imtTransactionType = 2 and imtIssueType = 0 and(imtShipmentID <> '' or imtSalesOrderID <> '') and imtJobID = '')");
		}
	}
}
