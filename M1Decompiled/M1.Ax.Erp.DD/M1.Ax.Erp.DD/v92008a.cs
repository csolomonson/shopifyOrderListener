using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.008", "Convert product configurator control classes to M1Controls92", "2016-05-19")]
public class v92008a
{
	public v92008a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1CONTROLS81.', 'M1CONTROLS92.')");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1CONTROLS90.', 'M1CONTROLS92.')");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1CONTROLS91.', 'M1CONTROLS92.')");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1CONTROLS.', 'M1CONTROLS92.')");
	}
}
