using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APPaymentLines to support unicode", "2013-10-17")]
public class v810RebuildAPPaymentLines
{
	public v810RebuildAPPaymentLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentLines", new DmoField[48]
		{
			new DmoField("apnAPPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("apnAPPaymentHeaderID", "int", 7, 0, nullable: false),
			new DmoField("apnAPPaymentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("apnAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("apnOriginalInvoiceBalance", "money", 12, 2, nullable: false),
			new DmoField("apnPaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("apnExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("apnDiscountAmount", "money", 12, 2, nullable: false),
			new DmoField("apnDiscountGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("apnDiscountTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apnDiscountTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("apnSecondDiscountTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apnSecondDiscountTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("apnTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apnNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apnTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("apnSecondTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apnSecondTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("apnTotalDiscountAmount", "money", 12, 2, nullable: false),
			new DmoField("apnCompleted", "bit", 1, 0, nullable: false),
			new DmoField("apnPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("apnAdjustmentAmount", "money", 12, 2, nullable: false),
			new DmoField("apnAdjustmentGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("apnBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apnDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("apnOverpayment", "bit", 1, 0, nullable: false),
			new DmoField("apnARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("apnOriginalInvBalanceForeign", "money", 12, 2, nullable: false),
			new DmoField("apnPaymentAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apnDiscountAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apnDiscountTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apnSecondDisTaxAmtForeign", "money", 12, 2, nullable: false),
			new DmoField("apnTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apnSecondTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apnTotalDiscountAmtForeign", "money", 12, 2, nullable: false),
			new DmoField("apnAdjustmentAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apnExchangeAmount", "money", 12, 2, nullable: false),
			new DmoField("apnExchangeGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("apnCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apnCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("apnExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("apnRetentionPaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("apnRetentionPayAmtForeign", "money", 12, 2, nullable: false),
			new DmoField("apnUnrealisedExchangeAmt", "money", 12, 2, nullable: false),
			new DmoField("apnUnrealisedExGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("apnCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("apnCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("apnUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[13]
		{
			new DmoIndex("APNAPPAYMENTSESSIONID,APNAPPAYMENTHEADERID,APNAPPAYMENTLINEID", unique: true),
			new DmoIndex("APNUNIQUEID", unique: true),
			new DmoIndex("apnAPPaymentSessionID", unique: false),
			new DmoIndex("apnAPPaymentHeaderID", unique: false),
			new DmoIndex("apnAPPaymentLineID", unique: false),
			new DmoIndex("apnAPInvoiceID", unique: false),
			new DmoIndex("apnDiscountTaxCodeID", unique: false),
			new DmoIndex("apnSecondDiscountTaxCodeID", unique: false),
			new DmoIndex("apnTaxCodeID", unique: false),
			new DmoIndex("apnSecondTaxCodeID", unique: false),
			new DmoIndex("apnCompleted", unique: false),
			new DmoIndex("apnPostedToGL", unique: false),
			new DmoIndex("apnARInvoiceID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
