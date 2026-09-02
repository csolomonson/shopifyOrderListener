using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.622", "", "")]
public class v92622
{
	public v92622(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1CLEARINGHOUSEEXPORTLUCRFSUPER', 'M1CLEARINGHOUSEEXPORTQUICKSUPER','M1CLEARINGHOUSEEXPORTSUNSUPER') and dgUserID <> ''");
	}
}
