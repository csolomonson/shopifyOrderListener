using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.300", "Add fields to JobSplitLog table", "2022-04-24")]
public class v95300c
{
	public v95300c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "JobSplitLog"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobSplitLog", new DmoField[11]
			{
				new DmoField("jsgJobSplitLogID", "identity", 4, 0, nullable: false),
				new DmoField("jsgSourceTableName", "nvarchar", 30, 0, nullable: false),
				new DmoField("jsgSourceTableUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("jsgDestTableName", "nvarchar", 30, 0, nullable: false),
				new DmoField("jsgDestTableUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("jsgSplitCostsOption", "tinyint", 1, 0, nullable: false),
				new DmoField("jsgSplitQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("jsgRequiredDate", "date", 14, 0, nullable: true),
				new DmoField("jsgCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("jsgCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("jsgUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[7]
			{
				new DmoIndex("jsgJobSplitLogID", unique: true),
				new DmoIndex("jsgUniqueID", unique: true),
				new DmoIndex("jsgSourceTableName", unique: false),
				new DmoIndex("jsgSourceTableUniqueID", unique: false),
				new DmoIndex("jsgDestTableName", unique: false),
				new DmoIndex("jsgDestTableUniqueID", unique: false),
				new DmoIndex("jsgRequiredDate", unique: false)
			});
		}
	}
}
