using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add Last Transferred Date to GLRecurringJournals", "2008-04-08")]
public class v710000f
{
	public v710000f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "GLRecurringJournals", "glrLastTransferredDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLRecurringJournals", "glrLastTransferredDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
