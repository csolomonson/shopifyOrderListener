using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPContactTitleDto
{
	[JsonProperty("cmeContactTitleID", Order = 1)]
	[Required(ErrorMessage = "cmeContactTitleID is required.")]
	[MaxLength(5)]
	public string cmeContactTitleID { get; set; }

	[JsonProperty("cmeCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string cmeCreatedBy { get; set; }

	[JsonProperty("cmeCreatedDate", Order = 3)]
	public DateTime? cmeCreatedDate { get; set; }

	[JsonProperty("cmeDescription", Order = 4)]
	[Required(ErrorMessage = "cmeDescription is required.")]
	[MaxLength(50)]
	public string cmeDescription { get; set; }

	[JsonProperty("cmeUniqueID", Order = 5)]
	public Guid cmeUniqueID { get; set; }

	[JsonProperty("cmeRowVersion", Order = 6)]
	public byte[] cmeRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
