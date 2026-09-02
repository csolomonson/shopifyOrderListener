using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.400", "Add field jmpNestlinkProcessed to Jobs table", "2024-05-20")]
public class v97400c
{
	public v97400c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Jobs", "jmpNestlinkProcessed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Jobs", "jmpNestlinkProcessed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
