using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPunchCalculatorDto
{
	[JsonProperty("ccuPunchCalculatorId", Order = 1)]
	[Required(ErrorMessage = "ccuPunchCalculatorId is required.")]
	public Guid ccuPunchCalculatorId { get; set; }

	[JsonProperty("ccuCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string ccuCreatedBy { get; set; }

	[JsonProperty("ccuCreatedDate", Order = 3)]
	public DateTime? ccuCreatedDate { get; set; }

	[JsonProperty("ccuUniqueID", Order = 4)]
	public Guid ccuUniqueID { get; set; }

	[JsonProperty("ccuHitRate", Order = 5)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuHitRate { get; set; }

	[JsonProperty("ccuHitsPerPart", Order = 6)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuHitsPerPart { get; set; }

	[JsonProperty("ccuPartsPerHour", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccuPartsPerHour { get; set; }

	[JsonProperty("ccuPartsPerSheet", Order = 8)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuPartsPerSheet { get; set; }

	[JsonProperty("ccuRepositions", Order = 9)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuRepositions { get; set; }

	[JsonProperty("ccuRepositionTime", Order = 10)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccuRepositionTime { get; set; }

	[JsonProperty("ccuRepositionTimeSec", Order = 11)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuRepositionTimeSec { get; set; }

	[JsonProperty("ccuRowVersion", Order = 12)]
	public byte[] ccuRowVersion { get; set; }

	[JsonProperty("ccuSheetLoadTime", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccuSheetLoadTime { get; set; }

	[JsonProperty("ccuSheetLoadTimeSec", Order = 14)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuSheetLoadTimeSec { get; set; }

	[JsonProperty("ccuSheetsPerHour", Order = 15)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccuSheetsPerHour { get; set; }

	[JsonProperty("ccuTimeToPiece", Order = 16)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccuTimeToPiece { get; set; }

	[JsonProperty("ccuToolChangeTimeSec", Order = 17)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuToolChangeTimeSec { get; set; }

	[JsonProperty("ccuToolChangeTimeTotal", Order = 18)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuToolChangeTimeTotal { get; set; }

	[JsonProperty("ccuTools", Order = 19)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuTools { get; set; }

	[JsonProperty("ccuTotalTimeMinutes", Order = 20)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccuTotalTimeMinutes { get; set; }

	[JsonProperty("ccuTotalTimeSeconds", Order = 21)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuTotalTimeSeconds { get; set; }

	[JsonProperty("ccuTurns", Order = 22)]
	[Range(0, 99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int ccuTurns { get; set; }

	[JsonProperty("customFields", Order = 23)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
