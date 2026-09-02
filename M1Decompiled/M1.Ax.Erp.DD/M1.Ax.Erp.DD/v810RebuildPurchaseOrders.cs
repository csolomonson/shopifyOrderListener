using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PurchaseOrders to support unicode", "2013-10-17")]
public class v810RebuildPurchaseOrders
{
	public v810RebuildPurchaseOrders(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrders", new DmoField[42]
		{
			new DmoField("pmpPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmpPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmpAPInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpAPInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpPurchaseContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpDropShipOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmpDropShipLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpDropShipContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpOrderDate", "date", 14, 0, nullable: true),
			new DmoField("pmpDueDate", "date", 14, 0, nullable: true),
			new DmoField("pmpFreeOnBoardDescription", "nvarchar", 15, 0, nullable: false),
			new DmoField("pmpPaymentTermID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpOrderCommentsRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("pmpOrderCommentsText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("pmpReadyToPrint", "bit", 1, 0, nullable: false),
			new DmoField("pmpStandardMessageID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmpCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmpBuyerEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmpCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("pmpExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("pmpDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("pmpOrderSubtotalBase", "money", 12, 2, nullable: false),
			new DmoField("pmpOrderSubtotalForeign", "money", 12, 2, nullable: false),
			new DmoField("pmpOrderTaxAmountBase", "money", 12, 2, nullable: false),
			new DmoField("pmpOrderTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("pmpOrderTotalBase", "money", 12, 2, nullable: false),
			new DmoField("pmpOrderTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("pmpStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("pmpApprovalRequestDate", "datetime", 14, 0, nullable: true),
			new DmoField("pmpApprovalDecisionDate", "datetime", 14, 0, nullable: true),
			new DmoField("pmpNextApprovalEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmpProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmpClosed", "bit", 1, 0, nullable: false),
			new DmoField("pmpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("pmpLandedCost", "bit", 1, 0, nullable: false),
			new DmoField("pmpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pmpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pmpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[15]
		{
			new DmoIndex("PMPPURCHASEORDERID", unique: true),
			new DmoIndex("PMPUNIQUEID", unique: true),
			new DmoIndex("pmpPlantDepartmentID", unique: false),
			new DmoIndex("pmpPlantID", unique: false),
			new DmoIndex("pmpSupplierOrganizationID", unique: false),
			new DmoIndex("pmpAPInvoiceLocationID", unique: false),
			new DmoIndex("pmpDropShipOrganizationID", unique: false),
			new DmoIndex("pmpDropShipLocationID", unique: false),
			new DmoIndex("pmpDropShipContactID", unique: false),
			new DmoIndex("pmpReadyToPrint", unique: false),
			new DmoIndex("pmpStatus", unique: false),
			new DmoIndex("pmpNextApprovalEmployeeID", unique: false),
			new DmoIndex("pmpProjectID", unique: false),
			new DmoIndex("pmpClosed", unique: false),
			new DmoIndex("pmpLandedCost", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
