using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.080", "Add fields to ShipmentPackages table", "2015-09-07")]
public class v900080c
{
	public v900080c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentPackages", "spaShipmentIDNumber"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackages", "spaShipmentIDNumber", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
