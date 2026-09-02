using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWorkCenterMemoInformationDto
{
	public string xakCreatedBy { get; set; }

	public DateTime? xakCreatedDate { get; set; }

	public Guid xakUniqueID { get; set; }

	public string xakLongDescriptionRtf { get; set; }

	public string xakLongDescriptionText { get; set; }

	public DateTime? xakMemoDate { get; set; }

	public byte[] xakRowVersion { get; set; }

	public short xakWorkCenterMemoID { get; set; }

	public string xakShortDescription { get; set; }

	public bool xakShowInJobs { get; set; }

	public bool xakShowInParts { get; set; }

	public bool xakShowInQuotes { get; set; }

	public bool xakShowInWorkCenters { get; set; }

	public string xakWorkCenterID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
