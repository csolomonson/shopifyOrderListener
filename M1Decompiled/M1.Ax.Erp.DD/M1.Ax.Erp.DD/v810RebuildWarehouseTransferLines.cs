using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WarehouseTransferLines to support unicode", "2013-10-17")]
public class v810RebuildWarehouseTransferLines
{
	public v810RebuildWarehouseTransferLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferLines", new DmoField[20]
		{
			new DmoField("mwlWarehouseTransferID", "nvarchar", 10, 0, nullable: false),
			new DmoField("mwlWarehouseTransferLineID", "smallint", 4, 0, nullable: false),
			new DmoField("mwlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("mwlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("mwlUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("mwlPartDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("mwlWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("mwlPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("mwlShipQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("mwlShippedComplete", "bit", 1, 0, nullable: false),
			new DmoField("mwlReceivedQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("mwlReceivedDate", "date", 14, 0, nullable: true),
			new DmoField("mwlReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("mwlWarehouseRequisitionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("mwlWarehouseRequisitionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("mwlClosed", "bit", 1, 0, nullable: false),
			new DmoField("mwlKitPart", "bit", 1, 0, nullable: false),
			new DmoField("mwlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("mwlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("mwlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[14]
		{
			new DmoIndex("MWLWAREHOUSETRANSFERID,MWLWAREHOUSETRANSFERLINEID", unique: true),
			new DmoIndex("MWLUNIQUEID", unique: true),
			new DmoIndex("mwlWarehouseTransferID", unique: false),
			new DmoIndex("mwlWarehouseTransferLineID", unique: false),
			new DmoIndex("mwlPartID", unique: false),
			new DmoIndex("mwlPartRevisionID", unique: false),
			new DmoIndex("mwlWarehouseID", unique: false),
			new DmoIndex("mwlPartBinID", unique: false),
			new DmoIndex("mwlShippedComplete", unique: false),
			new DmoIndex("mwlReceivedComplete", unique: false),
			new DmoIndex("mwlWarehouseRequisitionID", unique: false),
			new DmoIndex("mwlWarehouseRequisitionLineID", unique: false),
			new DmoIndex("mwlClosed", unique: false),
			new DmoIndex("mwlKitPart", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
