using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLFiscalYearBudgetLineDto
{
	[JsonProperty("glgAnnualAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glgAnnualAmount { get; set; }

	[JsonProperty("glgBudgetHeaderID", Order = 2)]
	[Required(ErrorMessage = "glgBudgetHeaderID is required.")]
	public short glgBudgetHeaderID { get; set; }

	[JsonProperty("glgBudgetLineID", Order = 3)]
	[Required(ErrorMessage = "glgBudgetLineID is required.")]
	public short glgBudgetLineID { get; set; }

	[JsonProperty("glgCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string glgCreatedBy { get; set; }

	[JsonProperty("glgCreatedDate", Order = 5)]
	public DateTime? glgCreatedDate { get; set; }

	[JsonProperty("glgUniqueID", Order = 6)]
	public Guid glgUniqueID { get; set; }

	[JsonProperty("glgGlFiscalYearID", Order = 7)]
	[Required(ErrorMessage = "glgGlFiscalYearID is required.")]
	public short glgGlFiscalYearID { get; set; }

	[JsonProperty("glgRowVersion", Order = 8)]
	public byte[] glgRowVersion { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
