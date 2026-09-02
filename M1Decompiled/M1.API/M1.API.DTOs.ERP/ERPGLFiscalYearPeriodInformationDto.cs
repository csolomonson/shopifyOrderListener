using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLFiscalYearPeriodInformationDto
{
	public string glfCreatedBy { get; set; }

	public DateTime? glfCreatedDate { get; set; }

	public DateTime? glfEndDate { get; set; }

	public Guid glfUniqueID { get; set; }

	public short glfGlFiscalYearID { get; set; }

	public bool glfApClosed { get; set; }

	public bool glfArClosed { get; set; }

	public bool glfGlClosed { get; set; }

	public byte[] glfRowVersion { get; set; }

	public byte glfGlFiscalYearPeriodID { get; set; }

	public DateTime? glfStartDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
