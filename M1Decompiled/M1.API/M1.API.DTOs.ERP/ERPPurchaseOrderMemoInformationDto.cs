using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchaseOrderMemoInformationDto
{
	public string pmkCreatedBy { get; set; }

	public DateTime? pmkCreatedDate { get; set; }

	public Guid pmkUniqueID { get; set; }

	public bool pmkClosed { get; set; }

	public string pmkLongDescriptionRtf { get; set; }

	public string pmkLongDescriptionText { get; set; }

	public DateTime? pmkMemoDate { get; set; }

	public string pmkPurchaseOrderID { get; set; }

	public byte[] pmkRowVersion { get; set; }

	public short pmkPurchaseOrderMemoID { get; set; }

	public string pmkShortDescription { get; set; }

	public bool pmkShowInApInvoices { get; set; }

	public bool pmkShowInPurchaseOrders { get; set; }

	public bool pmkShowInReceipts { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
