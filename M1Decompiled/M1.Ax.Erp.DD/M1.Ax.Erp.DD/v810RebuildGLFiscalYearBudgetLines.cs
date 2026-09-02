using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLFiscalYearBudgetLines to support unicode", "2013-10-17")]
public class v810RebuildGLFiscalYearBudgetLines
{
	public v810RebuildGLFiscalYearBudgetLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLFiscalYearBudgetLines", new DmoField[7]
		{
			new DmoField("glgGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("glgBudgetHeaderID", "smallint", 4, 0, nullable: false),
			new DmoField("glgBudgetLineID", "smallint", 4, 0, nullable: false),
			new DmoField("glgAnnualAmount", "money", 12, 2, nullable: false),
			new DmoField("glgCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glgCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glgUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("GLGGLFISCALYEARID,GLGBUDGETHEADERID,GLGBUDGETLINEID", unique: true),
			new DmoIndex("GLGUNIQUEID", unique: true),
			new DmoIndex("glgGLFiscalYearID", unique: false),
			new DmoIndex("glgBudgetHeaderID", unique: false),
			new DmoIndex("glgBudgetLineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
