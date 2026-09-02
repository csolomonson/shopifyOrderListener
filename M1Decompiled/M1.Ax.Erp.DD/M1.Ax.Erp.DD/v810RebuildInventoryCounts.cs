using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert InventoryCounts to support unicode", "2013-10-17")]
public class v810RebuildInventoryCounts
{
	public v810RebuildInventoryCounts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InventoryCounts", new DmoField[17]
		{
			new DmoField("imnInventoryCountID", "int", 9, 0, nullable: false),
			new DmoField("imnCycleCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imnPartClassIDs", "nvarchar(max)", 4, 0, nullable: true),
			new DmoField("imnPartWarehouseIDs", "nvarchar(max)", 4, 0, nullable: true),
			new DmoField("imnIncludeBlankWarehouse", "bit", 1, 0, nullable: false),
			new DmoField("imnIncludeBlankPartClass", "bit", 1, 0, nullable: false),
			new DmoField("imnPartGroupIDs", "nvarchar(max)", 4, 0, nullable: true),
			new DmoField("imnIncludeBlankPartGroup", "bit", 1, 0, nullable: false),
			new DmoField("imnSupplierOrganizationIDs", "nvarchar(max)", 4, 0, nullable: true),
			new DmoField("imnRecordsGenerated", "bit", 1, 0, nullable: false),
			new DmoField("imnGeneratedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imnStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("imnPostedToInventory", "bit", 1, 0, nullable: false),
			new DmoField("imnPostedDate", "date", 14, 0, nullable: true),
			new DmoField("imnCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imnCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imnUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("IMNINVENTORYCOUNTID", unique: true),
			new DmoIndex("IMNUNIQUEID", unique: true),
			new DmoIndex("imnCycleCodeID", unique: false),
			new DmoIndex("imnStatus", unique: false),
			new DmoIndex("imnPostedToInventory", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
