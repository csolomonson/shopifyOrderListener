using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.039", "Add fields to Jobs table", "2016-04-13")]
public class v91039b
{
	public v91039b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Jobs", "jmpReworkDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Jobs", "jmpReworkDate", "datetime", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
