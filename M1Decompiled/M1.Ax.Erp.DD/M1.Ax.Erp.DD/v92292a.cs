using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.292", "Add fields to APInvoiceExpenseAccounts table", "2017-06-11")]
public class v92292a
{
	public v92292a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoiceExpenseAccounts", "apxSourceTableName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceExpenseAccounts", "apxSourceTableName", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "APInvoiceExpenseAccounts", "apxSourceTableUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceExpenseAccounts", "apxSourceTableUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
