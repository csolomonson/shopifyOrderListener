using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.139", "Update NextIDs table", "2011-06-09")]
public class v800139b
{
	public v800139b(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE NextIDs SET xanNumericOnly = 2 WHERE xanTable = 'APPaymentSessions' OR xanTable = 'ARPaymentSessions'");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE NextIDs SET xanNumericOnly = 2 WHERE xanNumericOnly < 0");
	}
}
