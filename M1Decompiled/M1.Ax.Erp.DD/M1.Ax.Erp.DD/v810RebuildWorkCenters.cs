using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WorkCenters to support unicode", "2013-10-17")]
public class v810RebuildWorkCenters
{
	public v810RebuildWorkCenters(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenters", new DmoField[43]
		{
			new DmoField("xawWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xawDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xawPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xawProductionDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xawProcessID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xawSetupHours", "numeric", 8, 2, nullable: false),
			new DmoField("xawStandardFactor", "nvarchar", 2, 0, nullable: false),
			new DmoField("xawProductionStandard", "numeric", 10, 4, nullable: false),
			new DmoField("xawNumberOfMachines", "smallint", 3, 0, nullable: false),
			new DmoField("xawPeoplePerMachine", "smallint", 3, 0, nullable: false),
			new DmoField("xawExcludeFromShopLoad", "bit", 1, 0, nullable: false),
			new DmoField("xawHoursMon", "numeric", 5, 2, nullable: false),
			new DmoField("xawHoursTue", "numeric", 5, 2, nullable: false),
			new DmoField("xawHoursWed", "numeric", 5, 2, nullable: false),
			new DmoField("xawHoursThu", "numeric", 5, 2, nullable: false),
			new DmoField("xawHoursFri", "numeric", 5, 2, nullable: false),
			new DmoField("xawHoursSat", "numeric", 5, 2, nullable: false),
			new DmoField("xawHoursSun", "numeric", 5, 2, nullable: false),
			new DmoField("xawDayStartTimeMon", "numeric", 5, 2, nullable: false),
			new DmoField("xawDayStartTimeTue", "numeric", 5, 2, nullable: false),
			new DmoField("xawDayStartTimeWed", "numeric", 5, 2, nullable: false),
			new DmoField("xawDayStartTimeThu", "numeric", 5, 2, nullable: false),
			new DmoField("xawDayStartTimeFri", "numeric", 5, 2, nullable: false),
			new DmoField("xawDayStartTimeSat", "numeric", 5, 2, nullable: false),
			new DmoField("xawDayStartTimeSun", "numeric", 5, 2, nullable: false),
			new DmoField("xawQueueTime", "numeric", 6, 2, nullable: false),
			new DmoField("xawMoveTime", "numeric", 6, 2, nullable: false),
			new DmoField("xawOutsideProcessing", "bit", 1, 0, nullable: false),
			new DmoField("xawFiniteTolerance", "numeric", 5, 2, nullable: false),
			new DmoField("xawOverheadRate", "numeric", 8, 2, nullable: false),
			new DmoField("xawQuotingRate", "numeric", 8, 2, nullable: false),
			new DmoField("xawOverheadCalculationType", "tinyint", 1, 0, nullable: false),
			new DmoField("xawSplitMachineHours", "bit", 1, 0, nullable: false),
			new DmoField("xawSetMachineToLaborHours", "bit", 1, 0, nullable: false),
			new DmoField("xawExportToCalendar", "bit", 1, 0, nullable: false),
			new DmoField("xawCalendarLocation", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xawCalendarColor", "tinyint", 2, 0, nullable: false),
			new DmoField("xawStartHour", "numeric", 5, 2, nullable: false),
			new DmoField("xawInactive", "bit", 1, 0, nullable: false),
			new DmoField("xawInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("xawCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xawCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xawUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("XAWWORKCENTERID", unique: true),
			new DmoIndex("XAWUNIQUEID", unique: true),
			new DmoIndex("xawPlantID", unique: false),
			new DmoIndex("xawProductionDepartmentID", unique: false),
			new DmoIndex("xawOutsideProcessing", unique: false),
			new DmoIndex("xawExportToCalendar", unique: false),
			new DmoIndex("xawInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
