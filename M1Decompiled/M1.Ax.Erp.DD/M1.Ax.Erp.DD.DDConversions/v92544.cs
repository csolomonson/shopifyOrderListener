using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.544", "", "")]
public class v92544
{
	public v92544(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1LOOKUPBIN') and dgUserID <> ''");
	}
}
