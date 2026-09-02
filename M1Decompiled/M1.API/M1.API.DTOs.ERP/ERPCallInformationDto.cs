using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCallInformationDto
{
	public DateTime? kbpAcceptedDate { get; set; }

	public string kbpApInvoiceID { get; set; }

	public string kbpArInvoiceContactID { get; set; }

	public string kbpArInvoiceID { get; set; }

	public string kbpArInvoiceLocationID { get; set; }

	public string kbpArInvoiceOrganizationID { get; set; }

	public DateTime? kbpAssignedDate { get; set; }

	public string kbpAssignedToEmployeeID { get; set; }

	public string kbpCallTypeID { get; set; }

	public string kbpClosedByEmployeeID { get; set; }

	public DateTime? kbpClosedDate { get; set; }

	public string kbpCallID { get; set; }

	public string kbpContactID { get; set; }

	public string kbpContactMethodID { get; set; }

	public string kbpCreatedBy { get; set; }

	public DateTime? kbpCreatedDate { get; set; }

	public string kbpCurrencyRateID { get; set; }

	public string kbpDmrClaimID { get; set; }

	public DateTime? kbpDueDate { get; set; }

	public Guid kbpUniqueID { get; set; }

	public decimal kbpExchangeRate { get; set; }

	public decimal kbpExtraTime { get; set; }

	public bool kbpBillable { get; set; }

	public bool kbpCreatedFromMobile { get; set; }

	public bool kbpCustomRate { get; set; }

	public bool kbpFieldServiceCall { get; set; }

	public bool kbpFieldServiceJobCreated { get; set; }

	public bool kbpInbound { get; set; }

	public bool kbpInternalOnly { get; set; }

	public bool kbpInvoicedComplete { get; set; }

	public bool kbpPublished { get; set; }

	public string kbpJobID { get; set; }

	public string kbpLeadID { get; set; }

	public string kbpLocationID { get; set; }

	public string kbpLongDescriptionRtf { get; set; }

	public string kbpLongDescriptionText { get; set; }

	public string kbpMethodPartID { get; set; }

	public string kbpMethodRevisionID { get; set; }

	public string kbpOpenedByEmployeeID { get; set; }

	public DateTime? kbpOpenedDate { get; set; }

	public string kbpOrganizationID { get; set; }

	public string kbpOrgPartID { get; set; }

	public string kbpPartGroupID { get; set; }

	public string kbpPartID { get; set; }

	public string kbpPartRevisionID { get; set; }

	public string kbpPartShortDescription { get; set; }

	public string kbpPhoneNumber { get; set; }

	public byte kbpPriorityID { get; set; }

	public string kbpProjectAreaID { get; set; }

	public string kbpProjectID { get; set; }

	public string kbpPurchaseOrderID { get; set; }

	public string kbpQuoteID { get; set; }

	public string kbpReasonID { get; set; }

	public string kbpReceiptID { get; set; }

	public string kbpRfqID { get; set; }

	public string kbpRmaClaimID { get; set; }

	public byte[] kbpRowVersion { get; set; }

	public string kbpSalesOrderID { get; set; }

	public string kbpSerialNumberID { get; set; }

	public string kbpShipmentID { get; set; }

	public string kbpShortDescription { get; set; }

	public string kbpStatus { get; set; }

	public decimal kbpSubTotalTime { get; set; }

	public string kbpTemplateFile { get; set; }

	public decimal kbpTimeSpent { get; set; }

	public decimal kbpTotalTime { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
