using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.20.005", "Add Include Freight in Deposit Calc to Financial", "2009-03-26")]
public class v720005c
{
	public v720005c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafARIncludeFrgtInDepositCalc"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafARIncludeFrgtInDepositCalc", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
