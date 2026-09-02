using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLDivisionDto
{
	[JsonProperty("glvGlDivisionID", Order = 1)]
	[Required(ErrorMessage = "glvGlDivisionID is required.")]
	[MaxLength(3)]
	public string glvGlDivisionID { get; set; }

	[JsonProperty("glvCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string glvCreatedBy { get; set; }

	[JsonProperty("glvCreatedDate", Order = 3)]
	public DateTime? glvCreatedDate { get; set; }

	[JsonProperty("glvDescription", Order = 4)]
	[Required(ErrorMessage = "glvDescription is required.")]
	[MaxLength(30)]
	public string glvDescription { get; set; }

	[JsonProperty("glvUniqueID", Order = 5)]
	public Guid glvUniqueID { get; set; }

	[JsonProperty("glvRetainedEarningsAccountID", Order = 6)]
	[MaxLength(11)]
	public string glvRetainedEarningsAccountID { get; set; }

	[JsonProperty("glvRowVersion", Order = 7)]
	public byte[] glvRowVersion { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
