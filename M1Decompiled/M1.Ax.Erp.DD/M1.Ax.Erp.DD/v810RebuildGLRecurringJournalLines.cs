using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLRecurringJournalLines to support unicode", "2013-10-17")]
public class v810RebuildGLRecurringJournalLines
{
	public v810RebuildGLRecurringJournalLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLRecurringJournalLines", new DmoField[12]
		{
			new DmoField("gljRecurringJournalID", "int", 9, 0, nullable: false),
			new DmoField("gljRecurringJournalLineID", "int", 5, 0, nullable: false),
			new DmoField("gljAmount", "money", 12, 2, nullable: false),
			new DmoField("gljDebitAmount", "money", 12, 2, nullable: false),
			new DmoField("gljCreditAmount", "money", 12, 2, nullable: false),
			new DmoField("gljGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("gljReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("gljDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("gljInactive", "bit", 1, 0, nullable: false),
			new DmoField("gljCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("gljCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("gljUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("GLJRECURRINGJOURNALID,GLJRECURRINGJOURNALLINEID", unique: true),
			new DmoIndex("GLJUNIQUEID", unique: true),
			new DmoIndex("gljRecurringJournalID", unique: false),
			new DmoIndex("gljRecurringJournalLineID", unique: false),
			new DmoIndex("gljInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
