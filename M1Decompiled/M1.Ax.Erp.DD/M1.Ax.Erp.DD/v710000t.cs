using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add fields to GL Journal Lines table", "2008-06-02")]
public class v710000t
{
	public v710000t(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournalLines", "gllOrganizationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournalLines", "gllOrganizationID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournalLines", "gllLocationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournalLines", "gllLocationID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournalLines", "gllARPaymentSessionID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournalLines", "gllARPaymentSessionID", "numeric", 9, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLJournalLines", "gllARPaymentHeaderID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournalLines", "gllARPaymentHeaderID", "numeric", 9, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
