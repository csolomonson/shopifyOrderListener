using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPChangeRequestGroupInformationDto
{
	public string chgChangeRequestGroupID { get; set; }

	public string chgCreatedBy { get; set; }

	public DateTime? chgCreatedDate { get; set; }

	public string chgDescription { get; set; }

	public Guid chgUniqueID { get; set; }

	public DateTime? chgInactiveDate { get; set; }

	public bool chgInactive { get; set; }

	public byte[] chgRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
