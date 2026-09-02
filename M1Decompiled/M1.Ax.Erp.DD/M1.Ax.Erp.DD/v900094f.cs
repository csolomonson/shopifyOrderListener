using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.094", "Add fields to Quotes table", "2015-10-16")]
public class v900094f
{
	public v900094f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Quotes", "qmpSplitPercentTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Quotes", "qmpSplitPercentTotal", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
