using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.616", "Add fields to PurchasePlannerSessions table", "2018-01-15")]
public class v92616a
{
	public v92616a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions", "ppsSalesOrderIDs"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSessions", "ppsSalesOrderIDs", "nvarchar(max)", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
