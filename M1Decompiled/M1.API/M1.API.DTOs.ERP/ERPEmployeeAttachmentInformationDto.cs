using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeeAttachmentInformationDto
{
	public string lmaAttachmentTypeID { get; set; }

	public string lmaEmployeeAttachmentID { get; set; }

	public string lmaCreatedBy { get; set; }

	public DateTime? lmaCreatedDate { get; set; }

	public DateTime? lmaDate { get; set; }

	public string lmaEmployeeID { get; set; }

	public Guid lmaUniqueID { get; set; }

	public string lmaFileLocation { get; set; }

	public string lmaFileName { get; set; }

	public string lmaLongDescriptionRtf { get; set; }

	public string lmaLongDescriptionText { get; set; }

	public byte[] lmaRowVersion { get; set; }

	public string lmaShortDescription { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
