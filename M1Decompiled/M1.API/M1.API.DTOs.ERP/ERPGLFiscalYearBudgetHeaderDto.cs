using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLFiscalYearBudgetHeaderDto
{
	[JsonProperty("glkAnnualAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glkAnnualAmount { get; set; }

	[JsonProperty("glkBudgetHeaderID", Order = 2)]
	[Required(ErrorMessage = "glkBudgetHeaderID is required.")]
	public short glkBudgetHeaderID { get; set; }

	[JsonProperty("glkCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string glkCreatedBy { get; set; }

	[JsonProperty("glkCreatedDate", Order = 4)]
	public DateTime? glkCreatedDate { get; set; }

	[JsonProperty("glkUniqueID", Order = 5)]
	public Guid glkUniqueID { get; set; }

	[JsonProperty("glkGlAccountID", Order = 6)]
	[Required(ErrorMessage = "glkGlAccountID is required.")]
	[MaxLength(11)]
	public string glkGlAccountID { get; set; }

	[JsonProperty("glkGlFiscalYearID", Order = 7)]
	[Required(ErrorMessage = "glkGlFiscalYearID is required.")]
	public short glkGlFiscalYearID { get; set; }

	[JsonProperty("glkRowVersion", Order = 8)]
	public byte[] glkRowVersion { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
