using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.300", "Add fields to JobSplitLogLines table", "2022-03-24")]
public class v95300b
{
	public v95300b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "JobSplitLogLines"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobSplitLogLines", new DmoField[7]
			{
				new DmoField("jslJobSplitLogID", "int", 4, 0, nullable: false),
				new DmoField("jslJobSplitLogLineID", "int", 4, 0, nullable: false),
				new DmoField("jslSourceTableName", "nvarchar", 30, 0, nullable: false),
				new DmoField("jslSourceTableUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("jslDestTableName", "nvarchar", 30, 0, nullable: false),
				new DmoField("jslDestTableUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("jslUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[6]
			{
				new DmoIndex("jslJobSplitLogID,jslJobSplitLogLineID", unique: true),
				new DmoIndex("jslUniqueID", unique: true),
				new DmoIndex("jslSourceTableName", unique: false),
				new DmoIndex("jslSourceTableUniqueID", unique: false),
				new DmoIndex("jslDestTableName", unique: false),
				new DmoIndex("jslDestTableUniqueID", unique: false)
			});
		}
	}
}
