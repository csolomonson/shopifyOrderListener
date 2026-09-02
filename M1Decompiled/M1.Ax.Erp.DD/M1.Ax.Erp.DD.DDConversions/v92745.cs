using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.745", "", "")]
public class v92745
{
	public v92745(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1JOBOPERATIONSALL') and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1SERIALNUMBERTRANSACTIONSALL') and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID IN ('M1LOTNUMBERTRANSACTIONSALL') and dgUserID <> ''");
	}
}
