using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert POSTransactionPayments to support unicode", "2013-10-17")]
public class v810RebuildPOSTransactionPayments
{
	public v810RebuildPOSTransactionPayments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "POSTransactionPayments", new DmoField[17]
		{
			new DmoField("psyPOSTransactionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("psyPOSTransactionPaymentID", "tinyint", 2, 0, nullable: false),
			new DmoField("psyPaymentMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("psyCashTendered", "money", 12, 2, nullable: false),
			new DmoField("psyChangeGiven", "money", 12, 2, nullable: false),
			new DmoField("psyReference", "nvarchar", 50, 0, nullable: false),
			new DmoField("psyAmountPaid", "numeric", 12, 2, nullable: false),
			new DmoField("psyCustomerPO", "nvarchar", 40, 0, nullable: false),
			new DmoField("psyCCAuthNumber", "nvarchar", 6, 0, nullable: false),
			new DmoField("psyARPaymentEPayID", "int", 4, 0, nullable: false),
			new DmoField("psyCheckNo", "nvarchar", 10, 0, nullable: false),
			new DmoField("psyStoreCreditID", "nvarchar", 10, 0, nullable: false),
			new DmoField("psyVoided", "bit", 1, 0, nullable: false),
			new DmoField("psyPosted", "bit", 1, 0, nullable: false),
			new DmoField("psyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("psyCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("psyUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("PSYPOSTRANSACTIONID,PSYPOSTRANSACTIONPAYMENTID", unique: true),
			new DmoIndex("PSYUNIQUEID", unique: true),
			new DmoIndex("psyPOSTransactionID", unique: false),
			new DmoIndex("psyPOSTransactionPaymentID", unique: false),
			new DmoIndex("psyPaymentMethodID", unique: false),
			new DmoIndex("psyARPaymentEPayID", unique: false),
			new DmoIndex("psyVoided", unique: false),
			new DmoIndex("psyPosted", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
