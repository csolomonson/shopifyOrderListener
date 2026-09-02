using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.00.119", "", "")]
public class v800119
{
	public v800119(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1CHANGELOGTABLENAME' and dgUserID <> ''");
	}
}
