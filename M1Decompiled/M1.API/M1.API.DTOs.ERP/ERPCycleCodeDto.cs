using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCycleCodeDto
{
	[JsonProperty("imdCycleCodeID", Order = 1)]
	[Required(ErrorMessage = "imdCycleCodeID is required.")]
	[MaxLength(5)]
	public string imdCycleCodeID { get; set; }

	[JsonProperty("imdCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string imdCreatedBy { get; set; }

	[JsonProperty("imdCreatedDate", Order = 3)]
	public DateTime? imdCreatedDate { get; set; }

	[JsonProperty("imdDescription", Order = 4)]
	[Required(ErrorMessage = "imdDescription is required.")]
	[MaxLength(50)]
	public string imdDescription { get; set; }

	[JsonProperty("imdUniqueID", Order = 5)]
	public Guid imdUniqueID { get; set; }

	[JsonProperty("imdRowVersion", Order = 6)]
	public byte[] imdRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
