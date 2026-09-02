using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.058", "Add fields to PartWarehouseLocations table", "2016-05-18")]
public class v91058e
{
	public v91058e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartWarehouseLocations", "imLLastRunDatePurchasePlanner"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartWarehouseLocations", "imLLastRunDatePurchasePlanner", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
