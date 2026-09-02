using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.609", "Add fields to JobAssemblies table", "2018-01-04")]
public class v92609a
{
	public v92609a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobAssemblies", "jmaOverlapType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobAssemblies", "jmaOverlapType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
