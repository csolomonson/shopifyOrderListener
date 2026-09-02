using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.035", "Add salary sacrifice fields to Payroll Header/Line", "2008-03-03")]
public class v700035b
{
	public v700035b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPayments", "aprCurrencyRateID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPayments", "aprCurrencyRateID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APRecurringPayments Set aprCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) ");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPaymentLines", "apqPaymentAmountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPaymentLines", "apqPaymentAmountForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPaymentLines", "apqTaxAmountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPaymentLines", "apqTaxAmountForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPaymentLines", "apqSecondTaxAmountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPaymentLines", "apqSecondTaxAmountForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPaymentLines", "apqCurrencyRateID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPaymentLines", "apqCurrencyRateID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPaymentLines", "apqCustomRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPaymentLines", "apqCustomRate", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APRecurringPaymentLines", "apqExchangeRate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPaymentLines", "apqExchangeRate", "numeric", 13, 6, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APRecurringPaymentLines Set apqCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES), apqExchangeRate = 1, apqCustomRate = 0, apqPaymentAmountForeign = apqPaymentAmount, apqTaxAmountForeign = apqTaxAmount, apqSecondTaxAmountForeign = apqSecondTaxAmount ");
		}
	}
}
