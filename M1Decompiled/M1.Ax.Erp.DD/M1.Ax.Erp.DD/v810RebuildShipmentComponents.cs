using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ShipmentComponents to support unicode", "2013-10-17")]
public class v810RebuildShipmentComponents
{
	public v810RebuildShipmentComponents(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentComponents", new DmoField[23]
		{
			new DmoField("smoShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smoShipmentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("smoShipmentComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("smoPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("smoPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("smoPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("smoPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("smoQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("smoAdditionalQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("smoQuantityShipped", "numeric", 15, 5, nullable: false),
			new DmoField("smoShippedComplete", "bit", 1, 0, nullable: false),
			new DmoField("smoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("smoDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("smoWeight", "numeric", 15, 5, nullable: false),
			new DmoField("smoSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smoSalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("smoSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
			new DmoField("smoSalesOrderComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("smoClosed", "bit", 1, 0, nullable: false),
			new DmoField("smoPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("smoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("smoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("smoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("SMOSHIPMENTID,SMOSHIPMENTLINEID,SMOSHIPMENTCOMPONENTID", unique: true),
			new DmoIndex("SMOUNIQUEID", unique: true),
			new DmoIndex("smoShipmentID", unique: false),
			new DmoIndex("smoShipmentLineID", unique: false),
			new DmoIndex("smoShipmentComponentID", unique: false),
			new DmoIndex("smoPartID", unique: false),
			new DmoIndex("smoPartRevisionID", unique: false),
			new DmoIndex("smoShippedComplete", unique: false),
			new DmoIndex("smoSalesOrderID", unique: false),
			new DmoIndex("smoSalesOrderLineID", unique: false),
			new DmoIndex("smoSalesOrderDeliveryID", unique: false),
			new DmoIndex("smoSalesOrderComponentID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
