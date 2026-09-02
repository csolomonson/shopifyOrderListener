using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPARInvoiceDto
{
	[JsonProperty("arpArGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string arpArGlAccountID { get; set; }

	[JsonProperty("arpArInvoiceContactID", Order = 2)]
	[MaxLength(5)]
	public string arpArInvoiceContactID { get; set; }

	[JsonProperty("arpArInvoiceLocationID", Order = 3)]
	[MaxLength(5)]
	public string arpArInvoiceLocationID { get; set; }

	[JsonProperty("arpArInvoiceID", Order = 4)]
	[Required(ErrorMessage = "arpArInvoiceID is required.")]
	[MaxLength(10)]
	public string arpArInvoiceID { get; set; }

	[JsonProperty("arpCommissionAmountBase", Order = 5)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpCommissionAmountBase { get; set; }

	[JsonProperty("arpCreatedBy", Order = 6)]
	[MaxLength(20)]
	public string arpCreatedBy { get; set; }

	[JsonProperty("arpCreatedDate", Order = 7)]
	public DateTime? arpCreatedDate { get; set; }

	[JsonProperty("arpCreditArInvoiceID", Order = 8)]
	[MaxLength(10)]
	public string arpCreditArInvoiceID { get; set; }

	[JsonProperty("arpCreditDate", Order = 9)]
	public DateTime? arpCreditDate { get; set; }

	[JsonProperty("arpCreditReasonID", Order = 10)]
	[MaxLength(5)]
	public string arpCreditReasonID { get; set; }

	[JsonProperty("arpCurrencyRateID", Order = 11)]
	[MaxLength(5)]
	public string arpCurrencyRateID { get; set; }

	[JsonProperty("arpCustomerOrganizationID", Order = 12)]
	[Required(ErrorMessage = "arpCustomerOrganizationID is required.")]
	[MaxLength(10)]
	public string arpCustomerOrganizationID { get; set; }

	[JsonProperty("arpDepositAppliedBase", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpDepositAppliedBase { get; set; }

	[JsonProperty("arpDepositAppliedForeign", Order = 14)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpDepositAppliedForeign { get; set; }

	[JsonProperty("arpDepositBalanceBase", Order = 15)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpDepositBalanceBase { get; set; }

	[JsonProperty("arpDepositBalanceForeign", Order = 16)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpDepositBalanceForeign { get; set; }

	[JsonProperty("arpDepositGlAccountID", Order = 17)]
	[MaxLength(11)]
	public string arpDepositGlAccountID { get; set; }

	[JsonProperty("arpDepositTransferredBase", Order = 18)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpDepositTransferredBase { get; set; }

	[JsonProperty("arpDepositTransferredForeign", Order = 19)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpDepositTransferredForeign { get; set; }

	[JsonProperty("arpDiscountDueDate", Order = 20)]
	public DateTime? arpDiscountDueDate { get; set; }

	[JsonProperty("arpDiscountGlAccountID", Order = 21)]
	[MaxLength(11)]
	public string arpDiscountGlAccountID { get; set; }

	[JsonProperty("arpDiscountTotalBase", Order = 22)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpDiscountTotalBase { get; set; }

	[JsonProperty("arpDiscountTotalForeign", Order = 23)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpDiscountTotalForeign { get; set; }

	[JsonProperty("arpDueDate", Order = 24)]
	[Required(ErrorMessage = "arpDueDate is required.")]
	public DateTime? arpDueDate { get; set; }

	[JsonProperty("arpEdiTransferredDate", Order = 25)]
	public DateTime? arpEdiTransferredDate { get; set; }

	[JsonProperty("arpUniqueID", Order = 26)]
	public Guid arpUniqueID { get; set; }

	[JsonProperty("arpExchangeRate", Order = 27)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpExchangeRate { get; set; }

	[JsonProperty("arpFreeOnBoardDescription", Order = 28)]
	[MaxLength(15)]
	public string arpFreeOnBoardDescription { get; set; }

	[JsonProperty("arpFreightAmountBase", Order = 29)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpFreightAmountBase { get; set; }

	[JsonProperty("arpFreightAmountForeign", Order = 30)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpFreightAmountForeign { get; set; }

	[JsonProperty("arpFreightGlAccountID", Order = 31)]
	[MaxLength(11)]
	public string arpFreightGlAccountID { get; set; }

	[JsonProperty("arpFreightSubtotalBase", Order = 32)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpFreightSubtotalBase { get; set; }

	[JsonProperty("arpFreightSubtotalForeign", Order = 33)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpFreightSubtotalForeign { get; set; }

	[JsonProperty("arpFreightTaxAmountBase", Order = 34)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpFreightTaxAmountBase { get; set; }

	[JsonProperty("arpFreightTaxAmountForeign", Order = 35)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpFreightTaxAmountForeign { get; set; }

	[JsonProperty("arpFreightTaxCodeID", Order = 36)]
	[MaxLength(5)]
	public string arpFreightTaxCodeID { get; set; }

	[JsonProperty("arpFreightTotalBase", Order = 37)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpFreightTotalBase { get; set; }

	[JsonProperty("arpFreightTotalForeign", Order = 38)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpFreightTotalForeign { get; set; }

	[JsonProperty("arpFullInvoiceSubtotalBase", Order = 39)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpFullInvoiceSubtotalBase { get; set; }

	[JsonProperty("arpFullInvoiceSubtotalForeign", Order = 40)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpFullInvoiceSubtotalForeign { get; set; }

	[JsonProperty("arpGlFiscalYearID", Order = 41)]
	[Required(ErrorMessage = "arpGlFiscalYearID is required.")]
	public short arpGlFiscalYearID { get; set; }

	[JsonProperty("arpGlFiscalYearPeriodID", Order = 42)]
	[Required(ErrorMessage = "arpGlFiscalYearPeriodID is required.")]
	public byte arpGlFiscalYearPeriodID { get; set; }

	[JsonProperty("arpIntraCompanyPostedDate", Order = 43)]
	public DateTime? arpIntraCompanyPostedDate { get; set; }

	[JsonProperty("arpInvoiceBalanceBase", Order = 44)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpInvoiceBalanceBase { get; set; }

	[JsonProperty("arpInvoiceBalanceForeign", Order = 45)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpInvoiceBalanceForeign { get; set; }

	[JsonProperty("arpInvoiceCommentsRTF", Order = 46)]
	[MaxLength(50)]
	public string arpInvoiceCommentsRTF { get; set; }

	[JsonProperty("arpInvoiceCommentsText", Order = 47)]
	[MaxLength(50)]
	public string arpInvoiceCommentsText { get; set; }

	[JsonProperty("arpInvoiceDate", Order = 48)]
	[Required(ErrorMessage = "arpInvoiceDate is required.")]
	public DateTime? arpInvoiceDate { get; set; }

	[JsonProperty("arpInvoicePaidBase", Order = 49)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpInvoicePaidBase { get; set; }

	[JsonProperty("arpInvoicePaidForeign", Order = 50)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpInvoicePaidForeign { get; set; }

	[JsonProperty("arpInvoiceSubtotalBase", Order = 51)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpInvoiceSubtotalBase { get; set; }

	[JsonProperty("arpInvoiceSubtotalForeign", Order = 52)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpInvoiceSubtotalForeign { get; set; }

	[JsonProperty("arpInvoiceTaxAmountBase", Order = 53)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpInvoiceTaxAmountBase { get; set; }

	[JsonProperty("arpInvoiceTaxAmountForeign", Order = 54)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpInvoiceTaxAmountForeign { get; set; }

	[JsonProperty("arpInvoiceTotalBase", Order = 55)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpInvoiceTotalBase { get; set; }

	[JsonProperty("arpInvoiceTotalForeign", Order = 56)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpInvoiceTotalForeign { get; set; }

	[JsonProperty("arpInvoiceType", Order = 57)]
	[Required(ErrorMessage = "arpInvoiceType is required.")]
	public byte arpInvoiceType { get; set; }

	[JsonProperty("arpAvalaraOverrideTax", Order = 58)]
	public bool arpAvalaraOverrideTax { get; set; }

	[JsonProperty("arpAvalaraTaxCalculated", Order = 59)]
	public bool arpAvalaraTaxCalculated { get; set; }

	[JsonProperty("arpCustomRate", Order = 60)]
	public bool arpCustomRate { get; set; }

	[JsonProperty("arpDepositCredit", Order = 61)]
	public bool arpDepositCredit { get; set; }

	[JsonProperty("arpEdiTransferred", Order = 62)]
	public bool arpEdiTransferred { get; set; }

	[JsonProperty("arpIncludeFreightInPrice", Order = 63)]
	public bool arpIncludeFreightInPrice { get; set; }

	[JsonProperty("arpIncludeTaxInRetention", Order = 64)]
	public bool arpIncludeTaxInRetention { get; set; }

	[JsonProperty("arpIntraCompany", Order = 65)]
	public bool arpIntraCompany { get; set; }

	[JsonProperty("arpIntraCompanyPosted", Order = 66)]
	public bool arpIntraCompanyPosted { get; set; }

	[JsonProperty("arpOnHold", Order = 67)]
	public bool arpOnHold { get; set; }

	[JsonProperty("arpOpenInvoiceLoad", Order = 68)]
	public bool arpOpenInvoiceLoad { get; set; }

	[JsonProperty("arpOverpayment", Order = 69)]
	public bool arpOverpayment { get; set; }

	[JsonProperty("arpPaidComplete", Order = 70)]
	public bool arpPaidComplete { get; set; }

	[JsonProperty("arpPostedToGl", Order = 71)]
	public bool arpPostedToGl { get; set; }

	[JsonProperty("arpReadyToPrint", Order = 72)]
	public bool arpReadyToPrint { get; set; }

	[JsonProperty("arpRecurringInvoice", Order = 73)]
	public bool arpRecurringInvoice { get; set; }

	[JsonProperty("arpRefundCheckRequired", Order = 74)]
	public bool arpRefundCheckRequired { get; set; }

	[JsonProperty("arpLineCommissionTotal", Order = 75)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpLineCommissionTotal { get; set; }

	[JsonProperty("arpOrderDate", Order = 76)]
	[Required(ErrorMessage = "arpOrderDate is required.")]
	public DateTime? arpOrderDate { get; set; }

	[JsonProperty("arpOriginalExchangeRate", Order = 77)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpOriginalExchangeRate { get; set; }

	[JsonProperty("arpOverPaymentHeaderID", Order = 78)]
	public int arpOverPaymentHeaderID { get; set; }

	[JsonProperty("arpOverPaymentSessionID", Order = 79)]
	public int arpOverPaymentSessionID { get; set; }

	[JsonProperty("arpPaidDate", Order = 80)]
	public DateTime? arpPaidDate { get; set; }

	[JsonProperty("arpPaymentTermID", Order = 81)]
	[MaxLength(5)]
	public string arpPaymentTermID { get; set; }

	[JsonProperty("arpPlantDepartmentID", Order = 82)]
	[MaxLength(5)]
	public string arpPlantDepartmentID { get; set; }

	[JsonProperty("arpPlantID", Order = 83)]
	[MaxLength(5)]
	public string arpPlantID { get; set; }

	[JsonProperty("arpPointOfSaleTerminalID", Order = 84)]
	[MaxLength(5)]
	public string arpPointOfSaleTerminalID { get; set; }

	[JsonProperty("arpPostedDate", Order = 85)]
	public DateTime? arpPostedDate { get; set; }

	[JsonProperty("arpProjectID", Order = 86)]
	[MaxLength(10)]
	public string arpProjectID { get; set; }

	[JsonProperty("arpResellerCommissionAmount", Order = 87)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpResellerCommissionAmount { get; set; }

	[JsonProperty("arpResellerCommissionRate", Order = 88)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpResellerCommissionRate { get; set; }

	[JsonProperty("arpResellerContactID", Order = 89)]
	[MaxLength(5)]
	public string arpResellerContactID { get; set; }

	[JsonProperty("arpResellerLocationID", Order = 90)]
	[MaxLength(5)]
	public string arpResellerLocationID { get; set; }

	[JsonProperty("arpResellerOrganizationID", Order = 91)]
	[MaxLength(10)]
	public string arpResellerOrganizationID { get; set; }

	[JsonProperty("arpRetentionBalanceBase", Order = 92)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpRetentionBalanceBase { get; set; }

	[JsonProperty("arpRetentionBalanceForeign", Order = 93)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpRetentionBalanceForeign { get; set; }

	[JsonProperty("arpRetentionPaidBase", Order = 94)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpRetentionPaidBase { get; set; }

	[JsonProperty("arpRetentionPaidForeign", Order = 95)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpRetentionPaidForeign { get; set; }

	[JsonProperty("arpRetentionTotalBase", Order = 96)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpRetentionTotalBase { get; set; }

	[JsonProperty("arpRetentionTotalForeign", Order = 97)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpRetentionTotalForeign { get; set; }

	[JsonProperty("arpRowVersion", Order = 98)]
	public byte[] arpRowVersion { get; set; }

	[JsonProperty("arpSalesCommissionTotal", Order = 99)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpSalesCommissionTotal { get; set; }

	[JsonProperty("arpSalesGlAccountID", Order = 100)]
	[MaxLength(11)]
	public string arpSalesGlAccountID { get; set; }

	[JsonProperty("arpSecondFreightTaxAmtBase", Order = 101)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpSecondFreightTaxAmtBase { get; set; }

	[JsonProperty("arpSecondFreightTaxAmtForeign", Order = 102)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpSecondFreightTaxAmtForeign { get; set; }

	[JsonProperty("arpSecondFreightTaxCodeID", Order = 103)]
	[MaxLength(5)]
	public string arpSecondFreightTaxCodeID { get; set; }

	[JsonProperty("arpShipContactID", Order = 104)]
	[MaxLength(5)]
	public string arpShipContactID { get; set; }

	[JsonProperty("arpShipLocationID", Order = 105)]
	[MaxLength(5)]
	public string arpShipLocationID { get; set; }

	[JsonProperty("arpShipOrganizationID", Order = 106)]
	[Required(ErrorMessage = "arpShipOrganizationID is required.")]
	[MaxLength(10)]
	public string arpShipOrganizationID { get; set; }

	[JsonProperty("arpShippingMethodID", Order = 107)]
	[MaxLength(5)]
	public string arpShippingMethodID { get; set; }

	[JsonProperty("arpShippingPaymentTypeID", Order = 108)]
	[MaxLength(5)]
	public string arpShippingPaymentTypeID { get; set; }

	[JsonProperty("arpSplitPercentTotal", Order = 109)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpSplitPercentTotal { get; set; }

	[JsonProperty("arpStandardMessageID", Order = 110)]
	[MaxLength(10)]
	public string arpStandardMessageID { get; set; }

	[JsonProperty("arpTaxDate", Order = 111)]
	public DateTime? arpTaxDate { get; set; }

	[JsonProperty("arpTaxSubtotalBase", Order = 112)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpTaxSubtotalBase { get; set; }

	[JsonProperty("arpTaxSubtotalForeign", Order = 113)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpTaxSubtotalForeign { get; set; }

	[JsonProperty("arpTotalForResellerCommission", Order = 114)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpTotalForResellerCommission { get; set; }

	[JsonProperty("arpTotalForSalesCommission", Order = 115)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arpTotalForSalesCommission { get; set; }

	[JsonProperty("customFields", Order = 116)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
