using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.500", "RenameUI UX key columns", "2022-10-25")]
public class v95500d
{
	public v95500d(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "TopActivitiesLog", "rxlRecentActivityLogID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TopActivitiesLog", "rxlRecentActivityLogID", "rxlTopActivityID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RecentActivitiesLog", "rtlRecentTransactionLogID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RecentActivitiesLog", "rtlRecentTransactionLogID", "rtlRecentActivityID", dropTriggers: true);
		}
	}
}
