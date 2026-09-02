using System.Collections.Generic;
using M1.API.DTOs.Core;

namespace M1.API.DTOs.Custom;

public class OrganizationInformationDto
{
	public string CustomerOrganizationID { get; set; } = string.Empty;

	public string ShipOrganizationID { get; set; } = string.Empty;

	public string ARInvoiceLocationID { get; set; } = string.Empty;

	public string ARInvoiceContactID { get; set; } = string.Empty;

	public string ShipLocationID { get; set; } = string.Empty;

	public string ShipContactID { get; set; } = string.Empty;

	public string CurrencyRateID { get; set; } = string.Empty;

	public string PaymentTermsID { get; set; } = string.Empty;

	public string ShippingPaymentTypeID { get; set; } = string.Empty;

	public string TaxCodeID { get; set; } = string.Empty;

	public bool IsAnonymousCustomer { get; set; }

	public IList<SalesOrderSalespeopleDto> ShipLocationSalesPeople { get; set; } = new List<SalesOrderSalespeopleDto>();

	public OrganizationLocationDto ShipLocation { get; set; } = new OrganizationLocationDto();

	public OrganizationLocationDto ARInvoiceLocation { get; set; } = new OrganizationLocationDto();

	public IList<string> WarningsList { get; set; } = new List<string>();

	public IList<string> ErrorsList { get; set; } = new List<string>();
}
