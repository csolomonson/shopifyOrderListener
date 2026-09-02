using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartAlternateDto
{
	[JsonProperty("imeAlternatePartID", Order = 1)]
	[Required(ErrorMessage = "imeAlternatePartID is required.")]
	[MaxLength(30)]
	public string imeAlternatePartID { get; set; }

	[JsonProperty("imeAlternatePartRevisionID", Order = 2)]
	[MaxLength(15)]
	public string imeAlternatePartRevisionID { get; set; }

	[JsonProperty("imeComment", Order = 3)]
	[MaxLength(70)]
	public string imeComment { get; set; }

	[JsonProperty("imeCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string imeCreatedBy { get; set; }

	[JsonProperty("imeCreatedDate", Order = 5)]
	public DateTime? imeCreatedDate { get; set; }

	[JsonProperty("imeUniqueID", Order = 6)]
	public Guid imeUniqueID { get; set; }

	[JsonProperty("imePartID", Order = 7)]
	[Required(ErrorMessage = "imePartID is required.")]
	[MaxLength(30)]
	public string imePartID { get; set; }

	[JsonProperty("imePartRevisionID", Order = 8)]
	[MaxLength(15)]
	public string imePartRevisionID { get; set; }

	[JsonProperty("imeRowVersion", Order = 9)]
	public byte[] imeRowVersion { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
