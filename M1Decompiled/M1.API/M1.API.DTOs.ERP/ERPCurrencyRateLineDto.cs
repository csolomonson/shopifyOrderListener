using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCurrencyRateLineDto
{
	[JsonProperty("mclCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string mclCreatedBy { get; set; }

	[JsonProperty("mclCreatedDate", Order = 2)]
	public DateTime? mclCreatedDate { get; set; }

	[JsonProperty("mclCurrencyRateID", Order = 3)]
	[Required(ErrorMessage = "mclCurrencyRateID is required.")]
	[MaxLength(5)]
	public string mclCurrencyRateID { get; set; }

	[JsonProperty("mclEffectiveDate", Order = 4)]
	[Required(ErrorMessage = "mclEffectiveDate is required.")]
	public DateTime? mclEffectiveDate { get; set; }

	[JsonProperty("mclUniqueID", Order = 5)]
	public Guid mclUniqueID { get; set; }

	[JsonProperty("mclExchangeRate", Order = 6)]
	[Required(ErrorMessage = "mclExchangeRate is required.")]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal mclExchangeRate { get; set; }

	[JsonProperty("mclReference", Order = 7)]
	[MaxLength(50)]
	public string mclReference { get; set; }

	[JsonProperty("mclRowVersion", Order = 8)]
	public byte[] mclRowVersion { get; set; }

	[JsonProperty("mclCurrencyRateLineID", Order = 9)]
	[Required(ErrorMessage = "mclCurrencyRateLineID is required.")]
	[Range(0, 9999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int mclCurrencyRateLineID { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
