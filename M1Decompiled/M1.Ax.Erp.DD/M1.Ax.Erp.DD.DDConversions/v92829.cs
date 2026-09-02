using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.829", "", "")]
public class v92829
{
	public v92829(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PURCHASEPLANNERSUMMARY' and dgUserID <> ''");
	}
}
