using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSalesOrderDto
{
	[JsonProperty("ompApprovalDecisionDate", Order = 1)]
	public DateTime? ompApprovalDecisionDate { get; set; }

	[JsonProperty("ompApprovalRequestDate", Order = 2)]
	public DateTime? ompApprovalRequestDate { get; set; }

	[JsonProperty("ompArInvoiceContactID", Order = 3)]
	[MaxLength(5)]
	public string ompArInvoiceContactID { get; set; }

	[JsonProperty("ompArInvoiceLocationID", Order = 4)]
	[MaxLength(5)]
	public string ompArInvoiceLocationID { get; set; }

	[JsonProperty("ompCallID", Order = 5)]
	[MaxLength(10)]
	public string ompCallID { get; set; }

	[JsonProperty("ompClosedDate", Order = 6)]
	public DateTime? ompClosedDate { get; set; }

	[JsonProperty("ompSalesOrderID", Order = 7)]
	[Required(ErrorMessage = "ompSalesOrderID is required.")]
	[MaxLength(10)]
	public string ompSalesOrderID { get; set; }

	[JsonProperty("ompCreatedBy", Order = 8)]
	[MaxLength(20)]
	public string ompCreatedBy { get; set; }

	[JsonProperty("ompCreatedDate", Order = 9)]
	public DateTime? ompCreatedDate { get; set; }

	[JsonProperty("ompCurrencyRateID", Order = 10)]
	[MaxLength(5)]
	public string ompCurrencyRateID { get; set; }

	[JsonProperty("ompCustomerOrganizationID", Order = 11)]
	[Required(ErrorMessage = "ompCustomerOrganizationID is required.")]
	[MaxLength(10)]
	public string ompCustomerOrganizationID { get; set; }

	[JsonProperty("ompCustomerPo", Order = 12)]
	[MaxLength(40)]
	public string ompCustomerPo { get; set; }

	[JsonProperty("ompDepositAmountBase", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompDepositAmountBase { get; set; }

	[JsonProperty("ompDepositAmountForeign", Order = 14)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompDepositAmountForeign { get; set; }

	[JsonProperty("ompDepositPercent", Order = 15)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompDepositPercent { get; set; }

	[JsonProperty("ompDiscountTotalBase", Order = 16)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompDiscountTotalBase { get; set; }

	[JsonProperty("ompDiscountTotalForeign", Order = 17)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompDiscountTotalForeign { get; set; }

	[JsonProperty("ompUniqueID", Order = 18)]
	public Guid ompUniqueID { get; set; }

	[JsonProperty("ompExchangeRate", Order = 19)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompExchangeRate { get; set; }

	[JsonProperty("ompFedEx3rdPartyLocationID", Order = 20)]
	[MaxLength(5)]
	public string ompFedEx3rdPartyLocationID { get; set; }

	[JsonProperty("ompFedEx3rdPartyOrganizationID", Order = 21)]
	[MaxLength(10)]
	public string ompFedEx3rdPartyOrganizationID { get; set; }

	[JsonProperty("ompFedExAccountNumber", Order = 22)]
	[MaxLength(15)]
	public string ompFedExAccountNumber { get; set; }

	[JsonProperty("ompFedExBillingOption", Order = 23)]
	[MaxLength(20)]
	public string ompFedExBillingOption { get; set; }

	[JsonProperty("ompFreeOnBoardDescription", Order = 24)]
	[MaxLength(15)]
	public string ompFreeOnBoardDescription { get; set; }

	[JsonProperty("ompFreightAmountBase", Order = 25)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompFreightAmountBase { get; set; }

	[JsonProperty("ompFreightAmountForeign", Order = 26)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompFreightAmountForeign { get; set; }

	[JsonProperty("ompFreightSubtotalBase", Order = 27)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompFreightSubtotalBase { get; set; }

	[JsonProperty("ompFreightSubtotalForeign", Order = 28)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompFreightSubtotalForeign { get; set; }

	[JsonProperty("ompFreightTaxAmountBase", Order = 29)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompFreightTaxAmountBase { get; set; }

	[JsonProperty("ompFreightTaxAmountForeign", Order = 30)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompFreightTaxAmountForeign { get; set; }

	[JsonProperty("ompFreightTaxCodeID", Order = 31)]
	[MaxLength(5)]
	public string ompFreightTaxCodeID { get; set; }

	[JsonProperty("ompFreightTotalBase", Order = 32)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompFreightTotalBase { get; set; }

	[JsonProperty("ompFreightTotalForeign", Order = 33)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompFreightTotalForeign { get; set; }

	[JsonProperty("ompFullOrderSubtotalBase", Order = 34)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompFullOrderSubtotalBase { get; set; }

	[JsonProperty("ompFullOrderSubtotalForeign", Order = 35)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompFullOrderSubtotalForeign { get; set; }

	[JsonProperty("ompAvalaraTaxCalculated", Order = 36)]
	public bool ompAvalaraTaxCalculated { get; set; }

	[JsonProperty("ompClosed", Order = 37)]
	public bool ompClosed { get; set; }

	[JsonProperty("ompCreatedByEdi", Order = 38)]
	public bool ompCreatedByEdi { get; set; }

	[JsonProperty("ompCustomRate", Order = 39)]
	public bool ompCustomRate { get; set; }

	[JsonProperty("ompDeposit", Order = 40)]
	public bool ompDeposit { get; set; }

	[JsonProperty("ompDepositCreated", Order = 41)]
	public bool ompDepositCreated { get; set; }

	[JsonProperty("ompReadyToPrint", Order = 42)]
	public bool ompReadyToPrint { get; set; }

	[JsonProperty("ompNextApprovalEmployeeID", Order = 43)]
	[MaxLength(10)]
	public string ompNextApprovalEmployeeID { get; set; }

	[JsonProperty("ompOrderCommentsRTF", Order = 44)]
	[MaxLength(50)]
	public string ompOrderCommentsRTF { get; set; }

	[JsonProperty("ompOrderCommentsText", Order = 45)]
	[MaxLength(50)]
	public string ompOrderCommentsText { get; set; }

	[JsonProperty("ompOrderDate", Order = 46)]
	[Required(ErrorMessage = "ompOrderDate is required.")]
	public DateTime? ompOrderDate { get; set; }

	[JsonProperty("ompOrderSubtotalBase", Order = 47)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompOrderSubtotalBase { get; set; }

	[JsonProperty("ompOrderSubTotalForeign", Order = 48)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompOrderSubTotalForeign { get; set; }

	[JsonProperty("ompOrderTaxAmountBase", Order = 49)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompOrderTaxAmountBase { get; set; }

	[JsonProperty("ompOrderTaxAmountForeign", Order = 50)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompOrderTaxAmountForeign { get; set; }

	[JsonProperty("ompOrderTotalBase", Order = 51)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompOrderTotalBase { get; set; }

	[JsonProperty("ompOrderTotalForeign", Order = 52)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompOrderTotalForeign { get; set; }

	[JsonProperty("ompPaymentTermID", Order = 53)]
	[MaxLength(5)]
	public string ompPaymentTermID { get; set; }

	[JsonProperty("ompPlantDepartmentID", Order = 54)]
	[MaxLength(5)]
	public string ompPlantDepartmentID { get; set; }

	[JsonProperty("ompPlantID", Order = 55)]
	[MaxLength(5)]
	public string ompPlantID { get; set; }

	[JsonProperty("ompProjectID", Order = 56)]
	[MaxLength(10)]
	public string ompProjectID { get; set; }

	[JsonProperty("ompQuoteContactID", Order = 57)]
	[MaxLength(5)]
	public string ompQuoteContactID { get; set; }

	[JsonProperty("ompQuoteLocationID", Order = 58)]
	[MaxLength(5)]
	public string ompQuoteLocationID { get; set; }

	[JsonProperty("ompRequestedShipDate", Order = 59)]
	public DateTime? ompRequestedShipDate { get; set; }

	[JsonProperty("ompResellerContactID", Order = 60)]
	[MaxLength(5)]
	public string ompResellerContactID { get; set; }

	[JsonProperty("ompResellerLocationID", Order = 61)]
	[MaxLength(5)]
	public string ompResellerLocationID { get; set; }

	[JsonProperty("ompResellerOrganizationID", Order = 62)]
	[MaxLength(10)]
	public string ompResellerOrganizationID { get; set; }

	[JsonProperty("ompRowVersion", Order = 63)]
	public byte[] ompRowVersion { get; set; }

	[JsonProperty("ompSecondFreightTaxAmtBase", Order = 64)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompSecondFreightTaxAmtBase { get; set; }

	[JsonProperty("ompSecondFreightTaxAmtForeign", Order = 65)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompSecondFreightTaxAmtForeign { get; set; }

	[JsonProperty("ompSecondFreightTaxCodeID", Order = 66)]
	[MaxLength(5)]
	public string ompSecondFreightTaxCodeID { get; set; }

	[JsonProperty("ompShipContactID", Order = 67)]
	[MaxLength(5)]
	public string ompShipContactID { get; set; }

	[JsonProperty("ompShipLocationID", Order = 68)]
	[MaxLength(5)]
	public string ompShipLocationID { get; set; }

	[JsonProperty("ompShipOrganizationID", Order = 69)]
	[Required(ErrorMessage = "ompShipOrganizationID is required.")]
	[MaxLength(10)]
	public string ompShipOrganizationID { get; set; }

	[JsonProperty("ompShippingMethodID", Order = 70)]
	[MaxLength(5)]
	public string ompShippingMethodID { get; set; }

	[JsonProperty("ompShippingPaymentTypeID", Order = 71)]
	[MaxLength(5)]
	public string ompShippingPaymentTypeID { get; set; }

	[JsonProperty("ompSplitPercentTotal", Order = 72)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompSplitPercentTotal { get; set; }

	[JsonProperty("ompStandardMessageID", Order = 73)]
	[MaxLength(10)]
	public string ompStandardMessageID { get; set; }

	[JsonProperty("ompStatus", Order = 74)]
	[Required(ErrorMessage = "ompStatus is required.")]
	public byte ompStatus { get; set; }

	[JsonProperty("ompTaxSubtotalBase", Order = 75)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompTaxSubtotalBase { get; set; }

	[JsonProperty("ompTaxSubtotalForeign", Order = 76)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompTaxSubtotalForeign { get; set; }

	[JsonProperty("ompTotalOrderWeight", Order = 77)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ompTotalOrderWeight { get; set; }

	[JsonProperty("ompUps3rdPartyLocationID", Order = 78)]
	[MaxLength(5)]
	public string ompUps3rdPartyLocationID { get; set; }

	[JsonProperty("ompUps3rdPartyOrganizationID", Order = 79)]
	[MaxLength(10)]
	public string ompUps3rdPartyOrganizationID { get; set; }

	[JsonProperty("ompUpsAccountNumber", Order = 80)]
	[MaxLength(6)]
	public string ompUpsAccountNumber { get; set; }

	[JsonProperty("ompUpsBillingOption", Order = 81)]
	[MaxLength(20)]
	public string ompUpsBillingOption { get; set; }

	[JsonProperty("customFields", Order = 82)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
