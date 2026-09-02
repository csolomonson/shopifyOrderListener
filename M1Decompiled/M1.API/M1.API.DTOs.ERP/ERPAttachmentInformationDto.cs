using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAttachmentInformationDto
{
	public string cmaApInvoiceID { get; set; }

	public string cmaArInvoiceID { get; set; }

	public string cmaAttachmentTypeID { get; set; }

	public string cmaCallID { get; set; }

	public string cmaChangeRequestID { get; set; }

	public string cmaAttachmentID { get; set; }

	public string cmaContactID { get; set; }

	public string cmaCreatedBy { get; set; }

	public DateTime? cmaCreatedDate { get; set; }

	public string cmaCustomerGroupID { get; set; }

	public DateTime? cmaDate { get; set; }

	public string cmaDmrClaimID { get; set; }

	public Guid cmaUniqueID { get; set; }

	public string cmaFileLocation { get; set; }

	public string cmaFilename { get; set; }

	public string cmaInspectionID { get; set; }

	public short cmaInspectionLineID { get; set; }

	public bool cmaDoNotAllowDownload { get; set; }

	public bool cmaEmailDefault { get; set; }

	public bool cmaPrintDefault { get; set; }

	public bool cmaReviewed { get; set; }

	public string cmaJobID { get; set; }

	public string cmaKnowledgeBasePageID { get; set; }

	public string cmaLeadID { get; set; }

	public string cmaLocationID { get; set; }

	public string cmaLongDescriptionRtf { get; set; }

	public string cmaLongDescriptionText { get; set; }

	public string cmaNonConformanceID { get; set; }

	public string cmaOrganizationID { get; set; }

	public string cmaPartID { get; set; }

	public string cmaProjectAreaID { get; set; }

	public string cmaProjectID { get; set; }

	public string cmaPurchaseOrderID { get; set; }

	public string cmaQuoteID { get; set; }

	public string cmaReceiptID { get; set; }

	public string cmaRfqID { get; set; }

	public string cmaRmaClaimID { get; set; }

	public byte[] cmaRowVersion { get; set; }

	public string cmaSalesOrderID { get; set; }

	public string cmaShipmentID { get; set; }

	public string cmaShortDescription { get; set; }

	public string cmaWorkFlowID { get; set; }

	public short cmaWorkFlowLineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
