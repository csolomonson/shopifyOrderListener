using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProjectDto
{
	[JsonProperty("prpClosedDate", Order = 1)]
	public DateTime? prpClosedDate { get; set; }

	[JsonProperty("prpProjectID", Order = 2)]
	[Required(ErrorMessage = "prpProjectID is required.")]
	[MaxLength(10)]
	public string prpProjectID { get; set; }

	[JsonProperty("prpContactID", Order = 3)]
	[MaxLength(5)]
	public string prpContactID { get; set; }

	[JsonProperty("prpCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string prpCreatedBy { get; set; }

	[JsonProperty("prpCreatedDate", Order = 5)]
	public DateTime? prpCreatedDate { get; set; }

	[JsonProperty("prpDueDate", Order = 6)]
	public DateTime? prpDueDate { get; set; }

	[JsonProperty("prpUniqueID", Order = 7)]
	public Guid prpUniqueID { get; set; }

	[JsonProperty("prpClosed", Order = 8)]
	public bool prpClosed { get; set; }

	[JsonProperty("prpLocationID", Order = 9)]
	[MaxLength(5)]
	public string prpLocationID { get; set; }

	[JsonProperty("prpLongDescriptionRtf", Order = 10)]
	public string prpLongDescriptionRtf { get; set; }

	[JsonProperty("prpLongDescriptionText", Order = 11)]
	public string prpLongDescriptionText { get; set; }

	[JsonProperty("prpOrganizationID", Order = 12)]
	[MaxLength(10)]
	public string prpOrganizationID { get; set; }

	[JsonProperty("prpProjectDate", Order = 13)]
	[Required(ErrorMessage = "prpProjectDate is required.")]
	public DateTime? prpProjectDate { get; set; }

	[JsonProperty("prpProjectManagerEmployeeID", Order = 14)]
	[MaxLength(10)]
	public string prpProjectManagerEmployeeID { get; set; }

	[JsonProperty("prpProjectTypeID", Order = 15)]
	[MaxLength(5)]
	public string prpProjectTypeID { get; set; }

	[JsonProperty("prpRowVersion", Order = 16)]
	public byte[] prpRowVersion { get; set; }

	[JsonProperty("prpShortDescription", Order = 17)]
	[Required(ErrorMessage = "prpShortDescription is required.")]
	[MaxLength(50)]
	public string prpShortDescription { get; set; }

	[JsonProperty("prpStatus", Order = 18)]
	[MaxLength(1)]
	public string prpStatus { get; set; }

	[JsonProperty("customFields", Order = 19)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
