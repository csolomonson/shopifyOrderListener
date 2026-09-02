using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.100", "Drop fields from MaterialIssues table", "2015-10-30")]
public class v900100a
{
	public v900100a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssues", "iniProjectID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssues", "iniProjectID", dropTriggers: true);
		}
	}
}
