using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLFiscalYearOpeningBalanceDto
{
	[JsonProperty("glyCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string glyCreatedBy { get; set; }

	[JsonProperty("glyCreatedDate", Order = 2)]
	public DateTime? glyCreatedDate { get; set; }

	[JsonProperty("glyUniqueID", Order = 3)]
	public Guid glyUniqueID { get; set; }

	[JsonProperty("glyGlAccountID", Order = 4)]
	[MaxLength(11)]
	public string glyGlAccountID { get; set; }

	[JsonProperty("glyGlFiscalYearID", Order = 5)]
	public short glyGlFiscalYearID { get; set; }

	[JsonProperty("glyRowVersion", Order = 6)]
	public byte[] glyRowVersion { get; set; }

	[JsonProperty("glyYearOpeningBalance", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glyYearOpeningBalance { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
