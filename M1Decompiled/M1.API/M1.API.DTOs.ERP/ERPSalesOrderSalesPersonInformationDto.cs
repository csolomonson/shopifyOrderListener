using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSalesOrderSalesPersonInformationDto
{
	public string omiCreatedBy { get; set; }

	public DateTime? omiCreatedDate { get; set; }

	public Guid omiUniqueID { get; set; }

	public bool omiClosed { get; set; }

	public decimal omiPercent { get; set; }

	public byte[] omiRowVersion { get; set; }

	public string omiSalesEmployeeID { get; set; }

	public string omiSalesOrderID { get; set; }

	public short omiSequenceID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
