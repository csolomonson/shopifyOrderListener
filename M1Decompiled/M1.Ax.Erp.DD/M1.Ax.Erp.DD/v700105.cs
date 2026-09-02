using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.105", "Update FreightShipments field", "2008-04-17")]
public class v700105
{
	public v700105(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FreightShipments SET fspShipFromOrganizationID = '1' WHERE fspShipFromOrganizationID = ''");
	}
}
