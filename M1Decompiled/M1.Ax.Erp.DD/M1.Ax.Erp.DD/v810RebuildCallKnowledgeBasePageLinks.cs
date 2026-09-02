using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert CallKnowledgeBasePageLinks to support unicode", "2013-10-17")]
public class v810RebuildCallKnowledgeBasePageLinks
{
	public v810RebuildCallKnowledgeBasePageLinks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CallKnowledgeBasePageLinks", new DmoField[7]
		{
			new DmoField("kbwCallID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbwCallLineID", "smallint", 4, 0, nullable: false),
			new DmoField("kbwCallKnowledgebasePageLinkID", "smallint", 4, 0, nullable: false),
			new DmoField("kbwKnowledgeBasePageID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbwCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbwCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbwUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("KBWCALLID,KBWCALLKNOWLEDGEBASEPAGELINKID", unique: true),
			new DmoIndex("KBWUNIQUEID", unique: true),
			new DmoIndex("kbwCallID", unique: false),
			new DmoIndex("kbwCallLineID", unique: false),
			new DmoIndex("kbwCallKnowledgebasePageLinkID", unique: false),
			new DmoIndex("kbwKnowledgeBasePageID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
