using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCustomerGroupInformationDto
{
	public string cmuCustomerGroupID { get; set; }

	public string cmuCreatedBy { get; set; }

	public DateTime? cmuCreatedDate { get; set; }

	public string cmuDescription { get; set; }

	public Guid cmuUniqueID { get; set; }

	public byte[] cmuRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
