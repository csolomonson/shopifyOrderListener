using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.009", "Update field bindings", "2014-10-31")]
public class v900009g
{
	public v900009g(DBConversionParms parms)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin SET NOCOUNT ON DECLARE @SQL3 varchar(8000) set @sql3 = ' Update MaterialIssueLines Set injPosted = iniPosted From MaterialIssues Inner Join MaterialIssueLines On iniMaterialIssueID = injMaterialIssueID; Update MaterialIssueComponents Set inkPosted = injPosted From MaterialIssueLines Inner Join MaterialIssueComponents On injMaterialIssueID = inkMaterialIssueID And injMaterialIssueLineID = inkMaterialIssueLineID; ' exec(@sql3) SET NOCOUNT OFF end;");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
	}
}
