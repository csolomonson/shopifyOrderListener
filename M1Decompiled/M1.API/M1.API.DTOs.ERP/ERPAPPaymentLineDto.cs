using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAPPaymentLineDto
{
	[JsonProperty("apnAdjustmentAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnAdjustmentAmount { get; set; }

	[JsonProperty("apnAdjustmentAmountForeign", Order = 2)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnAdjustmentAmountForeign { get; set; }

	[JsonProperty("apnAdjustmentGlAccountID", Order = 3)]
	[MaxLength(11)]
	public string apnAdjustmentGlAccountID { get; set; }

	[JsonProperty("apnApInvoiceID", Order = 4)]
	[MaxLength(10)]
	public string apnApInvoiceID { get; set; }

	[JsonProperty("apnApPaymentHeaderID", Order = 5)]
	[Required(ErrorMessage = "apnApPaymentHeaderID is required.")]
	public int apnApPaymentHeaderID { get; set; }

	[JsonProperty("apnApPaymentSessionID", Order = 6)]
	[Required(ErrorMessage = "apnApPaymentSessionID is required.")]
	public int apnApPaymentSessionID { get; set; }

	[JsonProperty("apnArInvoiceID", Order = 7)]
	[MaxLength(10)]
	public string apnArInvoiceID { get; set; }

	[JsonProperty("apnBankAccountID", Order = 8)]
	[MaxLength(5)]
	public string apnBankAccountID { get; set; }

	[JsonProperty("apnCreatedBy", Order = 9)]
	[MaxLength(20)]
	public string apnCreatedBy { get; set; }

	[JsonProperty("apnCreatedDate", Order = 10)]
	public DateTime? apnCreatedDate { get; set; }

	[JsonProperty("apnCurrencyRateID", Order = 11)]
	[MaxLength(5)]
	public string apnCurrencyRateID { get; set; }

	[JsonProperty("apnDescription", Order = 12)]
	[MaxLength(50)]
	public string apnDescription { get; set; }

	[JsonProperty("apnDiscountAmount", Order = 13)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnDiscountAmount { get; set; }

	[JsonProperty("apnDiscountAmountForeign", Order = 14)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnDiscountAmountForeign { get; set; }

	[JsonProperty("apnDiscountGlAccountID", Order = 15)]
	[MaxLength(11)]
	public string apnDiscountGlAccountID { get; set; }

	[JsonProperty("apnDiscountTaxAmount", Order = 16)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnDiscountTaxAmount { get; set; }

	[JsonProperty("apnDiscountTaxAmountForeign", Order = 17)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnDiscountTaxAmountForeign { get; set; }

	[JsonProperty("apnDiscountTaxCodeID", Order = 18)]
	[MaxLength(5)]
	public string apnDiscountTaxCodeID { get; set; }

	[JsonProperty("apnUniqueID", Order = 19)]
	public Guid apnUniqueID { get; set; }

	[JsonProperty("apnExchangeAmount", Order = 20)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnExchangeAmount { get; set; }

	[JsonProperty("apnExchangeGlAccountID", Order = 21)]
	[MaxLength(11)]
	public string apnExchangeGlAccountID { get; set; }

	[JsonProperty("apnExchangeRate", Order = 22)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnExchangeRate { get; set; }

	[JsonProperty("apnExpenseGlAccountID", Order = 23)]
	[Required(ErrorMessage = "apnExpenseGlAccountID is required.")]
	[MaxLength(11)]
	public string apnExpenseGlAccountID { get; set; }

	[JsonProperty("apnCompleted", Order = 24)]
	public bool apnCompleted { get; set; }

	[JsonProperty("apnCustomRate", Order = 25)]
	public bool apnCustomRate { get; set; }

	[JsonProperty("apnOverpayment", Order = 26)]
	public bool apnOverpayment { get; set; }

	[JsonProperty("apnPostedToGl", Order = 27)]
	public bool apnPostedToGl { get; set; }

	[JsonProperty("apnNonTaxReasonID", Order = 28)]
	[MaxLength(5)]
	public string apnNonTaxReasonID { get; set; }

	[JsonProperty("apnOriginalInvBalanceForeign", Order = 29)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnOriginalInvBalanceForeign { get; set; }

	[JsonProperty("apnOriginalInvoiceBalance", Order = 30)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnOriginalInvoiceBalance { get; set; }

	[JsonProperty("apnPaymentAmount", Order = 31)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnPaymentAmount { get; set; }

	[JsonProperty("apnPaymentAmountForeign", Order = 32)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnPaymentAmountForeign { get; set; }

	[JsonProperty("apnRetentionPayAmtForeign", Order = 33)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnRetentionPayAmtForeign { get; set; }

	[JsonProperty("apnRetentionPaymentAmount", Order = 34)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnRetentionPaymentAmount { get; set; }

	[JsonProperty("apnRowVersion", Order = 35)]
	public byte[] apnRowVersion { get; set; }

	[JsonProperty("apnSecondDiscountTaxAmount", Order = 36)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnSecondDiscountTaxAmount { get; set; }

	[JsonProperty("apnSecondDiscountTaxCodeID", Order = 37)]
	[MaxLength(5)]
	public string apnSecondDiscountTaxCodeID { get; set; }

	[JsonProperty("apnSecondDisTaxAmtForeign", Order = 38)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnSecondDisTaxAmtForeign { get; set; }

	[JsonProperty("apnSecondTaxAmount", Order = 39)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnSecondTaxAmount { get; set; }

	[JsonProperty("apnSecondTaxAmountForeign", Order = 40)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnSecondTaxAmountForeign { get; set; }

	[JsonProperty("apnSecondTaxCodeID", Order = 41)]
	[MaxLength(5)]
	public string apnSecondTaxCodeID { get; set; }

	[JsonProperty("apnApPaymentLineID", Order = 42)]
	[Required(ErrorMessage = "apnApPaymentLineID is required.")]
	public short apnApPaymentLineID { get; set; }

	[JsonProperty("apnTaxAmount", Order = 43)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnTaxAmount { get; set; }

	[JsonProperty("apnTaxAmountForeign", Order = 44)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnTaxAmountForeign { get; set; }

	[JsonProperty("apnTaxCodeID", Order = 45)]
	[MaxLength(5)]
	public string apnTaxCodeID { get; set; }

	[JsonProperty("apnTotalDiscountAmount", Order = 46)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnTotalDiscountAmount { get; set; }

	[JsonProperty("apnTotalDiscountAmtForeign", Order = 47)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnTotalDiscountAmtForeign { get; set; }

	[JsonProperty("apnUnrealisedExchangeAmt", Order = 48)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apnUnrealisedExchangeAmt { get; set; }

	[JsonProperty("apnUnrealisedExGlAccountID", Order = 49)]
	[MaxLength(11)]
	public string apnUnrealisedExGlAccountID { get; set; }

	[JsonProperty("customFields", Order = 50)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
