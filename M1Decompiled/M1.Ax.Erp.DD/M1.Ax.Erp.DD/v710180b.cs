using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.180", "Add Stock In Transit GL Accounts", "2008-11-18")]
public class v710180b
{
	public v710180b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafStockInTransitGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafStockInTransitGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauStockInTransitGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauStockInTransitGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
