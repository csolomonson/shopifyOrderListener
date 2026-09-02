using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchaseOrderDto
{
	[JsonProperty("pmpApInvoiceContactID", Order = 1)]
	[MaxLength(5)]
	public string pmpApInvoiceContactID { get; set; }

	[JsonProperty("pmpApInvoiceLocationID", Order = 2)]
	[MaxLength(5)]
	public string pmpApInvoiceLocationID { get; set; }

	[JsonProperty("pmpApprovalDecisionDate", Order = 3)]
	public DateTime? pmpApprovalDecisionDate { get; set; }

	[JsonProperty("pmpApprovalRequestDate", Order = 4)]
	public DateTime? pmpApprovalRequestDate { get; set; }

	[JsonProperty("pmpBuyerEmployeeID", Order = 5)]
	[MaxLength(10)]
	public string pmpBuyerEmployeeID { get; set; }

	[JsonProperty("pmpClosedDate", Order = 6)]
	public DateTime? pmpClosedDate { get; set; }

	[JsonProperty("pmpPurchaseOrderID", Order = 7)]
	[Required(ErrorMessage = "pmpPurchaseOrderID is required.")]
	[MaxLength(10)]
	public string pmpPurchaseOrderID { get; set; }

	[JsonProperty("pmpCreatedBy", Order = 8)]
	[MaxLength(20)]
	public string pmpCreatedBy { get; set; }

	[JsonProperty("pmpCreatedDate", Order = 9)]
	public DateTime? pmpCreatedDate { get; set; }

	[JsonProperty("pmpCurrencyRateID", Order = 10)]
	[MaxLength(5)]
	public string pmpCurrencyRateID { get; set; }

	[JsonProperty("pmpDocuments", Order = 11)]
	[MaxLength(50)]
	public string pmpDocuments { get; set; }

	[JsonProperty("pmpDropShipContactID", Order = 12)]
	[MaxLength(5)]
	public string pmpDropShipContactID { get; set; }

	[JsonProperty("pmpDropShipLocationID", Order = 13)]
	[MaxLength(5)]
	public string pmpDropShipLocationID { get; set; }

	[JsonProperty("pmpDropShipOrganizationID", Order = 14)]
	[MaxLength(10)]
	public string pmpDropShipOrganizationID { get; set; }

	[JsonProperty("pmpDueDate", Order = 15)]
	public DateTime? pmpDueDate { get; set; }

	[JsonProperty("pmpUniqueID", Order = 16)]
	public Guid pmpUniqueID { get; set; }

	[JsonProperty("pmpExchangeRate", Order = 17)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmpExchangeRate { get; set; }

	[JsonProperty("pmpFreeOnBoardDescription", Order = 18)]
	[MaxLength(15)]
	public string pmpFreeOnBoardDescription { get; set; }

	[JsonProperty("pmpIntraCompanyPostedDate", Order = 19)]
	public DateTime? pmpIntraCompanyPostedDate { get; set; }

	[JsonProperty("pmpClosed", Order = 20)]
	public bool pmpClosed { get; set; }

	[JsonProperty("pmpCustomRate", Order = 21)]
	public bool pmpCustomRate { get; set; }

	[JsonProperty("pmpIntraCompany", Order = 22)]
	public bool pmpIntraCompany { get; set; }

	[JsonProperty("pmpIntraCompanyPosted", Order = 23)]
	public bool pmpIntraCompanyPosted { get; set; }

	[JsonProperty("pmpReadyToPrint", Order = 24)]
	public bool pmpReadyToPrint { get; set; }

	[JsonProperty("pmpNextApprovalEmployeeID", Order = 25)]
	[MaxLength(10)]
	public string pmpNextApprovalEmployeeID { get; set; }

	[JsonProperty("pmpOrderCommentsRTF", Order = 26)]
	[MaxLength(50)]
	public string pmpOrderCommentsRTF { get; set; }

	[JsonProperty("pmpOrderCommentsText", Order = 27)]
	[MaxLength(50)]
	public string pmpOrderCommentsText { get; set; }

	[JsonProperty("pmpOrderDate", Order = 28)]
	[Required(ErrorMessage = "pmpOrderDate is required.")]
	public DateTime? pmpOrderDate { get; set; }

	[JsonProperty("pmpOrderSubtotalBase", Order = 29)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmpOrderSubtotalBase { get; set; }

	[JsonProperty("pmpOrderSubtotalForeign", Order = 30)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmpOrderSubtotalForeign { get; set; }

	[JsonProperty("pmpOrderTaxAmountBase", Order = 31)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmpOrderTaxAmountBase { get; set; }

	[JsonProperty("pmpOrderTaxAmountForeign", Order = 32)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmpOrderTaxAmountForeign { get; set; }

	[JsonProperty("pmpOrderTotalBase", Order = 33)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmpOrderTotalBase { get; set; }

	[JsonProperty("pmpOrderTotalForeign", Order = 34)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmpOrderTotalForeign { get; set; }

	[JsonProperty("pmpPaymentTermID", Order = 35)]
	[MaxLength(5)]
	public string pmpPaymentTermID { get; set; }

	[JsonProperty("pmpPlantDepartmentID", Order = 36)]
	[MaxLength(5)]
	public string pmpPlantDepartmentID { get; set; }

	[JsonProperty("pmpPlantID", Order = 37)]
	[MaxLength(5)]
	public string pmpPlantID { get; set; }

	[JsonProperty("pmpProjectID", Order = 38)]
	[MaxLength(10)]
	public string pmpProjectID { get; set; }

	[JsonProperty("pmpPurchaseContactID", Order = 39)]
	[MaxLength(5)]
	public string pmpPurchaseContactID { get; set; }

	[JsonProperty("pmpPurchaseLocationID", Order = 40)]
	[MaxLength(5)]
	public string pmpPurchaseLocationID { get; set; }

	[JsonProperty("pmpRowVersion", Order = 41)]
	public byte[] pmpRowVersion { get; set; }

	[JsonProperty("pmpShippingMethodID", Order = 42)]
	[MaxLength(5)]
	public string pmpShippingMethodID { get; set; }

	[JsonProperty("pmpStandardMessageID", Order = 43)]
	[MaxLength(10)]
	public string pmpStandardMessageID { get; set; }

	[JsonProperty("pmpStatus", Order = 44)]
	[Required(ErrorMessage = "pmpStatus is required.")]
	public byte pmpStatus { get; set; }

	[JsonProperty("pmpSupplierOrganizationID", Order = 45)]
	[Required(ErrorMessage = "pmpSupplierOrganizationID is required.")]
	[MaxLength(10)]
	public string pmpSupplierOrganizationID { get; set; }

	[JsonProperty("customFields", Order = 46)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
