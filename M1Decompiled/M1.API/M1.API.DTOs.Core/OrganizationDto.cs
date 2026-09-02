using System;
using System.Collections.Generic;

namespace M1.API.DTOs.Core;

public class OrganizationDto
{
	public string OrganizationID { get; set; }

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

	public string ARInvoiceContactID { get; set; }

	public string ShipContactID { get; set; }

	public string CountryCode { get; set; }

	public string CustomerPaymentTermsID { get; set; }

	public string CustomerTaxCodeID { get; set; }

	public string CurrencyRateID { get; set; }

	public string CustomerShippingMethodID { get; set; }

	public bool EDIIntegrated { get; set; }

	public string EMailAddress { get; set; }

	public string UPSAcctNumber { get; set; }

	public bool UPSValidated { get; set; }

	public string CreatedBy { get; set; }

	public DateTime CreatedDate { get; set; }

	public IList<OrganizationLocationDto> OrganizationLocations { get; } = new List<OrganizationLocationDto>();

	public string SuperFundName { get; set; }

	public string SuperFundEmployerID { get; set; }

	public bool BareCostOfDuty { get; set; }

	public bool BareTransportationCost { get; set; }

	public string FedExAccountNumber { get; set; }

	public string FedEx3rdPartyOrganizationID { get; set; }

	public string FedEx3rdPartyLocationID { get; set; }

	public string FedExBillingOption { get; set; }
}
