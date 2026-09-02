using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AssetPoolTransactions to support unicode", "2013-10-17")]
public class v810RebuildAssetPoolTransactions
{
	public v810RebuildAssetPoolTransactions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetPoolTransactions", new DmoField[10]
		{
			new DmoField("fawPoolTransactionID", "int", 9, 0, nullable: false),
			new DmoField("fawPoolYearID", "smallint", 4, 0, nullable: false),
			new DmoField("fawAssetID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fawAssetAdjustmentID", "int", 9, 0, nullable: false),
			new DmoField("fawTransactionDate", "date", 14, 0, nullable: true),
			new DmoField("fawTransactionType", "nvarchar", 1, 0, nullable: false),
			new DmoField("fawAmount", "money", 12, 2, nullable: false),
			new DmoField("fawCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("fawCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("fawUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("FAWPOOLTRANSACTIONID", unique: true),
			new DmoIndex("FAWUNIQUEID", unique: true),
			new DmoIndex("fawPoolYearID", unique: false),
			new DmoIndex("fawAssetID", unique: false),
			new DmoIndex("fawAssetAdjustmentID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
