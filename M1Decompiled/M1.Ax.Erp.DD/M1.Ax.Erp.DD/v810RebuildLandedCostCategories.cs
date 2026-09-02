using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LandedCostCategories to support unicode", "2013-10-17")]
public class v810RebuildLandedCostCategories
{
	public v810RebuildLandedCostCategories(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCategories", new DmoField[10]
		{
			new DmoField("rmaLandedCostCategoryID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmaCategoryType", "tinyint", 1, 0, nullable: false),
			new DmoField("rmaDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("rmaDefault", "bit", 1, 0, nullable: false),
			new DmoField("rmaSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rmaSupplierLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmaLandedCostMethod", "tinyint", 1, 0, nullable: false),
			new DmoField("rmaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rmaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rmaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("RMALANDEDCOSTCATEGORYID", unique: true),
			new DmoIndex("RMAUNIQUEID", unique: true),
			new DmoIndex("rmaSupplierOrganizationID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
