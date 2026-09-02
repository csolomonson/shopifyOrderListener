using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert KnowledgeBasePages to support unicode", "2013-10-17")]
public class v810RebuildKnowledgeBasePages
{
	public v810RebuildKnowledgeBasePages(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "KnowledgeBasePages", new DmoField[21]
		{
			new DmoField("kbbKnowledgeBasePageID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbbPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("kbbPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("kbbDescription", "nvarchar", 70, 0, nullable: false),
			new DmoField("kbbProblemDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbbProblemDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbbResolutionDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbbResolutionDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbbWorkAroundDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbbWorkAroundDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kbbResolvedPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("kbbResolvedPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("kbbAccessedCount", "numeric", 10, 0, nullable: false),
			new DmoField("kbbOpenedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbbOpenedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbbStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("kbbClosedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kbbClosedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbbCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbbCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbbUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[10]
		{
			new DmoIndex("KBBKNOWLEDGEBASEPAGEID", unique: true),
			new DmoIndex("KBBUNIQUEID", unique: true),
			new DmoIndex("kbbPartID", unique: false),
			new DmoIndex("kbbPartRevisionID", unique: false),
			new DmoIndex("kbbResolvedPartID", unique: false),
			new DmoIndex("kbbResolvedPartRevisionID", unique: false),
			new DmoIndex("kbbAccessedCount", unique: false),
			new DmoIndex("kbbOpenedByEmployeeID", unique: false),
			new DmoIndex("kbbStatus", unique: false),
			new DmoIndex("kbbClosedByEmployeeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
