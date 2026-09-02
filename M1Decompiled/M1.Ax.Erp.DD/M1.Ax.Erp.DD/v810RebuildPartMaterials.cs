using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartMaterials to support unicode", "2013-10-17")]
public class v810RebuildPartMaterials
{
	public v810RebuildPartMaterials(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartMaterials", new DmoField[27]
		{
			new DmoField("immMethodID", "nvarchar", 30, 0, nullable: false),
			new DmoField("immMethodRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("immMethodAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("immMethodMaterialID", "int", 5, 0, nullable: false),
			new DmoField("immPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("immPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("immUseDefaultWarehouseAndBin", "bit", 1, 0, nullable: false),
			new DmoField("immPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("immPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("immUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("immPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("immPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("immPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("immQuantityPerAssembly", "numeric", 13, 6, nullable: false),
			new DmoField("immEstimatedUnitCost", "numeric", 15, 5, nullable: false),
			new DmoField("immScrapQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("immScrapPercent", "numeric", 6, 2, nullable: false),
			new DmoField("immSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("immPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("immLeadTime", "smallint", 3, 0, nullable: false),
			new DmoField("immMinimumCharge", "numeric", 8, 2, nullable: false),
			new DmoField("immRelatedPartOperationID", "int", 5, 0, nullable: false),
			new DmoField("immBackflush", "bit", 1, 0, nullable: false),
			new DmoField("immDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("immCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("immCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("immUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[10]
		{
			new DmoIndex("IMMMETHODID,IMMMETHODREVISIONID,IMMMETHODASSEMBLYID,IMMMETHODMATERIALID", unique: true),
			new DmoIndex("IMMUNIQUEID", unique: true),
			new DmoIndex("immMethodID", unique: false),
			new DmoIndex("immMethodRevisionID", unique: false),
			new DmoIndex("immMethodAssemblyID", unique: false),
			new DmoIndex("immMethodMaterialID", unique: false),
			new DmoIndex("immPartID", unique: false),
			new DmoIndex("immPartRevisionID", unique: false),
			new DmoIndex("immPartWarehouseLocationID", unique: false),
			new DmoIndex("immPartBinID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
