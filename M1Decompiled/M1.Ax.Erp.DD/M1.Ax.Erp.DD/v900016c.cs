using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.016", "Add fields to ScheduleTrees table", "2015-02-08")]
public class v900016c
{
	public v900016c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTrees"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", new DmoField[6]
			{
				new DmoField("sxtScheduleTreeID", "identity", 4, 0, nullable: false),
				new DmoField("sxtSourceTable", "nvarchar", 30, 0, nullable: false),
				new DmoField("sxtSourceUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("sxtCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("sxtCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("sxtUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("sxtScheduleTreeID", unique: true),
				new DmoIndex("sxtUniqueID", unique: true)
			});
		}
	}
}
