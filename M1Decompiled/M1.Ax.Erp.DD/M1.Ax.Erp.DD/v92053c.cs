using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.053", "Remove field from MaterialIssueLines", "2016-12-13")]
public class v92053c
{
	public v92053c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines", "injReverseIssue"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update MaterialIssues Set iniReversalEntry = 1 From MaterialIssueLines Inner Join MaterialIssues on injMaterialIssueID = iniMaterialIssueID Where injReverseIssue = 1");
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", "injReverseIssue", dropTriggers: true);
		}
	}
}
