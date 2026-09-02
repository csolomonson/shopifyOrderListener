using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeePOApprovalInformationDto
{
	public string lmhApprovalEmployeeID { get; set; }

	public string lmhCreatedBy { get; set; }

	public DateTime? lmhCreatedDate { get; set; }

	public string lmhEmployeeID { get; set; }

	public Guid lmhUniqueID { get; set; }

	public byte[] lmhRowVersion { get; set; }

	public byte lmhEmployeePoApprovalID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
