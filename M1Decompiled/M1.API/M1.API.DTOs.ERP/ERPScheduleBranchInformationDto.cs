using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPScheduleBranchInformationDto
{
	public string sxbCreatedBy { get; set; }

	public DateTime? sxbCreatedDate { get; set; }

	public byte sxbCurrentLinkedTaskDateType { get; set; }

	public int sxbCurrentLinkedTaskID { get; set; }

	public Guid sxbUniqueID { get; set; }

	public int sxbOffsetMinutes { get; set; }

	public byte sxbParentLinkedTaskDateType { get; set; }

	public int sxbParentLinkedTaskID { get; set; }

	public int sxbParentScheduleBranchID { get; set; }

	public byte[] sxbRowVersion { get; set; }

	public int sxbScheduleTreeID { get; set; }

	public int sxbScheduleBranchID { get; set; }

	public byte sxbSiblingBranchLink { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
