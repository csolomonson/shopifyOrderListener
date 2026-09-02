using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Add a new Data Missing into MRPJobDetails", "2022-01-21")]
public class v95200l
{
	public v95200l(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPJobDetails", "mrjDataMissing"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPJobDetails", "mrjDataMissing", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
