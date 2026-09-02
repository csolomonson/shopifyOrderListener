using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAPInvoiceLineDto
{
	[JsonProperty("aplApInvoiceID", Order = 1)]
	[Required(ErrorMessage = "aplApInvoiceID is required.")]
	[MaxLength(10)]
	public string aplApInvoiceID { get; set; }

	[JsonProperty("aplAssetID", Order = 2)]
	[MaxLength(10)]
	public string aplAssetID { get; set; }

	[JsonProperty("aplAssetTypeID", Order = 3)]
	[MaxLength(5)]
	public string aplAssetTypeID { get; set; }

	[JsonProperty("aplConversionFactor", Order = 4)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplConversionFactor { get; set; }

	[JsonProperty("aplCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string aplCreatedBy { get; set; }

	[JsonProperty("aplCreatedDate", Order = 6)]
	public DateTime? aplCreatedDate { get; set; }

	[JsonProperty("aplDmrClaimID", Order = 7)]
	[MaxLength(10)]
	public string aplDmrClaimID { get; set; }

	[JsonProperty("aplDmrClaimLineID", Order = 8)]
	public short aplDmrClaimLineID { get; set; }

	[JsonProperty("aplDmrShipmentID", Order = 9)]
	[MaxLength(10)]
	public string aplDmrShipmentID { get; set; }

	[JsonProperty("aplDmrShipmentLineID", Order = 10)]
	public short aplDmrShipmentLineID { get; set; }

	[JsonProperty("aplUniqueID", Order = 11)]
	public Guid aplUniqueID { get; set; }

	[JsonProperty("aplExtendedCostBase", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplExtendedCostBase { get; set; }

	[JsonProperty("aplExtendedCostForeign", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplExtendedCostForeign { get; set; }

	[JsonProperty("aplForm1099Box", Order = 14)]
	public byte aplForm1099Box { get; set; }

	[JsonProperty("aplInvoicedComplete", Order = 15)]
	public bool aplInvoicedComplete { get; set; }

	[JsonProperty("aplPostedToGl", Order = 16)]
	public bool aplPostedToGl { get; set; }

	[JsonProperty("aplRetention", Order = 17)]
	public bool aplRetention { get; set; }

	[JsonProperty("aplItemType", Order = 18)]
	[MaxLength(1)]
	public string aplItemType { get; set; }

	[JsonProperty("aplJobAssemblyID", Order = 19)]
	public int aplJobAssemblyID { get; set; }

	[JsonProperty("aplJobID", Order = 20)]
	[MaxLength(20)]
	public string aplJobID { get; set; }

	[JsonProperty("aplJobMaterialID", Order = 21)]
	public int aplJobMaterialID { get; set; }

	[JsonProperty("aplJobOperationID", Order = 22)]
	public int aplJobOperationID { get; set; }

	[JsonProperty("aplJobType", Order = 23)]
	public byte aplJobType { get; set; }

	[JsonProperty("aplLandedCostChargeID", Order = 24)]
	public short aplLandedCostChargeID { get; set; }

	[JsonProperty("aplLandedCostID", Order = 25)]
	[MaxLength(10)]
	public string aplLandedCostID { get; set; }

	[JsonProperty("aplNonTaxReasonID", Order = 26)]
	[MaxLength(5)]
	public string aplNonTaxReasonID { get; set; }

	[JsonProperty("aplOrgPartID", Order = 27)]
	[MaxLength(30)]
	public string aplOrgPartID { get; set; }

	[JsonProperty("aplOrgPartShortDescription", Order = 28)]
	[MaxLength(50)]
	public string aplOrgPartShortDescription { get; set; }

	[JsonProperty("aplPartDescription", Order = 29)]
	[MaxLength(50)]
	public string aplPartDescription { get; set; }

	[JsonProperty("aplPartID", Order = 30)]
	[MaxLength(30)]
	public string aplPartID { get; set; }

	[JsonProperty("aplPartLongDescriptionRtf", Order = 31)]
	public string aplPartLongDescriptionRtf { get; set; }

	[JsonProperty("aplPartLongDescriptionText", Order = 32)]
	public string aplPartLongDescriptionText { get; set; }

	[JsonProperty("aplPartRevisionID", Order = 33)]
	[MaxLength(15)]
	public string aplPartRevisionID { get; set; }

	[JsonProperty("aplProjectAreaID", Order = 34)]
	[MaxLength(15)]
	public string aplProjectAreaID { get; set; }

	[JsonProperty("aplProjectID", Order = 35)]
	[MaxLength(10)]
	public string aplProjectID { get; set; }

	[JsonProperty("aplPurchaseOrderID", Order = 36)]
	[MaxLength(10)]
	public string aplPurchaseOrderID { get; set; }

	[JsonProperty("aplPurchaseOrderLineID", Order = 37)]
	public short aplPurchaseOrderLineID { get; set; }

	[JsonProperty("aplPurchaseQuantity", Order = 38)]
	[Required(ErrorMessage = "aplPurchaseQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplPurchaseQuantity { get; set; }

	[JsonProperty("aplPurchaseUnitCostBase", Order = 39)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplPurchaseUnitCostBase { get; set; }

	[JsonProperty("aplPurchaseUnitCostForeign", Order = 40)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplPurchaseUnitCostForeign { get; set; }

	[JsonProperty("aplPurchaseUnitOfMeasure", Order = 41)]
	[MaxLength(2)]
	public string aplPurchaseUnitOfMeasure { get; set; }

	[JsonProperty("aplReceiptID", Order = 42)]
	[MaxLength(10)]
	public string aplReceiptID { get; set; }

	[JsonProperty("aplReceiptLineID", Order = 43)]
	public short aplReceiptLineID { get; set; }

	[JsonProperty("aplReceivedQuantity", Order = 44)]
	[Required(ErrorMessage = "aplReceivedQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplReceivedQuantity { get; set; }

	[JsonProperty("aplReceivedUnitOfMeasure", Order = 45)]
	[MaxLength(2)]
	public string aplReceivedUnitOfMeasure { get; set; }

	[JsonProperty("aplRetentionAmountBase", Order = 46)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplRetentionAmountBase { get; set; }

	[JsonProperty("aplRetentionAmountForeign", Order = 47)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplRetentionAmountForeign { get; set; }

	[JsonProperty("aplRetentionPercent", Order = 48)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplRetentionPercent { get; set; }

	[JsonProperty("aplRetentionReleaseDate", Order = 49)]
	public DateTime? aplRetentionReleaseDate { get; set; }

	[JsonProperty("aplRmaClaimID", Order = 50)]
	[MaxLength(10)]
	public string aplRmaClaimID { get; set; }

	[JsonProperty("aplRmaClaimLineID", Order = 51)]
	public short aplRmaClaimLineID { get; set; }

	[JsonProperty("aplRowVersion", Order = 52)]
	public byte[] aplRowVersion { get; set; }

	[JsonProperty("aplSecondTaxAmountBase", Order = 53)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplSecondTaxAmountBase { get; set; }

	[JsonProperty("aplSecondTaxAmountForeign", Order = 54)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplSecondTaxAmountForeign { get; set; }

	[JsonProperty("aplSecondTaxCodeID", Order = 55)]
	[MaxLength(5)]
	public string aplSecondTaxCodeID { get; set; }

	[JsonProperty("aplApInvoiceLineID", Order = 56)]
	public short aplApInvoiceLineID { get; set; }

	[JsonProperty("aplSetupChargeBase", Order = 57)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplSetupChargeBase { get; set; }

	[JsonProperty("aplSetupChargeForeign", Order = 58)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplSetupChargeForeign { get; set; }

	[JsonProperty("aplTaxAmountBase", Order = 59)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplTaxAmountBase { get; set; }

	[JsonProperty("aplTaxAmountForeign", Order = 60)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplTaxAmountForeign { get; set; }

	[JsonProperty("aplTaxCodeID", Order = 61)]
	[MaxLength(5)]
	public string aplTaxCodeID { get; set; }

	[JsonProperty("aplTotalExtendedCostBase", Order = 62)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplTotalExtendedCostBase { get; set; }

	[JsonProperty("aplTotalExtendedCostForeign", Order = 63)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aplTotalExtendedCostForeign { get; set; }

	[JsonProperty("customFields", Order = 64)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
