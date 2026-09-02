using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.080", "Add fields to ShipmentPackageDetails table", "2015-09-07")]
public class v900080a
{
	public v900080a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentPackageDetails", "spdWeightUnitOfMeasure"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackageDetails", "spdWeightUnitOfMeasure", "nvarchar", 3, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentPackageDetails", "spdShipmentIDNumber"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackageDetails", "spdShipmentIDNumber", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShipmentPackageDetails", "spdShipmentPackageLineID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackageDetails", "spdShipmentPackageLineID", "int", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
