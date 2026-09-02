using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPNextIDDto
{
	[JsonProperty("xanAutoIncrement", Order = 1)]
	public byte xanAutoIncrement { get; set; }

	[JsonProperty("xanCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string xanCreatedBy { get; set; }

	[JsonProperty("xanCreatedDate", Order = 3)]
	public DateTime? xanCreatedDate { get; set; }

	[JsonProperty("xanDatasets", Order = 4)]
	[MaxLength(50)]
	public string xanDatasets { get; set; }

	[JsonProperty("xanUniqueID", Order = 5)]
	public Guid xanUniqueID { get; set; }

	[JsonProperty("xanIncrementAmount", Order = 6)]
	public short xanIncrementAmount { get; set; }

	[JsonProperty("xanLogChanges", Order = 7)]
	public byte xanLogChanges { get; set; }

	[JsonProperty("xanNextID", Order = 8)]
	[MaxLength(30)]
	public string xanNextID { get; set; }

	[JsonProperty("xanNumericOnly", Order = 9)]
	public byte xanNumericOnly { get; set; }

	[JsonProperty("xanRowVersion", Order = 10)]
	public byte[] xanRowVersion { get; set; }

	[JsonProperty("xanTable", Order = 11)]
	[MaxLength(30)]
	public string xanTable { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
