using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPJobScenarioDto
{
	[JsonProperty("jmnJobScenarioID", Order = 1)]
	[Required(ErrorMessage = "jmnJobScenarioID is required.")]
	[MaxLength(5)]
	public string jmnJobScenarioID { get; set; }

	[JsonProperty("jmnCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string jmnCreatedBy { get; set; }

	[JsonProperty("jmnCreatedDate", Order = 3)]
	public DateTime? jmnCreatedDate { get; set; }

	[JsonProperty("jmnDescription", Order = 4)]
	[Required(ErrorMessage = "jmnDescription is required.")]
	[MaxLength(50)]
	public string jmnDescription { get; set; }

	[JsonProperty("jmnUniqueID", Order = 5)]
	public Guid jmnUniqueID { get; set; }

	[JsonProperty("jmnRowVersion", Order = 6)]
	public byte[] jmnRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
