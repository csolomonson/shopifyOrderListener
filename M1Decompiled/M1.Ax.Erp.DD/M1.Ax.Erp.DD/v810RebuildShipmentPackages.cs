using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ShipmentPackages to support unicode", "2013-10-17")]
public class v810RebuildShipmentPackages
{
	public v810RebuildShipmentPackages(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackages", new DmoField[28]
		{
			new DmoField("spaShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("spaShipmentPackageID", "int", 5, 0, nullable: false),
			new DmoField("SPAShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("SPACarrier", "nvarchar", 5, 0, nullable: false),
			new DmoField("spaUPSPackageTypes", "nvarchar", 20, 0, nullable: false),
			new DmoField("spaFedExPackageTypes", "nvarchar", 20, 0, nullable: false),
			new DmoField("spaCustomerPackageID", "char", 10, 0, nullable: false),
			new DmoField("spaPackageDimensionsUOM", "nvarchar", 2, 0, nullable: false),
			new DmoField("spaPackageHeight", "int", 3, 0, nullable: false),
			new DmoField("spaPackageLength", "int", 3, 0, nullable: false),
			new DmoField("spaPackageWidth", "int", 3, 0, nullable: false),
			new DmoField("spaPackageWeightUOM", "nvarchar", 3, 0, nullable: false),
			new DmoField("spaPackageWeight", "numeric", 15, 5, nullable: false),
			new DmoField("spaPackageRate", "money", 12, 2, nullable: false),
			new DmoField("spaLargePackage", "bit", 1, 0, nullable: false),
			new DmoField("spaAdditionalHandlingRequired", "bit", 1, 0, nullable: false),
			new DmoField("spaVerbalConfirmationRequired", "bit", 1, 0, nullable: false),
			new DmoField("spaShipmentIDNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("spaTrackingNo", "nvarchar", 20, 0, nullable: false),
			new DmoField("spaPackageValue", "money", 12, 2, nullable: false),
			new DmoField("spaPackageRateForeign", "money", 12, 2, nullable: false),
			new DmoField("spaReference1", "nvarchar", 35, 0, nullable: false),
			new DmoField("spaReference2", "nvarchar", 35, 0, nullable: false),
			new DmoField("spaLabelFilePath", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("spaPackageValueForeign", "money", 12, 2, nullable: false),
			new DmoField("spaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("spaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("spaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("SPASHIPMENTID,SPASHIPMENTPACKAGEID", unique: true),
			new DmoIndex("SPAUNIQUEID", unique: true),
			new DmoIndex("spaShipmentID", unique: false),
			new DmoIndex("spaShipmentPackageID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
