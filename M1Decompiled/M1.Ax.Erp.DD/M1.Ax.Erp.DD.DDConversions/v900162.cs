using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.162", "", "")]
public class v900162
{
	public v900162(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1EMPLOYEESALESBUDGETSALL' and dgUserID <> ''");
	}
}
