using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.024", "Add fields to RMAReceipts table", "2016-11-18")]
public class v92021a
{
	public v92021a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceipts", "rrpReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAReceipts", "rrpReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
