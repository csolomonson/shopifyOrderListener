using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartClasses to support unicode", "2013-10-17")]
public class v810RebuildPartClasses
{
	public v810RebuildPartClasses(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartClasses", new DmoField[24]
		{
			new DmoField("imcPartClassID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imcDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("imcShowGroupOnWeb", "bit", 1, 0, nullable: false),
			new DmoField("imcPartImageFileName", "nvarchar", 70, 0, nullable: false),
			new DmoField("imcParentPartClassID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imcRequiresInspection", "bit", 1, 0, nullable: false),
			new DmoField("imcWebConfigPriceRule", "tinyint", 1, 0, nullable: false),
			new DmoField("imcWebConfigMode", "tinyint", 1, 0, nullable: false),
			new DmoField("imcFdxPackaging", "nvarchar", 14, 0, nullable: false),
			new DmoField("imcWeight", "numeric", 15, 5, nullable: false),
			new DmoField("imcFdxOneItemPerShipment", "bit", 1, 0, nullable: false),
			new DmoField("imcFdxNonstandardContainer", "bit", 1, 0, nullable: false),
			new DmoField("imcFdxPackageLength", "int", 3, 0, nullable: false),
			new DmoField("imcFdxPackageWidth", "int", 3, 0, nullable: false),
			new DmoField("imcFdxPackageHeight", "int", 3, 0, nullable: false),
			new DmoField("imcFdxHandlingCost", "numeric", 7, 2, nullable: false),
			new DmoField("imcFdxPackagingCost", "numeric", 7, 2, nullable: false),
			new DmoField("imcFdxShipCostMarkupPct", "numeric", 5, 2, nullable: false),
			new DmoField("imcInventoryGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imcInactive", "bit", 1, 0, nullable: false),
			new DmoField("imcCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imcCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imcInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("imcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("IMCPARTCLASSID", unique: true),
			new DmoIndex("IMCUNIQUEID", unique: true),
			new DmoIndex("imcParentPartClassID", unique: false),
			new DmoIndex("imcInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
