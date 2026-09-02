using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.060", "Add TimeTotal fields to Calls", "2013-11-28")]
public class v810060
{
	public v810060(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "CallLines", "kblTotalTime"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CallLines", "kblTotalTime", "numeric", 7, 2, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update CallLines Set kblTotalTime = kblTimeSpent + kblExtraTime");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Calls", "kbpSubTotalTime"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Calls", "kbpSubTotalTime", "numeric", 7, 2, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE Calls SET kbpSubTotalTime = ISNULL(kbpTotalTime,0) FROM Calls LEFT OUTER JOIN (SELECT kblCallID,SUM(kblTotalTime) As kblTotalTime FROM CallLines WHERE kblCallLineID <> 0 GROUP BY kblCallID) AS Test ON kbpCallID = kblCallID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Calls", "kbpFieldServiceCall"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Calls", "kbpFieldServiceCall", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE Calls SET kbpFieldServiceCall = kbtFieldServiceCall From Calls Inner Join CallTypes On kbtCallTypeID = kbpCallTypeID");
		}
	}
}
