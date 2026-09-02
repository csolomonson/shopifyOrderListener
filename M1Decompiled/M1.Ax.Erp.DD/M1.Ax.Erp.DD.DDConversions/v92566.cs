using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.566", "", "")]
public class v92566
{
	public v92566(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1GLFISCALYEARBUDGETAMOUNTSENTRY') and dgUserID <> ''");
	}
}
