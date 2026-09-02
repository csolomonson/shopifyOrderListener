using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.015", "Add fields to JobOperations table", "2015-01-22")]
public class v900015a
{
	public v900015a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoOverlapOffsetTime"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoOverlapOffsetTime", "numeric", 8, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoMoveTime"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoMoveTime", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobOperations Set jmoMoveTime = xawMoveTime From JobOperations Inner Join WorkCenters On jmoWorkCenterID = xawWorkCenterID");
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoQueueTime"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoQueueTime", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobOperations Set jmoQueueTime = xawQueueTime From JobOperations Inner Join WorkCenters On jmoWorkCenterID = xawWorkCenterID");
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoOverlapDestinationLink"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoOverlapDestinationLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoOverlap"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobOperations Set jmoOverlapDestinationLink = Case When jmoOverlap = 1 Then 3 When jmoOverlap = 2 Then 4 Else 0 End From JobOperations Where jmoOverlap <> 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoOverlapSourceLink"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoOverlapSourceLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoOverlap"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update JobOperations Set jmoOverlapSourceLink = Case When jmoOverlap = 1 Then 3 When jmoOverlap = 2 Then 4 Else 0 End From JobOperations Where jmoOverlap <> 0");
		}
	}
}
