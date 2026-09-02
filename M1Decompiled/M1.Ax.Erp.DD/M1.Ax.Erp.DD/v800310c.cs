using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.310", "Add fields to PayrollNZYearSchedules table", "2015-05-19")]
public class v800310c
{
	public v800310c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PayrollNZYearSchedules"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYearSchedules", new DmoField[18]
			{
				new DmoField("nzsPayrollNZYearID", "smallint", 4, 0, nullable: false),
				new DmoField("nzsPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("nzsPayrollNZYearScheduleID", "smallint", 4, 0, nullable: false),
				new DmoField("nzsStartDate", "date", 14, 0, nullable: true),
				new DmoField("nzsEndDate", "date", 14, 0, nullable: true),
				new DmoField("nzsTotalPAYE", "numeric", 14, 2, nullable: false),
				new DmoField("nzsTotalChildSupport", "numeric", 14, 2, nullable: false),
				new DmoField("nzsTotalStudentLoan", "numeric", 14, 2, nullable: false),
				new DmoField("nzsTotalKiwiSaver", "numeric", 14, 2, nullable: false),
				new DmoField("nzsTotalKiwiSaverEmployer", "numeric", 14, 2, nullable: false),
				new DmoField("nzsTotalTaxCredits", "numeric", 14, 2, nullable: false),
				new DmoField("nzsTotalFamilyTaxCredits", "numeric", 14, 2, nullable: false),
				new DmoField("nzsTotalGrossEarnings", "numeric", 14, 2, nullable: false),
				new DmoField("nzsTotalEarningsNotLiableForEL", "numeric", 14, 2, nullable: false),
				new DmoField("nzsClosed", "bit", 1, 0, nullable: false),
				new DmoField("nzsCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("nzsCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("nzsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[5]
			{
				new DmoIndex("NZSPAYROLLNZYEARID,NZSPLANTID,NZSPAYROLLNZYEARSCHEDULEID", unique: true),
				new DmoIndex("NZSUNIQUEID", unique: true),
				new DmoIndex("nzsPayrollNZYearID", unique: false),
				new DmoIndex("nzsPlantID", unique: false),
				new DmoIndex("nzsPayrollNZYearScheduleID", unique: false)
			});
		}
	}
}
