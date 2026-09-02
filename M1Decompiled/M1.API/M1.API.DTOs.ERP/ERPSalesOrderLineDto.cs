using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSalesOrderLineDto
{
	[JsonProperty("omlCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string omlCreatedBy { get; set; }

	[JsonProperty("omlCreatedDate", Order = 2)]
	public DateTime? omlCreatedDate { get; set; }

	[JsonProperty("omlDeliveryQuantityTotal", Order = 3)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlDeliveryQuantityTotal { get; set; }

	[JsonProperty("omlDepositAmountBase", Order = 4)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlDepositAmountBase { get; set; }

	[JsonProperty("omlDepositAmountForeign", Order = 5)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlDepositAmountForeign { get; set; }

	[JsonProperty("omlDepositPercent", Order = 6)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlDepositPercent { get; set; }

	[JsonProperty("omlDiscountPercent", Order = 7)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlDiscountPercent { get; set; }

	[JsonProperty("omlDocuments", Order = 8)]
	[MaxLength(50)]
	public string omlDocuments { get; set; }

	[JsonProperty("omlUniqueID", Order = 9)]
	public Guid omlUniqueID { get; set; }

	[JsonProperty("omlExtendedDiscountBase", Order = 10)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlExtendedDiscountBase { get; set; }

	[JsonProperty("omlExtendedDiscountForeign", Order = 11)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlExtendedDiscountForeign { get; set; }

	[JsonProperty("omlExtendedPriceBase", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlExtendedPriceBase { get; set; }

	[JsonProperty("omlExtendedPriceForeign", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlExtendedPriceForeign { get; set; }

	[JsonProperty("omlExtendedWeight", Order = 14)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlExtendedWeight { get; set; }

	[JsonProperty("omlFreightAmountBase", Order = 15)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlFreightAmountBase { get; set; }

	[JsonProperty("omlFreightAmountForeign", Order = 16)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlFreightAmountForeign { get; set; }

	[JsonProperty("omlFullExtendedPriceBase", Order = 17)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlFullExtendedPriceBase { get; set; }

	[JsonProperty("omlFullExtendedPriceForeign", Order = 18)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlFullExtendedPriceForeign { get; set; }

	[JsonProperty("omlFullUnitPriceBase", Order = 19)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlFullUnitPriceBase { get; set; }

	[JsonProperty("omlFullUnitPriceForeign", Order = 20)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlFullUnitPriceForeign { get; set; }

	[JsonProperty("omlAvalaraIgnoreLine", Order = 21)]
	public bool omlAvalaraIgnoreLine { get; set; }

	[JsonProperty("omlClosed", Order = 22)]
	public bool omlClosed { get; set; }

	[JsonProperty("omlConfigured", Order = 23)]
	public bool omlConfigured { get; set; }

	[JsonProperty("omlDeposit", Order = 24)]
	public bool omlDeposit { get; set; }

	[JsonProperty("omlDepositCreated", Order = 25)]
	public bool omlDepositCreated { get; set; }

	[JsonProperty("omlDepositCredited", Order = 26)]
	public bool omlDepositCredited { get; set; }

	[JsonProperty("omlPayCommission", Order = 27)]
	public bool omlPayCommission { get; set; }

	[JsonProperty("omlPriceOverride", Order = 28)]
	public bool omlPriceOverride { get; set; }

	[JsonProperty("omlTimeAndMaterial", Order = 29)]
	public bool omlTimeAndMaterial { get; set; }

	[JsonProperty("omlLeadID", Order = 30)]
	[MaxLength(10)]
	public string omlLeadID { get; set; }

	[JsonProperty("omlLeadLineID", Order = 31)]
	public short omlLeadLineID { get; set; }

	[JsonProperty("omlNonTaxReasonID", Order = 32)]
	[MaxLength(5)]
	public string omlNonTaxReasonID { get; set; }

	[JsonProperty("omlOrderQuantity", Order = 33)]
	[Required(ErrorMessage = "omlOrderQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlOrderQuantity { get; set; }

	[JsonProperty("omlOrgPartID", Order = 34)]
	[MaxLength(30)]
	public string omlOrgPartID { get; set; }

	[JsonProperty("omlOrgPartShortDescription", Order = 35)]
	[MaxLength(50)]
	public string omlOrgPartShortDescription { get; set; }

	[JsonProperty("omlPartGroupID", Order = 36)]
	[MaxLength(5)]
	public string omlPartGroupID { get; set; }

	[JsonProperty("omlPartID", Order = 37)]
	[Required(ErrorMessage = "omlPartID is required.")]
	[MaxLength(30)]
	public string omlPartID { get; set; }

	[JsonProperty("omlPartLongDescriptionRtf", Order = 38)]
	public string omlPartLongDescriptionRtf { get; set; }

	[JsonProperty("omlPartLongDescriptionText", Order = 39)]
	public string omlPartLongDescriptionText { get; set; }

	[JsonProperty("omlPartRevisionID", Order = 40)]
	[MaxLength(15)]
	public string omlPartRevisionID { get; set; }

	[JsonProperty("omlPartShortDescription", Order = 41)]
	[Required(ErrorMessage = "omlPartShortDescription is required.")]
	[MaxLength(50)]
	public string omlPartShortDescription { get; set; }

	[JsonProperty("omlProjectAreaID", Order = 42)]
	[MaxLength(15)]
	public string omlProjectAreaID { get; set; }

	[JsonProperty("omlProjectID", Order = 43)]
	[MaxLength(10)]
	public string omlProjectID { get; set; }

	[JsonProperty("omlQuantityShipped", Order = 44)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlQuantityShipped { get; set; }

	[JsonProperty("omlQuoteID", Order = 45)]
	[MaxLength(10)]
	public string omlQuoteID { get; set; }

	[JsonProperty("omlQuoteLineID", Order = 46)]
	public short omlQuoteLineID { get; set; }

	[JsonProperty("omlQuoteQuantityID", Order = 47)]
	public byte omlQuoteQuantityID { get; set; }

	[JsonProperty("omlReleaseNumber", Order = 48)]
	[MaxLength(20)]
	public string omlReleaseNumber { get; set; }

	[JsonProperty("omlRmaClaimID", Order = 49)]
	[MaxLength(10)]
	public string omlRmaClaimID { get; set; }

	[JsonProperty("omlRmaClaimLineID", Order = 50)]
	public short omlRmaClaimLineID { get; set; }

	[JsonProperty("omlRowVersion", Order = 51)]
	public byte[] omlRowVersion { get; set; }

	[JsonProperty("omlSalesOrderID", Order = 52)]
	[Required(ErrorMessage = "omlSalesOrderID is required.")]
	[MaxLength(10)]
	public string omlSalesOrderID { get; set; }

	[JsonProperty("omlSecondTaxAmountBase", Order = 53)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlSecondTaxAmountBase { get; set; }

	[JsonProperty("omlSecondTaxAmountForeign", Order = 54)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlSecondTaxAmountForeign { get; set; }

	[JsonProperty("omlSecondTaxCodeID", Order = 55)]
	[MaxLength(5)]
	public string omlSecondTaxCodeID { get; set; }

	[JsonProperty("omlSalesOrderLineID", Order = 56)]
	[Required(ErrorMessage = "omlSalesOrderLineID is required.")]
	public short omlSalesOrderLineID { get; set; }

	[JsonProperty("omlTaxAmountBase", Order = 57)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlTaxAmountBase { get; set; }

	[JsonProperty("omlTaxAmountForeign", Order = 58)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlTaxAmountForeign { get; set; }

	[JsonProperty("omlTaxCodeID", Order = 59)]
	[MaxLength(5)]
	public string omlTaxCodeID { get; set; }

	[JsonProperty("omlUnitDiscountBase", Order = 60)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlUnitDiscountBase { get; set; }

	[JsonProperty("omlUnitDiscountForeign", Order = 61)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlUnitDiscountForeign { get; set; }

	[JsonProperty("omlUnitOfMeasure", Order = 62)]
	[MaxLength(2)]
	public string omlUnitOfMeasure { get; set; }

	[JsonProperty("omlUnitPriceBase", Order = 63)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlUnitPriceBase { get; set; }

	[JsonProperty("omlUnitPriceForeign", Order = 64)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlUnitPriceForeign { get; set; }

	[JsonProperty("omlWeight", Order = 65)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omlWeight { get; set; }

	[JsonProperty("customFields", Order = 66)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
