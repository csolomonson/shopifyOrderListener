using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLeadCompetitorInformationDto
{
	public string locCreatedBy { get; set; }

	public DateTime? locCreatedDate { get; set; }

	public Guid locUniqueID { get; set; }

	public string locLeadID { get; set; }

	public string locLeadNotesRTF { get; set; }

	public string locLeadNotesText { get; set; }

	public string locOrganizationID { get; set; }

	public string locProductName { get; set; }

	public byte[] locRowVersion { get; set; }

	public short locLeadCompetitorID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
