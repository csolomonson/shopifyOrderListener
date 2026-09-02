using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLFiscalYearPeriods to support unicode", "2013-10-17")]
public class v810RebuildGLFiscalYearPeriods
{
	public v810RebuildGLFiscalYearPeriods(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLFiscalYearPeriods", new DmoField[10]
		{
			new DmoField("glfGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("glfGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("glfStartDate", "date", 14, 0, nullable: true),
			new DmoField("glfEndDate", "date", 14, 0, nullable: true),
			new DmoField("glfARClosed", "bit", 1, 0, nullable: false),
			new DmoField("glfAPClosed", "bit", 1, 0, nullable: false),
			new DmoField("glfGLClosed", "bit", 1, 0, nullable: false),
			new DmoField("glfCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glfCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glfUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("GLFGLFISCALYEARID,GLFGLFISCALYEARPERIODID", unique: true),
			new DmoIndex("GLFUNIQUEID", unique: true),
			new DmoIndex("glfGLFiscalYearID", unique: false),
			new DmoIndex("glfGLFiscalYearPeriodID", unique: false),
			new DmoIndex("glfARClosed", unique: false),
			new DmoIndex("glfAPClosed", unique: false),
			new DmoIndex("glfGLClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
