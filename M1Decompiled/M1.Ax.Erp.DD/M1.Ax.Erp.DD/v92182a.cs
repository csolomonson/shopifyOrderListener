using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.182", "Add field to PurchasePlannerSessions table", "2017-03-07")]
public class v92182a
{
	public v92182a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions", "ppsFirmOnly"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSessions", "ppsFirmOnly", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
