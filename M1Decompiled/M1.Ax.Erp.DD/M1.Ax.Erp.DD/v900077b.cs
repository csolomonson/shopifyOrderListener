using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.077", "Add fields to ShipmentPackages table", "2015-08-21")]
public class v900077b
{
	public v900077b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentPackages", "spaUPSPackageTypes"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackages", "spaUPSPackageTypes", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentPackages", "spaCustomerPackageID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackages", "spaCustomerPackageID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentPackages", "SPAShippingMethodID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackages", "SPAShippingMethodID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentPackages", "spaFedExPackageTypes"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackages", "spaFedExPackageTypes", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentPackages", "SPACarrier"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackages", "SPACarrier", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTPACKAGES", "spaPackageDescription"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTPACKAGES", "spaPackageDescription", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTPACKAGES", "spaPackageType"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTPACKAGES", "spaPackageType", dropTriggers: true);
		}
		parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentPackages", "SPACarrier");
	}
}
