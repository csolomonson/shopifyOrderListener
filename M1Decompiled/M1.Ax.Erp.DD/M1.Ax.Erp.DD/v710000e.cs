using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add Retention fields to AP Invoice/Payment Lines", "2008-04-04")]
public class v710000e
{
	public v710000e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoices", "appRetentionBalanceBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoices", "appRetentionBalanceBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoices", "appRetentionBalanceForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoices", "appRetentionBalanceForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APInvoices Set appRetentionBalanceBase = appRetentionTotalBase, appRetentionBalanceForeign = appRetentionTotalForeign ");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APPaymentLines", "apnRetentionPaymentAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentLines", "apnRetentionPaymentAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APPaymentLines", "apnRetentionPayAmtForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentLines", "apnRetentionPayAmtForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
