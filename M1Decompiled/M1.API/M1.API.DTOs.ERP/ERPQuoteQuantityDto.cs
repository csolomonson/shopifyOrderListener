using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPQuoteQuantityDto
{
	[JsonProperty("qmqAdditionalChargeBase", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqAdditionalChargeBase { get; set; }

	[JsonProperty("qmqAdditionalChargeDescription", Order = 2)]
	[MaxLength(50)]
	public string qmqAdditionalChargeDescription { get; set; }

	[JsonProperty("qmqAdditionalChargeForeign", Order = 3)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqAdditionalChargeForeign { get; set; }

	[JsonProperty("qmqAdditionalCostAmount", Order = 4)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqAdditionalCostAmount { get; set; }

	[JsonProperty("qmqAdditionalCostDescription", Order = 5)]
	[MaxLength(50)]
	public string qmqAdditionalCostDescription { get; set; }

	[JsonProperty("qmqAdditionalCostPrice", Order = 6)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqAdditionalCostPrice { get; set; }

	[JsonProperty("qmqAdditionalMarkupPercent", Order = 7)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqAdditionalMarkupPercent { get; set; }

	[JsonProperty("qmqAddSecondTaxAmountBase", Order = 8)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqAddSecondTaxAmountBase { get; set; }

	[JsonProperty("qmqAddSecondTaxAmountForeign", Order = 9)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqAddSecondTaxAmountForeign { get; set; }

	[JsonProperty("qmqAddTaxAmountBase", Order = 10)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqAddTaxAmountBase { get; set; }

	[JsonProperty("qmqAddTaxAmountForeign", Order = 11)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqAddTaxAmountForeign { get; set; }

	[JsonProperty("qmqCalculatedUnitPrice", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqCalculatedUnitPrice { get; set; }

	[JsonProperty("qmqCommissionPercent", Order = 13)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqCommissionPercent { get; set; }

	[JsonProperty("qmqCreatedBy", Order = 14)]
	[MaxLength(20)]
	public string qmqCreatedBy { get; set; }

	[JsonProperty("qmqCreatedDate", Order = 15)]
	public DateTime? qmqCreatedDate { get; set; }

	[JsonProperty("qmqDiscountPercent", Order = 16)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqDiscountPercent { get; set; }

	[JsonProperty("qmqDueDate", Order = 17)]
	public DateTime? qmqDueDate { get; set; }

	[JsonProperty("qmqUniqueID", Order = 18)]
	public Guid qmqUniqueID { get; set; }

	[JsonProperty("qmqFullRevisedUnitPriceBase", Order = 19)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqFullRevisedUnitPriceBase { get; set; }

	[JsonProperty("qmqFullRevisedUnitPriceForeign", Order = 20)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqFullRevisedUnitPriceForeign { get; set; }

	[JsonProperty("qmqClosed", Order = 21)]
	public bool qmqClosed { get; set; }

	[JsonProperty("qmqCreatedFromMobile", Order = 22)]
	public bool qmqCreatedFromMobile { get; set; }

	[JsonProperty("qmqPurchaseToOrder", Order = 23)]
	public bool qmqPurchaseToOrder { get; set; }

	[JsonProperty("qmqLaborCost", Order = 24)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqLaborCost { get; set; }

	[JsonProperty("qmqLaborMarkupPercent", Order = 25)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqLaborMarkupPercent { get; set; }

	[JsonProperty("qmqLaborPrice", Order = 26)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqLaborPrice { get; set; }

	[JsonProperty("qmqLeadTime", Order = 27)]
	[MaxLength(15)]
	public string qmqLeadTime { get; set; }

	[JsonProperty("qmqMaterialCost", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqMaterialCost { get; set; }

	[JsonProperty("qmqMaterialMarkupPercent", Order = 29)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqMaterialMarkupPercent { get; set; }

	[JsonProperty("qmqMaterialPrice", Order = 30)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqMaterialPrice { get; set; }

	[JsonProperty("qmqOverheadCost", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqOverheadCost { get; set; }

	[JsonProperty("qmqOverheadMarkupPercent", Order = 32)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqOverheadMarkupPercent { get; set; }

	[JsonProperty("qmqOverheadPrice", Order = 33)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqOverheadPrice { get; set; }

	[JsonProperty("qmqProductionHours", Order = 34)]
	[Range(0.0, 99999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqProductionHours { get; set; }

	[JsonProperty("qmqPurchaseToOrderCost", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqPurchaseToOrderCost { get; set; }

	[JsonProperty("qmqPurchaseToOrderPrice", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqPurchaseToOrderPrice { get; set; }

	[JsonProperty("qmqPurchaseUnitCostBase", Order = 37)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqPurchaseUnitCostBase { get; set; }

	[JsonProperty("qmqPurToOrderMarkupPercent", Order = 38)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqPurToOrderMarkupPercent { get; set; }

	[JsonProperty("qmqQuoteID", Order = 39)]
	[Required(ErrorMessage = "qmqQuoteID is required.")]
	[MaxLength(10)]
	public string qmqQuoteID { get; set; }

	[JsonProperty("qmqQuoteLineID", Order = 40)]
	[Required(ErrorMessage = "qmqQuoteLineID is required.")]
	public short qmqQuoteLineID { get; set; }

	[JsonProperty("qmqQuoteMarkupType", Order = 41)]
	[Required(ErrorMessage = "qmqQuoteMarkupType is required.")]
	public byte qmqQuoteMarkupType { get; set; }

	[JsonProperty("qmqQuoteQuantity", Order = 42)]
	[Required(ErrorMessage = "qmqQuoteQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqQuoteQuantity { get; set; }

	[JsonProperty("qmqQuotingCost", Order = 43)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqQuotingCost { get; set; }

	[JsonProperty("qmqQuotingMarkupPercent", Order = 44)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqQuotingMarkupPercent { get; set; }

	[JsonProperty("qmqQuotingPrice", Order = 45)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqQuotingPrice { get; set; }

	[JsonProperty("qmqRevisedUnitPriceBase", Order = 46)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqRevisedUnitPriceBase { get; set; }

	[JsonProperty("qmqRevisedUnitPriceForeign", Order = 47)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqRevisedUnitPriceForeign { get; set; }

	[JsonProperty("qmqRowVersion", Order = 48)]
	public byte[] qmqRowVersion { get; set; }

	[JsonProperty("qmqScrapPercent", Order = 49)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqScrapPercent { get; set; }

	[JsonProperty("qmqSecondTaxCodeID", Order = 50)]
	[MaxLength(5)]
	public string qmqSecondTaxCodeID { get; set; }

	[JsonProperty("qmqQuoteQuantityID", Order = 51)]
	[Required(ErrorMessage = "qmqQuoteQuantityID is required.")]
	public byte qmqQuoteQuantityID { get; set; }

	[JsonProperty("qmqSetupHours", Order = 52)]
	[Range(0.0, 99999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqSetupHours { get; set; }

	[JsonProperty("qmqStartDate", Order = 53)]
	public DateTime? qmqStartDate { get; set; }

	[JsonProperty("qmqSubcontractCost", Order = 54)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqSubcontractCost { get; set; }

	[JsonProperty("qmqSubcontractMarkupPercent", Order = 55)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqSubcontractMarkupPercent { get; set; }

	[JsonProperty("qmqSubcontractPrice", Order = 56)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqSubcontractPrice { get; set; }

	[JsonProperty("qmqTaxCodeID", Order = 57)]
	[MaxLength(5)]
	public string qmqTaxCodeID { get; set; }

	[JsonProperty("qmqTaxDate", Order = 58)]
	public DateTime? qmqTaxDate { get; set; }

	[JsonProperty("qmqTotalCost", Order = 59)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqTotalCost { get; set; }

	[JsonProperty("qmqTotalMarkupPercent", Order = 60)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqTotalMarkupPercent { get; set; }

	[JsonProperty("qmqTotalPrice", Order = 61)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqTotalPrice { get; set; }

	[JsonProperty("qmqTotalRunQuantity", Order = 62)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqTotalRunQuantity { get; set; }

	[JsonProperty("qmqTotalUnitCost", Order = 63)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqTotalUnitCost { get; set; }

	[JsonProperty("qmqTotalUnitPrice", Order = 64)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqTotalUnitPrice { get; set; }

	[JsonProperty("qmqUnitDiscountBase", Order = 65)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqUnitDiscountBase { get; set; }

	[JsonProperty("qmqUnitDiscountForeign", Order = 66)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqUnitDiscountForeign { get; set; }

	[JsonProperty("qmqUnitSecondTaxAmountBase", Order = 67)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqUnitSecondTaxAmountBase { get; set; }

	[JsonProperty("qmqUnitSecondTaxAmountForeign", Order = 68)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqUnitSecondTaxAmountForeign { get; set; }

	[JsonProperty("qmqUnitTaxAmountBase", Order = 69)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqUnitTaxAmountBase { get; set; }

	[JsonProperty("qmqUnitTaxAmountForeign", Order = 70)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmqUnitTaxAmountForeign { get; set; }

	[JsonProperty("customFields", Order = 71)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
