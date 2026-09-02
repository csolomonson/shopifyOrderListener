using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Update mrrPartPlantID in MRPDemands", "2021-12-15")]
public class v95200e
{
	public v95200e(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE MRPDemands SET mrrPartPlantID=(select COALESCE(xauPlantID,'') from Warehouses left outer join Plants on xauPlantID=imwPlantID where mrrPartWarehouseLocationID=imwWarehouseID)");
	}
}
