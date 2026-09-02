using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.074", "Add Schedule tables", "2016-06-09")]
public class v91074
{
	public v91074(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleDates"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleDates");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleDates", new DmoField[16]
		{
			new DmoField("sxdScheduleTreeID", "int", 4, 0, nullable: false),
			new DmoField("sxdScheduleBranchID", "int", 4, 0, nullable: false),
			new DmoField("sxdScheduleTaskID", "int", 4, 0, nullable: false),
			new DmoField("sxdResourceLane", "smallint", 4, 0, nullable: false),
			new DmoField("sxdScheduleDateID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxdDateType", "tinyint", 1, 0, nullable: false),
			new DmoField("sxdStartDate", "date", 14, 0, nullable: true),
			new DmoField("sxdStartHour", "numeric", 5, 2, nullable: false),
			new DmoField("sxdStartActualDateTime", "datetime", 14, 0, nullable: true),
			new DmoField("sxdDuration", "numeric", 8, 2, nullable: false),
			new DmoField("sxdEndDate", "date", 14, 0, nullable: true),
			new DmoField("sxdEndHour", "numeric", 5, 2, nullable: false),
			new DmoField("sxdEndActualDateTime", "datetime", 14, 0, nullable: true),
			new DmoField("sxdResourceUniqueID", "uniqueidentifier", 16, 0, nullable: true),
			new DmoField("sxdGroupUniqueID", "uniqueidentifier", 16, 0, nullable: true),
			new DmoField("sxdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("sxdScheduleTreeID,sxdScheduleBranchID,sxdScheduleTaskID,sxdResourceLane,sxdScheduleDateID", unique: true),
			new DmoIndex("sxdUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleResourceLanes"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleResourceLanes");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleResourceLanes", new DmoField[10]
		{
			new DmoField("sxrScheduleTreeID", "int", 4, 0, nullable: false),
			new DmoField("sxrScheduleBranchID", "int", 4, 0, nullable: false),
			new DmoField("sxrScheduleTaskID", "int", 4, 0, nullable: false),
			new DmoField("sxrScheduleResourceLaneID", "smallint", 4, 0, nullable: false),
			new DmoField("sxrResourceType", "tinyint", 1, 0, nullable: false),
			new DmoField("sxrResourceTable", "nvarchar", 30, 0, nullable: false),
			new DmoField("sxrLockedResourceUniqueID", "uniqueidentifier", 16, 0, nullable: true),
			new DmoField("sxrGroupTable", "nvarchar", 30, 0, nullable: false),
			new DmoField("sxrGroupUniqueID", "uniqueidentifier", 16, 0, nullable: true),
			new DmoField("sxrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("sxrScheduleTreeID,sxrScheduleBranchID,sxrScheduleTaskID,sxrScheduleResourceLaneID", unique: true),
			new DmoIndex("sxrUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTypes"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleTypes");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTypes", new DmoField[7]
		{
			new DmoField("sxyScheduleTypeID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxyDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("sxyLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("sxyLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("sxyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("sxyCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxyUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("sxyScheduleTypeID", unique: true),
			new DmoIndex("sxyUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTypeBuckets"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleTypeBuckets");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTypeBuckets", new DmoField[6]
		{
			new DmoField("sxuScheduleTypeID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxuScheduleTypeBucketID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxuDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("sxuCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("sxuCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxuUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("sxuScheduleTypeID,sxuScheduleTypeBucketID", unique: true),
			new DmoIndex("sxuUniqueID", unique: true)
		});
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleTasks"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleTasks");
		}
		parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTasks", new DmoField[19]
		{
			new DmoField("sxkScheduleTreeID", "int", 4, 0, nullable: false),
			new DmoField("sxkScheduleBranchID", "int", 4, 0, nullable: false),
			new DmoField("sxkScheduleTaskID", "int", 4, 0, nullable: false),
			new DmoField("sxkLinkedTaskID", "int", 4, 0, nullable: false),
			new DmoField("sxkCurrentTaskDateType", "tinyint", 1, 0, nullable: false),
			new DmoField("sxkLinkedTaskDateType", "tinyint", 1, 0, nullable: false),
			new DmoField("sxkOffsetTime", "numeric", 8, 2, nullable: false),
			new DmoField("sxkScheduleTypeID", "tinyint", 1, 0, nullable: false),
			new DmoField("sxkStartDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxkStartHour", "numeric", 5, 2, nullable: false),
			new DmoField("sxkStartActualDateTime", "datetime", 14, 0, nullable: true),
			new DmoField("sxkDuration", "numeric", 8, 2, nullable: false),
			new DmoField("sxkEndDate", "datetime", 14, 0, nullable: true),
			new DmoField("sxkEndHour", "numeric", 5, 2, nullable: false),
			new DmoField("sxkEndActualDateTime", "datetime", 14, 0, nullable: true),
			new DmoField("sxkExchangeID", "nvarchar(max)", 50, 0, nullable: true),
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
