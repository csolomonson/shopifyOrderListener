using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAPPaymentHeaderDto
{
	[JsonProperty("aptApInvoiceContactID", Order = 1)]
	[MaxLength(5)]
	public string aptApInvoiceContactID { get; set; }

	[JsonProperty("aptApInvoiceLocationID", Order = 2)]
	[MaxLength(5)]
	public string aptApInvoiceLocationID { get; set; }

	[JsonProperty("aptApPaymentSessionID", Order = 3)]
	[Required(ErrorMessage = "aptApPaymentSessionID is required.")]
	public int aptApPaymentSessionID { get; set; }

	[JsonProperty("aptBankAccountName", Order = 4)]
	[MaxLength(50)]
	public string aptBankAccountName { get; set; }

	[JsonProperty("aptBankAccountNumber", Order = 5)]
	[MaxLength(24)]
	public string aptBankAccountNumber { get; set; }

	[JsonProperty("aptBankAccountType", Order = 6)]
	[MaxLength(2)]
	public string aptBankAccountType { get; set; }

	[JsonProperty("aptBankInitials", Order = 7)]
	[MaxLength(3)]
	public string aptBankInitials { get; set; }

	[JsonProperty("aptBic", Order = 8)]
	[MaxLength(50)]
	public string aptBic { get; set; }

	[JsonProperty("aptBsbNumber", Order = 9)]
	[MaxLength(10)]
	public string aptBsbNumber { get; set; }

	[JsonProperty("aptCashGlAccountID", Order = 10)]
	[MaxLength(11)]
	public string aptCashGlAccountID { get; set; }

	[JsonProperty("aptCreatedBy", Order = 11)]
	[MaxLength(20)]
	public string aptCreatedBy { get; set; }

	[JsonProperty("aptCreatedCreditApInvoiceID", Order = 12)]
	[MaxLength(10)]
	public string aptCreatedCreditApInvoiceID { get; set; }

	[JsonProperty("aptCreatedDate", Order = 13)]
	public DateTime? aptCreatedDate { get; set; }

	[JsonProperty("aptCreditApInvoiceID", Order = 14)]
	[MaxLength(10)]
	public string aptCreditApInvoiceID { get; set; }

	[JsonProperty("aptEftCode", Order = 15)]
	[MaxLength(12)]
	public string aptEftCode { get; set; }

	[JsonProperty("aptEftDescription", Order = 16)]
	[MaxLength(20)]
	public string aptEftDescription { get; set; }

	[JsonProperty("aptEftNumber", Order = 17)]
	[Range(0, 999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int aptEftNumber { get; set; }

	[JsonProperty("aptEftParticulars", Order = 18)]
	[MaxLength(12)]
	public string aptEftParticulars { get; set; }

	[JsonProperty("aptUniqueID", Order = 19)]
	public Guid aptUniqueID { get; set; }

	[JsonProperty("aptExchangeAmount", Order = 20)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aptExchangeAmount { get; set; }

	[JsonProperty("aptExchangeGlAccountID", Order = 21)]
	[MaxLength(11)]
	public string aptExchangeGlAccountID { get; set; }

	[JsonProperty("aptForm1099Box", Order = 22)]
	public byte aptForm1099Box { get; set; }

	[JsonProperty("aptGlFiscalYearID", Order = 23)]
	public short aptGlFiscalYearID { get; set; }

	[JsonProperty("aptGlFiscalYearPeriodID", Order = 24)]
	public byte aptGlFiscalYearPeriodID { get; set; }

	[JsonProperty("aptIban", Order = 25)]
	[MaxLength(50)]
	public string aptIban { get; set; }

	[JsonProperty("aptCompleted", Order = 26)]
	public bool aptCompleted { get; set; }

	[JsonProperty("aptManualPayment", Order = 27)]
	public bool aptManualPayment { get; set; }

	[JsonProperty("aptOpenPaymentLoad", Order = 28)]
	public bool aptOpenPaymentLoad { get; set; }

	[JsonProperty("aptOverpayment", Order = 29)]
	public bool aptOverpayment { get; set; }

	[JsonProperty("aptPostedToGl", Order = 30)]
	public bool aptPostedToGl { get; set; }

	[JsonProperty("aptSuppressVoid", Order = 31)]
	public bool aptSuppressVoid { get; set; }

	[JsonProperty("aptTaxReportable", Order = 32)]
	public bool aptTaxReportable { get; set; }

	[JsonProperty("aptVoidedPayment", Order = 33)]
	public bool aptVoidedPayment { get; set; }

	[JsonProperty("aptLongDescriptionRtf", Order = 34)]
	public string aptLongDescriptionRtf { get; set; }

	[JsonProperty("aptLongDescriptionText", Order = 35)]
	public string aptLongDescriptionText { get; set; }

	[JsonProperty("aptPaymentAmount", Order = 36)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aptPaymentAmount { get; set; }

	[JsonProperty("aptPaymentAmountForeign", Order = 37)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal aptPaymentAmountForeign { get; set; }

	[JsonProperty("aptPaymentDate", Order = 38)]
	public DateTime? aptPaymentDate { get; set; }

	[JsonProperty("aptPaymentMemo", Order = 39)]
	[MaxLength(50)]
	public string aptPaymentMemo { get; set; }

	[JsonProperty("aptPaymentNumber", Order = 40)]
	[Range(0, 999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int aptPaymentNumber { get; set; }

	[JsonProperty("aptPaymentType", Order = 41)]
	[Required(ErrorMessage = "aptPaymentType is required.")]
	public byte aptPaymentType { get; set; }

	[JsonProperty("aptRecurringPaymentID", Order = 42)]
	public int aptRecurringPaymentID { get; set; }

	[JsonProperty("aptRowVersion", Order = 43)]
	public byte[] aptRowVersion { get; set; }

	[JsonProperty("aptApPaymentHeaderID", Order = 44)]
	[Required(ErrorMessage = "aptApPaymentHeaderID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int aptApPaymentHeaderID { get; set; }

	[JsonProperty("aptShowAllInvoices", Order = 45)]
	public bool aptShowAllInvoices { get; set; }

	[JsonProperty("aptSupplierOrganizationID", Order = 46)]
	[MaxLength(10)]
	public string aptSupplierOrganizationID { get; set; }

	[JsonProperty("aptVoidApPaymentHeaderID", Order = 47)]
	public int aptVoidApPaymentHeaderID { get; set; }

	[JsonProperty("aptVoidApPaymentSessionID", Order = 48)]
	public int aptVoidApPaymentSessionID { get; set; }

	[JsonProperty("customFields", Order = 49)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
