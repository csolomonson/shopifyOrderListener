using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPFreightShipmentDto
{
	[JsonProperty("fspCarrier", Order = 1)]
	[MaxLength(5)]
	public string fspCarrier { get; set; }

	[JsonProperty("fspFreightShipmentID", Order = 2)]
	[Required(ErrorMessage = "fspFreightShipmentID is required.")]
	[MaxLength(10)]
	public string fspFreightShipmentID { get; set; }

	[JsonProperty("fspCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string fspCreatedBy { get; set; }

	[JsonProperty("fspCreatedDate", Order = 4)]
	public DateTime? fspCreatedDate { get; set; }

	[JsonProperty("fspDeclaredValue", Order = 5)]
	[Range(0.0, 99999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fspDeclaredValue { get; set; }

	[JsonProperty("fspDistributeCostsOption", Order = 6)]
	public byte fspDistributeCostsOption { get; set; }

	[JsonProperty("fspUniqueID", Order = 7)]
	public Guid fspUniqueID { get; set; }

	[JsonProperty("fspFdxAccessibility", Order = 8)]
	[MaxLength(12)]
	public string fspFdxAccessibility { get; set; }

	[JsonProperty("fspFdxCodCollectionAmount", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fspFdxCodCollectionAmount { get; set; }

	[JsonProperty("fspFdxCodCollectionType", Order = 10)]
	[MaxLength(16)]
	public string fspFdxCodCollectionType { get; set; }

	[JsonProperty("fspFdxDropOffType", Order = 11)]
	[MaxLength(30)]
	public string fspFdxDropOffType { get; set; }

	[JsonProperty("fspFdxHandlingCost", Order = 12)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fspFdxHandlingCost { get; set; }

	[JsonProperty("fspFdxHomeDeliveryType", Order = 13)]
	[MaxLength(11)]
	public string fspFdxHomeDeliveryType { get; set; }

	[JsonProperty("fspFdxLastLogID", Order = 14)]
	public int fspFdxLastLogID { get; set; }

	[JsonProperty("fspFdxLastReplyErrorCode", Order = 15)]
	[MaxLength(8)]
	public string fspFdxLastReplyErrorCode { get; set; }

	[JsonProperty("fspFdxLastReplyErrorMessage", Order = 16)]
	[MaxLength(120)]
	public string fspFdxLastReplyErrorMessage { get; set; }

	[JsonProperty("fspFdxLastReplySoftErrorCode", Order = 17)]
	[MaxLength(8)]
	public string fspFdxLastReplySoftErrorCode { get; set; }

	[JsonProperty("fspFdxLastReplySoftErrorMsg", Order = 18)]
	[MaxLength(50)]
	public string fspFdxLastReplySoftErrorMsg { get; set; }

	[JsonProperty("fspFdxLastReplySoftErrorType", Order = 19)]
	[MaxLength(25)]
	public string fspFdxLastReplySoftErrorType { get; set; }

	[JsonProperty("fspFdxLastRequestDate", Order = 20)]
	public DateTime? fspFdxLastRequestDate { get; set; }

	[JsonProperty("fspFdxLastUTI", Order = 21)]
	[MaxLength(4)]
	public string fspFdxLastUTI { get; set; }

	[JsonProperty("fspFdxPackagingCost", Order = 22)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fspFdxPackagingCost { get; set; }

	[JsonProperty("fspFdxPayorAccountNumber", Order = 23)]
	[MaxLength(12)]
	public string fspFdxPayorAccountNumber { get; set; }

	[JsonProperty("fspFdxPayorCountryCode", Order = 24)]
	[MaxLength(2)]
	public string fspFdxPayorCountryCode { get; set; }

	[JsonProperty("fspFdxPayorType", Order = 25)]
	[MaxLength(10)]
	public string fspFdxPayorType { get; set; }

	[JsonProperty("fspFdxRateRequestType", Order = 26)]
	[MaxLength(7)]
	public string fspFdxRateRequestType { get; set; }

	[JsonProperty("fspFdxReturnShipIndicator", Order = 27)]
	[MaxLength(30)]
	public string fspFdxReturnShipIndicator { get; set; }

	[JsonProperty("fspFdxService", Order = 28)]
	[MaxLength(30)]
	public string fspFdxService { get; set; }

	[JsonProperty("fspFdxShipCostMarkupPct", Order = 29)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fspFdxShipCostMarkupPct { get; set; }

	[JsonProperty("fspFdxSignatureOption", Order = 30)]
	[MaxLength(30)]
	public string fspFdxSignatureOption { get; set; }

	[JsonProperty("fspFdxSignatureReleaseAuthNum", Order = 31)]
	[MaxLength(10)]
	public string fspFdxSignatureReleaseAuthNum { get; set; }

	[JsonProperty("fspFdxStatus", Order = 32)]
	public byte fspFdxStatus { get; set; }

	[JsonProperty("fspFdxStatusText", Order = 33)]
	[MaxLength(50)]
	public string fspFdxStatusText { get; set; }

	[JsonProperty("fspFdxVHCAmountOrPercentage", Order = 34)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fspFdxVHCAmountOrPercentage { get; set; }

	[JsonProperty("fspFdxVHCLevel", Order = 35)]
	[MaxLength(8)]
	public string fspFdxVHCLevel { get; set; }

	[JsonProperty("fspFdxVHCType", Order = 36)]
	[MaxLength(40)]
	public string fspFdxVHCType { get; set; }

	[JsonProperty("fspFreightShipmentDate", Order = 37)]
	public DateTime? fspFreightShipmentDate { get; set; }

	[JsonProperty("fspFdxCod", Order = 38)]
	public bool fspFdxCod { get; set; }

	[JsonProperty("fspFdxHoldAtLocation", Order = 39)]
	public bool fspFdxHoldAtLocation { get; set; }

	[JsonProperty("fspFdxInsideDelivery", Order = 40)]
	public bool fspFdxInsideDelivery { get; set; }

	[JsonProperty("fspFdxInsidePickup", Order = 41)]
	public bool fspFdxInsidePickup { get; set; }

	[JsonProperty("fspFdxOneItemPerShipment", Order = 42)]
	public bool fspFdxOneItemPerShipment { get; set; }

	[JsonProperty("fspFdxSaturdayDelivery", Order = 43)]
	public bool fspFdxSaturdayDelivery { get; set; }

	[JsonProperty("fspFdxSaturdayPickup", Order = 44)]
	public bool fspFdxSaturdayPickup { get; set; }

	[JsonProperty("fspUpsSaturdayDelivery", Order = 45)]
	public bool fspUpsSaturdayDelivery { get; set; }

	[JsonProperty("fspVoidOnUps", Order = 46)]
	public bool fspVoidOnUps { get; set; }

	[JsonProperty("fspNotesRTF", Order = 47)]
	[MaxLength(50)]
	public string fspNotesRTF { get; set; }

	[JsonProperty("fspNotesText", Order = 48)]
	[MaxLength(50)]
	public string fspNotesText { get; set; }

	[JsonProperty("fspRowVersion", Order = 49)]
	public byte[] fspRowVersion { get; set; }

	[JsonProperty("fspShipFromOrganizationID", Order = 50)]
	[Required(ErrorMessage = "fspShipFromOrganizationID is required.")]
	[MaxLength(10)]
	public string fspShipFromOrganizationID { get; set; }

	[JsonProperty("fspShipLocationID", Order = 51)]
	[MaxLength(5)]
	public string fspShipLocationID { get; set; }

	[JsonProperty("fspShipOrganizationID", Order = 52)]
	[Required(ErrorMessage = "fspShipOrganizationID is required.")]
	[MaxLength(10)]
	public string fspShipOrganizationID { get; set; }

	[JsonProperty("fspShipperAcctNumber", Order = 53)]
	[MaxLength(20)]
	public string fspShipperAcctNumber { get; set; }

	[JsonProperty("fspShippingMethodID", Order = 54)]
	[MaxLength(5)]
	public string fspShippingMethodID { get; set; }

	[JsonProperty("fspTotalCharges", Order = 55)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fspTotalCharges { get; set; }

	[JsonProperty("fspTotalPublishedCharges", Order = 56)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fspTotalPublishedCharges { get; set; }

	[JsonProperty("fspUps3rdPartyLocationID", Order = 57)]
	[MaxLength(5)]
	public string fspUps3rdPartyLocationID { get; set; }

	[JsonProperty("fspUps3rdPartyOrganizationID", Order = 58)]
	[MaxLength(10)]
	public string fspUps3rdPartyOrganizationID { get; set; }

	[JsonProperty("fspUpsBillAcctNumber", Order = 59)]
	[MaxLength(6)]
	public string fspUpsBillAcctNumber { get; set; }

	[JsonProperty("fspUpsBillingOption", Order = 60)]
	[MaxLength(20)]
	public string fspUpsBillingOption { get; set; }

	[JsonProperty("fspUpsInterfaceStatus", Order = 61)]
	public byte fspUpsInterfaceStatus { get; set; }

	[JsonProperty("fspUpsServiceType", Order = 62)]
	[MaxLength(22)]
	public string fspUpsServiceType { get; set; }

	[JsonProperty("customFields", Order = 63)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
