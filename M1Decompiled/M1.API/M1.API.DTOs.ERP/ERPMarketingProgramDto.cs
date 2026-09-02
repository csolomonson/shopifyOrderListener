using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMarketingProgramDto
{
	[JsonProperty("looActivityType", Order = 1)]
	[MaxLength(5)]
	public string looActivityType { get; set; }

	[JsonProperty("looMarketingProgramID", Order = 2)]
	[Required(ErrorMessage = "looMarketingProgramID is required.")]
	[MaxLength(5)]
	public string looMarketingProgramID { get; set; }

	[JsonProperty("looCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string looCreatedBy { get; set; }

	[JsonProperty("looCreatedDate", Order = 4)]
	public DateTime? looCreatedDate { get; set; }

	[JsonProperty("looEndDate", Order = 5)]
	public DateTime? looEndDate { get; set; }

	[JsonProperty("looUniqueID", Order = 6)]
	public Guid looUniqueID { get; set; }

	[JsonProperty("looExpectedRevenue", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal looExpectedRevenue { get; set; }

	[JsonProperty("looInactiveDate", Order = 8)]
	public DateTime? looInactiveDate { get; set; }

	[JsonProperty("looInactive", Order = 9)]
	public bool looInactive { get; set; }

	[JsonProperty("looLongDescriptionRtf", Order = 10)]
	public string looLongDescriptionRtf { get; set; }

	[JsonProperty("looLongDescriptionText", Order = 11)]
	public string looLongDescriptionText { get; set; }

	[JsonProperty("looMarketingCost", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal looMarketingCost { get; set; }

	[JsonProperty("looRowVersion", Order = 13)]
	public byte[] looRowVersion { get; set; }

	[JsonProperty("looShortDescription", Order = 14)]
	[Required(ErrorMessage = "looShortDescription is required.")]
	[MaxLength(50)]
	public string looShortDescription { get; set; }

	[JsonProperty("looStartDate", Order = 15)]
	public DateTime? looStartDate { get; set; }

	[JsonProperty("customFields", Order = 16)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
