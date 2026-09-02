using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("7.10.015", "", "")]
public class v710015
{
	public v710015(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1SALESORDERSBYSALESPERSON' and dgUserID <> ''");
	}
}
