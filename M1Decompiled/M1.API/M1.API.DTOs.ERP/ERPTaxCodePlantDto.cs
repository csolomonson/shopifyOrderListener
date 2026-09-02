using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPTaxCodePlantDto
{
	[JsonProperty("xtpAccrualGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string xtpAccrualGlAccountID { get; set; }

	[JsonProperty("xtpCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string xtpCreatedBy { get; set; }

	[JsonProperty("xtpCreatedDate", Order = 3)]
	public DateTime? xtpCreatedDate { get; set; }

	[JsonProperty("xtpUniqueID", Order = 4)]
	public Guid xtpUniqueID { get; set; }

	[JsonProperty("xtpPlantID", Order = 5)]
	[MaxLength(5)]
	public string xtpPlantID { get; set; }

	[JsonProperty("xtpRowVersion", Order = 6)]
	public byte[] xtpRowVersion { get; set; }

	[JsonProperty("xtpTaxCodeID", Order = 7)]
	[MaxLength(5)]
	public string xtpTaxCodeID { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
