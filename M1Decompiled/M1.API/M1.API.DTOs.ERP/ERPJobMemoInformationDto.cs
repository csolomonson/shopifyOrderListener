using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPJobMemoInformationDto
{
	public string jmkCreatedBy { get; set; }

	public DateTime? jmkCreatedDate { get; set; }

	public Guid jmkUniqueID { get; set; }

	public bool jmkClosed { get; set; }

	public string jmkJobID { get; set; }

	public string jmkLongDescriptionRtf { get; set; }

	public string jmkLongDescriptionText { get; set; }

	public DateTime? jmkMemoDate { get; set; }

	public byte[] jmkRowVersion { get; set; }

	public short jmkJobMemoID { get; set; }

	public string jmkShortDescription { get; set; }

	public bool jmkShowInJobs { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
