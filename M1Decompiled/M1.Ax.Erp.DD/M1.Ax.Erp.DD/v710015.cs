using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.015", "Remove Payroll Categories table", "2008-06-19")]
public class v710015
{
	public v710015(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PayrollCategories"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "PayrollCategories");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollRates", "payPayrollCategoryID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollRates", "payPayrollCategoryID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxTypes", "pafPayrollCategoryID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTypes", "pafPayrollCategoryID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoPayrollCategoryID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoPayrollCategoryID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Deductions", "padPayrollCategoryID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Deductions", "padPayrollCategoryID", dropTriggers: true);
		}
	}
}
