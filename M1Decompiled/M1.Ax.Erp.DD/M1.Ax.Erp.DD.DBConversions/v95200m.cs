using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Update mrjDataMissing in MRPJobDetails", "2022-01-21")]
public class v95200m
{
	public v95200m(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE MRPJobDetails set mrjDataMissing = IIF(mrjPartWarehouseLocationID = '' or mrjPartBinID = '',1,0)");
	}
}
