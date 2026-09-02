using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCustomTableDto
{
	[JsonProperty("customFields", Order = 1)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
