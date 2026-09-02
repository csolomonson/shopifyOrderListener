using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WorkCenterMemos to support unicode", "2013-10-17")]
public class v810RebuildWorkCenterMemos
{
	public v810RebuildWorkCenterMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenterMemos", new DmoField[9]
		{
			new DmoField("xakWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xakWorkCenterMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("xakMemoDate", "date", 14, 0, nullable: true),
			new DmoField("xakShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xakLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xakLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xakCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xakCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xakUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("XAKWORKCENTERID,XAKWORKCENTERMEMOID", unique: true),
			new DmoIndex("XAKUNIQUEID", unique: true),
			new DmoIndex("xakWorkCenterID", unique: false),
			new DmoIndex("xakWorkCenterMemoID", unique: false),
			new DmoIndex("xakMemoDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
