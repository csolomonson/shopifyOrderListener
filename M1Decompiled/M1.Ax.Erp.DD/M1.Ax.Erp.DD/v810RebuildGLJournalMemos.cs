using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLJournalMemos to support unicode", "2013-10-17")]
public class v810RebuildGLJournalMemos
{
	public v810RebuildGLJournalMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLJournalMemos", new DmoField[10]
		{
			new DmoField("glmGLJournalID", "int", 9, 0, nullable: false),
			new DmoField("glmGlJournalMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("glmMemoDate", "date", 14, 0, nullable: true),
			new DmoField("glmShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("glmLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("glmLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("glmClosed", "bit", 1, 0, nullable: false),
			new DmoField("glmCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glmCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glmUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("GLMGLJOURNALID,GLMGLJOURNALMEMOID", unique: true),
			new DmoIndex("GLMUNIQUEID", unique: true),
			new DmoIndex("glmGLJournalID", unique: false),
			new DmoIndex("glmGlJournalMemoID", unique: false),
			new DmoIndex("glmMemoDate", unique: false),
			new DmoIndex("glmClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
