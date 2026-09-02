using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLaserCalculatorLineDto
{
	[JsonProperty("cclCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string cclCreatedBy { get; set; }

	[JsonProperty("cclCreatedDate", Order = 2)]
	public DateTime? cclCreatedDate { get; set; }

	[JsonProperty("cclCutTime", Order = 3)]
	[Range(0.0, 999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cclCutTime { get; set; }

	[JsonProperty("cclDescription", Order = 4)]
	[MaxLength(30)]
	public string cclDescription { get; set; }

	[JsonProperty("cclUniqueID", Order = 5)]
	public Guid cclUniqueID { get; set; }

	[JsonProperty("cclLaserCalculatorID", Order = 6)]
	[Required(ErrorMessage = "cclLaserCalculatorID is required.")]
	public Guid cclLaserCalculatorID { get; set; }

	[JsonProperty("ccllength", Order = 7)]
	[Range(0.0, 99999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ccllength { get; set; }

	[JsonProperty("cclQuantity", Order = 8)]
	[Range(0.0, 99999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cclQuantity { get; set; }

	[JsonProperty("cclRate", Order = 9)]
	[Range(0.0, 99999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cclRate { get; set; }

	[JsonProperty("cclRowVersion", Order = 10)]
	public byte[] cclRowVersion { get; set; }

	[JsonProperty("cclLaserCalculatorLineID", Order = 11)]
	[Required(ErrorMessage = "cclLaserCalculatorLineID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int cclLaserCalculatorLineID { get; set; }

	[JsonProperty("cclWidth", Order = 12)]
	[Range(0.0, 99999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cclWidth { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
