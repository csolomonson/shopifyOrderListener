using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPSalesOrderDeliveryInformationDto
{
	public decimal omdAmountToInvoice { get; set; }

	public decimal omdAmountToInvoiceForeign { get; set; }

	public string omdAvalaraNonTaxReasonID { get; set; }

	public string omdCreatedBy { get; set; }

	public DateTime? omdCreatedDate { get; set; }

	public string omdCustomerOrganizationID { get; set; }

	public DateTime? omdDeliveryDate { get; set; }

	public decimal omdDeliveryQuantity { get; set; }

	public byte omdDeliveryType { get; set; }

	public Guid omdUniqueID { get; set; }

	public decimal omdExtendedWeight { get; set; }

	public decimal omdFreightAmountBase { get; set; }

	public decimal omdFreightAmountForeign { get; set; }

	public bool omdClosed { get; set; }

	public bool omdDifferentLocation { get; set; }

	public bool omdFirm { get; set; }

	public bool omdInvoicedComplete { get; set; }

	public bool omdKitPart { get; set; }

	public bool omdPickInProgress { get; set; }

	public bool omdReceivedComplete { get; set; }

	public bool omdRequiresInspection { get; set; }

	public bool omdShippedComplete { get; set; }

	public string omdPartBinID { get; set; }

	public string omdPartID { get; set; }

	public string omdPartRevisionID { get; set; }

	public string omdPartWarehouseLocationID { get; set; }

	public string omdPurchaseLocationID { get; set; }

	public decimal omdPurchaseUnitCostBase { get; set; }

	public decimal omdPurchaseUnitCostForeign { get; set; }

	public decimal omdQuantityAllocated { get; set; }

	public decimal omdQuantityInvoiced { get; set; }

	public decimal omdQuantityOnOrder { get; set; }

	public decimal omdQuantityReceived { get; set; }

	public decimal omdQuantityShipped { get; set; }

	public byte[] omdRowVersion { get; set; }

	public string omdSalesOrderID { get; set; }

	public short omdSalesOrderLineID { get; set; }

	public short omdSalesOrderDeliveryID { get; set; }

	public string omdShipContactID { get; set; }

	public string omdShipLocationID { get; set; }

	public string omdShippingMethodID { get; set; }

	public string omdShippingPaymentTypeID { get; set; }

	public string omdSupplierOrganizationID { get; set; }

	public decimal omdWeight { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
