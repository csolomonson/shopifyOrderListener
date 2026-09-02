using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeeQAApprovalInformationDto
{
	public string lmbApprovalEmployeeID { get; set; }

	public string lmbCreatedBy { get; set; }

	public DateTime? lmbCreatedDate { get; set; }

	public string lmbEmployeeID { get; set; }

	public Guid lmbUniqueID { get; set; }

	public byte[] lmbRowVersion { get; set; }

	public byte lmbEmployeeQAApprovalID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
