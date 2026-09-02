using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SupplierRatings to support unicode", "2013-10-17")]
public class v810RebuildSupplierRatings
{
	public v810RebuildSupplierRatings(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SupplierRatings", new DmoField[5]
		{
			new DmoField("cmsSupplierRatingID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmsDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CMSSUPPLIERRATINGID", unique: true),
			new DmoIndex("CMSUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
