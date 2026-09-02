using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LeaveAccruals to support unicode", "2013-10-17")]
public class v810RebuildLeaveAccruals
{
	public v810RebuildLeaveAccruals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LeaveAccruals", new DmoField[21]
		{
			new DmoField("pajLeaveAccrualID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pajLeaveDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pajLeaveType", "nvarchar", 1, 0, nullable: false),
			new DmoField("pajCalculationMethod", "tinyint", 2, 0, nullable: false),
			new DmoField("pajAwardMethod", "tinyint", 2, 0, nullable: false),
			new DmoField("pajPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pajColor", "int", 8, 0, nullable: false),
			new DmoField("pajIndirectLaborID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pajPrintOnPaySlip", "bit", 1, 0, nullable: false),
			new DmoField("pajIncludeUnawardedOnPaySlip", "bit", 1, 0, nullable: false),
			new DmoField("pajRDOAccrual", "bit", 1, 0, nullable: false),
			new DmoField("pajLeaveTimecardHours", "numeric", 8, 2, nullable: false),
			new DmoField("pajCountLeaveAsStandardTime", "bit", 1, 0, nullable: false),
			new DmoField("pajIncludeRDOAsTaken", "bit", 1, 0, nullable: false),
			new DmoField("pajCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pajCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pajUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("pajDelayByMonth", "tinyint", 2, 0, nullable: false),
			new DmoField("pajAccrueUpfront", "bit", 1, 0, nullable: false),
			new DmoField("pajDelayAccrual", "bit", 1, 0, nullable: false),
			new DmoField("pajIncludeUnpaidTimeOffInCalc", "bit", 1, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("PAJLEAVEACCRUALID", unique: true),
			new DmoIndex("PAJUNIQUEID", unique: true),
			new DmoIndex("pajLeaveType", unique: false),
			new DmoIndex("pajPayrollRateID", unique: false),
			new DmoIndex("pajIndirectLaborID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
