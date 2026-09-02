using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProjectTypeInformationDto
{
	public string prtProjectTypeID { get; set; }

	public string prtCreatedBy { get; set; }

	public DateTime? prtCreatedDate { get; set; }

	public string prtDescription { get; set; }

	public Guid prtUniqueID { get; set; }

	public DateTime? prtInactiveDate { get; set; }

	public bool prtInactive { get; set; }

	public byte[] prtRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
