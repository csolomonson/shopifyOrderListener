using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCountyCodeInformationDto
{
	public string xccCountyCodeID { get; set; }

	public string xccCounty { get; set; }

	public string xccCountyCode { get; set; }

	public string xccCreatedBy { get; set; }

	public DateTime? xccCreatedDate { get; set; }

	public Guid xccUniqueID { get; set; }

	public byte[] XCCRowVersion { get; set; }

	public string xccStateCode { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
