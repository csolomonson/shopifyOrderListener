using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLJournalLines to support unicode", "2013-10-17")]
public class v810RebuildGLJournalLines
{
	public v810RebuildGLJournalLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournalLines", new DmoField[26]
		{
			new DmoField("gllGLJournalID", "int", 9, 0, nullable: false),
			new DmoField("gllGLJournalLineID", "int", 5, 0, nullable: false),
			new DmoField("gllTransactionAmount", "money", 12, 2, nullable: false),
			new DmoField("gllDebitAmount", "money", 12, 2, nullable: false),
			new DmoField("gllCreditAmount", "money", 12, 2, nullable: false),
			new DmoField("gllTransactionDate", "date", 14, 0, nullable: true),
			new DmoField("gllGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("gllReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("gllDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("gllTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("gllGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("gllGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("gllTaxableAmount", "money", 12, 2, nullable: false),
			new DmoField("gllPartTransactionID", "int", 9, 0, nullable: false),
			new DmoField("gllJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("gllJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("gllJobMaterialID", "int", 5, 0, nullable: false),
			new DmoField("gllJobOperationID", "int", 5, 0, nullable: false),
			new DmoField("gllOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("gllLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("gllARPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("gllARPaymentHeaderID", "int", 9, 0, nullable: false),
			new DmoField("gllPosted", "bit", 1, 0, nullable: false),
			new DmoField("gllCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("gllCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("gllUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[15]
		{
			new DmoIndex("GLLGLJOURNALID,GLLGLJOURNALLINEID", unique: true),
			new DmoIndex("GLLUNIQUEID", unique: true),
			new DmoIndex("gllGLJournalID", unique: false),
			new DmoIndex("gllGLJournalLineID", unique: false),
			new DmoIndex("gllGLAccountID", unique: false),
			new DmoIndex("gllTaxCodeID", unique: false),
			new DmoIndex("gllGLFiscalYearID", unique: false),
			new DmoIndex("gllGLFiscalYearPeriodID", unique: false),
			new DmoIndex("gllPartTransactionID", unique: false),
			new DmoIndex("gllJobID", unique: false),
			new DmoIndex("gllOrganizationID", unique: false),
			new DmoIndex("gllLocationID", unique: false),
			new DmoIndex("gllARPaymentSessionID", unique: false),
			new DmoIndex("gllARPaymentHeaderID", unique: false),
			new DmoIndex("gllPosted", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
