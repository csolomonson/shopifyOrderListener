using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ProductCategoryLines to support unicode", "2013-10-17")]
public class v810RebuildProductCategoryLines
{
	public v810RebuildProductCategoryLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductCategoryLines", new DmoField[13]
		{
			new DmoField("insProductCategoryID", "nvarchar", 30, 0, nullable: false),
			new DmoField("insProductCategoryLineID", "smallint", 4, 0, nullable: false),
			new DmoField("insParentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("insLevel", "tinyint", 1, 0, nullable: false),
			new DmoField("insStructureID", "nvarchar", 2, 0, nullable: false),
			new DmoField("insStructureCode", "nvarchar", 14, 0, nullable: false),
			new DmoField("insDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("insImageFilePath", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("insInactive", "bit", 1, 0, nullable: false),
			new DmoField("insInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("insCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("insCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("insUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("INSPRODUCTCATEGORYID,INSPRODUCTCATEGORYLINEID", unique: true),
			new DmoIndex("INSUNIQUEID", unique: true),
			new DmoIndex("insProductCategoryID", unique: false),
			new DmoIndex("insProductCategoryLineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
