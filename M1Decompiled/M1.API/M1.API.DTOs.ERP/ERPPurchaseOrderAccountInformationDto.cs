using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchaseOrderAccountInformationDto
{
	public decimal pmxAmount { get; set; }

	public string pmxCreatedBy { get; set; }

	public DateTime? pmxCreatedDate { get; set; }

	public Guid pmxUniqueID { get; set; }

	public string pmxExpenseGlAccountID { get; set; }

	public bool pmxClosed { get; set; }

	public decimal pmxPercent { get; set; }

	public string pmxPurchaseOrderID { get; set; }

	public short pmxPurchaseOrderLineID { get; set; }

	public byte[] pmxRowVersion { get; set; }

	public short pmxPurchaseOrderAccountID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
