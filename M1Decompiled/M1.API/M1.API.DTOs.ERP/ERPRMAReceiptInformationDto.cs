using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRMAReceiptInformationDto
{
	public string rrpArInvoiceContactID { get; set; }

	public string rrpArInvoiceLocationID { get; set; }

	public DateTime? rrpClosedDate { get; set; }

	public string rrpRmaReceiptID { get; set; }

	public string rrpCreatedBy { get; set; }

	public DateTime? rrpCreatedDate { get; set; }

	public string rrpCurrencyRateID { get; set; }

	public string rrpCustomerOrganizationID { get; set; }

	public string rrpDeliveryDocket { get; set; }

	public Guid rrpUniqueID { get; set; }

	public decimal rrpExchangeRate { get; set; }

	public decimal rrpFreightCharge { get; set; }

	public decimal rrpFreightChargeForeign { get; set; }

	public bool rrpClosed { get; set; }

	public bool rrpCustomRate { get; set; }

	public bool rrpPosted { get; set; }

	public bool rrpReversalEntry { get; set; }

	public bool rrpReversed { get; set; }

	public string rrpPlantDepartmentID { get; set; }

	public string rrpPlantID { get; set; }

	public DateTime? rrpPostedDate { get; set; }

	public string rrpProjectID { get; set; }

	public DateTime? rrpReceiptDate { get; set; }

	public byte[] rrpRowVersion { get; set; }

	public string rrpShipContactID { get; set; }

	public string rrpShipLocationID { get; set; }

	public string rrpShipOrganizationID { get; set; }

	public string rrpShippingMethodID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
