using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert BankStatements to support unicode", "2013-10-17")]
public class v810RebuildBankStatements
{
	public v810RebuildBankStatements(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "BankStatements", new DmoField[22]
		{
			new DmoField("glsBankStatementID", "int", 9, 0, nullable: false),
			new DmoField("glsBankStatementReference", "int", 9, 0, nullable: false),
			new DmoField("glsCashGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("glsOpeningDate", "date", 14, 0, nullable: true),
			new DmoField("glsOpeningBalance", "money", 12, 2, nullable: false),
			new DmoField("glsEndingDate", "date", 14, 0, nullable: true),
			new DmoField("glsEndingBalance", "money", 12, 2, nullable: false),
			new DmoField("glsPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("glsPostedDate", "date", 14, 0, nullable: true),
			new DmoField("glsBankAccountID", "nvarchar", 5, 0, nullable: false),
			new DmoField("glsShowTransactions", "bit", 1, 0, nullable: false),
			new DmoField("glsGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("glsCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("glsCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("glsExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("glsOpeningBalanceForeign", "money", 12, 2, nullable: false),
			new DmoField("glsEndingBalanceForeign", "money", 12, 2, nullable: false),
			new DmoField("glsExchangeAmount", "money", 12, 2, nullable: false),
			new DmoField("glsExchangeGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("glsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("GLSBANKSTATEMENTID", unique: true),
			new DmoIndex("GLSUNIQUEID", unique: true),
			new DmoIndex("glsPostedToGL", unique: false),
			new DmoIndex("glsGLFiscalYearID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
