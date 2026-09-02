using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAssetDto
{
	[JsonProperty("fapApInvoiceID", Order = 1)]
	[MaxLength(10)]
	public string fapApInvoiceID { get; set; }

	[JsonProperty("fapApInvoiceLineID", Order = 2)]
	public short fapApInvoiceLineID { get; set; }

	[JsonProperty("fapAssetTypeID", Order = 3)]
	[Required(ErrorMessage = "fapAssetTypeID is required.")]
	[MaxLength(5)]
	public string fapAssetTypeID { get; set; }

	[JsonProperty("fapBookDepreciationEndDate", Order = 4)]
	public DateTime? fapBookDepreciationEndDate { get; set; }

	[JsonProperty("fapBookDepreciationRate", Order = 5)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapBookDepreciationRate { get; set; }

	[JsonProperty("fapBookEffectiveLife", Order = 6)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapBookEffectiveLife { get; set; }

	[JsonProperty("fapBookStartValue", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapBookStartValue { get; set; }

	[JsonProperty("fapAssetID", Order = 8)]
	[Required(ErrorMessage = "fapAssetID is required.")]
	[MaxLength(10)]
	public string fapAssetID { get; set; }

	[JsonProperty("fapCreatedBy", Order = 9)]
	[MaxLength(20)]
	public string fapCreatedBy { get; set; }

	[JsonProperty("fapCreatedDate", Order = 10)]
	public DateTime? fapCreatedDate { get; set; }

	[JsonProperty("fapDeemedValue", Order = 11)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapDeemedValue { get; set; }

	[JsonProperty("fapDepreciationLimit", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapDepreciationLimit { get; set; }

	[JsonProperty("fapDepreciationStartDate", Order = 13)]
	public DateTime? fapDepreciationStartDate { get; set; }

	[JsonProperty("fapDescription", Order = 14)]
	[Required(ErrorMessage = "fapDescription is required.")]
	[MaxLength(50)]
	public string fapDescription { get; set; }

	[JsonProperty("fapDisposalDate", Order = 15)]
	public DateTime? fapDisposalDate { get; set; }

	[JsonProperty("fapDisposalValue", Order = 16)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapDisposalValue { get; set; }

	[JsonProperty("fapUniqueID", Order = 17)]
	public Guid fapUniqueID { get; set; }

	[JsonProperty("fapEstimatedProductionUnits", Order = 18)]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int fapEstimatedProductionUnits { get; set; }

	[JsonProperty("fapFinanceOrganizationID", Order = 19)]
	[MaxLength(10)]
	public string fapFinanceOrganizationID { get; set; }

	[JsonProperty("fapInServiceDate", Order = 20)]
	public DateTime? fapInServiceDate { get; set; }

	[JsonProperty("fapLowCostAsset", Order = 21)]
	public bool fapLowCostAsset { get; set; }

	[JsonProperty("fapLowValueAssetInPool", Order = 22)]
	public bool fapLowValueAssetInPool { get; set; }

	[JsonProperty("fapItemType", Order = 23)]
	[Required(ErrorMessage = "fapItemType is required.")]
	[MaxLength(1)]
	public string fapItemType { get; set; }

	[JsonProperty("fapLeaseExpiryDate", Order = 24)]
	public DateTime? fapLeaseExpiryDate { get; set; }

	[JsonProperty("fapLeaseMonths", Order = 25)]
	public short fapLeaseMonths { get; set; }

	[JsonProperty("fapLocation", Order = 26)]
	[Required(ErrorMessage = "fapLocation is required.")]
	[MaxLength(30)]
	public string fapLocation { get; set; }

	[JsonProperty("fapLongDescriptionRtf", Order = 27)]
	public string fapLongDescriptionRtf { get; set; }

	[JsonProperty("fapLongDescriptionText", Order = 28)]
	public string fapLongDescriptionText { get; set; }

	[JsonProperty("fapPaymentAmount", Order = 29)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapPaymentAmount { get; set; }

	[JsonProperty("fapPlantID", Order = 30)]
	[MaxLength(5)]
	public string fapPlantID { get; set; }

	[JsonProperty("fapPurchaseDate", Order = 31)]
	public DateTime? fapPurchaseDate { get; set; }

	[JsonProperty("fapPurchaseOrderID", Order = 32)]
	[MaxLength(10)]
	public string fapPurchaseOrderID { get; set; }

	[JsonProperty("fapPurchaseOrderLineID", Order = 33)]
	public short fapPurchaseOrderLineID { get; set; }

	[JsonProperty("fapPurchaseType", Order = 34)]
	[Required(ErrorMessage = "fapPurchaseType is required.")]
	[MaxLength(1)]
	public string fapPurchaseType { get; set; }

	[JsonProperty("fapPurchaseValue", Order = 35)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapPurchaseValue { get; set; }

	[JsonProperty("fapQuantity", Order = 36)]
	[Required(ErrorMessage = "fapQuantity is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int fapQuantity { get; set; }

	[JsonProperty("fapReceiptDate", Order = 37)]
	public DateTime? fapReceiptDate { get; set; }

	[JsonProperty("fapReceiptID", Order = 38)]
	[MaxLength(10)]
	public string fapReceiptID { get; set; }

	[JsonProperty("fapReceiptLineID", Order = 39)]
	public short fapReceiptLineID { get; set; }

	[JsonProperty("fapResidualAmount", Order = 40)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapResidualAmount { get; set; }

	[JsonProperty("fapRowVersion", Order = 41)]
	public byte[] fapRowVersion { get; set; }

	[JsonProperty("fapSerialNumber", Order = 42)]
	[MaxLength(30)]
	public string fapSerialNumber { get; set; }

	[JsonProperty("fapStartYearInPool", Order = 43)]
	public short fapStartYearInPool { get; set; }

	[JsonProperty("fapStatus", Order = 44)]
	[MaxLength(1)]
	public string fapStatus { get; set; }

	[JsonProperty("fapSupplierOrganizationID", Order = 45)]
	[MaxLength(10)]
	public string fapSupplierOrganizationID { get; set; }

	[JsonProperty("fapTaxableUsePercentage", Order = 46)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapTaxableUsePercentage { get; set; }

	[JsonProperty("fapTaxDepreciationEndDate", Order = 47)]
	public DateTime? fapTaxDepreciationEndDate { get; set; }

	[JsonProperty("fapTaxDepreciationRate", Order = 48)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapTaxDepreciationRate { get; set; }

	[JsonProperty("fapTaxEffectiveLife", Order = 49)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapTaxEffectiveLife { get; set; }

	[JsonProperty("fapTaxStartValue", Order = 50)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fapTaxStartValue { get; set; }

	[JsonProperty("fapWorkCenterID", Order = 51)]
	[MaxLength(5)]
	public string fapWorkCenterID { get; set; }

	[JsonProperty("customFields", Order = 52)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
