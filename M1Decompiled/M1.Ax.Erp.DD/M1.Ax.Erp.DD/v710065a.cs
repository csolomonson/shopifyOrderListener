using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.065", "Update TimeFormat field", "2008-07-21")]
public class v710065a
{
	public v710065a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadTimeFormat = 1 Where xadTimeFormat = 0");
	}
}
