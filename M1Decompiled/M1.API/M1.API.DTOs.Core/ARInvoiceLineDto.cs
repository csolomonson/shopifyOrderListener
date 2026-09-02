namespace M1.API.DTOs.Core;

public class ARInvoiceLineDto
{
	public string ARInvoiceID { get; set; }

	public string PartID { get; set; }

	public string PartRevisionID { get; set; }

	public string UnitOfMeasure { get; set; }

	public string PartShortDescription { get; set; }

	public string TaxCodeID { get; set; }

	public string NonTaxReasonID { get; set; }

	public string SalesOrderID { get; set; }

	public string CustomerPO { get; set; }

	public string OrgPartID { get; set; }

	public string OrgPartShortDescription { get; set; }

	public decimal OrderQuantity { get; set; }

	public decimal InvoiceQuantity { get; set; }

	public decimal UnitPriceForeign { get; set; }

	public short ARInvoiceLineID { get; set; }

	public short SalesOrderLineID { get; set; }

	public short SalesOrderDeliveryID { get; set; }

	public string ShipmentID { get; set; }

	public decimal FreightAmountForeign { get; set; }

	public decimal FullUnitPriceForeign { get; set; }

	public decimal FullExtendedPriceForeign { get; set; }

	public decimal ExtendedDiscountForeign { get; set; }

	public decimal ExtendedPriceForeign { get; set; }

	public decimal TaxAmountForeign { get; set; }

	public decimal SecondTaxAmountForeign { get; set; }
}
