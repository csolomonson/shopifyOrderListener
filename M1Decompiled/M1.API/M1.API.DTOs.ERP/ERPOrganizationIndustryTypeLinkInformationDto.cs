using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPOrganizationIndustryTypeLinkInformationDto
{
	public string cmdCreatedBy { get; set; }

	public DateTime? cmdCreatedDate { get; set; }

	public Guid cmdUniqueID { get; set; }

	public string cmdIndustryTypeID { get; set; }

	public short cmdIndustryTypeLinkID { get; set; }

	public string cmdOrganizationID { get; set; }

	public byte[] cmdRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
