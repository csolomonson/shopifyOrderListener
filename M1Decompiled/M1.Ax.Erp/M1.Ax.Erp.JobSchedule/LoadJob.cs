using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.JobSchedule;

public class LoadJob : LoadSchedule
{
	public override ScheduleTree Load(M1Database database, object[] sourceKeyValues, ScheduleCache cache)
	{
		string text = sourceKeyValues[0].ToString();
		SqlCommand sqlCommand = database.NewSqlCommand("Select jmpUniqueID From Jobs Where jmpJobID = @JobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = text;
		Guid value = (Guid)database.ExecuteScalar(sqlCommand);
		sqlCommand = database.NewSqlCommand("Select jmaJobAssemblyID,jmaParentAssemblyID,jmaOverlapSourceOperationID,jmaOverlapOperationID,jmaOverlapSourceLink,jmaOverlapDestinationLink,jmaOverlapOffsetTime,jmaAssemblyOverlap From JobAssemblies Where jmaJobID = @JobID Order By jmaJobAssemblyID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = text;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			sqlCommand = database.NewSqlCommand("Select jmoJobID,jmoJobAssemblyID,jmoJobOperationID,jmoOperationType,jmoPlantID,jmoPlantDepartmentID,jmoProcessID,jmoOverlapOperationID,jmoOverlapSourceLink,jmoOverlapDestinationLink,jmoOverlapOffsetTime,jmoWorkCenterID,jmoQueueTime,jmoMoveTime,jmoCompletedSetupHours,jmoCompletedProductionHours,jmoSetupHours,jmoEstimatedProductionHours,jmoMachineType,jmoMachinesToSchedule,jmoWorkCenterMachineID,jmoOperationQuantity,jmoSetupComplete,jmoProductionComplete,xawUniqueID From JobOperations Inner Join WorkCenters On jmoWorkCenterID = xawWorkCenterID Where jmoJobID = @JobID Order By jmoJobAssemblyID,jmoJobOperationID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = text;
			DataTable dataTable2 = database.GetDataTable(sqlCommand);
			ScheduleTree scheduleTree = new ScheduleTree(database.User.ID);
			scheduleTree.Description = "Job " + text;
			scheduleTree.SourceKeyValues = sourceKeyValues;
			scheduleTree.SourceTable = "Jobs";
			scheduleTree.SourceUniqueID = value;
			scheduleTree.GroupUniqueID = value;
			scheduleTree.SourceAssembliesTable = "JobAssemblies";
			scheduleTree.ScheduleType = 1;
			foreach (DataRow row3 in dataTable.Rows)
			{
				scheduleTree.AllBranches.Add(new ScheduleBranch(scheduleTree, row3.Field<int>("jmaJobAssemblyID"), row3.Field<int>("jmaParentAssemblyID"), row3.Field<int>("jmaOverlapSourceOperationID"), row3.Field<byte>("jmaOverlapSourceLink"), row3.Field<int>("jmaOverlapOperationID"), row3.Field<byte>("jmaOverlapDestinationLink"), Convert.ToInt32(row3.Field<decimal>("jmaOverlapOffsetTime") * 60.0m), row3.Field<BranchOverlap>("jmaAssemblyOverlap"), database.User.ID));
			}
			foreach (DataRow row4 in dataTable2.Rows)
			{
				scheduleTree.AllTasks.Add(LoadOperation(database, row4, scheduleTree, cache));
			}
			ScheduleBranch scheduleBranch = scheduleTree.AllBranches.Find((ScheduleBranch item) => item.BranchID == 0);
			if (scheduleBranch != null)
			{
				LoadAssembly(scheduleTree, scheduleBranch, cache);
			}
			ScheduleProcess.MakeSureScheduleIsInitialized(database, cache, scheduleTree);
			return scheduleTree;
		}
		return null;
	}

	private int getCurrentTreeID(M1Database database, Guid sourceJobGuid)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull((Select IsNull(sxtScheduleTreeID,0) As sxtScheduleTreeID From ScheduleTrees Where sxtSourceTable = 'Jobs' And sxtSourceUniqueID = @Guid), 0)");
		sqlCommand.Parameters.Add(new SqlParameter("@Guid", SqlDbType.UniqueIdentifier)).Value = sourceJobGuid;
		return (int)database.ExecuteScalar(sqlCommand);
	}

	private ScheduleTask LoadOperation(M1Database database, DataRow row, ScheduleTree source, ScheduleCache cache)
	{
		ScheduleTask scheduleTask = new ScheduleTask();
		scheduleTask.Source = source;
		scheduleTask.BranchID = row.Field<int>("jmoJobAssemblyID");
		scheduleTask.TaskID = row.Field<int>("jmoJobOperationID");
		scheduleTask.PlantID = row.Field<string>("jmoPlantID");
		scheduleTask.PlantDepartmentID = row.Field<string>("jmoPlantDepartmentID");
		scheduleTask.ProcessID = row.Field<string>("jmoProcessID");
		WorkProcess proc = cache.Processes[scheduleTask.ProcessID];
		scheduleTask.SetBuckets(cache, proc);
		scheduleTask.OverlapTaskID = row.Field<int>("jmoOverlapOperationID");
		scheduleTask.OverlapSourceLink = row.Field<byte>("jmoOverlapSourceLink");
		scheduleTask.OverlapDestinationLink = row.Field<byte>("jmoOverlapDestinationLink");
		scheduleTask.OverlapOffsetMinutes = Convert.ToInt32(row.Field<decimal>("jmoOverlapOffsetTime") * 60.0m);
		scheduleTask.PlantCalendar = cache.PlantCalendars[row.Field<string>("jmoPlantID")];
		IResourceGroup resourceGroup = ScheduleProcess.GetResourceGroup(database, cache, ScheduleProcess.ResourceTypes.WorkCenters, row.Field<Guid>("xawUniqueID"));
		short num = 0;
		Guid? lockedResourceUniqueID = null;
		if (row.Field<byte>("jmoMachineType") == 2)
		{
			num = (short)resourceGroup.ResourceGuids.Count;
		}
		else if (row.Field<byte>("jmoMachineType") == 1)
		{
			num = row.Field<short>("jmoMachinesToSchedule");
		}
		if (num < 1)
		{
			num = 1;
		}
		if (row.Field<byte>("jmoMachineType") == 3)
		{
			short num2 = row.Field<short>("jmoWorkCenterMachineID");
			if (num2 > 0 && resourceGroup.ResourceGuids.Count >= num2)
			{
				lockedResourceUniqueID = resourceGroup.ResourceGuids[num2 - 1];
			}
		}
		short peoplePerMachineSetup = resourceGroup.PeoplePerMachineSetup;
		short peoplePerMachineProduction = resourceGroup.PeoplePerMachineProduction;
		ScheduleTaskBucket scheduleTaskBucket = scheduleTask.Buckets[ScheduleType.QueueStart];
		ScheduleTaskBucket scheduleTaskBucket2 = scheduleTask.Buckets[ScheduleType.SetupStart];
		ScheduleTaskBucket scheduleTaskBucket3 = scheduleTask.Buckets[ScheduleType.ProductionStart];
		ScheduleTaskBucket scheduleTaskBucket4 = scheduleTask.Buckets[ScheduleType.ProductionEnd];
		_ = scheduleTask.Buckets[ScheduleType.MoveEnd];
		scheduleTaskBucket2.Minutes = (int)(row.Field<decimal>("jmoSetupHours") * 60.0m);
		scheduleTaskBucket2.UnsqueezedCompletedMinutes = Convert.ToInt32(row.Field<decimal>("jmoCompletedSetupHours") * 60.0m);
		scheduleTaskBucket2.Completed = row.Field<bool>("jmoSetupComplete");
		scheduleTaskBucket3.Minutes = (int)(row.Field<decimal>("jmoEstimatedProductionHours") * 60.0m);
		if (num > 0)
		{
			scheduleTaskBucket3.Minutes = (int)M1Math.Round((decimal)scheduleTaskBucket3.Minutes / (decimal)num, 0);
		}
		scheduleTaskBucket3.UnsqueezedCompletedMinutes = Convert.ToInt32(row.Field<decimal>("jmoCompletedProductionHours") * 60.0m);
		scheduleTaskBucket3.Completed = row.Field<bool>("jmoProductionComplete");
		if (scheduleTaskBucket2.UnsqueezedCompletedMinutes > 0 || scheduleTaskBucket3.UnsqueezedCompletedMinutes > 0)
		{
			scheduleTaskBucket.Completed = true;
		}
		if (row.Field<decimal>("jmoOperationQuantity") == 0m)
		{
			scheduleTaskBucket.Minutes = 0;
			scheduleTaskBucket4.Minutes = 0;
			scheduleTaskBucket2.Minutes = 0;
			scheduleTaskBucket3.Minutes = 0;
			scheduleTask.TotalRemainingHours = default(decimal);
		}
		else
		{
			scheduleTaskBucket.Minutes = (int)(row.Field<decimal>("jmoQueueTime") * 60.0m);
			scheduleTaskBucket4.Minutes = (int)(row.Field<decimal>("jmoMoveTime") * 60.0m);
			if (scheduleTaskBucket2.UnsqueezedCompletedMinutes > 0 || scheduleTaskBucket2.Completed || scheduleTaskBucket3.UnsqueezedCompletedMinutes > 0 || scheduleTaskBucket3.Completed)
			{
				scheduleTask.TotalRemainingHours = default(decimal);
			}
			else
			{
				scheduleTask.TotalRemainingHours = scheduleTaskBucket.Minutes;
			}
			scheduleTaskBucket2.CompletedMinutes = scheduleTaskBucket2.UnsqueezedCompletedMinutes;
			scheduleTaskBucket3.CompletedMinutes = scheduleTaskBucket3.UnsqueezedCompletedMinutes;
		}
		if (!scheduleTask.UniqueID.HasValue || scheduleTask.UniqueID.Value == Guid.Empty)
		{
			scheduleTask.UniqueID = Guid.NewGuid();
		}
		if (string.IsNullOrWhiteSpace(scheduleTask.CreatedBy))
		{
			scheduleTask.CreatedBy = database.User.ID;
		}
		if (!scheduleTask.CreatedDate.HasValue)
		{
			scheduleTask.CreatedDate = DateTime.Now;
		}
		scheduleTask.SetResources(num, row.Field<Guid>("xawUniqueID"), ScheduleProcess.ResourceTypes.WorkCenters);
		if (lockedResourceUniqueID.HasValue)
		{
			List<KeyValuePair<short, ResourceLane>> list = scheduleTask.ResourceLanes.Where((KeyValuePair<short, ResourceLane> item) => item.Value.ResourceType == ScheduleProcess.ResourceTypes.WorkCenters).ToList();
			if (list.Count == 1)
			{
				list[0].Value.LockedResourceUniqueID = lockedResourceUniqueID;
			}
		}
		if (row.Field<byte>("jmoOperationType") != 2)
		{
			scheduleTask.SetResources((short)(Math.Max(peoplePerMachineSetup, peoplePerMachineProduction) * num), null, ScheduleProcess.ResourceTypes.Shifts);
		}
		return scheduleTask;
	}
}
