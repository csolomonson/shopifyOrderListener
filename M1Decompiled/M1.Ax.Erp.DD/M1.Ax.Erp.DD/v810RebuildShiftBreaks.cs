using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ShiftBreaks to support unicode", "2013-10-17")]
public class v810RebuildShiftBreaks
{
	public v810RebuildShiftBreaks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShiftBreaks", new DmoField[16]
		{
			new DmoField("lmtShiftID", "smallint", 3, 0, nullable: false),
			new DmoField("lmtDay", "tinyint", 1, 0, nullable: false),
			new DmoField("lmtStartTime", "numeric", 5, 2, nullable: false),
			new DmoField("lmtEndTime", "numeric", 5, 2, nullable: false),
			new DmoField("lmtBreak1StartTime", "numeric", 5, 2, nullable: false),
			new DmoField("lmtBreak1EndTime", "numeric", 5, 2, nullable: false),
			new DmoField("lmtBreak1Paid", "bit", 1, 0, nullable: false),
			new DmoField("lmtBreak2StartTime", "numeric", 5, 2, nullable: false),
			new DmoField("lmtBreak2EndTime", "numeric", 5, 2, nullable: false),
			new DmoField("lmtBreak2Paid", "bit", 1, 0, nullable: false),
			new DmoField("lmtBreak3StartTime", "numeric", 5, 2, nullable: false),
			new DmoField("lmtBreak3EndTime", "numeric", 5, 2, nullable: false),
			new DmoField("lmtBreak3Paid", "bit", 1, 0, nullable: false),
			new DmoField("lmtCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmtCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmtUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("LMTSHIFTID,LMTDAY", unique: true),
			new DmoIndex("LMTUNIQUEID", unique: true),
			new DmoIndex("lmtShiftID", unique: false),
			new DmoIndex("lmtDay", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
