using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Add fields to WarehouseTransferLines table", "2015-08-14")]
public class v900074g
{
	public v900074g(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlQuantityInTransit"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferLines", "mwlQuantityInTransit", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlQuantityInTransit"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update WarehouseTransferLines Set mwlQuantityInTransit = CASE WHEN mwlShipQuantity-mwlReceivedQuantity <> 0 THEN 0 ELSE mwlShipQuantity-mwlReceivedQuantity END");
		}
	}
}
