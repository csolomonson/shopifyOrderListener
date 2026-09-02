using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLFiscalYearBudgetHeaders to support unicode", "2013-10-17")]
public class v810RebuildGLFiscalYearBudgetHeaders
{
	public v810RebuildGLFiscalYearBudgetHeaders(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLFiscalYearBudgetHeaders", new DmoField[7]
		{
			new DmoField("glkGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("glkBudgetHeaderID", "smallint", 4, 0, nullable: false),
			new DmoField("glkGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("glkAnnualAmount", "money", 12, 2, nullable: false),
			new DmoField("glkCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glkCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("GLKGLFISCALYEARID,GLKBUDGETHEADERID", unique: true),
			new DmoIndex("GLKUNIQUEID", unique: true),
			new DmoIndex("glkGLFiscalYearID", unique: false),
			new DmoIndex("glkBudgetHeaderID", unique: false),
			new DmoIndex("glkGLAccountID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
