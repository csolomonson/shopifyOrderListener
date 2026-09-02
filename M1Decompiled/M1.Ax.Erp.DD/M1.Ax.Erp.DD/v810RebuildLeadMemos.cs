using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LeadMemos to support unicode", "2013-10-17")]
public class v810RebuildLeadMemos
{
	public v810RebuildLeadMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LeadMemos", new DmoField[9]
		{
			new DmoField("lokLeadID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lokLeadMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("lokMemoDate", "date", 14, 0, nullable: true),
			new DmoField("lokShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lokLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lokLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lokCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lokCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lokUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LOKLEADID,LOKLEADMEMOID", unique: true),
			new DmoIndex("LOKUNIQUEID", unique: true),
			new DmoIndex("lokLeadID", unique: false),
			new DmoIndex("lokLeadMemoID", unique: false),
			new DmoIndex("lokMemoDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
