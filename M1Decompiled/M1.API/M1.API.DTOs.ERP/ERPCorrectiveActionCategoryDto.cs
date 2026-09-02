using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCorrectiveActionCategoryDto
{
	[JsonProperty("qatCorrectiveActionCategoryID", Order = 1)]
	[Required(ErrorMessage = "qatCorrectiveActionCategoryID is required.")]
	[MaxLength(5)]
	public string qatCorrectiveActionCategoryID { get; set; }

	[JsonProperty("qatCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string qatCreatedBy { get; set; }

	[JsonProperty("qatCreatedDate", Order = 3)]
	public DateTime? qatCreatedDate { get; set; }

	[JsonProperty("qatDescription", Order = 4)]
	[Required(ErrorMessage = "qatDescription is required.")]
	[MaxLength(50)]
	public string qatDescription { get; set; }

	[JsonProperty("qatUniqueID", Order = 5)]
	public Guid qatUniqueID { get; set; }

	[JsonProperty("qatRowVersion", Order = 6)]
	public byte[] qatRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
