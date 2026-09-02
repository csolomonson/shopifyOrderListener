using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPContactGroupInformationDto
{
	public string cmgContactGroupID { get; set; }

	public string cmgCreatedBy { get; set; }

	public DateTime? cmgCreatedDate { get; set; }

	public string cmgDescription { get; set; }

	public Guid cmgUniqueID { get; set; }

	public byte[] cmgRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
