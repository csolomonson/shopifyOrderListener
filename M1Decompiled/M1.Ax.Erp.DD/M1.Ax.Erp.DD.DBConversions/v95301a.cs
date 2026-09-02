using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.301", "Update stpSTPFFRSubmittedDate and stpSTPFFRSubmitted when full file replacement is checked", "2022-07-04")]
public class v95301a
{
	public v95301a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE STPSessions SET stpSTPFFRSubmitted = 1, stpSTPFFRSubmittedDate = stpSTPSubmittedDate WHERE stpFullFileReplacement = 1 AND stpSTPFFRSubmitted = 0 AND stpSTPFFRSubmittedDate IS NULL");
	}
}
