using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPFollowupInformationDto
{
	public string cmfApInvoiceID { get; set; }

	public string cmfArInvoiceID { get; set; }

	public string cmfAssetID { get; set; }

	public string cmfAssignedToEmployeeID { get; set; }

	public string cmfAttachedToEmployeeID { get; set; }

	public string cmfCallID { get; set; }

	public string cmfChangeRequestID { get; set; }

	public string cmfFollowupID { get; set; }

	public DateTime? cmfCompletedDate { get; set; }

	public string cmfContactID { get; set; }

	public string cmfCreatedBy { get; set; }

	public DateTime? cmfCreatedDate { get; set; }

	public string cmfDmrClaimID { get; set; }

	public DateTime? cmfDueDate { get; set; }

	public Guid cmfUniqueID { get; set; }

	public string cmfExchangeID { get; set; }

	public byte cmfFollowupType { get; set; }

	public bool cmfCreatedFromMobile { get; set; }

	public string cmfJobID { get; set; }

	public string cmfLeadID { get; set; }

	public string cmfLocationID { get; set; }

	public string cmfLongDescriptionRtf { get; set; }

	public string cmfLongDescriptionText { get; set; }

	public string cmfMeetingLocation { get; set; }

	public string cmfOrganizationID { get; set; }

	public byte cmfPriority { get; set; }

	public string cmfProjectAreaID { get; set; }

	public string cmfProjectID { get; set; }

	public string cmfPurchaseOrderID { get; set; }

	public string cmfQuoteID { get; set; }

	public string cmfReceiptID { get; set; }

	public string cmfRfqID { get; set; }

	public string cmfRmaClaimID { get; set; }

	public byte[] cmfRowVersion { get; set; }

	public string cmfSalesOrderID { get; set; }

	public string cmfShipmentID { get; set; }

	public string cmfShortDescription { get; set; }

	public DateTime? cmfStartDate { get; set; }

	public byte cmfStatus { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
