using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.011", "", "")]
public class v91011
{
	public v91011(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1LOOKUPWORKCENTERS' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ADDFROMSOQUOTE' and dgUserID <> ''");
	}
}
