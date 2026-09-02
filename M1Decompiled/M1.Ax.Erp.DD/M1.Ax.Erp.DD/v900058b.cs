using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.058", "Add fields to WarehouseRequisitionComponents table", "2015-07-09")]
public class v900058b
{
	public v900058b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseRequisitionComponents", "wqoPartWarehouseLocationID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseRequisitionComponents", "wqoPartWarehouseLocationID", "wqoSourceWarehouseID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseRequisitionComponents", "wqoPartBinID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseRequisitionComponents", "wqoPartBinID", "wqoSourcePartBinID", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseRequisitionComponents", "wqoParentQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseRequisitionComponents", "wqoParentQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
