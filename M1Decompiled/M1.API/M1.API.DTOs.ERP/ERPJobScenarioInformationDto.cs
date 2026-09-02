using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPJobScenarioInformationDto
{
	public string jmnJobScenarioID { get; set; }

	public string jmnCreatedBy { get; set; }

	public DateTime? jmnCreatedDate { get; set; }

	public string jmnDescription { get; set; }

	public Guid jmnUniqueID { get; set; }

	public byte[] jmnRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
