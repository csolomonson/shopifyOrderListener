using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.516", "Add ShipmentPackages and ShipmentPackageDetails", "2014-07-11")]
public class v800516
{
	public v800516(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ShipmentPackages"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackages");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ShipmentPackageDetails"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackageDetails");
		}
	}
}
