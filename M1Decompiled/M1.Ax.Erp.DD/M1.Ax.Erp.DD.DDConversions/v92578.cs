using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.578", "", "")]
public class v92578
{
	public v92578(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1PURCHASEPLANNERORDERDETAILSENTRY') and dgUserID <> ''");
	}
}
