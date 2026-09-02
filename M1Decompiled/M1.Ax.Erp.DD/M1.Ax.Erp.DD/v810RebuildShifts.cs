using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Shifts to support unicode", "2013-10-17")]
public class v810RebuildShifts
{
	public v810RebuildShifts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Shifts", new DmoField[23]
		{
			new DmoField("lmsShiftID", "smallint", 3, 0, nullable: false),
			new DmoField("lmsDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmsShiftGroup", "tinyint", 1, 0, nullable: false),
			new DmoField("lmsClockInWindow", "smallint", 3, 0, nullable: false),
			new DmoField("lmsClockOutWindow", "smallint", 3, 0, nullable: false),
			new DmoField("lmsRoundTo", "tinyint", 2, 0, nullable: false),
			new DmoField("lmsRoundClockInDirection", "nvarchar", 1, 0, nullable: false),
			new DmoField("lmsRoundClockOutDirection", "nvarchar", 1, 0, nullable: false),
			new DmoField("lmsRoundClockWithInShift", "bit", 1, 0, nullable: false),
			new DmoField("lmsRoundJobsWithinShift", "bit", 1, 0, nullable: false),
			new DmoField("lmsGraceTimeOut", "smallint", 3, 0, nullable: false),
			new DmoField("lmsGraceTimeIn", "smallint", 3, 0, nullable: false),
			new DmoField("lmsIdleTimeWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmsIdleTimeIndirectLaborID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmsInactive", "bit", 1, 0, nullable: false),
			new DmoField("lmsInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("lmsRoundOutsideOfShift", "bit", 1, 0, nullable: false),
			new DmoField("lmsRoundJobsOutsideOfShift", "bit", 1, 0, nullable: false),
			new DmoField("lmsAutoClockOutLastRunTime", "date", 14, 0, nullable: true),
			new DmoField("lmsAutoClockOutTime", "numeric", 5, 2, nullable: false),
			new DmoField("lmsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("LMSSHIFTID", unique: true),
			new DmoIndex("LMSUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
