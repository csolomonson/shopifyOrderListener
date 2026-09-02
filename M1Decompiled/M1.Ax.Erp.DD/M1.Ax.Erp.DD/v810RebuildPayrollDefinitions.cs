using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollDefinitions to support unicode", "2013-10-17")]
public class v810RebuildPayrollDefinitions
{
	public v810RebuildPayrollDefinitions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollDefinitions", new DmoField[30]
		{
			new DmoField("lmrPayrollDefinitionID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmrPayFrequency", "tinyint", 1, 0, nullable: false),
			new DmoField("lmrCalculationType", "tinyint", 1, 0, nullable: false),
			new DmoField("lmrStartDayOfWeek", "tinyint", 1, 0, nullable: false),
			new DmoField("lmrStandardHours", "numeric", 8, 2, nullable: false),
			new DmoField("lmrStandardPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrLeaveCalculationType", "tinyint", 1, 0, nullable: false),
			new DmoField("lmrShowTimecardsOnPaySlip", "bit", 1, 0, nullable: false),
			new DmoField("lmrOTPeriod1PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrOTPeriod2PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrOTPeriod3PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrOTPeriod4PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrPublicHolidayPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrPublicHolidayLeaveAccrualID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrCreateHolidayTimecards", "bit", 1, 0, nullable: false),
			new DmoField("lmrBPeriod1PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrBPeriod2PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrBPeriod3PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrBPeriod4PayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmrCalcBeforeShift", "bit", 1, 0, nullable: false),
			new DmoField("lmrLastPeriodStartDate", "date", 14, 0, nullable: true),
			new DmoField("lmrLastPeriodEndDate", "date", 14, 0, nullable: true),
			new DmoField("lmrCountHolidaysAsStandardTime", "bit", 1, 0, nullable: false),
			new DmoField("lmrCalcRDOProportion", "bit", 1, 0, nullable: false),
			new DmoField("lmrComboStdWeekHours", "numeric", 8, 2, nullable: false),
			new DmoField("lmrTotalHours", "numeric", 7, 2, nullable: false),
			new DmoField("lmrCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmrCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("LMRPAYROLLDEFINITIONID", unique: true),
			new DmoIndex("LMRUNIQUEID", unique: true),
			new DmoIndex("lmrPublicHolidayLeaveAccrualID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
