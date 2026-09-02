using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.296", "", "")]
public class v92296
{
	public v92296(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1SUPPLIERSPPENTRY','M1PURCHASEPLANNERORDERDETAILSENTRY') and dgUserID <> ''");
	}
}
