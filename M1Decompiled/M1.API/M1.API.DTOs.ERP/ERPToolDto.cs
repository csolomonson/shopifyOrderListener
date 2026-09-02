using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPToolDto
{
	[JsonProperty("xttAssetID", Order = 1)]
	[MaxLength(10)]
	public string xttAssetID { get; set; }

	[JsonProperty("xttCheckedOutToEmployeeID", Order = 2)]
	[MaxLength(10)]
	public string xttCheckedOutToEmployeeID { get; set; }

	[JsonProperty("xttCheckoutReasonID", Order = 3)]
	[MaxLength(5)]
	public string xttCheckoutReasonID { get; set; }

	[JsonProperty("xttToolID", Order = 4)]
	[Required(ErrorMessage = "xttToolID is required.")]
	[MaxLength(10)]
	public string xttToolID { get; set; }

	[JsonProperty("xttCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string xttCreatedBy { get; set; }

	[JsonProperty("xttCreatedDate", Order = 6)]
	public DateTime? xttCreatedDate { get; set; }

	[JsonProperty("xttDescription", Order = 7)]
	[Required(ErrorMessage = "xttDescription is required.")]
	[MaxLength(50)]
	public string xttDescription { get; set; }

	[JsonProperty("xttDocuments", Order = 8)]
	[MaxLength(50)]
	public string xttDocuments { get; set; }

	[JsonProperty("xttUniqueID", Order = 9)]
	public Guid xttUniqueID { get; set; }

	[JsonProperty("xttIdentificationNumber", Order = 10)]
	[MaxLength(30)]
	public string xttIdentificationNumber { get; set; }

	[JsonProperty("xttInactiveDate", Order = 11)]
	public DateTime? xttInactiveDate { get; set; }

	[JsonProperty("xttInactive", Order = 12)]
	public bool xttInactive { get; set; }

	[JsonProperty("xttLocation", Order = 13)]
	[MaxLength(30)]
	public string xttLocation { get; set; }

	[JsonProperty("xttLongDescriptionRtf", Order = 14)]
	public string xttLongDescriptionRtf { get; set; }

	[JsonProperty("xttLongDescriptionText", Order = 15)]
	public string xttLongDescriptionText { get; set; }

	[JsonProperty("xttMovementDate", Order = 16)]
	public DateTime? xttMovementDate { get; set; }

	[JsonProperty("xttMovementType", Order = 17)]
	[MaxLength(10)]
	public string xttMovementType { get; set; }

	[JsonProperty("xttPlannedReturnDate", Order = 18)]
	public DateTime? xttPlannedReturnDate { get; set; }

	[JsonProperty("xttRowVersion", Order = 19)]
	public byte[] xttRowVersion { get; set; }

	[JsonProperty("xttToolCategoryID", Order = 20)]
	[MaxLength(10)]
	public string xttToolCategoryID { get; set; }

	[JsonProperty("xttWorkCenterID", Order = 21)]
	[MaxLength(5)]
	public string xttWorkCenterID { get; set; }

	[JsonProperty("customFields", Order = 22)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
