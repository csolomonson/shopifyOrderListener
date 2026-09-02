using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShippingMethodDto
{
	[JsonProperty("xasAvalaraTaxCodeID", Order = 1)]
	[MaxLength(10)]
	public string xasAvalaraTaxCodeID { get; set; }

	[JsonProperty("xasCarrier", Order = 2)]
	[MaxLength(5)]
	public string xasCarrier { get; set; }

	[JsonProperty("xasCarrierAccountNumber", Order = 3)]
	[MaxLength(20)]
	public string xasCarrierAccountNumber { get; set; }

	[JsonProperty("xasShippingMethodID", Order = 4)]
	[Required(ErrorMessage = "xasShippingMethodID is required.")]
	[MaxLength(5)]
	public string xasShippingMethodID { get; set; }

	[JsonProperty("xasCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string xasCreatedBy { get; set; }

	[JsonProperty("xasCreatedDate", Order = 6)]
	public DateTime? xasCreatedDate { get; set; }

	[JsonProperty("xasDescription", Order = 7)]
	[Required(ErrorMessage = "xasDescription is required.")]
	[MaxLength(50)]
	public string xasDescription { get; set; }

	[JsonProperty("xasDistributeCostsOption", Order = 8)]
	public byte xasDistributeCostsOption { get; set; }

	[JsonProperty("xasUniqueID", Order = 9)]
	public Guid xasUniqueID { get; set; }

	[JsonProperty("xasFdxAccessibility", Order = 10)]
	[MaxLength(12)]
	public string xasFdxAccessibility { get; set; }

	[JsonProperty("xasFdxCodCollectionType", Order = 11)]
	[MaxLength(16)]
	public string xasFdxCodCollectionType { get; set; }

	[JsonProperty("xasFdxDropOffType", Order = 12)]
	[MaxLength(30)]
	public string xasFdxDropOffType { get; set; }

	[JsonProperty("xasFdxHomeDeliveryType", Order = 13)]
	[MaxLength(11)]
	public string xasFdxHomeDeliveryType { get; set; }

	[JsonProperty("xasFdxPackageType", Order = 14)]
	[MaxLength(20)]
	public string xasFdxPackageType { get; set; }

	[JsonProperty("xasFdxRateElementBasis", Order = 15)]
	[MaxLength(30)]
	public string xasFdxRateElementBasis { get; set; }

	[JsonProperty("xasFdxRateRequestType", Order = 16)]
	[MaxLength(7)]
	public string xasFdxRateRequestType { get; set; }

	[JsonProperty("xasFdxRateTypeBasis", Order = 17)]
	[MaxLength(10)]
	public string xasFdxRateTypeBasis { get; set; }

	[JsonProperty("xasFdxReturnShipIndicator", Order = 18)]
	[MaxLength(30)]
	public string xasFdxReturnShipIndicator { get; set; }

	[JsonProperty("xasFdxService", Order = 19)]
	[MaxLength(30)]
	public string xasFdxService { get; set; }

	[JsonProperty("xasFdxSignatureOption", Order = 20)]
	[MaxLength(30)]
	public string xasFdxSignatureOption { get; set; }

	[JsonProperty("xasFdxVHCAmountOrPercentage", Order = 21)]
	[Range(0.0, 99999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xasFdxVHCAmountOrPercentage { get; set; }

	[JsonProperty("xasFdxVHCLevel", Order = 22)]
	[MaxLength(8)]
	public string xasFdxVHCLevel { get; set; }

	[JsonProperty("xasFdxVHCType", Order = 23)]
	[MaxLength(40)]
	public string xasFdxVHCType { get; set; }

	[JsonProperty("xasFedExBillingOption", Order = 24)]
	[MaxLength(20)]
	public string xasFedExBillingOption { get; set; }

	[JsonProperty("xasInactiveDate", Order = 25)]
	public DateTime? xasInactiveDate { get; set; }

	[JsonProperty("xasInactive", Order = 26)]
	public bool xasInactive { get; set; }

	[JsonProperty("xasFdxCertificateOfOrigin", Order = 27)]
	public bool xasFdxCertificateOfOrigin { get; set; }

	[JsonProperty("xasFdxCod", Order = 28)]
	public bool xasFdxCod { get; set; }

	[JsonProperty("xasFdxCommercialInvoice", Order = 29)]
	public bool xasFdxCommercialInvoice { get; set; }

	[JsonProperty("xasFdxExportDeclaration", Order = 30)]
	public bool xasFdxExportDeclaration { get; set; }

	[JsonProperty("xasFdxHoldAtLocation", Order = 31)]
	public bool xasFdxHoldAtLocation { get; set; }

	[JsonProperty("xasFdxInsideDelivery", Order = 32)]
	public bool xasFdxInsideDelivery { get; set; }

	[JsonProperty("xasFdxInsidePickup", Order = 33)]
	public bool xasFdxInsidePickup { get; set; }

	[JsonProperty("xasFdxNAFTACO", Order = 34)]
	public bool xasFdxNAFTACO { get; set; }

	[JsonProperty("xasFdxNonStandardContainer", Order = 35)]
	public bool xasFdxNonStandardContainer { get; set; }

	[JsonProperty("xasFdxReturnInstructions", Order = 36)]
	public bool xasFdxReturnInstructions { get; set; }

	[JsonProperty("xasFdxSaturdayDelivery", Order = 37)]
	public bool xasFdxSaturdayDelivery { get; set; }

	[JsonProperty("xasFdxSaturdayPickup", Order = 38)]
	public bool xasFdxSaturdayPickup { get; set; }

	[JsonProperty("xasUpsCertificateOfOrigin", Order = 39)]
	public bool xasUpsCertificateOfOrigin { get; set; }

	[JsonProperty("xasUpsCod", Order = 40)]
	public bool xasUpsCod { get; set; }

	[JsonProperty("xasUpsCommercialInvoice", Order = 41)]
	public bool xasUpsCommercialInvoice { get; set; }

	[JsonProperty("xasUpsNAFTACO", Order = 42)]
	public bool xasUpsNAFTACO { get; set; }

	[JsonProperty("xasUpsPackingList", Order = 43)]
	public bool xasUpsPackingList { get; set; }

	[JsonProperty("xasUpsPartialInvoice", Order = 44)]
	public bool xasUpsPartialInvoice { get; set; }

	[JsonProperty("xasUpsSaturdayDelivery", Order = 45)]
	public bool xasUpsSaturdayDelivery { get; set; }

	[JsonProperty("xasUpsUseInterface", Order = 46)]
	public bool xasUpsUseInterface { get; set; }

	[JsonProperty("xasReferenceTrackingLink", Order = 47)]
	[MaxLength(50)]
	public string xasReferenceTrackingLink { get; set; }

	[JsonProperty("xasRowVersion", Order = 48)]
	public byte[] xasRowVersion { get; set; }

	[JsonProperty("xasSecondTaxCodeID", Order = 49)]
	[MaxLength(5)]
	public string xasSecondTaxCodeID { get; set; }

	[JsonProperty("xasShipChargeWeb", Order = 50)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xasShipChargeWeb { get; set; }

	[JsonProperty("xasShippingPaymentTypeID", Order = 51)]
	[MaxLength(5)]
	public string xasShippingPaymentTypeID { get; set; }

	[JsonProperty("xasTaxCodeID", Order = 52)]
	[MaxLength(5)]
	public string xasTaxCodeID { get; set; }

	[JsonProperty("xasTaxStatus", Order = 53)]
	public byte xasTaxStatus { get; set; }

	[JsonProperty("xasTrackingLink", Order = 54)]
	[MaxLength(50)]
	public string xasTrackingLink { get; set; }

	[JsonProperty("xasUpsBillingOptionDefault", Order = 55)]
	[MaxLength(20)]
	public string xasUpsBillingOptionDefault { get; set; }

	[JsonProperty("xasUpsCodFundsCode", Order = 56)]
	[MaxLength(1)]
	public string xasUpsCodFundsCode { get; set; }

	[JsonProperty("xasUpsCostCenter", Order = 57)]
	[MaxLength(30)]
	public string xasUpsCostCenter { get; set; }

	[JsonProperty("xasUpsPackageType", Order = 58)]
	[MaxLength(35)]
	public string xasUpsPackageType { get; set; }

	[JsonProperty("xasUpsServiceType", Order = 59)]
	[MaxLength(22)]
	public string xasUpsServiceType { get; set; }

	[JsonProperty("xasUpsWsBillingOption", Order = 60)]
	[MaxLength(20)]
	public string xasUpsWsBillingOption { get; set; }

	[JsonProperty("xasUpsWSPackageType", Order = 61)]
	[MaxLength(35)]
	public string xasUpsWSPackageType { get; set; }

	[JsonProperty("xasUpsWSServiceType", Order = 62)]
	[MaxLength(22)]
	public string xasUpsWSServiceType { get; set; }

	[JsonProperty("xasUSPSEndorsement", Order = 63)]
	[MaxLength(1)]
	public string xasUSPSEndorsement { get; set; }

	[JsonProperty("customFields", Order = 64)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
