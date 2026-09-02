using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLJournals to support unicode", "2013-10-17")]
public class v810RebuildGLJournals
{
	public v810RebuildGLJournals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournals", new DmoField[38]
		{
			new DmoField("glpGLJournalID", "int", 9, 0, nullable: false),
			new DmoField("glpTransactionDate", "date", 14, 0, nullable: true),
			new DmoField("glpReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("glpDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("glpGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("glpGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("glpSource", "tinyint", 1, 0, nullable: false),
			new DmoField("glpDetailSource", "tinyint", 2, 0, nullable: false),
			new DmoField("glpReversingEntry", "bit", 1, 0, nullable: false),
			new DmoField("glpOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glpLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("glpARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glpARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("glpARPaymentHeaderID", "int", 9, 0, nullable: false),
			new DmoField("glpAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glpAPPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("glpAPPaymentHeaderID", "int", 7, 0, nullable: false),
			new DmoField("glpPayrollSessionID", "int", 9, 0, nullable: false),
			new DmoField("glpReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glpShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glpTimecardID", "int", 9, 0, nullable: false),
			new DmoField("glpBankStatementID", "int", 9, 0, nullable: false),
			new DmoField("glpAssetAdjustmentID", "int", 9, 0, nullable: false),
			new DmoField("glpAssetID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glpLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("glpLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("glpTotalDebits", "money", 12, 2, nullable: false),
			new DmoField("glpTotalCredits", "money", 12, 2, nullable: false),
			new DmoField("glpPosted", "bit", 1, 0, nullable: false),
			new DmoField("glpPostedDate", "date", 14, 0, nullable: true),
			new DmoField("glpJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("glpJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("glpRMAReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glpDMRShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glpLandedCostID", "nvarchar", 10, 0, nullable: false),
			new DmoField("glpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[24]
		{
			new DmoIndex("GLPGLJOURNALID", unique: true),
			new DmoIndex("GLPUNIQUEID", unique: true),
			new DmoIndex("glpGLFiscalYearID", unique: false),
			new DmoIndex("glpGLFiscalYearPeriodID", unique: false),
			new DmoIndex("glpReversingEntry", unique: false),
			new DmoIndex("glpOrganizationID", unique: false),
			new DmoIndex("glpLocationID", unique: false),
			new DmoIndex("glpARInvoiceID", unique: false),
			new DmoIndex("glpARPaymentSessionID", unique: false),
			new DmoIndex("glpARPaymentHeaderID", unique: false),
			new DmoIndex("glpAPInvoiceID", unique: false),
			new DmoIndex("glpAPPaymentSessionID", unique: false),
			new DmoIndex("glpAPPaymentHeaderID", unique: false),
			new DmoIndex("glpPayrollSessionID", unique: false),
			new DmoIndex("glpReceiptID", unique: false),
			new DmoIndex("glpShipmentID", unique: false),
			new DmoIndex("glpTimecardID", unique: false),
			new DmoIndex("glpBankStatementID", unique: false),
			new DmoIndex("glpAssetAdjustmentID", unique: false),
			new DmoIndex("glpAssetID", unique: false),
			new DmoIndex("glpPosted", unique: false),
			new DmoIndex("glpRMAReceiptID", unique: false),
			new DmoIndex("glpDMRShipmentID", unique: false),
			new DmoIndex("glpLandedCostID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
