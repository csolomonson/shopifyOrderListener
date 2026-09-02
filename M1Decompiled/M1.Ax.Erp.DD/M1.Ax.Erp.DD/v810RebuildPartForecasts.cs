using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartForecasts to support unicode", "2013-10-17")]
public class v810RebuildPartForecasts
{
	public v810RebuildPartForecasts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartForecasts", new DmoField[10]
		{
			new DmoField("inpPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("inpPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("inpPartForecastYearID", "smallint", 4, 0, nullable: false),
			new DmoField("inpIntervalType", "nvarchar", 1, 0, nullable: false),
			new DmoField("inpAnnualQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("inpUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("inpStartDate", "date", 14, 0, nullable: true),
			new DmoField("inpEndDate", "date", 14, 0, nullable: true),
			new DmoField("inpForecastMethod", "nvarchar", 1, 0, nullable: false),
			new DmoField("inpForecastNumberOfYears", "tinyint", 2, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("INPPARTID,INPPARTREVISIONID,INPPARTFORECASTYEARID", unique: true),
			new DmoIndex("INPUNIQUEID", unique: true),
			new DmoIndex("inpPartID", unique: false),
			new DmoIndex("inpPartRevisionID", unique: false),
			new DmoIndex("inpPartForecastYearID", unique: false),
			new DmoIndex("inpForecastMethod", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
