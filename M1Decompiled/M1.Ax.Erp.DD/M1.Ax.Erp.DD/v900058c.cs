using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.058", "Add fields to WarehouseTransferComponents table", "2015-07-09")]
public class v900058c
{
	public v900058c(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferComponents", "mwoPartWarehouseLocationID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferComponents", "mwoPartWarehouseLocationID", "mwoSourceWarehouseID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferComponents", "mwoPartBinID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferComponents", "mwoPartBinID", "mwoSourcePartBinID", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferComponents", "mwoDestinationPartBinID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferComponents", "mwoDestinationPartBinID", "nvarchar", 15, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferComponents", "mwoDestinationWarehouseID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferComponents", "mwoDestinationWarehouseID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferComponents", "mwoParentQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferComponents", "mwoParentQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
