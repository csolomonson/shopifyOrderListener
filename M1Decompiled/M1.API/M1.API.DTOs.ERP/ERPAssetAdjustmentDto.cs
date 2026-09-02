using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAssetAdjustmentDto
{
	[JsonProperty("faaAccumulatedDepreciation", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal faaAccumulatedDepreciation { get; set; }

	[JsonProperty("faaAdjustmentDate", Order = 2)]
	[Required(ErrorMessage = "faaAdjustmentDate is required.")]
	public DateTime? faaAdjustmentDate { get; set; }

	[JsonProperty("faaAdjustmentType", Order = 3)]
	[Required(ErrorMessage = "faaAdjustmentType is required.")]
	[MaxLength(1)]
	public string faaAdjustmentType { get; set; }

	[JsonProperty("faaArInvoiceContactID", Order = 4)]
	[MaxLength(5)]
	public string faaArInvoiceContactID { get; set; }

	[JsonProperty("faaArInvoiceLocationID", Order = 5)]
	[MaxLength(5)]
	public string faaArInvoiceLocationID { get; set; }

	[JsonProperty("faaAssetID", Order = 6)]
	[Required(ErrorMessage = "faaAssetID is required.")]
	[MaxLength(10)]
	public string faaAssetID { get; set; }

	[JsonProperty("faaAuthorizedByEmployeeID", Order = 7)]
	[MaxLength(10)]
	public string faaAuthorizedByEmployeeID { get; set; }

	[JsonProperty("faaClosingPercent", Order = 8)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal faaClosingPercent { get; set; }

	[JsonProperty("faaClosingPeriodDepreciation", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal faaClosingPeriodDepreciation { get; set; }

	[JsonProperty("faaCreatedBy", Order = 10)]
	[MaxLength(20)]
	public string faaCreatedBy { get; set; }

	[JsonProperty("faaCreatedDate", Order = 11)]
	public DateTime? faaCreatedDate { get; set; }

	[JsonProperty("faaCurrencyRateID", Order = 12)]
	[MaxLength(5)]
	public string faaCurrencyRateID { get; set; }

	[JsonProperty("faaCustomerOrganizationID", Order = 13)]
	[MaxLength(10)]
	public string faaCustomerOrganizationID { get; set; }

	[JsonProperty("faaDepreciationThisYear", Order = 14)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal faaDepreciationThisYear { get; set; }

	[JsonProperty("faaDestinationPlantID", Order = 15)]
	[MaxLength(5)]
	public string faaDestinationPlantID { get; set; }

	[JsonProperty("faaUniqueID", Order = 16)]
	public Guid faaUniqueID { get; set; }

	[JsonProperty("faaExchangeRate", Order = 17)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal faaExchangeRate { get; set; }

	[JsonProperty("faaGlFiscalYearID", Order = 18)]
	public short faaGlFiscalYearID { get; set; }

	[JsonProperty("faaGlFiscalYearPeriodID", Order = 19)]
	public byte faaGlFiscalYearPeriodID { get; set; }

	[JsonProperty("faaCustomRate", Order = 20)]
	public bool faaCustomRate { get; set; }

	[JsonProperty("faaPostedToGl", Order = 21)]
	public bool faaPostedToGl { get; set; }

	[JsonProperty("faaLongDescriptionRtf", Order = 22)]
	public string faaLongDescriptionRtf { get; set; }

	[JsonProperty("faaLongDescriptionText", Order = 23)]
	public string faaLongDescriptionText { get; set; }

	[JsonProperty("faaNetAssetValue", Order = 24)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal faaNetAssetValue { get; set; }

	[JsonProperty("faaOpeningAssetValue", Order = 25)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal faaOpeningAssetValue { get; set; }

	[JsonProperty("faaPostedDate", Order = 26)]
	public DateTime? faaPostedDate { get; set; }

	[JsonProperty("faaProfitOrLoss", Order = 27)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal faaProfitOrLoss { get; set; }

	[JsonProperty("faaQuantity", Order = 28)]
	[Required(ErrorMessage = "faaQuantity is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int faaQuantity { get; set; }

	[JsonProperty("faaRowVersion", Order = 29)]
	public byte[] faaRowVersion { get; set; }

	[JsonProperty("faaAssetAdjustmentID", Order = 30)]
	[Required(ErrorMessage = "faaAssetAdjustmentID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int faaAssetAdjustmentID { get; set; }

	[JsonProperty("faaSourcePlantID", Order = 31)]
	[MaxLength(5)]
	public string faaSourcePlantID { get; set; }

	[JsonProperty("faaValue", Order = 32)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal faaValue { get; set; }

	[JsonProperty("faaValueForeign", Order = 33)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal faaValueForeign { get; set; }

	[JsonProperty("customFields", Order = 34)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
