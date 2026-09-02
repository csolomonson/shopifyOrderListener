using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCallTypeInformationDto
{
	public string kbtCallStatus { get; set; }

	public string kbtCallTypeID { get; set; }

	public string kbtCreatedBy { get; set; }

	public DateTime? kbtCreatedDate { get; set; }

	public string kbtDescription { get; set; }

	public Guid kbtUniqueID { get; set; }

	public DateTime? kbtInactiveDate { get; set; }

	public bool kbtInactive { get; set; }

	public bool kbtBillableCall { get; set; }

	public bool kbtFieldServiceCall { get; set; }

	public bool kbtInboundCall { get; set; }

	public bool kbtInternalOnlyCall { get; set; }

	public byte[] kbtRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
