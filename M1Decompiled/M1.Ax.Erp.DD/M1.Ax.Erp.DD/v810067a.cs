using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.067", "Remove fields from GLFiscalYearBudgetAmounts table", "2014-01-29")]
public class v810067a
{
	public v810067a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLFiscalYearBudgetAmounts", "glbGLAccountID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLFiscalYearBudgetAmounts", "glbGLAccountID", dropTriggers: true);
		}
	}
}
