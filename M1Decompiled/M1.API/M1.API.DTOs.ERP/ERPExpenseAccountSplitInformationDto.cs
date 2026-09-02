using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPExpenseAccountSplitInformationDto
{
	public Guid xazExpenseAccountSplitID { get; set; }

	public string xazCreatedBy { get; set; }

	public DateTime? xazCreatedDate { get; set; }

	public string xazExpenseGlAccountID { get; set; }

	public string xazLandedCostCategoryID { get; set; }

	public string xazPartID { get; set; }

	public string xazPartRevisionID { get; set; }

	public decimal xazPercent { get; set; }

	public byte[] xazRowVersion { get; set; }

	public short xazSequence { get; set; }

	public string xazSupplierOrganizationID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
