using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollNZYearScheduleLines to support unicode", "2015-10-08")]
public class v810RebuildPayrollNZYearScheduleLines
{
	public v810RebuildPayrollNZYearScheduleLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYearScheduleLines", new DmoField[26]
		{
			new DmoField("nzlPayrollNZYearID", "smallint", 4, 0, nullable: false),
			new DmoField("nzlPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("nzlPayrollNZYearScheduleID", "smallint", 4, 0, nullable: false),
			new DmoField("nzlPayrollNZYearScheduleLineID", "smallint", 4, 0, nullable: false),
			new DmoField("nzlEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("nzlEmployeeIRDNumber", "nvarchar", 11, 0, nullable: false),
			new DmoField("nzlEmployeeName", "nvarchar", 20, 0, nullable: false),
			new DmoField("nzlEmployeeTaxCode", "nvarchar", 5, 0, nullable: false),
			new DmoField("nzlStartDate", "date", 14, 0, nullable: true),
			new DmoField("nzlEndDate", "date", 14, 0, nullable: true),
			new DmoField("nzlGrossEarnings", "numeric", 14, 2, nullable: false),
			new DmoField("nzlEarningsNotLiableForEL", "numeric", 14, 2, nullable: false),
			new DmoField("nzlLumpSumIndicator", "bit", 1, 0, nullable: false),
			new DmoField("nzlPAYE", "numeric", 14, 2, nullable: false),
			new DmoField("nzlChildSupport", "numeric", 14, 2, nullable: false),
			new DmoField("nzlChildSupportCode", "nvarchar", 1, 0, nullable: false),
			new DmoField("nzlStudentLoan", "numeric", 14, 2, nullable: false),
			new DmoField("nzlKiwiSaver", "numeric", 14, 2, nullable: false),
			new DmoField("nzlKiwiSaverEmployer", "numeric", 14, 2, nullable: false),
			new DmoField("nzlTaxCredits", "numeric", 14, 2, nullable: false),
			new DmoField("nzlFamilyTaxCredits", "numeric", 14, 2, nullable: false),
			new DmoField("nzlClosed", "bit", 1, 0, nullable: false),
			new DmoField("nzlStudentLoanType", "tinyint", 1, 0, nullable: false),
			new DmoField("nzlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("nzlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("nzlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("NZLPAYROLLNZYEARID,NZLPLANTID,NZLPAYROLLNZYEARSCHEDULEID,NZLPAYROLLNZYEARSCHEDULELINEID", unique: true),
			new DmoIndex("NZLUNIQUEID", unique: true),
			new DmoIndex("nzlPayrollNZYearID", unique: false),
			new DmoIndex("nzlPlantID", unique: false),
			new DmoIndex("nzlPayrollNZYearScheduleID", unique: false),
			new DmoIndex("nzlPayrollNZYearScheduleLineID", unique: false),
			new DmoIndex("nzlEmployeeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
