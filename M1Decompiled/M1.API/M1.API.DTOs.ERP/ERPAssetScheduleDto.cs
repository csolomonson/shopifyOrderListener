using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAssetScheduleDto
{
	[JsonProperty("fasActualProductionUnits", Order = 1)]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int fasActualProductionUnits { get; set; }

	[JsonProperty("fasAdditionalAssetAmount", Order = 2)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fasAdditionalAssetAmount { get; set; }

	[JsonProperty("fasAssetID", Order = 3)]
	[Required(ErrorMessage = "fasAssetID is required.")]
	[MaxLength(10)]
	public string fasAssetID { get; set; }

	[JsonProperty("fasClosingAccumBalance", Order = 4)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fasClosingAccumBalance { get; set; }

	[JsonProperty("fasClosingAssetValue", Order = 5)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fasClosingAssetValue { get; set; }

	[JsonProperty("fasCreatedBy", Order = 6)]
	[MaxLength(20)]
	public string fasCreatedBy { get; set; }

	[JsonProperty("fasCreatedDate", Order = 7)]
	public DateTime? fasCreatedDate { get; set; }

	[JsonProperty("fasDepreciationAmount", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fasDepreciationAmount { get; set; }

	[JsonProperty("fasUniqueID", Order = 9)]
	public Guid fasUniqueID { get; set; }

	[JsonProperty("fasEstimatedProductionUnits", Order = 10)]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int fasEstimatedProductionUnits { get; set; }

	[JsonProperty("fasGlFiscalYearID", Order = 11)]
	[Required(ErrorMessage = "fasGlFiscalYearID is required.")]
	public short fasGlFiscalYearID { get; set; }

	[JsonProperty("fasGlFiscalYearPeriodID", Order = 12)]
	[Required(ErrorMessage = "fasGlFiscalYearPeriodID is required.")]
	public byte fasGlFiscalYearPeriodID { get; set; }

	[JsonProperty("fasPostedToGl", Order = 13)]
	public bool fasPostedToGl { get; set; }

	[JsonProperty("fasNetAssetValue", Order = 14)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fasNetAssetValue { get; set; }

	[JsonProperty("fasOpeningAccumBalance", Order = 15)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fasOpeningAccumBalance { get; set; }

	[JsonProperty("fasOpeningAssetValue", Order = 16)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fasOpeningAssetValue { get; set; }

	[JsonProperty("fasRowVersion", Order = 17)]
	public byte[] fasRowVersion { get; set; }

	[JsonProperty("fasAssetScheduleID", Order = 18)]
	[Required(ErrorMessage = "fasAssetScheduleID is required.")]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int fasAssetScheduleID { get; set; }

	[JsonProperty("fasSubtractAssetAmount", Order = 19)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fasSubtractAssetAmount { get; set; }

	[JsonProperty("fasType", Order = 20)]
	[Required(ErrorMessage = "fasType is required.")]
	[MaxLength(5)]
	public string fasType { get; set; }

	[JsonProperty("fasWritebackAmount", Order = 21)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fasWritebackAmount { get; set; }

	[JsonProperty("customFields", Order = 22)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
