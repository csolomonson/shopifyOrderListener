using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.094", "Add fields to Organizations table", "2015-10-16")]
public class v900094b
{
	public v900094b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoSplitPercentTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoSplitPercentTotal", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
