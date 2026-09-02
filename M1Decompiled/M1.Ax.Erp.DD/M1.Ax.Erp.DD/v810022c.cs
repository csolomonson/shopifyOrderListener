using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.022", "Add Total Fields to APRecurringPayments table", "2013-03-18")]
public class v810022c
{
	public v810022c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPaymentLines", "apqPaymentType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPaymentLines", "apqPaymentType", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APRecurringPaymentLines Set apqPaymentType = aprPaymentType From APRecurringPaymentLines Inner Join APRecurringPayments On apqRecurringPaymentID = aprRecurringPaymentID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPayments", "aprPaymentTotalBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPayments", "aprPaymentTotalBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APRecurringPayments Set aprPaymentTotalBase = apqPaymentAmount From APRecurringPayments Inner Join (Select apqRecurringPaymentID,Sum(apqPaymentAmount) As apqPaymentAmount From APRecurringPaymentLines Group By apqRecurringPaymentID) As APRecurringPaymentLines On apqRecurringPaymentID = aprRecurringPaymentID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPayments", "aprPaymentTotalForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPayments", "aprPaymentTotalForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APRecurringPayments Set aprPaymentTotalForeign = apqPaymentAmountForeign From APRecurringPayments Inner Join (Select apqRecurringPaymentID,Sum(apqPaymentAmountForeign) As apqPaymentAmountForeign From APRecurringPaymentLines Group By apqRecurringPaymentID) As APRecurringPaymentLines On apqRecurringPaymentID = aprRecurringPaymentID");
		}
	}
}
