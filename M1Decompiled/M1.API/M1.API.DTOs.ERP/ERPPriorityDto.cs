using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPriorityDto
{
	[JsonProperty("kbrCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string kbrCreatedBy { get; set; }

	[JsonProperty("kbrCreatedDate", Order = 2)]
	public DateTime? kbrCreatedDate { get; set; }

	[JsonProperty("kbrDescription", Order = 3)]
	[Required(ErrorMessage = "kbrDescription is required.")]
	[MaxLength(50)]
	public string kbrDescription { get; set; }

	[JsonProperty("kbrUniqueID", Order = 4)]
	public Guid kbrUniqueID { get; set; }

	[JsonProperty("kbrRowVersion", Order = 5)]
	public byte[] kbrRowVersion { get; set; }

	[JsonProperty("kbrPriorityID", Order = 6)]
	[Required(ErrorMessage = "kbrPriorityID is required.")]
	public byte kbrPriorityID { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
