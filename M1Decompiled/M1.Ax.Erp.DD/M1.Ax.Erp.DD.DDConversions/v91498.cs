using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.498", "", "")]
public class v91498
{
	public v91498(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1SHIPMENTPACKAGESENTRY') and dgUserID <> ''");
	}
}
