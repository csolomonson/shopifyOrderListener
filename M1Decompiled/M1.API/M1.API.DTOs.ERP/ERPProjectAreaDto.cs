using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProjectAreaDto
{
	[JsonProperty("praProjectAreaID", Order = 1)]
	[Required(ErrorMessage = "praProjectAreaID is required.")]
	[MaxLength(15)]
	public string praProjectAreaID { get; set; }

	[JsonProperty("praCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string praCreatedBy { get; set; }

	[JsonProperty("praCreatedDate", Order = 3)]
	public DateTime? praCreatedDate { get; set; }

	[JsonProperty("praDescription", Order = 4)]
	[Required(ErrorMessage = "praDescription is required.")]
	[MaxLength(50)]
	public string praDescription { get; set; }

	[JsonProperty("praUniqueID", Order = 5)]
	public Guid praUniqueID { get; set; }

	[JsonProperty("praProjectID", Order = 6)]
	[Required(ErrorMessage = "praProjectID is required.")]
	[MaxLength(10)]
	public string praProjectID { get; set; }

	[JsonProperty("praRowVersion", Order = 7)]
	public byte[] praRowVersion { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
