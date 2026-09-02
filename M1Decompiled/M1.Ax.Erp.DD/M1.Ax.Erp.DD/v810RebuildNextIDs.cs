using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert NextIDs to support unicode", "2013-10-17")]
public class v810RebuildNextIDs
{
	public v810RebuildNextIDs(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "NextIDs", new DmoField[10]
		{
			new DmoField("xanTable", "nvarchar", 30, 0, nullable: false),
			new DmoField("xanNextID", "nvarchar", 30, 0, nullable: false),
			new DmoField("xanAutoIncrement", "tinyint", 1, 0, nullable: false),
			new DmoField("xanIncrementAmount", "smallint", 3, 0, nullable: false),
			new DmoField("xanNumericOnly", "tinyint", 1, 0, nullable: false),
			new DmoField("xanLogChanges", "tinyint", 1, 0, nullable: false),
			new DmoField("xanDatasets", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xanCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xanCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xanUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("XANTABLE", unique: true),
			new DmoIndex("XANUNIQUEID", unique: true),
			new DmoIndex("xanLogChanges", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
