using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLDepartmentInformationDto
{
	public string gldGlDepartmentID { get; set; }

	public string gldCreatedBy { get; set; }

	public DateTime? gldCreatedDate { get; set; }

	public string gldDescription { get; set; }

	public Guid gldUniqueID { get; set; }

	public byte[] gldRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
