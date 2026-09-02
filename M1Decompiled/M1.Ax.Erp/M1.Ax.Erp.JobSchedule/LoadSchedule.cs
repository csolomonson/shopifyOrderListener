using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.JobSchedule;

public class LoadSchedule
{
	public ScheduleTree Load(M1Database database, int treeID, ScheduleCache cache)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select sxbScheduleBranchID,sxbParentScheduleBranchID,sxbCurrentLinkedTaskID,sxbParentLinkedTaskID,sxbCurrentLinkedTaskDateType,sxbParentLinkedTaskDateType,sxbOffsetMinutes,sxbSiblingBranchLink From ScheduleBranches Where sxbScheduleTreeID = @TreeID Order By sxbScheduleBranchID");
		sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = treeID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			sqlCommand = database.NewSqlCommand("Select ScheduleTrees.*,jmpJobID From ScheduleTrees Left Outer Join Jobs On jmpUniqueID=sxtSourceUniqueID Where sxtScheduleTreeID = @TreeID");
			sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = treeID;
			DataTable dataTable2 = database.GetDataTable(sqlCommand);
			sqlCommand = database.NewSqlCommand("Select * From ScheduleTasks Where sxkScheduleTreeID = @TreeID Order By sxkScheduleBranchID,sxkScheduleTaskID");
			sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = treeID;
			DataTable dataTable3 = database.GetDataTable(sqlCommand);
			sqlCommand = database.NewSqlCommand("Select * From ScheduleResourceLanes Where sxrScheduleTreeID = @TreeID Order By sxrScheduleBranchID,sxrScheduleTaskID,sxrScheduleResourceLaneID");
			sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = treeID;
			DataTable dataTable4 = database.GetDataTable(sqlCommand);
			sqlCommand = database.NewSqlCommand("Select * From ScheduleResourceCells Where sxcTreeID = @TreeID Order By sxcBranchID,sxcTaskID");
			sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = treeID;
			DataTable dataTable5 = database.GetDataTable(sqlCommand);
			sqlCommand = database.NewSqlCommand("Select * From ScheduleTaskBuckets Where sxeScheduleTreeID = @TreeID Order By sxeScheduleBranchID,sxeScheduleTaskID");
			sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = treeID;
			DataTable dataTable6 = database.GetDataTable(sqlCommand);
			sqlCommand = database.NewSqlCommand("Select * From ScheduleAllocations Where sxdScheduleTreeID = @TreeID Order By sxdScheduleBranchID,sxdScheduleTaskID,sxdScheduleResourceLaneID");
			sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int)).Value = treeID;
			DataTable dataTable7 = database.GetDataTable(sqlCommand);
			ScheduleTree scheduleTree = new ScheduleTree(database.User.ID);
			scheduleTree.TreeID = treeID;
			if (dataTable2.Rows.Count != 0)
			{
				DataRow dataRow = dataTable2.Rows[0];
				scheduleTree.Description = dataRow.Field<string>("sxtDescription");
				object[] sourceKeyValues = new string[1] { dataRow.Field<string>("jmpJobID") };
				scheduleTree.SourceKeyValues = sourceKeyValues;
				scheduleTree.SourceUniqueID = dataRow.Field<Guid>("sxtSourceUniqueID");
				if (!dataRow.IsNull("sxtGroupUniqueID"))
				{
					scheduleTree.GroupUniqueID = dataRow.Field<Guid>("sxtGroupUniqueID");
				}
				scheduleTree.SourceAssembliesTable = "JobAssemblies";
				scheduleTree.ScheduleType = dataRow.Field<byte>("sxtType");
				scheduleTree.JobScenarioID = dataRow.Field<string>("sxtJobScenarioID");
			}
			scheduleTree.SourceTable = "Jobs";
			foreach (DataRow row5 in dataTable.Rows)
			{
				scheduleTree.AllBranches.Add(new ScheduleBranch(scheduleTree, row5.Field<int>("sxbScheduleBranchID"), row5.Field<int>("sxbParentScheduleBranchID"), row5.Field<int>("sxbCurrentLinkedTaskID"), row5.Field<byte>("sxbCurrentLinkedTaskDateType"), row5.Field<int>("sxbParentLinkedTaskID"), row5.Field<byte>("sxbParentLinkedTaskDateType"), row5.Field<int>("sxbOffsetMinutes"), row5.Field<BranchOverlap>("sxbSiblingBranchLink"), database.User.ID));
			}
			foreach (DataRow row6 in dataTable3.Rows)
			{
				ScheduleTask scheduleTask = LoadTask(database, row6, dataTable5.Select("sxcBranchID = " + M1Util.ConvertToLinq(row6.Field<int>("sxkScheduleBranchID")) + " And sxcTaskID = " + M1Util.ConvertToLinq(row6.Field<int>("sxkScheduleTaskID")), "sxcResourceLaneID,sxcResourceCellID"), dataTable6.Select("sxeScheduleBranchID = " + M1Util.ConvertToLinq(row6.Field<int>("sxkScheduleBranchID")) + " And sxeScheduleTaskID = " + M1Util.ConvertToLinq(row6.Field<int>("sxkScheduleTaskID")), "sxeScheduleTaskBucketID"), scheduleTree, cache);
				scheduleTree.AllTasks.Add(scheduleTask);
				scheduleTask.ResourceLanes.Clear();
				DataRow[] array = dataTable4.Select("sxrScheduleBranchID = " + M1Util.ConvertToLinq(row6.Field<int>("sxkScheduleBranchID")) + " And sxrScheduleTaskID = " + M1Util.ConvertToLinq(row6.Field<int>("sxkScheduleTaskID")), "sxrScheduleResourceLaneID");
				foreach (DataRow row3 in array)
				{
					ResourceLane resourceLane = new ResourceLane(row3.Field<short>("sxrScheduleResourceLaneID"), row3.Field<Guid?>("sxrGroupUniqueID"), row3.Field<byte>("sxrResourceType"), scheduleTask, null);
					resourceLane.UniqueID = row3.Field<Guid>("sxrUniqueID");
					resourceLane.LockedResourceUniqueID = row3.Field<Guid?>("sxrLockedResourceUniqueID");
					scheduleTask.ResourceLanes.Add(resourceLane.LaneID, resourceLane);
					DataRow[] array2 = dataTable7.Select("sxdScheduleBranchID = " + M1Util.ConvertToLinq(row3.Field<int>("sxrScheduleBranchID")) + " And sxdScheduleTaskID = " + M1Util.ConvertToLinq(row3.Field<int>("sxrScheduleTaskID")) + " And sxdScheduleResourceLaneID = " + M1Util.ConvertToLinq(row3.Field<short>("sxrScheduleResourceLaneID")), "sxdScheduleResourceLaneID,sxdScheduleAllocationID");
					foreach (DataRow row4 in array2)
					{
						resourceLane.Allocations.Add(LoadAllocationFromDataRow(row4, new ScheduleAllocation(cache.GetTypeBucket(1, row4.Field<byte>("sxdDateType")), row4.Field<Guid?>("sxdGroupUniqueID"), null)));
					}
				}
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

	public virtual ScheduleTree Load(M1Database database, object[] sourceKeyValues, ScheduleCache cache)
	{
		return null;
	}

	private ScheduleTask LoadTask(M1Database database, DataRow row, DataRow[] cellRows, DataRow[] bucketRows, ScheduleTree source, ScheduleCache cache)
	{
		ScheduleTask scheduleTask = new ScheduleTask();
		scheduleTask.Source = source;
		scheduleTask.BranchID = row.Field<int>("sxkScheduleBranchID");
		scheduleTask.TaskID = row.Field<int>("sxkScheduleTaskID");
		scheduleTask.PlantID = row.Field<string>("sxkPlantID");
		scheduleTask.PlantDepartmentID = row.Field<string>("sxkPlantDepartmentID");
		scheduleTask.ProcessID = row.Field<string>("sxkProcessID");
		scheduleTask.OverlapTaskID = row.Field<int>("sxkLinkedTaskID");
		scheduleTask.OverlapSourceLink = row.Field<byte>("sxkCurrentTaskDateType");
		scheduleTask.OverlapDestinationLink = row.Field<byte>("sxkLinkedTaskDateType");
		scheduleTask.OverlapOffsetMinutes = row.Field<int>("sxkOffsetMinutes");
		scheduleTask.TypeID = row.Field<byte>("sxkScheduleTypeID");
		scheduleTask.PlantCalendar = cache.PlantCalendars[row.Field<string>("sxkPlantID")];
		scheduleTask.CreatedBy = row.Field<string>("sxkCreatedBy");
		scheduleTask.CreatedDate = row.Field<DateTime>("sxkCreatedDate");
		scheduleTask.UniqueID = row.Field<Guid>("sxkUniqueID");
		scheduleTask.Buckets.Clear();
		foreach (DataRow row2 in bucketRows)
		{
			ScheduleTypeBucket typeBucket = cache.GetTypeBucket(row2.Field<byte>("sxeScheduleTypeID"), row2.Field<byte>("sxeScheduleTypeBucketID"));
			ScheduleTaskBucket scheduleTaskBucket = new ScheduleTaskBucket(scheduleTask, typeBucket);
			scheduleTaskBucket.Minutes = row2.Field<int>("sxeMinutes");
			scheduleTaskBucket.Completed = row2.Field<bool>("sxeCompleted");
			scheduleTaskBucket.CompletedMinutes = row2.Field<int>("sxeCompletedMinutes");
			scheduleTaskBucket.UniqueID = row2.Field<Guid>("sxeUniqueID");
			scheduleTask.Buckets.Add(typeBucket.ID, scheduleTaskBucket);
		}
		scheduleTask.SetFirstLastBuckets();
		scheduleTask.StartDate = new ScheduleDate(row.Field<DateTime?>("sxkStartActualDateTime"), scheduleTask.PlantCalendar);
		scheduleTask.EndDate = new ScheduleDate(row.Field<DateTime?>("sxkEndActualDateTime"), scheduleTask.PlantCalendar);
		return scheduleTask;
	}

	protected void LoadAssembly(ScheduleTree source, ScheduleBranch asm, ScheduleCache cache)
	{
		asm.Tasks.AddRange(from item in source.AllTasks
			where item.BranchID == asm.BranchID
			orderby item.TaskID
			select item);
		asm.CurrentAndSubTasks.AddRange(asm.Tasks);
		asm.CurrentAndSubBranches.Add(asm);
		if (asm.Tasks.Count != 0)
		{
			ScheduleTask scheduleTask = null;
			foreach (ScheduleTask task in asm.Tasks)
			{
				task.ParentBranch = asm;
				task.PreviousTask = scheduleTask;
				scheduleTask = task;
			}
			asm.CurrentBranchLinkedTask = scheduleTask;
			if (asm.CurrentBranchLinkedTaskID != 0)
			{
				asm.CurrentBranchLinkedTask = asm.Tasks[asm.CurrentBranchLinkedTaskID];
			}
			while (scheduleTask.PreviousTask != null)
			{
				scheduleTask.PreviousTask.NextTask = scheduleTask;
				scheduleTask = scheduleTask.PreviousTask;
			}
		}
		foreach (ScheduleBranch item in from item in source.AllBranches
			where item.ParentBranchID == asm.BranchID && item.BranchID != 0
			orderby item.BranchID
			select item)
		{
			asm.Branches.Add(item);
			item.ParentBranch = asm;
			LoadAssembly(source, item, cache);
			asm.CurrentAndSubTasks.AddRange(item.CurrentAndSubTasks);
			asm.CurrentAndSubBranches.AddRange(item.CurrentAndSubBranches);
			if (!CheckInterAsmLink(asm, item))
			{
				asm.UnlinkedBranches.Add(item);
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
		asm.DistinctTaskLists = new List<ScheduleTaskCollection>();
		ScheduleTaskCollection scheduleTaskCollection = new ScheduleTaskCollection();
		asm.DistinctTaskLists.Add(scheduleTaskCollection);
		ScheduleTask overlapOpr;
		foreach (ScheduleTask opr in asm.Tasks)
		{
			if (opr.OverlapSourceLink == 0 && opr.OverlapDestinationLink != 0)
			{
				opr.OverlapSourceLink = opr.OverlapDestinationLink;
			}
			else if (opr.OverlapDestinationLink == 0 && opr.OverlapSourceLink != 0)
			{
				opr.OverlapDestinationLink = opr.OverlapSourceLink;
			}
			if (opr.OverlapTaskID != 0 && opr.OverlapTaskID != opr.TaskID && opr.OverlapSourceLink == 0 && opr.OverlapDestinationLink == 0)
			{
				if (opr.OverlapTaskID < opr.TaskID)
				{
					opr.OverlapSourceLink = opr.Buckets.First().Value.TypeBucketID;
					opr.OverlapDestinationLink = opr.Buckets.Last().Value.TypeBucketID;
				}
				else
				{
					opr.OverlapSourceLink = opr.Buckets.Last().Value.TypeBucketID;
					opr.OverlapDestinationLink = opr.Buckets.First().Value.TypeBucketID;
				}
			}
			scheduleTaskCollection.Add(opr);
			opr.ParentTaskCollection = scheduleTaskCollection;
			if (opr.OverlapSourceLink != 0 && opr.OverlapDestinationLink != 0 && opr.TaskID != opr.OverlapTaskID)
			{
				if (opr.OverlapTaskID == 0 || !asm.Tasks.Contains(opr.OverlapTaskID))
				{
					if (opr.PreviousTask == null)
					{
						overlapOpr = (from item in asm.Tasks
							where item.TaskID < opr.TaskID
							orderby item.TaskID descending
							select item).FirstOrDefault();
					}
					else
					{
						overlapOpr = opr.PreviousTask;
					}
				}
				else
				{
					overlapOpr = asm.Tasks[opr.OverlapTaskID];
				}
				if (overlapOpr != null && checkOperationOverlap(opr, opr.OverlapSourceLink, overlapOpr, opr.OverlapDestinationLink, opr.OverlapOffsetMinutes))
				{
					if (opr.TaskID < overlapOpr.TaskID)
					{
						if (opr.NextTask != null)
						{
							opr.NextTask.PreviousTask = null;
							opr.NextTask = null;
						}
						scheduleTaskCollection = new ScheduleTaskCollection();
					}
					else
					{
						ScheduleTaskCollection scheduleTaskCollection2 = new ScheduleTaskCollection();
						if (scheduleTaskCollection.Contains(overlapOpr.TaskID))
						{
							List<ScheduleTask> list = (from item in scheduleTaskCollection.ToList()
								where item.TaskID > overlapOpr.TaskID
								orderby item.TaskID
								select item).ToList();
							if (list.Count != scheduleTaskCollection.Count)
							{
								foreach (ScheduleTask item2 in list)
								{
									scheduleTaskCollection2.Add(item2);
									item2.ParentTaskCollection = scheduleTaskCollection2;
									scheduleTaskCollection.Remove(item2);
								}
								if (list[0].PreviousTask != null)
								{
									list[0].PreviousTask.NextTask = null;
									list[0].PreviousTask = null;
								}
								scheduleTaskCollection = scheduleTaskCollection2;
							}
						}
					}
					if (!asm.DistinctTaskLists.Contains(scheduleTaskCollection))
					{
						asm.DistinctTaskLists.Add(scheduleTaskCollection);
					}
				}
			}
			foreach (ScheduleBranch branch in opr.Branches)
			{
				if (branch.CurrentBranchLinkedTask != null)
				{
					if (branch.OverlapSourceLink == 0 || branch.OverlapDestinationLink == 0)
					{
						checkOperationOverlap(opr, opr.Buckets.First().Value.TypeBucketID, branch.CurrentBranchLinkedTask, branch.CurrentBranchLinkedTask.Buckets.Last().Value.TypeBucketID, branch.OverlapOffsetMinutes);
					}
					else
					{
						checkOperationOverlap(opr, branch.OverlapDestinationLink, branch.CurrentBranchLinkedTask, branch.OverlapSourceLink, branch.OverlapOffsetMinutes);
					}
				}
			}
		}
	}

	public static bool CheckInterAsmLink(ScheduleBranch asm, ScheduleBranch subAsm)
	{
		if (asm.Tasks.Contains(subAsm.ParentBranchTaskID))
		{
			if (!asm.Tasks[subAsm.ParentBranchTaskID].Branches.Contains(subAsm))
			{
				asm.Tasks[subAsm.ParentBranchTaskID].Branches.Add(subAsm);
			}
		}
		else if (asm.Tasks.Count != 0)
		{
			if (!asm.Tasks.First().Branches.Contains(subAsm))
			{
				asm.Tasks.First().Branches.Add(subAsm);
			}
		}
		else
		{
			if (asm.ParentBranch == null)
			{
				return false;
			}
			CheckInterAsmLink(asm.ParentBranch, subAsm);
		}
		return true;
	}

	public static bool checkOperationOverlap(ScheduleTask opr, byte sourceLink, ScheduleTask otherOpr, byte destinationLink, int offsetMinutes)
	{
		if (opr.Overlaps.Find((TaskOverlapLink item) => item.LinkOperation == otherOpr) == null && otherOpr.Overlaps.Find((TaskOverlapLink item) => item.LinkOperation == opr) == null)
		{
			opr.Overlaps.Add(new TaskOverlapLink(otherOpr, destinationLink, sourceLink, -offsetMinutes));
			otherOpr.Overlaps.Add(new TaskOverlapLink(opr, sourceLink, destinationLink, offsetMinutes));
			return true;
		}
		return false;
	}

	public static ScheduleAllocation LoadAllocationFromDataRow(DataRow row, ScheduleAllocation scheduleAllocation)
	{
		scheduleAllocation.ResourceUniqueID = row.Field<Guid?>("sxdResourceUniqueID");
		scheduleAllocation.GroupUniqueID = row.Field<Guid?>("sxdGroupUniqueID");
		scheduleAllocation.ResourceLaneID = row.Field<short>("sxdScheduleResourceLaneID");
		scheduleAllocation.StartDate = new ScheduleDate(row.Field<DateTime?>("sxdStartDate"), row.Field<short>("sxdStartMinute"), row.Field<DateTime?>("sxdStartActualDateTime"));
		scheduleAllocation.TotalMinutes = row.Field<int>("sxdMinutes");
		scheduleAllocation.TaskMinutes = scheduleAllocation.TotalMinutes;
		scheduleAllocation.EndDate = new ScheduleDate(row.Field<DateTime?>("sxdEndDate"), row.Field<short>("sxdEndMinute"), row.Field<DateTime?>("sxdEndActualDateTime"));
		scheduleAllocation.TreeID = row.Field<int>("sxdScheduleTreeID");
		scheduleAllocation.BranchID = row.Field<int>("sxdScheduleBranchID");
		scheduleAllocation.TaskID = row.Field<int>("sxdScheduleTaskID");
		scheduleAllocation.UniqueID = row.Field<Guid>("sxdUniqueID");
		return scheduleAllocation;
	}
}
