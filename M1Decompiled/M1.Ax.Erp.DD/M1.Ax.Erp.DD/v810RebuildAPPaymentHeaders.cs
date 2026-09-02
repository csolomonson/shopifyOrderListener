using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APPaymentHeaders to support unicode", "2013-10-17")]
public class v810RebuildAPPaymentHeaders
{
	public v810RebuildAPPaymentHeaders(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APPaymentHeaders Set aptForm1099Box = 0 Where aptForm1099Box = -1");
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APPaymentHeaders", new DmoField[47]
		{
			new DmoField("aptAPPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("aptAPPaymentHeaderID", "int", 7, 0, nullable: false),
			new DmoField("aptPaymentType", "tinyint", 1, 0, nullable: false),
			new DmoField("aptManualPayment", "bit", 1, 0, nullable: false),
			new DmoField("aptVoidedPayment", "bit", 1, 0, nullable: false),
			new DmoField("aptSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("aptAPInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("aptAPInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("aptPaymentNumber", "int", 6, 0, nullable: false),
			new DmoField("aptEFTNumber", "int", 6, 0, nullable: false),
			new DmoField("aptPaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("aptPaymentMemo", "nvarchar", 50, 0, nullable: false),
			new DmoField("aptPaymentDate", "date", 14, 0, nullable: true),
			new DmoField("aptLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("aptLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("aptCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("aptGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("aptGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("aptCompleted", "bit", 1, 0, nullable: false),
			new DmoField("aptPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("aptBankInitials", "nvarchar", 3, 0, nullable: false),
			new DmoField("aptBSBNumber", "nvarchar", 10, 0, nullable: false),
			new DmoField("aptBankAccountName", "nvarchar", 50, 0, nullable: false),
			new DmoField("aptBankAccountNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("aptEFTDescription", "nvarchar", 20, 0, nullable: false),
			new DmoField("aptSuppressVoid", "bit", 1, 0, nullable: false),
			new DmoField("aptVoidAPPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("aptVoidAPPaymentHeaderID", "int", 7, 0, nullable: false),
			new DmoField("aptCreditAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("aptCreatedCreditAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("aptBankAccountType", "nvarchar", 2, 0, nullable: false),
			new DmoField("aptShowAllInvoices", "bit", 1, 0, nullable: false),
			new DmoField("aptOverpayment", "bit", 1, 0, nullable: false),
			new DmoField("aptOpenPaymentLoad", "bit", 1, 0, nullable: false),
			new DmoField("aptRecurringPaymentID", "int", 6, 0, nullable: false),
			new DmoField("aptPaymentAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("aptExchangeAmount", "money", 12, 2, nullable: false),
			new DmoField("aptExchangeGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("aptForm1099Box", "tinyint", 2, 0, nullable: false),
			new DmoField("aptEFTCode", "nvarchar", 12, 0, nullable: false),
			new DmoField("aptEFTParticulars", "nvarchar", 12, 0, nullable: false),
			new DmoField("aptTaxReportable", "bit", 1, 0, nullable: false),
			new DmoField("aptCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("aptCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("aptUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("aptIBAN", "nvarchar", 50, 0, nullable: false),
			new DmoField("aptBIC", "nvarchar", 50, 0, nullable: false)
		}, new DmoIndex[16]
		{
			new DmoIndex("APTAPPAYMENTSESSIONID,APTAPPAYMENTHEADERID", unique: true),
			new DmoIndex("APTUNIQUEID", unique: true),
			new DmoIndex("aptAPPaymentSessionID", unique: false),
			new DmoIndex("aptAPPaymentHeaderID", unique: false),
			new DmoIndex("aptManualPayment", unique: false),
			new DmoIndex("aptVoidedPayment", unique: false),
			new DmoIndex("aptSupplierOrganizationID", unique: false),
			new DmoIndex("aptAPInvoiceLocationID", unique: false),
			new DmoIndex("aptAPInvoiceContactID", unique: false),
			new DmoIndex("aptPaymentNumber", unique: false),
			new DmoIndex("aptEFTNumber", unique: false),
			new DmoIndex("aptGLFiscalYearID", unique: false),
			new DmoIndex("aptGLFiscalYearPeriodID", unique: false),
			new DmoIndex("aptCompleted", unique: false),
			new DmoIndex("aptPostedToGL", unique: false),
			new DmoIndex("aptCreditAPInvoiceID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
