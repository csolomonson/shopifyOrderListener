using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLFiscalYearBudgetAmountDto
{
	[JsonProperty("glbBudgetAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glbBudgetAmount { get; set; }

	[JsonProperty("glbBudgetHeaderID", Order = 2)]
	[Required(ErrorMessage = "glbBudgetHeaderID is required.")]
	public short glbBudgetHeaderID { get; set; }

	[JsonProperty("glbBudgetLineID", Order = 3)]
	[Required(ErrorMessage = "glbBudgetLineID is required.")]
	public short glbBudgetLineID { get; set; }

	[JsonProperty("glbCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string glbCreatedBy { get; set; }

	[JsonProperty("glbCreatedDate", Order = 5)]
	public DateTime? glbCreatedDate { get; set; }

	[JsonProperty("glbUniqueID", Order = 6)]
	public Guid glbUniqueID { get; set; }

	[JsonProperty("glbGlFiscalYearID", Order = 7)]
	[Required(ErrorMessage = "glbGlFiscalYearID is required.")]
	public short glbGlFiscalYearID { get; set; }

	[JsonProperty("glbGlFiscalYearPeriodID", Order = 8)]
	[Required(ErrorMessage = "glbGlFiscalYearPeriodID is required.")]
	public byte glbGlFiscalYearPeriodID { get; set; }

	[JsonProperty("glbRowVersion", Order = 9)]
	public byte[] glbRowVersion { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
