using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.040", "", "")]
public class v91040
{
	public v91040(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1EMPLOYEESALESBUDGETSALL' and dgUserID <> ''");
	}
}
