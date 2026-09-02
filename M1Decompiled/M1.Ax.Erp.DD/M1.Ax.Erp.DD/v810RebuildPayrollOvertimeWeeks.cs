using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollOvertimeWeeks to support unicode", "2013-10-17")]
public class v810RebuildPayrollOvertimeWeeks
{
	public v810RebuildPayrollOvertimeWeeks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollOvertimeWeeks", new DmoField[16]
		{
			new DmoField("lmwPayrollDefinitionID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmwPayrollWeekID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmwDayOfWeek", "tinyint", 1, 0, nullable: false),
			new DmoField("lmwStandardHours", "numeric", 6, 2, nullable: false),
			new DmoField("lmwOvertimePeriod1", "numeric", 6, 2, nullable: false),
			new DmoField("lmwOvertimePeriod2", "numeric", 6, 2, nullable: false),
			new DmoField("lmwOvertimePeriod3", "numeric", 6, 2, nullable: false),
			new DmoField("lmwOvertimePeriod4", "numeric", 6, 2, nullable: false),
			new DmoField("lmwBreakPeriod1", "numeric", 6, 2, nullable: false),
			new DmoField("lmwBreakPeriod2", "numeric", 6, 2, nullable: false),
			new DmoField("lmwBreakPeriod3", "numeric", 6, 2, nullable: false),
			new DmoField("lmwBreakPeriod4", "numeric", 6, 2, nullable: false),
			new DmoField("lmwRDORate", "numeric", 6, 2, nullable: false),
			new DmoField("lmwCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmwCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmwUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("LMWPAYROLLDEFINITIONID,LMWPAYROLLWEEKID", unique: true),
			new DmoIndex("LMWUNIQUEID", unique: true),
			new DmoIndex("lmwPayrollDefinitionID", unique: false),
			new DmoIndex("lmwPayrollWeekID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
