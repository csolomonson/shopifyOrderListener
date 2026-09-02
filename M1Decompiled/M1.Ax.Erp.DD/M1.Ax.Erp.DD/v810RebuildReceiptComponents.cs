using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ReceiptComponents to support unicode", "2013-10-17")]
public class v810RebuildReceiptComponents
{
	public v810RebuildReceiptComponents(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", new DmoField[22]
		{
			new DmoField("rmoReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmoReceiptLineID", "smallint", 4, 0, nullable: false),
			new DmoField("rmoReceiptComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("rmoPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("rmoPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("rmoPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmoPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("rmoQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("rmoAdditionalQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("rmoQuantityReceived", "numeric", 15, 5, nullable: false),
			new DmoField("rmoReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("rmoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("rmoDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("rmoWeight", "numeric", 15, 5, nullable: false),
			new DmoField("rmoPurchaseOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmoPurchaseOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("rmoPurchaseOrderComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("rmoClosed", "bit", 1, 0, nullable: false),
			new DmoField("rmoPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("rmoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rmoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rmoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("RMORECEIPTID,RMORECEIPTLINEID,RMORECEIPTCOMPONENTID", unique: true),
			new DmoIndex("RMOUNIQUEID", unique: true),
			new DmoIndex("rmoReceiptID", unique: false),
			new DmoIndex("rmoReceiptLineID", unique: false),
			new DmoIndex("rmoReceiptComponentID", unique: false),
			new DmoIndex("rmoPartID", unique: false),
			new DmoIndex("rmoPartRevisionID", unique: false),
			new DmoIndex("rmoReceivedComplete", unique: false),
			new DmoIndex("rmoPurchaseOrderID", unique: false),
			new DmoIndex("rmoPurchaseOrderLineID", unique: false),
			new DmoIndex("rmoPurchaseOrderComponentID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
