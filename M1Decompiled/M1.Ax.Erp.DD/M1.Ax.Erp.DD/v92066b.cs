using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.066", "Remove field from PurchasePlannerSessions", "2017-01-06")]
public class v92066b
{
	public v92066b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions", "ppsIncludeBlankWarehouse"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSessions", "ppsIncludeBlankWarehouse", dropTriggers: true);
		}
	}
}
