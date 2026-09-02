using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add Inactive flag to AP Recurring Payments", "2008-06-04")]
public class v710000v
{
	public v710000v(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPayments", "aprInactive"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPayments", "aprInactive", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPayments", "aprInactiveDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPayments", "aprInactiveDate", "date", 14, 3, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPaymentLines", "apqInactive"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPaymentLines", "apqInactive", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
