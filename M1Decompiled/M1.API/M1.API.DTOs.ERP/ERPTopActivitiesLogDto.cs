using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPTopActivitiesLogDto
{
	[JsonProperty("rxlCount", Order = 1)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int rxlCount { get; set; }

	[JsonProperty("rxlExplorerType", Order = 2)]
	[MaxLength(30)]
	public string rxlExplorerType { get; set; }

	[JsonProperty("rxlGridID", Order = 3)]
	[MaxLength(100)]
	public string rxlGridID { get; set; }

	[JsonProperty("rxlObjectDataRun", Order = 4)]
	[MaxLength(50)]
	public string rxlObjectDataRun { get; set; }

	[JsonProperty("rxlObjectName", Order = 5)]
	[MaxLength(100)]
	public string rxlObjectName { get; set; }

	[JsonProperty("rxlProcessedDateTime", Order = 6)]
	public DateTime rxlProcessedDateTime { get; set; }

	[JsonProperty("rxlRowVersion", Order = 7)]
	public byte[] rxlRowVersion { get; set; }

	[JsonProperty("rxlTopActivityID", Order = 8)]
	public int rxlTopActivityID { get; set; }

	[JsonProperty("rxlUserID", Order = 9)]
	[MaxLength(50)]
	public string rxlUserID { get; set; }

	[JsonProperty("rxlVisualizerID", Order = 10)]
	[MaxLength(100)]
	public string rxlVisualizerID { get; set; }

	[JsonProperty("rxlVisualizerType", Order = 11)]
	[MaxLength(30)]
	public string rxlVisualizerType { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
