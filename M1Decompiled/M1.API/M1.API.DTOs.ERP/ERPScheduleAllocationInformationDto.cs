using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPScheduleAllocationInformationDto
{
	public byte sxdDateType { get; set; }

	public DateTime? sxdEndActualDateTime { get; set; }

	public DateTime? sxdEndDate { get; set; }

	public short sxdEndMinute { get; set; }

	public Guid sxdUniqueID { get; set; }

	public Guid? sxdGroupUniqueID { get; set; }

	public int sxdMinutes { get; set; }

	public Guid? sxdResourceUniqueID { get; set; }

	public byte[] sxdRowVersion { get; set; }

	public int sxdScheduleBranchID { get; set; }

	public short sxdScheduleResourceLaneID { get; set; }

	public int sxdScheduleTaskID { get; set; }

	public int sxdScheduleTreeID { get; set; }

	public byte sxdScheduleAllocationID { get; set; }

	public DateTime? sxdStartActualDateTime { get; set; }

	public DateTime? sxdStartDate { get; set; }

	public short sxdStartMinute { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
