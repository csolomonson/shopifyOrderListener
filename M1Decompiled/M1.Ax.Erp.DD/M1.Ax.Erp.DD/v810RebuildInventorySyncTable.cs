using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert InventorySyncTable to support unicode", "2013-10-17")]
public class v810RebuildInventorySyncTable
{
	public v810RebuildInventorySyncTable(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InventorySyncTable", new DmoField[9]
		{
			new DmoField("imyInventoryCountID", "int", 9, 0, nullable: false),
			new DmoField("imyInventoryCountLineID", "int", 7, 0, nullable: false),
			new DmoField("imyPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imyPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imyPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imyQuantityCounted", "numeric", 12, 2, nullable: false),
			new DmoField("imyPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imyPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imyCounteddate", "nvarchar", 20, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("IMYINVENTORYCOUNTLINEID", unique: true),
			new DmoIndex("imyInventoryCountID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
