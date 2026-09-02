using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPBankStatementDto
{
	[JsonProperty("glsBankAccountID", Order = 1)]
	[MaxLength(5)]
	public string glsBankAccountID { get; set; }

	[JsonProperty("glsBankStatementReference", Order = 2)]
	[Required(ErrorMessage = "glsBankStatementReference is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int glsBankStatementReference { get; set; }

	[JsonProperty("glsCashGlAccountID", Order = 3)]
	[Required(ErrorMessage = "glsCashGlAccountID is required.")]
	[MaxLength(11)]
	public string glsCashGlAccountID { get; set; }

	[JsonProperty("glsCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string glsCreatedBy { get; set; }

	[JsonProperty("glsCreatedDate", Order = 5)]
	public DateTime? glsCreatedDate { get; set; }

	[JsonProperty("glsCurrencyRateID", Order = 6)]
	[MaxLength(5)]
	public string glsCurrencyRateID { get; set; }

	[JsonProperty("glsEndingBalance", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glsEndingBalance { get; set; }

	[JsonProperty("glsEndingBalanceForeign", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glsEndingBalanceForeign { get; set; }

	[JsonProperty("glsEndingDate", Order = 9)]
	[Required(ErrorMessage = "glsEndingDate is required.")]
	public DateTime? glsEndingDate { get; set; }

	[JsonProperty("glsUniqueID", Order = 10)]
	public Guid glsUniqueID { get; set; }

	[JsonProperty("glsExchangeAmount", Order = 11)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glsExchangeAmount { get; set; }

	[JsonProperty("glsExchangeGlAccountID", Order = 12)]
	[MaxLength(11)]
	public string glsExchangeGlAccountID { get; set; }

	[JsonProperty("glsExchangeRate", Order = 13)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glsExchangeRate { get; set; }

	[JsonProperty("glsGlFiscalYearID", Order = 14)]
	[Required(ErrorMessage = "glsGlFiscalYearID is required.")]
	public short glsGlFiscalYearID { get; set; }

	[JsonProperty("glsCustomRate", Order = 15)]
	public bool glsCustomRate { get; set; }

	[JsonProperty("glsPostedToGl", Order = 16)]
	public bool glsPostedToGl { get; set; }

	[JsonProperty("glsOpeningBalance", Order = 17)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glsOpeningBalance { get; set; }

	[JsonProperty("glsOpeningBalanceForeign", Order = 18)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glsOpeningBalanceForeign { get; set; }

	[JsonProperty("glsOpeningDate", Order = 19)]
	[Required(ErrorMessage = "glsOpeningDate is required.")]
	public DateTime? glsOpeningDate { get; set; }

	[JsonProperty("glsPostedDate", Order = 20)]
	public DateTime? glsPostedDate { get; set; }

	[JsonProperty("glsRowVersion", Order = 21)]
	public byte[] glsRowVersion { get; set; }

	[JsonProperty("glsBankStatementID", Order = 22)]
	[Required(ErrorMessage = "glsBankStatementID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int glsBankStatementID { get; set; }

	[JsonProperty("glsShowTransactions", Order = 23)]
	public bool glsShowTransactions { get; set; }

	[JsonProperty("customFields", Order = 24)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
