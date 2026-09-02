using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.058", "Convert product configurator control classes to M1Controls92", "2016-05-19")]
public class v91058k
{
	public v91058k(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1CONTROLS81.', 'M1CONTROLS92.')");
	}
}
