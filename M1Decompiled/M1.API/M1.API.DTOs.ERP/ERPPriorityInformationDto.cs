using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPriorityInformationDto
{
	public string kbrCreatedBy { get; set; }

	public DateTime? kbrCreatedDate { get; set; }

	public string kbrDescription { get; set; }

	public Guid kbrUniqueID { get; set; }

	public byte[] kbrRowVersion { get; set; }

	public byte kbrPriorityID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
