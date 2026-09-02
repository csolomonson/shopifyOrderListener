using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.071", "Update field bindings", "2014-04-24")]
public class v810069c
{
	public v810069c(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderPickListLines Set omyStatus = omsStatus From SalesOrderPickListSessions Inner Join SalesOrderPickListLines On OMSPICKLISTSESSIONID = OMYPICKLISTSESSIONID");
	}
}
