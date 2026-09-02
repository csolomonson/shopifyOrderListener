using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPOrganizationLocationDto
{
	[JsonProperty("cmlAddressLine1", Order = 1)]
	[MaxLength(50)]
	public string cmlAddressLine1 { get; set; }

	[JsonProperty("cmlAddressLine2", Order = 2)]
	[MaxLength(50)]
	public string cmlAddressLine2 { get; set; }

	[JsonProperty("cmlAddressLine3", Order = 3)]
	[MaxLength(50)]
	public string cmlAddressLine3 { get; set; }

	[JsonProperty("cmlAddressValidationResult", Order = 4)]
	[MaxLength(50)]
	public string cmlAddressValidationResult { get; set; }

	[JsonProperty("cmlAlternatePhoneNumber", Order = 5)]
	[MaxLength(20)]
	public string cmlAlternatePhoneNumber { get; set; }

	[JsonProperty("cmlApInvoiceContactID", Order = 6)]
	[MaxLength(5)]
	public string cmlApInvoiceContactID { get; set; }

	[JsonProperty("cmlArInvoiceContactID", Order = 7)]
	[MaxLength(5)]
	public string cmlArInvoiceContactID { get; set; }

	[JsonProperty("cmlAvalaraUseCodes", Order = 8)]
	[MaxLength(1)]
	public string cmlAvalaraUseCodes { get; set; }

	[JsonProperty("cmlBankAccountName", Order = 9)]
	[MaxLength(50)]
	public string cmlBankAccountName { get; set; }

	[JsonProperty("cmlBankAccountNumber", Order = 10)]
	[MaxLength(24)]
	public string cmlBankAccountNumber { get; set; }

	[JsonProperty("cmlBankAccountType", Order = 11)]
	[MaxLength(2)]
	public string cmlBankAccountType { get; set; }

	[JsonProperty("cmlBankInitials", Order = 12)]
	[MaxLength(3)]
	public string cmlBankInitials { get; set; }

	[JsonProperty("cmlBic", Order = 13)]
	[MaxLength(50)]
	public string cmlBic { get; set; }

	[JsonProperty("cmlBsbNumber", Order = 14)]
	[MaxLength(10)]
	public string cmlBsbNumber { get; set; }

	[JsonProperty("cmlCity", Order = 15)]
	[MaxLength(30)]
	public string cmlCity { get; set; }

	[JsonProperty("cmlCountry", Order = 16)]
	[MaxLength(20)]
	public string cmlCountry { get; set; }

	[JsonProperty("cmlCountryCode", Order = 17)]
	[MaxLength(2)]
	public string cmlCountryCode { get; set; }

	[JsonProperty("cmlCounty", Order = 18)]
	[MaxLength(30)]
	public string cmlCounty { get; set; }

	[JsonProperty("cmlCreatedBy", Order = 19)]
	[MaxLength(20)]
	public string cmlCreatedBy { get; set; }

	[JsonProperty("cmlCreatedDate", Order = 20)]
	public DateTime? cmlCreatedDate { get; set; }

	[JsonProperty("cmlCurrencyRateID", Order = 21)]
	[MaxLength(5)]
	public string cmlCurrencyRateID { get; set; }

	[JsonProperty("cmlCustomerCreditLimit", Order = 22)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cmlCustomerCreditLimit { get; set; }

	[JsonProperty("cmlCustomerPaymentTermID", Order = 23)]
	[MaxLength(5)]
	public string cmlCustomerPaymentTermID { get; set; }

	[JsonProperty("cmlCustomerSecondTaxCodeID", Order = 24)]
	[MaxLength(5)]
	public string cmlCustomerSecondTaxCodeID { get; set; }

	[JsonProperty("cmlCustomerShipPaymentTypeID", Order = 25)]
	[MaxLength(5)]
	public string cmlCustomerShipPaymentTypeID { get; set; }

	[JsonProperty("cmlCustomerShippingCarrier", Order = 26)]
	[MaxLength(5)]
	public string cmlCustomerShippingCarrier { get; set; }

	[JsonProperty("cmlCustomerShippingMethodID", Order = 27)]
	[MaxLength(5)]
	public string cmlCustomerShippingMethodID { get; set; }

	[JsonProperty("cmlCustomerTaxCodeID", Order = 28)]
	[MaxLength(5)]
	public string cmlCustomerTaxCodeID { get; set; }

	[JsonProperty("cmlEdiLocationID", Order = 29)]
	[MaxLength(30)]
	public string cmlEdiLocationID { get; set; }

	[JsonProperty("cmlEftCode", Order = 30)]
	[MaxLength(12)]
	public string cmlEftCode { get; set; }

	[JsonProperty("cmlEftDescription", Order = 31)]
	[MaxLength(20)]
	public string cmlEftDescription { get; set; }

	[JsonProperty("cmlEftParticulars", Order = 32)]
	[MaxLength(12)]
	public string cmlEftParticulars { get; set; }

	[JsonProperty("cmlEmailAddress", Order = 33)]
	[MaxLength(50)]
	public string cmlEmailAddress { get; set; }

	[JsonProperty("cmlUniqueID", Order = 34)]
	public Guid cmlUniqueID { get; set; }

	[JsonProperty("cmlFaxNumber", Order = 35)]
	[MaxLength(20)]
	public string cmlFaxNumber { get; set; }

	[JsonProperty("cmlFedEx3rdPartyLocationID", Order = 36)]
	[MaxLength(5)]
	public string cmlFedEx3rdPartyLocationID { get; set; }

	[JsonProperty("cmlFedEx3rdPartyOrganizationID", Order = 37)]
	[MaxLength(10)]
	public string cmlFedEx3rdPartyOrganizationID { get; set; }

	[JsonProperty("cmlFedExAccountNumber", Order = 38)]
	[MaxLength(15)]
	public string cmlFedExAccountNumber { get; set; }

	[JsonProperty("cmlFedExBillingOption", Order = 39)]
	[MaxLength(20)]
	public string cmlFedExBillingOption { get; set; }

	[JsonProperty("cmlFinanceOrganizationID", Order = 40)]
	[MaxLength(10)]
	public string cmlFinanceOrganizationID { get; set; }

	[JsonProperty("cmlFirstGivenName", Order = 41)]
	[MaxLength(15)]
	public string cmlFirstGivenName { get; set; }

	[JsonProperty("cmlFreeOnBoardDescription", Order = 42)]
	[MaxLength(15)]
	public string cmlFreeOnBoardDescription { get; set; }

	[JsonProperty("cmlHdAttachmentFilePath", Order = 43)]
	[MaxLength(50)]
	public string cmlHdAttachmentFilePath { get; set; }

	[JsonProperty("cmlIban", Order = 44)]
	[MaxLength(50)]
	public string cmlIban { get; set; }

	[JsonProperty("cmlInactiveDate", Order = 45)]
	public DateTime? cmlInactiveDate { get; set; }

	[JsonProperty("cmlInactive", Order = 46)]
	public bool cmlInactive { get; set; }

	[JsonProperty("cmlApInvoiceLocation", Order = 47)]
	public bool cmlApInvoiceLocation { get; set; }

	[JsonProperty("cmlArInvoiceLocation", Order = 48)]
	public bool cmlArInvoiceLocation { get; set; }

	[JsonProperty("cmlArInvoicePerShipmentLine", Order = 49)]
	public bool cmlArInvoicePerShipmentLine { get; set; }

	[JsonProperty("cmlAvalaraAddressValidated", Order = 50)]
	public bool cmlAvalaraAddressValidated { get; set; }

	[JsonProperty("cmlBareCostOfDuty", Order = 51)]
	public bool cmlBareCostOfDuty { get; set; }

	[JsonProperty("cmlBareTransportationCost", Order = 52)]
	public bool cmlBareTransportationCost { get; set; }

	[JsonProperty("cmlContractor", Order = 53)]
	public bool cmlContractor { get; set; }

	[JsonProperty("cmlCreatedFromMobile", Order = 54)]
	public bool cmlCreatedFromMobile { get; set; }

	[JsonProperty("cmlCreditCheckForLocation", Order = 55)]
	public bool cmlCreditCheckForLocation { get; set; }

	[JsonProperty("cmlCreditHold", Order = 56)]
	public bool cmlCreditHold { get; set; }

	[JsonProperty("cmlCustomerTaxable", Order = 57)]
	public bool cmlCustomerTaxable { get; set; }

	[JsonProperty("cmlDirectPayment", Order = 58)]
	public bool cmlDirectPayment { get; set; }

	[JsonProperty("cmlEdiIntegrated", Order = 59)]
	public bool cmlEdiIntegrated { get; set; }

	[JsonProperty("cmlIgnoreAvalara", Order = 60)]
	public bool cmlIgnoreAvalara { get; set; }

	[JsonProperty("cmlPurchaseLocation", Order = 61)]
	public bool cmlPurchaseLocation { get; set; }

	[JsonProperty("cmlQuoteLocation", Order = 62)]
	public bool cmlQuoteLocation { get; set; }

	[JsonProperty("cmlResidentialAddress", Order = 63)]
	public bool cmlResidentialAddress { get; set; }

	[JsonProperty("cmlShipLocation", Order = 64)]
	public bool cmlShipLocation { get; set; }

	[JsonProperty("cmlTaxReportable", Order = 65)]
	public bool cmlTaxReportable { get; set; }

	[JsonProperty("cmlUpsValidated", Order = 66)]
	public bool cmlUpsValidated { get; set; }

	[JsonProperty("cmlLastName", Order = 67)]
	[MaxLength(30)]
	public string cmlLastName { get; set; }

	[JsonProperty("cmlLocationID", Order = 68)]
	[MaxLength(5)]
	public string cmlLocationID { get; set; }

	[JsonProperty("cmlName", Order = 69)]
	[Required(ErrorMessage = "cmlName is required.")]
	[MaxLength(50)]
	public string cmlName { get; set; }

	[JsonProperty("cmlNonTaxReasonID", Order = 70)]
	[MaxLength(5)]
	public string cmlNonTaxReasonID { get; set; }

	[JsonProperty("cmlOrganizationID", Order = 71)]
	[Required(ErrorMessage = "cmlOrganizationID is required.")]
	[MaxLength(10)]
	public string cmlOrganizationID { get; set; }

	[JsonProperty("cmlPhoneNumber", Order = 72)]
	[MaxLength(20)]
	public string cmlPhoneNumber { get; set; }

	[JsonProperty("cmlPostCode", Order = 73)]
	[MaxLength(10)]
	public string cmlPostCode { get; set; }

	[JsonProperty("cmlPurchaseContactID", Order = 74)]
	[MaxLength(5)]
	public string cmlPurchaseContactID { get; set; }

	[JsonProperty("cmlQuoteContactID", Order = 75)]
	[MaxLength(5)]
	public string cmlQuoteContactID { get; set; }

	[JsonProperty("cmlRowVersion", Order = 76)]
	public byte[] cmlRowVersion { get; set; }

	[JsonProperty("cmlSecondGivenName", Order = 77)]
	[MaxLength(15)]
	public string cmlSecondGivenName { get; set; }

	[JsonProperty("cmlShipContactID", Order = 78)]
	[MaxLength(5)]
	public string cmlShipContactID { get; set; }

	[JsonProperty("cmlSplitPercentTotal", Order = 79)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cmlSplitPercentTotal { get; set; }

	[JsonProperty("cmlState", Order = 80)]
	[MaxLength(3)]
	public string cmlState { get; set; }

	[JsonProperty("cmlSupplierPaymentTermID", Order = 81)]
	[MaxLength(5)]
	public string cmlSupplierPaymentTermID { get; set; }

	[JsonProperty("cmlSupplierShippingMethodID", Order = 82)]
	[MaxLength(5)]
	public string cmlSupplierShippingMethodID { get; set; }

	[JsonProperty("cmlTaxExemptNumber", Order = 83)]
	[MaxLength(16)]
	public string cmlTaxExemptNumber { get; set; }

	[JsonProperty("cmlTradingName", Order = 84)]
	[MaxLength(50)]
	public string cmlTradingName { get; set; }

	[JsonProperty("cmlUps3rdPartyLocationID", Order = 85)]
	[MaxLength(5)]
	public string cmlUps3rdPartyLocationID { get; set; }

	[JsonProperty("cmlUps3rdPartyOrganizationID", Order = 86)]
	[MaxLength(10)]
	public string cmlUps3rdPartyOrganizationID { get; set; }

	[JsonProperty("cmlUpsAcctNumber", Order = 87)]
	[MaxLength(6)]
	public string cmlUpsAcctNumber { get; set; }

	[JsonProperty("cmlUpsBillingOption", Order = 88)]
	[MaxLength(20)]
	public string cmlUpsBillingOption { get; set; }

	[JsonProperty("cmlUpsWsBillingOption", Order = 89)]
	[MaxLength(20)]
	public string cmlUpsWsBillingOption { get; set; }

	[JsonProperty("customFields", Order = 90)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
