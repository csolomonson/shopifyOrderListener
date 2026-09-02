using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARPaymentEPays to support unicode", "2013-10-17")]
public class v810RebuildARPaymentEPays
{
	public v810RebuildARPaymentEPays(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentEPays", new DmoField[38]
		{
			new DmoField("areARPaymentEPayID", "identity", 4, 0, nullable: false),
			new DmoField("areTender", "nvarchar", 1, 0, nullable: false),
			new DmoField("areOrigPnRef", "nvarchar", 12, 0, nullable: false),
			new DmoField("areAuthAcctSuffix", "nvarchar", 4, 0, nullable: false),
			new DmoField("areAuthExpDate", "nvarchar", 4, 0, nullable: false),
			new DmoField("areAuthTrxType", "nvarchar", 1, 0, nullable: false),
			new DmoField("areAuthAmt", "money", 12, 2, nullable: false),
			new DmoField("areAuthCode", "nvarchar", 6, 0, nullable: false),
			new DmoField("areAuthPnRef", "nvarchar", 12, 0, nullable: false),
			new DmoField("areAuthReqDate", "datetime", 14, 0, nullable: true),
			new DmoField("areAuthRcvDate", "datetime", 14, 0, nullable: true),
			new DmoField("areAuthResult", "int", 4, 0, nullable: false),
			new DmoField("areAuthResultClass", "int", 4, 0, nullable: false),
			new DmoField("areAuthRespMsg", "nvarchar", 30, 0, nullable: false),
			new DmoField("areCaptureCreditTrxType", "nvarchar", 1, 0, nullable: false),
			new DmoField("areCaptureCreditAmt", "money", 12, 2, nullable: false),
			new DmoField("areCaptureCreditPnRef", "nvarchar", 12, 0, nullable: false),
			new DmoField("areCaptureCreditReqDate", "datetime", 14, 0, nullable: true),
			new DmoField("areCaptureCreditRcvDate", "datetime", 14, 0, nullable: true),
			new DmoField("areCaptureCreditResult", "int", 4, 0, nullable: false),
			new DmoField("areCaptureCreditResultClass", "int", 4, 0, nullable: false),
			new DmoField("areCaptureCreditRespMsg", "nvarchar", 30, 0, nullable: false),
			new DmoField("areCVV2Match", "nvarchar", 1, 0, nullable: false),
			new DmoField("areAVSAddr", "nvarchar", 1, 0, nullable: false),
			new DmoField("areAVSZip", "nvarchar", 1, 0, nullable: false),
			new DmoField("areIAVS", "nvarchar", 1, 0, nullable: false),
			new DmoField("areSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("areARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("areARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("areARPaymentHeaderID", "int", 9, 0, nullable: false),
			new DmoField("areARPaymentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("arePaymentApplied", "bit", 1, 0, nullable: false),
			new DmoField("arePOSTransactionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arePOSTransactionPaymentID", "tinyint", 2, 0, nullable: false),
			new DmoField("areInactive", "bit", 1, 0, nullable: false),
			new DmoField("areCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("areCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("areUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[10]
		{
			new DmoIndex("AREARPAYMENTEPAYID", unique: true),
			new DmoIndex("AREUNIQUEID", unique: true),
			new DmoIndex("areAuthPnRef", unique: false),
			new DmoIndex("areSalesOrderID", unique: false),
			new DmoIndex("areARInvoiceID", unique: false),
			new DmoIndex("areARPaymentSessionID", unique: false),
			new DmoIndex("areARPaymentHeaderID", unique: false),
			new DmoIndex("areARPaymentLineID", unique: false),
			new DmoIndex("arePOSTransactionID", unique: false),
			new DmoIndex("arePOSTransactionPaymentID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
