using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSalesOrderApprovalInformationDto
{
	public string omaApprovalEmployeeID { get; set; }

	public string omaCreatedBy { get; set; }

	public DateTime? omaCreatedDate { get; set; }

	public string omaDescription { get; set; }

	public Guid omaUniqueID { get; set; }

	public byte[] omaRowVersion { get; set; }

	public string omaSalesOrderID { get; set; }

	public byte omaSalesOrderApprovalID { get; set; }

	public byte omaStatus { get; set; }

	public DateTime? omaStatusDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
