using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AttachmentMemos to support unicode", "2013-10-17")]
public class v810RebuildAttachmentMemos
{
	public v810RebuildAttachmentMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AttachmentMemos", new DmoField[9]
		{
			new DmoField("cmqAttachmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("cmqAttachmentMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("cmqMemoDate", "date", 14, 0, nullable: true),
			new DmoField("cmqShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmqLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmqLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("cmqCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmqCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("CMQATTACHMENTID,CMQATTACHMENTMEMOID", unique: true),
			new DmoIndex("CMQUNIQUEID", unique: true),
			new DmoIndex("cmqAttachmentID", unique: false),
			new DmoIndex("cmqAttachmentMemoID", unique: false),
			new DmoIndex("cmqMemoDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
