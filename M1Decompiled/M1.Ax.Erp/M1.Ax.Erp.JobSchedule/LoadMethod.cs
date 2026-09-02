using System;
using System.Collections.Generic;
using System.Linq;
using M1.Ax.Erp.Methods;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.JobSchedule;

public class LoadMethod : LoadSchedule
{
	public ScheduleTree Load(M1Database database, Assembly asm, ScheduleCache cache)
	{
		ScheduleTree scheduleTree = new ScheduleTree(database.User.ID);
		scheduleTree.ScheduleType = 1;
		ScheduleBranch asm2 = loadAsm(database, asm, cache, scheduleTree);
		LoadAssembly(scheduleTree, asm2, cache);
		ScheduleProcess.MakeSureScheduleIsInitialized(database, cache, scheduleTree);
		return scheduleTree;
	}

	private ScheduleBranch loadAsm(M1Database database, Assembly asm, ScheduleCache cache, ScheduleTree source)
	{
		ScheduleBranch scheduleBranch = new ScheduleBranch(source, asm.AssemblyID, asm.ParentAssemblyID, asm.OverlapSourceOperationID, asm.OverlapSourceLink, asm.OverlapOperationID, asm.OverlapDestinationLink, Convert.ToInt32(asm.OverlapOffsetTime * 60.0m), (BranchOverlap)asm.AssemblyOverlap, database.User.ID);
		source.AllBranches.Add(scheduleBranch);
		foreach (Assembly subAssembly in asm.SubAssemblies)
		{
			loadAsm(database, subAssembly, cache, source);
		}
		foreach (Operation operation in asm.Operations)
		{
			source.AllTasks.Add(LoadOperation(database, operation, source, cache));
		}
		return scheduleBranch;
	}

	private ScheduleTask LoadOperation(M1Database database, Operation opr, ScheduleTree source, ScheduleCache cache)
	{
		ScheduleTask scheduleTask = new ScheduleTask();
		scheduleTask.Source = source;
		scheduleTask.BranchID = opr.AssemblyID;
		scheduleTask.TaskID = opr.OperationID;
		scheduleTask.PlantID = opr.PlantID;
		scheduleTask.PlantDepartmentID = opr.PlantDepartmentID;
		scheduleTask.ProcessID = opr.ProcessID;
		WorkProcess proc = cache.Processes[scheduleTask.ProcessID];
		scheduleTask.SetBuckets(cache, proc);
		scheduleTask.OverlapTaskID = opr.OverlapOperationID;
		scheduleTask.OverlapSourceLink = opr.OverlapSourceLink;
		scheduleTask.OverlapDestinationLink = opr.OverlapDestinationLink;
		scheduleTask.OverlapOffsetMinutes = Convert.ToInt32(opr.OverlapOffsetTime * 60.0m);
		scheduleTask.PlantCalendar = cache.PlantCalendars[scheduleTask.PlantID];
		IResourceGroup resourceGroup = ScheduleProcess.GetResourceGroup(database, cache, ScheduleProcess.ResourceTypes.WorkCenters, opr.WorkCenterID);
		short num = 0;
		Guid? lockedResourceUniqueID = null;
		if (opr.MachineType == 2)
		{
			num = (short)resourceGroup.ResourceGuids.Count;
		}
		else if (opr.MachineType == 1)
		{
			num = opr.MachinesToSchedule;
		}
		if (num < 1)
		{
			num = 1;
		}
		if (opr.MachineType == 3)
		{
			short workCenterMachineID = opr.WorkCenterMachineID;
			if (workCenterMachineID > 0 && resourceGroup.ResourceGuids.Count >= workCenterMachineID)
			{
				lockedResourceUniqueID = resourceGroup.ResourceGuids[workCenterMachineID - 1];
			}
		}
		short peoplePerMachineSetup = resourceGroup.PeoplePerMachineSetup;
		short peoplePerMachineProduction = resourceGroup.PeoplePerMachineProduction;
		ScheduleTaskBucket scheduleTaskBucket = scheduleTask.Buckets[ScheduleType.QueueStart];
		ScheduleTaskBucket scheduleTaskBucket2 = scheduleTask.Buckets[ScheduleType.SetupStart];
		ScheduleTaskBucket scheduleTaskBucket3 = scheduleTask.Buckets[ScheduleType.ProductionStart];
		ScheduleTaskBucket scheduleTaskBucket4 = scheduleTask.Buckets[ScheduleType.ProductionEnd];
		_ = scheduleTask.Buckets[ScheduleType.MoveEnd];
		scheduleTaskBucket2.Minutes = (int)(opr.SetupHours * 60.0m);
		scheduleTaskBucket3.Minutes = (int)(opr.EstimatedProductionHours * 60.0m);
		if (num > 0)
		{
			scheduleTaskBucket3.Minutes = (int)M1Math.Round((decimal)scheduleTaskBucket3.Minutes / (decimal)num, 0);
		}
		if (scheduleTaskBucket2.UnsqueezedCompletedMinutes > 0 || scheduleTaskBucket3.UnsqueezedCompletedMinutes > 0)
		{
			scheduleTaskBucket.Completed = true;
		}
		if (opr.OperationQuantity == 0m)
		{
			scheduleTaskBucket.Minutes = 0;
			scheduleTaskBucket4.Minutes = 0;
			scheduleTaskBucket2.Minutes = 0;
			scheduleTaskBucket3.Minutes = 0;
			scheduleTask.TotalRemainingHours = default(decimal);
		}
		else
		{
			scheduleTaskBucket.Minutes = (int)(opr.QueueTime * 60.0m);
			scheduleTaskBucket4.Minutes = (int)(opr.MoveTime * 60.0m);
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
		scheduleTask.SetResources(num, resourceGroup.GroupID, ScheduleProcess.ResourceTypes.WorkCenters);
		if (lockedResourceUniqueID.HasValue)
		{
			List<KeyValuePair<short, ResourceLane>> list = scheduleTask.ResourceLanes.Where((KeyValuePair<short, ResourceLane> item) => item.Value.ResourceType == ScheduleProcess.ResourceTypes.WorkCenters).ToList();
			if (list.Count == 1)
			{
				list[0].Value.LockedResourceUniqueID = lockedResourceUniqueID;
			}
		}
		scheduleTask.SetResources((short)(Math.Max(peoplePerMachineSetup, peoplePerMachineProduction) * num), null, ScheduleProcess.ResourceTypes.Shifts);
		return scheduleTask;
	}
}
