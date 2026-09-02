using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPExpenseDto
{
	[JsonProperty("lmxExpenseID", Order = 1)]
	[Required(ErrorMessage = "lmxExpenseID is required.")]
	[MaxLength(5)]
	public string lmxExpenseID { get; set; }

	[JsonProperty("lmxCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string lmxCreatedBy { get; set; }

	[JsonProperty("lmxCreatedDate", Order = 3)]
	public DateTime? lmxCreatedDate { get; set; }

	[JsonProperty("lmxDescription", Order = 4)]
	[Required(ErrorMessage = "lmxDescription is required.")]
	[MaxLength(50)]
	public string lmxDescription { get; set; }

	[JsonProperty("lmxUniqueID", Order = 5)]
	public Guid lmxUniqueID { get; set; }

	[JsonProperty("lmxRowVersion", Order = 6)]
	public byte[] lmxRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
