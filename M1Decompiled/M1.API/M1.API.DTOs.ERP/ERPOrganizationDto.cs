using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPOrganizationDto
{
	[JsonProperty("cmoAccountManagerEmployeeID", Order = 1)]
	[MaxLength(10)]
	public string cmoAccountManagerEmployeeID { get; set; }

	[JsonProperty("cmoAddressLine1", Order = 2)]
	[MaxLength(50)]
	public string cmoAddressLine1 { get; set; }

	[JsonProperty("cmoAddressLine2", Order = 3)]
	[MaxLength(50)]
	public string cmoAddressLine2 { get; set; }

	[JsonProperty("cmoAddressLine3", Order = 4)]
	[MaxLength(50)]
	public string cmoAddressLine3 { get; set; }

	[JsonProperty("cmoAddressValidationResult", Order = 5)]
	[MaxLength(50)]
	public string cmoAddressValidationResult { get; set; }

	[JsonProperty("cmoAlternatePhoneNumber", Order = 6)]
	[MaxLength(20)]
	public string cmoAlternatePhoneNumber { get; set; }

	[JsonProperty("cmoApInvoiceContactID", Order = 7)]
	[MaxLength(5)]
	public string cmoApInvoiceContactID { get; set; }

	[JsonProperty("cmoArInvoiceContactID", Order = 8)]
	[MaxLength(5)]
	public string cmoArInvoiceContactID { get; set; }

	[JsonProperty("cmoAttachmentFileFolder", Order = 9)]
	[MaxLength(50)]
	public string cmoAttachmentFileFolder { get; set; }

	[JsonProperty("cmoAvalaraUseCodes", Order = 10)]
	[MaxLength(1)]
	public string cmoAvalaraUseCodes { get; set; }

	[JsonProperty("cmoBankAccountName", Order = 11)]
	[MaxLength(50)]
	public string cmoBankAccountName { get; set; }

	[JsonProperty("cmoBankAccountNumber", Order = 12)]
	[MaxLength(24)]
	public string cmoBankAccountNumber { get; set; }

	[JsonProperty("cmoBankAccountType", Order = 13)]
	[MaxLength(2)]
	public string cmoBankAccountType { get; set; }

	[JsonProperty("cmoBankInitials", Order = 14)]
	[MaxLength(3)]
	public string cmoBankInitials { get; set; }

	[JsonProperty("cmoBic", Order = 15)]
	[MaxLength(50)]
	public string cmoBic { get; set; }

	[JsonProperty("cmoBsbNumber", Order = 16)]
	[MaxLength(10)]
	public string cmoBsbNumber { get; set; }

	[JsonProperty("cmoCity", Order = 17)]
	[MaxLength(30)]
	public string cmoCity { get; set; }

	[JsonProperty("cmoOrganizationID", Order = 18)]
	[Required(ErrorMessage = "cmoOrganizationID is required.")]
	[MaxLength(10)]
	public string cmoOrganizationID { get; set; }

	[JsonProperty("cmoCompanyEntryDescription", Order = 19)]
	[MaxLength(10)]
	public string cmoCompanyEntryDescription { get; set; }

	[JsonProperty("cmoCountry", Order = 20)]
	[MaxLength(20)]
	public string cmoCountry { get; set; }

	[JsonProperty("cmoCountryCode", Order = 21)]
	[MaxLength(2)]
	public string cmoCountryCode { get; set; }

	[JsonProperty("cmoCounty", Order = 22)]
	[MaxLength(30)]
	public string cmoCounty { get; set; }

	[JsonProperty("cmoCreatedBy", Order = 23)]
	[MaxLength(20)]
	public string cmoCreatedBy { get; set; }

	[JsonProperty("cmoCreatedDate", Order = 24)]
	public DateTime? cmoCreatedDate { get; set; }

	[JsonProperty("cmoCurrencyRateID", Order = 25)]
	[MaxLength(5)]
	public string cmoCurrencyRateID { get; set; }

	[JsonProperty("cmoCustomerActiveDate", Order = 26)]
	public DateTime? cmoCustomerActiveDate { get; set; }

	[JsonProperty("cmoCustomerCreditLimit", Order = 27)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cmoCustomerCreditLimit { get; set; }

	[JsonProperty("cmoCustomerGroupID", Order = 28)]
	[MaxLength(5)]
	public string cmoCustomerGroupID { get; set; }

	[JsonProperty("cmoCustomerInactiveDate", Order = 29)]
	public DateTime? cmoCustomerInactiveDate { get; set; }

	[JsonProperty("cmoCustomerPaymentTermsID", Order = 30)]
	[MaxLength(5)]
	public string cmoCustomerPaymentTermsID { get; set; }

	[JsonProperty("cmoCustomerProspectDate", Order = 31)]
	public DateTime? cmoCustomerProspectDate { get; set; }

	[JsonProperty("cmoCustomerSecondTaxCodeID", Order = 32)]
	[MaxLength(5)]
	public string cmoCustomerSecondTaxCodeID { get; set; }

	[JsonProperty("cmoCustomerShipPaymentTypeID", Order = 33)]
	[MaxLength(5)]
	public string cmoCustomerShipPaymentTypeID { get; set; }

	[JsonProperty("cmoCustomerShippingCarrier", Order = 34)]
	[MaxLength(5)]
	public string cmoCustomerShippingCarrier { get; set; }

	[JsonProperty("cmoCustomerShippingMethodID", Order = 35)]
	[MaxLength(5)]
	public string cmoCustomerShippingMethodID { get; set; }

	[JsonProperty("cmoCustomerStatus", Order = 36)]
	public byte cmoCustomerStatus { get; set; }

	[JsonProperty("cmoCustomerTaxCodeID", Order = 37)]
	[MaxLength(5)]
	public string cmoCustomerTaxCodeID { get; set; }

	[JsonProperty("cmoDefaultApInvoiceLocationID", Order = 38)]
	[MaxLength(5)]
	public string cmoDefaultApInvoiceLocationID { get; set; }

	[JsonProperty("cmoDefaultArInvoiceLocationID", Order = 39)]
	[MaxLength(5)]
	public string cmoDefaultArInvoiceLocationID { get; set; }

	[JsonProperty("cmoDefaultPurchaseLocationID", Order = 40)]
	[MaxLength(5)]
	public string cmoDefaultPurchaseLocationID { get; set; }

	[JsonProperty("cmoDefaultQuoteLocationID", Order = 41)]
	[MaxLength(5)]
	public string cmoDefaultQuoteLocationID { get; set; }

	[JsonProperty("cmoDefaultShipLocationID", Order = 42)]
	[MaxLength(5)]
	public string cmoDefaultShipLocationID { get; set; }

	[JsonProperty("cmoDropShipLocationID", Order = 43)]
	[MaxLength(5)]
	public string cmoDropShipLocationID { get; set; }

	[JsonProperty("cmoDropShipOrganizationID", Order = 44)]
	[MaxLength(10)]
	public string cmoDropShipOrganizationID { get; set; }

	[JsonProperty("cmoEftCode", Order = 45)]
	[MaxLength(12)]
	public string cmoEftCode { get; set; }

	[JsonProperty("cmoEftDescription", Order = 46)]
	[MaxLength(20)]
	public string cmoEftDescription { get; set; }

	[JsonProperty("cmoEftParticulars", Order = 47)]
	[MaxLength(12)]
	public string cmoEftParticulars { get; set; }

	[JsonProperty("cmoEmailAddress", Order = 48)]
	[MaxLength(50)]
	public string cmoEmailAddress { get; set; }

	[JsonProperty("cmoEmployeeCount", Order = 49)]
	[Range(0, 999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int cmoEmployeeCount { get; set; }

	[JsonProperty("cmoUniqueID", Order = 50)]
	public Guid cmoUniqueID { get; set; }

	[JsonProperty("cmoEstablishedDate", Order = 51)]
	public DateTime? cmoEstablishedDate { get; set; }

	[JsonProperty("cmoExpenseSplitPercentTotal", Order = 52)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cmoExpenseSplitPercentTotal { get; set; }

	[JsonProperty("cmoFaxNumber", Order = 53)]
	[MaxLength(20)]
	public string cmoFaxNumber { get; set; }

	[JsonProperty("cmoFederalID", Order = 54)]
	[MaxLength(20)]
	public string cmoFederalID { get; set; }

	[JsonProperty("cmoFedEx3rdPartyLocationID", Order = 55)]
	[MaxLength(5)]
	public string cmoFedEx3rdPartyLocationID { get; set; }

	[JsonProperty("cmoFedEx3rdPartyOrganizationID", Order = 56)]
	[MaxLength(10)]
	public string cmoFedEx3rdPartyOrganizationID { get; set; }

	[JsonProperty("cmoFedExAccountNumber", Order = 57)]
	[MaxLength(15)]
	public string cmoFedExAccountNumber { get; set; }

	[JsonProperty("cmoFedExBillingOption", Order = 58)]
	[MaxLength(20)]
	public string cmoFedExBillingOption { get; set; }

	[JsonProperty("cmoFirstGivenName", Order = 59)]
	[MaxLength(15)]
	public string cmoFirstGivenName { get; set; }

	[JsonProperty("cmoForm1099Box", Order = 60)]
	public byte cmoForm1099Box { get; set; }

	[JsonProperty("cmoFreeOnBoardDescription", Order = 61)]
	[MaxLength(15)]
	public string cmoFreeOnBoardDescription { get; set; }

	[JsonProperty("cmoHdAttachmentFilePath", Order = 62)]
	[MaxLength(50)]
	public string cmoHdAttachmentFilePath { get; set; }

	[JsonProperty("cmoIban", Order = 63)]
	[MaxLength(50)]
	public string cmoIban { get; set; }

	[JsonProperty("cmoIntraCompanyDatasetID", Order = 64)]
	[MaxLength(40)]
	public string cmoIntraCompanyDatasetID { get; set; }

	[JsonProperty("cmoApIncludeTaxInRetention", Order = 65)]
	public bool cmoApIncludeTaxInRetention { get; set; }

	[JsonProperty("cmoArIncludeTaxInRetention", Order = 66)]
	public bool cmoArIncludeTaxInRetention { get; set; }

	[JsonProperty("cmoArInvoicePerShipmentLine", Order = 67)]
	public bool cmoArInvoicePerShipmentLine { get; set; }

	[JsonProperty("cmoAvalaraAddressValidated", Order = 68)]
	public bool cmoAvalaraAddressValidated { get; set; }

	[JsonProperty("cmoBareCostOfDuty", Order = 69)]
	public bool cmoBareCostOfDuty { get; set; }

	[JsonProperty("cmoBareTransportationCost", Order = 70)]
	public bool cmoBareTransportationCost { get; set; }

	[JsonProperty("cmoCalculateFinanceCharges", Order = 71)]
	public bool cmoCalculateFinanceCharges { get; set; }

	[JsonProperty("cmoCompetitor", Order = 72)]
	public bool cmoCompetitor { get; set; }

	[JsonProperty("cmoContractor", Order = 73)]
	public bool cmoContractor { get; set; }

	[JsonProperty("cmoCreatedFromMobile", Order = 74)]
	public bool cmoCreatedFromMobile { get; set; }

	[JsonProperty("cmoCreditHold", Order = 75)]
	public bool cmoCreditHold { get; set; }

	[JsonProperty("cmoCustomerTaxable", Order = 76)]
	public bool cmoCustomerTaxable { get; set; }

	[JsonProperty("cmoDirectPayment", Order = 77)]
	public bool cmoDirectPayment { get; set; }

	[JsonProperty("cmoEdiIntegrated", Order = 78)]
	public bool cmoEdiIntegrated { get; set; }

	[JsonProperty("cmoFinanceCompany", Order = 79)]
	public bool cmoFinanceCompany { get; set; }

	[JsonProperty("cmoIgnoreAvalara", Order = 80)]
	public bool cmoIgnoreAvalara { get; set; }

	[JsonProperty("cmoIncludeFreightInPrice", Order = 81)]
	public bool cmoIncludeFreightInPrice { get; set; }

	[JsonProperty("cmoPrintStatement", Order = 82)]
	public bool cmoPrintStatement { get; set; }

	[JsonProperty("cmoRequires1099", Order = 83)]
	public bool cmoRequires1099 { get; set; }

	[JsonProperty("cmoRequiresInspection", Order = 84)]
	public bool cmoRequiresInspection { get; set; }

	[JsonProperty("cmoResidentialAddress", Order = 85)]
	public bool cmoResidentialAddress { get; set; }

	[JsonProperty("cmoSuperFund", Order = 86)]
	public bool cmoSuperFund { get; set; }

	[JsonProperty("cmoSupplierAccredited", Order = 87)]
	public bool cmoSupplierAccredited { get; set; }

	[JsonProperty("cmoSupplierTaxable", Order = 88)]
	public bool cmoSupplierTaxable { get; set; }

	[JsonProperty("cmoTaxReportable", Order = 89)]
	public bool cmoTaxReportable { get; set; }

	[JsonProperty("cmoUpsValidated", Order = 90)]
	public bool cmoUpsValidated { get; set; }

	[JsonProperty("cmoJobPriorityID", Order = 91)]
	public short cmoJobPriorityID { get; set; }

	[JsonProperty("cmoLastName", Order = 92)]
	[MaxLength(30)]
	public string cmoLastName { get; set; }

	[JsonProperty("cmoLongDescriptionRtf", Order = 93)]
	public string cmoLongDescriptionRtf { get; set; }

	[JsonProperty("cmoLongDescriptionText", Order = 94)]
	public string cmoLongDescriptionText { get; set; }

	[JsonProperty("cmoName", Order = 95)]
	[Required(ErrorMessage = "cmoName is required.")]
	[MaxLength(50)]
	public string cmoName { get; set; }

	[JsonProperty("cmoNonTaxReasonID", Order = 96)]
	[MaxLength(5)]
	public string cmoNonTaxReasonID { get; set; }

	[JsonProperty("cmoOrganizationAccountID", Order = 97)]
	[MaxLength(20)]
	public string cmoOrganizationAccountID { get; set; }

	[JsonProperty("cmoPhoneNumber", Order = 98)]
	[MaxLength(20)]
	public string cmoPhoneNumber { get; set; }

	[JsonProperty("cmoPostCode", Order = 99)]
	[MaxLength(10)]
	public string cmoPostCode { get; set; }

	[JsonProperty("cmoPurchaseContactID", Order = 100)]
	[MaxLength(5)]
	public string cmoPurchaseContactID { get; set; }

	[JsonProperty("cmoQuoteContactID", Order = 101)]
	[MaxLength(5)]
	public string cmoQuoteContactID { get; set; }

	[JsonProperty("cmoResellerActiveDate", Order = 102)]
	public DateTime? cmoResellerActiveDate { get; set; }

	[JsonProperty("cmoResellerCommissionRate", Order = 103)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cmoResellerCommissionRate { get; set; }

	[JsonProperty("cmoResellerContactID", Order = 104)]
	[MaxLength(5)]
	public string cmoResellerContactID { get; set; }

	[JsonProperty("cmoResellerInactiveDate", Order = 105)]
	public DateTime? cmoResellerInactiveDate { get; set; }

	[JsonProperty("cmoResellerLocationID", Order = 106)]
	[MaxLength(5)]
	public string cmoResellerLocationID { get; set; }

	[JsonProperty("cmoResellerOrganizationID", Order = 107)]
	[MaxLength(10)]
	public string cmoResellerOrganizationID { get; set; }

	[JsonProperty("cmoResellerProspectDate", Order = 108)]
	public DateTime? cmoResellerProspectDate { get; set; }

	[JsonProperty("cmoResellerStatus", Order = 109)]
	public byte cmoResellerStatus { get; set; }

	[JsonProperty("cmoRowVersion", Order = 110)]
	public byte[] cmoRowVersion { get; set; }

	[JsonProperty("cmoSecondGivenName", Order = 111)]
	[MaxLength(15)]
	public string cmoSecondGivenName { get; set; }

	[JsonProperty("cmoShipContactID", Order = 112)]
	[MaxLength(5)]
	public string cmoShipContactID { get; set; }

	[JsonProperty("cmoSplitPercentTotal", Order = 113)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cmoSplitPercentTotal { get; set; }

	[JsonProperty("cmoState", Order = 114)]
	[MaxLength(3)]
	public string cmoState { get; set; }

	[JsonProperty("cmoSuperFundEmployerID", Order = 115)]
	[MaxLength(16)]
	public string cmoSuperFundEmployerID { get; set; }

	[JsonProperty("cmoSuperFundName", Order = 116)]
	[MaxLength(60)]
	public string cmoSuperFundName { get; set; }

	[JsonProperty("cmoSupplierAccreditedDate", Order = 117)]
	public DateTime? cmoSupplierAccreditedDate { get; set; }

	[JsonProperty("cmoSupplierActiveDate", Order = 118)]
	public DateTime? cmoSupplierActiveDate { get; set; }

	[JsonProperty("cmoSupplierInactiveDate", Order = 119)]
	public DateTime? cmoSupplierInactiveDate { get; set; }

	[JsonProperty("cmoSupplierPaymentTermID", Order = 120)]
	[MaxLength(5)]
	public string cmoSupplierPaymentTermID { get; set; }

	[JsonProperty("cmoSupplierProspectDate", Order = 121)]
	public DateTime? cmoSupplierProspectDate { get; set; }

	[JsonProperty("cmoSupplierRatingID", Order = 122)]
	[MaxLength(5)]
	public string cmoSupplierRatingID { get; set; }

	[JsonProperty("cmoSupplierSecondTaxCodeID", Order = 123)]
	[MaxLength(5)]
	public string cmoSupplierSecondTaxCodeID { get; set; }

	[JsonProperty("cmoSupplierShippingMethodID", Order = 124)]
	[MaxLength(5)]
	public string cmoSupplierShippingMethodID { get; set; }

	[JsonProperty("cmoSupplierStatus", Order = 125)]
	public byte cmoSupplierStatus { get; set; }

	[JsonProperty("cmoSupplierTaxCodeID", Order = 126)]
	[MaxLength(5)]
	public string cmoSupplierTaxCodeID { get; set; }

	[JsonProperty("cmoTaxExemptNumber", Order = 127)]
	[MaxLength(16)]
	public string cmoTaxExemptNumber { get; set; }

	[JsonProperty("cmoTradingName", Order = 128)]
	[MaxLength(50)]
	public string cmoTradingName { get; set; }

	[JsonProperty("cmoUps3rdPartyLocationID", Order = 129)]
	[MaxLength(5)]
	public string cmoUps3rdPartyLocationID { get; set; }

	[JsonProperty("cmoUps3rdPartyOrganizationID", Order = 130)]
	[MaxLength(10)]
	public string cmoUps3rdPartyOrganizationID { get; set; }

	[JsonProperty("cmoUpsAcctNumber", Order = 131)]
	[MaxLength(6)]
	public string cmoUpsAcctNumber { get; set; }

	[JsonProperty("cmoUpsBillingOption", Order = 132)]
	[MaxLength(20)]
	public string cmoUpsBillingOption { get; set; }

	[JsonProperty("cmoUpsWsBillingOption", Order = 133)]
	[MaxLength(20)]
	public string cmoUpsWsBillingOption { get; set; }

	[JsonProperty("cmoUsaTransactionTypeCode", Order = 134)]
	[MaxLength(3)]
	public string cmoUsaTransactionTypeCode { get; set; }

	[JsonProperty("cmoWebAddress", Order = 135)]
	[MaxLength(50)]
	public string cmoWebAddress { get; set; }

	[JsonProperty("customFields", Order = 136)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
