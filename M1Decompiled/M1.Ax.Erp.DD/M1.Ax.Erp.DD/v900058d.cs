using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.058", "Add fields to WarehouseRequisitionLines table", "2015-07-09")]
public class v900058d
{
	public v900058d(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseRequisitionLines", "wqlWarehouseID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseRequisitionLines", "wqlWarehouseID", "wqlSourceWarehouseID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseRequisitionLines", "wqlPartBinID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseRequisitionLines", "wqlPartBinID", "wqlSourcePartBinID", dropTriggers: true);
		}
	}
}
