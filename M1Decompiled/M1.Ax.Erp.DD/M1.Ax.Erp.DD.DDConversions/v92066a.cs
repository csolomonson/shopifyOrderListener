using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.066", "", "")]
public class v92066a
{
	public v92066a(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1INVENTORYCOUNTSENTRY' and dgUserID <> ''");
	}
}
