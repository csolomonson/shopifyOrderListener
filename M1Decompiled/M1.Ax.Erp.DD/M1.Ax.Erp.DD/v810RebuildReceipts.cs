using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Receipts to support unicode", "2013-10-17")]
public class v810RebuildReceipts
{
	public v810RebuildReceipts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Receipts", new DmoField[31]
		{
			new DmoField("rmpReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmpPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmpPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmpDeliveryDocket", "nvarchar", 20, 0, nullable: false),
			new DmoField("rmpSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmpAPInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmpAPInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmpPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmpPurchaseContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmpReceiptDate", "datetime", 14, 0, nullable: true),
			new DmoField("rmpShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmpReceiptSubtotal", "money", 12, 2, nullable: false),
			new DmoField("rmpFreightCharge", "money", 12, 2, nullable: false),
			new DmoField("rmpReceiptTotal", "money", 12, 2, nullable: false),
			new DmoField("rmpProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmpClosed", "bit", 1, 0, nullable: false),
			new DmoField("rmpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("rmpFreightChargeForeign", "money", 12, 2, nullable: false),
			new DmoField("rmpCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmpExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("rmpCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("rmpReceiptSubtotalForeign", "money", 12, 2, nullable: false),
			new DmoField("rmpReceiptTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("rmpLandedCostID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmpLandedCostPosted", "bit", 1, 0, nullable: false),
			new DmoField("rmpPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("rmpPostedDate", "date", 14, 0, nullable: true),
			new DmoField("rmpReversalEntry", "bit", 1, 0, nullable: false),
			new DmoField("rmpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rmpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rmpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("RMPRECEIPTID", unique: true),
			new DmoIndex("RMPUNIQUEID", unique: true),
			new DmoIndex("rmpPlantDepartmentID", unique: false),
			new DmoIndex("rmpPlantID", unique: false),
			new DmoIndex("rmpDeliveryDocket", unique: false),
			new DmoIndex("rmpSupplierOrganizationID", unique: false),
			new DmoIndex("rmpAPInvoiceLocationID", unique: false),
			new DmoIndex("rmpPurchaseLocationID", unique: false),
			new DmoIndex("rmpProjectID", unique: false),
			new DmoIndex("rmpClosed", unique: false),
			new DmoIndex("rmpLandedCostID", unique: false),
			new DmoIndex("rmpLandedCostPosted", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
