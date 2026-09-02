using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.050", "Add Salary Sacrifice to Deductions, EmployeeDeduct", "2008-03-04")]
public class v700050
{
	public v700050(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Deductions", "padSalarySacrifice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Deductions", "padSalarySacrifice", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeDeductions", "paeSalarySacrifice"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeDeductions", "paeSalarySacrifice", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
