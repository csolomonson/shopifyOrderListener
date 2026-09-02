using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAttachmentTypeInformationDto
{
	public string cmtAttachmentTypeID { get; set; }

	public string cmtCreatedBy { get; set; }

	public DateTime? cmtCreatedDate { get; set; }

	public string cmtDescription { get; set; }

	public Guid cmtUniqueID { get; set; }

	public bool cmtRequiresLogin { get; set; }

	public bool cmtRequiresServiceContract { get; set; }

	public byte[] cmtRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
