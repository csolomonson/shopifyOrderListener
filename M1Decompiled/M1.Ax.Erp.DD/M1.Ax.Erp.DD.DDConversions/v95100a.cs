using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.5.100", "Remove default grids from DDGridDetails to be upgraded", "2021-11-03")]
public class v95100a
{
	public v95100a(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1MRPSESSIONSALL') and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1MRPSESSIONSENTRY') and dgUserID <> ''");
	}
}
