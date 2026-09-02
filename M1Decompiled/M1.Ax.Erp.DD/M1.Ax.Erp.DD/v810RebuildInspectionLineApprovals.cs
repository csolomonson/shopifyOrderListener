using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert InspectionLineApprovals to support unicode", "2013-10-17")]
public class v810RebuildInspectionLineApprovals
{
	public v810RebuildInspectionLineApprovals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionLineApprovals", new DmoField[10]
		{
			new DmoField("qaaInspectionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qaaInspectionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("qaaApprovalEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qaaInspectionLineApprovalID", "tinyint", 2, 0, nullable: false),
			new DmoField("qaaStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("qaaDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qaaStatusDate", "datetime", 14, 0, nullable: true),
			new DmoField("qaaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qaaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qaaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("QAAINSPECTIONID,QAAINSPECTIONLINEID,QAAAPPROVALEMPLOYEEID,QAAINSPECTIONLINEAPPROVALID", unique: true),
			new DmoIndex("QAAUNIQUEID", unique: true),
			new DmoIndex("qaaInspectionID", unique: false),
			new DmoIndex("qaaInspectionLineID", unique: false),
			new DmoIndex("qaaApprovalEmployeeID", unique: false),
			new DmoIndex("qaaInspectionLineApprovalID", unique: false),
			new DmoIndex("qaaStatus", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
