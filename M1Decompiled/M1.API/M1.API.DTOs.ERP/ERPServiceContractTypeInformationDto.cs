using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPServiceContractTypeInformationDto
{
	public string kbyServiceContractTypeID { get; set; }

	public string kbyCreatedBy { get; set; }

	public DateTime? kbyCreatedDate { get; set; }

	public string kbyDescription { get; set; }

	public Guid kbyUniqueID { get; set; }

	public DateTime? kbyInactiveDate { get; set; }

	public bool kbyInactive { get; set; }

	public byte[] kbyRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
