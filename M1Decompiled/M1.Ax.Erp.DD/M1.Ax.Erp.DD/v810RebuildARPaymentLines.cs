using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARPaymentLines to support unicode", "2013-10-17")]
public class v810RebuildARPaymentLines
{
	public v810RebuildARPaymentLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentLines", new DmoField[46]
		{
			new DmoField("arnARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("arnARPaymentHeaderID", "int", 9, 0, nullable: false),
			new DmoField("arnARPaymentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("arnARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arnOriginalInvoiceBalance", "money", 12, 2, nullable: false),
			new DmoField("arnPaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("arnTotalDiscountAmount", "money", 12, 2, nullable: false),
			new DmoField("arnDiscountAmount", "money", 12, 2, nullable: false),
			new DmoField("arnDiscountGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("arnDiscountTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arnDiscountTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("arnSecondDiscountTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arnSecondDiscountTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("arnAdjustmentAmount", "money", 12, 2, nullable: false),
			new DmoField("arnAdjustmentGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("arnTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arnNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arnTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("arnSecondTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("arnSecondTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("arnARPaymentEPayID", "int", 4, 0, nullable: false),
			new DmoField("arnPOSSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arnPOSTransactionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arnPOSTransactionPaymentID", "tinyint", 2, 0, nullable: false),
			new DmoField("arnPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("arnRetentionPaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("arnOverpayment", "bit", 1, 0, nullable: false),
			new DmoField("arnAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arnOriginalInvBalanceForeign", "money", 12, 2, nullable: false),
			new DmoField("arnPaymentAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("arnTotalDiscountAmtForeign", "money", 12, 2, nullable: false),
			new DmoField("arnDiscountAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("arnDiscountTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("arnSecondDisTaxAmtForeign", "money", 12, 2, nullable: false),
			new DmoField("arnAdjustmentAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("arnTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("arnSecondTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("arnRetentionPayAmtForeign", "money", 12, 2, nullable: false),
			new DmoField("arnExchangeAmount", "money", 12, 2, nullable: false),
			new DmoField("arnExchangeGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("arnUnrealisedExchangeAmt", "money", 12, 2, nullable: false),
			new DmoField("arnUnrealisedExGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("arnAvalaraTaxCalculated", "bit", 1, 0, nullable: false),
			new DmoField("arnCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("arnCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("arnUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[13]
		{
			new DmoIndex("ARNARPAYMENTSESSIONID,ARNARPAYMENTHEADERID,ARNARPAYMENTLINEID", unique: true),
			new DmoIndex("ARNUNIQUEID", unique: true),
			new DmoIndex("arnARPaymentSessionID", unique: false),
			new DmoIndex("arnARPaymentHeaderID", unique: false),
			new DmoIndex("arnARPaymentLineID", unique: false),
			new DmoIndex("arnARInvoiceID", unique: false),
			new DmoIndex("arnTaxCodeID", unique: false),
			new DmoIndex("arnSecondTaxCodeID", unique: false),
			new DmoIndex("arnARPaymentEPayID", unique: false),
			new DmoIndex("arnPOSSessionID", unique: false),
			new DmoIndex("arnPOSTransactionID", unique: false),
			new DmoIndex("arnPostedToGL", unique: false),
			new DmoIndex("arnAPInvoiceID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
