using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCycleCodeInformationDto
{
	public string imdCycleCodeID { get; set; }

	public string imdCreatedBy { get; set; }

	public DateTime? imdCreatedDate { get; set; }

	public string imdDescription { get; set; }

	public Guid imdUniqueID { get; set; }

	public byte[] imdRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
