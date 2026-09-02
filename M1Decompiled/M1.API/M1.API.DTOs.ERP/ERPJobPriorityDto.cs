using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPJobPriorityDto
{
	[JsonProperty("jmjDescription", Order = 1)]
	[Required(ErrorMessage = "jmjDescription is required.")]
	[MaxLength(50)]
	public string jmjDescription { get; set; }

	[JsonProperty("jmjUniqueID", Order = 2)]
	public Guid jmjUniqueID { get; set; }

	[JsonProperty("jmjRowVersion", Order = 3)]
	public byte[] jmjRowVersion { get; set; }

	[JsonProperty("jmjJobPriorityID", Order = 4)]
	[Required(ErrorMessage = "jmjJobPriorityID is required.")]
	public short jmjJobPriorityID { get; set; }

	[JsonProperty("customFields", Order = 5)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
