using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert CallMemos to support unicode", "2013-10-17")]
public class v810RebuildCallMemos
{
	public v810RebuildCallMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CallMemos", new DmoField[9]
		{
			new DmoField("kbkCallID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbkCallMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("kbkMemoDate", "date", 14, 0, nullable: true),
			new DmoField("kbkShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("kbkLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbkLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbkCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbkCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("KBKCALLID,KBKCALLMEMOID", unique: true),
			new DmoIndex("KBKUNIQUEID", unique: true),
			new DmoIndex("kbkCallID", unique: false),
			new DmoIndex("kbkCallMemoID", unique: false),
			new DmoIndex("kbkMemoDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
