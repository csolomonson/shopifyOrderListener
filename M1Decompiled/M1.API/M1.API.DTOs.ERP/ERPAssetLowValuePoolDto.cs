using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAssetLowValuePoolDto
{
	[JsonProperty("favClosedDate", Order = 1)]
	public DateTime? favClosedDate { get; set; }

	[JsonProperty("favCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string favCreatedBy { get; set; }

	[JsonProperty("favCreatedDate", Order = 3)]
	public DateTime? favCreatedDate { get; set; }

	[JsonProperty("favEndingBalance", Order = 4)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal favEndingBalance { get; set; }

	[JsonProperty("favUniqueID", Order = 5)]
	public Guid favUniqueID { get; set; }

	[JsonProperty("favHighRate", Order = 6)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal favHighRate { get; set; }

	[JsonProperty("favHighRateDepreciation", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal favHighRateDepreciation { get; set; }

	[JsonProperty("favImprovement", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal favImprovement { get; set; }

	[JsonProperty("favClosed", Order = 9)]
	public bool favClosed { get; set; }

	[JsonProperty("favLowCostAddition", Order = 10)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal favLowCostAddition { get; set; }

	[JsonProperty("favLowRate", Order = 11)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal favLowRate { get; set; }

	[JsonProperty("favLowRateDepreciation", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal favLowRateDepreciation { get; set; }

	[JsonProperty("favLowValueAddition", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal favLowValueAddition { get; set; }

	[JsonProperty("favOpeningBalance", Order = 14)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal favOpeningBalance { get; set; }

	[JsonProperty("favPoolYearID", Order = 15)]
	[Required(ErrorMessage = "favPoolYearID is required.")]
	public short favPoolYearID { get; set; }

	[JsonProperty("favRowVersion", Order = 16)]
	public byte[] favRowVersion { get; set; }

	[JsonProperty("favTermination", Order = 17)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal favTermination { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
