using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMarketingProgramInformationDto
{
	public string looActivityType { get; set; }

	public string looMarketingProgramID { get; set; }

	public string looCreatedBy { get; set; }

	public DateTime? looCreatedDate { get; set; }

	public DateTime? looEndDate { get; set; }

	public Guid looUniqueID { get; set; }

	public decimal looExpectedRevenue { get; set; }

	public DateTime? looInactiveDate { get; set; }

	public bool looInactive { get; set; }

	public string looLongDescriptionRtf { get; set; }

	public string looLongDescriptionText { get; set; }

	public decimal looMarketingCost { get; set; }

	public byte[] looRowVersion { get; set; }

	public string looShortDescription { get; set; }

	public DateTime? looStartDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
