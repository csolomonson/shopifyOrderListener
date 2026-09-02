using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.563", "", "")]
public class v92563
{
	public v92563(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1SALESORDERSBYSALESPERSON') and dgUserID <> ''");
	}
}
