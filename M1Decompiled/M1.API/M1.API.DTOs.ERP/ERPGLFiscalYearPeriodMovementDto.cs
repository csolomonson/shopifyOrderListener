using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLFiscalYearPeriodMovementDto
{
	[JsonProperty("gliCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string gliCreatedBy { get; set; }

	[JsonProperty("gliCreatedDate", Order = 2)]
	public DateTime? gliCreatedDate { get; set; }

	[JsonProperty("gliUniqueID", Order = 3)]
	public Guid gliUniqueID { get; set; }

	[JsonProperty("gliGlAccountID", Order = 4)]
	[MaxLength(11)]
	public string gliGlAccountID { get; set; }

	[JsonProperty("gliGlFiscalYearID", Order = 5)]
	public short gliGlFiscalYearID { get; set; }

	[JsonProperty("gliGlFiscalYearPeriodID", Order = 6)]
	public byte gliGlFiscalYearPeriodID { get; set; }

	[JsonProperty("gliRowVersion", Order = 7)]
	public byte[] gliRowVersion { get; set; }

	[JsonProperty("gliTotalCredits", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gliTotalCredits { get; set; }

	[JsonProperty("gliTotalDebits", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gliTotalDebits { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
