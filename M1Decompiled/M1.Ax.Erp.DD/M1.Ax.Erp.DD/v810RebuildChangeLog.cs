using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ChangeLog to support unicode", "2013-10-17")]
public class v810RebuildChangeLog
{
	public v810RebuildChangeLog(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ChangeLog", new DmoField[9]
		{
			new DmoField("xagChangeLogID", "identity", 4, 0, nullable: false),
			new DmoField("xagChangeType", "nvarchar", 1, 0, nullable: false),
			new DmoField("xagTableName", "nvarchar", 30, 0, nullable: false),
			new DmoField("xagTableUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("xagTableKeyValues", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xagTableOldValues", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xagTableNewValues", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xagChangeDate", "datetime", 14, 0, nullable: true),
			new DmoField("xagChangeUserID", "nvarchar", 20, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("XAGCHANGELOGID", unique: true),
			new DmoIndex("xagChangeType", unique: false),
			new DmoIndex("xagTableName", unique: false),
			new DmoIndex("xagTableUniqueID", unique: false),
			new DmoIndex("xagChangeDate", unique: false),
			new DmoIndex("xagChangeUserID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
