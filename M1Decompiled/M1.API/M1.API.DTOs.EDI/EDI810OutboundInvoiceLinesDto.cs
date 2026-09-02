using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[Serializable]
[DataContract(Namespace = "", Name = "edI810InvoiceLine")]
public class EDI810OutboundInvoiceLinesDto
{
	[DataMember(Name = "invoiceLineID", Order = 1)]
	public short InvoiceLineID { get; set; }

	[DataMember(Name = "customerPO", Order = 2)]
	public string CustomerPO { get; set; }

	[DataMember(Name = "salesOrderID", Order = 3)]
	public string SalesOrderID { get; set; }

	[DataMember(Name = "orderDate", Order = 4)]
	public string OrderDate { get; set; }

	[DataMember(Name = "salesOrderLineID", Order = 5)]
	public short SalesOrderLineID { get; set; }

	[DataMember(Name = "shipDate", Order = 6)]
	public string ShipDate { get; set; }

	[DataMember(Name = "shipmentTrackingNumber", Order = 7)]
	public string ShipmentTrackingNumber { get; set; }

	[DataMember(Name = "releaseNumber", Order = 8)]
	public string ReleaseNumber { get; set; }

	[DataMember(Name = "shipmentID", Order = 9)]
	public string ShipmentID { get; set; }

	[DataMember(Name = "shipmentLineID", Order = 10)]
	public short ShipmentLineID { get; set; }

	[DataMember(Name = "vendorItemNo", Order = 11)]
	public string VendorItemNo { get; set; }

	[DataMember(Name = "orgPartID", Order = 12)]
	public string OrgPartID { get; set; }

	[DataMember(Name = "partShortDescription", Order = 13)]
	public string PartShortDescription { get; set; }

	[DataMember(Name = "orderQuantity", Order = 14)]
	public decimal OrderQuantity { get; set; }

	[DataMember(Name = "unitOfMeasure", Order = 15)]
	public string UnitOfMeasure { get; set; }

	[DataMember(Name = "itemPrice", Order = 16)]
	public decimal ItemPrice { get; set; }

	[DataMember(Name = "edICreatedSalesOrder", Order = 17)]
	public bool EDICreatedSalesOrder { get; set; }

	[DataMember(Name = "edI810SACLines", Order = 18)]
	public List<EDI810OutboundInvoiceSACLineDto> EDI810SACLines { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string PartID { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string InvoiceNumber { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public short SalesOrderDeliveryID { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal FullUnitPriceForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal ExtendedDiscountForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal UnitPriceForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal FullExtendedPriceForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal ExtendedPriceForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal TaxAmountForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal FreightAmountForeign { get; set; }
}
