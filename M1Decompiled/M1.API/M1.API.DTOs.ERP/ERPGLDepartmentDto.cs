using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLDepartmentDto
{
	[JsonProperty("gldGlDepartmentID", Order = 1)]
	[Required(ErrorMessage = "gldGlDepartmentID is required.")]
	[MaxLength(3)]
	public string gldGlDepartmentID { get; set; }

	[JsonProperty("gldCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string gldCreatedBy { get; set; }

	[JsonProperty("gldCreatedDate", Order = 3)]
	public DateTime? gldCreatedDate { get; set; }

	[JsonProperty("gldDescription", Order = 4)]
	[Required(ErrorMessage = "gldDescription is required.")]
	[MaxLength(30)]
	public string gldDescription { get; set; }

	[JsonProperty("gldUniqueID", Order = 5)]
	public Guid gldUniqueID { get; set; }

	[JsonProperty("gldRowVersion", Order = 6)]
	public byte[] gldRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
