using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.022", "Add Total Debits and Credits to GLJournals", "2013-03-07")]
public class v810022
{
	public v810022(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournalLines", "gllDebitAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournalLines", "gllDebitAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE GLJournalLines SET gllDebitAmount = gllTransactionAmount Where gllTransactionAmount > 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournalLines", "gllCreditAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournalLines", "gllCreditAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE GLJournalLines SET gllCreditAmount = -gllTransactionAmount Where gllTransactionAmount < 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournals", "glpTotalDebits"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournals", "glpTotalDebits", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE GLJournals SET glpTotalDebits = gllTransactionAmount From GLJournals Inner Join (Select gllGLJournalID,Sum(gllTransactionAmount) As gllTransactionAmount From GLJournalLines Where gllTransactionAmount > 0 Group By gllGLJournalID) As GLJournalLines On glpGLJournalID = gllGLJournalID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournals", "glpTotalCredits"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournals", "glpTotalCredits", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE GLJournals SET glpTotalCredits = -gllTransactionAmount From GLJournals Inner Join (Select gllGLJournalID,Sum(gllTransactionAmount) As gllTransactionAmount From GLJournalLines Where gllTransactionAmount < 0 Group By gllGLJournalID) As GLJournalLines On glpGLJournalID = gllGLJournalID");
		}
	}
}
