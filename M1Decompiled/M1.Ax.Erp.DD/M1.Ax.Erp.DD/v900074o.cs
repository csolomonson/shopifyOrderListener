using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Update field bindings", "2015-08-14")]
public class v900074o
{
	public v900074o(DBConversionParms parms)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin SET NOCOUNT ON DECLARE @SQL3 varchar(8000) set @sql3 = ' Update MfgReceiptComponents Set rmnPosted = rmmPosted From MfgReceipts Inner Join MfgReceiptComponents On rmmMfgReceiptID = rmnMfgReceiptID; Update MfgReceiptComponents Set rmnInvParentQuantity = rmmMiscInvQuantityReceived From MfgReceipts Inner Join MfgReceiptComponents On rmmMfgReceiptID = rmnMfgReceiptID; Update MfgReceiptComponents Set rmnJobMatParentQuantity = rmmJobMatQuantityReceived From MfgReceipts Inner Join MfgReceiptComponents On rmmMfgReceiptID = rmnMfgReceiptID; Update MfgReceiptComponents Set rmnReceivedComplete = rmmReceivedComplete From MfgReceipts Inner Join MfgReceiptComponents On rmmMfgReceiptID = rmnMfgReceiptID; Update InspectionComponents Set qamInvParentQtyToScrap = qalInvQuantityToScrap From InspectionLines Inner Join InspectionComponents On QALINSPECTIONID = qamInspectionID And QALINSPECTIONLINEID = qamInspectionLineID; Update InspectionComponents Set qamInvParentQtyAccepted = qalInvQuantityAccepted From InspectionLines Inner Join InspectionComponents On QALINSPECTIONID = qamInspectionID And QALINSPECTIONLINEID = qamInspectionLineID; Update InspectionComponents Set qamInvParentQtyToReturn = qalInvQuantityToReturn From InspectionLines Inner Join InspectionComponents On QALINSPECTIONID = qamInspectionID And QALINSPECTIONLINEID = qamInspectionLineID; Update InspectionComponents Set qamInspectionType = qalInspectionType From InspectionLines Inner Join InspectionComponents On QALINSPECTIONID = qamInspectionID And QALINSPECTIONLINEID = qamInspectionLineID; Update DMRClaimComponents Set dmoParentQuantity = dmlQuantity From DMRClaimLines Inner Join DMRClaimComponents On DMLDMRCLAIMID = dmoDMRClaimID And DMLDMRCLAIMLINEID = dmoDMRClaimLineID; Update DMRClaimComponents Set dmoShippedComplete = dmlShippedComplete From DMRClaimLines Inner Join DMRClaimComponents On DMLDMRCLAIMID = dmoDMRClaimID And DMLDMRCLAIMLINEID = dmoDMRClaimLineID; Update RMAReceiptComponents Set rroInspectionComplete = rrlInspectionComplete From RMAReceiptLines Inner Join RMAReceiptComponents On RRLRMARECEIPTID = rroRMAReceiptID And RRLRMARECEIPTLINEID = rroRMAReceiptLineID; ' exec(@sql3) SET NOCOUNT OFF end;");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
	}
}
