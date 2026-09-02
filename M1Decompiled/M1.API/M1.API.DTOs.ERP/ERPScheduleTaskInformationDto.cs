using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPScheduleTaskInformationDto
{
	public string sxkCreatedBy { get; set; }

	public DateTime? sxkCreatedDate { get; set; }

	public byte sxkCurrentTaskDateType { get; set; }

	public DateTime? sxkEndActualDateTime { get; set; }

	public DateTime? sxkEndDate { get; set; }

	public short sxkEndMinute { get; set; }

	public Guid sxkUniqueID { get; set; }

	public string sxkExchangeID { get; set; }

	public byte sxkLinkedTaskDateType { get; set; }

	public int sxkLinkedTaskID { get; set; }

	public int sxkMinutes { get; set; }

	public int sxkOffsetMinutes { get; set; }

	public string sxkPlantDepartmentID { get; set; }

	public string sxkPlantID { get; set; }

	public string sxkProcessID { get; set; }

	public byte[] sxkRowVersion { get; set; }

	public int sxkScheduleBranchID { get; set; }

	public int sxkScheduleTreeID { get; set; }

	public byte sxkScheduleTypeID { get; set; }

	public int sxkScheduleTaskID { get; set; }

	public DateTime? sxkStartActualDateTime { get; set; }

	public DateTime? sxkStartDate { get; set; }

	public short sxkStartMinute { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
