using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert JobPriorities to support unicode", "2013-10-17")]
public class v810RebuildJobPriorities
{
	public v810RebuildJobPriorities(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobPriorities", new DmoField[3]
		{
			new DmoField("jmjJobPriorityID", "tinyint", 2, 0, nullable: false),
			new DmoField("jmjDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("jmjUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("JMJJOBPRIORITYID", unique: true),
			new DmoIndex("JMJUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
