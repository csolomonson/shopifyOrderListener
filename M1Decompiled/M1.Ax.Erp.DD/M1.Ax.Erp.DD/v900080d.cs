using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.080", "Update field bindings", "2015-09-07")]
public class v900080d
{
	public v900080d(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Shipments", "smpShipmentIDNumber"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("begin SET NOCOUNT ON DECLARE @SQL3 varchar(8000) set @sql3 = ' Update ShipmentLines Set smlShipmentIDNumber = smpShipmentIDNumber From Shipments Inner Join ShipmentLines On SMPSHIPMENTID = SMLSHIPMENTID; Update ShipmentPackageDetails Set spdShipmentIDNumber = smlShipmentIDNumber From ShipmentLines Inner Join ShipmentPackageDetails On SMLSHIPMENTID = SPDSHIPMENTID And SMLSHIPMENTLINEID = SPDSHIPMENTLINEID; Update ShipmentPackages Set spaShipmentIDNumber = smpShipmentIDNumber From Shipments Inner Join ShipmentPackages On SMPSHIPMENTID = SPASHIPMENTID; ' exec(@sql3) SET NOCOUNT OFF end;");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
		}
	}
}
