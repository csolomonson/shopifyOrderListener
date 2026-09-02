using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.291", "", "")]
public class v92291
{
	public v92291(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1SHIPMENTPACKAGESENTRY') and dgUserID <> ''");
	}
}
