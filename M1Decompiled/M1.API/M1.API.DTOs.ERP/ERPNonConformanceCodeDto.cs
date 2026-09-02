using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPNonConformanceCodeDto
{
	[JsonProperty("qacNonConformanceCodeID", Order = 1)]
	[Required(ErrorMessage = "qacNonConformanceCodeID is required.")]
	[MaxLength(5)]
	public string qacNonConformanceCodeID { get; set; }

	[JsonProperty("qacCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string qacCreatedBy { get; set; }

	[JsonProperty("qacCreatedDate", Order = 3)]
	public DateTime? qacCreatedDate { get; set; }

	[JsonProperty("qacDescription", Order = 4)]
	[Required(ErrorMessage = "qacDescription is required.")]
	[MaxLength(50)]
	public string qacDescription { get; set; }

	[JsonProperty("qacUniqueID", Order = 5)]
	public Guid qacUniqueID { get; set; }

	[JsonProperty("qacNonConformanceCategoryID", Order = 6)]
	[MaxLength(5)]
	public string qacNonConformanceCategoryID { get; set; }

	[JsonProperty("qacRowVersion", Order = 7)]
	public byte[] qacRowVersion { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
