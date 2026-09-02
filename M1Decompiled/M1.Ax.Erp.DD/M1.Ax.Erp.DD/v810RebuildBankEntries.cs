using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert BankEntries to support unicode", "2013-10-17")]
public class v810RebuildBankEntries
{
	public v810RebuildBankEntries(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "BankEntries", new DmoField[44]
		{
			new DmoField("gleBankEntryID", "int", 9, 0, nullable: false),
			new DmoField("gleCleared", "bit", 1, 0, nullable: false),
			new DmoField("gleEntryType", "tinyint", 1, 0, nullable: false),
			new DmoField("gleSource", "tinyint", 1, 0, nullable: false),
			new DmoField("gleARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("gleARPaymentHeaderID", "int", 7, 0, nullable: false),
			new DmoField("gleAPPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("gleAPPaymentHeaderID", "int", 7, 0, nullable: false),
			new DmoField("glePayrollSessionID", "int", 9, 0, nullable: false),
			new DmoField("glePayrollHeaderID", "int", 7, 0, nullable: false),
			new DmoField("glePaymentDate", "date", 14, 0, nullable: true),
			new DmoField("gleOriginalAmount", "money", 12, 2, nullable: false),
			new DmoField("glePaymentAmount", "money", 12, 2, nullable: false),
			new DmoField("gleDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("gleTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("gleTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("gleNonTaxReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("gleVarianceAmount", "money", 12, 2, nullable: false),
			new DmoField("gleGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("gleDoNotUpdateGL", "bit", 1, 0, nullable: false),
			new DmoField("glePostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("gleBankStatementID", "int", 9, 0, nullable: false),
			new DmoField("gleTransactionDate", "date", 14, 0, nullable: true),
			new DmoField("gleCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("gleGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("gleGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("glePayType", "tinyint", 1, 0, nullable: false),
			new DmoField("glePaymentNumber", "int", 6, 0, nullable: false),
			new DmoField("gleEFTReferenceNumber", "nvarchar", 16, 0, nullable: false),
			new DmoField("gleUnpresentedPayment", "bit", 1, 0, nullable: false),
			new DmoField("gleGLJournalID", "int", 9, 0, nullable: false),
			new DmoField("gleGLJournalLineID", "int", 5, 0, nullable: false),
			new DmoField("gleOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glePresentedDate", "date", 14, 0, nullable: true),
			new DmoField("gleOriginalAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("glePaymentAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("gleTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("gleVarianceAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("gleCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("gleCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("gleExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("gleCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("gleCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("gleUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[16]
		{
			new DmoIndex("GLEBANKENTRYID", unique: true),
			new DmoIndex("GLEUNIQUEID", unique: true),
			new DmoIndex("gleCleared", unique: false),
			new DmoIndex("gleSource", unique: false),
			new DmoIndex("gleARPaymentSessionID", unique: false),
			new DmoIndex("gleARPaymentHeaderID", unique: false),
			new DmoIndex("gleAPPaymentSessionID", unique: false),
			new DmoIndex("gleAPPaymentHeaderID", unique: false),
			new DmoIndex("glePayrollSessionID", unique: false),
			new DmoIndex("glePayrollHeaderID", unique: false),
			new DmoIndex("gleTaxCodeID", unique: false),
			new DmoIndex("glePostedToGL", unique: false),
			new DmoIndex("gleCashGLAccountID", unique: false),
			new DmoIndex("glePaymentNumber", unique: false),
			new DmoIndex("gleGLJournalID", unique: false),
			new DmoIndex("gleGLJournalLineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
