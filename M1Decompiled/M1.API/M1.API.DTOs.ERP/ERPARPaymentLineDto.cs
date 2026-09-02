using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPARPaymentLineDto
{
	[JsonProperty("arnAdjustmentAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnAdjustmentAmount { get; set; }

	[JsonProperty("arnAdjustmentAmountForeign", Order = 2)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnAdjustmentAmountForeign { get; set; }

	[JsonProperty("arnAdjustmentGlAccountID", Order = 3)]
	[MaxLength(11)]
	public string arnAdjustmentGlAccountID { get; set; }

	[JsonProperty("arnApInvoiceID", Order = 4)]
	[MaxLength(10)]
	public string arnApInvoiceID { get; set; }

	[JsonProperty("arnArInvoiceID", Order = 5)]
	[MaxLength(10)]
	public string arnArInvoiceID { get; set; }

	[JsonProperty("arnArPaymentEPayID", Order = 6)]
	public int arnArPaymentEPayID { get; set; }

	[JsonProperty("arnArPaymentHeaderID", Order = 7)]
	[Required(ErrorMessage = "arnArPaymentHeaderID is required.")]
	public int arnArPaymentHeaderID { get; set; }

	[JsonProperty("arnArPaymentSessionID", Order = 8)]
	[Required(ErrorMessage = "arnArPaymentSessionID is required.")]
	public int arnArPaymentSessionID { get; set; }

	[JsonProperty("arnCreatedBy", Order = 9)]
	[MaxLength(20)]
	public string arnCreatedBy { get; set; }

	[JsonProperty("arnCreatedDate", Order = 10)]
	public DateTime? arnCreatedDate { get; set; }

	[JsonProperty("arnDiscountAmount", Order = 11)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnDiscountAmount { get; set; }

	[JsonProperty("arnDiscountAmountForeign", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnDiscountAmountForeign { get; set; }

	[JsonProperty("arnDiscountGlAccountID", Order = 13)]
	[MaxLength(11)]
	public string arnDiscountGlAccountID { get; set; }

	[JsonProperty("arnDiscountTaxAmount", Order = 14)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnDiscountTaxAmount { get; set; }

	[JsonProperty("arnDiscountTaxAmountForeign", Order = 15)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnDiscountTaxAmountForeign { get; set; }

	[JsonProperty("arnDiscountTaxCodeID", Order = 16)]
	[MaxLength(5)]
	public string arnDiscountTaxCodeID { get; set; }

	[JsonProperty("arnUniqueID", Order = 17)]
	public Guid arnUniqueID { get; set; }

	[JsonProperty("arnExchangeAmount", Order = 18)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnExchangeAmount { get; set; }

	[JsonProperty("arnExchangeGlAccountID", Order = 19)]
	[MaxLength(11)]
	public string arnExchangeGlAccountID { get; set; }

	[JsonProperty("arnAvalaraTaxCalculated", Order = 20)]
	public bool arnAvalaraTaxCalculated { get; set; }

	[JsonProperty("arnOverpayment", Order = 21)]
	public bool arnOverpayment { get; set; }

	[JsonProperty("arnPostedToGl", Order = 22)]
	public bool arnPostedToGl { get; set; }

	[JsonProperty("arnNonTaxReasonID", Order = 23)]
	[MaxLength(5)]
	public string arnNonTaxReasonID { get; set; }

	[JsonProperty("arnOriginalInvBalanceForeign", Order = 24)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnOriginalInvBalanceForeign { get; set; }

	[JsonProperty("arnOriginalInvoiceBalance", Order = 25)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnOriginalInvoiceBalance { get; set; }

	[JsonProperty("arnPaymentAmount", Order = 26)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnPaymentAmount { get; set; }

	[JsonProperty("arnPaymentAmountForeign", Order = 27)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnPaymentAmountForeign { get; set; }

	[JsonProperty("arnRetentionPayAmtForeign", Order = 28)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnRetentionPayAmtForeign { get; set; }

	[JsonProperty("arnRetentionPaymentAmount", Order = 29)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnRetentionPaymentAmount { get; set; }

	[JsonProperty("arnRowVersion", Order = 30)]
	public byte[] arnRowVersion { get; set; }

	[JsonProperty("arnSecondDiscountTaxAmount", Order = 31)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnSecondDiscountTaxAmount { get; set; }

	[JsonProperty("arnSecondDiscountTaxCodeID", Order = 32)]
	[MaxLength(5)]
	public string arnSecondDiscountTaxCodeID { get; set; }

	[JsonProperty("arnSecondDisTaxAmtForeign", Order = 33)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnSecondDisTaxAmtForeign { get; set; }

	[JsonProperty("arnSecondTaxAmount", Order = 34)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnSecondTaxAmount { get; set; }

	[JsonProperty("arnSecondTaxAmountForeign", Order = 35)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnSecondTaxAmountForeign { get; set; }

	[JsonProperty("arnSecondTaxCodeID", Order = 36)]
	[MaxLength(5)]
	public string arnSecondTaxCodeID { get; set; }

	[JsonProperty("arnArPaymentLineID", Order = 37)]
	[Required(ErrorMessage = "arnArPaymentLineID is required.")]
	public short arnArPaymentLineID { get; set; }

	[JsonProperty("arnTaxAmount", Order = 38)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnTaxAmount { get; set; }

	[JsonProperty("arnTaxAmountForeign", Order = 39)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnTaxAmountForeign { get; set; }

	[JsonProperty("arnTaxCodeID", Order = 40)]
	[MaxLength(5)]
	public string arnTaxCodeID { get; set; }

	[JsonProperty("arnTotalDiscountAmount", Order = 41)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnTotalDiscountAmount { get; set; }

	[JsonProperty("arnTotalDiscountAmtForeign", Order = 42)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnTotalDiscountAmtForeign { get; set; }

	[JsonProperty("arnUnrealisedExchangeAmt", Order = 43)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arnUnrealisedExchangeAmt { get; set; }

	[JsonProperty("arnUnrealisedExGlAccountID", Order = 44)]
	[MaxLength(11)]
	public string arnUnrealisedExGlAccountID { get; set; }

	[JsonProperty("customFields", Order = 45)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
