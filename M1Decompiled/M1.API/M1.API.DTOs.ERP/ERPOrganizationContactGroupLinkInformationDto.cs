using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPOrganizationContactGroupLinkInformationDto
{
	public string cmrContactGroupID { get; set; }

	public short cmrContactGroupLinkID { get; set; }

	public string cmrContactID { get; set; }

	public string cmrCreatedBy { get; set; }

	public DateTime? cmrCreatedDate { get; set; }

	public Guid cmrUniqueID { get; set; }

	public string cmrLocationID { get; set; }

	public string cmrOrganizationID { get; set; }

	public byte[] cmrRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
