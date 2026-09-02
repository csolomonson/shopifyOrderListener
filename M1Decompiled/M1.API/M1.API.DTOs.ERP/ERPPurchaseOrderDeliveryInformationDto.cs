using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPurchaseOrderDeliveryInformationDto
{
	public string pmdContactID { get; set; }

	public string pmdCreatedBy { get; set; }

	public DateTime? pmdCreatedDate { get; set; }

	public DateTime? pmdDeliveryDate { get; set; }

	public decimal pmdDeliveryQuantity { get; set; }

	public byte pmdDeliveryType { get; set; }

	public Guid pmdUniqueID { get; set; }

	public bool pmdClosed { get; set; }

	public bool pmdInTransit { get; set; }

	public bool pmdInvoicedComplete { get; set; }

	public bool pmdReceivedComplete { get; set; }

	public int pmdJobAssemblyID { get; set; }

	public string pmdJobID { get; set; }

	public int pmdJobMaterialID { get; set; }

	public int pmdJobOperationID { get; set; }

	public byte pmdJobType { get; set; }

	public string pmdLocationID { get; set; }

	public string pmdOrganizationID { get; set; }

	public string pmdPurchaseOrderID { get; set; }

	public short pmdPurchaseOrderLineID { get; set; }

	public decimal pmdQuantityInvoiced { get; set; }

	public decimal pmdQuantityReceived { get; set; }

	public byte[] pmdRowVersion { get; set; }

	public short pmdPurchaseOrderDeliveryID { get; set; }

	public string pmdShippingMethodID { get; set; }

	public string pmdTrackingNumber { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
