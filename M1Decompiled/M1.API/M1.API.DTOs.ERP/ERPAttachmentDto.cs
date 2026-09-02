using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAttachmentDto
{
	[JsonProperty("cmaApInvoiceID", Order = 1)]
	[MaxLength(10)]
	public string cmaApInvoiceID { get; set; }

	[JsonProperty("cmaArInvoiceID", Order = 2)]
	[MaxLength(10)]
	public string cmaArInvoiceID { get; set; }

	[JsonProperty("cmaAttachmentTypeID", Order = 3)]
	[MaxLength(5)]
	public string cmaAttachmentTypeID { get; set; }

	[JsonProperty("cmaCallID", Order = 4)]
	[MaxLength(10)]
	public string cmaCallID { get; set; }

	[JsonProperty("cmaChangeRequestID", Order = 5)]
	[MaxLength(10)]
	public string cmaChangeRequestID { get; set; }

	[JsonProperty("cmaAttachmentID", Order = 6)]
	[Required(ErrorMessage = "cmaAttachmentID is required.")]
	[MaxLength(10)]
	public string cmaAttachmentID { get; set; }

	[JsonProperty("cmaContactID", Order = 7)]
	[MaxLength(5)]
	public string cmaContactID { get; set; }

	[JsonProperty("cmaCreatedBy", Order = 8)]
	[MaxLength(20)]
	public string cmaCreatedBy { get; set; }

	[JsonProperty("cmaCreatedDate", Order = 9)]
	public DateTime? cmaCreatedDate { get; set; }

	[JsonProperty("cmaCustomerGroupID", Order = 10)]
	[MaxLength(5)]
	public string cmaCustomerGroupID { get; set; }

	[JsonProperty("cmaDate", Order = 11)]
	[Required(ErrorMessage = "cmaDate is required.")]
	public DateTime? cmaDate { get; set; }

	[JsonProperty("cmaDmrClaimID", Order = 12)]
	[MaxLength(10)]
	public string cmaDmrClaimID { get; set; }

	[JsonProperty("cmaUniqueID", Order = 13)]
	public Guid cmaUniqueID { get; set; }

	[JsonProperty("cmaFileLocation", Order = 14)]
	[MaxLength(255)]
	public string cmaFileLocation { get; set; }

	[JsonProperty("cmaFilename", Order = 15)]
	[MaxLength(255)]
	public string cmaFilename { get; set; }

	[JsonProperty("cmaInspectionID", Order = 16)]
	[MaxLength(10)]
	public string cmaInspectionID { get; set; }

	[JsonProperty("cmaInspectionLineID", Order = 17)]
	public short cmaInspectionLineID { get; set; }

	[JsonProperty("cmaDoNotAllowDownload", Order = 18)]
	public bool cmaDoNotAllowDownload { get; set; }

	[JsonProperty("cmaEmailDefault", Order = 19)]
	public bool cmaEmailDefault { get; set; }

	[JsonProperty("cmaPrintDefault", Order = 20)]
	public bool cmaPrintDefault { get; set; }

	[JsonProperty("cmaReviewed", Order = 21)]
	public bool cmaReviewed { get; set; }

	[JsonProperty("cmaJobID", Order = 22)]
	[MaxLength(20)]
	public string cmaJobID { get; set; }

	[JsonProperty("cmaKnowledgeBasePageID", Order = 23)]
	[MaxLength(10)]
	public string cmaKnowledgeBasePageID { get; set; }

	[JsonProperty("cmaLeadID", Order = 24)]
	[MaxLength(10)]
	public string cmaLeadID { get; set; }

	[JsonProperty("cmaLocationID", Order = 25)]
	[MaxLength(5)]
	public string cmaLocationID { get; set; }

	[JsonProperty("cmaLongDescriptionRtf", Order = 26)]
	public string cmaLongDescriptionRtf { get; set; }

	[JsonProperty("cmaLongDescriptionText", Order = 27)]
	public string cmaLongDescriptionText { get; set; }

	[JsonProperty("cmaNonConformanceID", Order = 28)]
	[MaxLength(10)]
	public string cmaNonConformanceID { get; set; }

	[JsonProperty("cmaOrganizationID", Order = 29)]
	[MaxLength(10)]
	public string cmaOrganizationID { get; set; }

	[JsonProperty("cmaPartID", Order = 30)]
	[MaxLength(30)]
	public string cmaPartID { get; set; }

	[JsonProperty("cmaProjectAreaID", Order = 31)]
	[MaxLength(15)]
	public string cmaProjectAreaID { get; set; }

	[JsonProperty("cmaProjectID", Order = 32)]
	[MaxLength(10)]
	public string cmaProjectID { get; set; }

	[JsonProperty("cmaPurchaseOrderID", Order = 33)]
	[MaxLength(10)]
	public string cmaPurchaseOrderID { get; set; }

	[JsonProperty("cmaQuoteID", Order = 34)]
	[MaxLength(10)]
	public string cmaQuoteID { get; set; }

	[JsonProperty("cmaReceiptID", Order = 35)]
	[MaxLength(10)]
	public string cmaReceiptID { get; set; }

	[JsonProperty("cmaRfqID", Order = 36)]
	[MaxLength(10)]
	public string cmaRfqID { get; set; }

	[JsonProperty("cmaRmaClaimID", Order = 37)]
	[MaxLength(10)]
	public string cmaRmaClaimID { get; set; }

	[JsonProperty("cmaRowVersion", Order = 38)]
	public byte[] cmaRowVersion { get; set; }

	[JsonProperty("cmaSalesOrderID", Order = 39)]
	[MaxLength(10)]
	public string cmaSalesOrderID { get; set; }

	[JsonProperty("cmaShipmentID", Order = 40)]
	[MaxLength(10)]
	public string cmaShipmentID { get; set; }

	[JsonProperty("cmaShortDescription", Order = 41)]
	[Required(ErrorMessage = "cmaShortDescription is required.")]
	[MaxLength(70)]
	public string cmaShortDescription { get; set; }

	[JsonProperty("cmaWorkFlowID", Order = 42)]
	[MaxLength(10)]
	public string cmaWorkFlowID { get; set; }

	[JsonProperty("cmaWorkFlowLineID", Order = 43)]
	public short cmaWorkFlowLineID { get; set; }

	[JsonProperty("customFields", Order = 44)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
