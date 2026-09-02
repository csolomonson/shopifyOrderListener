using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Priorities to support unicode", "2013-10-17")]
public class v810RebuildPriorities
{
	public v810RebuildPriorities(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Priorities", new DmoField[5]
		{
			new DmoField("kbrPriorityID", "tinyint", 2, 0, nullable: false),
			new DmoField("kbrDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("kbrCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbrCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("KBRPRIORITYID", unique: true),
			new DmoIndex("KBRUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
