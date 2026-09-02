using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert FreightPackages to support unicode", "2013-10-17")]
public class v810RebuildFreightPackages
{
	public v810RebuildFreightPackages(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FreightPackages", new DmoField[21]
		{
			new DmoField("fslFreightShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fslFreightPackageID", "smallint", 4, 0, nullable: false),
			new DmoField("fslUPSPackageType", "nvarchar", 35, 0, nullable: false),
			new DmoField("fslTrackingNumber", "nvarchar", 50, 0, nullable: false),
			new DmoField("fslPackageFullWeight", "numeric", 7, 1, nullable: false),
			new DmoField("fslPackageCharge", "numeric", 9, 2, nullable: false),
			new DmoField("fslPackagePublishedCharge", "numeric", 9, 2, nullable: false),
			new DmoField("fslNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fslNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fslVoidOnUPS", "bit", 1, 0, nullable: false),
			new DmoField("fslDistributeCostsOption", "tinyint", 2, 0, nullable: false),
			new DmoField("fslFdxPackaging", "nvarchar", 14, 0, nullable: false),
			new DmoField("fslFdxNonstandardContainer", "bit", 1, 0, nullable: false),
			new DmoField("fslFdxPackageLength", "int", 3, 0, nullable: false),
			new DmoField("fslFdxPackageWidth", "int", 3, 0, nullable: false),
			new DmoField("fslFdxPackageHeight", "int", 3, 0, nullable: false),
			new DmoField("fslDimensionsUnitOfMeasure", "nvarchar", 3, 0, nullable: false),
			new DmoField("fslWeightUnitOfMeasure", "nvarchar", 3, 0, nullable: false),
			new DmoField("fslCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("fslCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("fslUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("FSLFREIGHTSHIPMENTID,FSLFREIGHTPACKAGEID", unique: true),
			new DmoIndex("FSLUNIQUEID", unique: true),
			new DmoIndex("fslFreightShipmentID", unique: false),
			new DmoIndex("fslFreightPackageID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
