using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "organizationlocation")]
[XmlRoot(ElementName = "organizationlocation")]
[XmlType(AnonymousType = true)]
public class BOMOrganizationLocationDto
{
	[XmlElement(ElementName = "organizationID")]
	[DataMember(Name = "organizationID", Order = 1)]
	[Required(ErrorMessage = "OrganizationID is invalid or empty.")]
	public string OrganizationID { get; set; }

	[XmlElement(ElementName = "locationID")]
	[DataMember(Name = "locationID", Order = 2)]
	[Required(ErrorMessage = "Location is invalid or empty.")]
	public string LocationID { get; set; }

	[XmlElement(ElementName = "name")]
	[DataMember(Name = "name", Order = 3)]
	[Required(ErrorMessage = "Name is invalid or empty.")]
	public string Name { get; set; }

	[XmlElement(ElementName = "addressLine1")]
	[DataMember(Name = "addressLine1", Order = 4)]
	public string AddressLine1 { get; set; }

	[XmlElement(ElementName = "addressLine2")]
	[DataMember(Name = "addressLine2", Order = 5)]
	public string AddressLine2 { get; set; }

	[XmlElement(ElementName = "addressLine3")]
	[DataMember(Name = "addressLine3", Order = 6)]
	public string AddressLine3 { get; set; }

	[XmlElement(ElementName = "city")]
	[DataMember(Name = "city", Order = 7)]
	public string City { get; set; }

	[XmlElement(ElementName = "county")]
	[DataMember(Name = "county", Order = 8)]
	public string County { get; set; }

	[XmlElement(ElementName = "state")]
	[DataMember(Name = "state", Order = 9)]
	public string State { get; set; }

	[XmlElement(ElementName = "postCode")]
	[DataMember(Name = "postCode", Order = 10)]
	public string PostCode { get; set; }

	[XmlElement(ElementName = "country")]
	[DataMember(Name = "country", Order = 11)]
	public string Country { get; set; }

	[XmlElement(ElementName = "phoneNumber")]
	[DataMember(Name = "phoneNumber", Order = 12)]
	public string PhoneNumber { get; set; }

	[XmlElement(ElementName = "emailAddress")]
	[DataMember(Name = "emailAddress", Order = 13)]
	public string EmailAddress { get; set; }

	[XmlElement(ElementName = "quoteLocation")]
	[DataMember(Name = "quoteLocation", Order = 14)]
	public bool QuoteLocation { get; set; }

	[XmlElement(ElementName = "shipLocation")]
	[DataMember(Name = "shipLocation", Order = 15)]
	public bool ShipLocation { get; set; }

	[XmlElement(ElementName = "arInvoiceLocation")]
	[DataMember(Name = "arInvoiceLocation", Order = 16)]
	public bool ArInvoiceLocation { get; set; }

	[XmlElement(ElementName = "customerTaxable")]
	[DataMember(Name = "customerTaxable", Order = 17)]
	public bool CustomerTaxable { get; set; }

	[XmlElement(ElementName = "customerTaxCodeID")]
	[DataMember(Name = "customerTaxCodeID", Order = 18)]
	public string CustomerTaxCodeID { get; set; }

	[XmlElement(ElementName = "customerSecondTaxCodeID")]
	[DataMember(Name = "customerSecondTaxCodeID", Order = 19)]
	public string CustomerSecondTaxCodeID { get; set; }

	[XmlElement(ElementName = "customerShippingMethodID")]
	[DataMember(Name = "customerShippingMethodID", Order = 20)]
	public string CustomerShippingMethodID { get; set; }

	[XmlElement(ElementName = "customerShipPaymentTypeID")]
	[DataMember(Name = "customerShipPaymentTypeID", Order = 21)]
	public string CustomerShipPaymentTypeID { get; set; }

	[XmlElement(ElementName = "customerShippingCarrier")]
	[DataMember(Name = "customerShippingCarrier", Order = 22)]
	public string CustomerShippingCarrier { get; set; }

	[XmlElement(ElementName = "upsAcctNumber")]
	[DataMember(Name = "upsAcctNumber", Order = 23)]
	public string UpsAcctNumber { get; set; }

	[XmlElement(ElementName = "fedExAccountNumber")]
	[DataMember(Name = "fedExAccountNumber", Order = 24)]
	public string FedExAccountNumber { get; set; }

	[XmlElement(ElementName = "countryCode")]
	[DataMember(Name = "countryCode", Order = 25)]
	public string CountryCode { get; set; }
}
