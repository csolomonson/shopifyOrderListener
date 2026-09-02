using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARPaymentHeaders to support unicode", "2013-10-17")]
public class v810RebuildARPaymentHeaders
{
	public v810RebuildARPaymentHeaders(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentHeaders", new DmoField[45]
		{
			new DmoField("artARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("artARPaymentHeaderID", "int", 9, 0, nullable: false),
			new DmoField("artReceiptType", "tinyint", 1, 0, nullable: false),
			new DmoField("artPaymentMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("artCustomerOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("artARInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("artARInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("artCustomerPaymentNumber", "nvarchar", 10, 0, nullable: false),
			new DmoField("artReceiptAmount", "money", 12, 2, nullable: false),
			new DmoField("artDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("artLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("artLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("artReceiptDate", "date", 14, 0, nullable: true),
			new DmoField("artOpenPaymentLoad", "bit", 1, 0, nullable: false),
			new DmoField("artGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("artTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("artNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("artTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("artSecondTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("artSecondTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("artPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("artGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("artGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("artCreditARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("artCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("artARGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("artCreatedCreditARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("artBankInitials", "nvarchar", 3, 0, nullable: false),
			new DmoField("artBSBNumber", "nvarchar", 10, 0, nullable: false),
			new DmoField("artBankAccountName", "nvarchar", 50, 0, nullable: false),
			new DmoField("artBankAccountNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("artVoidedPayment", "bit", 1, 0, nullable: false),
			new DmoField("artVoidARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("artVoidARPaymentHeaderId", "int", 7, 0, nullable: false),
			new DmoField("artShowAllInvoices", "bit", 1, 0, nullable: false),
			new DmoField("artReceiptAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("artTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("artSecondTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("artExchangeAmount", "money", 12, 2, nullable: false),
			new DmoField("artExchangeGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("artAvalaraTaxCalculated", "bit", 1, 0, nullable: false),
			new DmoField("artCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("artCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("artUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("artNet1PaymentProcessed", "bit", 1, 0, nullable: false)
		}, new DmoIndex[16]
		{
			new DmoIndex("ARTARPAYMENTSESSIONID,ARTARPAYMENTHEADERID", unique: true),
			new DmoIndex("ARTUNIQUEID", unique: true),
			new DmoIndex("artARPaymentSessionID", unique: false),
			new DmoIndex("artARPaymentHeaderID", unique: false),
			new DmoIndex("artReceiptType", unique: false),
			new DmoIndex("artPaymentMethod", unique: false),
			new DmoIndex("artCustomerOrganizationID", unique: false),
			new DmoIndex("artARInvoiceLocationID", unique: false),
			new DmoIndex("artARInvoiceContactID", unique: false),
			new DmoIndex("artTaxCodeID", unique: false),
			new DmoIndex("artSecondTaxCodeID", unique: false),
			new DmoIndex("artPostedToGL", unique: false),
			new DmoIndex("artGLFiscalYearID", unique: false),
			new DmoIndex("artGLFiscalYearPeriodID", unique: false),
			new DmoIndex("artCreditARInvoiceID", unique: false),
			new DmoIndex("artVoidedPayment", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
