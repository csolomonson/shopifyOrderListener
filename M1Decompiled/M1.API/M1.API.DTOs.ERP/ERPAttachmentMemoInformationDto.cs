using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAttachmentMemoInformationDto
{
	public string cmqAttachmentID { get; set; }

	public string cmqCreatedBy { get; set; }

	public DateTime? cmqCreatedDate { get; set; }

	public Guid cmqUniqueID { get; set; }

	public string cmqLongDescriptionRtf { get; set; }

	public string cmqLongDescriptionText { get; set; }

	public DateTime? cmqMemoDate { get; set; }

	public byte[] cmqRowVersion { get; set; }

	public short cmqAttachmentMemoID { get; set; }

	public string cmqShortDescription { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
