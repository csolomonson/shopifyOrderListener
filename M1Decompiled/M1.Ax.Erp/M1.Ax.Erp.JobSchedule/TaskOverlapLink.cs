namespace M1.Ax.Erp.JobSchedule;

public class TaskOverlapLink
{
	public byte ThisDateType;

	public byte LinkDateType;

	public ScheduleTask LinkOperation;

	public int OffsetMinutes;

	public TaskOverlapLink(ScheduleTask linkOperation, byte linkDateType, byte thisDateType, int offset)
	{
		LinkDateType = linkDateType;
		LinkOperation = linkOperation;
		ThisDateType = thisDateType;
		OffsetMinutes = offset;
	}

	public override string ToString()
	{
		return "LinkOpr = " + LinkOperation.TaskID + ", LinkDate = " + LinkOperation.Buckets[LinkDateType].BucketDefinition.Text + ", ThisDate = " + LinkOperation.Buckets[ThisDateType].BucketDefinition.Text + ", Offset = " + OffsetMinutes;
	}
}
