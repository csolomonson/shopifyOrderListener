using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Update Employee Management to HR Management in ImplementationCheckList", "2015-08-13")]
public class v900074p
{
	public v900074p(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ImplementationCheckList"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicTask = Replace(xicTask, 'Employee', 'HR') Where xicTask Like '%Employee%' And xicParentID = 0");
		}
	}
}
