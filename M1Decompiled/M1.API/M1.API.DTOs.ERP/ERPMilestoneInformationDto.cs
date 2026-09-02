using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMilestoneInformationDto
{
	public string losMilestoneID { get; set; }

	public decimal losConfidenceFactor { get; set; }

	public string losCreatedBy { get; set; }

	public DateTime? losCreatedDate { get; set; }

	public Guid losUniqueID { get; set; }

	public string losLongDescriptionRtf { get; set; }

	public string losLongDescriptionText { get; set; }

	public byte[] losRowVersion { get; set; }

	public string losShortDescription { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
