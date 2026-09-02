using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPJobPriorityInformationDto
{
	public string jmjDescription { get; set; }

	public Guid jmjUniqueID { get; set; }

	public byte[] jmjRowVersion { get; set; }

	public short jmjJobPriorityID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
