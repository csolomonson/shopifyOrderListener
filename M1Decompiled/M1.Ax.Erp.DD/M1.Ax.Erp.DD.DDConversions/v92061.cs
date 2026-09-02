using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.061", "", "")]
public class v92061
{
	public v92061(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PARTBINSENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PARTWAREHOUSELOCATIONENTRY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMWHRECEIPTWHTRANSFER' and dgUserID <> ''");
	}
}
