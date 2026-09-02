using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.826", "Add fields to PurchasePlannerSessions table", "2020-03-26")]
public class v92826c
{
	public v92826c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerSessions", "ppsCutoffDatePOSupply"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerSessions", "ppsCutoffDatePOSupply", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
