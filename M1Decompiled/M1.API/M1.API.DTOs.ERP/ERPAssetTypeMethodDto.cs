using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAssetTypeMethodDto
{
	[JsonProperty("famAssetTypeID", Order = 1)]
	[Required(ErrorMessage = "famAssetTypeID is required.")]
	[MaxLength(5)]
	public string famAssetTypeID { get; set; }

	[JsonProperty("famBookDepreciationMethod", Order = 2)]
	[Required(ErrorMessage = "famBookDepreciationMethod is required.")]
	[MaxLength(5)]
	public string famBookDepreciationMethod { get; set; }

	[JsonProperty("famBookMultiplier", Order = 3)]
	[Required(ErrorMessage = "famBookMultiplier is required.")]
	[Range(0.0, 99.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal famBookMultiplier { get; set; }

	[JsonProperty("famCalculationMethod", Order = 4)]
	[Required(ErrorMessage = "famCalculationMethod is required.")]
	[MaxLength(1)]
	public string famCalculationMethod { get; set; }

	[JsonProperty("famCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string famCreatedBy { get; set; }

	[JsonProperty("famCreatedDate", Order = 6)]
	public DateTime? famCreatedDate { get; set; }

	[JsonProperty("famUniqueID", Order = 7)]
	public Guid famUniqueID { get; set; }

	[JsonProperty("famCurrentMethod", Order = 8)]
	public bool famCurrentMethod { get; set; }

	[JsonProperty("famMonthCalculationType", Order = 9)]
	[MaxLength(1)]
	public string famMonthCalculationType { get; set; }

	[JsonProperty("famRowVersion", Order = 10)]
	public byte[] famRowVersion { get; set; }

	[JsonProperty("famAssetTypeMethodID", Order = 11)]
	[Required(ErrorMessage = "famAssetTypeMethodID is required.")]
	public short famAssetTypeMethodID { get; set; }

	[JsonProperty("famStartDate", Order = 12)]
	[Required(ErrorMessage = "famStartDate is required.")]
	public DateTime? famStartDate { get; set; }

	[JsonProperty("famTaxDepreciationMethod", Order = 13)]
	[MaxLength(5)]
	public string famTaxDepreciationMethod { get; set; }

	[JsonProperty("famTaxMultiplier", Order = 14)]
	[Range(0.0, 99.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal famTaxMultiplier { get; set; }

	[JsonProperty("customFields", Order = 15)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
