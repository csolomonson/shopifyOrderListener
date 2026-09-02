using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.216", "Correct Total Credits value in GLRecurringJournals table", "2012-01-22")]
public class v800216
{
	public v800216(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLRecurringJournals", "glrTotalCredits"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE GLRecurringJournals SET glrTotalCredits = -gljAmount From GLRecurringJournals Inner Join (Select gljRecurringJournalID,Sum(gljAmount) As gljAmount From GLRecurringJournalLines Where gljAmount < 0 Group By gljRecurringJournalID) As GLRecurringJournalLines On glrRecurringJournalID = gljRecurringJournalID");
		}
	}
}
