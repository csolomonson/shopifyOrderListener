using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Rename fields in MRPLines table", "2022-01-12")]
public class v95200j
{
	public v95200j(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPLines", "mrlPlantID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", "mrlPlantID", "mrlPlantIDs", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPLines", "mrlPlantIDs"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", "mrlPlantIDs", "nvarchar(max)", 100, 0, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPLines", "mrlWarehouseID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", "mrlWarehouseID", "mrlWarehouseIDs", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPLines", "mrlWarehouseIDs"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", "mrlWarehouseIDs", "nvarchar(max)", 100, 0, isNullable: true, parms.Messages);
		}
	}
}
