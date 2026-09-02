using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert JobMemos to support unicode", "2013-10-17")]
public class v810RebuildJobMemos
{
	public v810RebuildJobMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMemos", new DmoField[11]
		{
			new DmoField("jmkJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmkJobMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("jmkMemoDate", "date", 14, 0, nullable: true),
			new DmoField("jmkShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("jmkLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("jmkLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("jmkClosed", "bit", 1, 0, nullable: false),
			new DmoField("jmkShowInJobs", "bit", 1, 0, nullable: false),
			new DmoField("jmkCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmkCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("jmkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("JMKJOBID,JMKJOBMEMOID", unique: true),
			new DmoIndex("JMKUNIQUEID", unique: true),
			new DmoIndex("jmkJobID", unique: false),
			new DmoIndex("jmkJobMemoID", unique: false),
			new DmoIndex("jmkMemoDate", unique: false),
			new DmoIndex("jmkClosed", unique: false),
			new DmoIndex("jmkShowInJobs", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
