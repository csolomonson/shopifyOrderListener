using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.067", "Update field bindings", "2015-07-30")]
public class v900067c
{
	public v900067c(DBConversionParms parms)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin SET NOCOUNT ON DECLARE @SQL3 varchar(8000) set @sql3 = ' Update MaterialIssueComponents Set inkInvParentQuantityScrap = injInvScrapQuantity From MaterialIssueLines Inner Join MaterialIssueComponents On injMaterialIssueID = inkMaterialIssueID And injMaterialIssueLineID = inkMaterialIssueLineID; Update MaterialIssueComponents Set inkJobMatParentQuantityScrap = injJobMatScrapQuantity From MaterialIssueLines Inner Join MaterialIssueComponents On injMaterialIssueID = inkMaterialIssueID And injMaterialIssueLineID = inkMaterialIssueLineID; Update ShipmentComponents Set smoJobParentQuantity = smlJobQuantityShipped From ShipmentLines Inner Join ShipmentComponents On SMLSHIPMENTID = SMOSHIPMENTID And SMLSHIPMENTLINEID = SMOSHIPMENTLINEID; ' exec(@sql3) SET NOCOUNT OFF end;");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
	}
}
