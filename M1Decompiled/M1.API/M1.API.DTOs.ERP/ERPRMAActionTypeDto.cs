using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRMAActionTypeDto
{
	[JsonProperty("ratRmaActionTypeID", Order = 1)]
	[Required(ErrorMessage = "ratRmaActionTypeID is required.")]
	[MaxLength(5)]
	public string ratRmaActionTypeID { get; set; }

	[JsonProperty("ratDescription", Order = 2)]
	[Required(ErrorMessage = "ratDescription is required.")]
	[MaxLength(30)]
	public string ratDescription { get; set; }

	[JsonProperty("ratUniqueID", Order = 3)]
	public Guid ratUniqueID { get; set; }

	[JsonProperty("ratRowVersion", Order = 4)]
	public byte[] ratRowVersion { get; set; }

	[JsonProperty("customFields", Order = 5)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
