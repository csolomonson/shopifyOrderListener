using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.003", "Add fields to PartTransactions table", "2014-09-25")]
public class v900003k
{
	public v900003k(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtJobCompleteStatus"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", "imtJobCompleteStatus", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
