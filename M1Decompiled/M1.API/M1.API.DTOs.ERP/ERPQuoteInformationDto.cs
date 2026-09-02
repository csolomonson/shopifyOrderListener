using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPQuoteInformationDto
{
	public string qmpArInvoiceContactID { get; set; }

	public string qmpArInvoiceLocationID { get; set; }

	public DateTime? qmpClosedDate { get; set; }

	public string qmpQuoteID { get; set; }

	public string qmpCreatedBy { get; set; }

	public DateTime? qmpCreatedDate { get; set; }

	public string qmpCurrencyRateID { get; set; }

	public string qmpCustomerOrganizationID { get; set; }

	public DateTime? qmpDueDate { get; set; }

	public Guid qmpUniqueID { get; set; }

	public decimal qmpExchangeRate { get; set; }

	public DateTime? qmpExpirationDate { get; set; }

	public string qmpFreeOnBoardDescription { get; set; }

	public bool qmpAvalaraTaxCalculated { get; set; }

	public bool qmpClosed { get; set; }

	public bool qmpCreatedFromMobile { get; set; }

	public bool qmpCustomRate { get; set; }

	public string qmpPaymentTermID { get; set; }

	public string qmpPlantDepartmentID { get; set; }

	public string qmpPlantID { get; set; }

	public string qmpProjectID { get; set; }

	public string qmpQuoteContactID { get; set; }

	public DateTime? qmpQuoteDate { get; set; }

	public string qmpQuoteFooterMessageRTF { get; set; }

	public string qmpQuoteFooterMessageText { get; set; }

	public string qmpQuoteHeaderMessageRTF { get; set; }

	public string qmpQuoteHeaderMessageText { get; set; }

	public string qmpQuoteLocationID { get; set; }

	public string qmpQuoterEmployeeID { get; set; }

	public byte[] qmpRowVersion { get; set; }

	public string qmpShipContactID { get; set; }

	public string qmpShipLocationID { get; set; }

	public string qmpShipOrganizationID { get; set; }

	public string qmpShippingMethodID { get; set; }

	public string qmpShippingPaymentTypeID { get; set; }

	public decimal qmpSplitPercentTotal { get; set; }

	public string qmpStandardMessageID { get; set; }

	public DateTime? qmpTaxDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
