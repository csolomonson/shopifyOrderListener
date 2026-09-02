using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPContactMethodInformationDto
{
	public string kbcContactMethodID { get; set; }

	public string kbcCreatedBy { get; set; }

	public DateTime? kbcCreatedDate { get; set; }

	public string kbcDescription { get; set; }

	public Guid kbcUniqueID { get; set; }

	public byte[] kbcRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
