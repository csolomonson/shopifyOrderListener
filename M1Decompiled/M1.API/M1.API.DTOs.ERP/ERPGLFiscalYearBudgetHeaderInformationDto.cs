using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLFiscalYearBudgetHeaderInformationDto
{
	public decimal glkAnnualAmount { get; set; }

	public short glkBudgetHeaderID { get; set; }

	public string glkCreatedBy { get; set; }

	public DateTime? glkCreatedDate { get; set; }

	public Guid glkUniqueID { get; set; }

	public string glkGlAccountID { get; set; }

	public short glkGlFiscalYearID { get; set; }

	public byte[] glkRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
