using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPNonConformanceCauseDto
{
	[JsonProperty("qauNonConformanceCauseID", Order = 1)]
	[Required(ErrorMessage = "qauNonConformanceCauseID is required.")]
	[MaxLength(5)]
	public string qauNonConformanceCauseID { get; set; }

	[JsonProperty("qauCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string qauCreatedBy { get; set; }

	[JsonProperty("qauCreatedDate", Order = 3)]
	public DateTime? qauCreatedDate { get; set; }

	[JsonProperty("qauDescription", Order = 4)]
	[Required(ErrorMessage = "qauDescription is required.")]
	[MaxLength(50)]
	public string qauDescription { get; set; }

	[JsonProperty("qauUniqueID", Order = 5)]
	public Guid qauUniqueID { get; set; }

	[JsonProperty("qauRowVersion", Order = 6)]
	public byte[] qauRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
