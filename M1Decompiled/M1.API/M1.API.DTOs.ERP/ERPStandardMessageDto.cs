using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPStandardMessageDto
{
	[JsonProperty("xamStandardMessageID", Order = 1)]
	[Required(ErrorMessage = "xamStandardMessageID is required.")]
	[MaxLength(10)]
	public string xamStandardMessageID { get; set; }

	[JsonProperty("xamCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string xamCreatedBy { get; set; }

	[JsonProperty("xamCreatedDate", Order = 3)]
	public DateTime? xamCreatedDate { get; set; }

	[JsonProperty("xamUniqueID", Order = 4)]
	public Guid xamUniqueID { get; set; }

	[JsonProperty("xamInactiveDate", Order = 5)]
	public DateTime? xamInactiveDate { get; set; }

	[JsonProperty("xamInactive", Order = 6)]
	public bool xamInactive { get; set; }

	[JsonProperty("xamLongDescriptionRtf", Order = 7)]
	public string xamLongDescriptionRtf { get; set; }

	[JsonProperty("xamLongDescriptionText", Order = 8)]
	[Required(ErrorMessage = "xamLongDescriptionText is required.")]
	public string xamLongDescriptionText { get; set; }

	[JsonProperty("xamMessageType", Order = 9)]
	[Required(ErrorMessage = "xamMessageType is required.")]
	public byte xamMessageType { get; set; }

	[JsonProperty("xamRowVersion", Order = 10)]
	public byte[] xamRowVersion { get; set; }

	[JsonProperty("xamShortDescription", Order = 11)]
	[Required(ErrorMessage = "xamShortDescription is required.")]
	[MaxLength(50)]
	public string xamShortDescription { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
