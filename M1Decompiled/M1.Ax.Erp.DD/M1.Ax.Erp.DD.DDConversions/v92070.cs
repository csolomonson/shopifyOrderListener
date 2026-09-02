using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.070", "", "")]
public class v92070
{
	public v92070(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMWHRECEIPTWHTRANSFER' and dgUserID <> ''");
	}
}
