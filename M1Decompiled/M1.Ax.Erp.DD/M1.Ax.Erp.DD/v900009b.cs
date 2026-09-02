using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.009", "Add fields to MaterialIssueLines table", "2014-10-31")]
public class v900009b
{
	public v900009b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injReverseIssue"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injReverseIssue", dropTriggers: true);
		}
	}
}
