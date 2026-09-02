using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCallDto
{
	[JsonProperty("kbpAcceptedDate", Order = 1)]
	public DateTime? kbpAcceptedDate { get; set; }

	[JsonProperty("kbpApInvoiceID", Order = 2)]
	[MaxLength(10)]
	public string kbpApInvoiceID { get; set; }

	[JsonProperty("kbpArInvoiceContactID", Order = 3)]
	[MaxLength(5)]
	public string kbpArInvoiceContactID { get; set; }

	[JsonProperty("kbpArInvoiceID", Order = 4)]
	[MaxLength(10)]
	public string kbpArInvoiceID { get; set; }

	[JsonProperty("kbpArInvoiceLocationID", Order = 5)]
	[MaxLength(5)]
	public string kbpArInvoiceLocationID { get; set; }

	[JsonProperty("kbpArInvoiceOrganizationID", Order = 6)]
	[MaxLength(10)]
	public string kbpArInvoiceOrganizationID { get; set; }

	[JsonProperty("kbpAssignedDate", Order = 7)]
	public DateTime? kbpAssignedDate { get; set; }

	[JsonProperty("kbpAssignedToEmployeeID", Order = 8)]
	[MaxLength(10)]
	public string kbpAssignedToEmployeeID { get; set; }

	[JsonProperty("kbpCallTypeID", Order = 9)]
	[Required(ErrorMessage = "kbpCallTypeID is required.")]
	[MaxLength(5)]
	public string kbpCallTypeID { get; set; }

	[JsonProperty("kbpClosedByEmployeeID", Order = 10)]
	[MaxLength(10)]
	public string kbpClosedByEmployeeID { get; set; }

	[JsonProperty("kbpClosedDate", Order = 11)]
	public DateTime? kbpClosedDate { get; set; }

	[JsonProperty("kbpCallID", Order = 12)]
	[Required(ErrorMessage = "kbpCallID is required.")]
	[MaxLength(10)]
	public string kbpCallID { get; set; }

	[JsonProperty("kbpContactID", Order = 13)]
	[MaxLength(5)]
	public string kbpContactID { get; set; }

	[JsonProperty("kbpContactMethodID", Order = 14)]
	[MaxLength(5)]
	public string kbpContactMethodID { get; set; }

	[JsonProperty("kbpCreatedBy", Order = 15)]
	[MaxLength(20)]
	public string kbpCreatedBy { get; set; }

	[JsonProperty("kbpCreatedDate", Order = 16)]
	public DateTime? kbpCreatedDate { get; set; }

	[JsonProperty("kbpCurrencyRateID", Order = 17)]
	[MaxLength(5)]
	public string kbpCurrencyRateID { get; set; }

	[JsonProperty("kbpDmrClaimID", Order = 18)]
	[MaxLength(10)]
	public string kbpDmrClaimID { get; set; }

	[JsonProperty("kbpDueDate", Order = 19)]
	public DateTime? kbpDueDate { get; set; }

	[JsonProperty("kbpUniqueID", Order = 20)]
	public Guid kbpUniqueID { get; set; }

	[JsonProperty("kbpExchangeRate", Order = 21)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal kbpExchangeRate { get; set; }

	[JsonProperty("kbpExtraTime", Order = 22)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal kbpExtraTime { get; set; }

	[JsonProperty("kbpBillable", Order = 23)]
	public bool kbpBillable { get; set; }

	[JsonProperty("kbpCreatedFromMobile", Order = 24)]
	public bool kbpCreatedFromMobile { get; set; }

	[JsonProperty("kbpCustomRate", Order = 25)]
	public bool kbpCustomRate { get; set; }

	[JsonProperty("kbpFieldServiceCall", Order = 26)]
	public bool kbpFieldServiceCall { get; set; }

	[JsonProperty("kbpFieldServiceJobCreated", Order = 27)]
	public bool kbpFieldServiceJobCreated { get; set; }

	[JsonProperty("kbpInbound", Order = 28)]
	public bool kbpInbound { get; set; }

	[JsonProperty("kbpInternalOnly", Order = 29)]
	public bool kbpInternalOnly { get; set; }

	[JsonProperty("kbpInvoicedComplete", Order = 30)]
	public bool kbpInvoicedComplete { get; set; }

	[JsonProperty("kbpPublished", Order = 31)]
	public bool kbpPublished { get; set; }

	[JsonProperty("kbpJobID", Order = 32)]
	[MaxLength(20)]
	public string kbpJobID { get; set; }

	[JsonProperty("kbpLeadID", Order = 33)]
	[MaxLength(10)]
	public string kbpLeadID { get; set; }

	[JsonProperty("kbpLocationID", Order = 34)]
	[MaxLength(5)]
	public string kbpLocationID { get; set; }

	[JsonProperty("kbpLongDescriptionRtf", Order = 35)]
	public string kbpLongDescriptionRtf { get; set; }

	[JsonProperty("kbpLongDescriptionText", Order = 36)]
	public string kbpLongDescriptionText { get; set; }

	[JsonProperty("kbpMethodPartID", Order = 37)]
	[MaxLength(30)]
	public string kbpMethodPartID { get; set; }

	[JsonProperty("kbpMethodRevisionID", Order = 38)]
	[MaxLength(15)]
	public string kbpMethodRevisionID { get; set; }

	[JsonProperty("kbpOpenedByEmployeeID", Order = 39)]
	[Required(ErrorMessage = "kbpOpenedByEmployeeID is required.")]
	[MaxLength(10)]
	public string kbpOpenedByEmployeeID { get; set; }

	[JsonProperty("kbpOpenedDate", Order = 40)]
	[Required(ErrorMessage = "kbpOpenedDate is required.")]
	public DateTime? kbpOpenedDate { get; set; }

	[JsonProperty("kbpOrganizationID", Order = 41)]
	[Required(ErrorMessage = "kbpOrganizationID is required.")]
	[MaxLength(10)]
	public string kbpOrganizationID { get; set; }

	[JsonProperty("kbpOrgPartID", Order = 42)]
	[MaxLength(30)]
	public string kbpOrgPartID { get; set; }

	[JsonProperty("kbpPartGroupID", Order = 43)]
	[MaxLength(5)]
	public string kbpPartGroupID { get; set; }

	[JsonProperty("kbpPartID", Order = 44)]
	[MaxLength(30)]
	public string kbpPartID { get; set; }

	[JsonProperty("kbpPartRevisionID", Order = 45)]
	[MaxLength(15)]
	public string kbpPartRevisionID { get; set; }

	[JsonProperty("kbpPartShortDescription", Order = 46)]
	[MaxLength(50)]
	public string kbpPartShortDescription { get; set; }

	[JsonProperty("kbpPhoneNumber", Order = 47)]
	[MaxLength(20)]
	public string kbpPhoneNumber { get; set; }

	[JsonProperty("kbpPriorityID", Order = 48)]
	public byte kbpPriorityID { get; set; }

	[JsonProperty("kbpProjectAreaID", Order = 49)]
	[MaxLength(15)]
	public string kbpProjectAreaID { get; set; }

	[JsonProperty("kbpProjectID", Order = 50)]
	[MaxLength(10)]
	public string kbpProjectID { get; set; }

	[JsonProperty("kbpPurchaseOrderID", Order = 51)]
	[MaxLength(10)]
	public string kbpPurchaseOrderID { get; set; }

	[JsonProperty("kbpQuoteID", Order = 52)]
	[MaxLength(10)]
	public string kbpQuoteID { get; set; }

	[JsonProperty("kbpReasonID", Order = 53)]
	[MaxLength(5)]
	public string kbpReasonID { get; set; }

	[JsonProperty("kbpReceiptID", Order = 54)]
	[MaxLength(10)]
	public string kbpReceiptID { get; set; }

	[JsonProperty("kbpRfqID", Order = 55)]
	[MaxLength(10)]
	public string kbpRfqID { get; set; }

	[JsonProperty("kbpRmaClaimID", Order = 56)]
	[MaxLength(10)]
	public string kbpRmaClaimID { get; set; }

	[JsonProperty("kbpRowVersion", Order = 57)]
	public byte[] kbpRowVersion { get; set; }

	[JsonProperty("kbpSalesOrderID", Order = 58)]
	[MaxLength(10)]
	public string kbpSalesOrderID { get; set; }

	[JsonProperty("kbpSerialNumberID", Order = 59)]
	[MaxLength(30)]
	public string kbpSerialNumberID { get; set; }

	[JsonProperty("kbpShipmentID", Order = 60)]
	[MaxLength(10)]
	public string kbpShipmentID { get; set; }

	[JsonProperty("kbpShortDescription", Order = 61)]
	[Required(ErrorMessage = "kbpShortDescription is required.")]
	[MaxLength(70)]
	public string kbpShortDescription { get; set; }

	[JsonProperty("kbpStatus", Order = 62)]
	[Required(ErrorMessage = "kbpStatus is required.")]
	[MaxLength(1)]
	public string kbpStatus { get; set; }

	[JsonProperty("kbpSubTotalTime", Order = 63)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal kbpSubTotalTime { get; set; }

	[JsonProperty("kbpTemplateFile", Order = 64)]
	[MaxLength(255)]
	public string kbpTemplateFile { get; set; }

	[JsonProperty("kbpTimeSpent", Order = 65)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal kbpTimeSpent { get; set; }

	[JsonProperty("kbpTotalTime", Order = 66)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal kbpTotalTime { get; set; }

	[JsonProperty("customFields", Order = 67)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
