using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.064", "Update class ids for 9.1", "2016-05-30")]
public class v91064a
{
	public v91064a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1CONTROLS90.', 'M1CONTROLS92.')");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1CONTROLS91.', 'M1CONTROLS92.')");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1CONTROLS81.', 'M1CONTROLS92.')");
	}
}
