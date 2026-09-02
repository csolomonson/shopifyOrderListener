using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLFiscalYearPeriodMovements to support unicode", "2013-10-17")]
public class v810RebuildGLFiscalYearPeriodMovements
{
	public v810RebuildGLFiscalYearPeriodMovements(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLFiscalYearPeriodMovements", new DmoField[8]
		{
			new DmoField("gliGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("gliGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("gliGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("gliTotalDebits", "money", 12, 2, nullable: false),
			new DmoField("gliTotalCredits", "money", 12, 2, nullable: false),
			new DmoField("gliCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("gliCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("gliUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("GLIGLACCOUNTID,GLIGLFISCALYEARID,GLIGLFISCALYEARPERIODID", unique: true),
			new DmoIndex("GLIUNIQUEID", unique: true),
			new DmoIndex("gliGLFiscalYearID", unique: false),
			new DmoIndex("gliGLFiscalYearPeriodID", unique: false),
			new DmoIndex("gliGLAccountID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
