using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AssetTypeMethods to support unicode", "2013-10-17")]
public class v810RebuildAssetTypeMethods
{
	public v810RebuildAssetTypeMethods(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetTypeMethods", new DmoField[13]
		{
			new DmoField("famAssetTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("famAssetTypeMethodID", "smallint", 3, 0, nullable: false),
			new DmoField("famStartDate", "date", 14, 0, nullable: true),
			new DmoField("famTaxDepreciationMethod", "nvarchar", 5, 0, nullable: false),
			new DmoField("famTaxMultiplier", "numeric", 4, 2, nullable: false),
			new DmoField("famBookDepreciationMethod", "nvarchar", 5, 0, nullable: false),
			new DmoField("famBookMultiplier", "numeric", 4, 2, nullable: false),
			new DmoField("famCalculationMethod", "nvarchar", 1, 0, nullable: false),
			new DmoField("famMonthCalculationType", "nvarchar", 1, 0, nullable: false),
			new DmoField("famCurrentMethod", "bit", 1, 0, nullable: false),
			new DmoField("famCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("famCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("famUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("FAMASSETTYPEID,FAMASSETTYPEMETHODID", unique: true),
			new DmoIndex("FAMUNIQUEID", unique: true),
			new DmoIndex("famAssetTypeID", unique: false),
			new DmoIndex("famAssetTypeMethodID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
