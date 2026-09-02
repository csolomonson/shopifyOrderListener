using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPStandardMessageInformationDto
{
	public string xamStandardMessageID { get; set; }

	public string xamCreatedBy { get; set; }

	public DateTime? xamCreatedDate { get; set; }

	public Guid xamUniqueID { get; set; }

	public DateTime? xamInactiveDate { get; set; }

	public bool xamInactive { get; set; }

	public string xamLongDescriptionRtf { get; set; }

	public string xamLongDescriptionText { get; set; }

	public byte xamMessageType { get; set; }

	public byte[] xamRowVersion { get; set; }

	public string xamShortDescription { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
