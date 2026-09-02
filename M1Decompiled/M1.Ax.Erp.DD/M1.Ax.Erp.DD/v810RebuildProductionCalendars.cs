using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ProductionCalendars to support unicode", "2013-10-17")]
public class v810RebuildProductionCalendars
{
	public v810RebuildProductionCalendars(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionCalendars", new DmoField[4]
		{
			new DmoField("jmlProductionCalendarYearID", "smallint", 4, 0, nullable: false),
			new DmoField("jmlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("jmlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("JMLPRODUCTIONCALENDARYEARID", unique: true),
			new DmoIndex("JMLUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
