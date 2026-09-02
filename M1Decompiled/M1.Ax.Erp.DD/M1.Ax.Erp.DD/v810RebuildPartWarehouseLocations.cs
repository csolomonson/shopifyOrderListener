using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartWarehouseLocations to support unicode", "2013-10-17")]
public class v810RebuildPartWarehouseLocations
{
	public v810RebuildPartWarehouseLocations(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartWarehouseLocations", new DmoField[10]
		{
			new DmoField("imlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imlPartWarehouseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imlMinimumQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("imlMaximumQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("imlQuantityInTransit", "numeric", 15, 5, nullable: false),
			new DmoField("imlNonNettable", "bit", 1, 0, nullable: false),
			new DmoField("imlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("IMLPARTID,IMLPARTREVISIONID,IMLPARTWAREHOUSEID", unique: true),
			new DmoIndex("IMLUNIQUEID", unique: true),
			new DmoIndex("imlPartID", unique: false),
			new DmoIndex("imlPartRevisionID", unique: false),
			new DmoIndex("imlPartWarehouseID", unique: false),
			new DmoIndex("imlNonNettable", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
