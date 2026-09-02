using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.005", "Add Posted to GL flag to Payroll header total/line", "2008-05-13")]
public class v710005
{
	public v710005(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaderTotals", "pagPostedToGL"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotals", "pagPostedToGL", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PayrollHeaderTotals SET pagPostedToGL = pasPostedToGL From PayrollSessions inner join PayrollHeaderTotals On pasPayrollSessionID = pagPayrollSessionID ");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaderTotalLines", "paiPostedToGL"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotalLines", "paiPostedToGL", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PayrollHeaderTotalLines SET paiPostedToGL = pasPostedToGL From PayrollSessions inner join PayrollHeaderTotalLines On pasPayrollSessionID = paiPayrollSessionID ");
		}
	}
}
