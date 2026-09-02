using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSkillDto
{
	[JsonProperty("lesSkillID", Order = 1)]
	[Required(ErrorMessage = "lesSkillID is required.")]
	[MaxLength(10)]
	public string lesSkillID { get; set; }

	[JsonProperty("lesCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string lesCreatedBy { get; set; }

	[JsonProperty("lesCreatedDate", Order = 3)]
	public DateTime? lesCreatedDate { get; set; }

	[JsonProperty("lesDescription", Order = 4)]
	[Required(ErrorMessage = "lesDescription is required.")]
	[MaxLength(50)]
	public string lesDescription { get; set; }

	[JsonProperty("lesUniqueID", Order = 5)]
	public Guid lesUniqueID { get; set; }

	[JsonProperty("lesInactiveDate", Order = 6)]
	public DateTime? lesInactiveDate { get; set; }

	[JsonProperty("lesInactive", Order = 7)]
	public bool lesInactive { get; set; }

	[JsonProperty("lesLongDescriptionRtf", Order = 8)]
	public string lesLongDescriptionRtf { get; set; }

	[JsonProperty("lesLongDescriptionText", Order = 9)]
	public string lesLongDescriptionText { get; set; }

	[JsonProperty("lesRowVersion", Order = 10)]
	public byte[] lesRowVersion { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
