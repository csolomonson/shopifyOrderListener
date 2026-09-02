using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.100", "Update fiscal year and period for journal lines with values 0 caused by SFE", "2021-10-26")]
public class v95100i
{
	public v95100i(DBConversionParms parms)
	{
		_ = parms.Database;
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE GLJournalLines SET gllGLFiscalYearID = g.glpGLFiscalYearID, gllGLFiscalYearPeriodID = g.glpGLFiscalYearPeriodID FROM GLJournals AS g INNER JOIN GLJournalLines as gl ON g.glpGLJournalID = gl.gllGLJournalID WHERE g.glpSource = 5 AND g.glpDetailSource = 13 AND (gl.gllGLFiscalYearID = 0 OR gl.gllGLFiscalYearPeriodID = 0) AND gl.gllTransactionType = 3");
	}
}
