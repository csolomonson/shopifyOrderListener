using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.034", "Add fields to MaterialIssueComponents table", "2016-11-29")]
public class v92034a
{
	public v92034a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueComponents", "inkReverseMaterialIssueCompID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueComponents", "inkReverseMaterialIssueCompID", "int", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueComponents", "inkReverseMaterialIssueLineID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueComponents", "inkReverseMaterialIssueLineID", "smallint", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueComponents", "inkReverseMaterialIssueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueComponents", "inkReverseMaterialIssueID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
