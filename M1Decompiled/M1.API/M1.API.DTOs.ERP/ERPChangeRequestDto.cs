using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPChangeRequestDto
{
	[JsonProperty("chpActualHours", Order = 1)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal chpActualHours { get; set; }

	[JsonProperty("chpAssignedDate", Order = 2)]
	public DateTime? chpAssignedDate { get; set; }

	[JsonProperty("chpAssignedToEmployeeID", Order = 3)]
	[MaxLength(10)]
	public string chpAssignedToEmployeeID { get; set; }

	[JsonProperty("chpAuthorizedByEmployeeID", Order = 4)]
	[MaxLength(10)]
	public string chpAuthorizedByEmployeeID { get; set; }

	[JsonProperty("chpAuthorizedDate", Order = 5)]
	public DateTime? chpAuthorizedDate { get; set; }

	[JsonProperty("chpChangeRequestTypeID", Order = 6)]
	[Required(ErrorMessage = "chpChangeRequestTypeID is required.")]
	[MaxLength(5)]
	public string chpChangeRequestTypeID { get; set; }

	[JsonProperty("chpClosedByEmployeeID", Order = 7)]
	[MaxLength(10)]
	public string chpClosedByEmployeeID { get; set; }

	[JsonProperty("chpClosedDate", Order = 8)]
	public DateTime? chpClosedDate { get; set; }

	[JsonProperty("chpClosedReasonID", Order = 9)]
	[MaxLength(5)]
	public string chpClosedReasonID { get; set; }

	[JsonProperty("chpChangeRequestID", Order = 10)]
	[Required(ErrorMessage = "chpChangeRequestID is required.")]
	[MaxLength(10)]
	public string chpChangeRequestID { get; set; }

	[JsonProperty("chpCreatedBy", Order = 11)]
	[MaxLength(20)]
	public string chpCreatedBy { get; set; }

	[JsonProperty("chpCreatedDate", Order = 12)]
	public DateTime? chpCreatedDate { get; set; }

	[JsonProperty("chpDueDate", Order = 13)]
	public DateTime? chpDueDate { get; set; }

	[JsonProperty("chpUniqueID", Order = 14)]
	public Guid chpUniqueID { get; set; }

	[JsonProperty("chpEstimatedHours", Order = 15)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal chpEstimatedHours { get; set; }

	[JsonProperty("chpJobID", Order = 16)]
	[MaxLength(20)]
	public string chpJobID { get; set; }

	[JsonProperty("chpLongDescriptionRtf", Order = 17)]
	public string chpLongDescriptionRtf { get; set; }

	[JsonProperty("chpLongDescriptionText", Order = 18)]
	public string chpLongDescriptionText { get; set; }

	[JsonProperty("chpNonConformanceID", Order = 19)]
	[MaxLength(10)]
	public string chpNonConformanceID { get; set; }

	[JsonProperty("chpOpenedByEmployeeID", Order = 20)]
	[Required(ErrorMessage = "chpOpenedByEmployeeID is required.")]
	[MaxLength(10)]
	public string chpOpenedByEmployeeID { get; set; }

	[JsonProperty("chpOpenedDate", Order = 21)]
	[Required(ErrorMessage = "chpOpenedDate is required.")]
	public DateTime? chpOpenedDate { get; set; }

	[JsonProperty("chpPartID", Order = 22)]
	[Required(ErrorMessage = "chpPartID is required.")]
	[MaxLength(30)]
	public string chpPartID { get; set; }

	[JsonProperty("chpPartRevisionID", Order = 23)]
	[MaxLength(15)]
	public string chpPartRevisionID { get; set; }

	[JsonProperty("chpPriorityID", Order = 24)]
	public byte chpPriorityID { get; set; }

	[JsonProperty("chpProjectAreaID", Order = 25)]
	[MaxLength(15)]
	public string chpProjectAreaID { get; set; }

	[JsonProperty("chpProjectID", Order = 26)]
	[MaxLength(10)]
	public string chpProjectID { get; set; }

	[JsonProperty("chpResolvedPartID", Order = 27)]
	[MaxLength(30)]
	public string chpResolvedPartID { get; set; }

	[JsonProperty("chpResolvedPartRevisionID", Order = 28)]
	[MaxLength(15)]
	public string chpResolvedPartRevisionID { get; set; }

	[JsonProperty("chpRowVersion", Order = 29)]
	public byte[] chpRowVersion { get; set; }

	[JsonProperty("chpShortDescription", Order = 30)]
	[Required(ErrorMessage = "chpShortDescription is required.")]
	[MaxLength(70)]
	public string chpShortDescription { get; set; }

	[JsonProperty("chpStatus", Order = 31)]
	[Required(ErrorMessage = "chpStatus is required.")]
	[MaxLength(1)]
	public string chpStatus { get; set; }

	[JsonProperty("customFields", Order = 32)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
