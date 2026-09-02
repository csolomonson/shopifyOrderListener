using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Transaction;

[Serializable]
[DataContract(Namespace = "", Name = "receipt")]
[XmlRoot(ElementName = "receipt")]
[XmlType(AnonymousType = true)]
public class BOMReceiptDto
{
	[XmlElement(ElementName = "receiptID")]
	[DataMember(Name = "receiptID", Order = 1)]
	[Required(ErrorMessage = "ReceiptID is invalid or empty.")]
	public string ReceiptID { get; set; }

	[XmlElement(ElementName = "receiptDate")]
	[DataMember(Name = "receiptDate", Order = 2)]
	[Required(ErrorMessage = "ReceiptDate is invalid or empty.")]
	public DateTime? ReceiptDate { get; set; }

	[XmlElement(ElementName = "plantDepartmentID")]
	[DataMember(Name = "plantDepartmentID", Order = 3)]
	public string PlantDepartmentID { get; set; }

	[XmlElement(ElementName = "plantID")]
	[DataMember(Name = "plantID", Order = 4)]
	public string PlantID { get; set; }

	[XmlElement(ElementName = "deliveryDocket")]
	[DataMember(Name = "deliveryDocket", Order = 5)]
	public string DeliveryDocket { get; set; }

	[XmlElement(ElementName = "supplierOrganizationID")]
	[DataMember(Name = "supplierOrganizationID", Order = 6)]
	[Required(ErrorMessage = "SupplierOrganizationID is invalid or empty.")]
	public string SupplierOrganizationID { get; set; }

	[XmlElement(ElementName = "purchaseLocationID")]
	[DataMember(Name = "purchaseLocationID", Order = 7)]
	public string PurchaseLocationID { get; set; }

	[XmlElement(ElementName = "apInvoiceLocationID")]
	[DataMember(Name = "apInvoiceLocationID", Order = 8)]
	public string ApInvoiceLocationID { get; set; }

	[XmlElement(ElementName = "apInvoiceContactID")]
	[DataMember(Name = "apInvoiceContactID", Order = 9)]
	public string ApInvoiceContactID { get; set; }

	[XmlElement(ElementName = "shippingMethodID")]
	[DataMember(Name = "shippingMethodID", Order = 10)]
	public string ShippingMethodID { get; set; }

	[XmlElement(ElementName = "receiptSubtotal")]
	[DataMember(Name = "receiptSubtotal", Order = 11)]
	public decimal ReceiptSubtotal { get; set; }

	[XmlElement(ElementName = "freightCharge")]
	[DataMember(Name = "freightCharge", Order = 12)]
	public decimal FreightCharge { get; set; }

	[XmlElement(ElementName = "receiptTotal")]
	[DataMember(Name = "receiptTotal", Order = 13)]
	public decimal ReceiptTotal { get; set; }

	[XmlElement(ElementName = "projectID")]
	[DataMember(Name = "projectID", Order = 14)]
	public string ProjectID { get; set; }

	[XmlElement(ElementName = "currencyRateID")]
	[DataMember(Name = "currencyRateID", Order = 15)]
	public string CurrencyRateID { get; set; }

	[XmlElement(ElementName = "exchangeRate")]
	[DataMember(Name = "exchangeRate", Order = 16)]
	public decimal ExchangeRate { get; set; }

	[XmlElement(ElementName = "nestlinkProcessed")]
	[DataMember(Name = "nestlinkProcessed", Order = 17)]
	public bool NestlinkProcessed { get; set; }

	[XmlElement(ElementName = "customRate")]
	[DataMember(Name = "customRate", Order = 18)]
	public bool CustomRate { get; set; }

	[XmlElement(ElementName = "reversalEntry")]
	[DataMember(Name = "reversalEntry", Order = 19)]
	public bool ReversalEntry { get; set; }

	[XmlElement(ElementName = "reversed")]
	[DataMember(Name = "reversed", Order = 20)]
	public bool Reversed { get; set; }

	[XmlElement(ElementName = "postedToGl")]
	[DataMember(Name = "postedToGl", Order = 21)]
	public bool PostedToGl { get; set; }

	[XmlElement(ElementName = "postedDate")]
	[DataMember(Name = "postedDate", Order = 22)]
	public DateTime? PostedDate { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 23)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "closedDate")]
	[DataMember(Name = "closedDate", Order = 24)]
	public DateTime? ClosedDate { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 25)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 26)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 27)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 28)]
	public byte[] RowVersion { get; set; }
}
