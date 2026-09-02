using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPEmployeeSalesBudgetLineInformationDto
{
	public decimal lnlBudgetAmount { get; set; }

	public string lnlEmployeeID { get; set; }

	public DateTime? lnlEndDate { get; set; }

	public Guid lnlUniqueID { get; set; }

	public byte[] lnlRowVersion { get; set; }

	public short lnlSalesBudgetPeriodID { get; set; }

	public short lnlSalesBudgetYearID { get; set; }

	public DateTime? lnlStartDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
