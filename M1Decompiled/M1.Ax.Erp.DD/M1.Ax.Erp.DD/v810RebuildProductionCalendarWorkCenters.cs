using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ProductionCalendarWorkCenters to support unicode", "2013-10-17")]
public class v810RebuildProductionCalendarWorkCenters
{
	public v810RebuildProductionCalendarWorkCenters(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendarWorkCenters", new DmoField[6]
		{
			new DmoField("jmrProductionCalendarYearID", "smallint", 4, 0, nullable: false),
			new DmoField("jmrProductionCalendarLineID", "smallint", 4, 0, nullable: false),
			new DmoField("jmrWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("jmrCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmrCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("jmrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("JMRPRODUCTIONCALENDARYEARID,JMRPRODUCTIONCALENDARLINEID", unique: true),
			new DmoIndex("JMRUNIQUEID", unique: true),
			new DmoIndex("jmrProductionCalendarYearID", unique: false),
			new DmoIndex("jmrProductionCalendarLineID", unique: false),
			new DmoIndex("jmrWorkCenterID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
