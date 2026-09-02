using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPBankEntryDto
{
	[JsonProperty("gleApPaymentHeaderID", Order = 1)]
	public int gleApPaymentHeaderID { get; set; }

	[JsonProperty("gleApPaymentSessionID", Order = 2)]
	public int gleApPaymentSessionID { get; set; }

	[JsonProperty("gleArPaymentHeaderID", Order = 3)]
	public int gleArPaymentHeaderID { get; set; }

	[JsonProperty("gleArPaymentSessionID", Order = 4)]
	public int gleArPaymentSessionID { get; set; }

	[JsonProperty("gleBankStatementID", Order = 5)]
	public int gleBankStatementID { get; set; }

	[JsonProperty("gleCashGlAccountID", Order = 6)]
	[MaxLength(11)]
	public string gleCashGlAccountID { get; set; }

	[JsonProperty("gleCreatedBy", Order = 7)]
	[MaxLength(20)]
	public string gleCreatedBy { get; set; }

	[JsonProperty("gleCreatedDate", Order = 8)]
	public DateTime? gleCreatedDate { get; set; }

	[JsonProperty("gleCurrencyRateID", Order = 9)]
	[MaxLength(5)]
	public string gleCurrencyRateID { get; set; }

	[JsonProperty("gleDescription", Order = 10)]
	[MaxLength(50)]
	public string gleDescription { get; set; }

	[JsonProperty("gleEftReferenceNumber", Order = 11)]
	[MaxLength(16)]
	public string gleEftReferenceNumber { get; set; }

	[JsonProperty("gleEntryType", Order = 12)]
	[Required(ErrorMessage = "gleEntryType is required.")]
	public byte gleEntryType { get; set; }

	[JsonProperty("gleUniqueID", Order = 13)]
	public Guid gleUniqueID { get; set; }

	[JsonProperty("gleExchangeRate", Order = 14)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gleExchangeRate { get; set; }

	[JsonProperty("gleGlAccountID", Order = 15)]
	[MaxLength(11)]
	public string gleGlAccountID { get; set; }

	[JsonProperty("gleGlFiscalYearID", Order = 16)]
	public short gleGlFiscalYearID { get; set; }

	[JsonProperty("gleGlFiscalYearPeriodID", Order = 17)]
	public byte gleGlFiscalYearPeriodID { get; set; }

	[JsonProperty("gleGlJournalID", Order = 18)]
	public int gleGlJournalID { get; set; }

	[JsonProperty("gleGlJournalLineID", Order = 19)]
	public int gleGlJournalLineID { get; set; }

	[JsonProperty("gleCleared", Order = 20)]
	public bool gleCleared { get; set; }

	[JsonProperty("gleCustomRate", Order = 21)]
	public bool gleCustomRate { get; set; }

	[JsonProperty("gleDoNotUpdateGl", Order = 22)]
	public bool gleDoNotUpdateGl { get; set; }

	[JsonProperty("glePostedToGl", Order = 23)]
	public bool glePostedToGl { get; set; }

	[JsonProperty("gleUnpresentedPayment", Order = 24)]
	public bool gleUnpresentedPayment { get; set; }

	[JsonProperty("gleNonTaxReasonID", Order = 25)]
	[MaxLength(5)]
	public string gleNonTaxReasonID { get; set; }

	[JsonProperty("gleOrganizationID", Order = 26)]
	[MaxLength(10)]
	public string gleOrganizationID { get; set; }

	[JsonProperty("gleOriginalAmount", Order = 27)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gleOriginalAmount { get; set; }

	[JsonProperty("gleOriginalAmountForeign", Order = 28)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gleOriginalAmountForeign { get; set; }

	[JsonProperty("glePaymentAmount", Order = 29)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glePaymentAmount { get; set; }

	[JsonProperty("glePaymentAmountForeign", Order = 30)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal glePaymentAmountForeign { get; set; }

	[JsonProperty("glePaymentDate", Order = 31)]
	public DateTime? glePaymentDate { get; set; }

	[JsonProperty("glePaymentNumber", Order = 32)]
	[Range(0, 999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int glePaymentNumber { get; set; }

	[JsonProperty("glePayrollHeaderID", Order = 33)]
	public int glePayrollHeaderID { get; set; }

	[JsonProperty("glePayrollSessionID", Order = 34)]
	public int glePayrollSessionID { get; set; }

	[JsonProperty("glePayType", Order = 35)]
	public byte glePayType { get; set; }

	[JsonProperty("glePresentedDate", Order = 36)]
	public DateTime? glePresentedDate { get; set; }

	[JsonProperty("gleRowVersion", Order = 37)]
	public byte[] gleRowVersion { get; set; }

	[JsonProperty("gleBankEntryID", Order = 38)]
	[Required(ErrorMessage = "gleBankEntryID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int gleBankEntryID { get; set; }

	[JsonProperty("gleSource", Order = 39)]
	[Required(ErrorMessage = "gleSource is required.")]
	public byte gleSource { get; set; }

	[JsonProperty("gleTaxAmount", Order = 40)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gleTaxAmount { get; set; }

	[JsonProperty("gleTaxAmountForeign", Order = 41)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gleTaxAmountForeign { get; set; }

	[JsonProperty("gleTaxCodeID", Order = 42)]
	[MaxLength(5)]
	public string gleTaxCodeID { get; set; }

	[JsonProperty("gleTransactionDate", Order = 43)]
	public DateTime? gleTransactionDate { get; set; }

	[JsonProperty("gleVarianceAmount", Order = 44)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gleVarianceAmount { get; set; }

	[JsonProperty("gleVarianceAmountForeign", Order = 45)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal gleVarianceAmountForeign { get; set; }

	[JsonProperty("customFields", Order = 46)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
