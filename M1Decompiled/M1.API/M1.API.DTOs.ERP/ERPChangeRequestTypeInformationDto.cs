using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPChangeRequestTypeInformationDto
{
	public string chtChangeRequestTypeID { get; set; }

	public string chtCreatedBy { get; set; }

	public DateTime? chtCreatedDate { get; set; }

	public string chtDescription { get; set; }

	public Guid chtUniqueID { get; set; }

	public DateTime? chtInactiveDate { get; set; }

	public bool chtInactive { get; set; }

	public byte[] chtRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
