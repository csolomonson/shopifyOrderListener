using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.049", "Add fields to JobAssemblies table", "2015-06-22")]
public class v900049e
{
	public v900049e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaReworkQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaReworkQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
