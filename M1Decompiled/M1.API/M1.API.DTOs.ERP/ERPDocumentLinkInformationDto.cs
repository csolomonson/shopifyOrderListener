using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPDocumentLinkInformationDto
{
	public string xalAddedByUserID { get; set; }

	public DateTime? xalAddedDate { get; set; }

	public string xalCloudFileId { get; set; }

	public string xalCreatedBy { get; set; }

	public DateTime? xalCreatedDate { get; set; }

	public string xalDescription { get; set; }

	public Guid xalUniqueID { get; set; }

	public DateTime? xalFileLastModifiedDate { get; set; }

	public string xalFileName { get; set; }

	public string xalFileNameWhenUploaded { get; set; }

	public bool xalEmailDefault { get; set; }

	public bool xalPrintDefault { get; set; }

	public string xalReference { get; set; }

	public byte[] xalRowVersion { get; set; }

	public int xalDocumentLinkID { get; set; }

	public string xalType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
