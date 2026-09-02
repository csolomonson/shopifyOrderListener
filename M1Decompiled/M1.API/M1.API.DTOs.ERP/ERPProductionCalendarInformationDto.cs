using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProductionCalendarInformationDto
{
	public string jmlCreatedBy { get; set; }

	public DateTime? jmlCreatedDate { get; set; }

	public Guid jmlUniqueID { get; set; }

	public string jmlPlantID { get; set; }

	public short jmlProductionCalendarYearID { get; set; }

	public byte[] jmlRowVersion { get; set; }

	public string jmlWorkCenterID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
