using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.058", "Add fields to WarehouseReceiptLines table", "2015-07-09")]
public class v900058a
{
	public v900058a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseReceiptLines", "wrlSourceTableUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceiptLines", "wrlSourceTableUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseReceiptLines", "wrlSourceTableName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceiptLines", "wrlSourceTableName", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "update WarehouseReceiptLines set wrlSourceTableName = 'WarehouseTransferLines', wrlSourceTableUniqueID = mwlUniqueID from WarehouseReceiptLines inner join WarehouseTransferLines on wrlWarehouseTransferID=mwlWarehouseTransferID and wrlWarehouseTransferLineID=mwlWarehouseTransferLineID");
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseReceiptLines", "wrlWTShippedQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceiptLines", "wrlWTShippedQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseReceiptLines", "wrlWTOpenQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceiptLines", "wrlWTOpenQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
