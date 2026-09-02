using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert RFQMemos to support unicode", "2013-10-17")]
public class v810RebuildRFQMemos
{
	public v810RebuildRFQMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RFQMemos", new DmoField[10]
		{
			new DmoField("rqkRFQID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rqkRFQMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("rqkMemoDate", "date", 14, 0, nullable: true),
			new DmoField("rqkShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("rqkLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rqkLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rqkClosed", "bit", 1, 0, nullable: false),
			new DmoField("rqkCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rqkCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rqkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("RQKRFQID,RQKRFQMEMOID", unique: true),
			new DmoIndex("RQKUNIQUEID", unique: true),
			new DmoIndex("rqkRFQID", unique: false),
			new DmoIndex("rqkRFQMemoID", unique: false),
			new DmoIndex("rqkMemoDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
