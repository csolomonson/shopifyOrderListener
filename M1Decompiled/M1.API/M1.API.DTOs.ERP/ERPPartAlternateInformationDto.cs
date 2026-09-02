using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartAlternateInformationDto
{
	public string imeAlternatePartID { get; set; }

	public string imeAlternatePartRevisionID { get; set; }

	public string imeComment { get; set; }

	public string imeCreatedBy { get; set; }

	public DateTime? imeCreatedDate { get; set; }

	public Guid imeUniqueID { get; set; }

	public string imePartID { get; set; }

	public string imePartRevisionID { get; set; }

	public byte[] imeRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
