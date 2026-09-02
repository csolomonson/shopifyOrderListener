using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.JobSchedule;

public static class ScheduleProcess
{
	private class TaskCoreHours
	{
		public decimal TotalMachineHours;

		public ScheduleTaskBucket FirstMachineBucket;

		public ScheduleTaskBucket LastMachineBucket;
	}

	private class BucketStartParameter
	{
		public ScheduleTaskBucket Bucket;

		public ScheduleDate InitialDate;

		public M1Database Database;

		public ResourceCalendarDefinition Calendar;

		public ScheduleDirection Direction;

		public BucketStartParameter(M1Database database, ResourceCalendarDefinition calendar, ScheduleDirection direction, ScheduleTaskBucket bucket, ScheduleDate initialDate)
		{
			Database = database;
			Calendar = calendar;
			Direction = direction;
			Bucket = bucket;
			InitialDate = initialDate;
		}
	}

	private class LaneGroup
	{
		public Dictionary<byte, LaneProcess> GroupsOneResourcePerTask;

		public Dictionary<byte, LaneProcess> GroupsMultipleResourcesPerTask;
	}

	private class LaneProcess
	{
		public byte ResourceType;

		public Dictionary<Guid, LaneInfo> GroupIDLanes = new Dictionary<Guid, LaneInfo>();

		public List<ResourceLane> EmptyGroupIDLanes = new List<ResourceLane>();
	}

	private class LaneInfo
	{
		public List<ResourceLane> Lanes = new List<ResourceLane>();

		public List<Guid> LockedLanes = new List<Guid>();
	}

	public class LocalScheduleData
	{
		public List<ScheduleTask> TasksToSchedule;

		public bool IgnoreOtherJobsForMachines;

		public bool IgnoreOtherJobsForEmployees;

		public List<string> Messages;

		public LocalScheduleData(List<ScheduleTask> tasksToSchedule, bool ignoreOtherJobsForMachines, bool ignoreOtherJobsForEmployees)
		{
			Messages = new List<string>();
			TasksToSchedule = tasksToSchedule;
			IgnoreOtherJobsForMachines = ignoreOtherJobsForMachines;
			IgnoreOtherJobsForEmployees = ignoreOtherJobsForEmployees;
		}
	}

	private class Overlaps
	{
		public List<ScheduleAllocation> AllOverlaps;

		public List<Guid> DistinctResources;
	}

	public class GetCheckRange : IGetWorkingDaysService
	{
		IGetWorkingDays IGetWorkingDaysService.GetWorkingDaysService(M1Database database, string plantID)
		{
			return new CheckRange(database, plantID);
		}
	}

	public class CheckRange : IGetWorkingDays, IDisposable
	{
		private string _PlantID;

		private M1Database _Database;

		private ScheduleCache _Cache;

		private ResourceCalendarDefinition _Calendar;

		public CheckRange(M1Database database, string plantID)
		{
			_PlantID = plantID;
			_Database = database;
			_Cache = new ScheduleCache();
			LoadPlants(_Cache, database, plantID);
			_Calendar = _Cache.PlantCalendars[plantID];
		}

		public Dictionary<DateTime, StartTimeAndHours> GetWorkingDaysInRange(DateTime startDate, DateTime endDate)
		{
			return ScheduleProcess.GetWorkingDaysInRange(_Database, _Cache, _Calendar, startDate, endDate);
		}

		public List<DateTime> GetNonWorkingDaysInRange(DateTime startDate, DateTime endDate)
		{
			return ScheduleProcess.GetNonWorkingDaysInRange(_Database, _Cache, _Calendar, startDate, endDate);
		}

		public void Dispose()
		{
			_Database = null;
			if (_Cache != null)
			{
				_Cache.Dispose();
				_Cache = null;
			}
			if (_Calendar != null)
			{
				_Calendar.Dispose();
				_Calendar = null;
			}
		}
	}

	public static class ResourceTypes
	{
		public static byte WorkCenters = 1;

		public static byte Shifts = 2;
	}

	private const int EntireDayMinutes = 1440;

	private static bool? _useStrictOverlapCheck;

	public static void CopyScheduleToScenario(M1Database database, string sourceScenarioID, string destScenarioID, SqlTransaction transaction)
	{
		bool flag = transaction != null;
		if (transaction == null)
		{
			transaction = database.BeginTransaction();
		}
		try
		{
			DeleteScheduleForScenario(database, destScenarioID, transaction);
			SqlCommand sqlCommand = database.NewSqlCommand("Set NoCount On\r\nSelect Identity(int, " + Convert.ToInt32(database.ExecuteScalar("Select IsNull(Max(sxtScheduleTreeID),0) + 1 As NextNo From ScheduleTrees", transaction)) + ", 1) As TempID, sxtScheduleTreeID Into #TreeMatcher From ScheduleTrees Where sxtJobScenarioID = @SourceScenarioID\r\nCREATE UNIQUE CLUSTERED INDEX myJoinIndex ON #TreeMatcher (sxtScheduleTreeID)\r\nSelect ScheduleTrees.sxtSourceTable,ScheduleTrees.sxtSourceUniqueID,ScheduleTrees.sxtCreatedBy,ScheduleTrees.sxtCreatedDate,ScheduleTrees.sxtUniqueID,ScheduleTrees.sxtType,ScheduleTrees.sxtDescription,ScheduleTrees.sxtGroupUniqueID,ScheduleTrees.sxtScheduleTreeID,ScheduleTrees.sxtJobScenarioID Into #ScheduleTrees From ScheduleTrees Inner Join #TreeMatcher On #TreeMatcher.sxtScheduleTreeID = ScheduleTrees.sxtScheduleTreeID\r\nUpdate #ScheduleTrees Set #ScheduleTrees.sxtScheduleTreeID = #TreeMatcher.TempID,#ScheduleTrees.sxtType=CASE WHEN ltrim(rtrim(@DestScenarioID))='' THEN 1 ELSE 0 END , #ScheduleTrees.sxtUniqueID = NewID(),#ScheduleTrees.sxtJobScenarioID = @DestScenarioID From #ScheduleTrees Inner Join #TreeMatcher On #ScheduleTrees.sxtScheduleTreeID = #TreeMatcher.sxtScheduleTreeID\r\nInsert Into ScheduleTrees (sxtSourceTable,sxtSourceUniqueID,sxtCreatedBy,sxtCreatedDate,sxtUniqueID,sxtType,sxtDescription,sxtGroupUniqueID,sxtScheduleTreeID,sxtJobScenarioID) Select sxtSourceTable,sxtSourceUniqueID,sxtCreatedBy,sxtCreatedDate,sxtUniqueID,sxtType,sxtDescription,sxtGroupUniqueID,sxtScheduleTreeID,sxtJobScenarioID From #ScheduleTrees\r\nDrop Table #ScheduleTrees\r\nSelect ScheduleBranches.sxbScheduleTreeID,ScheduleBranches.sxbScheduleBranchID,ScheduleBranches.sxbParentScheduleBranchID,ScheduleBranches.sxbSiblingBranchLink,ScheduleBranches.sxbParentLinkedTaskID,ScheduleBranches.sxbCurrentLinkedTaskID,ScheduleBranches.sxbCurrentLinkedTaskDateType,ScheduleBranches.sxbParentLinkedTaskDateType,ScheduleBranches.sxbCreatedBy,ScheduleBranches.sxbCreatedDate,ScheduleBranches.sxbUniqueID,ScheduleBranches.sxbOffsetMinutes Into #ScheduleBranches From ScheduleBranches Inner Join #TreeMatcher On #TreeMatcher.sxtScheduleTreeID = sxbScheduleTreeID\r\nUpdate #ScheduleBranches Set #ScheduleBranches.sxbScheduleTreeID = #TreeMatcher.TempID, #ScheduleBranches.sxbUniqueID = NewID() From #ScheduleBranches Inner Join #TreeMatcher On #ScheduleBranches.sxbScheduleTreeID = #TreeMatcher.sxtScheduleTreeID\r\nInsert Into ScheduleBranches (sxbScheduleTreeID,sxbScheduleBranchID,sxbParentScheduleBranchID,sxbSiblingBranchLink,sxbParentLinkedTaskID,sxbCurrentLinkedTaskID,sxbCurrentLinkedTaskDateType,sxbParentLinkedTaskDateType,sxbCreatedBy,sxbCreatedDate,sxbUniqueID,sxbOffsetMinutes) Select sxbScheduleTreeID,sxbScheduleBranchID,sxbParentScheduleBranchID,sxbSiblingBranchLink,sxbParentLinkedTaskID,sxbCurrentLinkedTaskID,sxbCurrentLinkedTaskDateType,sxbParentLinkedTaskDateType,sxbCreatedBy,sxbCreatedDate,sxbUniqueID,sxbOffsetMinutes From #ScheduleBranches\r\nDrop Table #ScheduleBranches\r\nSelect ScheduleTasks.sxkScheduleTreeID,ScheduleTasks.sxkScheduleBranchID,ScheduleTasks.sxkScheduleTaskID,ScheduleTasks.sxkPlantID,ScheduleTasks.sxkPlantDepartmentID,ScheduleTasks.sxkProcessID,ScheduleTasks.sxkLinkedTaskID,ScheduleTasks.sxkCurrentTaskDateType,ScheduleTasks.sxkLinkedTaskDateType,ScheduleTasks.sxkScheduleTypeID,ScheduleTasks.sxkStartActualDateTime,ScheduleTasks.sxkEndActualDateTime,ScheduleTasks.sxkExchangeID,ScheduleTasks.sxkCreatedBy,ScheduleTasks.sxkCreatedDate,ScheduleTasks.sxkUniqueID,ScheduleTasks.sxkStartDate,ScheduleTasks.sxkEndMinute,ScheduleTasks.sxkEndDate,ScheduleTasks.sxkStartMinute,ScheduleTasks.sxkMinutes,ScheduleTasks.sxkOffsetMinutes Into #ScheduleTasks From ScheduleTasks Inner Join #TreeMatcher On #TreeMatcher.sxtScheduleTreeID = sxkScheduleTreeID\r\nUpdate #ScheduleTasks Set #ScheduleTasks.sxkScheduleTreeID = #TreeMatcher.TempID, #ScheduleTasks.sxkUniqueID = NewID() From #ScheduleTasks Inner Join #TreeMatcher On #ScheduleTasks.sxkScheduleTreeID = #TreeMatcher.sxtScheduleTreeID\r\nInsert Into ScheduleTasks (sxkScheduleTreeID,sxkScheduleBranchID,sxkScheduleTaskID,sxkPlantID,sxkPlantDepartmentID,sxkProcessID,sxkLinkedTaskID,sxkCurrentTaskDateType,sxkLinkedTaskDateType,sxkScheduleTypeID,sxkStartActualDateTime,sxkEndActualDateTime,sxkExchangeID,sxkCreatedBy,sxkCreatedDate,sxkUniqueID,sxkStartDate,sxkEndMinute,sxkEndDate,sxkStartMinute,sxkMinutes,sxkOffsetMinutes) Select sxkScheduleTreeID,sxkScheduleBranchID,sxkScheduleTaskID,sxkPlantID,sxkPlantDepartmentID,sxkProcessID,sxkLinkedTaskID,sxkCurrentTaskDateType,sxkLinkedTaskDateType,sxkScheduleTypeID,sxkStartActualDateTime,sxkEndActualDateTime,sxkExchangeID,sxkCreatedBy,sxkCreatedDate,sxkUniqueID,sxkStartDate,sxkEndMinute,sxkEndDate,sxkStartMinute,sxkMinutes,sxkOffsetMinutes From #ScheduleTasks\r\nDrop Table #ScheduleTasks\r\nSelect ScheduleAllocations.sxdScheduleTreeID,ScheduleAllocations.sxdScheduleBranchID,ScheduleAllocations.sxdScheduleTaskID,ScheduleAllocations.sxdScheduleResourceLaneID,ScheduleAllocations.sxdScheduleAllocationID,ScheduleAllocations.sxdDateType,ScheduleAllocations.sxdStartActualDateTime,ScheduleAllocations.sxdEndActualDateTime,ScheduleAllocations.sxdResourceUniqueID,ScheduleAllocations.sxdGroupUniqueID,ScheduleAllocations.sxdUniqueID,ScheduleAllocations.sxdMinutes,ScheduleAllocations.sxdStartMinute,ScheduleAllocations.sxdEndMinute,ScheduleAllocations.sxdEndDate,ScheduleAllocations.sxdStartDate Into #ScheduleAllocations From ScheduleAllocations Inner Join #TreeMatcher On #TreeMatcher.sxtScheduleTreeID = sxdScheduleTreeID\r\nUpdate #ScheduleAllocations Set #ScheduleAllocations.sxdScheduleTreeID = #TreeMatcher.TempID, #ScheduleAllocations.sxdUniqueID = NewID() From #ScheduleAllocations Inner Join #TreeMatcher On #ScheduleAllocations.sxdScheduleTreeID = #TreeMatcher.sxtScheduleTreeID\r\nInsert Into ScheduleAllocations (sxdScheduleTreeID,sxdScheduleBranchID,sxdScheduleTaskID,sxdScheduleResourceLaneID,sxdScheduleAllocationID,sxdDateType,sxdStartActualDateTime,sxdEndActualDateTime,sxdResourceUniqueID,sxdGroupUniqueID,sxdUniqueID,sxdMinutes,sxdStartMinute,sxdEndMinute,sxdEndDate,sxdStartDate) Select sxdScheduleTreeID,sxdScheduleBranchID,sxdScheduleTaskID,sxdScheduleResourceLaneID,sxdScheduleAllocationID,sxdDateType,sxdStartActualDateTime,sxdEndActualDateTime,sxdResourceUniqueID,sxdGroupUniqueID,sxdUniqueID,sxdMinutes,sxdStartMinute,sxdEndMinute,sxdEndDate,sxdStartDate From #ScheduleAllocations\r\nDrop Table #ScheduleAllocations\r\nSelect ScheduleResourceLanes.sxrScheduleTreeID,ScheduleResourceLanes.sxrScheduleBranchID,ScheduleResourceLanes.sxrScheduleTaskID,ScheduleResourceLanes.sxrScheduleResourceLaneID,ScheduleResourceLanes.sxrLockedResourceUniqueID,ScheduleResourceLanes.sxrGroupUniqueID,ScheduleResourceLanes.sxrUniqueID,ScheduleResourceLanes.sxrResourceType Into #ScheduleResourceLanes From ScheduleResourceLanes Inner Join #TreeMatcher On #TreeMatcher.sxtScheduleTreeID = sxrScheduleTreeID\r\nUpdate #ScheduleResourceLanes Set #ScheduleResourceLanes.sxrScheduleTreeID = #TreeMatcher.TempID, #ScheduleResourceLanes.sxrUniqueID = NewID() From #ScheduleResourceLanes Inner Join #TreeMatcher On #ScheduleResourceLanes.sxrScheduleTreeID = #TreeMatcher.sxtScheduleTreeID\r\nInsert Into ScheduleResourceLanes (sxrScheduleTreeID,sxrScheduleBranchID,sxrScheduleTaskID,sxrScheduleResourceLaneID,sxrLockedResourceUniqueID,sxrGroupUniqueID,sxrUniqueID,sxrResourceType) Select sxrScheduleTreeID,sxrScheduleBranchID,sxrScheduleTaskID,sxrScheduleResourceLaneID,sxrLockedResourceUniqueID,sxrGroupUniqueID,sxrUniqueID,sxrResourceType From #ScheduleResourceLanes\r\nDrop Table #ScheduleResourceLanes\r\nSelect ScheduleResourceCells.sxcTreeID,ScheduleResourceCells.sxcBranchID,ScheduleResourceCells.sxcTaskID,ScheduleResourceCells.sxcResourceLaneID,ScheduleResourceCells.sxcResourceCellID,ScheduleResourceCells.sxcResourceUniqueID,ScheduleResourceCells.sxcUniqueID Into #ScheduleResourceCells From ScheduleResourceCells Inner Join #TreeMatcher On #TreeMatcher.sxtScheduleTreeID = sxcTreeID\r\nUpdate #ScheduleResourceCells Set #ScheduleResourceCells.sxcTreeID = #TreeMatcher.TempID, #ScheduleResourceCells.sxcUniqueID = NewID() From #ScheduleResourceCells Inner Join #TreeMatcher On #ScheduleResourceCells.sxcTreeID = #TreeMatcher.sxtScheduleTreeID\r\nInsert Into ScheduleResourceCells (sxcTreeID,sxcBranchID,sxcTaskID,sxcResourceLaneID,sxcResourceCellID,sxcResourceUniqueID,sxcUniqueID) Select sxcTreeID,sxcBranchID,sxcTaskID,sxcResourceLaneID,sxcResourceCellID,sxcResourceUniqueID,sxcUniqueID From #ScheduleResourceCells\r\nDrop Table #ScheduleResourceCells\r\nSelect ScheduleTaskBuckets.sxeScheduleTreeID,ScheduleTaskBuckets.sxeScheduleBranchID,ScheduleTaskBuckets.sxeScheduleTaskID,ScheduleTaskBuckets.sxeScheduleTaskBucketID,ScheduleTaskBuckets.sxeScheduleTypeID,ScheduleTaskBuckets.sxeScheduleTypeBucketID,ScheduleTaskBuckets.sxeCompleted,ScheduleTaskBuckets.sxeUniqueID,ScheduleTaskBuckets.sxeCompletedMinutes,ScheduleTaskBuckets.sxeMinutes,ScheduleTaskBuckets.sxePercentComplete Into #ScheduleTaskBuckets From ScheduleTaskBuckets Inner Join #TreeMatcher On #TreeMatcher.sxtScheduleTreeID = sxeScheduleTreeID\r\nUpdate #ScheduleTaskBuckets Set #ScheduleTaskBuckets.sxeScheduleTreeID = #TreeMatcher.TempID, #ScheduleTaskBuckets.sxeUniqueID = NewID() From #ScheduleTaskBuckets Inner Join #TreeMatcher On #ScheduleTaskBuckets.sxeScheduleTreeID = #TreeMatcher.sxtScheduleTreeID\r\nInsert Into ScheduleTaskBuckets (sxeScheduleTreeID,sxeScheduleBranchID,sxeScheduleTaskID,sxeScheduleTaskBucketID,sxeScheduleTypeID,sxeScheduleTypeBucketID,sxeCompleted,sxeUniqueID,sxeCompletedMinutes,sxeMinutes,sxePercentComplete) Select sxeScheduleTreeID,sxeScheduleBranchID,sxeScheduleTaskID,sxeScheduleTaskBucketID,sxeScheduleTypeID,sxeScheduleTypeBucketID,sxeCompleted,sxeUniqueID,sxeCompletedMinutes,sxeMinutes,sxePercentComplete From #ScheduleTaskBuckets\r\nDrop Table #ScheduleTaskBuckets\r\nDrop Table #TreeMatcher\r\nSet NoCount Off\r\n");
			sqlCommand.Parameters.Add(new SqlParameter("@SourceScenarioID", SqlDbType.NVarChar)).Value = sourceScenarioID;
			sqlCommand.Parameters.Add(new SqlParameter("@DestScenarioID", SqlDbType.NVarChar)).Value = destScenarioID;
			database.ExecuteCommand(sqlCommand, transaction);
			if (!flag)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (!flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	public static void DeleteScheduleForScenario(M1Database database, string scenarioID, SqlTransaction transaction)
	{
		bool flag = transaction != null;
		if (transaction == null)
		{
			transaction = database.BeginTransaction();
		}
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand("");
			sqlCommand.Parameters.Add(new SqlParameter("@ScenarioID", SqlDbType.NVarChar)).Value = scenarioID;
			sqlCommand.CommandText = "Delete ScheduleTaskBuckets From ScheduleTaskBuckets Inner Join ScheduleTrees On sxeScheduleTreeID = sxtScheduleTreeID Where sxtJobScenarioID = @ScenarioID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete ScheduleResourceCells From ScheduleResourceCells Inner Join ScheduleTrees On sxcTreeID = sxtScheduleTreeID Where sxtJobScenarioID = @ScenarioID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete ScheduleResourceLanes From ScheduleResourceLanes Inner Join ScheduleTrees On sxrScheduleTreeID = sxtScheduleTreeID Where sxtJobScenarioID = @ScenarioID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete ScheduleAllocations From ScheduleAllocations Inner Join ScheduleTrees On sxdScheduleTreeID = sxtScheduleTreeID Where sxtJobScenarioID = @ScenarioID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete ScheduleTasks From ScheduleTasks Inner Join ScheduleTrees On sxkScheduleTreeID = sxtScheduleTreeID Where sxtJobScenarioID = @ScenarioID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete ScheduleBranches From ScheduleBranches Inner Join ScheduleTrees On sxbScheduleTreeID = sxtScheduleTreeID Where sxtJobScenarioID = @ScenarioID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete From ScheduleTrees Where sxtJobScenarioID = @ScenarioID";
			database.ExecuteCommand(sqlCommand, transaction);
			if (!flag)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (!flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	public static void DeleteSchedule(M1Database database, string sourceTable, Guid sourceUniqueID, SqlTransaction transaction)
	{
		bool flag = transaction != null;
		if (transaction == null)
		{
			transaction = database.BeginTransaction();
		}
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand("");
			sqlCommand.Parameters.Add(new SqlParameter("@SourceTable", SqlDbType.NVarChar)).Value = sourceTable;
			sqlCommand.Parameters.Add(new SqlParameter("@SourceUniqueID", SqlDbType.UniqueIdentifier)).Value = sourceUniqueID;
			sqlCommand.CommandText = "Delete ScheduleTaskBuckets From ScheduleTaskBuckets Inner Join ScheduleTrees On sxeScheduleTreeID = sxtScheduleTreeID Where sxtSourceTable = @SourceTable And sxtSourceUniqueID = @SourceUniqueID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete ScheduleResourceCells From ScheduleResourceCells Inner Join ScheduleTrees On sxcTreeID = sxtScheduleTreeID Where sxtSourceTable = @SourceTable And sxtSourceUniqueID = @SourceUniqueID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete ScheduleResourceLanes From ScheduleResourceLanes Inner Join ScheduleTrees On sxrScheduleTreeID = sxtScheduleTreeID Where sxtSourceTable = @SourceTable And sxtSourceUniqueID = @SourceUniqueID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete ScheduleAllocations From ScheduleAllocations Inner Join ScheduleTrees On sxdScheduleTreeID = sxtScheduleTreeID Where sxtSourceTable = @SourceTable And sxtSourceUniqueID = @SourceUniqueID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete ScheduleTasks From ScheduleTasks Inner Join ScheduleTrees On sxkScheduleTreeID = sxtScheduleTreeID Where sxtSourceTable = @SourceTable And sxtSourceUniqueID = @SourceUniqueID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete ScheduleBranches From ScheduleBranches Inner Join ScheduleTrees On sxbScheduleTreeID = sxtScheduleTreeID Where sxtSourceTable = @SourceTable And sxtSourceUniqueID = @SourceUniqueID";
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand.CommandText = "Delete From ScheduleTrees Where sxtSourceTable = @SourceTable And sxtSourceUniqueID = @SourceUniqueID";
			database.ExecuteCommand(sqlCommand, transaction);
			if (!flag)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (!flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	public static LocalScheduleData StartSchedule(M1Database database, ScheduleCache cache, ScheduleParameters parameters, ScheduleTree job)
	{
		List<ScheduleTask> list = null;
		ResetSchedule(job);
		ScheduleTask initialOperation = GetInitialOperation(job, parameters);
		if (initialOperation == null)
		{
			throw new M1MissingOrInvalidDataException("Initial operation is required.");
		}
		M1.Core.AppContext appContext = database.GetService(typeof(M1.Core.AppContext)) as M1.Core.AppContext;
		if (!_useStrictOverlapCheck.HasValue)
		{
			_useStrictOverlapCheck = appContext?.Server.IniSettings.GetAsBool("UseStrictOverlapCheck", defaultValue: false);
		}
		if (!parameters.IncludeSubSequentOperations)
		{
			list = new List<ScheduleTask>();
			list.AddRange(IgnoreSubSequentOperationsForInitialAssembly(initialOperation));
		}
		if (!parameters.IncludePreviousOperations)
		{
			if (list == null)
			{
				list = new List<ScheduleTask>();
			}
			list.AddRange(IgnorePreviousOperationsForInitialAssembly(initialOperation));
		}
		List<ScheduleBranch> assembliesRange = GetAssembliesRange(job.AllBranches.Find((ScheduleBranch item) => item.BranchID == parameters.BaseAssemblyID), initialOperation.ParentBranch, parameters.AssemblyScope);
		List<ScheduleTask> operationsToSchedule = GetOperationsToSchedule(initialOperation, null, parameters.OperationScope, assembliesRange, list);
		foreach (ScheduleTask item in operationsToSchedule)
		{
			item.ClearDates();
		}
		byte initialDateType = GetInitialDateType(initialOperation, parameters);
		ScheduleDate date = NewDate(database, GetTaskCalendar(cache, initialOperation), parameters.InitialDate, (short)Math.Round(parameters.InitialHour * 60.0m));
		LocalScheduleData localScheduleData = new LocalScheduleData(operationsToSchedule, parameters.IgnoreOtherJobsForMachines, ignoreOtherJobsForEmployees: true);
		ScheduleOperationCollection(database, cache, initialOperation, initialDateType, null, date, parameters.OperationScope, assembliesRange, parameters.Direction, list, localScheduleData);
		SetStartEndOperations(job);
		if (job.ScheduledOrder.FirstOrDefault((ScheduleTask t) => t?.StartDate?.ActualDateTime?.Date < DateTime.Today) != null)
		{
			localScheduleData.Messages.Add("At least one of the tasks scheduled on " + job.Description + " needs to start before today.");
		}
		return localScheduleData;
	}

	private static List<ScheduleTask> IgnorePreviousOperationsForInitialAssembly(ScheduleTask initialOperation)
	{
		List<ScheduleTask> list = new List<ScheduleTask>();
		int initialOperationIndex = initialOperation.ParentBranch.Tasks.IndexOf(initialOperation);
		list.AddRange(initialOperation.ParentBranch.Tasks.Where((ScheduleTask item, int i) => i < initialOperationIndex));
		return list;
	}

	private static List<ScheduleTask> IgnoreSubSequentOperationsForInitialAssembly(ScheduleTask initialOperation)
	{
		List<ScheduleTask> list = new List<ScheduleTask>();
		int initialOperationIndex = initialOperation.ParentBranch.Tasks.IndexOf(initialOperation);
		list.AddRange(initialOperation.ParentBranch.Tasks.Where((ScheduleTask item, int i) => i > initialOperationIndex));
		return list;
	}

	public static void CopyTaskHours(ScheduleTree sourceTree, ScheduleTree destTree)
	{
		foreach (ScheduleTask sourceTask in sourceTree.AllTasks)
		{
			ScheduleTask scheduleTask = destTree.AllTasks.FirstOrDefault((ScheduleTask destItem) => destItem.BranchID == sourceTask.BranchID && destItem.TaskID == sourceTask.TaskID);
			if (scheduleTask == null)
			{
				continue;
			}
			foreach (KeyValuePair<byte, ScheduleTaskBucket> bucket in sourceTask.Buckets)
			{
				scheduleTask.Buckets[bucket.Key].Minutes = bucket.Value.Minutes;
				scheduleTask.Buckets[bucket.Key].CompletedMinutes = bucket.Value.CompletedMinutes;
				scheduleTask.Buckets[bucket.Key].Completed = bucket.Value.Completed;
				scheduleTask.Buckets[bucket.Key].PercentComplete = bucket.Value.PercentComplete;
			}
		}
	}

	public static IEnumerable<DataRow> GetJobIdsForScenario(M1Database database, SqlTransaction transaction, string currentScenarioID)
	{
		IEnumerable<DataRow> enumerable = null;
		string queryString = "SELECT Jobs.jmpJobID FROM ScheduleTrees \r\n                                INNER JOIN Jobs ON ScheduleTrees.sxtSourceUniqueID = Jobs.jmpUniqueID\r\n                                WHERE ScheduleTrees.sxtJobScenarioID =@JobScenarioID";
		using SqlCommand sqlCommand = database.NewSqlCommand(queryString);
		sqlCommand.Parameters.AddWithValue("@JobScenarioID", currentScenarioID);
		return database.GetDataTable(sqlCommand, transaction).AsEnumerable();
	}

	public static void ScheduleRefreshOpsAndMat(M1Database database, string jobID, int assemblyID)
	{
		ScheduleRefreshOpsAndMat(database, null, jobID, assemblyID);
		database.OnTableChanged(new TableChangedEventArgs("Jobs", null, null, null));
	}

	public static void ScheduleRefreshOpsAndMat(M1Database database, string jobID, int assemblyID, LocalScheduleData localScheduleData)
	{
		ScheduleRefreshOpsAndMat(database, null, jobID, assemblyID);
		using (SqlCommand sqlCommand = database.NewSqlCommand("SELECT @earliestMaterialDate = jmmOrderByDate FROM JobMaterials WHERE jmmJobID = @jobID AND jmmOrderByDate < CONVERT(date, GETDATE()) AND jmmPullAllFromStock = 0"))
		{
			sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.NVarChar)).Value = jobID;
			sqlCommand.Parameters.Add(new SqlParameter("@earliestMaterialDate", SqlDbType.DateTime)).Direction = ParameterDirection.Output;
			database.ExecuteCommand(sqlCommand, null);
			if (sqlCommand.Parameters["@earliestMaterialDate"].Value != DBNull.Value)
			{
				localScheduleData.Messages.Add("Material for Job " + jobID + " needs to be ordered before today.");
			}
		}
		database.OnTableChanged(new TableChangedEventArgs("Jobs", null, null, null));
	}

	public static void ScheduleRefreshOpsAndMat(M1Database database, SqlTransaction transaction, string jobID, int assemblyID)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		empty = new Job().GetAssembliesList(database, transaction, jobID, assemblyID);
		if (empty.Length > 0)
		{
			if (empty.IndexOf(',') == 0)
			{
				empty2 = empty2 + " And jmaJobAssemblyID = " + empty;
				empty3 = empty3 + " And jmmJobAssemblyID = " + empty;
				empty4 = empty4 + " And jmoJobAssemblyID = " + empty;
			}
			else
			{
				empty2 = empty2 + " And jmaJobAssemblyID in (" + empty + ")";
				empty3 = empty3 + " And jmmJobAssemblyID in (" + empty + ")";
				empty4 = empty4 + " And jmoJobAssemblyID in (" + empty + ")";
			}
			SqlCommand sqlCommand = database.NewSqlCommand("UPDATE JobOperations SET jmoStartDate = sxkStartDate, jmoStartHour = sxkStartMinute/60.0, jmoDueDate = sxkEndDate, jmoDueHour = sxkEndMinute/60.0 From Jobs Inner Join JobOperations on jmpJobID = jmoJobID Inner Join ScheduleTrees on jmpUniqueID = sxtSourceUniqueID Inner Join ScheduleBranches on sxtScheduleTreeID = sxbScheduleTreeID And sxbScheduleBranchID = jmoJobAssemblyID Inner Join ScheduleTasks on sxbScheduleTreeID = sxkScheduleTreeID And sxbScheduleBranchID = sxkScheduleBranchID And sxkScheduleTaskID = jmoJobOperationID Where jmpJobID = @jobID " + empty4);
			sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.NVarChar)).Value = jobID;
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("Update JobAssemblies Set jmaScheduledStartDate = isnull((Select Top 1 jmoStartDate From JobOperations Where jmoJobID = jmaJobID and jmoJobAssemblyID = jmaJobAssemblyID Order By jmoStartDate), (Select jmpproductionDueDate from jobs where jmpJobID = jmajobid)) Where jmaJobID = @jobID " + empty2);
			sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.NVarChar)).Value = jobID;
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("Update JobAssemblies Set jmaScheduledDueDate = isnull((Select Top 1 jmoDueDate From JobOperations Where jmoJobID = jmaJobID and jmoJobAssemblyID = jmaJobAssemblyID Order By jmoDueDate Desc), (Select jmpproductionDueDate from jobs where jmpJobID = jmajobid)) Where jmaJobID = @jobID " + empty2);
			sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.NVarChar)).Value = jobID;
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("Update Jobs Set jmpScheduledStartDate = isnull((Select Top 1 jmoStartDate From JobOperations Where jmoJobID = jmpJobID Order By jmoStartDate), jmpProductionDueDate) Where jmpJobID = @jobID");
			sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.NVarChar)).Value = jobID;
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("Update Jobs Set jmpScheduledStartHour = isnull((Select Top 1 isnull(jmoStartHour,0) As jmoStartHour From JobOperations Where jmoJobID = jmpJobID Order By jmoStartDate,jmoStartHour),0) Where jmpJobID = @jobID");
			sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.NVarChar)).Value = jobID;
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("Update Jobs Set jmpScheduledDueDate = isnull((Select Top 1 jmoDueDate From JobOperations Where jmoJobID = jmpJobID Order By jmoDueDate Desc), jmpProductionDueDate) Where jmpJobID = @jobID");
			sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.NVarChar)).Value = jobID;
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("Update Jobs Set jmpScheduleComplete = 1, jmpScheduledDueHour = isnull((Select Top 1 isnull(jmoDueHour,0) As jmoDueHour From JobOperations Where jmoJobID = jmpJobID Order By jmoDueDate Desc,jmoDueHour Desc),0) Where jmpJobID = @jobID");
			sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.NVarChar)).Value = jobID;
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("UPDATE JobMaterials SET jmmRequiredDate = jmaScheduledStartDate, jmmOrderByDate = DateAdd(d,-jmmLeadTime,jmaScheduledStartDate) FROM JobMaterials INNER JOIN JobAssemblies ON jmmJobID = jmaJobID AND jmmJobAssemblyID = jmaJobAssemblyID WHERE jmmJobID = @jobID " + empty3);
			sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.NVarChar)).Value = jobID;
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("UPDATE JobMaterials SET jmmRequiredDate = jmoStartDate, jmmOrderByDate = DateAdd(d,-jmmLeadTime,jmoStartDate) FROM JobMaterials INNER JOIN JobOperations ON jmmJobID = jmoJobID AND jmmJobAssemblyID = jmoJobAssemblyID AND jmmRelatedJobOperationID = jmoJobOperationID WHERE jmmJobID = @jobID " + empty3);
			sqlCommand.Parameters.Add(new SqlParameter("@jobID", SqlDbType.NVarChar)).Value = jobID;
			database.ExecuteCommand(sqlCommand, transaction);
		}
	}

	public static void MakeSureScheduleIsInitialized(M1Database database, ScheduleCache cache, ScheduleTree source)
	{
		if (source.StartTask != null && source.FinalTask != null)
		{
			return;
		}
		ResetSchedule(source);
		if (source.AllTasks.Count == 0)
		{
			return;
		}
		Dictionary<ResourceLane, ScheduleAllocation[]> prevAllocations = new Dictionary<ResourceLane, ScheduleAllocation[]>();
		Dictionary<ScheduleTask, Tuple<ScheduleDate, ScheduleDate>> prevDates = new Dictionary<ScheduleTask, Tuple<ScheduleDate, ScheduleDate>>();
		source.AllTasks.ForEach(delegate(ScheduleTask item)
		{
			if (item.StartDate != null)
			{
				prevDates.Add(item, new Tuple<ScheduleDate, ScheduleDate>(item.StartDate, item.EndDate));
			}
			foreach (ResourceLane value in item.ResourceLanes.Values)
			{
				prevAllocations.Add(value, value.Allocations.ToArray());
			}
			item.ClearDates();
		});
		InitSchedules(database, cache, source);
		SetStartEndOperations(source);
		if (ProcessAssemblySiblingLinks(source) != 0)
		{
			InitSchedules(database, cache, source);
			SetStartEndOperations(source);
			if (source.UnlinkedBranches != null)
			{
				source.UnlinkedBranches.Clear();
				CheckForUnlinkedBranches(source, source.AllBranches[0]);
			}
		}
		foreach (ScheduleTask item in source.ScheduledOrder)
		{
			item.ClearDates();
		}
		source.ScheduledOrder.Clear();
		source.AllTasks.ForEach(delegate(ScheduleTask item)
		{
			foreach (ResourceLane value2 in item.ResourceLanes.Values)
			{
				value2.Allocations.AddRange(prevAllocations[value2]);
			}
			if (prevDates.ContainsKey(item))
			{
				item.StartDate = prevDates[item].Item1;
				item.EndDate = prevDates[item].Item2;
			}
			else
			{
				item.StartDate = null;
				item.EndDate = null;
			}
		});
	}

	public static decimal CalculateScheduleInitialHour(M1Database database, string jobId, int assemblyId, int operationId, object actualDateTime, int treeId = 0)
	{
		LoadJob loadJob = new LoadJob();
		LoadSchedule loadSchedule = new LoadSchedule();
		DateTime value = Convert.ToDateTime(actualDateTime);
		object[] keys = new object[1] { jobId };
		ScheduleDate scheduleDate;
		using (ScheduleCache cache = LoadCache(database))
		{
			using ScheduleTree scheduleTree = LoadSource(treeId, loadSchedule, database, cache, loadJob, keys);
			ScheduleTask scheduleTask = scheduleTree.AllBranches.Find((ScheduleBranch item) => item.BranchID == assemblyId).CurrentAndSubTasks.Find((ScheduleTask item) => item.BranchID == assemblyId && item.TaskID == operationId);
			if (scheduleTask == null)
			{
				return 0m;
			}
			scheduleDate = NewDate(database, GetTaskCalendar(cache, scheduleTask), value, 0);
		}
		double totalMinutes = value.TimeOfDay.TotalMinutes;
		double num = scheduleDate.ActualDateTime?.TimeOfDay.TotalMinutes ?? 0.0;
		if (totalMinutes - num < 0.0)
		{
			return 0m;
		}
		return Convert.ToDecimal(totalMinutes - num) / 60m;
	}

	private static ScheduleTree LoadSource(int treeId, LoadSchedule loadSchedule, M1Database database, ScheduleCache cache, LoadJob loadJob, object[] keys)
	{
		if (treeId == 0)
		{
			return loadJob.Load(database, keys, cache);
		}
		return loadSchedule.Load(database, treeId, cache);
	}

	private static void InitSchedules(M1Database database, ScheduleCache cache, ScheduleTree source)
	{
		M1.Core.AppContext appContext = database.GetService(typeof(M1.Core.AppContext)) as M1.Core.AppContext;
		if (!_useStrictOverlapCheck.HasValue)
		{
			_useStrictOverlapCheck = appContext?.Server.IniSettings.GetAsBool("UseStrictOverlapCheck", defaultValue: false);
		}
		ScheduleDate date = new ScheduleDate(DateTime.Today, 0, DateTime.Today);
		if (source.UnlinkedBranches != null && source.UnlinkedBranches.Count != 0)
		{
			foreach (ScheduleUnlinkedBranch unlinkedBranch in source.UnlinkedBranches)
			{
				ScheduleBranch branch = unlinkedBranch.Branch;
				if (branch.Tasks.Count != 0)
				{
					ScheduleTask scheduleTask = branch.Tasks.First();
					ScheduleOperationCollection(database, cache, scheduleTask, scheduleTask.Buckets.Last().Value.TypeBucketID, null, date, (ScheduleOperationScope)7, branch.CurrentAndSubBranches, ScheduleDirection.Backward, null, null);
				}
			}
			return;
		}
		ScheduleTask scheduleTask2 = source.AllTasks[0];
		ScheduleOperationCollection(database, cache, scheduleTask2, scheduleTask2.Buckets.Last().Value.TypeBucketID, null, date, (ScheduleOperationScope)7, source.AllBranches, ScheduleDirection.Backward, null, null);
	}

	private static void CheckForUnlinkedBranches(ScheduleTree source, ScheduleBranch asm)
	{
		foreach (ScheduleBranch item in from item in source.AllBranches
			where item.ParentBranchID == asm.BranchID && item.BranchID != 0
			orderby item.BranchID
			select item)
		{
			CheckForUnlinkedBranches(source, item);
			if (!LoadSchedule.CheckInterAsmLink(asm, item) && item.ParentBranch != null && item.ParentBranch.Tasks.Count == 0)
			{
				source.UnlinkedBranches.Add(new ScheduleUnlinkedBranch
				{
					Branch = item
				});
			}
		}
		if (asm.BranchID == 0 && asm.Tasks.Count != 0)
		{
			source.UnlinkedBranches.Add(new ScheduleUnlinkedBranch
			{
				Branch = asm
			});
		}
	}

	private static byte GetInitialDateType(ScheduleTask operation, ScheduleParameters parms)
	{
		if (parms.InitialDateType == 0)
		{
			if (parms.Direction == ScheduleDirection.Backward)
			{
				return operation.Buckets.Last().Value.TypeBucketID;
			}
			return operation.Buckets.First().Value.TypeBucketID;
		}
		return parms.InitialDateType;
	}

	private static ScheduleTask GetInitialOperation(ScheduleTree source, ScheduleParameters parms)
	{
		ScheduleTask scheduleTask = null;
		if (parms.InitialOperationID != 0)
		{
			scheduleTask = source.AllTasks.Find((ScheduleTask item) => item.BranchID == parms.InitialAssemblyID && item.TaskID == parms.InitialOperationID);
		}
		if (scheduleTask == null)
		{
			scheduleTask = ((parms.Direction != ScheduleDirection.Backward) ? source.StartTask : source.FinalTask);
		}
		return scheduleTask;
	}

	private static List<ScheduleTask> GetScheduleOverlap(ScheduleTask opr, ScheduleOperationScope scope, List<ScheduleBranch> assemblyScope, Stack<ScheduleTask> parentOprStack, List<ScheduleTask> ignoreOperations)
	{
		List<ScheduleTask> list = new List<ScheduleTask>();
		if (opr.Overlaps == null || opr.Overlaps.Count == 0)
		{
			return list;
		}
		if (parentOprStack == null)
		{
			parentOprStack = new Stack<ScheduleTask>();
		}
		else if (parentOprStack.Contains(opr))
		{
			return list;
		}
		parentOprStack.Push(opr);
		Stack<ScheduleTask> stack = new Stack<ScheduleTask>(new Stack<ScheduleTask>(parentOprStack));
		if (_useStrictOverlapCheck.HasValue && _useStrictOverlapCheck.Value)
		{
			foreach (TaskOverlapLink item in opr.Overlaps.Where((TaskOverlapLink taskOverlapLink) => !parentOprStack.Contains(taskOverlapLink.LinkOperation)))
			{
				stack.Push(item.LinkOperation);
			}
		}
		foreach (TaskOverlapLink item2 in opr.Overlaps.Where((TaskOverlapLink oprToSchedule) => oprToSchedule.LinkOperation != opr && !parentOprStack.Contains(oprToSchedule.LinkOperation) && assemblyScope.Contains(oprToSchedule.LinkOperation.ParentBranch)))
		{
			list.AddRange(GetOperationsToSchedule(item2.LinkOperation, stack, scope, assemblyScope, ignoreOperations));
		}
		parentOprStack.Pop();
		return list;
	}

	private static List<ScheduleTask> GetOperationsToSchedule(ScheduleTask startOpr, Stack<ScheduleTask> parentOprStack, ScheduleOperationScope scope, List<ScheduleBranch> assemblyScope, List<ScheduleTask> ignoreOperations)
	{
		List<ScheduleTask> list = new List<ScheduleTask>();
		if (ignoreOperations == null || !ignoreOperations.Contains(startOpr))
		{
			list.Add(startOpr);
		}
		list.AddRange(GetScheduleOverlap(startOpr, ScheduleOperationScope.CurrentOperation, assemblyScope, parentOprStack, MergeList(ignoreOperations, list)));
		if (scope != ScheduleOperationScope.CurrentOperation)
		{
			if ((scope & ScheduleOperationScope.PreviousOperationsThisAssembly) == ScheduleOperationScope.PreviousOperationsThisAssembly)
			{
				for (ScheduleTask previousTask = startOpr.PreviousTask; previousTask != null; previousTask = previousTask.PreviousTask)
				{
					List<ScheduleTask> operationsToSchedule = GetOperationsToSchedule(previousTask, null, ScheduleOperationScope.CurrentOperation, new ScheduleBranch[1] { previousTask.ParentBranch }.ToList(), MergeList(ignoreOperations, list));
					list.AddRange(operationsToSchedule);
				}
			}
			if ((scope & ScheduleOperationScope.SubsequentOperationsThisAssembly) == ScheduleOperationScope.SubsequentOperationsThisAssembly)
			{
				for (ScheduleTask previousTask = startOpr.NextTask; previousTask != null; previousTask = previousTask.NextTask)
				{
					List<ScheduleTask> operationsToSchedule = GetOperationsToSchedule(previousTask, null, ScheduleOperationScope.CurrentOperation, new ScheduleBranch[1] { previousTask.ParentBranch }.ToList(), MergeList(ignoreOperations, list));
					list.AddRange(operationsToSchedule);
				}
			}
			list.AddRange(GetScheduleOverlap(startOpr, scope, assemblyScope, parentOprStack, MergeList(ignoreOperations, list)));
			if ((scope & ScheduleOperationScope.PreviousOperationsThisAssembly) == ScheduleOperationScope.PreviousOperationsThisAssembly)
			{
				for (ScheduleTask previousTask = startOpr.PreviousTask; previousTask != null; previousTask = previousTask.PreviousTask)
				{
					list.AddRange(GetScheduleOverlap(previousTask, (ScheduleOperationScope)7, assemblyScope, parentOprStack, MergeList(ignoreOperations, list)));
				}
			}
			if ((scope & ScheduleOperationScope.SubsequentOperationsThisAssembly) == ScheduleOperationScope.SubsequentOperationsThisAssembly)
			{
				for (ScheduleTask previousTask = startOpr.NextTask; previousTask != null; previousTask = previousTask.NextTask)
				{
					list.AddRange(GetScheduleOverlap(previousTask, (ScheduleOperationScope)7, assemblyScope, parentOprStack, MergeList(ignoreOperations, list)));
				}
			}
		}
		return list;
	}

	private static List<ScheduleBranch> GetAssembliesRange(ScheduleBranch baseAssembly, ScheduleBranch initialAssembly, ScheduleAssemblyScope asmScope)
	{
		List<ScheduleBranch> list;
		if ((asmScope & ScheduleAssemblyScope.ParentAssemblies) == ScheduleAssemblyScope.ParentAssemblies && (asmScope & ScheduleAssemblyScope.ChildAssemblies) == ScheduleAssemblyScope.ChildAssemblies)
		{
			list = baseAssembly.CurrentAndSubBranches;
		}
		else if ((asmScope & ScheduleAssemblyScope.ChildAssemblies) == ScheduleAssemblyScope.ChildAssemblies)
		{
			list = initialAssembly.CurrentAndSubBranches;
		}
		else if ((asmScope & ScheduleAssemblyScope.ParentAssemblies) == ScheduleAssemblyScope.ParentAssemblies)
		{
			list = new List<ScheduleBranch>();
			list.AddRange(baseAssembly.CurrentAndSubBranches.Where((ScheduleBranch item) => !initialAssembly.CurrentAndSubBranches.Contains(item)));
			list.Add(initialAssembly);
		}
		else
		{
			list = new List<ScheduleBranch> { initialAssembly };
		}
		return list;
	}

	private static void ResetSchedule(ScheduleTree source)
	{
		source.ScheduledOrder.Clear();
	}

	private static int ProcessAssemblySiblingLinks(ScheduleTree source)
	{
		int num = 0;
		foreach (ScheduleBranch item in source.AllBranches.Where((ScheduleBranch item) => item.Tasks.Count == 0))
		{
			ScheduleBranch scheduleBranch = null;
			foreach (ScheduleBranch branch in item.Branches)
			{
				if (scheduleBranch != null && scheduleBranch.FinalTask != null && branch.FinalTask != null && branch.SiblingBranchOverlap == BranchOverlap.None)
				{
					num++;
					LoadSchedule.checkOperationOverlap(scheduleBranch.FinalTask, scheduleBranch.FinalTask.Buckets.Last().Value.TypeBucketID, branch.FinalTask, branch.FinalTask.Buckets.Last().Value.TypeBucketID, 0);
					ScheduleBranch parentBranch = branch.ParentBranch;
					branch.ParentBranch = scheduleBranch.FinalTask.ParentBranch;
					FixUpLists(branch, parentBranch);
				}
				if (branch.FinalTask != null && branch.SiblingBranchOverlap == BranchOverlap.None)
				{
					scheduleBranch = branch;
				}
			}
		}
		foreach (ScheduleBranch asm in source.AllBranches.Where((ScheduleBranch item) => item.SiblingBranchOverlap != BranchOverlap.None))
		{
			num++;
			ScheduleBranch scheduleBranch2 = ((asm.SiblingBranchOverlap != BranchOverlap.Next) ? (from item in asm.ParentBranch.Branches
				where item.BranchID < asm.BranchID
				orderby item.BranchID descending
				select item).FirstOrDefault() : (from item in asm.ParentBranch.Branches
				where item.BranchID > asm.BranchID
				orderby item.BranchID
				select item).FirstOrDefault());
			if (scheduleBranch2 == null || asm.CurrentBranchLinkedTask == null)
			{
				continue;
			}
			TaskOverlapLink taskOverlapLink = asm.CurrentBranchLinkedTask.Overlaps.Find((TaskOverlapLink item) => item.LinkOperation.BranchID == asm.ParentBranchID);
			if (taskOverlapLink != null)
			{
				TaskOverlapLink taskOverlapLink2 = taskOverlapLink.LinkOperation.Overlaps.Find((TaskOverlapLink item) => item.LinkOperation == asm.CurrentBranchLinkedTask);
				if (taskOverlapLink2 != null)
				{
					taskOverlapLink.LinkOperation.Overlaps.Remove(taskOverlapLink2);
				}
				taskOverlapLink.LinkOperation = scheduleBranch2.StartTask;
				scheduleBranch2.StartTask.Overlaps.Add(taskOverlapLink2);
				ScheduleBranch parentBranch = asm.ParentBranch;
				if (scheduleBranch2.StartTask != null)
				{
					asm.ParentBranch = scheduleBranch2.StartTask.ParentBranch;
				}
				FixUpLists(asm, parentBranch);
			}
			else if ((asm.SiblingBranchOverlap != BranchOverlap.Previous || scheduleBranch2.SiblingBranchOverlap != BranchOverlap.Next) && asm.CurrentBranchLinkedTask != null && scheduleBranch2.StartTask != null)
			{
				byte b = asm.OverlapSourceLink;
				byte b2 = asm.OverlapDestinationLink;
				if (b == 0)
				{
					b = asm.FinalTask.Buckets.Last().Value.TypeBucketID;
				}
				if (b2 == 0)
				{
					b2 = scheduleBranch2.StartTask.Buckets.First().Value.TypeBucketID;
				}
				LoadSchedule.checkOperationOverlap(asm.CurrentBranchLinkedTask, b, scheduleBranch2.StartTask, b2, 0);
				ScheduleBranch parentBranch = asm.ParentBranch;
				if (scheduleBranch2.StartTask != null)
				{
					asm.ParentBranch = scheduleBranch2.StartTask.ParentBranch;
				}
				FixUpLists(asm, parentBranch);
			}
		}
		return num;
	}

	private static void FixUpLists(ScheduleBranch asm, ScheduleBranch stopAsm)
	{
		ScheduleBranch scheduleBranch = asm;
		while (asm.ParentBranch != null)
		{
			asm = asm.ParentBranch;
			if (asm != stopAsm)
			{
				asm.CurrentAndSubBranches.AddRange(scheduleBranch.CurrentAndSubBranches);
				asm.CurrentAndSubTasks.AddRange(scheduleBranch.CurrentAndSubTasks);
				asm.StartTask = scheduleBranch.StartTask;
				continue;
			}
			break;
		}
	}

	private static void SetStartEndOperations(ScheduleTree source)
	{
		foreach (ScheduleBranch allBranch in source.AllBranches)
		{
			allBranch.StartTask = (from item in allBranch.CurrentAndSubTasks
				where item.StartDate != null
				orderby item.StartDate.ActualDateTime
				select item).FirstOrDefault();
			allBranch.FinalTask = (from item in allBranch.CurrentAndSubTasks
				where item.EndDate != null
				orderby item.EndDate.ActualDateTime
				select item).LastOrDefault();
		}
		ScheduleBranch scheduleBranch = source.AllBranches.Find((ScheduleBranch item) => item.BranchID == 0);
		source.StartTask = scheduleBranch.StartTask;
		source.FinalTask = scheduleBranch.FinalTask;
	}

	private static List<ScheduleTask> ScheduleOperationCollection(M1Database database, ScheduleCache cache, ScheduleTask startOpr, byte oprToScheduleDateType, ScheduleTask linkedOpr, byte linkedOprDateType, ScheduleOperationScope scope, List<ScheduleBranch> assemblyScope, ScheduleDirection initialBias, List<ScheduleTask> ignoreOperations, LocalScheduleData data)
	{
		return ScheduleOperationCollection(database, cache, startOpr, oprToScheduleDateType, null, linkedOpr.Buckets[linkedOprDateType].StartDate, scope, assemblyScope, initialBias, ignoreOperations, data);
	}

	private static List<ScheduleTask> ScheduleOperationCollection(M1Database database, ScheduleCache cache, ScheduleTask startOpr, byte oprToScheduleDateType, Stack<ScheduleTask> parentOprStack, ScheduleDate date, ScheduleOperationScope scope, List<ScheduleBranch> assemblyScope, ScheduleDirection initialBias, List<ScheduleTask> ignoreOperations, LocalScheduleData data)
	{
		List<ScheduleTask> list = new List<ScheduleTask>();
		if (ignoreOperations == null || !ignoreOperations.Contains(startOpr))
		{
			startOpr.ParentBranch.Source.ScheduledOrder.Add(startOpr);
			list.Add(ScheduleOperation(database, cache, startOpr, date, oprToScheduleDateType, initialBias, data));
		}
		list.AddRange(ScheduleOverlap(database, cache, startOpr, ScheduleOperationScope.CurrentOperation, assemblyScope, parentOprStack, initialBias, MergeList(ignoreOperations, list), data));
		if (scope != ScheduleOperationScope.CurrentOperation)
		{
			if ((scope & ScheduleOperationScope.PreviousOperationsThisAssembly) == ScheduleOperationScope.PreviousOperationsThisAssembly)
			{
				for (ScheduleTask previousTask = startOpr.PreviousTask; previousTask != null; previousTask = previousTask.PreviousTask)
				{
					ScheduleDate date2 = GetEarliestOverlap(previousTask.NextTask, previousTask.NextTask, null).StartDate;
					if (previousTask.NextTask.OverlapOffsetMinutes != 0)
					{
						date2 = AdjustDate(database, previousTask.NextTask, date2, -previousTask.NextTask.OverlapOffsetMinutes, ignoreCalendar: false);
					}
					List<ScheduleTask> collection = ScheduleOperationCollection(database, cache, previousTask, previousTask.Buckets.Last().Value.TypeBucketID, null, date2, ScheduleOperationScope.CurrentOperation, new ScheduleBranch[1] { previousTask.ParentBranch }.ToList(), ScheduleDirection.Backward, MergeList(ignoreOperations, list), data);
					ScheduleTask latestOverlap = GetLatestOverlap(previousTask, previousTask, null);
					if (latestOverlap != previousTask)
					{
						collection = ScheduleOperationCollection(database, cache, latestOverlap, latestOverlap.Buckets.Last().Value.TypeBucketID, null, date2, ScheduleOperationScope.CurrentOperation, new ScheduleBranch[1] { previousTask.ParentBranch }.ToList(), ScheduleDirection.Backward, MergeList(ignoreOperations, list), data);
					}
					list.AddRange(collection);
				}
			}
			if ((scope & ScheduleOperationScope.SubsequentOperationsThisAssembly) == ScheduleOperationScope.SubsequentOperationsThisAssembly)
			{
				for (ScheduleTask previousTask = startOpr.NextTask; previousTask != null; previousTask = previousTask.NextTask)
				{
					ScheduleDate date2 = GetLatestOverlap(previousTask.PreviousTask, previousTask.PreviousTask, null).EndDate;
					if (previousTask.OverlapOffsetMinutes != 0)
					{
						date2 = AdjustDate(database, previousTask, date2, previousTask.OverlapOffsetMinutes, ignoreCalendar: false);
					}
					List<ScheduleTask> collection = ScheduleOperationCollection(database, cache, previousTask, previousTask.Buckets.First().Value.TypeBucketID, null, date2, ScheduleOperationScope.CurrentOperation, new ScheduleBranch[1] { previousTask.ParentBranch }.ToList(), ScheduleDirection.Forward, MergeList(ignoreOperations, list), data);
					ScheduleTask earliestOverlap = GetEarliestOverlap(previousTask, previousTask, null);
					if (earliestOverlap != previousTask)
					{
						collection = ScheduleOperationCollection(database, cache, earliestOverlap, earliestOverlap.Buckets.First().Value.TypeBucketID, null, date2, ScheduleOperationScope.CurrentOperation, new ScheduleBranch[1] { previousTask.ParentBranch }.ToList(), ScheduleDirection.Forward, MergeList(ignoreOperations, list), data);
					}
					list.AddRange(collection);
				}
			}
			list.AddRange(ScheduleOverlap(database, cache, startOpr, scope, assemblyScope, parentOprStack, initialBias, MergeList(ignoreOperations, list), data));
			if ((scope & ScheduleOperationScope.PreviousOperationsThisAssembly) == ScheduleOperationScope.PreviousOperationsThisAssembly)
			{
				for (ScheduleTask previousTask = startOpr.PreviousTask; previousTask != null; previousTask = previousTask.PreviousTask)
				{
					list.AddRange(ScheduleOverlap(database, cache, previousTask, (ScheduleOperationScope)7, assemblyScope, parentOprStack, ScheduleDirection.Backward, MergeList(ignoreOperations, list), data));
				}
			}
			if ((scope & ScheduleOperationScope.SubsequentOperationsThisAssembly) == ScheduleOperationScope.SubsequentOperationsThisAssembly)
			{
				for (ScheduleTask previousTask = startOpr.NextTask; previousTask != null; previousTask = previousTask.NextTask)
				{
					list.AddRange(ScheduleOverlap(database, cache, previousTask, (ScheduleOperationScope)7, assemblyScope, parentOprStack, ScheduleDirection.Forward, MergeList(ignoreOperations, list), data));
				}
			}
		}
		return list;
	}

	private static ScheduleTask GetLatestOverlap(ScheduleTask opr, ScheduleTask latestOpr, ScheduleTask prevLinkOpr)
	{
		if (opr.Overlaps == null || opr.Overlaps.Count == 0)
		{
			return latestOpr;
		}
		foreach (TaskOverlapLink item in opr.Overlaps.Where((TaskOverlapLink otherOpr) => otherOpr.LinkOperation != opr && otherOpr.LinkOperation.ParentBranch == opr.ParentBranch && otherOpr.LinkOperation != prevLinkOpr))
		{
			if (item.LinkOperation.EndDate?.ActualDateTime > latestOpr.EndDate?.ActualDateTime)
			{
				latestOpr = item.LinkOperation;
			}
			latestOpr = GetLatestOverlap(item.LinkOperation, latestOpr, opr);
		}
		return latestOpr;
	}

	private static ScheduleTask GetEarliestOverlap(ScheduleTask opr, ScheduleTask earliestOpr, ScheduleTask prevLinkOpr)
	{
		if (opr.Overlaps == null || opr.Overlaps.Count == 0)
		{
			return earliestOpr;
		}
		foreach (TaskOverlapLink item in opr.Overlaps.Where((TaskOverlapLink otherOpr) => otherOpr.LinkOperation != opr && otherOpr.LinkOperation.ParentBranch == opr.ParentBranch && otherOpr.LinkOperation != prevLinkOpr))
		{
			if (item.LinkOperation.StartDate?.ActualDateTime < earliestOpr.StartDate.ActualDateTime)
			{
				earliestOpr = item.LinkOperation;
			}
			earliestOpr = GetEarliestOverlap(item.LinkOperation, earliestOpr, opr);
		}
		return earliestOpr;
	}

	private static List<ScheduleTask> ScheduleOverlap(M1Database database, ScheduleCache cache, ScheduleTask opr, ScheduleOperationScope scope, List<ScheduleBranch> assemblyScope, Stack<ScheduleTask> parentOprStack, ScheduleDirection initialBias, List<ScheduleTask> ignoreOperations, LocalScheduleData data)
	{
		List<ScheduleTask> list = new List<ScheduleTask>();
		if (opr.Overlaps == null || opr.Overlaps.Count == 0)
		{
			return list;
		}
		if (parentOprStack == null)
		{
			parentOprStack = new Stack<ScheduleTask>();
		}
		else if (parentOprStack.Contains(opr))
		{
			return list;
		}
		parentOprStack.Push(opr);
		Stack<ScheduleTask> stack = new Stack<ScheduleTask>(new Stack<ScheduleTask>(parentOprStack));
		if (_useStrictOverlapCheck.HasValue && _useStrictOverlapCheck.Value)
		{
			foreach (TaskOverlapLink item in opr.Overlaps.Where((TaskOverlapLink taskOverlapLink) => taskOverlapLink.LinkOperation.StartDate != null && !parentOprStack.Contains(taskOverlapLink.LinkOperation)))
			{
				stack.Push(item.LinkOperation);
			}
		}
		foreach (TaskOverlapLink item2 in opr.Overlaps.Where((TaskOverlapLink oprToSchedule) => oprToSchedule.LinkOperation != opr && !parentOprStack.Contains(oprToSchedule.LinkOperation) && assemblyScope.Contains(oprToSchedule.LinkOperation.ParentBranch)))
		{
			ScheduleDirection initialBias2 = initialBias;
			if (opr.ParentBranch != item2.LinkOperation.ParentBranch)
			{
				if (opr.ParentBranch.CurrentAndSubBranches.Contains(item2.LinkOperation.ParentBranch))
				{
					initialBias2 = ScheduleDirection.Backward;
				}
				else if (item2.LinkOperation.ParentBranch.CurrentAndSubBranches.Contains(opr.ParentBranch))
				{
					initialBias2 = ScheduleDirection.Forward;
				}
			}
			list.AddRange(ScheduleOperationCollection(database, cache, item2.LinkOperation, item2.LinkDateType, stack, AdjustDate(database, opr, opr.Buckets[item2.ThisDateType].StartDate, item2.OffsetMinutes, ignoreCalendar: false), scope, assemblyScope, initialBias2, ignoreOperations, data));
		}
		parentOprStack.Pop();
		return list;
	}

	private static List<ScheduleTask> MergeList(List<ScheduleTask> ignoreList, List<ScheduleTask> scheduledList)
	{
		if (ignoreList == null || ignoreList.Count == 0)
		{
			return scheduledList;
		}
		if (scheduledList == null || scheduledList.Count == 0)
		{
			return ignoreList;
		}
		List<ScheduleTask> list = new List<ScheduleTask>(ignoreList);
		list.AddRange(scheduledList);
		return list;
	}

	public static ScheduleDate AdjustDate(M1Database database, ScheduleTask opr, ScheduleDate date, int minutesToChange, bool ignoreCalendar)
	{
		if (minutesToChange > 0)
		{
			return DateAddByMinutes(database, opr.PlantCalendar, date, minutesToChange, ignoreCalendar);
		}
		if (minutesToChange < 0)
		{
			return DateSubtractByMinutes(database, opr.PlantCalendar, date, -minutesToChange, ignoreCalendar);
		}
		return date;
	}

	private static ScheduleTask ScheduleOperation(M1Database database, ScheduleCache cache, ScheduleTask task, ScheduleDate date, byte dateType, ScheduleDirection direction, LocalScheduleData data)
	{
		ScheduleTaskNew(database, cache, task, date, dateType, direction, data);
		task.Changed = true;
		return task;
	}

	private static TaskCoreHours GetTaskCoreBuckets(ScheduleTask task)
	{
		TaskCoreHours taskCoreHours = new TaskCoreHours();
		ScheduleTaskBucket scheduleTaskBucket = task.FirstBucket;
		taskCoreHours.TotalMachineHours = default(decimal);
		while (scheduleTaskBucket != null)
		{
			if (scheduleTaskBucket.BucketDefinition.RequiresMachine && taskCoreHours.FirstMachineBucket == null)
			{
				taskCoreHours.FirstMachineBucket = scheduleTaskBucket;
			}
			if (scheduleTaskBucket.BucketDefinition.RequiresMachine)
			{
				taskCoreHours.LastMachineBucket = scheduleTaskBucket;
				taskCoreHours.TotalMachineHours += (decimal)scheduleTaskBucket.Minutes;
			}
			scheduleTaskBucket = scheduleTaskBucket.Next;
		}
		return taskCoreHours;
	}

	private static void SetDatesOnBuckets(M1Database database, ResourceCalendarDefinition calendar, BucketStartParameter startParm)
	{
		ScheduleTaskBucket bucket = startParm.Bucket;
		ScheduleDate initialDate = startParm.InitialDate;
		initialDate = ((startParm.Direction != ScheduleDirection.Forward) ? DateSubtractByMinutes(database, calendar, initialDate, 0, bucket.IgnoreProductionCalendar) : DateAddByMinutes(database, calendar, initialDate, 0, bucket.IgnoreProductionCalendar));
		ScheduleDate endDate = DateAddByMinutes(database, calendar, initialDate, bucket.Minutes, bucket.IgnoreProductionCalendar);
		bucket.StartDate = initialDate;
		bucket.EndDate = endDate;
		for (ScheduleTaskBucket previous = bucket.Previous; previous != null; previous = previous.Previous)
		{
			previous.EndDate = previous.Next.StartDate;
			if (previous.Minutes == 0)
			{
				previous.StartDate = previous.EndDate;
			}
			else
			{
				previous.StartDate = DateSubtractByMinutes(database, calendar, previous.EndDate, previous.Minutes, previous.IgnoreProductionCalendar);
			}
		}
		for (ScheduleTaskBucket previous = bucket.Next; previous != null; previous = previous.Next)
		{
			previous.StartDate = previous.Previous.EndDate;
			if (previous.Minutes == 0)
			{
				previous.EndDate = previous.StartDate;
			}
			else
			{
				previous.EndDate = DateAddByMinutes(database, calendar, previous.StartDate, previous.Minutes, previous.IgnoreProductionCalendar);
			}
		}
	}

	private static void CalculateNewInitialDateFromOverlaps(BucketStartParameter startParm, List<ScheduleAllocation> overlaps, TaskCoreHours taskCore)
	{
		if (startParm.Direction == ScheduleDirection.Forward)
		{
			ScheduleAllocation scheduleAllocation = overlaps.OrderBy((ScheduleAllocation item) => item.EndDate.ActualDateTime).First();
			ScheduleAllocation scheduleAllocation2 = scheduleAllocation?.Next;
			while (NextAllocationIsValid(startParm, scheduleAllocation, scheduleAllocation2))
			{
				scheduleAllocation = scheduleAllocation2;
				scheduleAllocation2 = scheduleAllocation2?.Next;
			}
			startParm.InitialDate = scheduleAllocation?.EndDate;
			startParm.Bucket = taskCore.FirstMachineBucket;
			return;
		}
		ScheduleAllocation scheduleAllocation3 = overlaps.OrderBy((ScheduleAllocation item) => item.StartDate.ActualDateTime).Last();
		while (scheduleAllocation3.Previous != null && scheduleAllocation3.Previous.BucketDefinition.RequiresMachine)
		{
			scheduleAllocation3 = scheduleAllocation3.Previous;
		}
		startParm.InitialDate = scheduleAllocation3.StartDate;
		startParm.Bucket = taskCore.LastMachineBucket;
		if (startParm.Bucket.Next != null)
		{
			startParm.Bucket = startParm.Bucket.Next;
		}
	}

	private static bool NextAllocationIsValid(BucketStartParameter startParam, ScheduleAllocation earliestAllocation, ScheduleAllocation nextAllocation)
	{
		if (nextAllocation != null && nextAllocation.BucketDefinition.RequiresMachine && nextAllocation.StartDate.ActualDateTime >= startParam.Bucket.StartDate.ActualDateTime)
		{
			DateTime? actualDateTime = nextAllocation.EndDate.ActualDateTime;
			DateTime? actualDateTime2 = startParam.Bucket.EndDate.ActualDateTime;
			if ((actualDateTime.HasValue & actualDateTime2.HasValue) && actualDateTime.GetValueOrDefault() >= actualDateTime2.GetValueOrDefault() && nextAllocation.StartDate.ActualDateTime >= earliestAllocation.StartDate.ActualDateTime)
			{
				return nextAllocation.EndDate.ActualDateTime >= earliestAllocation.EndDate.ActualDateTime;
			}
		}
		return false;
	}

	private static LaneGroup GetLaneGroups(Dictionary<short, ResourceLane> resourceLanes)
	{
		LaneGroup laneGroup = new LaneGroup();
		laneGroup.GroupsOneResourcePerTask = new Dictionary<byte, LaneProcess>();
		laneGroup.GroupsMultipleResourcesPerTask = new Dictionary<byte, LaneProcess>();
		foreach (ResourceLane value in resourceLanes.Values)
		{
			if (value.ResourceType == 0)
			{
				continue;
			}
			LaneProcess laneProcess;
			if (value.OneResourcePerTask || value.LockedResourceUniqueID.HasValue)
			{
				if (!laneGroup.GroupsOneResourcePerTask.ContainsKey(value.ResourceType))
				{
					laneGroup.GroupsOneResourcePerTask.Add(value.ResourceType, new LaneProcess());
				}
				laneProcess = laneGroup.GroupsOneResourcePerTask[value.ResourceType];
				if (value.GroupUniqueID.HasValue)
				{
					if (!laneProcess.GroupIDLanes.ContainsKey(value.GroupUniqueID.Value))
					{
						laneProcess.GroupIDLanes.Add(value.GroupUniqueID.Value, new LaneInfo());
					}
					laneProcess.GroupIDLanes[value.GroupUniqueID.Value].Lanes.Add(value);
					if (value.LockedResourceUniqueID.HasValue)
					{
						laneProcess.GroupIDLanes[value.GroupUniqueID.Value].LockedLanes.Add(value.LockedResourceUniqueID.Value);
					}
					laneProcess.ResourceType = value.ResourceType;
				}
				else
				{
					laneProcess.EmptyGroupIDLanes.Add(value);
				}
				continue;
			}
			if (!laneGroup.GroupsMultipleResourcesPerTask.ContainsKey(value.ResourceType))
			{
				laneGroup.GroupsMultipleResourcesPerTask.Add(value.ResourceType, new LaneProcess());
			}
			laneProcess = laneGroup.GroupsMultipleResourcesPerTask[value.ResourceType];
			if (value.GroupUniqueID.HasValue)
			{
				if (!laneProcess.GroupIDLanes.ContainsKey(value.GroupUniqueID.Value))
				{
					laneProcess.GroupIDLanes.Add(value.GroupUniqueID.Value, new LaneInfo());
				}
				laneProcess.GroupIDLanes[value.GroupUniqueID.Value].Lanes.Add(value);
				if (value.LockedResourceUniqueID.HasValue)
				{
					laneProcess.GroupIDLanes[value.GroupUniqueID.Value].LockedLanes.Add(value.LockedResourceUniqueID.Value);
				}
			}
			else
			{
				laneProcess.EmptyGroupIDLanes.Add(value);
			}
		}
		return laneGroup;
	}

	public static ResourceCalendarDefinition GetTaskCalendar(ScheduleCache cache, ScheduleTask task)
	{
		foreach (ResourceLane value in task.ResourceLanes.Values)
		{
			if (value.GroupUniqueID.HasValue && value.ResourceType == ResourceTypes.WorkCenters && cache.ResourceGroups[value.ResourceType].ContainsKey(value.GroupUniqueID.Value) && !string.IsNullOrWhiteSpace(cache.ResourceGroups[value.ResourceType][value.GroupUniqueID.Value].Calendar.WorkCenterID))
			{
				return cache.ResourceGroups[value.ResourceType][value.GroupUniqueID.Value].Calendar;
			}
		}
		return task.PlantCalendar;
	}

	private static void ScheduleTaskNew(M1Database database, ScheduleCache cache, ScheduleTask task, ScheduleDate date, byte dateType, ScheduleDirection direction, LocalScheduleData data)
	{
		ResourceCalendarDefinition taskCalendar = GetTaskCalendar(cache, task);
		BucketStartParameter startParm = new BucketStartParameter(database, taskCalendar, direction, task.Buckets[dateType], date);
		LaneGroup laneGroups = GetLaneGroups(task.ResourceLanes);
		int num = 0;
		bool flag = false;
		TaskCoreHours taskCoreBuckets = GetTaskCoreBuckets(task);
		List<ResourceLane> list = new List<ResourceLane>();
		if (task.ResourceLanes.Count != 0)
		{
			list.AddRange(task.ResourceLanes.Values.Where((ResourceLane item) => item.ResourceType == 0));
		}
		while (num < 10000)
		{
			task.ClearDates();
			flag = true;
			SetDatesOnBuckets(database, taskCalendar, startParm);
			CreateAllocationPerBucketForLanes(null, null, task, list);
			foreach (KeyValuePair<byte, LaneProcess> item in laneGroups.GroupsOneResourcePerTask)
			{
				foreach (KeyValuePair<Guid, LaneInfo> groupIDLane in item.Value.GroupIDLanes)
				{
					if (data != null && !CheckScheduleForSingleResource(database, cache, task, taskCoreBuckets, item.Value.ResourceType, groupIDLane.Key, data.IgnoreOtherJobsForMachines, groupIDLane.Value.Lanes, groupIDLane.Value.LockedLanes, startParm, data))
					{
						num++;
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				if (item.Value.EmptyGroupIDLanes == null || item.Value.EmptyGroupIDLanes.Count == 0)
				{
					continue;
				}
				IEnumerable<KeyValuePair<Guid, IResourceGroup>> enumerable = cache.ResourceGroups[item.Value.ResourceType].Where((KeyValuePair<Guid, IResourceGroup> item) => item.Value.ProcessID == null || item.Value.ProcessID.Equals(task.ProcessID, StringComparison.CurrentCultureIgnoreCase));
				Queue<ResourceLane> queue = new Queue<ResourceLane>(item.Value.EmptyGroupIDLanes);
				foreach (KeyValuePair<Guid, IResourceGroup> item2 in enumerable)
				{
					while (queue.Count != 0)
					{
						ResourceLane resourceLane = queue.Peek();
						if (!CheckScheduleForSingleResource(database, cache, task, taskCoreBuckets, item.Value.ResourceType, item2.Key, data.IgnoreOtherJobsForMachines, new List<ResourceLane>(new ResourceLane[1] { resourceLane }), null, startParm, data))
						{
							break;
						}
						queue.Dequeue();
					}
					if (queue.Count == 0)
					{
						break;
					}
				}
				if (queue.Count != 0)
				{
					num++;
					flag = false;
					break;
				}
			}
			if (flag)
			{
				foreach (KeyValuePair<byte, LaneProcess> item3 in laneGroups.GroupsMultipleResourcesPerTask)
				{
					foreach (KeyValuePair<Guid, LaneInfo> groupIDLane2 in item3.Value.GroupIDLanes)
					{
						foreach (ResourceLane lane in groupIDLane2.Value.Lanes)
						{
							if (!CheckScheduleForMultiResources(database, cache, taskCalendar, taskCoreBuckets, startParm, lane, task, data))
							{
								num++;
								flag = false;
								break;
							}
						}
					}
					foreach (ResourceLane emptyGroupIDLane in item3.Value.EmptyGroupIDLanes)
					{
						if (!CheckScheduleForMultiResources(database, cache, taskCalendar, taskCoreBuckets, startParm, emptyGroupIDLane, task, data))
						{
							num++;
							flag = false;
							break;
						}
					}
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			task.ClearDates();
		}
		task.StartDate = task.FirstBucket.StartDate;
		task.EndDate = task.LastBucket.EndDate;
	}

	private static bool CheckScheduleForSingleResource(M1Database database, ScheduleCache cache, ScheduleTask task, TaskCoreHours taskCore, byte resourceType, Guid groupID, bool ignoreOtherTasks, List<ResourceLane> machineLanes, List<Guid> lockedMachines, BucketStartParameter startParm, LocalScheduleData data)
	{
		Overlaps overlaps = null;
		IResourceGroup resourceGroup = GetResourceGroup(database, cache, resourceType, groupID);
		if (data != null && !ignoreOtherTasks && !resourceGroup.InfiniteCapacity)
		{
			overlaps = CheckScheduleForOtherTasks(database, cache, resourceGroup.ResourceType, resourceGroup.GroupID, taskCore.FirstMachineBucket.StartDate.ActualDateTime.Value, taskCore.LastMachineBucket.EndDate.ActualDateTime.Value, data);
		}
		if (overlaps != null && (overlaps.DistinctResources.Count + machineLanes.Count > resourceGroup.ResourceGuids.Count || (lockedMachines != null && lockedMachines.Exists((Guid item) => overlaps.DistinctResources.Contains(item)))))
		{
			if (overlaps.AllOverlaps.Count != 0)
			{
				CalculateNewInitialDateFromOverlaps(startParm, overlaps.AllOverlaps, taskCore);
				return false;
			}
			data.Messages.Add($"Task {task.TaskID} has been set up to use more machines than are defined on the work center ({resourceGroup.DisplayID}).");
		}
		else
		{
			CreateAllocationPerBucketForLanes(overlaps, resourceGroup, task, machineLanes);
		}
		return true;
	}

	private static void CreateAllocationPerBucketForLanes(Overlaps overlaps, IResourceGroup machineGroup, ScheduleTask task, List<ResourceLane> machineLanes)
	{
		Guid? resourceUniqueID = null;
		foreach (ResourceLane machineLane in machineLanes)
		{
			if (overlaps != null)
			{
				resourceUniqueID = ((!machineLane.LockedResourceUniqueID.HasValue || overlaps.DistinctResources.Contains(machineLane.LockedResourceUniqueID.Value)) ? new Guid?(machineGroup.ResourceGuids.First((Guid item) => !overlaps.DistinctResources.Contains(item))) : new Guid?(machineLane.LockedResourceUniqueID.Value));
				overlaps.DistinctResources.Add(resourceUniqueID.Value);
			}
			foreach (KeyValuePair<byte, ScheduleTaskBucket> bucket in task.Buckets)
			{
				ScheduleAllocation scheduleAllocation = new ScheduleAllocation(bucket.Value.BucketDefinition, machineLane.GroupUniqueID, task);
				scheduleAllocation.StartDate = bucket.Value.StartDate;
				scheduleAllocation.EndDate = bucket.Value.EndDate;
				scheduleAllocation.TotalMinutes = bucket.Value.Minutes;
				scheduleAllocation.ResourceLaneID = machineLane.LaneID;
				if (bucket.Value.BucketDefinition.RequiresMachine)
				{
					scheduleAllocation.ResourceUniqueID = resourceUniqueID;
				}
				machineLane.Allocations.Add(scheduleAllocation);
			}
		}
	}

	private static bool CheckScheduleForMultiResources(M1Database database, ScheduleCache cache, ResourceCalendarDefinition taskCalendar, TaskCoreHours taskCore, BucketStartParameter startParm, ResourceLane employeeLane, ScheduleTask task, LocalScheduleData data)
	{
		ScheduleTaskBucket scheduleTaskBucket = null;
		ScheduleTaskBucket scheduleTaskBucket2 = null;
		if (employeeLane.Cells != null && employeeLane.Cells.Count != 0)
		{
			foreach (KeyValuePair<byte, ScheduleTaskBucket> bucket in task.Buckets)
			{
				if (employeeLane.Cells.ContainsKey(bucket.Key) && employeeLane.Cells[bucket.Key].ResourceUniqueID.HasValue)
				{
					if (scheduleTaskBucket == null)
					{
						scheduleTaskBucket = bucket.Value;
					}
					scheduleTaskBucket2 = bucket.Value;
				}
			}
		}
		else
		{
			scheduleTaskBucket = taskCore.FirstMachineBucket;
			scheduleTaskBucket2 = taskCore.LastMachineBucket;
		}
		if (scheduleTaskBucket != null && scheduleTaskBucket2 != null)
		{
			DateTime date = scheduleTaskBucket.StartDate.Date.Value;
			double totalDays = scheduleTaskBucket2.EndDate.Date.Value.Subtract(scheduleTaskBucket.StartDate.Date.Value).TotalDays;
			for (int i = 0; (double)i <= totalDays; i++)
			{
				DayCalendar calendarForDay = GetCalendarForDay(database, cache, taskCalendar, date);
				if (calendarForDay != null && calendarForDay.Total.TotalMinutes > 0.0)
				{
					TimeSpan value = calendarForDay.Total;
					DateTime startTime = date.AddMinutes(calendarForDay.StartTimeMinutes);
					if (i == 0)
					{
						value = TimeSpan.FromMinutes(value.TotalMinutes - Math.Max(scheduleTaskBucket.StartDate.ActualDateTime.Value.TimeOfDay.TotalMinutes - (double)calendarForDay.StartTimeMinutes, 0.0));
						startTime = scheduleTaskBucket.StartDate.ActualDateTime.Value;
					}
					if ((double)i == totalDays)
					{
						value = TimeSpan.FromMinutes(value.TotalMinutes - Math.Max((double)calendarForDay.EndTimeMinutes - scheduleTaskBucket2.EndDate.ActualDateTime.Value.TimeOfDay.TotalMinutes, 0.0));
					}
					if (value.TotalMinutes > 0.0)
					{
						List<IResourceGroup> list = new List<IResourceGroup>();
						List<ScheduleAllocation> list2 = new List<ScheduleAllocation>();
						do
						{
							DateTime dateTime = startTime.Add(value);
							CalendarOverlap resourceGroupForDoWTimeSpan = GetResourceGroupForDoWTimeSpan(database, cache, date.DayOfWeek, Convert.ToInt32(startTime.TimeOfDay.TotalMinutes), Convert.ToInt32(dateTime.TimeOfDay.TotalMinutes), employeeLane.ResourceType, employeeLane.GroupUniqueID, task.ProcessID, list);
							if (resourceGroupForDoWTimeSpan == null)
							{
								if (list2.Count == 0)
								{
									return true;
								}
								CalculateNewInitialDateFromOverlaps(startParm, list2, taskCore);
								return false;
							}
							list.Add(resourceGroupForDoWTimeSpan.ResourceGroup);
							dateTime = startTime.AddMinutes(resourceGroupForDoWTimeSpan.OverlapMinutes);
							List<ScheduleAllocation> list3 = CreateResourceAllocationsForTimeSpan(database, cache, resourceGroupForDoWTimeSpan.ResourceGroup, resourceGroupForDoWTimeSpan.ResourceGroup.Calendar, startTime, dateTime, list2, employeeLane, task, scheduleTaskBucket, data);
							if (list3.Count == 0)
							{
								continue;
							}
							foreach (ScheduleAllocation item in list3)
							{
								value = TimeSpan.FromMinutes(value.TotalMinutes - (double)item.TotalMinutes);
								startTime = startTime.AddMinutes(item.TotalMinutes);
								employeeLane.Allocations.Add(item);
							}
						}
						while (!(value.TotalMinutes <= 0.0));
					}
				}
				date = date.AddDays(1.0);
			}
		}
		return true;
	}

	private static Overlaps CheckScheduleForOtherTasks(M1Database database, ScheduleCache cache, byte resourceType, Guid groupUniqueID, DateTime startDate, DateTime endDate, LocalScheduleData data)
	{
		LoadResourcesForTimeSpan(cache, startDate, endDate, resourceType, groupUniqueID, database);
		return GetOverlaps(cache.GetResources(resourceType, groupUniqueID).ResourceAllocations, data?.TasksToSchedule, startDate, endDate, resourceType, groupUniqueID);
	}

	public static void LoadAllocationsIntoResourceGroups(ScheduleTree tree, ScheduleCache cache)
	{
		foreach (ScheduleTask allTask in tree.AllTasks)
		{
			foreach (KeyValuePair<short, ResourceLane> resourceLane in allTask.ResourceLanes)
			{
				foreach (ScheduleAllocation allocation in resourceLane.Value.Allocations)
				{
					if (resourceLane.Value.ResourceType != 0 && allocation.GroupUniqueID.HasValue)
					{
						ScheduleCache.LoadedResourceCache resources = cache.GetResources(resourceLane.Value.ResourceType, allocation.GroupUniqueID.Value);
						if (!resources.ResourceAllocations.ContainsKey(allocation.UniqueID.Value))
						{
							resources.ResourceAllocations.Add(allocation.UniqueID.Value, allocation);
						}
					}
				}
			}
		}
	}

	private static Overlaps GetOverlaps(Dictionary<Guid, ScheduleAllocation> resourceAllocations, List<ScheduleTask> tasksToSchedule, DateTime startDate, DateTime endDate, byte resourceType, Guid? groupID)
	{
		Overlaps overlaps = new Overlaps();
		overlaps.AllOverlaps = resourceAllocations.Values.Where((ScheduleAllocation searchedItem) => searchedItem.TotalMinutes != 0 && searchedItem.ResourceUniqueID.HasValue && searchedItem.EndDate.ActualDateTime > startDate && searchedItem.StartDate.ActualDateTime < endDate).ToList();
		overlaps.AllOverlaps.RemoveAll((ScheduleAllocation o) => o.EndDate.ActualDateTime.Value.Date == startDate.Date && o.EndDate.ActualDateTime.Value.Hour == startDate.Hour && o.EndDate.ActualDateTime.Value.Minute == startDate.Minute);
		overlaps.AllOverlaps.RemoveAll((ScheduleAllocation o) => o.StartDate.ActualDateTime.Value.Date == endDate.Date && o.StartDate.ActualDateTime.Value.Hour == endDate.Hour && o.StartDate.ActualDateTime.Value.Minute == endDate.Minute);
		if (tasksToSchedule != null)
		{
			ScheduleAllocation overlap;
			for (int num = overlaps.AllOverlaps.Count - 1; num >= 0; num--)
			{
				overlap = overlaps.AllOverlaps[num];
				if (tasksToSchedule.Find((ScheduleTask item) => item.TaskID == overlap.TaskID && item.TreeID == overlap.TreeID && item.BranchID == overlap.BranchID) != null)
				{
					overlaps.AllOverlaps.RemoveAt(num);
				}
			}
			List<ScheduleAllocation> list = new List<ScheduleAllocation>();
			foreach (ScheduleTask item in tasksToSchedule)
			{
				foreach (KeyValuePair<short, ResourceLane> item2 in item.ResourceLanes.Where(delegate(KeyValuePair<short, ResourceLane> l)
				{
					if (l.Value.ResourceType == resourceType)
					{
						Guid? groupUniqueID = l.Value.GroupUniqueID;
						Guid? guid = groupID;
						if (groupUniqueID.HasValue != guid.HasValue)
						{
							return false;
						}
						if (!groupUniqueID.HasValue)
						{
							return true;
						}
						return groupUniqueID.GetValueOrDefault() == guid.GetValueOrDefault();
					}
					return false;
				}))
				{
					list.AddRange(item2.Value.Allocations.Where((ScheduleAllocation item) => item.ResourceUniqueID.HasValue && item.StartDate != null && item.TotalMinutes != 0));
				}
			}
			List<ScheduleAllocation> collection = list.Where((ScheduleAllocation searchedItem) => searchedItem.TotalMinutes != 0 && searchedItem.StartDate != null && searchedItem.EndDate.ActualDateTime > startDate && searchedItem.StartDate.ActualDateTime < endDate).ToList();
			overlaps.AllOverlaps.AddRange(collection);
		}
		overlaps.DistinctResources = overlaps.AllOverlaps.ConvertAll((ScheduleAllocation item) => item.ResourceUniqueID.Value).Distinct().ToList();
		return overlaps;
	}

	private static void SetResourceIDOnMachines(IResourceGroup calendar, ScheduleTaskBucket bucket, List<ResourceLane> resourceLanes, List<Guid> usedResources)
	{
		Queue<ResourceLane> queue = new Queue<ResourceLane>(resourceLanes);
		foreach (Guid resourceID in calendar.ResourceGuids)
		{
			if (usedResources.Contains(resourceID))
			{
				continue;
			}
			queue.Dequeue().Allocations.ForEach(delegate(ScheduleAllocation item)
			{
				if (item.BucketDefinition.RequiresMachine)
				{
					item.ResourceUniqueID = resourceID;
				}
				else
				{
					item.ResourceUniqueID = null;
				}
			});
			usedResources.Add(resourceID);
			if (queue.Count == 0)
			{
				break;
			}
		}
	}

	private static bool IsTimeValidForDay(ScheduleDate date, DayCalendar day)
	{
		if (day != null && (double)day.StartTimeMinutes <= date.ActualDateTime.Value.TimeOfDay.TotalMinutes && (double)day.EndTimeMinutes >= date.ActualDateTime.Value.TimeOfDay.TotalMinutes)
		{
			return true;
		}
		return false;
	}

	private static ScheduleDate DateAddByMinutes(M1Database database, ResourceCalendarDefinition calendar, ScheduleDate date, int minutesToChange, bool ignoreCalendar)
	{
		if (ignoreCalendar)
		{
			return new ScheduleDate(date.ActualDateTime.Value.AddMinutes(minutesToChange), calendar);
		}
		int num = date.ActualDateTime.Value.Year;
		int num2 = date.ActualDateTime.Value.DayOfYear;
		short? num3 = null;
		YearCalendar workingDaysForYear = GetWorkingDaysForYear(database, num, calendar);
		short dayMinutes = GetDayMinutes(workingDaysForYear[num2]);
		bool flag = true;
		if (dayMinutes != 0 && dayMinutes >= date.Minute)
		{
			DayCalendar dayCalendar = workingDaysForYear[num2];
			flag = date.ActualDateTime.Value.TimeOfDay.TotalMinutes >= (double)dayCalendar.StartTimeMinutes;
			if (date.ActualDateTime.Value.TimeOfDay.TotalMinutes != date.ActualDateTime.Value.Date.AddMinutes(dayCalendar.StartTimeMinutes + date.Minute).TimeOfDay.TotalMinutes)
			{
				date = NewDate(database, calendar, date.ActualDateTime.Value.Date, (short)(date.ActualDateTime.Value.TimeOfDay.TotalMinutes - (double)dayCalendar.StartTimeMinutes));
			}
			int remainingTime = GetRemainingTime(date, dayMinutes, dayCalendar);
			if (dayCalendar != null && (double)dayCalendar.EndTimeMinutes < date.ActualDateTime.Value.TimeOfDay.TotalMinutes)
			{
				num2++;
			}
			else if (dayCalendar == null || !((double)dayCalendar.StartTimeMinutes > date.ActualDateTime.Value.TimeOfDay.TotalMinutes) || remainingTime <= minutesToChange)
			{
				minutesToChange += date.Minute;
			}
		}
		else
		{
			num2++;
			if (minutesToChange != 0)
			{
			}
		}
		while (!num3.HasValue)
		{
			while (num2 <= workingDaysForYear.Count && !num3.HasValue)
			{
				dayMinutes = GetDayMinutes(workingDaysForYear[num2]);
				if (dayMinutes != 0)
				{
					if (minutesToChange <= dayMinutes)
					{
						num3 = (short)minutesToChange;
						minutesToChange = 0;
						break;
					}
					minutesToChange -= dayMinutes;
				}
				num2 = (flag ? (num2 + 1) : num2);
				flag = true;
			}
			if (!num3.HasValue)
			{
				num++;
				workingDaysForYear = GetWorkingDaysForYear(database, num, calendar);
				num2 = 1;
			}
		}
		DateTime value = new DateTime(num, 1, 1).AddDays(num2 - 1);
		return NewDate(database, calendar, value, num3.Value);
	}

	private static int GetRemainingTime(ScheduleDate date, short workingDayMinutes, DayCalendar workingDay)
	{
		bool flag = workingDayMinutes + workingDay.StartTimeMinutes > 1440;
		if (date.ActualDateTime.HasValue && date.ActualDateTime.Value.TimeOfDay.TotalMinutes < (double)workingDay.StartTimeMinutes && date.Minute < 0 && flag)
		{
			return workingDayMinutes - (1440 - workingDay.StartTimeMinutes);
		}
		return workingDayMinutes - date.Minute;
	}

	private static short GetDayMinutes(DayCalendar data)
	{
		if (data == null || data.Count == 0)
		{
			return 0;
		}
		return Convert.ToInt16(data.Total.TotalMinutes);
	}

	public static DateTime DateAddByDays(M1Database database, string plantID, DateTime date, int daysToChange)
	{
		using ScheduleCache scheduleCache = new ScheduleCache();
		LoadPlants(scheduleCache, database, plantID);
		ResourceCalendarDefinition calendar = scheduleCache.PlantCalendars[plantID];
		return DateAddByDays(database, calendar, date, daysToChange);
	}

	public static DateTime DateAddByDays(M1Database database, ResourceCalendarDefinition calendar, DateTime date, int daysToChange)
	{
		int num = date.Year;
		int i = date.DayOfYear;
		YearCalendar workingDaysForYear = GetWorkingDaysForYear(database, num, calendar);
		bool flag = false;
		if (GetDayMinutes(workingDaysForYear[i]) == 0)
		{
			i++;
			while (true)
			{
				if (i <= workingDaysForYear.Count)
				{
					if (GetDayMinutes(workingDaysForYear[i]) == 0)
					{
						i++;
						continue;
					}
					flag = true;
				}
				if (flag)
				{
					break;
				}
				num++;
				workingDaysForYear = GetWorkingDaysForYear(database, num, calendar);
				i = 1;
			}
		}
		bool flag2 = false;
		while (daysToChange >= 0)
		{
			for (; i <= workingDaysForYear.Count; i++)
			{
				if (daysToChange < 0)
				{
					break;
				}
				if (GetDayMinutes(workingDaysForYear[i]) != 0)
				{
					daysToChange--;
					if (daysToChange < 0)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (daysToChange >= 0 && !flag2)
			{
				num++;
				workingDaysForYear = GetWorkingDaysForYear(database, num, calendar);
				i = 1;
			}
		}
		return new DateTime(num, 1, 1).AddDays(i - 1);
	}

	public static DateTime DateSubtractByDays(M1Database database, ResourceCalendarDefinition calendar, DateTime date, int daysToChange)
	{
		int num = date.Year;
		int num2 = date.DayOfYear;
		YearCalendar workingDaysForYear = GetWorkingDaysForYear(database, num, calendar);
		while (daysToChange > 0)
		{
			while (num2 > 0 && daysToChange > 0)
			{
				if (GetDayMinutes(workingDaysForYear[num2]) != 0)
				{
					daysToChange--;
				}
				num2--;
			}
			if (daysToChange > 0)
			{
				num--;
				workingDaysForYear = GetWorkingDaysForYear(database, num, calendar);
				num2 = workingDaysForYear.Count;
			}
		}
		return new DateTime(num, 1, 1).AddDays(num2 - 1);
	}

	public static DateTime DateSubtractByDays(M1Database database, string plantID, DateTime date, int daysToChange)
	{
		using ScheduleCache scheduleCache = new ScheduleCache();
		LoadPlants(scheduleCache, database, plantID);
		ResourceCalendarDefinition calendar = scheduleCache.PlantCalendars[plantID];
		return DateSubtractByDays(database, calendar, date, daysToChange);
	}

	private static ScheduleDate DateSubtractByMinutes(M1Database database, ResourceCalendarDefinition calendar, ScheduleDate date, int minutesToChange, bool ignoreCalendar)
	{
		if (ignoreCalendar)
		{
			return new ScheduleDate(date.ActualDateTime.Value.AddMinutes(-minutesToChange), calendar);
		}
		int num = date.Date.Value.Year;
		int num2 = date.Date.Value.DayOfYear;
		short? num3 = null;
		YearCalendar workingDaysForYear = GetWorkingDaysForYear(database, num, calendar);
		short dayMinutes = GetDayMinutes(workingDaysForYear[num2]);
		if (dayMinutes != 0 && dayMinutes >= date.Minute)
		{
			DayCalendar dayCalendar = workingDaysForYear[num2];
			if (dayCalendar == null || !((double)dayCalendar.EndTimeMinutes < date.ActualDateTime.Value.TimeOfDay.TotalMinutes))
			{
				if (dayCalendar != null && (double)dayCalendar.StartTimeMinutes > date.ActualDateTime.Value.TimeOfDay.TotalMinutes)
				{
					num2--;
				}
				else
				{
					if (date.ActualDateTime.Value.TimeOfDay.TotalMinutes != date.ActualDateTime.Value.Date.AddMinutes(dayCalendar.StartTimeMinutes + date.Minute).TimeOfDay.TotalMinutes)
					{
						date = NewDate(database, calendar, date.ActualDateTime.Value.Date, (short)(date.ActualDateTime.Value.TimeOfDay.TotalMinutes - (double)dayCalendar.StartTimeMinutes));
					}
					minutesToChange += dayMinutes - date.Minute;
				}
			}
		}
		else
		{
			num2--;
			if (minutesToChange != 0)
			{
			}
		}
		while (!num3.HasValue)
		{
			while (num2 > 0 && !num3.HasValue)
			{
				dayMinutes = GetDayMinutes(workingDaysForYear[num2]);
				if (dayMinutes != 0)
				{
					if (minutesToChange <= dayMinutes)
					{
						num3 = (short)(dayMinutes - minutesToChange);
						minutesToChange = 0;
						break;
					}
					minutesToChange -= dayMinutes;
				}
				num2--;
			}
			if (!num3.HasValue)
			{
				num--;
				workingDaysForYear = GetWorkingDaysForYear(database, num, calendar);
				num2 = workingDaysForYear.Count;
			}
		}
		DateTime value = new DateTime(num, 1, 1).AddDays(num2 - 1);
		return NewDate(database, calendar, value, num3.Value);
	}

	public static Dictionary<DateTime, StartTimeAndHours> GetWorkingDaysInRange(M1Database database, ScheduleCache cache, ResourceCalendarDefinition calendar, DateTime startDate, DateTime endDate)
	{
		Dictionary<DateTime, StartTimeAndHours> dictionary = new Dictionary<DateTime, StartTimeAndHours>();
		DateTime dateTime = startDate.Date;
		while (dateTime <= endDate)
		{
			DayCalendar calendarForDay = GetCalendarForDay(database, cache, calendar, dateTime);
			if (calendarForDay != null && calendarForDay.Total.TotalMinutes != 0.0)
			{
				dictionary.Add(dateTime, new StartTimeAndHours(calendarForDay.StartTimeMinutes, (decimal)calendarForDay.Total.TotalMinutes / 60.0m));
			}
			dateTime = dateTime.AddDays(1.0);
		}
		return dictionary;
	}

	public static List<DateTime> GetNonWorkingDaysInRange(M1Database database, ScheduleCache cache, ResourceCalendarDefinition calendar, DateTime startDate, DateTime endDate)
	{
		List<DateTime> list = new List<DateTime>();
		DateTime dateTime = startDate.Date;
		while (dateTime <= endDate)
		{
			DayCalendar calendarForDay = GetCalendarForDay(database, cache, calendar, dateTime);
			if (calendarForDay == null || calendarForDay.Total.TotalMinutes == 0.0)
			{
				list.Add(dateTime);
			}
			dateTime = dateTime.AddDays(1.0);
		}
		return list;
	}

	public static bool IsNonWorkingDay(M1Database database, ResourceCalendarDefinition calendar, DateTime date)
	{
		DayCalendar dayCalendar = GetWorkingDaysForYear(database, date.Year, calendar)[date.DayOfYear];
		if (dayCalendar != null)
		{
			return dayCalendar.Total.TotalMinutes == 0.0;
		}
		return true;
	}

	public static bool IsHoliday(M1Database database, ScheduleCache cache, ResourceCalendarDefinition calendar, DateTime date)
	{
		YearCalendar workingDaysForYear = GetWorkingDaysForYear(database, date.Year, calendar);
		if (workingDaysForYear.Holidays == null)
		{
			workingDaysForYear.Holidays = new List<DateTime>();
			SqlCommand sqlCommand = database.NewSqlCommand("select jmyProductionCalendarMonth,jmyProductionCalendarDay from ProductionCalendarDays Where jmyProductionCalendarYearID = @YearID And jmyPlantID = @PlantID And jmyWorkCenterID = @WorkCenterID And jmyHoliday <> 0 Order By jmyProductionCalendarMonth,jmyProductionCalendarDay");
			sqlCommand.Parameters.Add(new SqlParameter("@YearID", SqlDbType.Int)).Value = workingDaysForYear.Year;
			sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = calendar.PlantID;
			sqlCommand.Parameters.Add(new SqlParameter("@WorkCenterID", SqlDbType.NVarChar)).Value = calendar.WorkCenterID;
			foreach (DataRow row in database.GetDataTable(sqlCommand).Rows)
			{
				workingDaysForYear.Holidays.Add(new DateTime(workingDaysForYear.Year, row.Field<byte>("jmyProductionCalendarMonth"), row.Field<byte>("jmyProductionCalendarDay")));
			}
		}
		return workingDaysForYear.Holidays.Contains(date);
	}

	private static DayCalendar GetCalendarForDay(M1Database database, ScheduleCache cache, ResourceCalendarDefinition calendar, DateTime date)
	{
		return GetWorkingDaysForYear(database, date.Year, calendar)[date.DayOfYear];
	}

	private static DataTable FillTableWithDaysForYear(M1Database database, int year, string plantID, string workCenterID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select jmyProductionCalendarYearID,jmyProductionCalendarMonth,jmyProductionCalendarDay,jmyHours,jmyDayStartTime,jmyHoliday from ProductionCalendarDays Where jmyProductionCalendarYearID = @YearID And jmyPlantID = @PlantID And jmyWorkCenterID = @WorkCenterID Order By jmyProductionCalendarMonth,jmyProductionCalendarDay");
		sqlCommand.Parameters.Add(new SqlParameter("@YearID", SqlDbType.Int)).Value = year;
		sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = string.Empty;
		sqlCommand.Parameters.Add(new SqlParameter("@WorkCenterID", SqlDbType.NVarChar)).Value = string.Empty;
		return database.GetDataTable(sqlCommand);
	}

	public static void FillTableWithDaysForYear(M1Database database, int year, string plantID, string workCenterID, DataTable daysTable)
	{
		DataRow dataRow = null;
		if (!string.IsNullOrWhiteSpace(workCenterID))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select xawHoursSun As xadHoursSun,xawDayStartTimeSun As xadDayStartTimeSun,xawHoursMon As xadHoursMon,xawDayStartTimeMon As xadDayStartTimeMon,xawHoursTue As xadHoursTue,xawDayStartTimeTue As xadDayStartTimeTue,xawHoursWed As xadHoursWed,xawDayStartTimeWed As xadDayStartTimeWed,xawHoursThu As xadHoursThu,xawDayStartTimeThu As xadDayStartTimeThu,xawHoursFri As xadHoursFri,xawDayStartTimeFri As xadDayStartTimeFri,xawHoursSat As xadHoursSat,xawDayStartTimeSat As xadDayStartTimeSat From WorkCenters Where xawWorkCenterID = @WorkCenterID");
			sqlCommand.Parameters.Add(new SqlParameter("@WorkCenterID", SqlDbType.NVarChar)).Value = workCenterID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				dataRow = dataTable.Rows[0];
			}
		}
		if (dataRow == null && !string.IsNullOrWhiteSpace(plantID))
		{
			SqlCommand sqlCommand2 = database.NewSqlCommand("Select xauHoursSun As xadHoursSun,xauDayStartTimeSun As xadDayStartTimeSun,xauHoursMon As xadHoursMon,xauDayStartTimeMon As xadDayStartTimeMon,xauHoursTue As xadHoursTue,xauDayStartTimeTue As xadDayStartTimeTue,xauHoursWed As xadHoursWed,xauDayStartTimeWed As xadDayStartTimeWed,xauHoursThu As xadHoursThu,xauDayStartTimeThu As xadDayStartTimeThu,xauHoursFri As xadHoursFri,xauDayStartTimeFri As xadDayStartTimeFri,xauHoursSat As xadHoursSat,xauDayStartTimeSat As xadDayStartTimeSat From Plants Where xauPlantID = @PlantID");
			sqlCommand2.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = plantID;
			DataTable dataTable2 = database.GetDataTable(sqlCommand2);
			if (dataTable2.Rows.Count != 0)
			{
				dataRow = dataTable2.Rows[0];
			}
		}
		if (dataRow == null)
		{
			dataRow = database.Props("DS");
		}
		DataTable dataTable3 = null;
		if (!string.IsNullOrWhiteSpace(workCenterID))
		{
			dataTable3 = FillTableWithDaysForYear(database, year, plantID, string.Empty);
		}
		if (!string.IsNullOrWhiteSpace(plantID) && (dataTable3 == null || dataTable3.Rows.Count == 0))
		{
			dataTable3 = FillTableWithDaysForYear(database, year, string.Empty, string.Empty);
		}
		if (dataTable3 != null && dataTable3.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable3.Rows)
			{
				DataRow dataRow3 = daysTable.AddBlankRow();
				dataRow3["jmyProductionCalendarYearID"] = row["jmyProductionCalendarYearID"];
				dataRow3["jmyProductionCalendarMonth"] = row["jmyProductionCalendarMonth"];
				dataRow3["jmyProductionCalendarDay"] = row["jmyProductionCalendarDay"];
				dataRow3["jmyHours"] = row["jmyHours"];
				dataRow3["jmyDayStartTime"] = row["jmyDayStartTime"];
				dataRow3["jmyHoliday"] = row["jmyHoliday"];
				dataRow3["jmyPlantID"] = plantID;
				dataRow3["jmyWorkCenterID"] = workCenterID;
			}
			foreach (DataRow row2 in daysTable.Rows)
			{
				if (!row2.Field<bool>("jmyHoliday"))
				{
					DateTime dateTime = new DateTime(row2.Field<short>("jmyProductionCalendarYearID"), row2.Field<byte>("jmyProductionCalendarMonth"), row2.Field<byte>("jmyProductionCalendarDay"));
					if (dateTime.DayOfWeek == DayOfWeek.Sunday)
					{
						row2["jmyHours"] = dataRow["xadHoursSun"];
						row2["jmyDayStartTime"] = dataRow["xadDayStartTimeSun"];
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Monday)
					{
						row2["jmyHours"] = dataRow["xadHoursMon"];
						row2["jmyDayStartTime"] = dataRow["xadDayStartTimeMon"];
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Tuesday)
					{
						row2["jmyHours"] = dataRow["xadHoursTue"];
						row2["jmyDayStartTime"] = dataRow["xadDayStartTimeTue"];
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Wednesday)
					{
						row2["jmyHours"] = dataRow["xadHoursWed"];
						row2["jmyDayStartTime"] = dataRow["xadDayStartTimeWed"];
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Thursday)
					{
						row2["jmyHours"] = dataRow["xadHoursThu"];
						row2["jmyDayStartTime"] = dataRow["xadDayStartTimeThu"];
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Friday)
					{
						row2["jmyHours"] = dataRow["xadHoursFri"];
						row2["jmyDayStartTime"] = dataRow["xadDayStartTimeFri"];
					}
					else if (dateTime.DayOfWeek == DayOfWeek.Saturday)
					{
						row2["jmyHours"] = dataRow["xadHoursSat"];
						row2["jmyDayStartTime"] = dataRow["xadDayStartTimeSat"];
					}
				}
			}
		}
		if (daysTable.Rows.Count != 0)
		{
			return;
		}
		DateTime dateTime2 = new DateTime(year, 1, 1);
		while (dateTime2.Year == year)
		{
			DataRow dataRow3 = daysTable.AddBlankRow();
			dataRow3["jmyProductionCalendarYearID"] = dateTime2.Year;
			dataRow3["jmyProductionCalendarMonth"] = dateTime2.Month;
			dataRow3["jmyProductionCalendarDay"] = dateTime2.Day;
			dataRow3["jmyPlantID"] = plantID;
			dataRow3["jmyWorkCenterID"] = workCenterID;
			if ((dateTime2.Month == 1 && dateTime2.Day == 1) || (dateTime2.Month == 12 && dateTime2.Day == 25))
			{
				dataRow3["jmyHours"] = 0;
				dataRow3["jmyDayStartTime"] = 0;
			}
			else if (dateTime2.DayOfWeek == DayOfWeek.Sunday)
			{
				dataRow3["jmyHours"] = dataRow["xadHoursSun"];
				dataRow3["jmyDayStartTime"] = dataRow["xadDayStartTimeSun"];
			}
			else if (dateTime2.DayOfWeek == DayOfWeek.Monday)
			{
				dataRow3["jmyHours"] = dataRow["xadHoursMon"];
				dataRow3["jmyDayStartTime"] = dataRow["xadDayStartTimeMon"];
			}
			else if (dateTime2.DayOfWeek == DayOfWeek.Tuesday)
			{
				dataRow3["jmyHours"] = dataRow["xadHoursTue"];
				dataRow3["jmyDayStartTime"] = dataRow["xadDayStartTimeTue"];
			}
			else if (dateTime2.DayOfWeek == DayOfWeek.Wednesday)
			{
				dataRow3["jmyHours"] = dataRow["xadHoursWed"];
				dataRow3["jmyDayStartTime"] = dataRow["xadDayStartTimeWed"];
			}
			else if (dateTime2.DayOfWeek == DayOfWeek.Thursday)
			{
				dataRow3["jmyHours"] = dataRow["xadHoursThu"];
				dataRow3["jmyDayStartTime"] = dataRow["xadDayStartTimeThu"];
			}
			else if (dateTime2.DayOfWeek == DayOfWeek.Friday)
			{
				dataRow3["jmyHours"] = dataRow["xadHoursFri"];
				dataRow3["jmyDayStartTime"] = dataRow["xadDayStartTimeFri"];
			}
			else if (dateTime2.DayOfWeek == DayOfWeek.Saturday)
			{
				dataRow3["jmyHours"] = dataRow["xadHoursSat"];
				dataRow3["jmyDayStartTime"] = dataRow["xadDayStartTimeSat"];
			}
			dateTime2 = dateTime2.AddDays(1.0);
		}
	}

	public static ScheduleDate NewDate(M1Database database, ResourceCalendarDefinition calendar, DateTime? date, short minute)
	{
		DateTime? actualTime;
		if (date.HasValue)
		{
			if (calendar == null)
			{
				actualTime = date.Value.Date.AddMinutes(minute);
			}
			else
			{
				DayCalendar dayCalendar = GetWorkingDaysForYear(database, date.Value.Year, calendar)[date.Value.DayOfYear];
				actualTime = ((dayCalendar == null || dayCalendar.StartTimeMinutes <= 0) ? new DateTime?(date.Value.Date.AddMinutes(minute)) : new DateTime?(date.Value.Date.AddMinutes(dayCalendar.StartTimeMinutes + minute)));
			}
		}
		else
		{
			actualTime = null;
		}
		return new ScheduleDate(date, minute, actualTime);
	}

	private static YearCalendar GetWorkingDaysForYear(M1Database database, int year, ResourceCalendarDefinition calendar)
	{
		if (!calendar.LoadedYears.ContainsKey(year))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select jmyProductionCalendarMonth,jmyProductionCalendarDay,jmyHours,jmyDayStartTime from ProductionCalendarDays Where jmyProductionCalendarYearID = @YearID And jmyPlantID = @PlantID And jmyWorkCenterID = @WorkCenterID Order By jmyProductionCalendarMonth,jmyProductionCalendarDay");
			sqlCommand.Parameters.Add(new SqlParameter("@YearID", SqlDbType.Int)).Value = year;
			sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = calendar.PlantID;
			sqlCommand.Parameters.Add(new SqlParameter("@WorkCenterID", SqlDbType.NVarChar)).Value = calendar.WorkCenterID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			YearCalendar yearCalendar;
			if (dataTable.Rows.Count == 0)
			{
				bool setAllDays = false;
				if (!string.IsNullOrWhiteSpace(calendar.WorkCenterID))
				{
					yearCalendar = new YearCalendar(GetWorkingDaysForYear(database, year, calendar.ParentCalendar));
				}
				else if (!string.IsNullOrWhiteSpace(calendar.PlantID))
				{
					yearCalendar = new YearCalendar(GetWorkingDaysForYear(database, year, calendar.ParentCalendar));
				}
				else
				{
					setAllDays = true;
					yearCalendar = new YearCalendar(year);
				}
				ApplyDayOfWeekDefaultsToYear(database, yearCalendar, calendar.DayOfWeekDefaults, setAllDays);
			}
			else
			{
				yearCalendar = new YearCalendar(year);
				foreach (DataRow row in dataTable.Rows)
				{
					DateTime dateTime = new DateTime(year, row.Field<byte>("jmyProductionCalendarMonth"), row.Field<byte>("jmyProductionCalendarDay"));
					DayCalendar dayCalendar = calendar.DayOfWeekDefaults[dateTime.DayOfWeek];
					if (row.Field<decimal>("jmyHours") == 0m)
					{
						yearCalendar[dateTime.DayOfYear] = null;
						continue;
					}
					if (dayCalendar.Total.TotalMinutes.Equals(M1Time.ConvertDecimalHoursToMinutes(row.Field<decimal>("jmyHours"))) && dayCalendar.StartTimeMinutes.Equals(M1Time.ConvertDecimalTimeToMinutes(row.Field<decimal>("jmyDayStartTime"))) && dayCalendar.Count == 1)
					{
						yearCalendar[dateTime.DayOfYear] = dayCalendar;
						continue;
					}
					yearCalendar[dateTime.DayOfYear] = new DayCalendar(new TimeBucket(row.Field<decimal>("jmyHours"), row.Field<decimal>("jmyDayStartTime")));
				}
			}
			calendar.LoadedYears.Add(year, yearCalendar);
		}
		return calendar.LoadedYears[year];
	}

	private static void ApplyDayOfWeekDefaultsToYear(M1Database database, YearCalendar yearCalendar, Dictionary<DayOfWeek, DayCalendar> dayOfWeekDefaults, bool setAllDays)
	{
		DayOfWeek dayOfWeek = yearCalendar.StartDayOfWeek;
		for (int i = 1; i <= yearCalendar.Count; i++)
		{
			DayCalendar dayCalendar = dayOfWeekDefaults[dayOfWeek];
			if (dayCalendar == null || dayCalendar.Total.TotalMinutes == 0.0 || i == 1)
			{
				yearCalendar[i] = null;
			}
			else if (setAllDays || yearCalendar[i] != null)
			{
				yearCalendar[i] = dayOfWeekDefaults[dayOfWeek];
			}
			dayOfWeek = ((dayOfWeek != DayOfWeek.Saturday) ? (dayOfWeek + 1) : DayOfWeek.Sunday);
		}
	}

	public static IResourceGroup GetResourceGroup(M1Database database, ScheduleCache cache, byte resourceType, object groupDisplayID)
	{
		foreach (IResourceGroup value in cache.ResourceGroups[resourceType].Values)
		{
			if (value.DisplayID.Equals(groupDisplayID))
			{
				return value;
			}
		}
		return null;
	}

	public static IResourceGroup GetResourceGroup(M1Database database, ScheduleCache cache, byte resourceType, Guid groupUniqueID)
	{
		return cache.ResourceGroups[resourceType][groupUniqueID];
	}

	private static string LoadWorkCentersFieldList()
	{
		return "xawWorkCenterID,xawUniqueID,xawPlantID,xawProcessID,xawInfiniteCapacity,xawFiniteTolerance,xawPeoplePerMachineSetup,xawPeoplePerMachineProd,xawEnableCalendar,xawHoursSun,xawDayStartTimeSun,xawHoursMon,xawDayStartTimeMon,xawHoursTue,xawDayStartTimeTue,xawHoursWed,xawDayStartTimeWed,xawHoursThu,xawDayStartTimeThu,xawHoursFri,xawDayStartTimeFri,xawHoursSat,xawDayStartTimeSat";
	}

	public static void LoadWorkCenters(ScheduleCache cache, M1Database database)
	{
		DataTable dataTable = database.GetDataTable("Select " + LoadWorkCentersFieldList() + " From WorkCenters");
		DataTable dataTable2 = database.GetDataTable("Select xaqWorkCenterID, xaqWorkCenterMachineID, xaqUniqueID From WorkCenterMachines");
		LoadWorkCenters(cache, dataTable, dataTable2);
	}

	private static void LoadWorkCenters(ScheduleCache cache, DataTable wcTable, DataTable workCenterMachines)
	{
		Dictionary<Guid, IResourceGroup> dictionary = new Dictionary<Guid, IResourceGroup>();
		foreach (DataRow row in wcTable.Rows)
		{
			if (!dictionary.ContainsKey(row.Field<Guid>("xawUniqueID")))
			{
				ResourceCalendarDefinition calendar = ((!row.Field<bool>("xawEnableCalendar")) ? cache.PlantCalendars[row.Field<string>("xawPlantID")] : new ResourceCalendarDefinition(cache.PlantCalendars[row.Field<string>("xawPlantID")], "WorkCenters", row));
				IResourceGroup value = new ResourceGroup(calendar, row, workCenterMachines.Select("xaqWorkCenterID = " + row.Field<string>("xawWorkCenterID").ToLinq(), "xaqWorkCenterMachineID"));
				dictionary.Add(row.Field<Guid>("xawUniqueID"), value);
			}
		}
		cache.ResourceGroups.Add(ResourceTypes.WorkCenters, dictionary);
	}

	private static void LoadProcesses(ScheduleCache cache, M1Database database)
	{
		if (cache.Processes != null)
		{
			return;
		}
		cache.Processes = new Dictionary<string, WorkProcess>(StringComparer.CurrentCultureIgnoreCase);
		foreach (DataRow row in database.GetDataTable("Select xacProcessID,xacIgnoreCalendarQueue,xacIgnoreCalendarMove from Processes Order By xacProcessID").Rows)
		{
			WorkProcess value = new WorkProcess(row.Field<string>("xacProcessID"), row.Field<bool>("xacIgnoreCalendarQueue"), row.Field<bool>("xacIgnoreCalendarMove"), 1);
			cache.Processes.Add(row.Field<string>("xacProcessID"), value);
		}
	}

	public static void LoadPlants(ScheduleCache cache, M1Database database)
	{
		LoadPlants(cache, database, null);
	}

	public static void LoadPlants(ScheduleCache cache, M1Database database, string plantID)
	{
		if (cache.PlantCalendars != null)
		{
			return;
		}
		DataTable dataTable = database.GetDataTable("Select xauUniqueID,xauPlantID,xauHoursSun,xauDayStartTimeSun,xauHoursMon,xauDayStartTimeMon,xauHoursTue,xauDayStartTimeTue,xauHoursWed,xauDayStartTimeWed,xauHoursThu,xauDayStartTimeThu,xauHoursFri,xauDayStartTimeFri,xauHoursSat,xauDayStartTimeSat From Plants Union All Select xadUniqueID,'',xadHoursSun,xadDayStartTimeSun,xadHoursMon,xadDayStartTimeMon,xadHoursTue,xadDayStartTimeTue,xadHoursWed,xadDayStartTimeWed,xadHoursThu,xadDayStartTimeThu,xadHoursFri,xadDayStartTimeFri,xadHoursSat,xadDayStartTimeSat From DatasetProperties Order By xauPlantID");
		Dictionary<string, ResourceCalendarDefinition> dictionary = new Dictionary<string, ResourceCalendarDefinition>(StringComparer.CurrentCultureIgnoreCase);
		ResourceCalendarDefinition resourceCalendarDefinition = null;
		if (resourceCalendarDefinition == null)
		{
			DataRow dataRow = (from r in dataTable?.AsEnumerable()
				where r.Field<string>("xauPlantID") == string.Empty
				select r).FirstOrDefault();
			if (dataRow != null)
			{
				ResourceCalendarDefinition resourceCalendarDefinition2 = new ResourceCalendarDefinition(resourceCalendarDefinition, "Plants", dataRow);
				resourceCalendarDefinition = resourceCalendarDefinition2;
				resourceCalendarDefinition2 = null;
			}
		}
		foreach (DataRow row in dataTable.Rows)
		{
			if (!dictionary.ContainsKey(row.Field<string>("xauPlantID")) && (plantID == null || row.Field<string>("xauPlantID").Equals(plantID, StringComparison.CurrentCultureIgnoreCase)))
			{
				dictionary.Add(value: (!string.IsNullOrWhiteSpace(row.Field<string>("xauPlantID"))) ? new ResourceCalendarDefinition(resourceCalendarDefinition, "Plants", row) : resourceCalendarDefinition, key: row.Field<string>("xauPlantID"));
			}
		}
		cache.PlantCalendars = dictionary;
	}

	private static void LoadResourcesForTimeSpan(ScheduleCache cache, DateTime start, DateTime end, byte resourceType, Guid groupID, M1Database database)
	{
		while (start <= end)
		{
			int year = start.Year;
			int month = start.Month;
			if (!cache.IsYearMonthLoaded(resourceType, groupID, year, month))
			{
				DateTime dateTime = new DateTime(year, month, 1);
				DateTime dateTime2 = dateTime.AddMonths(1);
				SqlCommand sqlCommand = database.NewSqlCommand("Select ScheduleAllocations.* From ScheduleAllocations Inner Join ScheduleResourceLanes On sxdScheduleTreeID=sxrScheduleTreeID And sxdScheduleBranchID=sxrScheduleBranchID And sxdScheduleResourceLaneID=sxrScheduleResourceLaneID Inner Join ScheduleTrees On sxdScheduleTreeID=sxtScheduleTreeID Inner Join Jobs On jmpUniqueID = sxtSourceUniqueID Inner Join JobOperations On jmpJobID = jmoJobID And sxdScheduleBranchID = jmoJobAssemblyID And sxdScheduleTaskID = jmoJobOperationID Where sxtType = 1 And jmoProductionComplete = 0 And sxtJobScenarioID = @ScenarioID And ((sxdStartActualDateTime >= @StartTime And sxdStartActualDateTime < @EndTime) Or (sxdEndActualDateTime >= @StartTime And sxdEndActualDateTime < @EndTime)) And sxrResourceType = @ResourceType And sxdGroupUniqueID = @GroupID Order By sxdScheduleResourceLaneID");
				sqlCommand.Parameters.Add(new SqlParameter("@ScenarioID", SqlDbType.NVarChar)).Value = cache.ScenarioID;
				sqlCommand.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime)).Value = dateTime;
				sqlCommand.Parameters.Add(new SqlParameter("@EndTime", SqlDbType.DateTime)).Value = dateTime2;
				sqlCommand.Parameters.Add(new SqlParameter("@ResourceType", SqlDbType.TinyInt)).Value = resourceType;
				sqlCommand.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.UniqueIdentifier)).Value = groupID;
				DataTable dataTable = database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count != 0)
				{
					ScheduleCache.LoadedResourceCache resources = cache.GetResources(resourceType, groupID);
					ScheduleAllocation alloc;
					foreach (DataRow row in dataTable.Rows)
					{
						Guid key = row.Field<Guid>("sxdUniqueID");
						if (resources.ResourceAllocations.ContainsKey(key))
						{
							continue;
						}
						alloc = LoadSchedule.LoadAllocationFromDataRow(row, new ScheduleAllocation(cache.GetTypeBucket(1, row.Field<byte>("sxdDateType")), row.Field<Guid?>("sxdGroupUniqueID"), null));
						resources.ResourceAllocations.Add(key, alloc);
						List<ScheduleAllocation> list = (from a in resources.ResourceAllocations
							where a.Value.TreeID == alloc.TreeID && a.Value.BranchID == alloc.BranchID && a.Value.TaskID == alloc.TaskID && a.Value.ResourceLaneID == alloc.ResourceLaneID
							select a.Value into a
							orderby a.StartDate.ActualDateTime
							select a).ToList();
						if (list.Count <= 1)
						{
							continue;
						}
						ScheduleAllocation scheduleAllocation = null;
						foreach (ScheduleAllocation item in list)
						{
							item.Previous = scheduleAllocation;
							if (scheduleAllocation != null)
							{
								scheduleAllocation.Next = item;
							}
							scheduleAllocation = item;
						}
					}
				}
				cache.SetYearMonthLoaded(resourceType, groupID, year, month);
			}
			start = start.AddMonths(1);
		}
	}

	private static List<ScheduleAllocation> CreateResourceAllocationsForTimeSpan(M1Database database, ScheduleCache cache, IResourceGroup shift, ResourceCalendarDefinition calendar, DateTime startTime, DateTime endTime, List<ScheduleAllocation> allResourceOverlaps, ResourceLane lane, ScheduleTask task, ScheduleTaskBucket bucketDefinition, LocalScheduleData data)
	{
		List<ScheduleAllocation> list = new List<ScheduleAllocation>();
		Overlaps overlaps = null;
		Guid? guid = null;
		if (data != null && !data.IgnoreOtherJobsForEmployees && !shift.InfiniteCapacity)
		{
			overlaps = CheckScheduleForOtherTasks(database, cache, shift.ResourceType, shift.GroupID, startTime, endTime, data);
			allResourceOverlaps.AddRange(overlaps.AllOverlaps);
			guid = shift.ResourceGuids.Where((Guid item) => !overlaps.DistinctResources.Contains(item)).FirstOrDefault();
		}
		if (guid.HasValue)
		{
			Guid? guid2 = guid;
			Guid empty = Guid.Empty;
			if (guid2.HasValue && !(guid2.GetValueOrDefault() != empty) && overlaps != null)
			{
				List<Guid> list2 = new List<Guid>();
				DateTime dateTime = startTime;
				foreach (ScheduleAllocation item in overlaps.AllOverlaps.OrderBy((ScheduleAllocation item) => item.StartDate.ActualDateTime.Value))
				{
					if (!list2.Contains(item.ResourceUniqueID.Value))
					{
						if (item.StartDate.ActualDateTime.Value < dateTime && item.EndDate.ActualDateTime.Value > dateTime)
						{
							list2.Add(item.ResourceUniqueID.Value);
						}
						else if (item.StartDate.ActualDateTime.Value > dateTime)
						{
							bucketDefinition = CreateAllocationsForTimeSpan(list, lane, dateTime, item.StartDate.ActualDateTime.Value, item.ResourceUniqueID, item.GroupUniqueID, bucketDefinition, calendar, task);
							dateTime = item.StartDate.ActualDateTime.Value;
							list2.Clear();
						}
					}
				}
				goto IL_0225;
			}
		}
		CreateAllocationsForTimeSpan(list, lane, startTime, endTime, guid, shift.GroupID, bucketDefinition, calendar, task);
		goto IL_0225;
		IL_0225:
		return list;
	}

	private static ScheduleTaskBucket CreateAllocationsForTimeSpan(List<ScheduleAllocation> allocations, ResourceLane lane, DateTime startTime, DateTime endTime, Guid? resourceID, Guid? groupID, ScheduleTaskBucket bucket, ResourceCalendarDefinition calendar, ScheduleTask task)
	{
		DateTime dateTime = endTime;
		DateTime dateTime2 = startTime;
		while (dateTime2 > bucket.EndDate.ActualDateTime.Value && bucket.Next != null)
		{
			bucket = bucket.Next;
		}
		TimeSpan timeSpan;
		ScheduleAllocation scheduleAllocation;
		while (bucket.Next != null && bucket.Next.BucketDefinition.RequiresMachine && !(dateTime <= bucket.EndDate.ActualDateTime.Value))
		{
			dateTime = bucket.EndDate.ActualDateTime.Value;
			timeSpan = dateTime.Subtract(dateTime2);
			if (timeSpan.TotalMinutes > 0.0)
			{
				scheduleAllocation = new ScheduleAllocation(bucket.BucketDefinition, groupID, task);
				scheduleAllocation.ResourceLaneID = lane.LaneID;
				scheduleAllocation.ResourceUniqueID = resourceID;
				allocations.Add(scheduleAllocation);
				scheduleAllocation.StartDate = new ScheduleDate(dateTime2, calendar);
				scheduleAllocation.EndDate = new ScheduleDate(dateTime, calendar);
				scheduleAllocation.TotalMinutes = Convert.ToInt32(timeSpan.TotalMinutes);
			}
			dateTime2 = dateTime;
			dateTime = endTime;
			bucket = bucket.Next;
		}
		timeSpan = dateTime.Subtract(dateTime2);
		scheduleAllocation = new ScheduleAllocation(bucket.BucketDefinition, groupID, task);
		scheduleAllocation.ResourceLaneID = lane.LaneID;
		scheduleAllocation.ResourceUniqueID = resourceID;
		allocations.Add(scheduleAllocation);
		scheduleAllocation.StartDate = new ScheduleDate(dateTime2, calendar);
		scheduleAllocation.EndDate = new ScheduleDate(dateTime, calendar);
		scheduleAllocation.TotalMinutes = Convert.ToInt32(timeSpan.TotalMinutes);
		return bucket;
	}

	public static ScheduleCache LoadCache(M1Database database)
	{
		ScheduleCache scheduleCache = new ScheduleCache();
		LoadCalendarMatrix(database, scheduleCache);
		return scheduleCache;
	}

	private static CalendarOverlap GetResourceGroupForDoWTimeSpan(M1Database database, ScheduleCache cache, DayOfWeek dow, int startTimeMinutes, int endTimeMinutes, byte resourceType, Guid? groupID, string processID, List<IResourceGroup> processedResourceGroups)
	{
		List<CalendarOverlap> list = new List<CalendarOverlap>();
		if (cache.CalendarMatrix.ContainsKey(resourceType))
		{
			List<CalendarDayOfWeekInfo> list2 = cache.CalendarMatrix[resourceType][dow];
			if (list2.Count != 0)
			{
				foreach (CalendarDayOfWeekInfo item in list2)
				{
					if ((!groupID.HasValue || groupID.Value == item.ResourceGroup.GroupID) && (item.ResourceGroup.ProcessID == null || item.ResourceGroup.ProcessID.Equals(processID, StringComparison.CurrentCultureIgnoreCase)))
					{
						CalendarOverlap overlapTime = item.DayCalendar.GetOverlapTime(startTimeMinutes, endTimeMinutes, item.ResourceGroup);
						if (overlapTime.OverlapMinutes > 0)
						{
							list.Add(overlapTime);
						}
					}
				}
			}
			return (from item in list
				where item.StartTimeMinutes <= startTimeMinutes && !processedResourceGroups.Contains(item.ResourceGroup)
				orderby item.OverlapMinutes descending
				select item).FirstOrDefault();
		}
		IResourceGroup resourceGroup = cache.ResourceGroups[resourceType].Values.Where((IResourceGroup item) => (!groupID.HasValue || groupID.Value == item.GroupID) && (item.ProcessID == null || item.ProcessID.Equals(processID, StringComparison.CurrentCultureIgnoreCase)) && !processedResourceGroups.Contains(item)).FirstOrDefault();
		if (resourceGroup == null)
		{
			return null;
		}
		return new CalendarOverlap(startTimeMinutes, endTimeMinutes, resourceGroup, resourceGroup.Calendar.DayOfWeekDefaults[dow]);
	}

	private static void LoadCalendarMatrix(M1Database database, ScheduleCache cache)
	{
		cache.ScheduleTypes.Clear();
		cache.ScheduleTypes.Add(1, new ScheduleType(1));
		if (cache.PlantCalendars == null)
		{
			LoadPlants(cache, database);
		}
		if (cache.Processes == null)
		{
			LoadProcesses(cache, database);
		}
		if (!cache.ResourceGroups.ContainsKey(ResourceTypes.Shifts))
		{
			LoadShifts(database, cache);
		}
		if (!cache.ResourceGroups.ContainsKey(ResourceTypes.WorkCenters))
		{
			LoadWorkCenters(cache, database);
		}
		if (cache.CalendarMatrix == null)
		{
			cache.CalendarMatrix = new Dictionary<byte, Dictionary<DayOfWeek, List<CalendarDayOfWeekInfo>>>();
		}
		if (cache.CalendarMatrix.ContainsKey(ResourceTypes.Shifts))
		{
			return;
		}
		Dictionary<Guid, IResourceGroup> dictionary = cache.ResourceGroups[ResourceTypes.Shifts];
		Dictionary<DayOfWeek, List<CalendarDayOfWeekInfo>> dictionary2 = new Dictionary<DayOfWeek, List<CalendarDayOfWeekInfo>>();
		cache.CalendarMatrix.Add(ResourceTypes.Shifts, dictionary2);
		foreach (DayOfWeek value in Enum.GetValues(typeof(DayOfWeek)))
		{
			List<CalendarDayOfWeekInfo> list = null;
			foreach (KeyValuePair<Guid, IResourceGroup> item in dictionary)
			{
				if (!item.Value.Calendar.DayOfWeekDefaults.ContainsKey(value))
				{
					continue;
				}
				DayCalendar dayCalendar = item.Value.Calendar.DayOfWeekDefaults[value];
				if (dayCalendar != null)
				{
					CalendarDayOfWeekInfo calendarDayOfWeekInfo = new CalendarDayOfWeekInfo();
					calendarDayOfWeekInfo.ResourceGroup = item.Value;
					calendarDayOfWeekInfo.DayCalendar = dayCalendar;
					if (list == null)
					{
						list = new List<CalendarDayOfWeekInfo>();
						dictionary2.Add(value, list);
					}
					list.Add(calendarDayOfWeekInfo);
				}
			}
		}
	}

	private static void LoadShifts(M1Database database, ScheduleCache cache)
	{
		DataTable dataTable = database.GetDataTable("Select lmtShiftID,lmsPlantID,lmtDay,lmtStartTime,lmtEndTime,lmtBreak1StartTime,lmtBreak1EndTime,lmtBreak1Paid,lmtBreak2StartTime,lmtBreak2EndTime,lmtBreak2Paid,lmtBreak3StartTime,lmtBreak3EndTime,lmtBreak3Paid,lmsUniqueID From ShiftBreaks Inner Join Shifts On lmtShiftID = lmsShiftID Where lmsInactive = 0 Order By lmtShiftID,lmtDay");
		DataTable dataTable2 = database.GetDataTable("Select lmeUniqueID,lmeDefaultShiftID From Employees Where lmeDefaultShiftID <> 0 And lmeTerminationDate Is Null And lmeShopEmployee <> 0");
		LoadShifts(cache, dataTable, dataTable2);
	}

	private static void LoadShifts(ScheduleCache cache, DataTable shiftBreaksTable, DataTable resources)
	{
		Dictionary<Guid, IResourceGroup> dictionary = new Dictionary<Guid, IResourceGroup>();
		short num = 0;
		Guid guid = Guid.Empty;
		string text = string.Empty;
		List<DataRow> list = new List<DataRow>();
		foreach (DataRow row3 in shiftBreaksTable.Rows)
		{
			if (row3.Field<short>("lmtShiftID") == num)
			{
				list.Add(row3);
				continue;
			}
			if (num != 0 && list.Count != 0 && !dictionary.ContainsKey(guid))
			{
				IResourceGroup resourceGroup = new ResourceGroup(new ResourceCalendarDefinition(cache.PlantCalendars[text], "Shifts", list.ToArray()), num, guid, text);
				dictionary.Add(guid, resourceGroup);
				DataRow[] array = resources.Select("lmeDefaultShiftID = " + M1Util.ConvertToLinq(num));
				foreach (DataRow row in array)
				{
					resourceGroup.ResourceGuids.Add(row.Field<Guid>("lmeUniqueID"));
				}
			}
			num = row3.Field<short>("lmtShiftID");
			guid = row3.Field<Guid>("lmsUniqueID");
			text = row3.Field<string>("lmsPlantID");
			list.Clear();
			list.Add(row3);
		}
		if (num != 0 && list.Count != 0 && !dictionary.ContainsKey(guid))
		{
			IResourceGroup resourceGroup = new ResourceGroup(new ResourceCalendarDefinition(cache.PlantCalendars[text], "Shifts", list.ToArray()), num, guid, list[0].Field<string>("lmsPlantID"));
			dictionary.Add(guid, resourceGroup);
			DataRow[] array = resources.Select("lmeDefaultShiftID = " + M1Util.ConvertToLinq(num));
			foreach (DataRow row2 in array)
			{
				resourceGroup.ResourceGuids.Add(row2.Field<Guid>("lmeUniqueID"));
			}
		}
		cache.ResourceGroups.Add(ResourceTypes.Shifts, dictionary);
	}
}
