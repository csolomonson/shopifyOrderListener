using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.6.100", "Remove part bin entry grids", "2022-11-20")]
public class v96100
{
	public v96100(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1PARTBINSENTRY') and dgUserID <> ''");
	}
}
