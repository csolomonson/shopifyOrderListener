using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARPayflowPro to support unicode", "2013-10-17")]
public class v810RebuildARPayflowPro
{
	public v810RebuildARPayflowPro(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPayflowPro", new DmoField[34]
		{
			new DmoField("arxPayflowProID", "identity", 10, 0, nullable: false),
			new DmoField("arxUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("arxPNRef", "nvarchar", 12, 0, nullable: false),
			new DmoField("arxPPRef", "nvarchar", 12, 0, nullable: false),
			new DmoField("arxResult", "numeric", 10, 0, nullable: false),
			new DmoField("arxCVV2Match", "nvarchar", 1, 0, nullable: false),
			new DmoField("arxAuthCode", "nvarchar", 6, 0, nullable: false),
			new DmoField("arxAVSAddr", "nvarchar", 1, 0, nullable: false),
			new DmoField("arxAVSZip", "nvarchar", 1, 0, nullable: false),
			new DmoField("arxIAVS", "nvarchar", 1, 0, nullable: false),
			new DmoField("arxProcAVS", "nvarchar", 1, 0, nullable: false),
			new DmoField("arxProcCVV2", "nvarchar", 1, 0, nullable: false),
			new DmoField("arxPaymentType", "nvarchar", 7, 0, nullable: false),
			new DmoField("arxCorrelationID", "nvarchar", 13, 0, nullable: false),
			new DmoField("arxBalAmt", "numeric", 15, 5, nullable: false),
			new DmoField("arxAmexID", "numeric", 15, 0, nullable: false),
			new DmoField("arxAmexPOSData", "nvarchar", 12, 0, nullable: false),
			new DmoField("arxTrxType", "nvarchar", 1, 0, nullable: false),
			new DmoField("arxOriginalPNRef", "nvarchar", 12, 0, nullable: false),
			new DmoField("arxCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("arxCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("arxARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arxSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arxTransactionAmount", "money", 12, 2, nullable: false),
			new DmoField("arxARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("arxARPaymentHeaderID", "int", 9, 0, nullable: false),
			new DmoField("arxARPaymentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("arxRespMsg", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("arxAddlMsgs", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("arxCaptured", "bit", 1, 0, nullable: false),
			new DmoField("arxPOSTransactionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arxAPPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("arxAPPaymentHeaderID", "int", 9, 0, nullable: false),
			new DmoField("arxAPPaymentLineID", "smallint", 4, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("ARXPAYFLOWPROID", unique: true),
			new DmoIndex("ARXUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
