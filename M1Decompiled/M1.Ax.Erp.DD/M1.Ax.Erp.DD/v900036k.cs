using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.036", "Add fields to ShipmentPackages table", "2015-05-19")]
public class v900036k
{
	public v900036k(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ShipmentPackages"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackages", new DmoField[24]
			{
				new DmoField("spaShipmentID", "nvarchar", 10, 0, nullable: false),
				new DmoField("spaShipmentPackageID", "int", 5, 0, nullable: false),
				new DmoField("spaPackageDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("spaPackageType", "nvarchar", 20, 0, nullable: false),
				new DmoField("spaPackageDimensionsUOM", "nvarchar", 2, 0, nullable: false),
				new DmoField("spaPackageHeight", "int", 3, 0, nullable: false),
				new DmoField("spaPackageLength", "int", 3, 0, nullable: false),
				new DmoField("spaPackageWidth", "int", 3, 0, nullable: false),
				new DmoField("spaPackageWeight", "numeric", 6, 2, nullable: false),
				new DmoField("spaPackageWeightUOM", "nvarchar", 3, 0, nullable: false),
				new DmoField("spaPackageRate", "money", 12, 2, nullable: false),
				new DmoField("spaLargePackage", "bit", 1, 0, nullable: false),
				new DmoField("spaAdditionalHandlingRequired", "bit", 1, 0, nullable: false),
				new DmoField("spaVerbalConfirmationRequired", "bit", 1, 0, nullable: false),
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
			});
		}
	}
}
