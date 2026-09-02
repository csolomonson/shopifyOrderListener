using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartForecastLines to support unicode", "2013-10-17")]
public class v810RebuildPartForecastLines
{
	public v810RebuildPartForecastLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartForecastLines", new DmoField[13]
		{
			new DmoField("inlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("inlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("inlPartForecastYearID", "smallint", 4, 0, nullable: false),
			new DmoField("inlPartForecastPeriodID", "smallint", 4, 0, nullable: false),
			new DmoField("inlUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("inlStartDate", "date", 14, 0, nullable: true),
			new DmoField("inlEndDate", "date", 14, 0, nullable: true),
			new DmoField("inlForecastQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("inlForecastBalance", "numeric", 15, 5, nullable: false),
			new DmoField("inlRemainingQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("inlActualQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("inlActualBalance", "numeric", 15, 5, nullable: false),
			new DmoField("inlRemainingQuantityBalance", "numeric", 15, 5, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("INLPARTID,INLPARTREVISIONID,INLPARTFORECASTYEARID,INLPARTFORECASTPERIODID", unique: true),
			new DmoIndex("INLUNIQUEID", unique: true),
			new DmoIndex("inlPartID", unique: false),
			new DmoIndex("inlPartRevisionID", unique: false),
			new DmoIndex("inlPartForecastYearID", unique: false),
			new DmoIndex("inlPartForecastPeriodID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
