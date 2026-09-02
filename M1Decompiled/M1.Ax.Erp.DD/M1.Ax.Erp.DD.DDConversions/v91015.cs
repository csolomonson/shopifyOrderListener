using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.015", "", "")]
public class v91015
{
	public v91015(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PARTTRANSACTIONSALL' and dgUserID <> ''");
	}
}
