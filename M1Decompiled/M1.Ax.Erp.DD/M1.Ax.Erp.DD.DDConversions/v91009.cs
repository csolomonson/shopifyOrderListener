using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.1.009", "", "")]
public class v91009
{
	public v91009(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDGridDetails WHERE dgGridID = 'M1SHIFTBREAKSENTRY' and dgUserID <> ''");
	}
}
