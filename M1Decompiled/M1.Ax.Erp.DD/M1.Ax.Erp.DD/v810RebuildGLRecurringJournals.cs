using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert GLRecurringJournals to support unicode", "2013-10-17")]
public class v810RebuildGLRecurringJournals
{
	public v810RebuildGLRecurringJournals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "GLRecurringJournals", new DmoField[29]
		{
			new DmoField("glrRecurringJournalID", "int", 9, 0, nullable: false),
			new DmoField("glrDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("glrReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("glrStartGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("glrStartGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("glrEndGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("glrEndGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("glrReversingEntry", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod01", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod02", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod03", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod04", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod05", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod06", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod07", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod08", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod09", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod10", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod11", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod12", "bit", 1, 0, nullable: false),
			new DmoField("glrPeriod13", "bit", 1, 0, nullable: false),
			new DmoField("glrLastTransferredDate", "date", 14, 0, nullable: true),
			new DmoField("glrTotalDebits", "money", 12, 2, nullable: false),
			new DmoField("glrTotalCredits", "money", 12, 2, nullable: false),
			new DmoField("glrInactive", "bit", 1, 0, nullable: false),
			new DmoField("glrInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("glrCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("glrCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("glrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("GLRRECURRINGJOURNALID", unique: true),
			new DmoIndex("GLRUNIQUEID", unique: true),
			new DmoIndex("glrReversingEntry", unique: false),
			new DmoIndex("glrInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
