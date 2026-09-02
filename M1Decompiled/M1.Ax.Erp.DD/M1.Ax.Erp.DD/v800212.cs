using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.212", "Add Total Debits and Credits to GLRecurringJournals", "2012-01-12")]
public class v800212
{
	public v800212(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLRecurringJournalLines", "gljDebitAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLRecurringJournalLines", "gljDebitAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE GLRecurringJournalLines SET gljDebitAmount = gljAmount Where gljAmount > 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLRecurringJournalLines", "gljCreditAmount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLRecurringJournalLines", "gljCreditAmount", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE GLRecurringJournalLines SET gljCreditAmount = -gljAmount Where gljAmount < 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLRecurringJournals", "glrTotalDebits"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLRecurringJournals", "glrTotalDebits", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE GLRecurringJournals SET glrTotalDebits = gljAmount From GLRecurringJournals Inner Join (Select gljRecurringJournalID,Sum(gljAmount) As gljAmount From GLRecurringJournalLines Where gljAmount > 0 Group By gljRecurringJournalID) As GLRecurringJournalLines On glrRecurringJournalID = gljRecurringJournalID");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLRecurringJournals", "glrTotalCredits"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLRecurringJournals", "glrTotalCredits", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE GLRecurringJournals SET glrTotalCredits = -gljAmount From GLRecurringJournals Inner Join (Select gljRecurringJournalID,Sum(gljAmount) As gljAmount From GLRecurringJournalLines Where gljAmount < 0 Group By gljRecurringJournalID) As GLRecurringJournalLines On glrRecurringJournalID = gljRecurringJournalID");
		}
	}
}
