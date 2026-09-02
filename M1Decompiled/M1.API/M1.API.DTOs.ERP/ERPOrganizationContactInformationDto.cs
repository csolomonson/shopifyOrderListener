using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPOrganizationContactInformationDto
{
	public string cmcAlternatePhoneNumber { get; set; }

	public string cmcContactID { get; set; }

	public string cmcContactTitleID { get; set; }

	public string cmcCorrespondenceMethod { get; set; }

	public string cmcCreatedBy { get; set; }

	public DateTime? cmcCreatedDate { get; set; }

	public string cmcEmailAddress { get; set; }

	public Guid cmcUniqueID { get; set; }

	public string cmcFaxNumber { get; set; }

	public DateTime? cmcInactiveDate { get; set; }

	public bool cmcInactive { get; set; }

	public bool cmcCreatedFromMobile { get; set; }

	public bool cmcNoMailings { get; set; }

	public string cmcLocationID { get; set; }

	public string cmcMobileNumber { get; set; }

	public string cmcName { get; set; }

	public string cmcNoteRtf { get; set; }

	public string cmcNoteText { get; set; }

	public string cmcOrganizationID { get; set; }

	public string cmcPhoneNumber { get; set; }

	public byte[] cmcRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
