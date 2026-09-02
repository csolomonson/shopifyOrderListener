using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SalesOrderComponents to support unicode", "2013-10-17")]
public class v810RebuildSalesOrderComponents
{
	public v810RebuildSalesOrderComponents(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderComponents", new DmoField[20]
		{
			new DmoField("omoSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omoSalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omoSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
			new DmoField("omoSalesOrderComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("omoPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("omoPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("omoPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("omoPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("omoQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("omoAdditionalQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("omoDeliveryQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("omoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("omoDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("omoWeight", "numeric", 15, 5, nullable: false),
			new DmoField("omoQuantityShipped", "numeric", 15, 5, nullable: false),
			new DmoField("omoShippedComplete", "bit", 1, 0, nullable: false),
			new DmoField("omoClosed", "bit", 1, 0, nullable: false),
			new DmoField("omoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("omoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("omoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[10]
		{
			new DmoIndex("OMOSALESORDERID,OMOSALESORDERLINEID,OMOSALESORDERDELIVERYID,OMOSALESORDERCOMPONENTID", unique: true),
			new DmoIndex("OMOUNIQUEID", unique: true),
			new DmoIndex("omoSalesOrderID", unique: false),
			new DmoIndex("omoSalesOrderLineID", unique: false),
			new DmoIndex("omoSalesOrderDeliveryID", unique: false),
			new DmoIndex("omoSalesOrderComponentID", unique: false),
			new DmoIndex("omoPartID", unique: false),
			new DmoIndex("omoPartRevisionID", unique: false),
			new DmoIndex("omoShippedComplete", unique: false),
			new DmoIndex("omoClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
