using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.035", "", "")]
public class v710035
{
	public v710035(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1RMAREQSWEBTODAY' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1APPAYMENTHEADERSVOIDABLE' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ARPAYMENTHEADERSVOIDABLE' and dgUserID <> ''");
	}
}
