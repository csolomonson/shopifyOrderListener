using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.085", "Add fields to MaterialIssues table", "2017-01-24")]
public class v92085c
{
	public v92085c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssues", "iniSourceTableUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssues", "iniSourceTableUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
