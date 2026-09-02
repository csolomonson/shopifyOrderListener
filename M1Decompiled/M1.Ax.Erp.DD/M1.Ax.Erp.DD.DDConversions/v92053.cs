using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.053", "", "")]
public class v92053
{
	public v92053(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1MATERIALISSUELINESENTRY' and dgUserID <> ''");
	}
}
