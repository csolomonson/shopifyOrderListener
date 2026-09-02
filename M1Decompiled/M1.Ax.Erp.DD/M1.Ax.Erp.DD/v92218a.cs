using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.218", "Add fields to ProductionCalendarDays table", "2017-04-10")]
public class v92218a
{
	public v92218a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionCalendarDays", "jmyWorkCenterID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendarDays", "jmyWorkCenterID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "ProductionCalendarDays", new DmoIndex[1]
		{
			new DmoIndex("JMYPRODUCTIONCALENDARYEARID,jmyPlantID,JMYPRODUCTIONCALENDARMONTH,JMYPRODUCTIONCALENDARDAY", unique: true)
		}, parms.Messages);
		parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "ProductionCalendarDays", new DmoIndex[1]
		{
			new DmoIndex("JMYPRODUCTIONCALENDARYEARID,JMYPRODUCTIONCALENDARMONTH,JMYPRODUCTIONCALENDARDAY,jmyPlantID", unique: true)
		}, parms.Messages);
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionCalendarDays", "JMYPRODUCTIONCALENDARDAY"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendarDays", new DmoIndex[1]
			{
				new DmoIndex("JMYPRODUCTIONCALENDARYEARID,jmyPlantID,jmyWorkCenterID,JMYPRODUCTIONCALENDARMONTH,JMYPRODUCTIONCALENDARDAY", unique: true)
			}, parms.Messages);
		}
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ProductionCalendarDaysWC"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Insert Into ProductionCalendarDays (jmyProductionCalendarYearID,jmyPlantID,jmyWorkCenterID,jmyProductionCalendarMonth,jmyProductionCalendarDay,jmyHours,jmyDayStartTime,jmyDayOfWeek,jmyHoliday) Select jmyProductionCalendarYearID,xawPlantID,jmyWorkCenterID,jmyProductionCalendarMonth,jmyProductionCalendarDay,jmyHours,jmyDayStartTime,jmyDayOfWeek,jmyHoliday From ProductionCalendarDaysWC Inner Join WorkCenters On jmyWorkCenterID = xawWorkCenterID COLLATE SQL_Latin1_General_CP1_CS_AS Where jmyWorkCenterID <> '' And Not Exists (Select A.jmyProductionCalendarYearID, A.jmyPlantID, A.jmyWorkCenterID, A.jmyProductionCalendarMonth, A.jmyProductionCalendarDay From ProductionCalendarDays A Where ProductionCalendarDaysWC.jmyProductionCalendarYearID = A.jmyProductionCalendarYearID and WorkCenters.xawPlantID = A.jmyPlantID and ProductionCalendarDaysWC.jmyWorkCenterID = A.jmyWorkCenterID and ProductionCalendarDaysWC.jmyProductionCalendarMonth = A.jmyProductionCalendarMonth and ProductionCalendarDaysWC.jmyProductionCalendarDay = A.jmyProductionCalendarDay)");
		}
	}
}
