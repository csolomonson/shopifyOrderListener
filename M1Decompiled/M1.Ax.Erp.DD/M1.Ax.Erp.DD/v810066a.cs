using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.066", "Add fields to PartTransactions table", "2014-01-28")]
public class v810066a
{
	public v810066a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtTableName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", "imtTableName", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtTableUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", "imtTableUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
