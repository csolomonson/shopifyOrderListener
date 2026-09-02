using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.075", "Add fields to PurchasePlannerSessions table", "2016-06-10")]
public class v91075b
{
	public v91075b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions", "ppsSessionSubtotalBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSessions", "ppsSessionSubtotalBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
