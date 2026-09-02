using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AssetLowValuePool to support unicode", "2013-10-17")]
public class v810RebuildAssetLowValuePool
{
	public v810RebuildAssetLowValuePool(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetLowValuePool", new DmoField[16]
		{
			new DmoField("favPoolYearID", "smallint", 4, 0, nullable: false),
			new DmoField("favOpeningBalance", "money", 12, 2, nullable: false),
			new DmoField("favLowCostAddition", "money", 12, 2, nullable: false),
			new DmoField("favLowValueAddition", "money", 12, 2, nullable: false),
			new DmoField("favImprovement", "money", 12, 2, nullable: false),
			new DmoField("favLowRateDepreciation", "money", 12, 2, nullable: false),
			new DmoField("favHighRateDepreciation", "money", 12, 2, nullable: false),
			new DmoField("favTermination", "money", 12, 2, nullable: false),
			new DmoField("favEndingBalance", "money", 12, 2, nullable: false),
			new DmoField("favLowRate", "numeric", 6, 2, nullable: false),
			new DmoField("favHighRate", "numeric", 6, 2, nullable: false),
			new DmoField("favClosed", "bit", 1, 0, nullable: false),
			new DmoField("favClosedDate", "date", 14, 0, nullable: true),
			new DmoField("favCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("favCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("favUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("FAVPOOLYEARID", unique: true),
			new DmoIndex("FAVUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
