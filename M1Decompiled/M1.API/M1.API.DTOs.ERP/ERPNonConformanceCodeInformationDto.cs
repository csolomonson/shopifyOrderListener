using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPNonConformanceCodeInformationDto
{
	public string qacNonConformanceCodeID { get; set; }

	public string qacCreatedBy { get; set; }

	public DateTime? qacCreatedDate { get; set; }

	public string qacDescription { get; set; }

	public Guid qacUniqueID { get; set; }

	public string qacNonConformanceCategoryID { get; set; }

	public byte[] qacRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
