using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.058", "Add fields to WarehouseTransferLines table", "2015-07-09")]
public class v900058e
{
	public v900058e(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlWarehouseID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferLines", "mwlWarehouseID", "mwlSourceWarehouseID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlPartBinID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferLines", "mwlPartBinID", "mwlSourcePartBinID", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlWROpenQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferLines", "mwlWROpenQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlDestinationPartBinID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferLines", "mwlDestinationPartBinID", "nvarchar", 15, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlWRRequestedQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferLines", "mwlWRRequestedQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlDestinationWarehouseID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferLines", "mwlDestinationWarehouseID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
