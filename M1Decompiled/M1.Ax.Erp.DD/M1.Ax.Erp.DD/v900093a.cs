using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.093", "Remove fields from Warehouse Transfer tables", "2015-10-13")]
public class v900093a
{
	public v900093a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferLines", "mwlDestinationPartBinID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferLines", "mwlDestinationPartBinID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferComponents", "mwoDestinationPartBinID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferComponents", "mwoDestinationPartBinID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseRequisitionLines", "wqlSourcePartBinID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseRequisitionLines", "wqlSourcePartBinID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseRequisitionComponents", "wqoSourcePartBinID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseRequisitionComponents", "wqoSourcePartBinID", dropTriggers: true);
		}
	}
}
