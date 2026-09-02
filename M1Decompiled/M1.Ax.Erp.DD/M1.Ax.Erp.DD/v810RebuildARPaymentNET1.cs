using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARPaymentNET1 to support unicode", "2013-10-17")]
public class v810RebuildARPaymentNET1
{
	public v810RebuildARPaymentNET1(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentNET1", new DmoField[27]
		{
			new DmoField("arcARPaymentNET1ID", "numeric", 10, 0, nullable: false),
			new DmoField("arcTransactionType", "nvarchar", 2, 0, nullable: false),
			new DmoField("arcTransactionDate", "datetime", 14, 0, nullable: true),
			new DmoField("arcTransactionAmount", "money", 12, 2, nullable: false),
			new DmoField("arcSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arcARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arcPOSTransactionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arcOriginalTranReference", "nvarchar", 10, 0, nullable: false),
			new DmoField("arcApprovalIndicator", "nvarchar", 1, 0, nullable: false),
			new DmoField("arcApprovalCode", "nvarchar", 6, 0, nullable: false),
			new DmoField("arcResponseMessage", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("arcCVVIndicator", "nvarchar", 1, 0, nullable: false),
			new DmoField("arcAVSIndicator", "nvarchar", 1, 0, nullable: false),
			new DmoField("arcRiskIndicator", "nvarchar", 2, 0, nullable: false),
			new DmoField("arcReference", "nvarchar", 10, 0, nullable: false),
			new DmoField("arcARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("arcARPaymentHeaderID", "int", 9, 0, nullable: false),
			new DmoField("arcARPaymentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("arcAPPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("arcAPPaymentHeaderID", "int", 9, 0, nullable: false),
			new DmoField("arcAPPaymentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("arcCaptured", "bit", 1, 0, nullable: false),
			new DmoField("arcCardSuffix", "nvarchar", 4, 0, nullable: false),
			new DmoField("arcCardType", "nvarchar", 20, 0, nullable: false),
			new DmoField("arcCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("arcCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("arcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("ARCARPAYMENTNET1ID", unique: true),
			new DmoIndex("ARCUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
