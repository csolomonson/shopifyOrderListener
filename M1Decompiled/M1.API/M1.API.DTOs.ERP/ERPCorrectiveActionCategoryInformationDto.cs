using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCorrectiveActionCategoryInformationDto
{
	public string qatCorrectiveActionCategoryID { get; set; }

	public string qatCreatedBy { get; set; }

	public DateTime? qatCreatedDate { get; set; }

	public string qatDescription { get; set; }

	public Guid qatUniqueID { get; set; }

	public byte[] qatRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
