using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartFindAndReplaceLog to support unicode", "2013-10-17")]
public class v810RebuildPartFindAndReplaceLog
{
	public v810RebuildPartFindAndReplaceLog(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartFindAndReplaceLog", new DmoField[9]
		{
			new DmoField("abgPartFindAndReplaceLogID", "int", 9, 0, nullable: false),
			new DmoField("abgFindPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("abgFindPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("abgReplacePartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("abgReplacePartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("abgReplaceDate", "datetime", 14, 0, nullable: true),
			new DmoField("abgReplaceUserID", "nvarchar", 20, 0, nullable: false),
			new DmoField("abgFindProcessID", "nvarchar", 5, 0, nullable: false),
			new DmoField("abgReplaceProcessID", "nvarchar", 5, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("ABGPARTFINDANDREPLACELOGID", unique: true),
			new DmoIndex("abgFindPartID", unique: false),
			new DmoIndex("abgFindPartRevisionID", unique: false),
			new DmoIndex("abgReplacePartID", unique: false),
			new DmoIndex("abgReplacePartRevisionID", unique: false),
			new DmoIndex("abgReplaceDate", unique: false),
			new DmoIndex("abgReplaceUserID", unique: false),
			new DmoIndex("abgFindProcessID", unique: false),
			new DmoIndex("abgReplaceProcessID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
