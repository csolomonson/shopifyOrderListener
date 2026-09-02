using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert CurrencyRates to support unicode", "2013-10-17")]
public class v810RebuildCurrencyRates
{
	public v810RebuildCurrencyRates(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CurrencyRates", new DmoField[12]
		{
			new DmoField("mcpCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("mcpSymbol", "nvarchar", 4, 0, nullable: false),
			new DmoField("mcpDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("mcpExchangeGainGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("mcpARGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("mcpAPGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("mcpExchangeLossGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("mcpUnrealisedExGainGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("mcpUnrealisedExLossGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("mcpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("mcpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("mcpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("MCPCURRENCYRATEID", unique: true),
			new DmoIndex("MCPUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
