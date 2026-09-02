using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.4.200", "Remove default grids from DDGridDetails to be upgraded", "2021-09-09")]
public class v94200a
{
	public v94200a(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1SHIPMENTPACKAGESENTRY') and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1SHIPMENTPACKAGESALL') and dgUserID <> ''");
	}
}
