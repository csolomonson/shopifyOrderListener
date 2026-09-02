using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCorrectiveActionCodeDto
{
	[JsonProperty("qaoCorrectiveActionCodeID", Order = 1)]
	[Required(ErrorMessage = "qaoCorrectiveActionCodeID is required.")]
	[MaxLength(5)]
	public string qaoCorrectiveActionCodeID { get; set; }

	[JsonProperty("qaoCorrectiveActionCategoryID", Order = 2)]
	[MaxLength(5)]
	public string qaoCorrectiveActionCategoryID { get; set; }

	[JsonProperty("qaoCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string qaoCreatedBy { get; set; }

	[JsonProperty("qaoCreatedDate", Order = 4)]
	public DateTime? qaoCreatedDate { get; set; }

	[JsonProperty("qaoDescription", Order = 5)]
	[Required(ErrorMessage = "qaoDescription is required.")]
	[MaxLength(50)]
	public string qaoDescription { get; set; }

	[JsonProperty("qaoUniqueID", Order = 6)]
	public Guid qaoUniqueID { get; set; }

	[JsonProperty("qaoHoursAllowed", Order = 7)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qaoHoursAllowed { get; set; }

	[JsonProperty("qaoRowVersion", Order = 8)]
	public byte[] qaoRowVersion { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
