using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.197", "Modify BankAccountNumber length in various tables", "2017-03-22")]
public class v92197a
{
	public v92197a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "BankAccounts", "glnBankAccountNumber"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "BankAccounts", "glnBankAccountNumber", "nvarchar", 24, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PayrollHeaderBankAccounts", "paaBankAccountNumber"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderBankAccounts", "paaBankAccountNumber", "nvarchar", 24, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentHeaders", "artBankAccountNumber"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentHeaders", "artBankAccountNumber", "nvarchar", 24, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "EmployeeBankAccounts", "pabBankAccountNumber"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeBankAccounts", "pabBankAccountNumber", "nvarchar", 24, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APPaymentHeaders", "aptBankAccountNumber"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentHeaders", "aptBankAccountNumber", "nvarchar", 24, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlBankAccountNumber"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlBankAccountNumber", "nvarchar", 24, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoBankAccountNumber"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoBankAccountNumber", "nvarchar", 24, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APTaxablePaymentTotals", "tptPayeeBankAccountNumber"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APTaxablePaymentTotals", "tptPayeeBankAccountNumber", "nvarchar", 24, 0, isNullable: false, parms.Messages);
		}
	}
}
