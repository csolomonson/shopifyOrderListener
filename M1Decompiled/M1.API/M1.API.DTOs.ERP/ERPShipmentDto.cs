using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShipmentDto
{
	[JsonProperty("smpAccBaseChargeBase", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpAccBaseChargeBase { get; set; }

	[JsonProperty("smpAccBaseChargeForeign", Order = 2)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpAccBaseChargeForeign { get; set; }

	[JsonProperty("smpAccCarrierFreightBase", Order = 3)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpAccCarrierFreightBase { get; set; }

	[JsonProperty("smpAccCarrierFreightForeign", Order = 4)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpAccCarrierFreightForeign { get; set; }

	[JsonProperty("smpAccDiscountBase", Order = 5)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpAccDiscountBase { get; set; }

	[JsonProperty("smpAccDiscountForeign", Order = 6)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpAccDiscountForeign { get; set; }

	[JsonProperty("smpAccSurchargeBase", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpAccSurchargeBase { get; set; }

	[JsonProperty("smpAccSurchargeForeign", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpAccSurchargeForeign { get; set; }

	[JsonProperty("smpAdditionalWeight", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpAdditionalWeight { get; set; }

	[JsonProperty("smpAESITN", Order = 10)]
	[MaxLength(30)]
	public string smpAESITN { get; set; }

	[JsonProperty("smpArInvoiceContactID", Order = 11)]
	[MaxLength(5)]
	public string smpArInvoiceContactID { get; set; }

	[JsonProperty("smpArInvoiceLocationID", Order = 12)]
	[MaxLength(5)]
	public string smpArInvoiceLocationID { get; set; }

	[JsonProperty("smpBlindShipContactID", Order = 13)]
	[MaxLength(5)]
	public string smpBlindShipContactID { get; set; }

	[JsonProperty("smpBlindShipLocationID", Order = 14)]
	[MaxLength(5)]
	public string smpBlindShipLocationID { get; set; }

	[JsonProperty("smpBlindShipOrganizationID", Order = 15)]
	[MaxLength(10)]
	public string smpBlindShipOrganizationID { get; set; }

	[JsonProperty("smpCarrierDocumentFilePath", Order = 16)]
	[MaxLength(50)]
	public string smpCarrierDocumentFilePath { get; set; }

	[JsonProperty("smpClosedDate", Order = 17)]
	public DateTime? smpClosedDate { get; set; }

	[JsonProperty("smpShipmentID", Order = 18)]
	[Required(ErrorMessage = "smpShipmentID is required.")]
	[MaxLength(10)]
	public string smpShipmentID { get; set; }

	[JsonProperty("smpCodLabelFilePath", Order = 19)]
	[MaxLength(50)]
	public string smpCodLabelFilePath { get; set; }

	[JsonProperty("smpCreatedBy", Order = 20)]
	[MaxLength(20)]
	public string smpCreatedBy { get; set; }

	[JsonProperty("smpCreatedDate", Order = 21)]
	public DateTime? smpCreatedDate { get; set; }

	[JsonProperty("smpCurrencyRateID", Order = 22)]
	[MaxLength(5)]
	public string smpCurrencyRateID { get; set; }

	[JsonProperty("smpCustomerOrganizationID", Order = 23)]
	[Required(ErrorMessage = "smpCustomerOrganizationID is required.")]
	[MaxLength(10)]
	public string smpCustomerOrganizationID { get; set; }

	[JsonProperty("smpDocuments", Order = 24)]
	[MaxLength(50)]
	public string smpDocuments { get; set; }

	[JsonProperty("smpEdiTransferredDate", Order = 25)]
	public DateTime? smpEdiTransferredDate { get; set; }

	[JsonProperty("smpUniqueID", Order = 26)]
	public Guid smpUniqueID { get; set; }

	[JsonProperty("smpExchangeRate", Order = 27)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpExchangeRate { get; set; }

	[JsonProperty("smpExportingCarrier", Order = 28)]
	[MaxLength(35)]
	public string smpExportingCarrier { get; set; }

	[JsonProperty("smpFedEx3rdPartyLocationID", Order = 29)]
	[MaxLength(5)]
	public string smpFedEx3rdPartyLocationID { get; set; }

	[JsonProperty("smpFedEx3rdPartyOrganizationID", Order = 30)]
	[MaxLength(10)]
	public string smpFedEx3rdPartyOrganizationID { get; set; }

	[JsonProperty("smpFedExAccountNumber", Order = 31)]
	[MaxLength(15)]
	public string smpFedExAccountNumber { get; set; }

	[JsonProperty("smpFedExBillingOption", Order = 32)]
	[MaxLength(20)]
	public string smpFedExBillingOption { get; set; }

	[JsonProperty("smpFreightCharge", Order = 33)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpFreightCharge { get; set; }

	[JsonProperty("smpFreightChargeForeign", Order = 34)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpFreightChargeForeign { get; set; }

	[JsonProperty("smpFreightSubtotal", Order = 35)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpFreightSubtotal { get; set; }

	[JsonProperty("smpFreightSubtotalForeign", Order = 36)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpFreightSubtotalForeign { get; set; }

	[JsonProperty("smpFreightTotal", Order = 37)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpFreightTotal { get; set; }

	[JsonProperty("smpFreightTotalForeign", Order = 38)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpFreightTotalForeign { get; set; }

	[JsonProperty("smpClosed", Order = 39)]
	public bool smpClosed { get; set; }

	[JsonProperty("smpCustomRate", Order = 40)]
	public bool smpCustomRate { get; set; }

	[JsonProperty("smpEdiShipmentReady", Order = 41)]
	public bool smpEdiShipmentReady { get; set; }

	[JsonProperty("smpEdiTransferred", Order = 42)]
	public bool smpEdiTransferred { get; set; }

	[JsonProperty("smpPostedToGl", Order = 43)]
	public bool smpPostedToGl { get; set; }

	[JsonProperty("smpPrintLabels", Order = 44)]
	public bool smpPrintLabels { get; set; }

	[JsonProperty("smpPrintPackingSlip", Order = 45)]
	public bool smpPrintPackingSlip { get; set; }

	[JsonProperty("smpReversalEntry", Order = 46)]
	public bool smpReversalEntry { get; set; }

	[JsonProperty("smpReversed", Order = 47)]
	public bool smpReversed { get; set; }

	[JsonProperty("smpListBaseChargeBase", Order = 48)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpListBaseChargeBase { get; set; }

	[JsonProperty("smpListBaseChargeForeign", Order = 49)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpListBaseChargeForeign { get; set; }

	[JsonProperty("smpListCarrierFreightBase", Order = 50)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpListCarrierFreightBase { get; set; }

	[JsonProperty("smpListCarrierFreightForeign", Order = 51)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpListCarrierFreightForeign { get; set; }

	[JsonProperty("smpListDiscountBase", Order = 52)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpListDiscountBase { get; set; }

	[JsonProperty("smpListDiscountForeign", Order = 53)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpListDiscountForeign { get; set; }

	[JsonProperty("smpListSurchargeBase", Order = 54)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpListSurchargeBase { get; set; }

	[JsonProperty("smpListSurchargeForeign", Order = 55)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpListSurchargeForeign { get; set; }

	[JsonProperty("smpNumberOfLabels", Order = 56)]
	public short smpNumberOfLabels { get; set; }

	[JsonProperty("smpPlantDepartmentID", Order = 57)]
	[MaxLength(5)]
	public string smpPlantDepartmentID { get; set; }

	[JsonProperty("smpPlantID", Order = 58)]
	[MaxLength(5)]
	public string smpPlantID { get; set; }

	[JsonProperty("smpPostedDate", Order = 59)]
	public DateTime? smpPostedDate { get; set; }

	[JsonProperty("smpProjectID", Order = 60)]
	[MaxLength(10)]
	public string smpProjectID { get; set; }

	[JsonProperty("smpReasonForExport", Order = 61)]
	[MaxLength(20)]
	public string smpReasonForExport { get; set; }

	[JsonProperty("smpReturnInstructionsRTF", Order = 62)]
	[MaxLength(50)]
	public string smpReturnInstructionsRTF { get; set; }

	[JsonProperty("smpReturnInstructionsText", Order = 63)]
	[MaxLength(50)]
	public string smpReturnInstructionsText { get; set; }

	[JsonProperty("smpRowVersion", Order = 64)]
	public byte[] smpRowVersion { get; set; }

	[JsonProperty("smpShipContactID", Order = 65)]
	[MaxLength(5)]
	public string smpShipContactID { get; set; }

	[JsonProperty("smpShipDate", Order = 66)]
	[Required(ErrorMessage = "smpShipDate is required.")]
	public DateTime? smpShipDate { get; set; }

	[JsonProperty("smpShipLocationID", Order = 67)]
	[MaxLength(5)]
	public string smpShipLocationID { get; set; }

	[JsonProperty("smpShipmentIDNumber", Order = 68)]
	[MaxLength(20)]
	public string smpShipmentIDNumber { get; set; }

	[JsonProperty("smpShipmentSubtotal", Order = 69)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpShipmentSubtotal { get; set; }

	[JsonProperty("smpShipmentSubtotalForeign", Order = 70)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpShipmentSubtotalForeign { get; set; }

	[JsonProperty("smpShipmentTotal", Order = 71)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpShipmentTotal { get; set; }

	[JsonProperty("smpShipmentTotalForeign", Order = 72)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpShipmentTotalForeign { get; set; }

	[JsonProperty("smpShipOrganizationID", Order = 73)]
	[Required(ErrorMessage = "smpShipOrganizationID is required.")]
	[MaxLength(10)]
	public string smpShipOrganizationID { get; set; }

	[JsonProperty("smpShippingCommentsRTF", Order = 74)]
	[MaxLength(50)]
	public string smpShippingCommentsRTF { get; set; }

	[JsonProperty("smpShippingCommentsText", Order = 75)]
	[MaxLength(50)]
	public string smpShippingCommentsText { get; set; }

	[JsonProperty("smpShippingMethodID", Order = 76)]
	[MaxLength(5)]
	public string smpShippingMethodID { get; set; }

	[JsonProperty("smpShippingPaymentTypeID", Order = 77)]
	[MaxLength(5)]
	public string smpShippingPaymentTypeID { get; set; }

	[JsonProperty("smpStandardMessageID", Order = 78)]
	[MaxLength(10)]
	public string smpStandardMessageID { get; set; }

	[JsonProperty("smpTrackingNumber", Order = 79)]
	[MaxLength(30)]
	public string smpTrackingNumber { get; set; }

	[JsonProperty("smpUps3rdPartyLocationID", Order = 80)]
	[MaxLength(5)]
	public string smpUps3rdPartyLocationID { get; set; }

	[JsonProperty("smpUps3rdPartyOrganizationID", Order = 81)]
	[MaxLength(10)]
	public string smpUps3rdPartyOrganizationID { get; set; }

	[JsonProperty("smpUpsAccountNumber", Order = 82)]
	[MaxLength(6)]
	public string smpUpsAccountNumber { get; set; }

	[JsonProperty("smpUpsBillingOption", Order = 83)]
	[MaxLength(20)]
	public string smpUpsBillingOption { get; set; }

	[JsonProperty("smpWeightSubtotal", Order = 84)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpWeightSubtotal { get; set; }

	[JsonProperty("smpWeightTotal", Order = 85)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smpWeightTotal { get; set; }

	[JsonProperty("customFields", Order = 86)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
