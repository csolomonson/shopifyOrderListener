using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WorkFlows to support unicode", "2013-10-17")]
public class v810RebuildWorkFlows
{
	public v810RebuildWorkFlows(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkFlows", new DmoField[9]
		{
			new DmoField("wfpWorkFlowID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wfpDescription", "nvarchar", 30, 0, nullable: false),
			new DmoField("wfpLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("wfpLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("wfpUserID", "nvarchar", 20, 0, nullable: false),
			new DmoField("wfpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("wfpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("wfpUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("wfpJobId", "nvarchar", 20, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("WFPWORKFLOWID", unique: true),
			new DmoIndex("WFPUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
