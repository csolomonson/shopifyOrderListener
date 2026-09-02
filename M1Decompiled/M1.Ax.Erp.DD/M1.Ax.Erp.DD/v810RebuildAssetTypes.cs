using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AssetTypes to support unicode", "2013-10-17")]
public class v810RebuildAssetTypes
{
	public v810RebuildAssetTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetTypes", new DmoField[13]
		{
			new DmoField("fatAssetTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("fatDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("fatAssetGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fatDepreciationGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fatAccumDeprGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fatExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fatRepairsGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fatRevaluationGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fatProfitGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fatLossGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fatCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("fatCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("fatUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("FATASSETTYPEID", unique: true),
			new DmoIndex("FATUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
