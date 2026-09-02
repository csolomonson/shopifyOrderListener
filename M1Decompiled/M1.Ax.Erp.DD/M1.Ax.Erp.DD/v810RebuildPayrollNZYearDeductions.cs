using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollNZYearDeductions to support unicode", "2015-10-08")]
public class v810RebuildPayrollNZYearDeductions
{
	public v810RebuildPayrollNZYearDeductions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYearDeductions", new DmoField[16]
		{
			new DmoField("nzdPayrollNZYearID", "smallint", 4, 0, nullable: false),
			new DmoField("nzdPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("nzdPayrollNZYearDeductionID", "smallint", 4, 0, nullable: false),
			new DmoField("nzdStartDate", "date", 14, 0, nullable: false),
			new DmoField("nzdEndDate", "date", 14, 0, nullable: false),
			new DmoField("nzdTotalPAYE", "numeric", 13, 2, nullable: false),
			new DmoField("nzdTotalChildSupport", "numeric", 13, 2, nullable: false),
			new DmoField("nzdTotalStudentLoan", "numeric", 13, 2, nullable: false),
			new DmoField("nzdTotalKiwiSaver", "numeric", 13, 2, nullable: false),
			new DmoField("nzdTotalKiwiSaverEmployer", "numeric", 13, 2, nullable: false),
			new DmoField("nzdTotalESCT", "numeric", 13, 2, nullable: false),
			new DmoField("nzdTotalAmountsPayable", "numeric", 13, 2, nullable: false),
			new DmoField("nzdClosed", "bit", 1, 0, nullable: false),
			new DmoField("nzdCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("nzdCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("nzdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("NZDPAYROLLNZYEARID,NZDPLANTID,NZDPAYROLLNZYEARDEDUCTIONID", unique: true),
			new DmoIndex("NZDUNIQUEID", unique: true),
			new DmoIndex("nzdPayrollNZYearID", unique: false),
			new DmoIndex("nzdPlantID", unique: false),
			new DmoIndex("nzdPayrollNZYearDeductionID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
