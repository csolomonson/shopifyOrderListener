using System;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.077", "Add Schedule tables", "2016-06-09")]
public class v91077
{
	[DBConversion("9.1.079", "Add fields to ProductionCalendarDays table", "2016-06-14")]
	public class v91079a
	{
		public v91079a(DBConversionParms parms)
		{
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ProductionCalendarDays") && !parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ProductionCalendarDaysWC"))
			{
				parms.Dmo.RenameTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendarDays", "ProductionCalendarDaysWC");
			}
			if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ProductionCalendarDays"))
			{
				parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendarDays", new DmoField[8]
				{
					new DmoField("jmyProductionCalendarYearID", "smallint", 4, 0, nullable: false),
					new DmoField("jmyProductionCalendarMonth", "tinyint", 2, 0, nullable: false),
					new DmoField("jmyProductionCalendarDay", "tinyint", 2, 0, nullable: false),
					new DmoField("jmyPlantID", "nvarchar", 5, 0, nullable: false),
					new DmoField("jmyHours", "numeric", 5, 2, nullable: false),
					new DmoField("jmyDayStartTime", "numeric", 5, 2, nullable: false),
					new DmoField("jmyDayOfWeek", "tinyint", 1, 0, nullable: false),
					new DmoField("jmyHoliday", "bit", 1, 0, nullable: false)
				}, new DmoIndex[2]
				{
					new DmoIndex("JMYPRODUCTIONCALENDARYEARID,JMYPRODUCTIONCALENDARMONTH,JMYPRODUCTIONCALENDARDAY,JMYPLANTID", unique: true),
					new DmoIndex("jmyPlantID", unique: false)
				});
				if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ProductionCalendarDaysWC"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Insert Into ProductionCalendarDays (jmyProductionCalendarYearID,jmyProductionCalendarMonth,jmyProductionCalendarDay,jmyHours,jmyDayStartTime,jmyDayOfWeek,jmyHoliday) Select jmyProductionCalendarYearID,jmyProductionCalendarMonth,jmyProductionCalendarDay,jmyHours,jmyDayStartTime,jmyDayOfWeek,jmyHoliday From ProductionCalendarDaysWC Where jmyWorkCenterID = ''");
				}
			}
		}
	}

	[DBConversion("9.1.079", "Add StartTime and Hours to DatasetProperties and Plants tables", "2016-06-14")]
	public class v91079b
	{
		public v91079b(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadHoursSun"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadHoursSun", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursSun"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadHoursSun = (select IsNull(Max(xawHoursSun), (select IsNull(Max(xawHoursSun), 0) From WorkCenters Where xawHoursSun <> 0)) From WorkCenters Where xawHoursSun <> 0 And xawHoursSun <> 24)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadHoursMon"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadHoursMon", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursMon"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadHoursMon = (select IsNull(Max(xawHoursMon), (select IsNull(Max(xawHoursMon), 0) From WorkCenters Where xawHoursMon <> 0)) From WorkCenters Where xawHoursMon <> 0 And xawHoursMon <> 24)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadHoursTue"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadHoursTue", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursTue"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadHoursTue = (select IsNull(Max(xawHoursTue), (select IsNull(Max(xawHoursTue), 0) From WorkCenters Where xawHoursTue <> 0)) From WorkCenters Where xawHoursTue <> 0 And xawHoursTue <> 24)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadHoursWed"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadHoursWed", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursWed"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadHoursWed = (select IsNull(Max(xawHoursWed), (select IsNull(Max(xawHoursWed), 0) From WorkCenters Where xawHoursWed <> 0)) From WorkCenters Where xawHoursWed <> 0 And xawHoursWed <> 24)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadHoursThu"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadHoursThu", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursThu"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadHoursThu = (select IsNull(Max(xawHoursThu), (select IsNull(Max(xawHoursThu), 0) From WorkCenters Where xawHoursThu <> 0)) From WorkCenters Where xawHoursThu <> 0 And xawHoursThu <> 24)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadHoursFri"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadHoursFri", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursFri"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadHoursFri = (select IsNull(Max(xawHoursFri), (select IsNull(Max(xawHoursFri), 0) From WorkCenters Where xawHoursFri <> 0)) From WorkCenters Where xawHoursFri <> 0 And xawHoursFri <> 24)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadHoursSat"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadHoursSat", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursSat"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadHoursSat = (select IsNull(Max(xawHoursSat), (select IsNull(Max(xawHoursSat), 0) From WorkCenters Where xawHoursSat <> 0)) From WorkCenters Where xawHoursSat <> 0 And xawHoursSat <> 24)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeSun"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeSun", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeSun"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadDayStartTimeSun = (select IsNull(Min(xawDayStartTimeSun), 0) From WorkCenters Where xawDayStartTimeSun <> 0)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeMon"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeMon", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeMon"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadDayStartTimeMon = (select IsNull(Min(xawDayStartTimeMon), 0) From WorkCenters Where xawDayStartTimeMon <> 0)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeTue"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeTue", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeTue"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadDayStartTimeTue = (select IsNull(Min(xawDayStartTimeTue), 0) From WorkCenters Where xawDayStartTimeTue <> 0)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeWed"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeWed", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeWed"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadDayStartTimeWed = (select IsNull(Min(xawDayStartTimeWed), 0) From WorkCenters Where xawDayStartTimeWed <> 0)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeThu"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeThu", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeThu"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadDayStartTimeThu = (select IsNull(Min(xawDayStartTimeThu), 0) From WorkCenters Where xawDayStartTimeThu <> 0)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeFri"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeFri", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeFri"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadDayStartTimeFri = (select IsNull(Min(xawDayStartTimeFri), 0) From WorkCenters Where xawDayStartTimeFri <> 0)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeSat"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadDayStartTimeSat", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeSat"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadDayStartTimeSat = (select IsNull(Min(xawDayStartTimeSat), 0) From WorkCenters Where xawDayStartTimeSat <> 0)");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauHoursSun"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauHoursSun", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursSun = xadHoursSun From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursSun"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursSun = xawHoursSun From Plants Inner Join (select Max(xawHoursSun) As xawHoursSun,xawPlantID From WorkCenters Where xawHoursSun <> 24 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauHoursMon"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauHoursMon", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursMon = xadHoursMon From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursMon"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursMon = xawHoursMon From Plants Inner Join (select Max(xawHoursMon) As xawHoursMon,xawPlantID From WorkCenters Where xawHoursMon <> 24 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauHoursTue"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauHoursTue", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursTue = xadHoursTue From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursTue"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursTue = xawHoursTue From Plants Inner Join (select Max(xawHoursTue) As xawHoursTue,xawPlantID From WorkCenters Where xawHoursTue <> 24 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauHoursWed"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauHoursWed", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursWed = xadHoursWed From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursWed"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursWed = xawHoursWed From Plants Inner Join (select Max(xawHoursWed) As xawHoursWed,xawPlantID From WorkCenters Where xawHoursWed <> 24 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauHoursThu"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauHoursThu", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursThu = xadHoursThu From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursThu"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursThu = xawHoursThu From Plants Inner Join (select Max(xawHoursThu) As xawHoursThu,xawPlantID From WorkCenters Where xawHoursThu <> 24 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauHoursFri"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauHoursFri", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursFri = xadHoursFri From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursFri"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursFri = xawHoursFri From Plants Inner Join (select Max(xawHoursFri) As xawHoursFri,xawPlantID From WorkCenters Where xawHoursFri <> 24 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauHoursSat"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauHoursSat", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursSat = xadHoursSat From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawHoursSat"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauHoursSat = xawHoursSat From Plants Inner Join (select Max(xawHoursSat) As xawHoursSat,xawPlantID From WorkCenters Where xawHoursSat <> 24 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauDayStartTimeSun"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauDayStartTimeSun", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeSun = xadDayStartTimeSun From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeSun"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeSun = xawDayStartTimeSun From Plants Inner Join (select Min(xawDayStartTimeSun) As xawDayStartTimeSun,xawPlantID From WorkCenters Where xawDayStartTimeSun <> 0 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauDayStartTimeMon"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauDayStartTimeMon", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeMon = xadDayStartTimeMon From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeMon"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeMon = xawDayStartTimeMon From Plants Inner Join (select Min(xawDayStartTimeMon) As xawDayStartTimeMon,xawPlantID From WorkCenters Where xawDayStartTimeMon <> 0 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauDayStartTimeTue"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauDayStartTimeTue", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeTue = xadDayStartTimeTue From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeTue"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeTue = xawDayStartTimeTue From Plants Inner Join (select Min(xawDayStartTimeTue) As xawDayStartTimeTue,xawPlantID From WorkCenters Where xawDayStartTimeTue <> 0 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauDayStartTimeWed"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauDayStartTimeWed", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeWed = xadDayStartTimeWed From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeWed"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeWed = xawDayStartTimeWed From Plants Inner Join (select Min(xawDayStartTimeWed) As xawDayStartTimeWed,xawPlantID From WorkCenters Where xawDayStartTimeWed <> 0 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauDayStartTimeThu"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauDayStartTimeThu", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeThu = xadDayStartTimeThu From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeThu"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeThu = xawDayStartTimeThu From Plants Inner Join (select Min(xawDayStartTimeThu) As xawDayStartTimeThu,xawPlantID From WorkCenters Where xawDayStartTimeThu <> 0 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauDayStartTimeFri"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauDayStartTimeFri", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeFri = xadDayStartTimeFri From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeFri"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeFri = xawDayStartTimeFri From Plants Inner Join (select Min(xawDayStartTimeFri) As xawDayStartTimeFri,xawPlantID From WorkCenters Where xawDayStartTimeFri <> 0 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauDayStartTimeSat"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauDayStartTimeSat", "numeric", 5, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeSat = xadDayStartTimeSat From Plants,DatasetProperties");
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawDayStartTimeSat"))
				{
					parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Plants Set xauDayStartTimeSat = xawDayStartTimeSat From Plants Inner Join (select Min(xawDayStartTimeSat) As xawDayStartTimeSat,xawPlantID From WorkCenters Where xawDayStartTimeSat <> 0 Group By xawPlantID) as test On xawPlantID=xauPlantID");
				}
			}
		}
	}

	[DBConversion("9.1.079", "Add fields to Shifts table", "2016-06-14")]
	public class v91079d
	{
		public v91079d(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Shifts", "lmsPlantID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Shifts", "lmsPlantID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.080", "Increased the size and precision for Shipment Packages Weight", "2016-06-16")]
	public class v91080a
	{
		public v91080a(DBConversionParms parms)
		{
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ShipmentPackages"))
			{
				parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackages", new DmoField[28]
				{
					new DmoField("spaShipmentID", "nvarchar", 10, 0, nullable: false),
					new DmoField("spaShipmentPackageID", "int", 5, 0, nullable: false),
					new DmoField("spaShippingMethodID", "nvarchar", 5, 0, nullable: false),
					new DmoField("spaCarrier", "nvarchar", 5, 0, nullable: false),
					new DmoField("spaUPSPackageTypes", "nvarchar", 20, 0, nullable: false),
					new DmoField("spaFedExPackageTypes", "nvarchar", 20, 0, nullable: false),
					new DmoField("spaCustomerPackageID", "char", 10, 0, nullable: false),
					new DmoField("spaPackageDimensionsUOM", "nvarchar", 2, 0, nullable: false),
					new DmoField("spaPackageHeight", "int", 3, 0, nullable: false),
					new DmoField("spaPackageLength", "int", 3, 0, nullable: false),
					new DmoField("spaPackageWidth", "int", 3, 0, nullable: false),
					new DmoField("spaPackageWeightUOM", "nvarchar", 3, 0, nullable: false),
					new DmoField("spaPackageWeight", "numeric", 15, 5, nullable: false),
					new DmoField("spaPackageRate", "money", 12, 2, nullable: false),
					new DmoField("spaLargePackage", "bit", 1, 0, nullable: false),
					new DmoField("spaAdditionalHandlingRequired", "bit", 1, 0, nullable: false),
					new DmoField("spaVerbalConfirmationRequired", "bit", 1, 0, nullable: false),
					new DmoField("spaShipmentIDNumber", "nvarchar", 20, 0, nullable: false),
					new DmoField("spaTrackingNo", "nvarchar", 20, 0, nullable: false),
					new DmoField("spaPackageValue", "money", 12, 2, nullable: false),
					new DmoField("spaPackageRateForeign", "money", 12, 2, nullable: false),
					new DmoField("spaReference1", "nvarchar", 35, 0, nullable: false),
					new DmoField("spaReference2", "nvarchar", 35, 0, nullable: false),
					new DmoField("spaLabelFilePath", "nvarchar(max)", 50, 0, nullable: true),
					new DmoField("spaPackageValueForeign", "money", 12, 2, nullable: false),
					new DmoField("spaCreatedBy", "nvarchar", 20, 0, nullable: false),
					new DmoField("spaCreatedDate", "datetime", 14, 0, nullable: true),
					new DmoField("spaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
				}, new DmoIndex[4]
				{
					new DmoIndex("SPASHIPMENTID,SPASHIPMENTPACKAGEID", unique: true),
					new DmoIndex("SPAUNIQUEID", unique: true),
					new DmoIndex("spaShipmentID", unique: false),
					new DmoIndex("spaShipmentPackageID", unique: false)
				}, mergeCustomFields: true);
			}
		}
	}

	[DBConversion("9.1.080", "Add fields to ProductionCalendars table", "2016-06-14")]
	public class v91080b
	{
		public v91080b(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionCalendars", "jmlWorkCenterID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendars", "jmlWorkCenterID", "nvarchar", 5, 0, verifyIndexes: false, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionCalendars", "jmlPlantID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendars", "jmlPlantID", "nvarchar", 5, 0, verifyIndexes: false, dropTriggers: true, isNullable: false, parms.Messages);
			}
			parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "ProductionCalendars", new DmoIndex[1]
			{
				new DmoIndex("jmlProductionCalendarYearID", unique: true)
			}, parms.Messages);
			parms.Dmo.VerifyIndexesOnTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendars", parms.Messages, null);
		}
	}

	[DBConversion("9.1.085", "Add fields to PartTransactions table", "2016-06-24")]
	public class v91085a
	{
		public v91085a(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtTableName"))
			{
				parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", new DmoIndex[1]
				{
					new DmoIndex("imtTableName", unique: false)
				}, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtTableUniqueID"))
			{
				parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", new DmoIndex[1]
				{
					new DmoIndex("imtTableUniqueID", unique: false)
				}, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtInventoryQuantityReceived"))
			{
				parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", new DmoIndex[1]
				{
					new DmoIndex("imtInventoryQuantityReceived", unique: false)
				}, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.086", "Add fields to PartWarehouseLocations table", "2016-06-27")]
	public class v91086a
	{
		public v91086a(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartWarehouseLocations", "imLLastRunDatePurchasePlanner"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartWarehouseLocations", "imLLastRunDatePurchasePlanner", "datetime", 14, 0, isNullable: true, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.086", "Add fields to PartRevisions table", "2016-06-27")]
	public class v91086b
	{
		public v91086b(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrLastRunDatePurchasePlanner"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrLastRunDatePurchasePlanner", "datetime", 14, 0, isNullable: true, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.086", "Add fields to PurchasePlannerLines table", "2016-06-27")]
	public class v91086c
	{
		public v91086c(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerLines", "pplLastRunDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerLines", "pplLastRunDate", "datetime", 14, 0, isNullable: true, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.097", "Add fields to JobResourceLanes table", "2016-07-06")]
	public class v91097a
	{
		public v91097a(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "JobResourceLanes"))
			{
				parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobResourceLanes", new DmoField[11]
				{
					new DmoField("jmuJobID", "nvarchar", 20, 0, nullable: false),
					new DmoField("jmuJobAssemblyID", "int", 5, 0, nullable: false),
					new DmoField("jmuJobOperationID", "int", 5, 0, nullable: false),
					new DmoField("jmuJobResourceLaneID", "smallint", 4, 0, nullable: false),
					new DmoField("jmuResourceType", "tinyint", 1, 0, nullable: false),
					new DmoField("jmuResourceTable", "nvarchar", 30, 0, nullable: false),
					new DmoField("jmuLockedResourceUniqueID", "uniqueidentifier", 16, 0, nullable: true),
					new DmoField("jmuGroupTable", "nvarchar", 30, 0, nullable: false),
					new DmoField("jmuGroupUniqueID", "uniqueidentifier", 16, 0, nullable: true),
					new DmoField("jmuSetup", "uniqueidentifier", 16, 0, nullable: true),
					new DmoField("jmuProduction", "uniqueidentifier", 16, 0, nullable: true)
				}, new DmoIndex[1]
				{
					new DmoIndex("jmuJobID,jmuJobAssemblyID,jmuJobOperationID,jmuJobResourceLaneID", unique: true)
				});
			}
		}
	}

	[DBConversion("9.1.097", "Add fields to JobResourceCells table", "2016-07-06")]
	public class v91097b
	{
		public v91097b(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "JobResourceCells"))
			{
				parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobResourceCells", new DmoField[6]
				{
					new DmoField("jmvJobID", "nvarchar", 20, 0, nullable: false),
					new DmoField("jmvJobAssemblyID", "int", 5, 0, nullable: false),
					new DmoField("jmvJobOperationID", "int", 5, 0, nullable: false),
					new DmoField("jmvJobResourceLaneID", "smallint", 4, 0, nullable: false),
					new DmoField("jmvJobResourceCellID", "tinyint", 1, 0, nullable: false),
					new DmoField("jmvResourceUniqueID", "uniqueidentifier", 16, 0, nullable: true)
				}, new DmoIndex[1]
				{
					new DmoIndex("jmvJobID,jmvJobAssemblyID,jmvJobOperationID,jmvJobResourceLaneID,jmvJobResourceCellID", unique: true)
				});
			}
		}
	}

	[DBConversion("9.1.097", "Add fields to ScheduleResourceLanes table", "2016-07-06")]
	public class v91097c
	{
		public v91097c(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleResourceLanes", "sxrResourceType"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleResourceLanes", "sxrResourceType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.097", "Add fields to ScheduleTrees table", "2016-07-09")]
	public class v91097d
	{
		public v91097d(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTrees", "sxtType"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", "sxtType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTrees", "sxtDescription"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", "sxtDescription", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTrees", "sxtGroupUniqueID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", "sxtGroupUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.097", "Add fields to QuoteAssemblies table", "2016-07-13")]
	public class v91097e
	{
		public v91097e(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteAssemblies", "qmaAssemblyOverlap"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteAssemblies", "qmaAssemblyOverlap", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteAssemblies", "qmaOverlapSourceOperationID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteAssemblies", "qmaOverlapSourceOperationID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteAssemblies", "qmaOverlapOffsetTime"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteAssemblies", "qmaOverlapOffsetTime", "numeric", 8, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteAssemblies", "qmaOverlapSourceLink"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteAssemblies", "qmaOverlapSourceLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteAssemblies", "qmaOverlapDestinationLink"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteAssemblies", "qmaOverlapDestinationLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.097", "Add fields to PartAssemblies table", "2016-07-13")]
	public class v91097h
	{
		public v91097h(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartAssemblies", "imaOverlapDestinationLink"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartAssemblies", "imaOverlapDestinationLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartAssemblies", "imaAssemblyOverlap"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartAssemblies", "imaAssemblyOverlap", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartAssemblies", "imaOverlapSourceOperationID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartAssemblies", "imaOverlapSourceOperationID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartAssemblies", "imaOverlapSourceLink"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartAssemblies", "imaOverlapSourceLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartAssemblies", "imaOverlapOffsetTime"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartAssemblies", "imaOverlapOffsetTime", "numeric", 8, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.097", "Add fields to PartOperations table", "2016-07-13")]
	public class v91097i
	{
		public v91097i(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartOperations", "imoOverlapSourceLink"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOperations", "imoOverlapSourceLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartOperations", "imoOverlapOffsetTime"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOperations", "imoOverlapOffsetTime", "numeric", 8, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartOperations", "imoOverlapOperationID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOperations", "imoOverlapOperationID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartOperations", "imoOverlapDestinationLink"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOperations", "imoOverlapDestinationLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.097", "Add fields to QuoteOperations table", "2016-07-13")]
	public class v91097j
	{
		public v91097j(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteOperations", "qmoOverlapSourceLink"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteOperations", "qmoOverlapSourceLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteOperations", "qmoOverlapOperationID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteOperations", "qmoOverlapOperationID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteOperations", "qmoOverlapDestinationLink"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteOperations", "qmoOverlapDestinationLink", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteOperations", "qmoOverlapOffsetTime"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteOperations", "qmoOverlapOffsetTime", "numeric", 8, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.097", "Add fields to JobOperations table", "2016-07-13")]
	public class v91097m
	{
		public v91097m(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoOverlapJobOperationID") && !parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoOverlapOperationID"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoOverlapJobOperationID", "jmoOverlapOperationID", dropTriggers: true);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoOverlapJobOperationID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoOverlapJobOperationID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.097", "Add fields to Assemblies tables", "2016-07-13")]
	public class v91097n
	{
		public v91097n(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlapSourceJobOperationID") && !parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlapSourceOperationID"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaOverlapSourceJobOperationID", "jmaOverlapSourceOperationID", dropTriggers: true);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlapSourceOperationID"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaOverlapSourceOperationID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlapJobOperationID") && !parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlapOperationID"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaOverlapJobOperationID", "jmaOverlapOperationID", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartAssemblies", "imaOverlapMethodOperationID"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartAssemblies", "imaOverlapMethodOperationID", "imaOverlapOperationID", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteAssemblies", "qmaOverlapQuoteOperationID"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteAssemblies", "qmaOverlapQuoteOperationID", "qmaOverlapOperationID", dropTriggers: true);
			}
		}
	}

	[DBConversion("9.1.102", "Add fields to Processes table", "2016-07-19")]
	public class v91102a
	{
		public v91102a(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Processes", "xacIgnoreCalendarQueue"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Processes", "xacIgnoreCalendarQueue", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Processes", "xacIgnoreCalendarMove"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Processes", "xacIgnoreCalendarMove", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.102", "Remove fields from WorkCenters table", "2016-07-19")]
	public class v91102b
	{
		public v91102b(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawIgnoreCalendarQueue"))
			{
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenters", "xawIgnoreCalendarQueue", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WorkCenters", "xawIgnoreCalendarMove"))
			{
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenters", "xawIgnoreCalendarMove", dropTriggers: true);
			}
		}
	}

	[DBConversion("9.1.102", "Remove fields from ScheduleTasks table", "2016-07-19")]
	public class v91102c
	{
		public v91102c(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkGroupTable"))
			{
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkGroupTable", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkGroupUniqueID"))
			{
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkGroupUniqueID", dropTriggers: true);
			}
		}
	}

	[DBConversion("9.1.103", "Add fields to ScheduleTaskBuckets table", "2016-07-20")]
	public class v91103a
	{
		public v91103a(DBConversionParms parms)
		{
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets"))
			{
				parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets");
			}
			if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets"))
			{
				parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", new DmoField[11]
				{
					new DmoField("sxeScheduleTreeID", "int", 4, 0, nullable: false),
					new DmoField("sxeScheduleBranchID", "int", 4, 0, nullable: false),
					new DmoField("sxeScheduleTaskID", "int", 4, 0, nullable: false),
					new DmoField("sxeScheduleTaskBucketID", "tinyint", 1, 0, nullable: false),
					new DmoField("sxeScheduleTypeID", "tinyint", 1, 0, nullable: false),
					new DmoField("sxeScheduleTypeBucketID", "tinyint", 1, 0, nullable: false),
					new DmoField("sxeHours", "numeric", 8, 2, nullable: false),
					new DmoField("sxePercentComplete", "smallint", 3, 0, nullable: false),
					new DmoField("sxeCompletedHours", "numeric", 8, 2, nullable: false),
					new DmoField("sxeCompleted", "bit", 1, 0, nullable: false),
					new DmoField("sxeUniqueID", "uniqueidentifier", 16, 0, nullable: false)
				}, new DmoIndex[1]
				{
					new DmoIndex("sxeScheduleTreeID,sxeScheduleBranchID,sxeScheduleTaskID,sxeScheduleTaskBucketID", unique: true)
				});
			}
		}
	}

	[DBConversion("9.1.103", "Add fields to ProductionProperties table", "2016-07-20")]
	public class v91103b
	{
		public v91103b(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapJMIgnoreMachines"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapJMIgnoreMachines", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapJMIgnoreEmployees"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapJMIgnoreEmployees", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.109", "Add fields to ScheduleAllocations table", "2016-07-23")]
	public class v91109a
	{
		public v91109a(DBConversionParms parms)
		{
			if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleDates"))
			{
				parms.Dmo.RenameTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleDates", "ScheduleAllocations");
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdDuration"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", "sxdDuration", "sxdMinutes", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdMinutes"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", "sxdMinutes", "int", 4, 0, isNullable: false, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdResourceLane"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", "sxdResourceLane", "sxdScheduleResourceLaneID", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdStartHour"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", "sxdStartHour", "sxdStartMinute", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdStartMinute"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", "sxdStartMinute", "smallint", 5, 2, isNullable: false, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdScheduleDateID"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", "sxdScheduleDateID", "sxdScheduleAllocationID", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdEndHour"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", "sxdEndHour", "sxdEndMinute", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdEndMinute"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", "sxdEndMinute", "smallint", 5, 2, isNullable: false, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdEndDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", "sxdEndDate", "date", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdStartDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", "sxdStartDate", "date", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdGroupUniqueID"))
			{
				parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", new DmoIndex[1]
				{
					new DmoIndex("sxdGroupUniqueID", unique: false)
				}, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdEndActualDateTime"))
			{
				parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", new DmoIndex[1]
				{
					new DmoIndex("sxdEndActualDateTime", unique: false)
				}, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdStartActualDateTime"))
			{
				parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", new DmoIndex[1]
				{
					new DmoIndex("sxdStartActualDateTime", unique: false)
				}, parms.Messages);
			}
			parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "ScheduleAllocations", new DmoIndex[1]
			{
				new DmoIndex("sxdScheduleTreeID,sxdScheduleBranchID,sxdScheduleTaskID,sxdResourceLane,sxdScheduleDateID", unique: true)
			}, parms.Messages);
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleAllocations", "sxdScheduleAllocationID"))
			{
				parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleAllocations", new DmoIndex[1]
				{
					new DmoIndex("sxdScheduleTreeID,sxdScheduleBranchID,sxdScheduleTaskID,sxdScheduleResourceLaneID,sxdScheduleAllocationID", unique: true)
				}, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.109", "Add fields to ScheduleTasks table", "2016-07-23")]
	public class v91109b
	{
		public v91109b(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkStartDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkStartDate", "date", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkEndHour"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkEndHour", "sxkEndMinute", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkEndMinute"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkEndMinute", "smallint", 5, 2, isNullable: false, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkEndDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkEndDate", "date", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkStartHour"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkStartHour", "sxkStartMinute", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkStartMinute"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkStartMinute", "smallint", 5, 2, isNullable: false, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkDuration"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkDuration", "sxkMinutes", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkMinutes"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkMinutes", "int", 4, 0, isNullable: false, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkOffsetTime"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkOffsetTime", "sxkOffsetMinutes", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTasks", "sxkOffsetMinutes"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", "sxkOffsetMinutes", "int", 4, 0, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.109", "Remove fields from ScheduleResourceLanes table", "2016-07-23")]
	public class v91109c
	{
		public v91109c(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleResourceLanes", "sxrResourceType"))
			{
				parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleResourceLanes", new DmoIndex[1]
				{
					new DmoIndex("sxrResourceType", unique: false)
				}, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleResourceLanes", "sxrResourceTable"))
			{
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleResourceLanes", "sxrResourceTable", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleResourceLanes", "sxrGroupTable"))
			{
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ScheduleResourceLanes Set sxrResourceType = Case When sxrGroupTable = 'WorkCenters' Then 1 When sxrGroupTable = 'Shifts' Then 2 Else 0 End");
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleResourceLanes", "sxrGroupTable", dropTriggers: true);
			}
		}
	}

	[DBConversion("9.1.109", "Add fields to ScheduleBranches table", "2016-07-23")]
	public class v91109d
	{
		public v91109d(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleBranches", "sxbOffsetTime"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleBranches", "sxbOffsetTime", "sxbOffsetMinutes", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleBranches", "sxbOffsetMinutes"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleBranches", "sxbOffsetMinutes", "int", 4, 0, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.109", "Add fields to ScheduleTaskBuckets table", "2016-07-23")]
	public class v91109e
	{
		public v91109e(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets", "sxeCompletedHours"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", "sxeCompletedHours", "sxeCompletedMinutes", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets", "sxeCompletedMinutes"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", "sxeCompletedMinutes", "int", 4, 0, isNullable: false, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets", "sxeHours"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", "sxeHours", "sxeMinutes", dropTriggers: true);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets", "sxeMinutes"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", "sxeMinutes", "int", 4, 0, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.109", "Add fields to QuoteOperations table", "2016-07-24")]
	public class v91109f
	{
		public v91109f(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteOperations", "qmoMoveTime"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteOperations", "qmoMoveTime", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QuoteOperations Set qmoMoveTime = xawMoveTime From QuoteOperations Inner Join WorkCenters On qmoWorkCenterID = xawWorkCenterID");
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteOperations", "qmoQueueTime"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteOperations", "qmoQueueTime", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QuoteOperations Set qmoQueueTime = xawQueueTime From QuoteOperations Inner Join WorkCenters On qmoWorkCenterID = xawWorkCenterID");
			}
		}
	}

	[DBConversion("9.1.109", "Add fields to PartOperations table", "2016-07-24")]
	public class v91109g
	{
		public v91109g(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartOperations", "imoMoveTime"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOperations", "imoMoveTime", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PartOperations Set imoMoveTime = xawMoveTime From PartOperations Inner Join WorkCenters On imoWorkCenterID = xawWorkCenterID");
			}
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartOperations", "imoQueueTime"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOperations", "imoQueueTime", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PartOperations Set imoQueueTime = xawQueueTime From PartOperations Inner Join WorkCenters On imoWorkCenterID = xawWorkCenterID");
			}
		}
	}

	[DBConversion("9.1.122", "Alter null status of fields in various tables", "2016-08-05")]
	public class v91122a
	{
		public v91122a(DBConversionParms parms)
		{
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1094YearALEMembers", "hcaCreatedDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1094YearALEMembers", "hcaCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FORM1094YEARTOTALLINES", "hclDateOfBirth"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FORM1094YEARTOTALLINES", "hclDateOfBirth", "date", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FORM1094YEARTOTALLINES", "hclCreatedDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FORM1094YEARTOTALLINES", "hclCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollNZYearSchedules", "nzsEndDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYearSchedules", "nzsEndDate", "date", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollNZYearSchedules", "nzsStartDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYearSchedules", "nzsStartDate", "date", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1094Years", "hcpCreatedDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1094Years", "hcpCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1094Years", "hcpClosedDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1094Years", "hcpClosedDate", "date", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1094YearMonths", "hcmCreatedDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1094YearMonths", "hcmCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1094YearTotals", "hctCreatedDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1094YearTotals", "hctCreatedDate", "datetime", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollNZYearDeductions", "nzdStartDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYearDeductions", "nzdStartDate", "date", 14, 0, isNullable: true, parms.Messages);
			}
			if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollNZYearDeductions", "nzdEndDate"))
			{
				parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollNZYearDeductions", "nzdEndDate", "date", 14, 0, isNullable: true, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.254", "Add fields to ProductionProperties table", "2016-08-21")]
	public class v91254a
	{
		public v91254a(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapJMRefreshHours"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapJMRefreshHours", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.265", "Convert JobSchedules", "2016-09-03")]
	public class v91265a
	{
		public v91265a(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "JobSchedules"))
			{
				return;
			}
			int num = Convert.ToInt32(parms.Database.ExecuteScalar("Select IsNull(Max(sxtScheduleTreeID),0) From ScheduleTrees")) + 1;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Select Identity(int, " + num + ", 1) As sxtScheduleTreeID, jmpJobID, jmpUniqueID Into TreeTemp From (Select jmpJobID,jmpUniqueID From JobSchedules Inner Join Jobs On jmsJobID = jmpJobID Where jmpClosed = 0 And jmsJobID Not In (Select jmpJobID From Jobs Inner Join ScheduleTrees On jmpUniqueID=sxtGroupUniqueID) Group By jmpJobID,jmpUniqueID) As sub\r");
			stringBuilder.Append("Create Index jobid On TreeTemp (jmpJobID)\r");
			stringBuilder.Append("Select sxtScheduleTreeID As sxkScheduleTreeID, jmsJobID As JobID, jmsJobAssemblyID As sxkScheduleBranchID, jmsJobOperationID As sxkScheduleTaskID, RowNum = ROW_NUMBER() Over(Order By sxtScheduleTreeID, jmsJobID, jmsJobAssemblyID, jmsJobOperationID), Convert(smallint, 0) As WorkCenterMachineID ");
			stringBuilder.Append("Into TaskTemp ");
			stringBuilder.Append("From TreeTemp Inner Join JobSchedules On TreeTemp.jmpJobID=jmsJobID Group By sxtScheduleTreeID,jmsJobID,jmsJobAssemblyID,jmsJobOperationID\r");
			stringBuilder.Append("Create Index jobid On TaskTemp (JobID)\r");
			stringBuilder.Append("Create Index branchid on TaskTemp (sxkScheduleBranchID)\r");
			stringBuilder.Append("Create Index taskid on TaskTemp (sxkScheduleTaskID)\r");
			stringBuilder.Append("Update TaskTemp Set WorkCenterMachineID=jmsWorkCenterMachineID From TaskTemp Inner Join JobSchedules On JobID=jmsJobID And sxkScheduleBranchID=jmsJobAssemblyID And sxkScheduleTaskID=jmsJobOperationID And jmsJobScenarioID = ''\r");
			stringBuilder.Append("Insert Into ScheduleTrees(sxtScheduleTreeID, sxtType, sxtDescription, sxtSourceUniqueID, sxtGroupUniqueID, sxtSourceTable, sxtCreatedBy, sxtCreatedDate) Select sxtScheduleTreeID, CONVERT(tinyint, 1) As sxtType, 'Job ' + jmpJobID As sxtDescription, jmpUniqueID As sxtSourceUniqueID, jmpUniqueID As sxtGroupUniqueID, 'Jobs' As sxtSourceTable, " + M1Util.ConvertToSql(parms.User.ID) + " As sxtCreatedBy, GetDate() As sxtCreatedDate From TreeTemp\r");
			stringBuilder.Append("Insert Into ScheduleBranches(sxbScheduleTreeID, sxbScheduleBranchID, sxbParentScheduleBranchID, sxbCreatedBy, sxbCreatedDate)\r");
			stringBuilder.Append("Select sxkScheduleTreeID As sxbScheduleTreeID, sxkScheduleBranchID As sxbScheduleBranchID, jmaParentAssemblyID As sxbParentScheduleBranchID, " + M1Util.ConvertToSql(parms.User.ID) + " As sxbCreatedBy, GetDate() As sxbCreatedDate ");
			stringBuilder.Append("From (Select JobID, sxkScheduleTreeID, sxkScheduleBranchID From TaskTemp Group By JobID, sxkScheduleTreeID, sxkScheduleBranchID) as main Inner Join JobAssemblies On JobID=jmaJobID And sxkScheduleBranchID=jmaJobAssemblyID\r");
			stringBuilder.Append("Insert Into ScheduleTasks(sxkScheduleTreeID, sxkScheduleBranchID, sxkScheduleTaskID, sxkPlantID, sxkProcessID, ");
			stringBuilder.Append("sxkStartActualDateTime, sxkEndActualDateTime, sxkStartDate, sxkStartMinute, sxkEndDate, sxkEndMinute, ");
			stringBuilder.Append("sxkCurrentTaskDateType, sxkLinkedTaskDateType, sxkExchangeID, sxkScheduleTypeID, sxkCreatedBy, sxkCreatedDate) ");
			stringBuilder.Append("Select sxkScheduleTreeID, jmsJobAssemblyID As sxkScheduleBranchID, jmsJobOperationID As sxkScheduleTaskID, jmoPlantID As sxkPlantID, jmoProcessID As sxkProcessID, ");
			stringBuilder.Append("jmsQueueStartTime As sxkStartActualDateTime, jmsMoveDueTime As sxkEndActualDateTime, ");
			stringBuilder.Append("jmsQueueStartDate As sxkStartDate, Convert(smallint, jmsQueueStartHour * 60.0) As sxkStartMinute, ");
			stringBuilder.Append("jmsMoveDueDate As sxkEndDate, Convert(smallint, jmsMoveDueHour * 60.0) As sxkEndMinute, ");
			stringBuilder.Append("Case When jmsOverlap = 1 Then 2 When jmsOverlap = 2 Then 4 Else 0 End As sxkCurrentTaskDateType, ");
			stringBuilder.Append("Case When jmsOverlap = 1 Then 2 When jmsOverlap = 2 Then 4 Else 0 End As sxkLinkedTaskDateType, ");
			stringBuilder.Append("jmsExchangeID As sxkExchangeID, Convert(tinyint, 1) As sxkScheduleTypeID, " + M1Util.ConvertToSql(parms.User.ID) + " As sxkCreatedBy, GetDate() As sxkCreatedDate ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Update ScheduleTasks Set sxkLinkedTaskID = b.sxkScheduleTaskID ");
			stringBuilder.Append("From ScheduleTasks ");
			stringBuilder.Append("Inner Join TaskTemp a On Scheduletasks.sxkScheduleTreeID=a.sxkScheduleTreeID And ScheduleTasks.sxkScheduleBranchID=a.sxkScheduleBranchID And ScheduleTasks.sxkScheduleTaskID=a.sxkScheduleTreeID ");
			stringBuilder.Append("Inner Join TaskTemp b On Scheduletasks.sxkScheduleTreeID=b.sxkScheduleTreeID And ScheduleTasks.sxkScheduleBranchID=b.sxkScheduleBranchID And b.RowNum=a.RowNum-1 ");
			stringBuilder.Append("Where sxkLinkedTaskDateType <> 0\r");
			stringBuilder.Append("Insert Into ScheduleResourceLanes(sxrScheduleTreeID, sxrScheduleBranchID, sxrScheduleTaskID, sxrScheduleResourceLaneID, sxrResourceType, sxrLockedResourceUniqueID, sxrGroupUniqueID) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxrScheduleTreeID, sxkScheduleBranchID As sxrScheduleBranchID, sxkScheduleTaskID As sxrScheduleTaskID, Convert(smallint, 0) As sxrScheduleResourceLaneID, Convert(tinyint, 0) As sxrResourceType, Null As sxrLockedResourceUniqueID, Null As sxrGroupUniqueID ");
			stringBuilder.Append("From TaskTemp\r");
			stringBuilder.Append("Insert Into ScheduleAllocations(sxdScheduleTreeID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleResourceLaneID, sxdScheduleAllocationID, sxdDateType, sxdResourceUniqueID, sxdGroupUniqueID, sxdStartActualDateTime, sxdStartDate, sxdStartMinute, sxdEndActualDateTime, sxdEndDate, sxdEndMinute, sxdMinutes) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxdScheduleTreeID, sxkScheduleBranchID As sxdScheduleBranchID, sxkScheduleTaskID As sxdScheduleTaskID, Convert(smallint, 0) As sxdScheduleResourceLaneID, Convert(tinyint, 1) As sxdScheduleAllocationID, Convert(tinyint, 1) As sxdDateType, Null As sxdResourceUniqueID, Null As sxdGroupUniqueID, ");
			stringBuilder.Append("jmsQueueStartTime As sxdStartActualDateTime, jmsQueueStartDate As sxdStartDate, Convert(smallint, jmsQueueStartHour * 60.0) As sxdStartMinute, ");
			stringBuilder.Append("jmsStartTime As sxdEndActualDateTime, jmsStartDate As sxdEndDate, Convert(smallint, jmsStartHour * 60.0) As sxdEndMinute, ");
			stringBuilder.Append("Convert(int, jmsQueueTime * 60.0) As sxdMinutes ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Insert Into ScheduleAllocations(sxdScheduleTreeID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleResourceLaneID, sxdScheduleAllocationID, sxdDateType, sxdResourceUniqueID, sxdGroupUniqueID, sxdStartActualDateTime, sxdStartDate, sxdStartMinute, sxdEndActualDateTime, sxdEndDate, sxdEndMinute, sxdMinutes) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxdScheduleTreeID, sxkScheduleBranchID As sxdScheduleBranchID, sxkScheduleTaskID As sxdScheduleTaskID, Convert(smallint, 0) As sxdScheduleResourceLaneID, Convert(tinyint, 2) As sxdScheduleAllocationID, Convert(tinyint, 2) As sxdDateType, Null As sxdResourceUniqueID, Null As sxdGroupUniqueID, ");
			stringBuilder.Append("jmsStartTime As sxdStartActualDateTime, jmsStartDate As sxdStartDate, Convert(smallint, jmsStartHour * 60.0) As sxdStartMinute, ");
			stringBuilder.Append("jmsProdStartTime As sxdEndActualDateTime, jmsProdStartDate As sxdEndDate, Convert(smallint, jmsProdStartHour * 60.0) As sxdEndMinute, ");
			stringBuilder.Append("Convert(int, jmsEstimatedSetupHours * 60.0) As sxdMinutes ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Insert Into ScheduleAllocations(sxdScheduleTreeID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleResourceLaneID, sxdScheduleAllocationID, sxdDateType, sxdResourceUniqueID, sxdGroupUniqueID, sxdStartActualDateTime, sxdStartDate, sxdStartMinute, sxdEndActualDateTime, sxdEndDate, sxdEndMinute, sxdMinutes) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxdScheduleTreeID, sxkScheduleBranchID As sxdScheduleBranchID, sxkScheduleTaskID As sxdScheduleTaskID, Convert(smallint, 0) As sxdScheduleResourceLaneID, Convert(tinyint, 3) As sxdScheduleAllocationID, Convert(tinyint, 3) As sxdDateType, Null As sxdResourceUniqueID, Null As sxdGroupUniqueID, ");
			stringBuilder.Append("jmsProdStartTime As sxdStartActualDateTime, jmsProdStartDate As sxdStartDate, Convert(smallint, jmsProdStartHour * 60.0) As sxdStartMinute, ");
			stringBuilder.Append("jmsDueTime As sxdEndActualDateTime, jmsDueDate As sxdEndDate, Convert(smallint, jmsDueHour * 60.0) As sxdEndMinute, ");
			stringBuilder.Append("Convert(int, jmsEstimatedProductionHours * 60.0) As sxdMinutes ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Insert Into ScheduleAllocations(sxdScheduleTreeID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleResourceLaneID, sxdScheduleAllocationID, sxdDateType, sxdResourceUniqueID, sxdGroupUniqueID, sxdStartActualDateTime, sxdStartDate, sxdStartMinute, sxdEndActualDateTime, sxdEndDate, sxdEndMinute, sxdMinutes) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxdScheduleTreeID, sxkScheduleBranchID As sxdScheduleBranchID, sxkScheduleTaskID As sxdScheduleTaskID, Convert(smallint, 0) As sxdScheduleResourceLaneID, Convert(tinyint, 4) As sxdScheduleAllocationID, Convert(tinyint, 4) As sxdDateType, Null As sxdResourceUniqueID, Null As sxdGroupUniqueID, ");
			stringBuilder.Append("jmsDueTime As sxdStartActualDateTime, jmsDueDate As sxdStartDate, Convert(smallint, jmsDueHour * 60.0) As sxdStartMinute, ");
			stringBuilder.Append("jmsMoveDueTime As sxdEndActualDateTime, jmsMoveDueDate As sxdEndDate, Convert(smallint, jmsMoveDueHour * 60.0) As sxdEndMinute, ");
			stringBuilder.Append("Convert(int, jmsMoveTime * 60.0) As sxdMinutes ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Insert Into ScheduleAllocations(sxdScheduleTreeID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleResourceLaneID, sxdScheduleAllocationID, sxdDateType, sxdResourceUniqueID, sxdGroupUniqueID, sxdStartActualDateTime, sxdStartDate, sxdStartMinute, sxdEndActualDateTime, sxdEndDate, sxdEndMinute, sxdMinutes) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxdScheduleTreeID, sxkScheduleBranchID As sxdScheduleBranchID, sxkScheduleTaskID As sxdScheduleTaskID, Convert(smallint, 0) As sxdScheduleResourceLaneID, Convert(tinyint, 5) As sxdScheduleAllocationID, Convert(tinyint, 5) As sxdDateType, Null As sxdResourceUniqueID, Null As sxdGroupUniqueID, ");
			stringBuilder.Append("jmsMoveDueTime As sxdStartActualDateTime, jmsMoveDueDate As sxdStartDate, Convert(smallint, jmsMoveDueHour * 60.0) As sxdStartMinute, ");
			stringBuilder.Append("jmsMoveDueTime As sxdEndActualDateTime, jmsMoveDueDate As sxdEndDate, Convert(smallint, jmsMoveDueHour * 60.0) As sxdEndMinute, ");
			stringBuilder.Append("Convert(int, 0) As sxdMinutes ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Insert Into ScheduleTaskBuckets(sxeScheduleTreeID, sxeScheduleBranchID, sxeScheduleTaskID, sxeScheduleTaskBucketID, sxeScheduleTypeID, sxeScheduleTypeBucketID, sxeMinutes, sxeCompletedMinutes, sxeCompleted, sxePercentComplete) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxeScheduleTreeID, sxkScheduleBranchID As sxeScheduleBranchID, sxkScheduleTaskID As sxeScheduleTaskID, Convert(tinyint, 1) As sxeScheduleTaskBucketID, Convert(tinyint, 1) As sxeScheduleTypeID, Convert(tinyint, 1) As sxeScheduleTypeBucketID, ");
			stringBuilder.Append("Convert(int, jmsQueueTime * 60.0) As sxeMinutes, ");
			stringBuilder.Append("Case When(jmoCompletedSetupHours > 0 Or jmoSetupPercentComplete > 0 Or jmoSetupComplete = 1 Or jmoCompletedProductionHours > 0 Or jmoProductionComplete = 1) Then Convert(int, jmsQueueTime * 60.0) Else 0 End As sxeCompletedMinutes, ");
			stringBuilder.Append("Case When(jmoCompletedSetupHours > 0 Or jmoSetupPercentComplete > 0 Or jmoSetupComplete = 1 Or jmoCompletedProductionHours > 0 Or jmoProductionComplete = 1) Then Convert(bit, 1) Else Convert(bit, 0) End as sxeCompleted, ");
			stringBuilder.Append("Case When(jmoCompletedSetupHours > 0 Or jmoSetupPercentComplete > 0 Or jmoSetupComplete = 1 Or jmoCompletedProductionHours > 0 Or jmoProductionComplete = 1) Then Convert(smallint, 100) Else Convert(smallint, 0) End as sxePercentComplete ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Insert Into ScheduleTaskBuckets(sxeScheduleTreeID, sxeScheduleBranchID, sxeScheduleTaskID, sxeScheduleTaskBucketID, sxeScheduleTypeID, sxeScheduleTypeBucketID, sxeMinutes, sxeCompletedMinutes, sxeCompleted, sxePercentComplete) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxeScheduleTreeID, sxkScheduleBranchID As sxeScheduleBranchID, sxkScheduleTaskID As sxeScheduleTaskID, Convert(tinyint, 2) As sxeScheduleTaskBucketID, Convert(tinyint, 1) As sxeScheduleTypeID, Convert(tinyint, 2) As sxeScheduleTypeBucketID, ");
			stringBuilder.Append("Convert(int, jmsEstimatedSetupHours * 60.0) As sxeMinutes, Convert(int, jmoCompletedSetupHours * 60.0) As sxeCompletedMinutes, jmoSetupComplete as sxeCompleted, jmoSetupPercentComplete as sxePercentComplete ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Insert Into ScheduleTaskBuckets(sxeScheduleTreeID, sxeScheduleBranchID, sxeScheduleTaskID, sxeScheduleTaskBucketID, sxeScheduleTypeID, sxeScheduleTypeBucketID, sxeMinutes, sxeCompletedMinutes, sxeCompleted, sxePercentComplete) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxeScheduleTreeID, sxkScheduleBranchID As sxeScheduleBranchID, sxkScheduleTaskID As sxeScheduleTaskID, Convert(tinyint, 3) As sxeScheduleTaskBucketID, Convert(tinyint, 1) As sxeScheduleTypeID, Convert(tinyint, 3) As sxeScheduleTypeBucketID, ");
			stringBuilder.Append("Convert(int, jmsEstimatedProductionHours * 60.0) As sxeMinutes, Convert(int, jmoCompletedProductionHours * 60.0) As sxeCompletedMinutes, jmoProductionComplete as sxeCompleted, Convert(smallint, Case When jmoEstimatedProductionHours = 0 Then 0 When Round((jmoCompletedProductionHours / jmoEstimatedProductionHours) * 100.0, 0) > 100.0 Then 100.0 Else Round((jmoCompletedProductionHours / jmoEstimatedProductionHours) * 100.0, 0) End) as sxePercentComplete ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Insert Into ScheduleTaskBuckets(sxeScheduleTreeID, sxeScheduleBranchID, sxeScheduleTaskID, sxeScheduleTaskBucketID, sxeScheduleTypeID, sxeScheduleTypeBucketID, sxeMinutes, sxeCompletedMinutes, sxeCompleted, sxePercentComplete) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxeScheduleTreeID, sxkScheduleBranchID As sxeScheduleBranchID, sxkScheduleTaskID As sxeScheduleTaskID, Convert(tinyint, 4) As sxeScheduleTaskBucketID, Convert(tinyint, 1) As sxeScheduleTypeID, Convert(tinyint, 4) As sxeScheduleTypeBucketID, ");
			stringBuilder.Append("Convert(int, jmsMoveTime * 60.0) As sxeMinutes, Convert(int, Case When jmoProductionComplete = 1 Then jmsMoveTime * 60.0 Else 0 End) As sxeCompletedMinutes, jmoProductionComplete as sxeCompleted, Convert(smallint, Case When jmoProductionComplete = 1 Then 100 Else 0 End) as sxePercentComplete ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Insert Into ScheduleTaskBuckets(sxeScheduleTreeID, sxeScheduleBranchID, sxeScheduleTaskID, sxeScheduleTaskBucketID, sxeScheduleTypeID, sxeScheduleTypeBucketID, sxeMinutes, sxeCompletedMinutes, sxeCompleted, sxePercentComplete) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxeScheduleTreeID, sxkScheduleBranchID As sxeScheduleBranchID, sxkScheduleTaskID As sxeScheduleTaskID, Convert(tinyint, 5) As sxeScheduleTaskBucketID, Convert(tinyint, 1) As sxeScheduleTypeID, Convert(tinyint, 5) As sxeScheduleTypeBucketID, ");
			stringBuilder.Append("Convert(int, 0) As sxeMinutes, Convert(int, 0) As sxeCompletedMinutes, jmoProductionComplete as sxeCompleted, Convert(smallint, Case When jmoProductionComplete = 1 Then 100 Else 0 End) as sxePercentComplete ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID\r");
			stringBuilder.Append("Insert Into ScheduleResourceLanes(sxrScheduleTreeID, sxrScheduleBranchID, sxrScheduleTaskID, sxrScheduleResourceLaneID, sxrResourceType, sxrLockedResourceUniqueID, sxrGroupUniqueID) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxrScheduleTreeID, sxkScheduleBranchID As sxrScheduleBranchID, sxkScheduleTaskID As sxrScheduleTaskID, Case When WorkCenterMachineID = 0 Then 1 Else WorkCenterMachineID End As sxrScheduleResourceLaneID, Convert(tinyint, 1) As sxrResourceType, Case When jmsMachineType = 3 Then xaqUniqueID Else Null End As sxrLockedResourceUniqueID, xawUniqueID As sxrGroupUniqueID ");
			stringBuilder.Append("From JobSchedules Inner Join TaskTemp On jmsjobid = JobID And jmsJobAssemblyID = TaskTemp.sxkScheduleBranchID And jmsJobOperationID = TaskTemp.sxkScheduleTaskID And jmsWorkCenterMachineID = TaskTemp.WorkCenterMachineID And jmsJobScenarioID = '' ");
			stringBuilder.Append("Inner Join WorkCenters On jmsWorkCenterID = xawWorkCenterID Left Outer Join WorkCenterMachines On xaqWorkCenterID = jmsWorkCenterID And xaqWorkCenterMachineID = jmsWorkCenterMachineID\r");
			stringBuilder.Append("Insert Into ScheduleAllocations(sxdScheduleTreeID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleResourceLaneID, sxdScheduleAllocationID, sxdDateType, sxdResourceUniqueID, sxdGroupUniqueID, sxdStartActualDateTime, sxdStartDate, sxdStartMinute, sxdEndActualDateTime, sxdEndDate, sxdEndMinute, sxdMinutes) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxdScheduleTreeID, sxkScheduleBranchID As sxdScheduleBranchID, sxkScheduleTaskID As sxdScheduleTaskID, Case When WorkCenterMachineID = 0 Then 1 Else WorkCenterMachineID End As sxdScheduleResourceLaneID, Convert(tinyint, 1) As sxdScheduleAllocationID, Convert(tinyint, 1) As sxdDateType, xaqUniqueID As sxdResourceUniqueID, xawUniqueID As sxdGroupUniqueID, ");
			stringBuilder.Append("jmsQueueStartTime As sxdStartActualDateTime, jmsQueueStartDate As sxdStartDate, Convert(smallint, jmsQueueStartHour * 60.0) As sxdStartMinute, ");
			stringBuilder.Append("jmsStartTime As sxdEndActualDateTime, jmsStartDate As sxdEndDate, Convert(smallint, jmsStartHour * 60.0) As sxdEndMinute, ");
			stringBuilder.Append("Convert(int, jmsQueueTime * 60.0) As sxdMinutes ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID ");
			stringBuilder.Append("Inner Join WorkCenters On jmsWorkCenterID = xawWorkCenterID Left Outer Join WorkCenterMachines On xaqWorkCenterID = jmsWorkCenterID And xaqWorkCenterMachineID = jmsWorkCenterMachineID\r");
			stringBuilder.Append("Insert Into ScheduleAllocations(sxdScheduleTreeID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleResourceLaneID, sxdScheduleAllocationID, sxdDateType, sxdResourceUniqueID, sxdGroupUniqueID, sxdStartActualDateTime, sxdStartDate, sxdStartMinute, sxdEndActualDateTime, sxdEndDate, sxdEndMinute, sxdMinutes) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxdScheduleTreeID, sxkScheduleBranchID As sxdScheduleBranchID, sxkScheduleTaskID As sxdScheduleTaskID, Case When WorkCenterMachineID = 0 Then 1 Else WorkCenterMachineID End As sxdScheduleResourceLaneID, Convert(tinyint, 2) As sxdScheduleAllocationID, Convert(tinyint, 2) As sxdDateType, xaqUniqueID As sxdResourceUniqueID, xawUniqueID As sxdGroupUniqueID, ");
			stringBuilder.Append("jmsStartTime As sxdStartActualDateTime, jmsStartDate As sxdStartDate, Convert(smallint, jmsStartHour * 60.0) As sxdStartMinute, ");
			stringBuilder.Append("jmsProdStartTime As sxdEndActualDateTime, jmsProdStartDate As sxdEndDate, Convert(smallint, jmsProdStartHour * 60.0) As sxdEndMinute, ");
			stringBuilder.Append("Convert(int, jmsEstimatedSetupHours * 60.0) As sxdMinutes ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID ");
			stringBuilder.Append("Inner Join WorkCenters On jmsWorkCenterID = xawWorkCenterID Left Outer Join WorkCenterMachines On xaqWorkCenterID = jmsWorkCenterID And xaqWorkCenterMachineID = jmsWorkCenterMachineID\r");
			stringBuilder.Append("Insert Into ScheduleAllocations(sxdScheduleTreeID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleResourceLaneID, sxdScheduleAllocationID, sxdDateType, sxdResourceUniqueID, sxdGroupUniqueID, sxdStartActualDateTime, sxdStartDate, sxdStartMinute, sxdEndActualDateTime, sxdEndDate, sxdEndMinute, sxdMinutes) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxdScheduleTreeID, sxkScheduleBranchID As sxdScheduleBranchID, sxkScheduleTaskID As sxdScheduleTaskID, Case When WorkCenterMachineID = 0 Then 1 Else WorkCenterMachineID End As sxdScheduleResourceLaneID, Convert(tinyint, 3) As sxdScheduleAllocationID, Convert(tinyint, 3) As sxdDateType, xaqUniqueID As sxdResourceUniqueID, xawUniqueID As sxdGroupUniqueID, ");
			stringBuilder.Append("jmsProdStartTime As sxdStartActualDateTime, jmsProdStartDate As sxdStartDate, Convert(smallint, jmsProdStartHour * 60.0) As sxdStartMinute, ");
			stringBuilder.Append("jmsDueTime As sxdEndActualDateTime, jmsDueDate As sxdEndDate, Convert(smallint, jmsDueHour * 60.0) As sxdEndMinute, ");
			stringBuilder.Append("Convert(int, jmsEstimatedProductionHours * 60.0) As sxdMinutes ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID ");
			stringBuilder.Append("Inner Join WorkCenters On jmsWorkCenterID = xawWorkCenterID Left Outer Join WorkCenterMachines On xaqWorkCenterID = jmsWorkCenterID And xaqWorkCenterMachineID = jmsWorkCenterMachineID\r");
			stringBuilder.Append("Insert Into ScheduleAllocations(sxdScheduleTreeID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleResourceLaneID, sxdScheduleAllocationID, sxdDateType, sxdResourceUniqueID, sxdGroupUniqueID, sxdStartActualDateTime, sxdStartDate, sxdStartMinute, sxdEndActualDateTime, sxdEndDate, sxdEndMinute, sxdMinutes) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxdScheduleTreeID, sxkScheduleBranchID As sxdScheduleBranchID, sxkScheduleTaskID As sxdScheduleTaskID, Case When WorkCenterMachineID = 0 Then 1 Else WorkCenterMachineID End As sxdScheduleResourceLaneID, Convert(tinyint, 4) As sxdScheduleAllocationID, Convert(tinyint, 4) As sxdDateType, xaqUniqueID As sxdResourceUniqueID, xawUniqueID As sxdGroupUniqueID, ");
			stringBuilder.Append("jmsDueTime As sxdStartActualDateTime, jmsDueDate As sxdStartDate, Convert(smallint, jmsDueHour * 60.0) As sxdStartMinute, ");
			stringBuilder.Append("jmsMoveDueTime As sxdEndActualDateTime, jmsMoveDueDate As sxdEndDate, Convert(smallint, jmsMoveDueHour * 60.0) As sxdEndMinute, ");
			stringBuilder.Append("Convert(int, jmsMoveTime * 60.0) As sxdMinutes ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID ");
			stringBuilder.Append("Inner Join WorkCenters On jmsWorkCenterID = xawWorkCenterID Left Outer Join WorkCenterMachines On xaqWorkCenterID = jmsWorkCenterID And xaqWorkCenterMachineID = jmsWorkCenterMachineID\r");
			stringBuilder.Append("Insert Into ScheduleAllocations(sxdScheduleTreeID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleResourceLaneID, sxdScheduleAllocationID, sxdDateType, sxdResourceUniqueID, sxdGroupUniqueID, sxdStartActualDateTime, sxdStartDate, sxdStartMinute, sxdEndActualDateTime, sxdEndDate, sxdEndMinute, sxdMinutes) ");
			stringBuilder.Append("Select sxkScheduleTreeID As sxdScheduleTreeID, sxkScheduleBranchID As sxdScheduleBranchID, sxkScheduleTaskID As sxdScheduleTaskID, Case When WorkCenterMachineID = 0 Then 1 Else WorkCenterMachineID End As sxdScheduleResourceLaneID, Convert(tinyint, 5) As sxdScheduleAllocationID, Convert(tinyint, 5) As sxdDateType, xaqUniqueID As sxdResourceUniqueID, xawUniqueID As sxdGroupUniqueID, ");
			stringBuilder.Append("jmsMoveDueTime As sxdStartActualDateTime, jmsMoveDueDate As sxdStartDate, Convert(smallint, jmsMoveDueHour * 60.0) As sxdStartMinute, ");
			stringBuilder.Append("jmsMoveDueTime As sxdEndActualDateTime, jmsMoveDueDate As sxdEndDate, Convert(smallint, jmsMoveDueHour * 60.0) As sxdEndMinute, ");
			stringBuilder.Append("Convert(int, 0) As sxdMinutes ");
			stringBuilder.Append("From TaskTemp Inner Join JobSchedules On TaskTemp.JobID = jmsJobID And TaskTemp.sxkScheduleBranchID=jmsJobAssemblyID And TaskTemp.sxkScheduleTaskID=jmsJobOperationID And TaskTemp.WorkCenterMachineID=jmsWorkCenterMachineID And jmsJobScenarioID = '' Inner Join JobOperations On jmsJobID=jmoJobID And jmsJobAssemblyID=jmoJobAssemblyID And jmsJobOperationID=jmoJobOperationID ");
			stringBuilder.Append("Inner Join WorkCenters On jmsWorkCenterID = xawWorkCenterID Left Outer Join WorkCenterMachines On xaqWorkCenterID = jmsWorkCenterID And xaqWorkCenterMachineID = jmsWorkCenterMachineID\r");
			stringBuilder.Append("Drop Table TreeTemp\r");
			stringBuilder.Append("Drop Table TaskTemp\r");
			try
			{
				parms.Database.ExecuteCommand(stringBuilder.ToString());
			}
			finally
			{
				if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "TreeTemp"))
				{
					parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "TreeTemp");
				}
				if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "TaskTemp"))
				{
					parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "TaskTemp");
				}
				if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "JobSchedules"))
				{
					parms.Database.ExecuteCommand("TRUNCATE TABLE JobSchedules");
				}
			}
		}
	}

	[DBConversion("9.1.360", "Add fields to DatasetProperties table", "2016-10-07")]
	public class v91360a
	{
		public v91360a(DBConversionParms parms)
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadMaxItemsOnGantt"))
			{
				parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadMaxItemsOnGantt", "int", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			}
		}
	}

	[DBConversion("9.1.438", "Update ImplementationCheckList actions", "2017-03-30")]
	public class v91438a
	{
		public v91438a(DBConversionParms parms)
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'App.OpenObject', 'Forms.OpenObject') where xicAction is not null and xicAction like '%App.OpenObject%';");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'App.OpenForm', 'Forms.OpenForm') where xicAction is not null and xicAction like '%App.OpenForm%';");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'Call App.ShowUserAdministrationForm', 'Forms.OpenForm \"M1.Forms.User.Administration.UserAdministrationForm\"') where xicAction is not null and xicAction like '%Call App.ShowUserAdministrationForm%';");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'Call App.PayrollFunctions.ShowLoadTaxTablesForm(\"Import\")', 'Forms.OpenProcessForm \"M1.Ax.Erp.ImportTaxTableProcess\"') where xicAction is not null and xicAction like '%Call App.PayrollFunctions.ShowLoadTaxTablesForm(\"Import\")%';");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'Call App.Ax(\"PayrollFunctions\").ShowLoadTaxTablesForm(\"Import\")', 'Forms.OpenProcessForm \"M1.Ax.Erp.ImportTaxTableProcess\"') where xicAction is not null and xicAction like '%Call App.Ax(\"PayrollFunctions\").ShowLoadTaxTablesForm(\"Import\")%';");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ImplementationCheckList Set xicAction = Replace(Convert(nvarchar(max), xicAction), 'Call App.PropsShowDataset', 'Call Forms.Show.DatabaseOptions') where xicAction is not null and xicAction like '%Call App.PropsShowDataset%';");
		}
	}

	public v91077(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleDates"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleDates");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleDates", new DmoField[16]
		{
			new DmoField("sxdScheduleTreeID", "int", 4, 0, nullable: false),
			new DmoField("sxdScheduleBranchID", "int", 4, 0, nullable: false),
			new DmoField("sxdScheduleTaskID", "int", 4, 0, nullable: false),
			new DmoField("sxdResourceLane", "smallint", 4, 0, nullable: false),
			new DmoField("sxdScheduleDateID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxdDateType", "tinyint", 1, 0, nullable: false),
			new DmoField("sxdStartDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxdStartHour", "numeric", 5, 2, nullable: false),
			new DmoField("sxdStartActualDateTime", "datetime", 14, 0, nullable: true),
			new DmoField("sxdDuration", "numeric", 8, 2, nullable: false),
			new DmoField("sxdEndDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxdEndHour", "numeric", 5, 2, nullable: false),
			new DmoField("sxdEndActualDateTime", "datetime", 14, 0, nullable: true),
			new DmoField("sxdResourceUniqueID", "uniqueidentifier", 16, 0, nullable: true),
			new DmoField("sxdGroupUniqueID", "uniqueidentifier", 16, 0, nullable: true),
			new DmoField("sxdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("sxdScheduleTreeID,sxdScheduleBranchID,sxdScheduleTaskID,sxdResourceLane,sxdScheduleDateID", unique: true),
			new DmoIndex("sxdUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleResourceLanes"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleResourceLanes");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleResourceLanes", new DmoField[9]
		{
			new DmoField("sxrScheduleTreeID", "int", 4, 0, nullable: false),
			new DmoField("sxrScheduleBranchID", "int", 4, 0, nullable: false),
			new DmoField("sxrScheduleTaskID", "int", 4, 0, nullable: false),
			new DmoField("sxrScheduleResourceLaneID", "smallint", 4, 0, nullable: false),
			new DmoField("sxrResourceTable", "nvarchar", 30, 0, nullable: false),
			new DmoField("sxrLockedResourceUniqueID", "uniqueidentifier", 16, 0, nullable: true),
			new DmoField("sxrGroupTable", "nvarchar", 30, 0, nullable: false),
			new DmoField("sxrGroupUniqueID", "uniqueidentifier", 16, 0, nullable: true),
			new DmoField("sxrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("sxrScheduleTreeID,sxrScheduleBranchID,sxrScheduleTaskID,sxrScheduleResourceLaneID", unique: true),
			new DmoIndex("sxrUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTypes"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleTypes");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTypes", new DmoField[7]
		{
			new DmoField("sxyScheduleTypeID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxyDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("sxyLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("sxyLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("sxyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("sxyCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxyUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("sxyScheduleTypeID", unique: true),
			new DmoIndex("sxyUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTypeBuckets"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleTypeBuckets");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTypeBuckets", new DmoField[6]
		{
			new DmoField("sxuScheduleTypeID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxuScheduleTypeBucketID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxuDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("sxuCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("sxuCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxuUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("sxuScheduleTypeID,sxuScheduleTypeBucketID", unique: true),
			new DmoIndex("sxuUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTasks"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleTasks");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", new DmoField[22]
		{
			new DmoField("sxkScheduleTreeID", "int", 4, 0, nullable: false),
			new DmoField("sxkScheduleBranchID", "int", 4, 0, nullable: false),
			new DmoField("sxkScheduleTaskID", "int", 4, 0, nullable: false),
			new DmoField("sxkPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("sxkPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("sxkProcessID", "nvarchar", 5, 0, nullable: false),
			new DmoField("sxkLinkedTaskID", "int", 4, 0, nullable: false),
			new DmoField("sxkCurrentTaskDateType", "tinyint", 1, 0, nullable: false),
			new DmoField("sxkLinkedTaskDateType", "tinyint", 1, 0, nullable: false),
			new DmoField("sxkOffsetTime", "numeric", 8, 2, nullable: false),
			new DmoField("sxkScheduleTypeID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxkStartDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxkStartHour", "numeric", 5, 2, nullable: false),
			new DmoField("sxkStartActualDateTime", "datetime", 14, 0, nullable: true),
			new DmoField("sxkDuration", "numeric", 8, 2, nullable: false),
			new DmoField("sxkEndDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxkEndHour", "numeric", 5, 2, nullable: false),
			new DmoField("sxkEndActualDateTime", "datetime", 14, 0, nullable: true),
			new DmoField("sxkExchangeID", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("sxkCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("sxkCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("sxkScheduleTreeID,sxkScheduleBranchID,sxkScheduleTaskID", unique: true),
			new DmoIndex("sxkUniqueID", unique: true),
			new DmoIndex("sxkPlantID", unique: false),
			new DmoIndex("sxkPlantDepartmentID", unique: false)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleResourceCells"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleResourceCells");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleResourceCells", new DmoField[7]
		{
			new DmoField("sxcTreeID", "int", 4, 0, nullable: false),
			new DmoField("sxcBranchID", "int", 4, 0, nullable: false),
			new DmoField("sxcTaskID", "int", 4, 0, nullable: false),
			new DmoField("sxcResourceLaneID", "smallint", 1, 0, nullable: false),
			new DmoField("sxcResourceCellID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxcResourceUniqueID", "uniqueidentifier", 16, 0, nullable: true),
			new DmoField("sxcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("sxcTreeID,sxcBranchID,sxcTaskID,sxcResourceLaneID,sxcResourceCellID", unique: true),
			new DmoIndex("sxcUniqueID", unique: true)
		});
	}
}
