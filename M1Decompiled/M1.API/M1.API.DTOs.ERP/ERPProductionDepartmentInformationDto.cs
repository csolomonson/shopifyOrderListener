using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProductionDepartmentInformationDto
{
	public string xaeProductionDepartmentID { get; set; }

	public string xaeCreatedBy { get; set; }

	public DateTime? xaeCreatedDate { get; set; }

	public string xaeDescription { get; set; }

	public Guid xaeUniqueID { get; set; }

	public byte[] xaeRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
