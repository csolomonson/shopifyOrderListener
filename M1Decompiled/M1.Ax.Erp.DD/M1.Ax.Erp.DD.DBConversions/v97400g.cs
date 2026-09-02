using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.400", "Add field rmpFlaggedForWMS to Receipts table", "2024-09-18")]
public class v97400g
{
	public v97400g(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Receipts", "rmpFlaggedForWMS"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Receipts", "rmpFlaggedForWMS", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
