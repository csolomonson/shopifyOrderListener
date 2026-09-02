using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLFiscalYears to support unicode", "2013-10-17")]
public class v810RebuildGLFiscalYears
{
	public v810RebuildGLFiscalYears(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLFiscalYears", new DmoField[6]
		{
			new DmoField("glzGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("glzStartDate", "date", 14, 0, nullable: true),
			new DmoField("glzEndDate", "date", 14, 0, nullable: true),
			new DmoField("glzCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glzCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glzUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("GLZGLFISCALYEARID", unique: true),
			new DmoIndex("GLZUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
