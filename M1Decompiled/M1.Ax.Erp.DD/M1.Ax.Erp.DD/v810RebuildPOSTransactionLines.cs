using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert POSTransactionLines to support unicode", "2013-10-17")]
public class v810RebuildPOSTransactionLines
{
	public v810RebuildPOSTransactionLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "POSTransactionLines", new DmoField[36]
		{
			new DmoField("pslPOSTransactionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pslPOSTransactionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("pslPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("pslOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("pslPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("pslUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("pslPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pslOrgPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pslPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("pslPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("pslPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pslAlwaysNonTaxable", "bit", 1, 0, nullable: false),
			new DmoField("pslQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("pslFullUnitPrice", "numeric", 15, 5, nullable: false),
			new DmoField("pslDiscountPercent", "numeric", 6, 2, nullable: false),
			new DmoField("pslUnitDiscount", "numeric", 15, 5, nullable: false),
			new DmoField("pslUnitPrice", "numeric", 15, 5, nullable: false),
			new DmoField("pslFullExtendedPrice", "money", 12, 2, nullable: false),
			new DmoField("pslExtendedDiscount", "money", 12, 2, nullable: false),
			new DmoField("pslExtendedPrice", "money", 12, 2, nullable: false),
			new DmoField("pslFreightAmount", "money", 12, 2, nullable: false),
			new DmoField("pslTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pslNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pslTaxAmount", "money", 14, 4, nullable: false),
			new DmoField("pslSecondTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pslSecondTaxAmount", "money", 14, 4, nullable: false),
			new DmoField("pslPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pslPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("pslSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pslSalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("pslSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
			new DmoField("pslVoided", "bit", 1, 0, nullable: false),
			new DmoField("pslPosted", "bit", 1, 0, nullable: false),
			new DmoField("pslCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pslCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pslUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[13]
		{
			new DmoIndex("PSLPOSTRANSACTIONID,PSLPOSTRANSACTIONLINEID", unique: true),
			new DmoIndex("PSLUNIQUEID", unique: true),
			new DmoIndex("pslPOSTransactionID", unique: false),
			new DmoIndex("pslPOSTransactionLineID", unique: false),
			new DmoIndex("pslPartID", unique: false),
			new DmoIndex("pslOrgPartID", unique: false),
			new DmoIndex("pslPartRevisionID", unique: false),
			new DmoIndex("pslPartWarehouseLocationID", unique: false),
			new DmoIndex("pslPartBinID", unique: false),
			new DmoIndex("pslSalesOrderID", unique: false),
			new DmoIndex("pslSalesOrderLineID", unique: false),
			new DmoIndex("pslVoided", unique: false),
			new DmoIndex("pslPosted", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
