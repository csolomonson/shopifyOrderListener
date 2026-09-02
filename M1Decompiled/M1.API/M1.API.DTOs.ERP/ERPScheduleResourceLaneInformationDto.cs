using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPScheduleResourceLaneInformationDto
{
	public Guid sxrUniqueID { get; set; }

	public Guid? sxrGroupUniqueID { get; set; }

	public Guid? sxrLockedResourceUniqueID { get; set; }

	public byte sxrResourceType { get; set; }

	public byte[] sxrRowVersion { get; set; }

	public int sxrScheduleBranchID { get; set; }

	public int sxrScheduleTaskID { get; set; }

	public int sxrScheduleTreeID { get; set; }

	public short sxrScheduleResourceLaneID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
