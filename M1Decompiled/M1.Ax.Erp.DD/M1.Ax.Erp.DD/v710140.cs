using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.140", "Add Unrealised Exchange Gain/Loss to Currency Rate", "2008-09-23")]
public class v710140
{
	public v710140(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "CurrencyRates", "mcpUnrealisedExGainGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CurrencyRates", "mcpUnrealisedExGainGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "CurrencyRates", "mcpUnrealisedExLossGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CurrencyRates", "mcpUnrealisedExLossGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
