using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeeSOApprovalInformationDto
{
	public string lmoApprovalEmployeeID { get; set; }

	public string lmoCreatedBy { get; set; }

	public DateTime? lmoCreatedDate { get; set; }

	public string lmoEmployeeID { get; set; }

	public Guid lmoUniqueID { get; set; }

	public byte[] lmoRowVersion { get; set; }

	public byte lmoEmployeeSOApprovalID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
