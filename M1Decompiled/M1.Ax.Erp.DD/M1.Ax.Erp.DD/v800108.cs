using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.108", "Convert product configurator control classes", "2011-03-11")]
public class v800108
{
	public v800108(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoClassID = Replace(xaoClassID, 'M1.', 'M1CONTROLS.')");
	}
}
