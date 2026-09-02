using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.065", "", "")]
public class v92065c
{
	public v92065c(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1WAREHOUSEBINSALL' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1WAREHOUSEBINSENTRY' and dgUserID <> ''");
	}
}
