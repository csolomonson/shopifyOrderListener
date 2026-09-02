using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Custom;

[Serializable]
[DataContract(Namespace = "", Name = "ctmorganizationlocation")]
[XmlRoot(ElementName = "ctmorganizationlocation")]
[XmlType(AnonymousType = true)]
public class CTMOrganizationLocationDto
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

	[XmlElement(ElementName = "quoteContactID")]
	[DataMember(Name = "quoteContactID", Order = 15)]
	public string QuoteContactID { get; set; }

	[XmlElement(ElementName = "shipLocation")]
	[DataMember(Name = "shipLocation", Order = 16)]
	public bool ShipLocation { get; set; }

	[XmlElement(ElementName = "shipContactID")]
	[DataMember(Name = "shipContactID", Order = 17)]
	public string ShipContactID { get; set; }

	[XmlElement(ElementName = "arInvoiceLocation")]
	[DataMember(Name = "arInvoiceLocation", Order = 18)]
	public bool ArInvoiceLocation { get; set; }

	[XmlElement(ElementName = "arInvoiceContactID")]
	[DataMember(Name = "arInvoiceContactID", Order = 19)]
	public string ArInvoiceContactID { get; set; }

	[XmlElement(ElementName = "purchaseLocation")]
	[DataMember(Name = "purchaseLocation", Order = 20)]
	public bool PurchaseLocation { get; set; }

	[XmlElement(ElementName = "purchaseContactID")]
	[DataMember(Name = "purchaseContactID", Order = 21)]
	public string PurchaseContactID { get; set; }

	[XmlElement(ElementName = "apInvoiceLocation")]
	[DataMember(Name = "apInvoiceLocation", Order = 22)]
	public bool ApInvoiceLocation { get; set; }

	[XmlElement(ElementName = "apInvoiceContactID")]
	[DataMember(Name = "apInvoiceContactID", Order = 23)]
	public string ApInvoiceContactID { get; set; }

	[XmlElement(ElementName = "customerTaxable")]
	[DataMember(Name = "customerTaxable", Order = 24)]
	public bool CustomerTaxable { get; set; }

	[XmlElement(ElementName = "customerTaxCodeID")]
	[DataMember(Name = "customerTaxCodeID", Order = 25)]
	public string CustomerTaxCodeID { get; set; }

	[XmlElement(ElementName = "customerSecondTaxCodeID")]
	[DataMember(Name = "customerSecondTaxCodeID", Order = 26)]
	public string CustomerSecondTaxCodeID { get; set; }

	[XmlElement(ElementName = "customerShippingMethodID")]
	[DataMember(Name = "customerShippingMethodID", Order = 27)]
	public string CustomerShippingMethodID { get; set; }

	[XmlElement(ElementName = "customerShipPaymentTypeID")]
	[DataMember(Name = "customerShipPaymentTypeID", Order = 28)]
	public string CustomerShipPaymentTypeID { get; set; }

	[XmlElement(ElementName = "taxExemptNumber")]
	[DataMember(Name = "taxExemptNumber", Order = 29)]
	public string TaxExemptNumber { get; set; }

	[XmlElement(ElementName = "nonTaxReasonID")]
	[DataMember(Name = "nonTaxReasonID", Order = 30)]
	public string NonTaxReasonID { get; set; }

	[XmlElement(ElementName = "customerPaymentTermID")]
	[DataMember(Name = "customerPaymentTermID", Order = 31)]
	public string CustomerPaymentTermID { get; set; }

	[XmlElement(ElementName = "currencyRateID")]
	[DataMember(Name = "currencyRateID", Order = 32)]
	public string CurrencyRateID { get; set; } = string.Empty;

	[XmlElement(ElementName = "supplierPaymentTermID")]
	[DataMember(Name = "supplierPaymentTermID", Order = 33)]
	public string SupplierPaymentTermID { get; set; }

	[XmlElement(ElementName = "supplierShippingMethodID")]
	[DataMember(Name = "supplierShippingMethodID", Order = 34)]
	public string SupplierShippingMethodID { get; set; }

	[XmlElement(ElementName = "inactive")]
	[DataMember(Name = "inactive", Order = 35)]
	public bool Inactive { get; set; }

	[XmlElement(ElementName = "inactiveDate")]
	[DataMember(Name = "inactiveDate", Order = 36)]
	public DateTime? InactiveDate { get; set; }

	[XmlElement(ElementName = "customerShippingCarrier")]
	[DataMember(Name = "customerShippingCarrier", Order = 37)]
	public string CustomerShippingCarrier { get; set; }

	[XmlElement(ElementName = "upsAcctNumber")]
	[DataMember(Name = "upsAcctNumber", Order = 38)]
	public string UpsAcctNumber { get; set; }

	[XmlElement(ElementName = "upsWsBillingOption")]
	[DataMember(Name = "upsWsBillingOption", Order = 39)]
	public string UpsWsBillingOption { get; set; }

	[XmlElement(ElementName = "ups3rdPartyOrganizationID")]
	[DataMember(Name = "ups3rdPartyOrganizationID", Order = 40)]
	public string Ups3rdPartyOrganizationID { get; set; }

	[XmlElement(ElementName = "ups3rdPartyLocationID")]
	[DataMember(Name = "ups3rdPartyLocationID", Order = 41)]
	public string Ups3rdPartyLocationID { get; set; }

	[XmlElement(ElementName = "residentialAddress")]
	[DataMember(Name = "residentialAddress", Order = 42)]
	public bool ResidentialAddress { get; set; }

	[XmlElement(ElementName = "fedExAccountNumber")]
	[DataMember(Name = "fedExAccountNumber", Order = 43)]
	public string FedExAccountNumber { get; set; }

	[XmlElement(ElementName = "fedEx3rdPartyOrganizationID")]
	[DataMember(Name = "fedEx3rdPartyOrganizationID", Order = 44)]
	public string FedEx3rdPartyOrganizationID { get; set; }

	[XmlElement(ElementName = "fedExBillingOption")]
	[DataMember(Name = "fedExBillingOption", Order = 45)]
	public string FedExBillingOption { get; set; }

	[XmlElement(ElementName = "creditCheckForLocation")]
	[DataMember(Name = "creditCheckForLocation", Order = 46)]
	public bool CreditCheckForLocation { get; set; }

	[XmlElement(ElementName = "customerCreditLimit")]
	[DataMember(Name = "customerCreditLimit", Order = 47)]
	public decimal CustomerCreditLimit { get; set; }

	[XmlElement(ElementName = "creditHold")]
	[DataMember(Name = "creditHold", Order = 48)]
	public bool CreditHold { get; set; }

	[XmlElement(ElementName = "countryCode")]
	[DataMember(Name = "countryCode", Order = 49)]
	public string CountryCode { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 50)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "createdBy")]
	[DataMember(Name = "createdBy", Order = 51)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "avalaraUseCodes")]
	[DataMember(Name = "avalaraUseCodes", Order = 52)]
	public string AvalaraUseCodes { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 53)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 54)]
	public byte[] RowVersion { get; set; }
}
