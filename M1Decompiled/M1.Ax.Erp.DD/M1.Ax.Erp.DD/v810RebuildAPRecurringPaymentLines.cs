using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APRecurringPaymentLines to support unicode", "2013-10-17")]
public class v810RebuildAPRecurringPaymentLines
{
	public v810RebuildAPRecurringPaymentLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APRecurringPaymentLines", new DmoField[22]
		{
			new DmoField("apqRecurringPaymentID", "int", 6, 0, nullable: false),
			new DmoField("apqRecurringPaymentLineID", "smallint", 3, 0, nullable: false),
			new DmoField("apqPaymentType", "tinyint", 1, 0, nullable: false),
			new DmoField("apqBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apqPaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("apqPaymentAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apqGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("apqDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("apqTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apqNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apqTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("apqTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apqSecondTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apqSecondTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("apqSecondTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apqCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apqCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("apqExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("apqInactive", "bit", 1, 0, nullable: false),
			new DmoField("apqCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("apqCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("apqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("APQRECURRINGPAYMENTID,APQRECURRINGPAYMENTLINEID", unique: true),
			new DmoIndex("APQUNIQUEID", unique: true),
			new DmoIndex("apqTaxCodeID", unique: false),
			new DmoIndex("apqSecondTaxCodeID", unique: false),
			new DmoIndex("apqInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
