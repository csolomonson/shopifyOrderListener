using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPContactMethodDto
{
	[JsonProperty("kbcContactMethodID", Order = 1)]
	[Required(ErrorMessage = "kbcContactMethodID is required.")]
	[MaxLength(5)]
	public string kbcContactMethodID { get; set; }

	[JsonProperty("kbcCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string kbcCreatedBy { get; set; }

	[JsonProperty("kbcCreatedDate", Order = 3)]
	public DateTime? kbcCreatedDate { get; set; }

	[JsonProperty("kbcDescription", Order = 4)]
	[Required(ErrorMessage = "kbcDescription is required.")]
	[MaxLength(50)]
	public string kbcDescription { get; set; }

	[JsonProperty("kbcUniqueID", Order = 5)]
	public Guid kbcUniqueID { get; set; }

	[JsonProperty("kbcRowVersion", Order = 6)]
	public byte[] kbcRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
