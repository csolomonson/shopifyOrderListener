using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LeaveAccrualLines to support unicode", "2013-10-17")]
public class v810RebuildLeaveAccrualLines
{
	public v810RebuildLeaveAccrualLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LeaveAccrualLines", new DmoField[9]
		{
			new DmoField("pakLeaveAccrualID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pakLeaveAccrualLineID", "smallint", 4, 0, nullable: false),
			new DmoField("pakYearsOver", "tinyint", 2, 0, nullable: false),
			new DmoField("pakYearsNotOver", "tinyint", 2, 0, nullable: false),
			new DmoField("pakHoursEarned", "numeric", 7, 3, nullable: false),
			new DmoField("pakCarryoverMaximum", "numeric", 8, 3, nullable: false),
			new DmoField("pakCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pakCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pakUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("PAKLEAVEACCRUALID,PAKLEAVEACCRUALLINEID", unique: true),
			new DmoIndex("PAKUNIQUEID", unique: true),
			new DmoIndex("pakLeaveAccrualID", unique: false),
			new DmoIndex("pakLeaveAccrualLineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
