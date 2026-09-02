using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.20.005", "Add Unrealised exchange amount to payment lines", "2009-03-26")]
public class v720005a
{
	public v720005a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APPaymentLines", "apnUnrealisedExchangeAmt"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentLines", "apnUnrealisedExchangeAmt", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APPaymentLines", "apnUnrealisedExGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentLines", "apnUnrealisedExGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentLines", "arnUnrealisedExchangeAmt"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentLines", "arnUnrealisedExchangeAmt", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentLines", "arnUnrealisedExGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentLines", "arnUnrealisedExGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
