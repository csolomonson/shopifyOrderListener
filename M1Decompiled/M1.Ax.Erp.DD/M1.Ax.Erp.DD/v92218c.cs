using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.218", "Add fields to WorkCenters table", "2017-04-10")]
public class v92218c
{
	public v92218c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawEnableCalendar"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenters", "xawEnableCalendar", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update WorkCenters Set xawEnableCalendar = 1 Where xawHoursMon <> 0 Or xawHoursTue <> 0 Or xawHoursWed <> 0 Or xawHoursThu <> 0 Or xawHoursFri <> 0 Or xawHoursSat <> 0 Or xawHoursSun <> 0");
		}
	}
}
