using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.20.005", "Add Exchange Amount fields to Payment Headers", "2009-05-19")]
public class v720005d
{
	public v720005d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentHeaders", "artExchangeAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentHeaders", "artExchangeAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentHeaders", "artExchangeGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentHeaders", "artExchangeGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APPaymentHeaders", "aptExchangeAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentHeaders", "aptExchangeAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APPaymentHeaders", "aptExchangeGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentHeaders", "aptExchangeGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
