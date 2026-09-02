using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert InventoryCountLines to support unicode", "2013-10-17")]
public class v810RebuildInventoryCountLines
{
	public v810RebuildInventoryCountLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InventoryCountLines", new DmoField[16]
		{
			new DmoField("imqInventoryCountID", "int", 9, 0, nullable: false),
			new DmoField("imqInventoryCountLineID", "int", 7, 0, nullable: false),
			new DmoField("imqPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imqPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imqPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imqPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imqBinDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imqQuantityOnHand", "numeric", 15, 5, nullable: false),
			new DmoField("imqFinalCount", "numeric", 15, 5, nullable: false),
			new DmoField("imqCountedBy", "nvarchar", 3, 0, nullable: false),
			new DmoField("imqCountedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imqPartClassID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imqPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imqCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imqCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("IMQINVENTORYCOUNTID,IMQINVENTORYCOUNTLINEID", unique: true),
			new DmoIndex("IMQUNIQUEID", unique: true),
			new DmoIndex("imqInventoryCountID", unique: false),
			new DmoIndex("imqInventoryCountLineID", unique: false),
			new DmoIndex("imqPartID", unique: false),
			new DmoIndex("imqPartRevisionID", unique: false),
			new DmoIndex("imqPartClassID", unique: false),
			new DmoIndex("imqPartShortDescription", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
