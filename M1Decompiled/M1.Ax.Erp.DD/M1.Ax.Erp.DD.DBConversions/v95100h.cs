using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.100", "Update column definitions on IntegrationTransactionQueue table", "2021-10-12")]
public class v95100h
{
	public v95100h(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IntegrationTransactionQueue", "itqEntityType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IntegrationTransactionQueue", "itqEntityType", "nvarchar", 50, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IntegrationTransactionQueue", "itqSourceTableName"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IntegrationTransactionQueue", "itqSourceTableName", "nvarchar", 60, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IntegrationTransactionQueue", "itqTransactionType"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IntegrationTransactionQueue", "itqTransactionType", "nvarchar", 100, 0, isNullable: true, parms.Messages);
		}
	}
}
