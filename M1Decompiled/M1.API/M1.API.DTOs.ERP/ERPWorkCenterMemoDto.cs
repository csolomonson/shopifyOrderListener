using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWorkCenterMemoDto
{
	[JsonProperty("xakCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string xakCreatedBy { get; set; }

	[JsonProperty("xakCreatedDate", Order = 2)]
	public DateTime? xakCreatedDate { get; set; }

	[JsonProperty("xakUniqueID", Order = 3)]
	public Guid xakUniqueID { get; set; }

	[JsonProperty("xakLongDescriptionRtf", Order = 4)]
	public string xakLongDescriptionRtf { get; set; }

	[JsonProperty("xakLongDescriptionText", Order = 5)]
	public string xakLongDescriptionText { get; set; }

	[JsonProperty("xakMemoDate", Order = 6)]
	[Required(ErrorMessage = "xakMemoDate is required.")]
	public DateTime? xakMemoDate { get; set; }

	[JsonProperty("xakRowVersion", Order = 7)]
	public byte[] xakRowVersion { get; set; }

	[JsonProperty("xakWorkCenterMemoID", Order = 8)]
	[Required(ErrorMessage = "xakWorkCenterMemoID is required.")]
	public short xakWorkCenterMemoID { get; set; }

	[JsonProperty("xakShortDescription", Order = 9)]
	[Required(ErrorMessage = "xakShortDescription is required.")]
	[MaxLength(50)]
	public string xakShortDescription { get; set; }

	[JsonProperty("xakShowInJobs", Order = 10)]
	public bool xakShowInJobs { get; set; }

	[JsonProperty("xakShowInParts", Order = 11)]
	public bool xakShowInParts { get; set; }

	[JsonProperty("xakShowInQuotes", Order = 12)]
	public bool xakShowInQuotes { get; set; }

	[JsonProperty("xakShowInWorkCenters", Order = 13)]
	public bool xakShowInWorkCenters { get; set; }

	[JsonProperty("xakWorkCenterID", Order = 14)]
	[Required(ErrorMessage = "xakWorkCenterID is required.")]
	[MaxLength(5)]
	public string xakWorkCenterID { get; set; }

	[JsonProperty("customFields", Order = 15)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
