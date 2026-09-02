using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.400", "Add field rmpNestlinkProcessed to Receipts table", "2024-05-20")]
public class v97400b
{
	public v97400b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Receipts", "rmpNestlinkProcessed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Receipts", "rmpNestlinkProcessed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
