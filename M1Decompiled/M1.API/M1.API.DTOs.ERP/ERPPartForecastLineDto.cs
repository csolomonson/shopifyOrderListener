using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartForecastLineDto
{
	[JsonProperty("inlActualBalance", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inlActualBalance { get; set; }

	[JsonProperty("inlActualQuantity", Order = 2)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inlActualQuantity { get; set; }

	[JsonProperty("inlCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string inlCreatedBy { get; set; }

	[JsonProperty("inlCreatedDate", Order = 4)]
	public DateTime? inlCreatedDate { get; set; }

	[JsonProperty("inlEndDate", Order = 5)]
	public DateTime? inlEndDate { get; set; }

	[JsonProperty("inlUniqueID", Order = 6)]
	public Guid inlUniqueID { get; set; }

	[JsonProperty("inlForecastBalance", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inlForecastBalance { get; set; }

	[JsonProperty("inlForecastQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inlForecastQuantity { get; set; }

	[JsonProperty("inlIncludeInMRP", Order = 9)]
	public bool inlIncludeInMRP { get; set; }

	[JsonProperty("inlPartForecastPeriodID", Order = 10)]
	public short inlPartForecastPeriodID { get; set; }

	[JsonProperty("inlPartForecastYearID", Order = 11)]
	public short inlPartForecastYearID { get; set; }

	[JsonProperty("inlPartID", Order = 12)]
	[MaxLength(30)]
	public string inlPartID { get; set; }

	[JsonProperty("inlPartRevisionID", Order = 13)]
	[MaxLength(15)]
	public string inlPartRevisionID { get; set; }

	[JsonProperty("inlRemainingQuantity", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inlRemainingQuantity { get; set; }

	[JsonProperty("inlRemainingQuantityBalance", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inlRemainingQuantityBalance { get; set; }

	[JsonProperty("inlRowVersion", Order = 16)]
	public byte[] inlRowVersion { get; set; }

	[JsonProperty("inlStartDate", Order = 17)]
	public DateTime? inlStartDate { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
