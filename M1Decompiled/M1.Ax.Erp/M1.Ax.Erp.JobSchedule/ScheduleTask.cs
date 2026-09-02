using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace M1.Ax.Erp.JobSchedule;

[Table("ScheduleTasks")]
[TablePrefix("sxk")]
public class ScheduleTask : IDisposable, IEntityCreated, IEntityUniqueID
{
	public ScheduleBranch ParentBranch;

	private ScheduleTree _Source;

	public ScheduleDate CurrentStart;

	public Dictionary<byte, ScheduleTaskBucket> Buckets = new Dictionary<byte, ScheduleTaskBucket>();

	public Dictionary<short, ResourceLane> ResourceLanes = new Dictionary<short, ResourceLane>();

	public decimal TotalRemainingHours;

	public List<TaskOverlapLink> Overlaps = new List<TaskOverlapLink>();

	public ScheduleTaskCollection ParentTaskCollection;

	public ScheduleBranchCollection Branches = new ScheduleBranchCollection();

	public ScheduleTask PreviousTask;

	public ScheduleTask NextTask;

	public ScheduleTaskBucket FirstBucket;

	public ScheduleTaskBucket LastBucket;

	public bool Changed;

	public ScheduleTree Source
	{
		get
		{
			return _Source;
		}
		set
		{
			if (_Source != value)
			{
				_Source = value;
				TreeID = _Source.TreeID;
			}
		}
	}

	[Column("sxkScheduleTreeID")]
	public int TreeID { get; set; }

	[Column("sxkScheduleBranchID")]
	public int BranchID { get; set; }

	[Column("sxkScheduleTaskID")]
	public int TaskID { get; set; }

	[Column("sxkPlantID")]
	public string PlantID { get; set; }

	[Column("sxkPlantDepartmentID")]
	public string PlantDepartmentID { get; set; }

	[Column("sxkProcessID")]
	public string ProcessID { get; set; }

	public ResourceCalendarDefinition PlantCalendar { get; set; }

	[Column("sxkScheduleTypeID")]
	public byte TypeID { get; set; }

	[Column("sxkCurrentTaskDateType")]
	public byte OverlapSourceLink { get; set; }

	[Column("sxkLinkedTaskDateType")]
	public byte OverlapDestinationLink { get; set; }

	[Column("sxkOffsetMinutes")]
	public int OverlapOffsetMinutes { get; set; }

	[Column("sxkLinkedTaskID")]
	public int OverlapTaskID { get; set; }

	[ComplexTypePrefix("Start")]
	public ScheduleDate StartDate { get; set; }

	[ComplexTypePrefix("End")]
	public ScheduleDate EndDate { get; set; }

	[Column("sxkCreatedBy")]
	public string CreatedBy { get; set; }

	[Column("sxkCreatedDate")]
	public DateTime? CreatedDate { get; set; }

	[Column("sxkUniqueID")]
	public Guid? UniqueID { get; set; }

	[Column("sxkExchangeID")]
	public string ExchangeID { get; set; }

	public void ClearDates()
	{
		foreach (KeyValuePair<short, ResourceLane> resourceLane in ResourceLanes)
		{
			resourceLane.Value.Allocations.Clear();
		}
	}

	public void SetResources(short count, Guid? groupUniqueID, byte resourceType)
	{
		for (short num = 1; num <= count; num++)
		{
			short num2 = (short)ResourceLanes.Count;
			ResourceLanes.Add(num2, new ResourceLane(num2, groupUniqueID, resourceType, this, null));
		}
	}

	public ScheduleTask()
	{
		CreatedDate = DateTime.Now;
		SetResources(1, null, 0);
	}

	public void SetFirstLastBuckets()
	{
		ScheduleTaskBucket scheduleTaskBucket = null;
		foreach (KeyValuePair<byte, ScheduleTaskBucket> bucket in Buckets)
		{
			if (FirstBucket == null)
			{
				FirstBucket = bucket.Value;
			}
			bucket.Value.Previous = scheduleTaskBucket;
			scheduleTaskBucket = bucket.Value;
		}
		LastBucket = scheduleTaskBucket;
		ScheduleTaskBucket scheduleTaskBucket2 = scheduleTaskBucket;
		while (scheduleTaskBucket2.Previous != null)
		{
			scheduleTaskBucket2.Previous.Next = scheduleTaskBucket2;
			scheduleTaskBucket2 = scheduleTaskBucket2.Previous;
		}
	}

	public void SetBuckets(ScheduleCache cache, WorkProcess proc)
	{
		TypeID = proc.TypeID;
		foreach (ScheduleTypeBucket item in cache.ScheduleTypes[proc.TypeID])
		{
			Buckets.Add(item.ID, new ScheduleTaskBucket(this, item));
		}
		SetFirstLastBuckets();
		Buckets[ScheduleType.QueueStart].IgnoreProductionCalendar = proc.IgnoreCalendarQueue;
		Buckets[ScheduleType.ProductionEnd].IgnoreProductionCalendar = proc.IgnoreCalendarMove;
		Buckets[ScheduleType.MoveEnd].IgnoreProductionCalendar = true;
	}

	public override string ToString()
	{
		return string.Format("{0}: {1}, Hours = {2}, Start = {3}, End = {4}", BranchID + "-" + TaskID, (PlantCalendar == null) ? "null" : PlantCalendar.PlantID.ToString(), TotalRemainingHours.ToString("N2"), (StartDate == null) ? "null" : StartDate.ToString(), (EndDate == null) ? "null" : EndDate.ToString());
	}

	public void Dispose()
	{
		Overlaps = null;
		ParentTaskCollection = null;
		ParentBranch = null;
		PreviousTask = null;
		NextTask = null;
		Branches = null;
		PlantCalendar = null;
	}
}
