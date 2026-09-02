using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.015", "Add fields to WorkCenters table", "2015-01-27")]
public class v900015e
{
	public v900015e(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawPeoplePerMachine"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenters", "xawPeoplePerMachine", "xawPeoplePerMachineProd", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawPeoplePerMachineSetup"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenters", "xawPeoplePerMachineSetup", "smallint", 3, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawPeoplePerMachineProd"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update WorkCenters Set xawPeoplePerMachineSetup = xawPeoplePerMachineProd");
			}
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawInfiniteCapacity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenters", "xawInfiniteCapacity", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawIgnoreCalendarMove"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenters", "xawIgnoreCalendarMove", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawIgnoreCalendarQueue"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenters", "xawIgnoreCalendarQueue", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
