using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.350", "Add language code field on Bank Accounts table", "2022-08-11")]
public class v95350a
{
	public v95350a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "BankAccounts", "glnLanguageCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "BankAccounts", "glnLanguageCode", "nvarchar", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
