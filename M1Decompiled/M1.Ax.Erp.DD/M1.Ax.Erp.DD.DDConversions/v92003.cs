using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.003", "", "")]
public class v92003
{
	public v92003(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1RMARECEIPTLINESENTRY' and dgUserID <> ''");
	}
}
