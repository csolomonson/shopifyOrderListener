using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLFiscalYearBudgetAmounts to support unicode", "2013-10-17")]
public class v810RebuildGLFiscalYearBudgetAmounts
{
	public v810RebuildGLFiscalYearBudgetAmounts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLFiscalYearBudgetAmounts", new DmoField[9]
		{
			new DmoField("glbGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("glbBudgetHeaderID", "smallint", 4, 0, nullable: false),
			new DmoField("glbBudgetLineID", "smallint", 4, 0, nullable: false),
			new DmoField("glbGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("glbGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("glbBudgetAmount", "money", 12, 2, nullable: false),
			new DmoField("glbCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glbCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glbUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("GLBGLFISCALYEARID,GLBBUDGETHEADERID,GLBBUDGETLINEID,GLBGLFISCALYEARPERIODID", unique: true),
			new DmoIndex("GLBUNIQUEID", unique: true),
			new DmoIndex("glbGLFiscalYearID", unique: false),
			new DmoIndex("glbBudgetHeaderID", unique: false),
			new DmoIndex("glbBudgetLineID", unique: false),
			new DmoIndex("glbGLAccountID", unique: false),
			new DmoIndex("glbGLFiscalYearPeriodID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
