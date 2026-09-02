using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProjectAreaInformationDto
{
	public string praProjectAreaID { get; set; }

	public string praCreatedBy { get; set; }

	public DateTime? praCreatedDate { get; set; }

	public string praDescription { get; set; }

	public Guid praUniqueID { get; set; }

	public string praProjectID { get; set; }

	public byte[] praRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
