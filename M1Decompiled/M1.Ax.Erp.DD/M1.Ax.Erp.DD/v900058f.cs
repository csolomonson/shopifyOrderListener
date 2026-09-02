using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.058", "Add fields to WarehouseReceiptComponents table", "2015-07-09")]
public class v900058f
{
	public v900058f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseReceiptComponents", "wroSourceTableName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceiptComponents", "wroSourceTableName", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseReceiptComponents", "wroSourceTableUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceiptComponents", "wroSourceTableUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "update WarehouseReceiptComponents set wroSourceTableName = 'WarehouseTransferComponents', wroSourceTableUniqueID = mwoUniqueID from WarehouseReceiptComponents inner join WarehouseTransferComponents on wroWarehouseTransferID=mwoWarehouseTransferID and wroWarehouseTransferLineID=mwoWarehouseTransferLineID and wroWarehouseTransComponentID=mwoWarehouseTransComponentID");
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseReceiptComponents", "wroParentQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceiptComponents", "wroParentQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
