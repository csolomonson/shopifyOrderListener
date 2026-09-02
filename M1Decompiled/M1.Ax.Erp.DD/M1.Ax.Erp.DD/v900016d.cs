using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.016", "Add fields to ScheduleTasks table", "2015-02-08")]
public class v900016d
{
	public v900016d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTasks"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", new DmoField[16]
			{
				new DmoField("sxkScheduleTreeID", "int", 4, 0, nullable: false),
				new DmoField("sxkScheduleBranchID", "int", 4, 0, nullable: false),
				new DmoField("sxkScheduleTaskID", "int", 4, 0, nullable: false),
				new DmoField("sxkWorkCenterID", "nvarchar", 5, 0, nullable: false),
				new DmoField("sxkMachineType", "tinyint", 1, 0, nullable: false),
				new DmoField("sxkWorkCenterMachineID", "smallint", 3, 0, nullable: false),
				new DmoField("sxkLinkedTaskID", "int", 4, 0, nullable: false),
				new DmoField("sxkCurrentTaskDateType", "tinyint", 1, 0, nullable: false),
				new DmoField("sxkLinkedTaskDateType", "tinyint", 1, 0, nullable: false),
				new DmoField("sxkOffsetTime", "numeric", 8, 2, nullable: false),
				new DmoField("sxkExchangeID", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("sxkStartDate", "datetime", 14, 0, nullable: true),
				new DmoField("sxkEndDate", "datetime", 14, 0, nullable: true),
				new DmoField("sxkCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("sxkCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("sxkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("sxkScheduleTreeID,sxkScheduleBranchID,sxkScheduleTaskID", unique: true),
				new DmoIndex("sxkUniqueID", unique: true)
			});
		}
	}
}
