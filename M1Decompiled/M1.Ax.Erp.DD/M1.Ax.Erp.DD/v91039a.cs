using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.039", "Add fields to JobAssemblies table", "2016-04-13")]
public class v91039a
{
	public v91039a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaReworkDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaReworkDate", "datetime", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
