using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLCategoryInformationDto
{
	public byte gltCategoryType { get; set; }

	public string gltGlCategoryID { get; set; }

	public string gltCreatedBy { get; set; }

	public DateTime? gltCreatedDate { get; set; }

	public string gltDescription { get; set; }

	public Guid gltUniqueID { get; set; }

	public byte gltReportSequence { get; set; }

	public byte[] gltRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
