using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.066", "Add fields to PurchasePlannerSessions table", "2016-06-01")]
public class v91066b
{
	public v91066b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions", "ppsIncludeBlankWarehouse"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSessions", "ppsIncludeBlankWarehouse", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
