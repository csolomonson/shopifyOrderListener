using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AssetTypePlants to support unicode", "2013-10-17")]
public class v810RebuildAssetTypePlants
{
	public v810RebuildAssetTypePlants(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetTypePlants", new DmoField[13]
		{
			new DmoField("fayAssetTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("fayAssetTypePlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("fayAssetGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fayDepreciationGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fayAccumDeprGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fayExpenseGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fayRepairsGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fayRevaluationGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fayProfitGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fayLossGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("fayCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("fayCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("fayUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("FAYASSETTYPEID,FAYASSETTYPEPLANTID", unique: true),
			new DmoIndex("FAYUNIQUEID", unique: true),
			new DmoIndex("fayAssetTypeID", unique: false),
			new DmoIndex("fayAssetTypePlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
