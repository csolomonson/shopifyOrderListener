using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WarehouseReceiptComponents to support unicode", "2013-10-17")]
public class v810RebuildWarehouseReceiptComponents
{
	public v810RebuildWarehouseReceiptComponents(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceiptComponents", new DmoField[26]
		{
			new DmoField("wroWarehouseReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wroWarehouseReceiptLineID", "smallint", 4, 0, nullable: false),
			new DmoField("wroWarehouseReceiptComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("wroPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("wroPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("wroDestinationWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wroDestinationPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("wroQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("wroAdditionalQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("wroUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("wroDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("wroWeight", "numeric", 15, 5, nullable: false),
			new DmoField("wroQuantityReceived", "numeric", 15, 5, nullable: false),
			new DmoField("wroReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("wroClosed", "bit", 1, 0, nullable: false),
			new DmoField("wroSourceWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wroSourcePartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("wroWarehouseTransferID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wroWarehouseTransferLineID", "smallint", 4, 0, nullable: false),
			new DmoField("wroWarehouseTransComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("wroWarehouseRequisitionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wroWarehouseRequisitionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("wroWarehouseReqComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("wroCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("wroCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("wroUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[16]
		{
			new DmoIndex("WROWAREHOUSERECEIPTID,WROWAREHOUSERECEIPTLINEID,WROWAREHOUSERECEIPTCOMPONENTID", unique: true),
			new DmoIndex("WROUNIQUEID", unique: true),
			new DmoIndex("wroWarehouseReceiptID", unique: false),
			new DmoIndex("wroWarehouseReceiptLineID", unique: false),
			new DmoIndex("wroWarehouseReceiptComponentID", unique: false),
			new DmoIndex("wroPartID", unique: false),
			new DmoIndex("wroPartRevisionID", unique: false),
			new DmoIndex("wroReceivedComplete", unique: false),
			new DmoIndex("wroClosed", unique: false),
			new DmoIndex("wroSourcePartBinID", unique: false),
			new DmoIndex("wroWarehouseTransferID", unique: false),
			new DmoIndex("wroWarehouseTransferLineID", unique: false),
			new DmoIndex("wroWarehouseTransComponentID", unique: false),
			new DmoIndex("wroWarehouseRequisitionID", unique: false),
			new DmoIndex("wroWarehouseRequisitionLineID", unique: false),
			new DmoIndex("wroWarehouseReqComponentID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
