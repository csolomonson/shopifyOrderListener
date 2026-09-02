using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProductionCalendarWorkCenterInformationDto
{
	public string jmrCreatedBy { get; set; }

	public DateTime? jmrCreatedDate { get; set; }

	public Guid jmrUniqueID { get; set; }

	public short jmrProductionCalendarLineID { get; set; }

	public short jmrProductionCalendarYearID { get; set; }

	public byte[] jmrRowVersion { get; set; }

	public string jmrWorkCenterID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
