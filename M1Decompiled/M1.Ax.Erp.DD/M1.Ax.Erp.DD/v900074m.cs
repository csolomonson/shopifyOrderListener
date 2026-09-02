using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Add fields to WarehouseTransferComponents table", "2015-08-14")]
public class v900074m
{
	public v900074m(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferComponents", "mwoQuantityInTransit"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferComponents", "mwoQuantityInTransit", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferComponents", "mwoQuantityInTransit"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update WarehouseTransferComponents Set mwoQuantityInTransit = CASE WHEN mwoShipQuantity-mwoReceivedQuantity <= 0 THEN 0 ELSE mwoShipQuantity-mwoReceivedQuantity END");
		}
	}
}
