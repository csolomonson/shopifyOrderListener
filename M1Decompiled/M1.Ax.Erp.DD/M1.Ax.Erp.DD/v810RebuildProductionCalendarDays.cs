using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ProductionCalendarDays to support unicode", "2013-10-17")]
public class v810RebuildProductionCalendarDays
{
	public v810RebuildProductionCalendarDays(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendarDays", new DmoField[8]
		{
			new DmoField("jmyProductionCalendarYearID", "smallint", 4, 0, nullable: false),
			new DmoField("jmyProductionCalendarMonth", "tinyint", 2, 0, nullable: false),
			new DmoField("jmyProductionCalendarDay", "tinyint", 2, 0, nullable: false),
			new DmoField("jmyWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("jmyHours", "numeric", 5, 2, nullable: false),
			new DmoField("jmyDayStartTime", "numeric", 5, 2, nullable: false),
			new DmoField("jmyDayOfWeek", "tinyint", 1, 0, nullable: false),
			new DmoField("jmyHoliday", "bit", 1, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("JMYPRODUCTIONCALENDARYEARID,JMYPRODUCTIONCALENDARMONTH,JMYPRODUCTIONCALENDARDAY,JMYWORKCENTERID", unique: true),
			new DmoIndex("jmyWorkCenterID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
