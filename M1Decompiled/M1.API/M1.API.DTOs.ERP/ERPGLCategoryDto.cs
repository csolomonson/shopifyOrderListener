using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLCategoryDto
{
	[JsonProperty("gltCategoryType", Order = 1)]
	[Required(ErrorMessage = "gltCategoryType is required.")]
	public byte gltCategoryType { get; set; }

	[JsonProperty("gltGlCategoryID", Order = 2)]
	[Required(ErrorMessage = "gltGlCategoryID is required.")]
	[MaxLength(5)]
	public string gltGlCategoryID { get; set; }

	[JsonProperty("gltCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string gltCreatedBy { get; set; }

	[JsonProperty("gltCreatedDate", Order = 4)]
	public DateTime? gltCreatedDate { get; set; }

	[JsonProperty("gltDescription", Order = 5)]
	[Required(ErrorMessage = "gltDescription is required.")]
	[MaxLength(50)]
	public string gltDescription { get; set; }

	[JsonProperty("gltUniqueID", Order = 6)]
	public Guid gltUniqueID { get; set; }

	[JsonProperty("gltReportSequence", Order = 7)]
	public byte gltReportSequence { get; set; }

	[JsonProperty("gltRowVersion", Order = 8)]
	public byte[] gltRowVersion { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
