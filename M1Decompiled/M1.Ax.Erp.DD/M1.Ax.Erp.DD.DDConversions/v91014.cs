using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.014", "", "")]
public class v91014
{
	public v91014(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1CLEARINGHOUSEEXPORTSUNSUPER' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMSOQUOTE' and dgUserID <> ''");
	}
}
