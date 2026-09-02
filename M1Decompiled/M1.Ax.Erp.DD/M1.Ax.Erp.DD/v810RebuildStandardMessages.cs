using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert StandardMessages to support unicode", "2013-10-17")]
public class v810RebuildStandardMessages
{
	public v810RebuildStandardMessages(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "StandardMessages", new DmoField[10]
		{
			new DmoField("xamStandardMessageID", "nvarchar", 10, 0, nullable: false),
			new DmoField("xamMessageType", "tinyint", 1, 0, nullable: false),
			new DmoField("xamShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xamLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xamLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xamInactive", "bit", 1, 0, nullable: false),
			new DmoField("xamInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("xamCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xamCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xamUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("XAMSTANDARDMESSAGEID", unique: true),
			new DmoIndex("XAMUNIQUEID", unique: true),
			new DmoIndex("xamMessageType", unique: false),
			new DmoIndex("xamInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
