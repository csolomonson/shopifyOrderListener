using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.019", "Convert product configurator control classes to M1Controls91", "2013-02-11")]
public class v810019a
{
	public v810019a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1CONTROLS.', 'M1CONTROLS92.')");
	}
}
