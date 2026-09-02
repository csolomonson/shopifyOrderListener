using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WarehouseReceiptLines to support unicode", "2013-10-17")]
public class v810RebuildWarehouseReceiptLines
{
	public v810RebuildWarehouseReceiptLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceiptLines", new DmoField[24]
		{
			new DmoField("wrlWarehouseReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wrlWarehouseReceiptLineID", "smallint", 4, 0, nullable: false),
			new DmoField("wrlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("wrlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("wrlPartDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("wrlSourceWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wrlSourcePartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("wrlDestinationWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wrlDestinationPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("wrlQuantityReceived", "numeric", 15, 5, nullable: false),
			new DmoField("wrlUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("wrlUnitCost", "numeric", 15, 5, nullable: false),
			new DmoField("wrlReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("wrlReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("wrlHeatLot", "nvarchar", 50, 0, nullable: false),
			new DmoField("wrlClosed", "bit", 1, 0, nullable: false),
			new DmoField("wrlWarehouseRequisitionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wrlWarehouseRequisitionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("wrlWarehouseTransferID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wrlWarehouseTransferLineID", "smallint", 4, 0, nullable: false),
			new DmoField("wrlKitPart", "bit", 1, 0, nullable: false),
			new DmoField("wrlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("wrlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("wrlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("WRLWAREHOUSERECEIPTID,WRLWAREHOUSERECEIPTLINEID", unique: true),
			new DmoIndex("WRLUNIQUEID", unique: true),
			new DmoIndex("wrlWarehouseReceiptID", unique: false),
			new DmoIndex("wrlWarehouseReceiptLineID", unique: false),
			new DmoIndex("wrlPartID", unique: false),
			new DmoIndex("wrlPartRevisionID", unique: false),
			new DmoIndex("wrlReceivedComplete", unique: false),
			new DmoIndex("wrlWarehouseRequisitionID", unique: false),
			new DmoIndex("wrlWarehouseRequisitionLineID", unique: false),
			new DmoIndex("wrlWarehouseTransferID", unique: false),
			new DmoIndex("wrlWarehouseTransferLineID", unique: false),
			new DmoIndex("wrlKitPart", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
