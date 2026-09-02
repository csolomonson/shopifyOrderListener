using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.200", "Add fields to MRPJobDetails table", "2023-05-09")]
public class v96200d
{
	public v96200d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPJobDetails", "mrjFirm"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPJobDetails", "mrjFirm", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
