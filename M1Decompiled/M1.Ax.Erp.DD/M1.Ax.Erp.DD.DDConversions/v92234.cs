using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.234", "", "")]
public class v92234
{
	public v92234(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1LOOKUPTARIFFS','M1TARIFFSALL') and dgUserID <> ''");
	}
}
