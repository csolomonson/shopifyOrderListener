using System;

namespace M1.Ax.Erp.Models;

public class SalesOrderDeliveryFields
{
	public int SalesOrderDeliveryId { get; set; }

	public string SalesOrderId { get; set; }

	public int SalesOrderLineId { get; set; }

	public string PartId { get; set; }

	public string PartRevisionId { get; set; }

	public string PartWarehouseLocationId { get; set; }

	public string PartBinId { get; set; }

	public decimal DeliveryQty { get; set; }

	public DateTime DeliveryDate { get; set; }

	public int DeliveryType { get; set; }

	public bool Firm { get; set; }

	public decimal AmountToInvoice { get; set; }

	public decimal AmountToInvoiceForeign { get; set; }

	public bool DifferentLocation { get; set; }

	public string CustomerOrganizationId { get; set; }

	public string ShipLocationId { get; set; }

	public string ShipContactId { get; set; }

	public string ShippingMethodId { get; set; }

	public string ShippingPaymentTypeId { get; set; }

	public decimal FreightAmountBase { get; set; }

	public decimal FreightAmountForeign { get; set; }

	public decimal QuantityShipped { get; set; }

	public decimal QuantityInvoiced { get; set; }

	public bool ShippedComplete { get; set; }

	public bool InvoicedComplete { get; set; }

	public bool Closed { get; set; }

	public bool RequiresInspection { get; set; }

	public decimal PurchaseUnitCostBase { get; set; }

	public bool PickInProgress { get; set; }

	public decimal PurchaseUnitCostForeign { get; set; }

	public string SupplierOrganizationID { get; set; }

	public string PurchaseLocationID { get; set; }

	public decimal QuantityReceived { get; set; }

	public bool ReceivedComplete { get; set; }

	public string AvalaraNonTaxReasonID { get; set; }

	public string CreatedBy { get; set; }

	public DateTime CreatedDate { get; set; }

	public Guid UniqueId { get; set; }

	public bool KitPart { get; set; }

	public decimal QuantityAllocated { get; set; }

	public decimal QuantityOnOrder { get; set; }

	public decimal Weight { get; set; }

	public decimal ExtendedWeight { get; set; }
}
