using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ImplementationCheckList to support unicode", "2013-10-17")]
public class v810RebuildImplementationCheckList
{
	public v810RebuildImplementationCheckList(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ImplementationCheckList", new DmoField[13]
		{
			new DmoField("xicCompleted", "bit", 1, 0, nullable: false),
			new DmoField("xicImplementationCheckListID", "smallint", 4, 0, nullable: false),
			new DmoField("xicParentID", "smallint", 4, 0, nullable: false),
			new DmoField("xicTask", "nvarchar", 50, 0, nullable: false),
			new DmoField("xicStartDate", "date", 14, 0, nullable: true),
			new DmoField("xicDueDate", "date", 14, 0, nullable: true),
			new DmoField("xicPercentDone", "smallint", 3, 0, nullable: false),
			new DmoField("xicAssignedTo", "nvarchar", 30, 0, nullable: false),
			new DmoField("xicAction", "nvarchar(max)", 10, 0, nullable: true),
			new DmoField("xicSequence", "smallint", 3, 0, nullable: false),
			new DmoField("xicCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xicCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xicUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("xicImplementationCheckListID", unique: true),
			new DmoIndex("XICUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
