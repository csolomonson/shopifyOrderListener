using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPContactTitleInformationDto
{
	public string cmeContactTitleID { get; set; }

	public string cmeCreatedBy { get; set; }

	public DateTime? cmeCreatedDate { get; set; }

	public string cmeDescription { get; set; }

	public Guid cmeUniqueID { get; set; }

	public byte[] cmeRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
