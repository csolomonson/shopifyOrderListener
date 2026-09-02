using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ProductCategories to support unicode", "2013-10-17")]
public class v810RebuildProductCategories
{
	public v810RebuildProductCategories(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductCategories", new DmoField[10]
		{
			new DmoField("incProductCategoryID", "nvarchar", 30, 0, nullable: false),
			new DmoField("incStructureID", "nvarchar", 2, 0, nullable: false),
			new DmoField("incStructureCode", "nvarchar", 2, 0, nullable: false),
			new DmoField("incDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("incImageFilePath", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("incInactive", "bit", 1, 0, nullable: false),
			new DmoField("incInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("incCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("incCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("incUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("INCPRODUCTCATEGORYID", unique: true),
			new DmoIndex("INCUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
