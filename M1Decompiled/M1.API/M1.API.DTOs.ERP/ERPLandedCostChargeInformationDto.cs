using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLandedCostChargeInformationDto
{
	public string rmhApInvoiceID { get; set; }

	public short rmhApInvoiceLineID { get; set; }

	public string rmhCreatedBy { get; set; }

	public DateTime? rmhCreatedDate { get; set; }

	public string rmhCurrencyRateID { get; set; }

	public string rmhDescription { get; set; }

	public Guid rmhUniqueID { get; set; }

	public decimal rmhEstExchangeRate { get; set; }

	public decimal rmhEstTotalCost { get; set; }

	public decimal rmhEstTotalCostForeign { get; set; }

	public decimal rmhExchangeRate { get; set; }

	public bool rmhCustomRate { get; set; }

	public bool rmhInTransitJournalsCreated { get; set; }

	public bool rmhInvoicedComplete { get; set; }

	public bool rmhReversed { get; set; }

	public string rmhLandedCostCategoryID { get; set; }

	public string rmhLandedCostID { get; set; }

	public byte rmhLandedCostMethod { get; set; }

	public short rmhReverseLandedCostChargeID { get; set; }

	public string rmhReverseLandedCostID { get; set; }

	public byte[] rmhRowVersion { get; set; }

	public short rmhLandedCostChargeID { get; set; }

	public string rmhSupplierContactID { get; set; }

	public string rmhSupplierLocationID { get; set; }

	public string rmhSupplierOrganizationID { get; set; }

	public decimal rmhTotalCost { get; set; }

	public decimal rmhTotalCostForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
