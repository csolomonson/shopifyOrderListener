using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WorkFlowLines to support unicode", "2013-10-17")]
public class v810RebuildWorkFlowLines
{
	public v810RebuildWorkFlowLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlowLines", new DmoField[22]
		{
			new DmoField("wflWorkFlowID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wflWorkFlowLineID", "smallint", 4, 0, nullable: false),
			new DmoField("wflDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("wflAssignedToEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wflStartDate", "date", 14, 0, nullable: true),
			new DmoField("wflDueDate", "date", 14, 0, nullable: true),
			new DmoField("wflCompleted", "bit", 1, 0, nullable: false),
			new DmoField("wflCompletedDate", "date", 14, 0, nullable: true),
			new DmoField("wflParentID", "smallint", 4, 0, nullable: false),
			new DmoField("wflType", "tinyint", 1, 0, nullable: false),
			new DmoField("wflPercentComplete", "smallint", 3, 0, nullable: false),
			new DmoField("wflCode", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("wflSequence", "smallint", 4, 0, nullable: false),
			new DmoField("wflNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("wflNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("wflSequenceTask", "smallint", 4, 0, nullable: false),
			new DmoField("wflPriority", "nvarchar", 1, 0, nullable: false),
			new DmoField("wflStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("wflMilestone", "bit", 1, 0, nullable: false),
			new DmoField("wflCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("wflCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("wflUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[10]
		{
			new DmoIndex("WFLWORKFLOWID,WFLWORKFLOWLINEID", unique: true),
			new DmoIndex("WFLUNIQUEID", unique: true),
			new DmoIndex("wflWorkFlowID", unique: false),
			new DmoIndex("wflWorkFlowLineID", unique: false),
			new DmoIndex("wflAssignedToEmployeeID", unique: false),
			new DmoIndex("wflCompleted", unique: false),
			new DmoIndex("wflParentID", unique: false),
			new DmoIndex("wflType", unique: false),
			new DmoIndex("wflSequence", unique: false),
			new DmoIndex("wflSequenceTask", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
