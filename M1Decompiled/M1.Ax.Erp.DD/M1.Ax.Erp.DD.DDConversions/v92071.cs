using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.071", "", "")]
public class v92071
{
	public v92071(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1PARTWAREHOUSELOCATIONENTRY' and dgUserID <> ''");
	}
}
