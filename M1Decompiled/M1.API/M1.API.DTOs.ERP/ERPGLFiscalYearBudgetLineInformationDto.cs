using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPGLFiscalYearBudgetLineInformationDto
{
	public decimal glgAnnualAmount { get; set; }

	public short glgBudgetHeaderID { get; set; }

	public short glgBudgetLineID { get; set; }

	public string glgCreatedBy { get; set; }

	public DateTime? glgCreatedDate { get; set; }

	public Guid glgUniqueID { get; set; }

	public short glgGlFiscalYearID { get; set; }

	public byte[] glgRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
