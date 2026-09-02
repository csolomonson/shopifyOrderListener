using System;
using System.Collections.Generic;

namespace M1.API.DTOs.Core;

public class OrganizationLocationDto
{
	public string OrganizationID { get; set; }

	public string LocationID { get; set; }

	public string Name { get; set; }

	public bool ARInvoiceLocation { get; set; }

	public string ARInvoiceContactID { get; set; }

	public bool ShipLocation { get; set; }

	public string ShipContactID { get; set; }

	public string AddressLine1 { get; set; }

	public string AddressLine2 { get; set; }

	public string AddressLine3 { get; set; }

	public string City { get; set; }

	public string CountryCode { get; set; }

	public string Country { get; set; }

	public string EMailAddress { get; set; }

	public bool Inactive { get; set; }

	public string PhoneNumber { get; set; }

	public string PostCode { get; set; }

	public string State { get; set; }

	public string CustomerShippingMethodID { get; set; }

	public string CustomerPaymentTermID { get; set; }

	public string UPSAcctNumber { get; set; }

	public bool UPSValidated { get; set; }

	public string EDILocationID { get; set; }

	public string CustomerTaxCodeID { get; set; }

	public string CustomerSecondTaxCodeID { get; set; }

	public string CreatedBy { get; set; }

	public DateTime CreatedDate { get; set; }

	public IList<OrganizationContactDto> OrganizationContacts { get; } = new List<OrganizationContactDto>();
}
