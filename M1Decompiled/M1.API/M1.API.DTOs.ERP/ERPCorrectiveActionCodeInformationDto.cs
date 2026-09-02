using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCorrectiveActionCodeInformationDto
{
	public string qaoCorrectiveActionCodeID { get; set; }

	public string qaoCorrectiveActionCategoryID { get; set; }

	public string qaoCreatedBy { get; set; }

	public DateTime? qaoCreatedDate { get; set; }

	public string qaoDescription { get; set; }

	public Guid qaoUniqueID { get; set; }

	public decimal qaoHoursAllowed { get; set; }

	public byte[] qaoRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
