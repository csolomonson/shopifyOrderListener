using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPChangeRequestGroupLinkInformationDto
{
	public string chrChangeRequestGroupID { get; set; }

	public string chrChangeRequestID { get; set; }

	public string chrCreatedBy { get; set; }

	public DateTime? chrCreatedDate { get; set; }

	public Guid chrUniqueID { get; set; }

	public byte[] chrRowVersion { get; set; }

	public short chrChangeRequestGroupLinkID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
