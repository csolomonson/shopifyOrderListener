using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.220", "", "")]
public class v710220
{
	public v710220(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1SALESORDERSALL' and dgUserID <> ''");
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1ARINVOICESALL' and dgUserID <> ''");
	}
}
