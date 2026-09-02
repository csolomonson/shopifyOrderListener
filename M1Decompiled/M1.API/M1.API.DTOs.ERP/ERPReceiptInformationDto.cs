using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPReceiptInformationDto
{
	public string rmpApInvoiceContactID { get; set; }

	public string rmpApInvoiceLocationID { get; set; }

	public DateTime? rmpClosedDate { get; set; }

	public string rmpReceiptID { get; set; }

	public string rmpCreatedBy { get; set; }

	public DateTime? rmpCreatedDate { get; set; }

	public string rmpCurrencyRateID { get; set; }

	public string rmpDeliveryDocket { get; set; }

	public Guid rmpUniqueID { get; set; }

	public decimal rmpExchangeRate { get; set; }

	public decimal rmpFreightCharge { get; set; }

	public decimal rmpFreightChargeForeign { get; set; }

	public bool rmpClosed { get; set; }

	public bool rmpCustomRate { get; set; }

	public bool rmpNestlinkProcessed { get; set; }

	public bool rmpPostedToGl { get; set; }

	public bool rmpReversalEntry { get; set; }

	public bool rmpReversed { get; set; }

	public string rmpLandedCostID { get; set; }

	public string rmpPlantDepartmentID { get; set; }

	public string rmpPlantID { get; set; }

	public DateTime? rmpPostedDate { get; set; }

	public string rmpProjectID { get; set; }

	public string rmpPurchaseContactID { get; set; }

	public string rmpPurchaseLocationID { get; set; }

	public DateTime? rmpReceiptDate { get; set; }

	public decimal rmpReceiptSubtotal { get; set; }

	public decimal rmpReceiptSubtotalForeign { get; set; }

	public decimal rmpReceiptTotal { get; set; }

	public decimal rmpReceiptTotalForeign { get; set; }

	public byte[] rmpRowVersion { get; set; }

	public string rmpShippingMethodID { get; set; }

	public string rmpSupplierOrganizationID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
