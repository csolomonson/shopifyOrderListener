using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.036", "Add AUS Reportable Percent to EmployeeAllowances", "2010-05-24")]
public class v800036b
{
	public v800036b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeAllowances", "pawAUSReportablePercent"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeAllowances", "pawAUSReportablePercent", "numeric", 8, 4, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollLines", "panAUSReportableAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollLines", "panAUSReportableAmount", "money", 10, 2, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
