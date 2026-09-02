using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPChangeLogDto
{
	[JsonProperty("xagChangeDate", Order = 1)]
	[Required(ErrorMessage = "xagChangeDate is required.")]
	public DateTime? xagChangeDate { get; set; }

	[JsonProperty("xagChangeType", Order = 2)]
	[Required(ErrorMessage = "xagChangeType is required.")]
	[MaxLength(1)]
	public string xagChangeType { get; set; }

	[JsonProperty("xagChangeUserID", Order = 3)]
	[Required(ErrorMessage = "xagChangeUserID is required.")]
	[MaxLength(20)]
	public string xagChangeUserID { get; set; }

	[JsonProperty("xagRowVersion", Order = 4)]
	public byte[] xagRowVersion { get; set; }

	[JsonProperty("xagChangeLogID", Order = 5)]
	[Required(ErrorMessage = "xagChangeLogID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xagChangeLogID { get; set; }

	[JsonProperty("xagTableKeyValues", Order = 6)]
	[MaxLength(50)]
	public string xagTableKeyValues { get; set; }

	[JsonProperty("xagTableName", Order = 7)]
	[Required(ErrorMessage = "xagTableName is required.")]
	[MaxLength(30)]
	public string xagTableName { get; set; }

	[JsonProperty("xagTableNewValues", Order = 8)]
	[MaxLength(50)]
	public string xagTableNewValues { get; set; }

	[JsonProperty("xagTableOldValues", Order = 9)]
	[MaxLength(50)]
	public string xagTableOldValues { get; set; }

	[JsonProperty("xagTableUniqueID", Order = 10)]
	public Guid xagTableUniqueID { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
