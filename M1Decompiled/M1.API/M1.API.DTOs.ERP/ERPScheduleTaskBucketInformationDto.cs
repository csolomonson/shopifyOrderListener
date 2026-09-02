using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPScheduleTaskBucketInformationDto
{
	public int sxeCompletedMinutes { get; set; }

	public Guid sxeUniqueID { get; set; }

	public bool sxeCompleted { get; set; }

	public int sxeMinutes { get; set; }

	public int sxePercentComplete { get; set; }

	public byte[] sxeRowVersion { get; set; }

	public int sxeScheduleBranchID { get; set; }

	public int sxeScheduleTaskID { get; set; }

	public int sxeScheduleTreeID { get; set; }

	public byte sxeScheduleTypeBucketID { get; set; }

	public byte sxeScheduleTypeID { get; set; }

	public byte sxeScheduleTaskBucketID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
