using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLFiscalYearPeriodDto
{
	[JsonProperty("glfCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string glfCreatedBy { get; set; }

	[JsonProperty("glfCreatedDate", Order = 2)]
	public DateTime? glfCreatedDate { get; set; }

	[JsonProperty("glfEndDate", Order = 3)]
	public DateTime? glfEndDate { get; set; }

	[JsonProperty("glfUniqueID", Order = 4)]
	public Guid glfUniqueID { get; set; }

	[JsonProperty("glfGlFiscalYearID", Order = 5)]
	[Required(ErrorMessage = "glfGlFiscalYearID is required.")]
	public short glfGlFiscalYearID { get; set; }

	[JsonProperty("glfApClosed", Order = 6)]
	public bool glfApClosed { get; set; }

	[JsonProperty("glfArClosed", Order = 7)]
	public bool glfArClosed { get; set; }

	[JsonProperty("glfGlClosed", Order = 8)]
	public bool glfGlClosed { get; set; }

	[JsonProperty("glfRowVersion", Order = 9)]
	public byte[] glfRowVersion { get; set; }

	[JsonProperty("glfGlFiscalYearPeriodID", Order = 10)]
	[Required(ErrorMessage = "glfGlFiscalYearPeriodID is required.")]
	public byte glfGlFiscalYearPeriodID { get; set; }

	[JsonProperty("glfStartDate", Order = 11)]
	public DateTime? glfStartDate { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
