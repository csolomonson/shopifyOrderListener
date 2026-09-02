using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Add Superannuation to Allowances/Deductions", "2011-12-06")]
public class v800205c
{
	public v800205c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoSuperannuation"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoSuperannuation", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Deductions", "padSuperannuation"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Deductions", "padSuperannuation", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeDeductions", "paeSuperannuation"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeDeductions", "paeSuperannuation", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
