using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AssetMemos to support unicode", "2013-10-17")]
public class v810RebuildAssetMemos
{
	public v810RebuildAssetMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AssetMemos", new DmoField[9]
		{
			new DmoField("fakAssetID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fakAssetMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("fakMemoDate", "date", 14, 0, nullable: true),
			new DmoField("fakShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("fakLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fakLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fakCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("fakCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("fakUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("FAKASSETID,FAKASSETMEMOID", unique: true),
			new DmoIndex("FAKUNIQUEID", unique: true),
			new DmoIndex("fakAssetID", unique: false),
			new DmoIndex("fakAssetMemoID", unique: false),
			new DmoIndex("fakMemoDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
