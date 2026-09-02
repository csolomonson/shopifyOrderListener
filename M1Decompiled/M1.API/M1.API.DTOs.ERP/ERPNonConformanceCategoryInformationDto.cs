using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPNonConformanceCategoryInformationDto
{
	public string qagNonConformanceCategoryID { get; set; }

	public string qagCreatedBy { get; set; }

	public DateTime? qagCreatedDate { get; set; }

	public string qagDescription { get; set; }

	public Guid qagUniqueID { get; set; }

	public byte[] qagRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
