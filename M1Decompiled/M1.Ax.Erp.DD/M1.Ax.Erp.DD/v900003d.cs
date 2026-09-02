using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.003", "Add fields to Receipts table", "2014-09-25")]
public class v900003d
{
	public v900003d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Receipts", "rmpLandedCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Receipts", "rmpLandedCost", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
