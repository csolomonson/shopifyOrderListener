using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace M1.API.DTOs.EDI;

[Serializable]
[DataContract(Namespace = "", Name = "edI810Invoice")]
public class EDI810OutboundInvoice
{
	public string EDI810InvoiceElementName
	{
		get
		{
			return InvoiceNumber;
		}
		set
		{
			value = InvoiceNumber;
		}
	}

	[DataMember(Name = "invoiceNumber", Order = 1)]
	public string InvoiceNumber { get; set; }

	[DataMember(Name = "invoiceDate", Order = 2)]
	public string InvoiceDate { get; set; }

	[DataMember(Name = "dueDate", Order = 3)]
	public string DueDate { get; set; }

	[DataMember(Name = "customerOrganizationID", Order = 4)]
	public string CustomerOrganizationID { get; set; }

	[DataMember(Name = "shipFromLocation", Order = 5)]
	public OrganizationLocationAddressDto ShipFromLocation { get; set; }

	[DataMember(Name = "shipToLocation", Order = 6)]
	public EDIOrganizationLocationAddressDto ShipToLocation { get; set; }

	[DataMember(Name = "billToLocation", Order = 7)]
	public EDIOrganizationLocationAddressDto BillToLocation { get; set; }

	[DataMember(Name = "paymentTerm", Order = 8)]
	public string PaymentTerm { get; set; }

	[DataMember(Name = "shippingMethod", Order = 9)]
	public string ShippingMethod { get; set; }

	[DataMember(Name = "freeOnBoardDescription", Order = 10)]
	public string FreeOnBoardDescription { get; set; }

	[DataMember(Name = "currencyCode", Order = 11)]
	public string CurrencyCode { get; set; }

	[DataMember(Name = "edI810InvoiceLines", Order = 12)]
	public List<EDI810OutboundInvoiceLinesDto> EDI810InvoiceLines { get; set; }

	[DataMember(Name = "numberOfLineItems", Order = 13)]
	public int NumberOfLineItems { get; set; }

	[DataMember(Name = "totalQuantity", Order = 14)]
	public decimal TotalQuantity { get; set; }

	[DataMember(Name = "freightAmountForeign", Order = 15)]
	public decimal FreightAmountForeign { get; set; }

	[DataMember(Name = "freightTaxAmountForeign", Order = 16)]
	public decimal FreightTaxAmountForeign { get; set; }

	[DataMember(Name = "finalInvAmt", Order = 17)]
	public decimal FinalInvAmt { get; set; }

	[DataMember(Name = "invoiceBalanceForeign", Order = 18)]
	public decimal InvoiceBalanceForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public byte InvoiceType { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal FullInvoiceSubtotalForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal DiscountTotalForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal InvoiceSubtotalForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal FreightTotalForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal InvoiceTaxAmountForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal InvoiceTotalForeign { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool EDITransferred { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public DateTime EDITransferredDate { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public IList<string> ProcessingErrorsList { get; set; }
}
