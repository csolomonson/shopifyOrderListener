using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMilestoneDto
{
	[JsonProperty("losMilestoneID", Order = 1)]
	[Required(ErrorMessage = "losMilestoneID is required.")]
	[MaxLength(5)]
	public string losMilestoneID { get; set; }

	[JsonProperty("losConfidenceFactor", Order = 2)]
	[Range(0.0, 99.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal losConfidenceFactor { get; set; }

	[JsonProperty("losCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string losCreatedBy { get; set; }

	[JsonProperty("losCreatedDate", Order = 4)]
	public DateTime? losCreatedDate { get; set; }

	[JsonProperty("losUniqueID", Order = 5)]
	public Guid losUniqueID { get; set; }

	[JsonProperty("losLongDescriptionRtf", Order = 6)]
	public string losLongDescriptionRtf { get; set; }

	[JsonProperty("losLongDescriptionText", Order = 7)]
	public string losLongDescriptionText { get; set; }

	[JsonProperty("losRowVersion", Order = 8)]
	public byte[] losRowVersion { get; set; }

	[JsonProperty("losShortDescription", Order = 9)]
	[Required(ErrorMessage = "losShortDescription is required.")]
	[MaxLength(50)]
	public string losShortDescription { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
