using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPARPaymentHeaderDto
{
	[JsonProperty("artArGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string artArGlAccountID { get; set; }

	[JsonProperty("artArInvoiceContactID", Order = 2)]
	[MaxLength(5)]
	public string artArInvoiceContactID { get; set; }

	[JsonProperty("artArInvoiceLocationID", Order = 3)]
	[MaxLength(5)]
	public string artArInvoiceLocationID { get; set; }

	[JsonProperty("artArPaymentSessionID", Order = 4)]
	[Required(ErrorMessage = "artArPaymentSessionID is required.")]
	public int artArPaymentSessionID { get; set; }

	[JsonProperty("artBankAccountName", Order = 5)]
	[MaxLength(50)]
	public string artBankAccountName { get; set; }

	[JsonProperty("artBankAccountNumber", Order = 6)]
	[MaxLength(24)]
	public string artBankAccountNumber { get; set; }

	[JsonProperty("artBankInitials", Order = 7)]
	[MaxLength(3)]
	public string artBankInitials { get; set; }

	[JsonProperty("artBsbNumber", Order = 8)]
	[MaxLength(10)]
	public string artBsbNumber { get; set; }

	[JsonProperty("artCashGlAccountID", Order = 9)]
	[MaxLength(11)]
	public string artCashGlAccountID { get; set; }

	[JsonProperty("artCreatedBy", Order = 10)]
	[MaxLength(20)]
	public string artCreatedBy { get; set; }

	[JsonProperty("artCreatedCreditArInvoiceID", Order = 11)]
	[MaxLength(10)]
	public string artCreatedCreditArInvoiceID { get; set; }

	[JsonProperty("artCreatedDate", Order = 12)]
	public DateTime? artCreatedDate { get; set; }

	[JsonProperty("artCreditArInvoiceID", Order = 13)]
	[MaxLength(10)]
	public string artCreditArInvoiceID { get; set; }

	[JsonProperty("artCustomerOrganizationID", Order = 14)]
	[MaxLength(10)]
	public string artCustomerOrganizationID { get; set; }

	[JsonProperty("artCustomerPaymentNumber", Order = 15)]
	[MaxLength(10)]
	public string artCustomerPaymentNumber { get; set; }

	[JsonProperty("artDescription", Order = 16)]
	[MaxLength(50)]
	public string artDescription { get; set; }

	[JsonProperty("artUniqueID", Order = 17)]
	public Guid artUniqueID { get; set; }

	[JsonProperty("artExchangeAmount", Order = 18)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal artExchangeAmount { get; set; }

	[JsonProperty("artExchangeGlAccountID", Order = 19)]
	[MaxLength(11)]
	public string artExchangeGlAccountID { get; set; }

	[JsonProperty("artGlAccountID", Order = 20)]
	[MaxLength(11)]
	public string artGlAccountID { get; set; }

	[JsonProperty("artGlFiscalYearID", Order = 21)]
	public short artGlFiscalYearID { get; set; }

	[JsonProperty("artGlFiscalYearPeriodID", Order = 22)]
	public byte artGlFiscalYearPeriodID { get; set; }

	[JsonProperty("artAvalaraTaxCalculated", Order = 23)]
	public bool artAvalaraTaxCalculated { get; set; }

	[JsonProperty("artNet1PaymentProcessed", Order = 24)]
	public bool artNet1PaymentProcessed { get; set; }

	[JsonProperty("artOpenPaymentLoad", Order = 25)]
	public bool artOpenPaymentLoad { get; set; }

	[JsonProperty("artPostedToGl", Order = 26)]
	public bool artPostedToGl { get; set; }

	[JsonProperty("artVoidedPayment", Order = 27)]
	public bool artVoidedPayment { get; set; }

	[JsonProperty("artLongDescriptionRtf", Order = 28)]
	public string artLongDescriptionRtf { get; set; }

	[JsonProperty("artLongDescriptionText", Order = 29)]
	public string artLongDescriptionText { get; set; }

	[JsonProperty("artNonTaxReasonID", Order = 30)]
	[MaxLength(5)]
	public string artNonTaxReasonID { get; set; }

	[JsonProperty("artPaymentMethod", Order = 31)]
	public byte artPaymentMethod { get; set; }

	[JsonProperty("artReceiptAmount", Order = 32)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal artReceiptAmount { get; set; }

	[JsonProperty("artReceiptAmountForeign", Order = 33)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal artReceiptAmountForeign { get; set; }

	[JsonProperty("artReceiptDate", Order = 34)]
	public DateTime? artReceiptDate { get; set; }

	[JsonProperty("artReceiptType", Order = 35)]
	[Required(ErrorMessage = "artReceiptType is required.")]
	public byte artReceiptType { get; set; }

	[JsonProperty("artRowVersion", Order = 36)]
	public byte[] artRowVersion { get; set; }

	[JsonProperty("artSecondTaxAmount", Order = 37)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal artSecondTaxAmount { get; set; }

	[JsonProperty("artSecondTaxAmountForeign", Order = 38)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal artSecondTaxAmountForeign { get; set; }

	[JsonProperty("artSecondTaxCodeID", Order = 39)]
	[MaxLength(5)]
	public string artSecondTaxCodeID { get; set; }

	[JsonProperty("artArPaymentHeaderID", Order = 40)]
	[Required(ErrorMessage = "artArPaymentHeaderID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int artArPaymentHeaderID { get; set; }

	[JsonProperty("artShowAllInvoices", Order = 41)]
	public bool artShowAllInvoices { get; set; }

	[JsonProperty("artTaxAmount", Order = 42)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal artTaxAmount { get; set; }

	[JsonProperty("artTaxAmountForeign", Order = 43)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal artTaxAmountForeign { get; set; }

	[JsonProperty("artTaxCodeID", Order = 44)]
	[MaxLength(5)]
	public string artTaxCodeID { get; set; }

	[JsonProperty("artVoidArPaymentHeaderId", Order = 45)]
	public int artVoidArPaymentHeaderId { get; set; }

	[JsonProperty("artVoidArPaymentSessionID", Order = 46)]
	public int artVoidArPaymentSessionID { get; set; }

	[JsonProperty("customFields", Order = 47)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
