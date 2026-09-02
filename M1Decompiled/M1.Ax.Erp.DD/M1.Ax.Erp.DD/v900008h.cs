using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.008", "Update field bindings", "2014-10-23")]
public class v900008h
{
	public v900008h(DBConversionParms parms)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin SET NOCOUNT ON DECLARE @SQL3 varchar(8000) set @sql3 = ' Update ReceiptLines Set rmlClosed = rmpClosed From Receipts Inner Join ReceiptLines On RMPRECEIPTID = RMLRECEIPTID; Update ReceiptComponents Set rmoClosed = rmpClosed From Receipts Inner Join ReceiptComponents On RMPRECEIPTID = RMORECEIPTID; Update ReceiptComponents Set rmoJobParentQuantity = rmlJobMatQuantityReceived From ReceiptLines Inner Join ReceiptComponents On RMLRECEIPTID = RMORECEIPTID And RMLRECEIPTLINEID = RMORECEIPTLINEID; Update JobMaterialComponents Set jmtParentQuantity = jmmEstimatedQuantity From JobMaterials Inner Join JobMaterialComponents On JMMJOBID = JMTJOBID And JMMJOBASSEMBLYID = JMTJOBASSEMBLYID And JMMJOBMATERIALID = JMTJOBMATERIALID; Update MaterialIssueComponents Set inkJobParentQuantity = injJobMatIssueQuantity From MaterialIssueLines Inner Join MaterialIssueComponents On injMaterialIssueID = inkMaterialIssueID And injMaterialIssueLineID = inkMaterialIssueLineID; Update MaterialIssueComponents Set inkInvParentQuantity = injInvIssueQuantity From MaterialIssueLines Inner Join MaterialIssueComponents On injMaterialIssueID = inkMaterialIssueID And injMaterialIssueLineID = inkMaterialIssueLineID; Update MaterialIssueComponents Set inkReceivedComplete = injIssueComplete From MaterialIssueLines Inner Join MaterialIssueComponents On injMaterialIssueID = inkMaterialIssueID And injMaterialIssueLineID = inkMaterialIssueLineID; ' exec(@sql3) SET NOCOUNT OFF end;");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
	}
}
