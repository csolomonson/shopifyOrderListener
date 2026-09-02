using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSkillCompetencyDto
{
	[JsonProperty("lecColor", Order = 1)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int lecColor { get; set; }

	[JsonProperty("lecCompetencyID", Order = 2)]
	[Required(ErrorMessage = "lecCompetencyID is required.")]
	[MaxLength(10)]
	public string lecCompetencyID { get; set; }

	[JsonProperty("lecCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string lecCreatedBy { get; set; }

	[JsonProperty("lecCreatedDate", Order = 4)]
	public DateTime? lecCreatedDate { get; set; }

	[JsonProperty("lecDescription", Order = 5)]
	[Required(ErrorMessage = "lecDescription is required.")]
	[MaxLength(50)]
	public string lecDescription { get; set; }

	[JsonProperty("lecUniqueID", Order = 6)]
	public Guid lecUniqueID { get; set; }

	[JsonProperty("lecInactiveDate", Order = 7)]
	public DateTime? lecInactiveDate { get; set; }

	[JsonProperty("lecInactive", Order = 8)]
	public bool lecInactive { get; set; }

	[JsonProperty("lecLevel", Order = 9)]
	public byte lecLevel { get; set; }

	[JsonProperty("lecLongDescriptionRtf", Order = 10)]
	public string lecLongDescriptionRtf { get; set; }

	[JsonProperty("lecLongDescriptionText", Order = 11)]
	public string lecLongDescriptionText { get; set; }

	[JsonProperty("lecRowVersion", Order = 12)]
	public byte[] lecRowVersion { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
