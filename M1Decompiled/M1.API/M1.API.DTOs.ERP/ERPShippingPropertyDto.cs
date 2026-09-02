using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShippingPropertyDto
{
	[JsonProperty("xsmCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string xsmCreatedBy { get; set; }

	[JsonProperty("xsmCreatedDate", Order = 2)]
	public DateTime? xsmCreatedDate { get; set; }

	[JsonProperty("xsmUniqueID", Order = 3)]
	public Guid xsmUniqueID { get; set; }

	[JsonProperty("xsmFdxAccessibility", Order = 4)]
	[MaxLength(12)]
	public string xsmFdxAccessibility { get; set; }

	[JsonProperty("xsmFdxAccountNumber", Order = 5)]
	[MaxLength(15)]
	public string xsmFdxAccountNumber { get; set; }

	[JsonProperty("xsmFdxAccountNumberOAuth", Order = 6)]
	[MaxLength(15)]
	public string xsmFdxAccountNumberOAuth { get; set; }

	[JsonProperty("xsmFdxAddressLine1", Order = 7)]
	[MaxLength(50)]
	public string xsmFdxAddressLine1 { get; set; }

	[JsonProperty("xsmFdxAddressLine2", Order = 8)]
	[MaxLength(50)]
	public string xsmFdxAddressLine2 { get; set; }

	[JsonProperty("xsmFdxAddrValAccuracyIndicator", Order = 9)]
	[MaxLength(10)]
	public string xsmFdxAddrValAccuracyIndicator { get; set; }

	[JsonProperty("xsmFdxCity", Order = 10)]
	[MaxLength(30)]
	public string xsmFdxCity { get; set; }

	[JsonProperty("xsmFdxClientID", Order = 11)]
	[MaxLength(35)]
	public string xsmFdxClientID { get; set; }

	[JsonProperty("xsmFdxClientIDTrack", Order = 12)]
	[MaxLength(35)]
	public string xsmFdxClientIDTrack { get; set; }

	[JsonProperty("xsmFdxClientSecret", Order = 13)]
	[MaxLength(35)]
	public string xsmFdxClientSecret { get; set; }

	[JsonProperty("xsmFdxClientSecretTrack", Order = 14)]
	[MaxLength(35)]
	public string xsmFdxClientSecretTrack { get; set; }

	[JsonProperty("xsmFdxCodCollectionAmount", Order = 15)]
	[Range(0.0, 99999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xsmFdxCodCollectionAmount { get; set; }

	[JsonProperty("xsmFdxCodCollectionType", Order = 16)]
	[MaxLength(16)]
	public string xsmFdxCodCollectionType { get; set; }

	[JsonProperty("xsmFdxCountry", Order = 17)]
	[MaxLength(2)]
	public string xsmFdxCountry { get; set; }

	[JsonProperty("xsmFdxCurrencyType", Order = 18)]
	[MaxLength(3)]
	public string xsmFdxCurrencyType { get; set; }

	[JsonProperty("xsmFdxDeclaredValueCurrency", Order = 19)]
	[MaxLength(3)]
	public string xsmFdxDeclaredValueCurrency { get; set; }

	[JsonProperty("xsmFdxDepartment", Order = 20)]
	[MaxLength(30)]
	public string xsmFdxDepartment { get; set; }

	[JsonProperty("xsmFdxDimensionsUnitOfMeasure", Order = 21)]
	[MaxLength(3)]
	public string xsmFdxDimensionsUnitOfMeasure { get; set; }

	[JsonProperty("xsmFdxDropOffType", Order = 22)]
	[MaxLength(30)]
	public string xsmFdxDropOffType { get; set; }

	[JsonProperty("xsmFdxEmailAddress", Order = 23)]
	[MaxLength(50)]
	public string xsmFdxEmailAddress { get; set; }

	[JsonProperty("xsmFdxFaxNumber", Order = 24)]
	[MaxLength(20)]
	public string xsmFdxFaxNumber { get; set; }

	[JsonProperty("xsmFdxHandlingCost", Order = 25)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xsmFdxHandlingCost { get; set; }

	[JsonProperty("xsmFdxHomeDeliveryDate", Order = 26)]
	public DateTime? xsmFdxHomeDeliveryDate { get; set; }

	[JsonProperty("xsmFdxHomeDeliveryType", Order = 27)]
	[MaxLength(12)]
	public string xsmFdxHomeDeliveryType { get; set; }

	[JsonProperty("xsmFdxHostAddress", Order = 28)]
	[MaxLength(120)]
	public string xsmFdxHostAddress { get; set; }

	[JsonProperty("xsmFdxHostPort", Order = 29)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xsmFdxHostPort { get; set; }

	[JsonProperty("xsmFdxHostService", Order = 30)]
	[MaxLength(120)]
	public string xsmFdxHostService { get; set; }

	[JsonProperty("xsmFdxLabelFormatType", Order = 31)]
	[MaxLength(50)]
	public string xsmFdxLabelFormatType { get; set; }

	[JsonProperty("xsmFdxLabelImageType", Order = 32)]
	[MaxLength(10)]
	public string xsmFdxLabelImageType { get; set; }

	[JsonProperty("xsmFdxLabelStockType", Order = 33)]
	[MaxLength(35)]
	public string xsmFdxLabelStockType { get; set; }

	[JsonProperty("xsmFdxLabelStoreLocation", Order = 34)]
	[MaxLength(250)]
	public string xsmFdxLabelStoreLocation { get; set; }

	[JsonProperty("xsmFdxLabelType", Order = 35)]
	[MaxLength(10)]
	public string xsmFdxLabelType { get; set; }

	[JsonProperty("xsmFdxLblPrintOrientType", Order = 36)]
	[MaxLength(30)]
	public string xsmFdxLblPrintOrientType { get; set; }

	[JsonProperty("xsmFdxMeterNumber", Order = 37)]
	[Range(0.0, 9999999999.0, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xsmFdxMeterNumber { get; set; }

	[JsonProperty("xsmFdxName", Order = 38)]
	[MaxLength(50)]
	public string xsmFdxName { get; set; }

	[JsonProperty("xsmFdxPackageHeight", Order = 39)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xsmFdxPackageHeight { get; set; }

	[JsonProperty("xsmFdxPackageLength", Order = 40)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xsmFdxPackageLength { get; set; }

	[JsonProperty("xsmFdxPackageWidth", Order = 41)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xsmFdxPackageWidth { get; set; }

	[JsonProperty("xsmFdxPackaging", Order = 42)]
	[MaxLength(14)]
	public string xsmFdxPackaging { get; set; }

	[JsonProperty("xsmFdxPackagingCost", Order = 43)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xsmFdxPackagingCost { get; set; }

	[JsonProperty("xsmFdxPagerNumber", Order = 44)]
	[MaxLength(20)]
	public string xsmFdxPagerNumber { get; set; }

	[JsonProperty("xsmFdxPayorType", Order = 45)]
	[MaxLength(10)]
	public string xsmFdxPayorType { get; set; }

	[JsonProperty("xsmFdxPersonName", Order = 46)]
	[MaxLength(50)]
	public string xsmFdxPersonName { get; set; }

	[JsonProperty("xsmFdxPhoneNumber", Order = 47)]
	[MaxLength(20)]
	public string xsmFdxPhoneNumber { get; set; }

	[JsonProperty("xsmFdxPostCode", Order = 48)]
	[MaxLength(10)]
	public string xsmFdxPostCode { get; set; }

	[JsonProperty("xsmFdxRateElementBasis", Order = 49)]
	[MaxLength(30)]
	public string xsmFdxRateElementBasis { get; set; }

	[JsonProperty("xsmFdxRateRequestType", Order = 50)]
	[MaxLength(7)]
	public string xsmFdxRateRequestType { get; set; }

	[JsonProperty("xsmFdxRateTypeBasis", Order = 51)]
	[MaxLength(10)]
	public string xsmFdxRateTypeBasis { get; set; }

	[JsonProperty("xsmFdxReturnShipIndicator", Order = 52)]
	[MaxLength(30)]
	public string xsmFdxReturnShipIndicator { get; set; }

	[JsonProperty("xsmFdxShipCostMarkupPct", Order = 53)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xsmFdxShipCostMarkupPct { get; set; }

	[JsonProperty("xsmFdxShipDocImageType", Order = 54)]
	[MaxLength(10)]
	public string xsmFdxShipDocImageType { get; set; }

	[JsonProperty("xsmFdxSignatureOption", Order = 55)]
	[MaxLength(30)]
	public string xsmFdxSignatureOption { get; set; }

	[JsonProperty("xsmFdxState", Order = 56)]
	[MaxLength(3)]
	public string xsmFdxState { get; set; }

	[JsonProperty("xsmFdxSubscribedServices", Order = 57)]
	[MaxLength(50)]
	public string xsmFdxSubscribedServices { get; set; }

	[JsonProperty("xsmFdxVHCAmountOrPercentage", Order = 58)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xsmFdxVHCAmountOrPercentage { get; set; }

	[JsonProperty("xsmFdxVHCLevel", Order = 59)]
	[MaxLength(8)]
	public string xsmFdxVHCLevel { get; set; }

	[JsonProperty("xsmFdxVHCType", Order = 60)]
	[MaxLength(40)]
	public string xsmFdxVHCType { get; set; }

	[JsonProperty("xsmFdxWeightUnitOfMeasure", Order = 61)]
	[MaxLength(3)]
	public string xsmFdxWeightUnitOfMeasure { get; set; }

	[JsonProperty("xsmFedExAccessKey", Order = 62)]
	[MaxLength(20)]
	public string xsmFedExAccessKey { get; set; }

	[JsonProperty("xsmFedExAccessToken", Order = 63)]
	[MaxLength(50)]
	public string xsmFedExAccessToken { get; set; }

	[JsonProperty("xsmFedExAccessTokenTrack", Order = 64)]
	[MaxLength(50)]
	public string xsmFedExAccessTokenTrack { get; set; }

	[JsonProperty("xsmFedExAuthenticationMethod", Order = 65)]
	[Required(ErrorMessage = "xsmFedExAuthenticationMethod is required.")]
	[MaxLength(10)]
	public string xsmFedExAuthenticationMethod { get; set; }

	[JsonProperty("xsmFedExPassword", Order = 66)]
	[MaxLength(30)]
	public string xsmFedExPassword { get; set; }

	[JsonProperty("xsmFedExTokenExpiresIn", Order = 67)]
	public DateTime? xsmFedExTokenExpiresIn { get; set; }

	[JsonProperty("xsmFedExTokenExpiresInTrack", Order = 68)]
	public DateTime? xsmFedExTokenExpiresInTrack { get; set; }

	[JsonProperty("xsmFedExUserName", Order = 69)]
	[MaxLength(50)]
	public string xsmFedExUserName { get; set; }

	[JsonProperty("xsmFdxBareCostOfDuty", Order = 70)]
	public bool xsmFdxBareCostOfDuty { get; set; }

	[JsonProperty("xsmFdxBareTrasportationCost", Order = 71)]
	public bool xsmFdxBareTrasportationCost { get; set; }

	[JsonProperty("xsmFdxCod", Order = 72)]
	public bool xsmFdxCod { get; set; }

	[JsonProperty("xsmFdxHoldAtLocation", Order = 73)]
	public bool xsmFdxHoldAtLocation { get; set; }

	[JsonProperty("xsmFdxInsideDelivery", Order = 74)]
	public bool xsmFdxInsideDelivery { get; set; }

	[JsonProperty("xsmFdxInsidePickup", Order = 75)]
	public bool xsmFdxInsidePickup { get; set; }

	[JsonProperty("xsmFdxNonstandardContainer", Order = 76)]
	public bool xsmFdxNonstandardContainer { get; set; }

	[JsonProperty("xsmFdxOneItemPerShipment", Order = 77)]
	public bool xsmFdxOneItemPerShipment { get; set; }

	[JsonProperty("xsmFdxResidentialAddress", Order = 78)]
	public bool xsmFdxResidentialAddress { get; set; }

	[JsonProperty("xsmFdxSaturdayDelivery", Order = 79)]
	public bool xsmFdxSaturdayDelivery { get; set; }

	[JsonProperty("xsmFdxSaturdayPickup", Order = 80)]
	public bool xsmFdxSaturdayPickup { get; set; }

	[JsonProperty("xsmFedExIsProduction", Order = 81)]
	public bool xsmFedExIsProduction { get; set; }

	[JsonProperty("xsmUpsIsProduction", Order = 82)]
	public bool xsmUpsIsProduction { get; set; }

	[JsonProperty("xsmRowVersion", Order = 83)]
	public byte[] xsmRowVersion { get; set; }

	[JsonProperty("xsmUpsAccessKey", Order = 84)]
	[MaxLength(20)]
	public string xsmUpsAccessKey { get; set; }

	[JsonProperty("xsmUpsAccessToken", Order = 85)]
	[MaxLength(50)]
	public string xsmUpsAccessToken { get; set; }

	[JsonProperty("xsmUpsAccountNo", Order = 86)]
	[MaxLength(6)]
	public string xsmUpsAccountNo { get; set; }

	[JsonProperty("xsmUpsAccountNoOAuth", Order = 87)]
	[MaxLength(6)]
	public string xsmUpsAccountNoOAuth { get; set; }

	[JsonProperty("xsmUpsAuthenticationMethod", Order = 88)]
	[Required(ErrorMessage = "xsmUpsAuthenticationMethod is required.")]
	[MaxLength(10)]
	public string xsmUpsAuthenticationMethod { get; set; }

	[JsonProperty("xsmUpsLabelStockSize", Order = 89)]
	[MaxLength(10)]
	public string xsmUpsLabelStockSize { get; set; }

	[JsonProperty("xsmUpsLabelStoreLocation", Order = 90)]
	[MaxLength(250)]
	public string xsmUpsLabelStoreLocation { get; set; }

	[JsonProperty("xsmUpsLabelType", Order = 91)]
	[MaxLength(3)]
	public string xsmUpsLabelType { get; set; }

	[JsonProperty("xsmUpsLocIDPref", Order = 92)]
	[MaxLength(1)]
	public string xsmUpsLocIDPref { get; set; }

	[JsonProperty("xsmUpsLocPostCodePref", Order = 93)]
	[MaxLength(1)]
	public string xsmUpsLocPostCodePref { get; set; }

	[JsonProperty("xsmUpsPassword", Order = 94)]
	[MaxLength(20)]
	public string xsmUpsPassword { get; set; }

	[JsonProperty("xsmUpsRefreshToken", Order = 95)]
	[MaxLength(50)]
	public string xsmUpsRefreshToken { get; set; }

	[JsonProperty("xsmUpsUsername", Order = 96)]
	[MaxLength(250)]
	public string xsmUpsUsername { get; set; }

	[JsonProperty("xsmUSDcurrencyCode", Order = 97)]
	[MaxLength(5)]
	public string xsmUSDcurrencyCode { get; set; }

	[JsonProperty("customFields", Order = 98)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
