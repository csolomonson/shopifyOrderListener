using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.3.100", "Add transaction type to IntegrationTransactionQueue table", "2021-03-09")]
public class v93100d
{
	public v93100d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IntegrationTransactionQueue", "itqTransactionType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IntegrationTransactionQueue", "itqTransactionType", "nvarchar", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
