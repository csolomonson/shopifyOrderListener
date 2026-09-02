using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeeSalesBudgetInformationDto
{
	public decimal lnsAnnualAmount { get; set; }

	public string lnsEmployeeID { get; set; }

	public DateTime? lnsEndDate { get; set; }

	public Guid lnsUniqueID { get; set; }

	public byte[] lnsRowVersion { get; set; }

	public short lnsSalesBudgetYearID { get; set; }

	public DateTime? lnsStartDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
