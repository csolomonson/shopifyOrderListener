using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLFiscalYearOpeningBalances to support unicode", "2013-10-17")]
public class v810RebuildGLFiscalYearOpeningBalances
{
	public v810RebuildGLFiscalYearOpeningBalances(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLFiscalYearOpeningBalances", new DmoField[6]
		{
			new DmoField("glyGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("glyGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("glyYearOpeningBalance", "money", 12, 2, nullable: false),
			new DmoField("glyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glyCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glyUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("GLYGLACCOUNTID,GLYGLFISCALYEARID", unique: true),
			new DmoIndex("GLYUNIQUEID", unique: true),
			new DmoIndex("glyGLFiscalYearID", unique: false),
			new DmoIndex("glyGLAccountID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
