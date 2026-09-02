using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLFiscalYearInformationDto
{
	public string glzCreatedBy { get; set; }

	public DateTime? glzCreatedDate { get; set; }

	public DateTime? glzEndDate { get; set; }

	public Guid glzUniqueID { get; set; }

	public byte[] glzRowVersion { get; set; }

	public short glzGlFiscalYearID { get; set; }

	public DateTime? glzStartDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
