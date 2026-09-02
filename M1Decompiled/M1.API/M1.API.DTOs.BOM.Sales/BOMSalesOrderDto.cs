using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Sales;

[Serializable]
[DataContract(Namespace = "", Name = "salesorder")]
[XmlRoot(ElementName = "salesorder")]
[XmlType(AnonymousType = true)]
public class BOMSalesOrderDto
{
	[XmlElement(ElementName = "salesOrderID")]
	[DataMember(Name = "salesOrderID", Order = 1)]
	[Required(ErrorMessage = "SalesOrderID is invalid or empty.")]
	public string SalesOrderID { get; set; }

	[XmlElement(ElementName = "customerOrganizationID")]
	[DataMember(Name = "customerOrganizationID", Order = 2)]
	[Required(ErrorMessage = "CustomerOrganizationID is invalid or empty.")]
	public string CustomerOrganizationID { get; set; }

	[XmlElement(ElementName = "shipOrganizationID")]
	[DataMember(Name = "shipOrganizationID", Order = 3)]
	[Required(ErrorMessage = "ShipOrganizationID is invalid or empty.")]
	public string ShipOrganizationID { get; set; }

	[XmlElement(ElementName = "customerPo")]
	[DataMember(Name = "customerPo", Order = 4)]
	public string CustomerPo { get; set; }

	[XmlElement(ElementName = "plantID")]
	[DataMember(Name = "plantID", Order = 5)]
	public string PlantID { get; set; }

	[XmlElement(ElementName = "plantDepartmentID")]
	[DataMember(Name = "plantDepartmentID", Order = 6)]
	public string PlantDepartmentID { get; set; }

	[XmlElement(ElementName = "shipLocationID")]
	[DataMember(Name = "shipLocationID", Order = 7)]
	public string ShipLocationID { get; set; }

	[XmlElement(ElementName = "shipContactID")]
	[DataMember(Name = "shipContactID", Order = 8)]
	public string ShipContactID { get; set; }

	[XmlElement(ElementName = "arInvoiceLocationID")]
	[DataMember(Name = "arInvoiceLocationID", Order = 9)]
	public string ArInvoiceLocationID { get; set; }

	[XmlElement(ElementName = "arInvoiceContactID")]
	[DataMember(Name = "arInvoiceContactID", Order = 10)]
	public string ArInvoiceContactID { get; set; }

	[XmlElement(ElementName = "paymentTermID")]
	[DataMember(Name = "paymentTermID", Order = 11)]
	public string PaymentTermID { get; set; }

	[XmlElement(ElementName = "currencyRateID")]
	[DataMember(Name = "currencyRateID", Order = 12)]
	public string CurrencyRateID { get; set; }

	[XmlElement(ElementName = "exchangeRate")]
	[DataMember(Name = "exchangeRate", Order = 13)]
	public decimal ExchangeRate { get; set; }

	[XmlElement(ElementName = "fullOrderSubtotalBase")]
	[DataMember(Name = "fullOrderSubtotalBase", Order = 14)]
	public decimal FullOrderSubtotalBase { get; set; }

	[XmlElement(ElementName = "requestedShipDate")]
	[DataMember(Name = "requestedShipDate", Order = 15)]
	public DateTime? RequestedShipDate { get; set; }

	[XmlElement(ElementName = "orderDate")]
	[DataMember(Name = "orderDate", Order = 16)]
	[Required(ErrorMessage = "OrderDate is invalid or empty.")]
	public DateTime? OrderDate { get; set; }

	[XmlElement(ElementName = "orderTotalBase")]
	[DataMember(Name = "orderTotalBase", Order = 17)]
	public decimal OrderTotalBase { get; set; }

	[XmlElement(ElementName = "orderTotalForeign")]
	[DataMember(Name = "orderTotalForeign", Order = 18)]
	public decimal OrderTotalForeign { get; set; }

	[XmlElement(ElementName = "status")]
	[DataMember(Name = "status", Order = 19)]
	[Required(ErrorMessage = "Status is invalid or empty.")]
	public byte Status { get; set; }

	[XmlElement(ElementName = "customRate")]
	[DataMember(Name = "customRate", Order = 20)]
	public bool CustomRate { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 21)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "closedDate")]
	[DataMember(Name = "closedDate", Order = 22)]
	public DateTime? ClosedDate { get; set; }

	[XmlElement(ElementName = "freightAmountBase")]
	[DataMember(Name = "freightAmountBase", Order = 23)]
	public decimal FreightAmountBase { get; set; }

	[XmlElement(ElementName = "freightAmountForeign")]
	[DataMember(Name = "freightAmountForeign", Order = 24)]
	public decimal FreightAmountForeign { get; set; }

	[XmlElement(ElementName = "freightTaxAmountBase")]
	[DataMember(Name = "freightTaxAmountBase", Order = 25)]
	public decimal FreightTaxAmountBase { get; set; }

	[XmlElement(ElementName = "freightTaxAmountForeign")]
	[DataMember(Name = "freightTaxAmountForeign", Order = 26)]
	public decimal FreightTaxAmountForeign { get; set; }

	[XmlElement(ElementName = "freightTaxCodeID")]
	[DataMember(Name = "freightTaxCodeID", Order = 27)]
	public string FreightTaxCodeID { get; set; }

	[XmlElement(ElementName = "freightTotalBase")]
	[DataMember(Name = "freightTotalBase", Order = 28)]
	public decimal FreightTotalBase { get; set; }

	[XmlElement(ElementName = "freightTotalForeign")]
	[DataMember(Name = "freightTotalForeign", Order = 29)]
	public decimal FreightTotalForeign { get; set; }

	[XmlElement(ElementName = "orderSubtotalBase")]
	[DataMember(Name = "orderSubtotalBase", Order = 30)]
	public decimal OrderSubtotalBase { get; set; }

	[XmlElement(ElementName = "orderSubTotalForeign")]
	[DataMember(Name = "orderSubTotalForeign", Order = 31)]
	public decimal OrderSubTotalForeign { get; set; }

	[XmlElement(ElementName = "orderTaxAmountBase")]
	[DataMember(Name = "orderTaxAmountBase", Order = 32)]
	public decimal OrderTaxAmountBase { get; set; }

	[XmlElement(ElementName = "orderTaxAmountForeign")]
	[DataMember(Name = "orderTaxAmountForeign", Order = 33)]
	public decimal OrderTaxAmountForeign { get; set; }

	[XmlElement(ElementName = "shippingMethodID")]
	[DataMember(Name = "shippingMethodID", Order = 34)]
	public string ShippingMethodID { get; set; }

	[XmlElement(ElementName = "shippingPaymentTypeID")]
	[DataMember(Name = "shippingPaymentTypeID", Order = 35)]
	public string ShippingPaymentTypeID { get; set; }

	[XmlElement(ElementName = "totalOrderWeight")]
	[DataMember(Name = "totalOrderWeight", Order = 36)]
	public decimal TotalOrderWeight { get; set; }
}
