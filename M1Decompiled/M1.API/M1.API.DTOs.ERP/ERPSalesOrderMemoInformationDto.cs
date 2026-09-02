using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSalesOrderMemoInformationDto
{
	public string omkCreatedBy { get; set; }

	public DateTime? omkCreatedDate { get; set; }

	public Guid omkUniqueID { get; set; }

	public bool omkClosed { get; set; }

	public string omkLongDescriptionRtf { get; set; }

	public string omkLongDescriptionText { get; set; }

	public DateTime? omkMemoDate { get; set; }

	public byte[] omkRowVersion { get; set; }

	public string omkSalesOrderID { get; set; }

	public short omkSalesOrderMemoID { get; set; }

	public string omkShortDescription { get; set; }

	public bool omkShowInArInvoices { get; set; }

	public bool omkShowInSalesOrders { get; set; }

	public bool omkShowInShipments { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
