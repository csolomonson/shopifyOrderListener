using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Add Employee Sales Budget tables", "2011-12-06")]
public class v800205b
{
	public v800205b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "EmployeeSalesBudgets"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeSalesBudgets");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "EmployeeSalesBudgetLines"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeSalesBudgetLines");
		}
	}
}
