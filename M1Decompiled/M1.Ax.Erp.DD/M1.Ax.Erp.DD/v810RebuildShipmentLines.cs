using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ShipmentLines to support unicode", "2013-10-17")]
public class v810RebuildShipmentLines
{
	public v810RebuildShipmentLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentLines", new DmoField[45]
		{
			new DmoField("smlShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smlShipmentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("smlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("smlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("smlOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("smlPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("smlPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("smlQuantityShipped", "numeric", 15, 5, nullable: false),
			new DmoField("smlOverridePrice", "bit", 1, 0, nullable: false),
			new DmoField("smlUnitPrice", "numeric", 15, 5, nullable: false),
			new DmoField("smlUnitPriceForeign", "numeric", 15, 5, nullable: false),
			new DmoField("smlExtendedPriceBase", "money", 12, 2, nullable: false),
			new DmoField("smlExtendedPriceForeign", "money", 12, 2, nullable: false),
			new DmoField("smlFreightAmount", "money", 12, 2, nullable: false),
			new DmoField("smlFreightAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("smlShippedComplete", "bit", 1, 0, nullable: false),
			new DmoField("smlInvoicedComplete", "bit", 1, 0, nullable: false),
			new DmoField("smlUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("smlKitPart", "bit", 1, 0, nullable: false),
			new DmoField("smlDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("smlOrgPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("smlPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("smlPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("smlRequiresInspection", "bit", 1, 0, nullable: false),
			new DmoField("smlSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smlSalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("smlSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
			new DmoField("smlJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("smlHeatLot", "nvarchar", 50, 0, nullable: false),
			new DmoField("smlPartGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("smlWeight", "numeric", 15, 5, nullable: false),
			new DmoField("smlExtendedWeight", "numeric", 15, 5, nullable: false),
			new DmoField("smlProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smlProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("smlPOSSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smlPOSTransactionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smlPOSTransactionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("smlClosed", "bit", 1, 0, nullable: false),
			new DmoField("smlPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("smlReversed", "bit", 1, 0, nullable: false),
			new DmoField("smlReverseShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smlReverseShipmentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("smlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("smlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("smlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[19]
		{
			new DmoIndex("SMLSHIPMENTID,SMLSHIPMENTLINEID", unique: true),
			new DmoIndex("SMLUNIQUEID", unique: true),
			new DmoIndex("smlShipmentID", unique: false),
			new DmoIndex("smlShipmentLineID", unique: false),
			new DmoIndex("smlPartID", unique: false),
			new DmoIndex("smlPartRevisionID", unique: false),
			new DmoIndex("smlOrgPartID", unique: false),
			new DmoIndex("smlShippedComplete", unique: false),
			new DmoIndex("smlInvoicedComplete", unique: false),
			new DmoIndex("smlRequiresInspection", unique: false),
			new DmoIndex("smlSalesOrderID", unique: false),
			new DmoIndex("smlSalesOrderLineID", unique: false),
			new DmoIndex("smlJobID", unique: false),
			new DmoIndex("smlProjectID", unique: false),
			new DmoIndex("smlProjectAreaID", unique: false),
			new DmoIndex("smlPOSSessionID", unique: false),
			new DmoIndex("smlPOSTransactionID", unique: false),
			new DmoIndex("smlReverseShipmentID", unique: false),
			new DmoIndex("smlReverseShipmentLineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
