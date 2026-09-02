using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPScheduleTreeDto
{
	[JsonProperty("sxtCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string sxtCreatedBy { get; set; }

	[JsonProperty("sxtCreatedDate", Order = 2)]
	public DateTime? sxtCreatedDate { get; set; }

	[JsonProperty("sxtDescription", Order = 3)]
	[MaxLength(30)]
	public string sxtDescription { get; set; }

	[JsonProperty("sxtUniqueID", Order = 4)]
	public Guid sxtUniqueID { get; set; }

	[JsonProperty("sxtGroupUniqueID", Order = 5)]
	public Guid sxtGroupUniqueID { get; set; }

	[JsonProperty("sxtJobScenarioID", Order = 6)]
	[MaxLength(5)]
	public string sxtJobScenarioID { get; set; }

	[JsonProperty("sxtRowVersion", Order = 7)]
	public byte[] sxtRowVersion { get; set; }

	[JsonProperty("sxtScheduleTreeID", Order = 8)]
	[Required(ErrorMessage = "sxtScheduleTreeID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sxtScheduleTreeID { get; set; }

	[JsonProperty("sxtSourceTable", Order = 9)]
	[MaxLength(30)]
	public string sxtSourceTable { get; set; }

	[JsonProperty("sxtSourceUniqueID", Order = 10)]
	public Guid sxtSourceUniqueID { get; set; }

	[JsonProperty("sxtType", Order = 11)]
	public byte sxtType { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
