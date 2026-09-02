using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchaseOrderApprovalInformationDto
{
	public string pmaApprovalEmployeeID { get; set; }

	public string pmaCreatedBy { get; set; }

	public DateTime? pmaCreatedDate { get; set; }

	public string pmaDescription { get; set; }

	public Guid pmaUniqueID { get; set; }

	public string pmaPurchaseOrderID { get; set; }

	public byte[] pmaRowVersion { get; set; }

	public byte pmaPurchaseOrderApprovalID { get; set; }

	public byte pmaStatus { get; set; }

	public DateTime? pmaStatusDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
