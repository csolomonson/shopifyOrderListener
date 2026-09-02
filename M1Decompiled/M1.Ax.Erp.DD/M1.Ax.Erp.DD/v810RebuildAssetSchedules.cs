using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AssetSchedules to support unicode", "2013-10-17")]
public class v810RebuildAssetSchedules
{
	public v810RebuildAssetSchedules(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetSchedules", new DmoField[20]
		{
			new DmoField("fasAssetID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fasAssetScheduleID", "int", 5, 0, nullable: false),
			new DmoField("fasType", "nvarchar", 5, 0, nullable: false),
			new DmoField("fasGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("fasGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("fasOpeningAssetValue", "money", 12, 2, nullable: false),
			new DmoField("fasAdditionalAssetAmount", "money", 12, 2, nullable: false),
			new DmoField("fasSubtractAssetAmount", "money", 12, 2, nullable: false),
			new DmoField("fasClosingAssetValue", "money", 12, 2, nullable: false),
			new DmoField("fasOpeningAccumBalance", "money", 12, 2, nullable: false),
			new DmoField("fasDepreciationAmount", "money", 12, 2, nullable: false),
			new DmoField("fasWritebackAmount", "money", 12, 2, nullable: false),
			new DmoField("fasClosingAccumBalance", "money", 12, 2, nullable: false),
			new DmoField("fasNetAssetValue", "money", 12, 2, nullable: false),
			new DmoField("fasEstimatedProductionUnits", "int", 9, 0, nullable: false),
			new DmoField("fasActualProductionUnits", "int", 9, 0, nullable: false),
			new DmoField("fasPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("fasCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("fasCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("fasUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("FASASSETID,FASASSETSCHEDULEID", unique: true),
			new DmoIndex("FASUNIQUEID", unique: true),
			new DmoIndex("fasAssetID", unique: false),
			new DmoIndex("fasAssetScheduleID", unique: false),
			new DmoIndex("fasType", unique: false),
			new DmoIndex("fasGLFiscalYearID", unique: false),
			new DmoIndex("fasGLFiscalYearPeriodID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
