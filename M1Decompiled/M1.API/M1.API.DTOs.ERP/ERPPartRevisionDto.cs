using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartRevisionDto
{
	[JsonProperty("imrAverageDutyCost", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrAverageDutyCost { get; set; }

	[JsonProperty("imrAverageFreightCost", Order = 2)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrAverageFreightCost { get; set; }

	[JsonProperty("imrAverageLaborCost", Order = 3)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrAverageLaborCost { get; set; }

	[JsonProperty("imrAverageMaterialCost", Order = 4)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrAverageMaterialCost { get; set; }

	[JsonProperty("imrAverageMiscCost", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrAverageMiscCost { get; set; }

	[JsonProperty("imrAverageOverheadCost", Order = 6)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrAverageOverheadCost { get; set; }

	[JsonProperty("imrAverageSubcontractCost", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrAverageSubcontractCost { get; set; }

	[JsonProperty("imrBarLength", Order = 8)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrBarLength { get; set; }

	[JsonProperty("imrBlanketPeriodBegin", Order = 9)]
	public DateTime? imrBlanketPeriodBegin { get; set; }

	[JsonProperty("imrBlanketPeriodEnd", Order = 10)]
	public DateTime? imrBlanketPeriodEnd { get; set; }

	[JsonProperty("imrPartRevisionID", Order = 11)]
	[MaxLength(15)]
	public string imrPartRevisionID { get; set; }

	[JsonProperty("imrCommodityCode", Order = 12)]
	[MaxLength(20)]
	public string imrCommodityCode { get; set; }

	[JsonProperty("imrCommodityDescription", Order = 13)]
	[MaxLength(35)]
	public string imrCommodityDescription { get; set; }

	[JsonProperty("imrConversionFactor", Order = 14)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrConversionFactor { get; set; }

	[JsonProperty("imrCountryOfManufacture", Order = 15)]
	[MaxLength(2)]
	public string imrCountryOfManufacture { get; set; }

	[JsonProperty("imrCreatedBy", Order = 16)]
	[MaxLength(20)]
	public string imrCreatedBy { get; set; }

	[JsonProperty("imrCreatedDate", Order = 17)]
	public DateTime? imrCreatedDate { get; set; }

	[JsonProperty("imrDocuments", Order = 18)]
	[MaxLength(50)]
	public string imrDocuments { get; set; }

	[JsonProperty("imrEffectiveEndDate", Order = 19)]
	public DateTime? imrEffectiveEndDate { get; set; }

	[JsonProperty("imrEffectiveStartDate", Order = 20)]
	[Required(ErrorMessage = "imrEffectiveStartDate is required.")]
	public DateTime? imrEffectiveStartDate { get; set; }

	[JsonProperty("imrUniqueID", Order = 21)]
	public Guid imrUniqueID { get; set; }

	[JsonProperty("imrExpenseSplitPercentTotal", Order = 22)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrExpenseSplitPercentTotal { get; set; }

	[JsonProperty("imrFdxHandlingCost", Order = 23)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrFdxHandlingCost { get; set; }

	[JsonProperty("imrFdxPackageHeight", Order = 24)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imrFdxPackageHeight { get; set; }

	[JsonProperty("imrFdxPackageLength", Order = 25)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imrFdxPackageLength { get; set; }

	[JsonProperty("imrFdxPackageWidth", Order = 26)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int imrFdxPackageWidth { get; set; }

	[JsonProperty("imrFdxPackaging", Order = 27)]
	[MaxLength(14)]
	public string imrFdxPackaging { get; set; }

	[JsonProperty("imrFdxPackagingCost", Order = 28)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrFdxPackagingCost { get; set; }

	[JsonProperty("imrFdxShipCostMarkupPct", Order = 29)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrFdxShipCostMarkupPct { get; set; }

	[JsonProperty("imrFormID", Order = 30)]
	[MaxLength(75)]
	public string imrFormID { get; set; }

	[JsonProperty("imrInspectionNotesRTF", Order = 31)]
	[MaxLength(50)]
	public string imrInspectionNotesRTF { get; set; }

	[JsonProperty("imrInspectionNotesText", Order = 32)]
	[MaxLength(50)]
	public string imrInspectionNotesText { get; set; }

	[JsonProperty("imrInventoryUnitOfMeasure", Order = 33)]
	[MaxLength(2)]
	public string imrInventoryUnitOfMeasure { get; set; }

	[JsonProperty("imrInactive", Order = 34)]
	public bool imrInactive { get; set; }

	[JsonProperty("imrConfigured", Order = 35)]
	public bool imrConfigured { get; set; }

	[JsonProperty("imrFdxNonstandardContainer", Order = 36)]
	public bool imrFdxNonstandardContainer { get; set; }

	[JsonProperty("imrFdxOneItemPerShipment", Order = 37)]
	public bool imrFdxOneItemPerShipment { get; set; }

	[JsonProperty("imrPreferredRefExists", Order = 38)]
	public bool imrPreferredRefExists { get; set; }

	[JsonProperty("imrPurchasableItem", Order = 39)]
	public bool imrPurchasableItem { get; set; }

	[JsonProperty("imrSuppressShortDescription", Order = 40)]
	public bool imrSuppressShortDescription { get; set; }

	[JsonProperty("imrUseQuotePrice", Order = 41)]
	public bool imrUseQuotePrice { get; set; }

	[JsonProperty("imrLastDutyCost", Order = 42)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrLastDutyCost { get; set; }

	[JsonProperty("imrLastFreightCost", Order = 43)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrLastFreightCost { get; set; }

	[JsonProperty("imrLastLaborCost", Order = 44)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrLastLaborCost { get; set; }

	[JsonProperty("imrLastMaterialCost", Order = 45)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrLastMaterialCost { get; set; }

	[JsonProperty("imrLastMiscCost", Order = 46)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrLastMiscCost { get; set; }

	[JsonProperty("imrLastOverheadCost", Order = 47)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrLastOverheadCost { get; set; }

	[JsonProperty("imrLastReceiptDate", Order = 48)]
	public DateTime? imrLastReceiptDate { get; set; }

	[JsonProperty("imrLastRunDatePurchasePlanner", Order = 49)]
	public DateTime? imrLastRunDatePurchasePlanner { get; set; }

	[JsonProperty("imrLastSubcontractCost", Order = 50)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrLastSubcontractCost { get; set; }

	[JsonProperty("imrLastTransactionDate", Order = 51)]
	public DateTime? imrLastTransactionDate { get; set; }

	[JsonProperty("imrLeadTime", Order = 52)]
	public short imrLeadTime { get; set; }

	[JsonProperty("imrLongDescriptionHtml", Order = 53)]
	[MaxLength(50)]
	public string imrLongDescriptionHtml { get; set; }

	[JsonProperty("imrLongDescriptionRtf", Order = 54)]
	public string imrLongDescriptionRtf { get; set; }

	[JsonProperty("imrLongDescriptionText", Order = 55)]
	public string imrLongDescriptionText { get; set; }

	[JsonProperty("imrManufacturingLotSize", Order = 56)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrManufacturingLotSize { get; set; }

	[JsonProperty("imrMaximumQuantity", Order = 57)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrMaximumQuantity { get; set; }

	[JsonProperty("imrMinimumQuantity", Order = 58)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrMinimumQuantity { get; set; }

	[JsonProperty("imrNetCostBeginDate", Order = 59)]
	public DateTime? imrNetCostBeginDate { get; set; }

	[JsonProperty("imrNetCostCode", Order = 60)]
	[MaxLength(2)]
	public string imrNetCostCode { get; set; }

	[JsonProperty("imrNetCostEndDate", Order = 61)]
	public DateTime? imrNetCostEndDate { get; set; }

	[JsonProperty("imrPartID", Order = 62)]
	[Required(ErrorMessage = "imrPartID is required.")]
	[MaxLength(30)]
	public string imrPartID { get; set; }

	[JsonProperty("imrPartImageFileName", Order = 63)]
	[MaxLength(70)]
	public string imrPartImageFileName { get; set; }

	[JsonProperty("imrPreferenceCriteria", Order = 64)]
	[MaxLength(1)]
	public string imrPreferenceCriteria { get; set; }

	[JsonProperty("imrProducerDetermination", Order = 65)]
	[MaxLength(5)]
	public string imrProducerDetermination { get; set; }

	[JsonProperty("imrProductCategoryID", Order = 66)]
	[MaxLength(30)]
	public string imrProductCategoryID { get; set; }

	[JsonProperty("imrProductCategoryLineID", Order = 67)]
	public short imrProductCategoryLineID { get; set; }

	[JsonProperty("imrProductionNotesRTF", Order = 68)]
	[MaxLength(50)]
	public string imrProductionNotesRTF { get; set; }

	[JsonProperty("imrProductionNotesText", Order = 69)]
	[MaxLength(50)]
	public string imrProductionNotesText { get; set; }

	[JsonProperty("imrPurchaseLocationID", Order = 70)]
	[MaxLength(5)]
	public string imrPurchaseLocationID { get; set; }

	[JsonProperty("imrPurchaseUnitOfMeasure", Order = 71)]
	[MaxLength(2)]
	public string imrPurchaseUnitOfMeasure { get; set; }

	[JsonProperty("imrQuantityAllocated", Order = 72)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrQuantityAllocated { get; set; }

	[JsonProperty("imrQuantityOnHand", Order = 73)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrQuantityOnHand { get; set; }

	[JsonProperty("imrQuantityOnOrderPurchases", Order = 74)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrQuantityOnOrderPurchases { get; set; }

	[JsonProperty("imrQuantityOnOrderSales", Order = 75)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrQuantityOnOrderSales { get; set; }

	[JsonProperty("imrQuantityToInspect", Order = 76)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrQuantityToInspect { get; set; }

	[JsonProperty("imrQuantityToReturn", Order = 77)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrQuantityToReturn { get; set; }

	[JsonProperty("imrQuantityToReturnJob", Order = 78)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrQuantityToReturnJob { get; set; }

	[JsonProperty("imrRequiresInspection", Order = 79)]
	public byte imrRequiresInspection { get; set; }

	[JsonProperty("imrRowVersion", Order = 80)]
	public byte[] imrRowVersion { get; set; }

	[JsonProperty("imrSheetSizeX", Order = 81)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrSheetSizeX { get; set; }

	[JsonProperty("imrSheetSizeY", Order = 82)]
	[Range(0.0, 999999999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrSheetSizeY { get; set; }

	[JsonProperty("imrShortDescription", Order = 83)]
	[Required(ErrorMessage = "imrShortDescription is required.")]
	[MaxLength(50)]
	public string imrShortDescription { get; set; }

	[JsonProperty("imrSourceMethodID", Order = 84)]
	[MaxLength(30)]
	public string imrSourceMethodID { get; set; }

	[JsonProperty("imrSourceRevisionID", Order = 85)]
	[MaxLength(15)]
	public string imrSourceRevisionID { get; set; }

	[JsonProperty("imrStandardDutyCost", Order = 86)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrStandardDutyCost { get; set; }

	[JsonProperty("imrStandardFreightCost", Order = 87)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrStandardFreightCost { get; set; }

	[JsonProperty("imrStandardLaborCost", Order = 88)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrStandardLaborCost { get; set; }

	[JsonProperty("imrStandardMaterialCost", Order = 89)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrStandardMaterialCost { get; set; }

	[JsonProperty("imrStandardMiscCost", Order = 90)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrStandardMiscCost { get; set; }

	[JsonProperty("imrStandardOverheadCost", Order = 91)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrStandardOverheadCost { get; set; }

	[JsonProperty("imrStandardSubcontractCost", Order = 92)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrStandardSubcontractCost { get; set; }

	[JsonProperty("imrSupplierOrganizationID", Order = 93)]
	[MaxLength(10)]
	public string imrSupplierOrganizationID { get; set; }

	[JsonProperty("imrThickness", Order = 94)]
	[Range(0.0, 1000000000000.0, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrThickness { get; set; }

	[JsonProperty("imrUniversalProductCode", Order = 95)]
	[MaxLength(13)]
	public string imrUniversalProductCode { get; set; }

	[JsonProperty("imrVolume", Order = 96)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrVolume { get; set; }

	[JsonProperty("imrWeight", Order = 97)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imrWeight { get; set; }

	[JsonProperty("imrWeightUnitOfMeasure", Order = 98)]
	[MaxLength(3)]
	public string imrWeightUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 99)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
