using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.049", "Add fields to Jobs table", "2015-06-22")]
public class v900049b
{
	public v900049b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Jobs", "jmpReworkQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Jobs", "jmpReworkQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
