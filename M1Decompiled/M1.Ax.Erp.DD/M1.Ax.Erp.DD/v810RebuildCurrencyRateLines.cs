using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert CurrencyRateLines to support unicode", "2013-10-17")]
public class v810RebuildCurrencyRateLines
{
	public v810RebuildCurrencyRateLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CurrencyRateLines", new DmoField[8]
		{
			new DmoField("mclCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("mclCurrencyRateLineID", "int", 7, 0, nullable: false),
			new DmoField("mclEffectiveDate", "date", 14, 0, nullable: true),
			new DmoField("mclExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("mclReference", "nvarchar", 50, 0, nullable: false),
			new DmoField("mclCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("mclCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("mclUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("MCLCURRENCYRATEID,MCLCURRENCYRATELINEID", unique: true),
			new DmoIndex("MCLUNIQUEID", unique: true),
			new DmoIndex("mclCurrencyRateID", unique: false),
			new DmoIndex("mclCurrencyRateLineID", unique: false),
			new DmoIndex("mclEffectiveDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
