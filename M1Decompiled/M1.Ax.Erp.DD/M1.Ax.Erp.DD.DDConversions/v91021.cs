using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.021", "", "")]
public class v91021
{
	public v91021(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMWHRECEIPTWHTRANSFER' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMWHTRANSFERWHREQ' and dgUserID <> ''");
	}
}
