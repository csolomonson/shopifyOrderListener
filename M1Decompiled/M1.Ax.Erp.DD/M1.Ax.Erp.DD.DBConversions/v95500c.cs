using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.500", "Rename UI UX tables", "2022-10-25")]
public class v95500c
{
	public v95500c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "RecentActivityLog"))
		{
			parms.Dmo.RenameTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RecentActivityLog", "TopActivitiesLog");
		}
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "RecentTransactionsLog"))
		{
			parms.Dmo.RenameTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RecentTransactionsLog", "RecentActivitiesLog");
		}
	}
}
