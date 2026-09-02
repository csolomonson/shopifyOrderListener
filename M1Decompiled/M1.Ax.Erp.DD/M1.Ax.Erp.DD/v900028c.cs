using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.028", "Add fields to FinancialProperties table", "2015-04-08")]
public class v900028c
{
	public v900028c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafARFinanceShowCreditBalance"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafARFinanceShowCreditBalance", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FinancialProperties Set xafARFinanceShowCreditBalance = 1");
		}
	}
}
