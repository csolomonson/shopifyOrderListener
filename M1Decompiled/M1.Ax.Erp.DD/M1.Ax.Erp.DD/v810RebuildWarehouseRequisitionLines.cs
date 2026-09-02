using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WarehouseRequisitionLines to support unicode", "2013-10-17")]
public class v810RebuildWarehouseRequisitionLines
{
	public v810RebuildWarehouseRequisitionLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseRequisitionLines", new DmoField[16]
		{
			new DmoField("wqlWarehouseRequisitionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wqlWarehouseRequisitionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("wqlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("wqlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("wqlUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("wqlPartDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("wqlWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wqlPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("wqlRequestedQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("wqlClosed", "bit", 1, 0, nullable: false),
			new DmoField("wqlQuantityTransferred", "numeric", 15, 5, nullable: false),
			new DmoField("wqlTransferredComplete", "bit", 1, 0, nullable: false),
			new DmoField("wqlKitPart", "bit", 1, 0, nullable: false),
			new DmoField("wqlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("wqlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("wqlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("WQLWAREHOUSEREQUISITIONID,WQLWAREHOUSEREQUISITIONLINEID", unique: true),
			new DmoIndex("WQLUNIQUEID", unique: true),
			new DmoIndex("wqlWarehouseRequisitionID", unique: false),
			new DmoIndex("wqlWarehouseRequisitionLineID", unique: false),
			new DmoIndex("wqlPartID", unique: false),
			new DmoIndex("wqlPartRevisionID", unique: false),
			new DmoIndex("wqlWarehouseID", unique: false),
			new DmoIndex("wqlPartBinID", unique: false),
			new DmoIndex("wqlClosed", unique: false),
			new DmoIndex("wqlTransferredComplete", unique: false),
			new DmoIndex("wqlKitPart", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
