using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.664", "Initialise fields in PurchasePlannerSessions table", "2018-03-15")]
public class v92664a
{
	public v92664a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions", "ppsSalesOrderIDs"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PurchasePlannerSessions set ppsSalesOrderIDs = '' where ppsSalesOrderIDs is null");
		}
	}
}
