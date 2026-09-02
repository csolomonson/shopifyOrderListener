using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Update mrjPartPlantID in MRPJobDetails", "2021-12-16")]
public class v95200g
{
	public v95200g(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE MRPJobDetails SET mrjPartPlantID=(select COALESCE(xauPlantID,'') from Warehouses left outer join Plants on xauPlantID=imwPlantID where mrjPartWarehouseLocationID=imwWarehouseID)");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE MRPJobDetails SET mrjPartPlantID='' where mrjPartPlantID is null");
	}
}
