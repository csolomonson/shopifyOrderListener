using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.008", "Add fields to MaterialIssues table", "2014-10-23")]
public class v900008b
{
	public v900008b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssues", "iniMaterialIssueID"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssues", new DmoIndex[1]
			{
				new DmoIndex("iniMaterialIssueID", unique: false)
			}, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssues", "iniPostedDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssues", "iniPostedDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssues", "iniPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssues", "iniPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssues", "iniProjectID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssues", "iniProjectID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssues", "iniClosed"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssues", "iniClosed", dropTriggers: true);
		}
	}
}
