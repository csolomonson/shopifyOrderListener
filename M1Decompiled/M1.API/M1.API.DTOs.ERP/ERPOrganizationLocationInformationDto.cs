using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPOrganizationLocationInformationDto
{
	public string cmlAddressLine1 { get; set; }

	public string cmlAddressLine2 { get; set; }

	public string cmlAddressLine3 { get; set; }

	public string cmlAddressValidationResult { get; set; }

	public string cmlAlternatePhoneNumber { get; set; }

	public string cmlApInvoiceContactID { get; set; }

	public string cmlArInvoiceContactID { get; set; }

	public string cmlAvalaraUseCodes { get; set; }

	public string cmlBankAccountName { get; set; }

	public string cmlBankAccountNumber { get; set; }

	public string cmlBankAccountType { get; set; }

	public string cmlBankInitials { get; set; }

	public string cmlBic { get; set; }

	public string cmlBsbNumber { get; set; }

	public string cmlCity { get; set; }

	public string cmlCountry { get; set; }

	public string cmlCountryCode { get; set; }

	public string cmlCounty { get; set; }

	public string cmlCreatedBy { get; set; }

	public DateTime? cmlCreatedDate { get; set; }

	public string cmlCurrencyRateID { get; set; }

	public decimal cmlCustomerCreditLimit { get; set; }

	public string cmlCustomerPaymentTermID { get; set; }

	public string cmlCustomerSecondTaxCodeID { get; set; }

	public string cmlCustomerShipPaymentTypeID { get; set; }

	public string cmlCustomerShippingCarrier { get; set; }

	public string cmlCustomerShippingMethodID { get; set; }

	public string cmlCustomerTaxCodeID { get; set; }

	public string cmlEdiLocationID { get; set; }

	public string cmlEftCode { get; set; }

	public string cmlEftDescription { get; set; }

	public string cmlEftParticulars { get; set; }

	public string cmlEmailAddress { get; set; }

	public Guid cmlUniqueID { get; set; }

	public string cmlFaxNumber { get; set; }

	public string cmlFedEx3rdPartyLocationID { get; set; }

	public string cmlFedEx3rdPartyOrganizationID { get; set; }

	public string cmlFedExAccountNumber { get; set; }

	public string cmlFedExBillingOption { get; set; }

	public string cmlFinanceOrganizationID { get; set; }

	public string cmlFirstGivenName { get; set; }

	public string cmlFreeOnBoardDescription { get; set; }

	public string cmlHdAttachmentFilePath { get; set; }

	public string cmlIban { get; set; }

	public DateTime? cmlInactiveDate { get; set; }

	public bool cmlInactive { get; set; }

	public bool cmlApInvoiceLocation { get; set; }

	public bool cmlArInvoiceLocation { get; set; }

	public bool cmlArInvoicePerShipmentLine { get; set; }

	public bool cmlAvalaraAddressValidated { get; set; }

	public bool cmlBareCostOfDuty { get; set; }

	public bool cmlBareTransportationCost { get; set; }

	public bool cmlContractor { get; set; }

	public bool cmlCreatedFromMobile { get; set; }

	public bool cmlCreditCheckForLocation { get; set; }

	public bool cmlCreditHold { get; set; }

	public bool cmlCustomerTaxable { get; set; }

	public bool cmlDirectPayment { get; set; }

	public bool cmlEdiIntegrated { get; set; }

	public bool cmlIgnoreAvalara { get; set; }

	public bool cmlPurchaseLocation { get; set; }

	public bool cmlQuoteLocation { get; set; }

	public bool cmlResidentialAddress { get; set; }

	public bool cmlShipLocation { get; set; }

	public bool cmlTaxReportable { get; set; }

	public bool cmlUpsValidated { get; set; }

	public string cmlLastName { get; set; }

	public string cmlLocationID { get; set; }

	public string cmlName { get; set; }

	public string cmlNonTaxReasonID { get; set; }

	public string cmlOrganizationID { get; set; }

	public string cmlPhoneNumber { get; set; }

	public string cmlPostCode { get; set; }

	public string cmlPurchaseContactID { get; set; }

	public string cmlQuoteContactID { get; set; }

	public byte[] cmlRowVersion { get; set; }

	public string cmlSecondGivenName { get; set; }

	public string cmlShipContactID { get; set; }

	public decimal cmlSplitPercentTotal { get; set; }

	public string cmlState { get; set; }

	public string cmlSupplierPaymentTermID { get; set; }

	public string cmlSupplierShippingMethodID { get; set; }

	public string cmlTaxExemptNumber { get; set; }

	public string cmlTradingName { get; set; }

	public string cmlUps3rdPartyLocationID { get; set; }

	public string cmlUps3rdPartyOrganizationID { get; set; }

	public string cmlUpsAcctNumber { get; set; }

	public string cmlUpsBillingOption { get; set; }

	public string cmlUpsWsBillingOption { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
