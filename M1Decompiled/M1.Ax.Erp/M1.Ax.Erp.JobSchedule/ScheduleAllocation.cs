using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace M1.Ax.Erp.JobSchedule;

[Table("ScheduleAllocations")]
[TablePrefix("sxd")]
public class ScheduleAllocation : ScheduleBaseFields
{
	private byte _AllocationID;

	public ScheduleTypeBucket BucketDefinition;

	public ScheduleAllocation Previous;

	public int CompletedMinutes;

	public int UnsqueezedCompletedMinutes;

	public bool Completed;

	public bool IgnoreProductionCalendar;

	public ScheduleAllocation Next;

	[Column("sxdScheduleAllocationID")]
	public byte AllocationID
	{
		get
		{
			return _AllocationID;
		}
		set
		{
			_AllocationID = value;
		}
	}

	[Column("sxdScheduleResourceLaneID")]
	public short ResourceLaneID { get; set; }

	[Column("sxdDateType")]
	public byte DateType { get; set; }

	[ComplexTypePrefix("Start")]
	public ScheduleDate StartDate { get; set; }

	[ComplexTypePrefix("End")]
	public ScheduleDate EndDate { get; set; }

	[Column("sxdMinutes")]
	public int TotalMinutes { get; set; }

	public int TaskMinutes { get; set; }

	[Column("sxdResourceUniqueID")]
	public Guid? ResourceUniqueID { get; set; }

	[Column("sxdGroupUniqueID")]
	public Guid? GroupUniqueID { get; set; }

	public ScheduleAllocation(ScheduleTypeBucket dateDef, Guid? groupID, ScheduleTask sourceTask)
	{
		UniqueID = Guid.NewGuid();
		SourceTask = sourceTask;
		if (dateDef != null)
		{
			DateType = dateDef.ID;
		}
		BucketDefinition = dateDef;
		GroupUniqueID = groupID;
	}

	public override string ToString()
	{
		return ((BucketDefinition == null) ? DateType.ToString() : BucketDefinition.Text) + " " + StartDate.ToString() + " " + EndDate.ToString() + " Hours = " + TaskMinutes;
	}
}
