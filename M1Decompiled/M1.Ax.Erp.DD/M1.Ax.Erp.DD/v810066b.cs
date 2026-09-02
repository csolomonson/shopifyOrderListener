using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.066", "Add fields to PartRevisions table", "2014-01-28")]
public class v810066b
{
	public v810066b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrLastTransactionDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrLastTransactionDate", "datetime", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
