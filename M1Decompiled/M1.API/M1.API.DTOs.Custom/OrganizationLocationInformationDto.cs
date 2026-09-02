using System;

namespace M1.API.DTOs.Custom;

public class OrganizationLocationInformationDto
{
	public string OrganizationID { get; set; }

	public string LocationID { get; set; }

	public string Name { get; set; }

	public string AddressLine1 { get; set; }

	public string AddressLine2 { get; set; }

	public string AddressLine3 { get; set; }

	public string City { get; set; }

	public string County { get; set; }

	public string State { get; set; }

	public string PostCode { get; set; }

	public string Country { get; set; }

	public string PhoneNumber { get; set; }

	public string EmailAddress { get; set; }

	public bool QuoteLocation { get; set; }

	public string QuoteContactID { get; set; }

	public bool ShipLocation { get; set; }

	public string ShipContactID { get; set; }

	public bool ArInvoiceLocation { get; set; }

	public string ArInvoiceContactID { get; set; }

	public bool PurchaseLocation { get; set; }

	public string PurchaseContactID { get; set; }

	public bool ApInvoiceLocation { get; set; }

	public string ApInvoiceContactID { get; set; }

	public bool CustomerTaxable { get; set; }

	public string CustomerTaxCodeID { get; set; }

	public string CustomerSecondTaxCodeID { get; set; }

	public string CustomerShippingMethodID { get; set; }

	public string CustomerShipPaymentTypeID { get; set; }

	public string TaxExemptNumber { get; set; }

	public string NonTaxReasonID { get; set; }

	public string CustomerPaymentTermID { get; set; }

	public string CurrencyRateID { get; set; }

	public string SupplierPaymentTermID { get; set; }

	public string SupplierShippingMethodID { get; set; }

	public bool Inactive { get; set; }

	public DateTime? InactiveDate { get; set; }

	public string CustomerShippingCarrier { get; set; }

	public string UpsAcctNumber { get; set; }

	public string UpsWsBillingOption { get; set; }

	public string Ups3rdPartyOrganizationID { get; set; }

	public string Ups3rdPartyLocationID { get; set; }

	public bool ResidentialAddress { get; set; }

	public string FedExAccountNumber { get; set; }

	public string FedEx3rdPartyOrganizationID { get; set; }

	public string FedExBillingOption { get; set; }

	public bool CreditCheckForLocation { get; set; }

	public decimal CustomerCreditLimit { get; set; }

	public bool CreditHold { get; set; }

	public string CountryCode { get; set; }

	public DateTime? CreatedDate { get; set; }

	public string CreatedBy { get; set; }

	public string AvalaraUseCodes { get; set; }

	public Guid UniqueID { get; set; }

	public byte[] RowVersion { get; set; }
}
