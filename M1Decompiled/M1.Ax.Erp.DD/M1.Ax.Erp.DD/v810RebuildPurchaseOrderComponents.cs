using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PurchaseOrderComponents to support unicode", "2013-10-17")]
public class v810RebuildPurchaseOrderComponents
{
	public v810RebuildPurchaseOrderComponents(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderComponents", new DmoField[19]
		{
			new DmoField("pmoPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pmoPurchaseOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("pmoPurchaseOrderComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("pmoPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("pmoPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("pmoPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pmoPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("pmoQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("pmoAdditionalQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("pmoDeliveryQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("pmoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("pmoDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pmoWeight", "numeric", 15, 5, nullable: false),
			new DmoField("pmoQuantityReceived", "numeric", 15, 5, nullable: false),
			new DmoField("pmoReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("pmoClosed", "bit", 1, 0, nullable: false),
			new DmoField("pmoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pmoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pmoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("PMOPURCHASEORDERID,PMOPURCHASEORDERLINEID,PMOPURCHASEORDERCOMPONENTID", unique: true),
			new DmoIndex("PMOUNIQUEID", unique: true),
			new DmoIndex("pmoPurchaseOrderID", unique: false),
			new DmoIndex("pmoPurchaseOrderLineID", unique: false),
			new DmoIndex("pmoPurchaseOrderComponentID", unique: false),
			new DmoIndex("pmoPartID", unique: false),
			new DmoIndex("pmoPartRevisionID", unique: false),
			new DmoIndex("pmoReceivedComplete", unique: false),
			new DmoIndex("pmoClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
