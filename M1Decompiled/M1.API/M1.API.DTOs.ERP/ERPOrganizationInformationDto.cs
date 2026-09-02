using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPOrganizationInformationDto
{
	public string cmoAccountManagerEmployeeID { get; set; }

	public string cmoAddressLine1 { get; set; }

	public string cmoAddressLine2 { get; set; }

	public string cmoAddressLine3 { get; set; }

	public string cmoAddressValidationResult { get; set; }

	public string cmoAlternatePhoneNumber { get; set; }

	public string cmoApInvoiceContactID { get; set; }

	public string cmoArInvoiceContactID { get; set; }

	public string cmoAttachmentFileFolder { get; set; }

	public string cmoAvalaraUseCodes { get; set; }

	public string cmoBankAccountName { get; set; }

	public string cmoBankAccountNumber { get; set; }

	public string cmoBankAccountType { get; set; }

	public string cmoBankInitials { get; set; }

	public string cmoBic { get; set; }

	public string cmoBsbNumber { get; set; }

	public string cmoCity { get; set; }

	public string cmoOrganizationID { get; set; }

	public string cmoCompanyEntryDescription { get; set; }

	public string cmoCountry { get; set; }

	public string cmoCountryCode { get; set; }

	public string cmoCounty { get; set; }

	public string cmoCreatedBy { get; set; }

	public DateTime? cmoCreatedDate { get; set; }

	public string cmoCurrencyRateID { get; set; }

	public DateTime? cmoCustomerActiveDate { get; set; }

	public decimal cmoCustomerCreditLimit { get; set; }

	public string cmoCustomerGroupID { get; set; }

	public DateTime? cmoCustomerInactiveDate { get; set; }

	public string cmoCustomerPaymentTermsID { get; set; }

	public DateTime? cmoCustomerProspectDate { get; set; }

	public string cmoCustomerSecondTaxCodeID { get; set; }

	public string cmoCustomerShipPaymentTypeID { get; set; }

	public string cmoCustomerShippingCarrier { get; set; }

	public string cmoCustomerShippingMethodID { get; set; }

	public byte cmoCustomerStatus { get; set; }

	public string cmoCustomerTaxCodeID { get; set; }

	public string cmoDefaultApInvoiceLocationID { get; set; }

	public string cmoDefaultArInvoiceLocationID { get; set; }

	public string cmoDefaultPurchaseLocationID { get; set; }

	public string cmoDefaultQuoteLocationID { get; set; }

	public string cmoDefaultShipLocationID { get; set; }

	public string cmoDropShipLocationID { get; set; }

	public string cmoDropShipOrganizationID { get; set; }

	public string cmoEftCode { get; set; }

	public string cmoEftDescription { get; set; }

	public string cmoEftParticulars { get; set; }

	public string cmoEmailAddress { get; set; }

	public int cmoEmployeeCount { get; set; }

	public Guid cmoUniqueID { get; set; }

	public DateTime? cmoEstablishedDate { get; set; }

	public decimal cmoExpenseSplitPercentTotal { get; set; }

	public string cmoFaxNumber { get; set; }

	public string cmoFederalID { get; set; }

	public string cmoFedEx3rdPartyLocationID { get; set; }

	public string cmoFedEx3rdPartyOrganizationID { get; set; }

	public string cmoFedExAccountNumber { get; set; }

	public string cmoFedExBillingOption { get; set; }

	public string cmoFirstGivenName { get; set; }

	public byte cmoForm1099Box { get; set; }

	public string cmoFreeOnBoardDescription { get; set; }

	public string cmoHdAttachmentFilePath { get; set; }

	public string cmoIban { get; set; }

	public string cmoIntraCompanyDatasetID { get; set; }

	public bool cmoApIncludeTaxInRetention { get; set; }

	public bool cmoArIncludeTaxInRetention { get; set; }

	public bool cmoArInvoicePerShipmentLine { get; set; }

	public bool cmoAvalaraAddressValidated { get; set; }

	public bool cmoBareCostOfDuty { get; set; }

	public bool cmoBareTransportationCost { get; set; }

	public bool cmoCalculateFinanceCharges { get; set; }

	public bool cmoCompetitor { get; set; }

	public bool cmoContractor { get; set; }

	public bool cmoCreatedFromMobile { get; set; }

	public bool cmoCreditHold { get; set; }

	public bool cmoCustomerTaxable { get; set; }

	public bool cmoDirectPayment { get; set; }

	public bool cmoEdiIntegrated { get; set; }

	public bool cmoFinanceCompany { get; set; }

	public bool cmoIgnoreAvalara { get; set; }

	public bool cmoIncludeFreightInPrice { get; set; }

	public bool cmoPrintStatement { get; set; }

	public bool cmoRequires1099 { get; set; }

	public bool cmoRequiresInspection { get; set; }

	public bool cmoResidentialAddress { get; set; }

	public bool cmoSuperFund { get; set; }

	public bool cmoSupplierAccredited { get; set; }

	public bool cmoSupplierTaxable { get; set; }

	public bool cmoTaxReportable { get; set; }

	public bool cmoUpsValidated { get; set; }

	public short cmoJobPriorityID { get; set; }

	public string cmoLastName { get; set; }

	public string cmoLongDescriptionRtf { get; set; }

	public string cmoLongDescriptionText { get; set; }

	public string cmoName { get; set; }

	public string cmoNonTaxReasonID { get; set; }

	public string cmoOrganizationAccountID { get; set; }

	public string cmoPhoneNumber { get; set; }

	public string cmoPostCode { get; set; }

	public string cmoPurchaseContactID { get; set; }

	public string cmoQuoteContactID { get; set; }

	public DateTime? cmoResellerActiveDate { get; set; }

	public decimal cmoResellerCommissionRate { get; set; }

	public string cmoResellerContactID { get; set; }

	public DateTime? cmoResellerInactiveDate { get; set; }

	public string cmoResellerLocationID { get; set; }

	public string cmoResellerOrganizationID { get; set; }

	public DateTime? cmoResellerProspectDate { get; set; }

	public byte cmoResellerStatus { get; set; }

	public byte[] cmoRowVersion { get; set; }

	public string cmoSecondGivenName { get; set; }

	public string cmoShipContactID { get; set; }

	public decimal cmoSplitPercentTotal { get; set; }

	public string cmoState { get; set; }

	public string cmoSuperFundEmployerID { get; set; }

	public string cmoSuperFundName { get; set; }

	public DateTime? cmoSupplierAccreditedDate { get; set; }

	public DateTime? cmoSupplierActiveDate { get; set; }

	public DateTime? cmoSupplierInactiveDate { get; set; }

	public string cmoSupplierPaymentTermID { get; set; }

	public DateTime? cmoSupplierProspectDate { get; set; }

	public string cmoSupplierRatingID { get; set; }

	public string cmoSupplierSecondTaxCodeID { get; set; }

	public string cmoSupplierShippingMethodID { get; set; }

	public byte cmoSupplierStatus { get; set; }

	public string cmoSupplierTaxCodeID { get; set; }

	public string cmoTaxExemptNumber { get; set; }

	public string cmoTradingName { get; set; }

	public string cmoUps3rdPartyLocationID { get; set; }

	public string cmoUps3rdPartyOrganizationID { get; set; }

	public string cmoUpsAcctNumber { get; set; }

	public string cmoUpsBillingOption { get; set; }

	public string cmoUpsWsBillingOption { get; set; }

	public string cmoUsaTransactionTypeCode { get; set; }

	public string cmoWebAddress { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
