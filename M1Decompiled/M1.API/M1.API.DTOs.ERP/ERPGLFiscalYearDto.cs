using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLFiscalYearDto
{
	[JsonProperty("glzCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string glzCreatedBy { get; set; }

	[JsonProperty("glzCreatedDate", Order = 2)]
	public DateTime? glzCreatedDate { get; set; }

	[JsonProperty("glzEndDate", Order = 3)]
	[Required(ErrorMessage = "glzEndDate is required.")]
	public DateTime? glzEndDate { get; set; }

	[JsonProperty("glzUniqueID", Order = 4)]
	public Guid glzUniqueID { get; set; }

	[JsonProperty("glzRowVersion", Order = 5)]
	public byte[] glzRowVersion { get; set; }

	[JsonProperty("glzGlFiscalYearID", Order = 6)]
	[Required(ErrorMessage = "glzGlFiscalYearID is required.")]
	public short glzGlFiscalYearID { get; set; }

	[JsonProperty("glzStartDate", Order = 7)]
	[Required(ErrorMessage = "glzStartDate is required.")]
	public DateTime? glzStartDate { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
