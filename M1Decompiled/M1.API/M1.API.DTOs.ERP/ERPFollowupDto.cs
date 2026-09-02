using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPFollowupDto
{
	[JsonProperty("cmfApInvoiceID", Order = 1)]
	[MaxLength(10)]
	public string cmfApInvoiceID { get; set; }

	[JsonProperty("cmfArInvoiceID", Order = 2)]
	[MaxLength(10)]
	public string cmfArInvoiceID { get; set; }

	[JsonProperty("cmfAssetID", Order = 3)]
	[MaxLength(10)]
	public string cmfAssetID { get; set; }

	[JsonProperty("cmfAssignedToEmployeeID", Order = 4)]
	[Required(ErrorMessage = "cmfAssignedToEmployeeID is required.")]
	[MaxLength(10)]
	public string cmfAssignedToEmployeeID { get; set; }

	[JsonProperty("cmfAttachedToEmployeeID", Order = 5)]
	[MaxLength(10)]
	public string cmfAttachedToEmployeeID { get; set; }

	[JsonProperty("cmfCallID", Order = 6)]
	[MaxLength(10)]
	public string cmfCallID { get; set; }

	[JsonProperty("cmfChangeRequestID", Order = 7)]
	[MaxLength(10)]
	public string cmfChangeRequestID { get; set; }

	[JsonProperty("cmfFollowupID", Order = 8)]
	[Required(ErrorMessage = "cmfFollowupID is required.")]
	[MaxLength(10)]
	public string cmfFollowupID { get; set; }

	[JsonProperty("cmfCompletedDate", Order = 9)]
	public DateTime? cmfCompletedDate { get; set; }

	[JsonProperty("cmfContactID", Order = 10)]
	[MaxLength(5)]
	public string cmfContactID { get; set; }

	[JsonProperty("cmfCreatedBy", Order = 11)]
	[MaxLength(20)]
	public string cmfCreatedBy { get; set; }

	[JsonProperty("cmfCreatedDate", Order = 12)]
	public DateTime? cmfCreatedDate { get; set; }

	[JsonProperty("cmfDmrClaimID", Order = 13)]
	[MaxLength(10)]
	public string cmfDmrClaimID { get; set; }

	[JsonProperty("cmfDueDate", Order = 14)]
	[Required(ErrorMessage = "cmfDueDate is required.")]
	public DateTime? cmfDueDate { get; set; }

	[JsonProperty("cmfUniqueID", Order = 15)]
	public Guid cmfUniqueID { get; set; }

	[JsonProperty("cmfExchangeID", Order = 16)]
	[MaxLength(50)]
	public string cmfExchangeID { get; set; }

	[JsonProperty("cmfFollowupType", Order = 17)]
	[Required(ErrorMessage = "cmfFollowupType is required.")]
	public byte cmfFollowupType { get; set; }

	[JsonProperty("cmfCreatedFromMobile", Order = 18)]
	public bool cmfCreatedFromMobile { get; set; }

	[JsonProperty("cmfJobID", Order = 19)]
	[MaxLength(20)]
	public string cmfJobID { get; set; }

	[JsonProperty("cmfLeadID", Order = 20)]
	[MaxLength(10)]
	public string cmfLeadID { get; set; }

	[JsonProperty("cmfLocationID", Order = 21)]
	[MaxLength(5)]
	public string cmfLocationID { get; set; }

	[JsonProperty("cmfLongDescriptionRtf", Order = 22)]
	public string cmfLongDescriptionRtf { get; set; }

	[JsonProperty("cmfLongDescriptionText", Order = 23)]
	public string cmfLongDescriptionText { get; set; }

	[JsonProperty("cmfMeetingLocation", Order = 24)]
	[MaxLength(50)]
	public string cmfMeetingLocation { get; set; }

	[JsonProperty("cmfOrganizationID", Order = 25)]
	[MaxLength(10)]
	public string cmfOrganizationID { get; set; }

	[JsonProperty("cmfPriority", Order = 26)]
	[Required(ErrorMessage = "cmfPriority is required.")]
	public byte cmfPriority { get; set; }

	[JsonProperty("cmfProjectAreaID", Order = 27)]
	[MaxLength(15)]
	public string cmfProjectAreaID { get; set; }

	[JsonProperty("cmfProjectID", Order = 28)]
	[MaxLength(10)]
	public string cmfProjectID { get; set; }

	[JsonProperty("cmfPurchaseOrderID", Order = 29)]
	[MaxLength(10)]
	public string cmfPurchaseOrderID { get; set; }

	[JsonProperty("cmfQuoteID", Order = 30)]
	[MaxLength(10)]
	public string cmfQuoteID { get; set; }

	[JsonProperty("cmfReceiptID", Order = 31)]
	[MaxLength(10)]
	public string cmfReceiptID { get; set; }

	[JsonProperty("cmfRfqID", Order = 32)]
	[MaxLength(10)]
	public string cmfRfqID { get; set; }

	[JsonProperty("cmfRmaClaimID", Order = 33)]
	[MaxLength(10)]
	public string cmfRmaClaimID { get; set; }

	[JsonProperty("cmfRowVersion", Order = 34)]
	public byte[] cmfRowVersion { get; set; }

	[JsonProperty("cmfSalesOrderID", Order = 35)]
	[MaxLength(10)]
	public string cmfSalesOrderID { get; set; }

	[JsonProperty("cmfShipmentID", Order = 36)]
	[MaxLength(10)]
	public string cmfShipmentID { get; set; }

	[JsonProperty("cmfShortDescription", Order = 37)]
	[Required(ErrorMessage = "cmfShortDescription is required.")]
	[MaxLength(50)]
	public string cmfShortDescription { get; set; }

	[JsonProperty("cmfStartDate", Order = 38)]
	public DateTime? cmfStartDate { get; set; }

	[JsonProperty("cmfStatus", Order = 39)]
	[Required(ErrorMessage = "cmfStatus is required.")]
	public byte cmfStatus { get; set; }

	[JsonProperty("customFields", Order = 40)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
