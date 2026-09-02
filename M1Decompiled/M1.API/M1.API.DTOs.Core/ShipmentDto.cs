using System;
using System.Collections.Generic;

namespace M1.API.DTOs.Core;

public class ShipmentDto
{
	public string ShipmentID { get; set; }

	public DateTime? ShipDate { get; set; }

	public string CustomerOrganizationID { get; set; }

	public string ShipOrganizationID { get; set; }

	public string ShipLocationID { get; set; }

	public string ShipContactID { get; set; }

	public string ARInvoiceLocationID { get; set; }

	public string ARInvoiceContactID { get; set; }

	public string PlantID { get; set; }

	public string ShippingMethodID { get; set; }

	public string TrackingNumber { get; set; }

	public decimal WeightTotal { get; set; }

	public decimal AdditionalWeight { get; set; }

	public string CurrencyRateID { get; set; }

	public string ShippingCommentsText { get; set; }

	public IList<ShipmentLineDto> ShipmentLines { get; set; }

	public IList<ShipmentPackageDto> ShipmentPackages { get; set; }
}
