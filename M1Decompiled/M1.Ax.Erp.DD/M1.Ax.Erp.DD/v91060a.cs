using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.060", "Add fields to PurchasePlannerOrderDetails table", "2016-05-24")]
public class v91060a
{
	public v91060a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoPartBinID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoPartBinID", "nvarchar", 15, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoPartWarehouseLocationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoPartWarehouseLocationID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
