using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLDivisionInformationDto
{
	public string glvGlDivisionID { get; set; }

	public string glvCreatedBy { get; set; }

	public DateTime? glvCreatedDate { get; set; }

	public string glvDescription { get; set; }

	public Guid glvUniqueID { get; set; }

	public string glvRetainedEarningsAccountID { get; set; }

	public byte[] glvRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
