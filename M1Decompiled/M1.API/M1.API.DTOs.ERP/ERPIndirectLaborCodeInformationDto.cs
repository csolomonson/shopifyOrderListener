using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPIndirectLaborCodeInformationDto
{
	public string lmiCreatedBy { get; set; }

	public DateTime? lmiCreatedDate { get; set; }

	public string lmiDescription { get; set; }

	public Guid lmiUniqueID { get; set; }

	public DateTime? lmiInactiveDate { get; set; }

	public string lmiIndirectLaborID { get; set; }

	public byte lmiIndirectLaborType { get; set; }

	public bool lmiInactive { get; set; }

	public byte[] lmiRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
