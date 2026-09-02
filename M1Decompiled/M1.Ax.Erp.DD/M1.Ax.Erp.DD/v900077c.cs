using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.077", "Update field bindings", "2015-08-21")]
public class v900077c
{
	public v900077c(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ShipmentPackages Set SPAShippingMethodID = smpShippingMethodID From Shipments Inner Join ShipmentPackages On SMPSHIPMENTID = SPASHIPMENTID");
	}
}
