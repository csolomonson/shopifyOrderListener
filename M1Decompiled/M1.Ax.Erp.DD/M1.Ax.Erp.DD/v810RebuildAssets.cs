using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Assets to support unicode", "2013-10-17")]
public class v810RebuildAssets
{
	public v810RebuildAssets(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Assets", new DmoField[50]
		{
			new DmoField("fapAssetID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fapAssetTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("fapPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("fapPurchaseType", "nvarchar", 1, 0, nullable: false),
			new DmoField("fapItemType", "nvarchar", 1, 0, nullable: false),
			new DmoField("fapDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("fapWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("fapLocation", "nvarchar", 30, 0, nullable: false),
			new DmoField("fapSerialNumber", "nvarchar", 30, 0, nullable: false),
			new DmoField("fapLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fapLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fapPurchaseDate", "date", 14, 0, nullable: true),
			new DmoField("fapReceiptDate", "date", 14, 0, nullable: true),
			new DmoField("fapInServiceDate", "date", 14, 0, nullable: true),
			new DmoField("fapDisposalDate", "date", 14, 0, nullable: true),
			new DmoField("fapPurchaseValue", "money", 12, 2, nullable: false),
			new DmoField("fapDeemedValue", "money", 12, 2, nullable: false),
			new DmoField("fapDisposalValue", "money", 12, 2, nullable: false),
			new DmoField("fapQuantity", "int", 9, 0, nullable: false),
			new DmoField("fapTaxEffectiveLife", "numeric", 5, 2, nullable: false),
			new DmoField("fapTaxDepreciationRate", "numeric", 6, 2, nullable: false),
			new DmoField("fapBookEffectiveLife", "numeric", 5, 2, nullable: false),
			new DmoField("fapBookDepreciationRate", "numeric", 6, 2, nullable: false),
			new DmoField("fapDepreciationStartDate", "date", 14, 0, nullable: true),
			new DmoField("fapTaxDepreciationEndDate", "date", 14, 0, nullable: true),
			new DmoField("fapBookDepreciationEndDate", "date", 14, 0, nullable: true),
			new DmoField("fapBookStartValue", "money", 12, 2, nullable: false),
			new DmoField("fapTaxStartValue", "money", 12, 2, nullable: false),
			new DmoField("fapDepreciationLimit", "money", 12, 2, nullable: false),
			new DmoField("fapStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("fapEstimatedProductionUnits", "int", 9, 0, nullable: false),
			new DmoField("fapPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fapPurchaseOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("fapSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fapReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fapReceiptLineID", "smallint", 4, 0, nullable: false),
			new DmoField("fapAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fapAPInvoiceLineID", "smallint", 4, 0, nullable: false),
			new DmoField("fapFinanceOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fapLeaseMonths", "smallint", 3, 0, nullable: false),
			new DmoField("fapLeaseExpiryDate", "date", 14, 0, nullable: true),
			new DmoField("fapResidualAmount", "money", 12, 2, nullable: false),
			new DmoField("fapPaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("fapLowCostAsset", "bit", 1, 0, nullable: false),
			new DmoField("fapLowValueAssetInPool", "bit", 1, 0, nullable: false),
			new DmoField("fapStartYearInPool", "smallint", 4, 0, nullable: false),
			new DmoField("fapTaxableUsePercentage", "numeric", 6, 2, nullable: false),
			new DmoField("fapCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("fapCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("fapUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("FAPASSETID", unique: true),
			new DmoIndex("FAPUNIQUEID", unique: true),
			new DmoIndex("fapAssetTypeID", unique: false),
			new DmoIndex("fapPlantID", unique: false),
			new DmoIndex("fapWorkCenterID", unique: false),
			new DmoIndex("fapSupplierOrganizationID", unique: false),
			new DmoIndex("fapReceiptID", unique: false),
			new DmoIndex("fapReceiptLineID", unique: false),
			new DmoIndex("fapAPInvoiceID", unique: false),
			new DmoIndex("fapAPInvoiceLineID", unique: false),
			new DmoIndex("fapFinanceOrganizationID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
