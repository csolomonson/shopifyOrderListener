using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLFiscalYearBudgetAmountInformationDto
{
	public decimal glbBudgetAmount { get; set; }

	public short glbBudgetHeaderID { get; set; }

	public short glbBudgetLineID { get; set; }

	public string glbCreatedBy { get; set; }

	public DateTime? glbCreatedDate { get; set; }

	public Guid glbUniqueID { get; set; }

	public short glbGlFiscalYearID { get; set; }

	public byte glbGlFiscalYearPeriodID { get; set; }

	public byte[] glbRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
