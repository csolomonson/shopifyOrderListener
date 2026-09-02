using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSheetCalculatorDto
{
	[JsonProperty("ccs0Rotation", Order = 1)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccs0Rotation { get; set; }

	[JsonProperty("ccs90Rotation", Order = 2)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccs90Rotation { get; set; }

	[JsonProperty("ccsSheetCalculatorID", Order = 3)]
	[Required(ErrorMessage = "ccsSheetCalculatorID is required.")]
	public Guid ccsSheetCalculatorID { get; set; }

	[JsonProperty("ccsCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string ccsCreatedBy { get; set; }

	[JsonProperty("ccsCreatedDate", Order = 5)]
	public DateTime? ccsCreatedDate { get; set; }

	[JsonProperty("ccsUniqueID", Order = 6)]
	public Guid ccsUniqueID { get; set; }

	[JsonProperty("ccsGrain", Order = 7)]
	public bool ccsGrain { get; set; }

	[JsonProperty("ccsMeasurementType", Order = 8)]
	[MaxLength(1)]
	public string ccsMeasurementType { get; set; }

	[JsonProperty("ccsPartSizeX", Order = 9)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccsPartSizeX { get; set; }

	[JsonProperty("ccsPartSizeY", Order = 10)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccsPartSizeY { get; set; }

	[JsonProperty("ccsPartSpacingX", Order = 11)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccsPartSpacingX { get; set; }

	[JsonProperty("ccsPartSpacingY", Order = 12)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccsPartSpacingY { get; set; }

	[JsonProperty("ccsRowVersion", Order = 13)]
	public byte[] ccsRowVersion { get; set; }

	[JsonProperty("ccsSheetSizeX", Order = 14)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccsSheetSizeX { get; set; }

	[JsonProperty("ccsSheetSizeY", Order = 15)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccsSheetSizeY { get; set; }

	[JsonProperty("ccsTotalTrimX", Order = 16)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccsTotalTrimX { get; set; }

	[JsonProperty("ccsTotalTrimY", Order = 17)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccsTotalTrimY { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
