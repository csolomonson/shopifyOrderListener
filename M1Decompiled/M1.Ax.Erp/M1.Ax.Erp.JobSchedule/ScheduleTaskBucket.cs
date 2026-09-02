using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace M1.Ax.Erp.JobSchedule;

[Table("ScheduleTaskBuckets")]
[TablePrefix("sxe")]
public class ScheduleTaskBucket : ScheduleBaseFields
{
	public ScheduleTaskBucket Previous;

	public ScheduleTaskBucket Next;

	public ScheduleTypeBucket BucketDefinition;

	public ScheduleDate StartDate;

	public ScheduleDate EndDate;

	public int UnsqueezedCompletedMinutes;

	public bool IgnoreProductionCalendar;

	[Column("sxeScheduleTaskBucketID")]
	public byte ID { get; set; }

	[Column("sxeScheduleTypeID")]
	public byte TypeID { get; set; }

	[Column("sxeScheduleTypeBucketID")]
	public byte TypeBucketID { get; set; }

	[Column("sxeMinutes")]
	public int Minutes { get; set; }

	[Column("sxeCompletedMinutes")]
	public int CompletedMinutes { get; set; }

	[Column("sxePercentComplete")]
	public decimal PercentComplete { get; set; }

	[Column("sxeCompleted")]
	public bool Completed { get; set; }

	public ScheduleTaskBucket(ScheduleTask sourceTask, ScheduleTypeBucket dateDef)
	{
		SourceTask = sourceTask;
		BucketDefinition = dateDef;
		TypeID = dateDef.TypeID;
		ID = dateDef.ID;
		TypeBucketID = dateDef.ID;
		UniqueID = Guid.NewGuid();
	}

	public override string ToString()
	{
		return TypeID + ", " + TypeBucketID + " - " + ((BucketDefinition == null) ? string.Empty : (BucketDefinition.Text + " - ")) + Minutes;
	}
}
