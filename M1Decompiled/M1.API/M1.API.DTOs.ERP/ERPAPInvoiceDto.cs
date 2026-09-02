using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAPInvoiceDto
{
	[JsonProperty("appApGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string appApGlAccountID { get; set; }

	[JsonProperty("appApInvoiceContactID", Order = 2)]
	[MaxLength(5)]
	public string appApInvoiceContactID { get; set; }

	[JsonProperty("appApInvoiceLocationID", Order = 3)]
	[MaxLength(5)]
	public string appApInvoiceLocationID { get; set; }

	[JsonProperty("appApInvoiceID", Order = 4)]
	[Required(ErrorMessage = "appApInvoiceID is required.")]
	[MaxLength(10)]
	public string appApInvoiceID { get; set; }

	[JsonProperty("appCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string appCreatedBy { get; set; }

	[JsonProperty("appCreatedDate", Order = 6)]
	public DateTime? appCreatedDate { get; set; }

	[JsonProperty("appCreditApInvoiceID", Order = 7)]
	[MaxLength(10)]
	public string appCreditApInvoiceID { get; set; }

	[JsonProperty("appCreditDate", Order = 8)]
	public DateTime? appCreditDate { get; set; }

	[JsonProperty("appCreditReasonID", Order = 9)]
	[MaxLength(5)]
	public string appCreditReasonID { get; set; }

	[JsonProperty("appCurrencyRateID", Order = 10)]
	[MaxLength(5)]
	public string appCurrencyRateID { get; set; }

	[JsonProperty("appDiscountAmountBase", Order = 11)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appDiscountAmountBase { get; set; }

	[JsonProperty("appDiscountAmountForeign", Order = 12)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appDiscountAmountForeign { get; set; }

	[JsonProperty("appDiscountDueDate", Order = 13)]
	public DateTime? appDiscountDueDate { get; set; }

	[JsonProperty("appDueDate", Order = 14)]
	[Required(ErrorMessage = "appDueDate is required.")]
	public DateTime? appDueDate { get; set; }

	[JsonProperty("appUniqueID", Order = 15)]
	public Guid appUniqueID { get; set; }

	[JsonProperty("appExchangeRate", Order = 16)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appExchangeRate { get; set; }

	[JsonProperty("appFreightAmountBase", Order = 17)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appFreightAmountBase { get; set; }

	[JsonProperty("appFreightAmountForeign", Order = 18)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appFreightAmountForeign { get; set; }

	[JsonProperty("appFreightGlAccountID", Order = 19)]
	[MaxLength(11)]
	public string appFreightGlAccountID { get; set; }

	[JsonProperty("appFreightTaxAmountBase", Order = 20)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appFreightTaxAmountBase { get; set; }

	[JsonProperty("appFreightTaxAmountForeign", Order = 21)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appFreightTaxAmountForeign { get; set; }

	[JsonProperty("appFreightTaxCodeID", Order = 22)]
	[MaxLength(5)]
	public string appFreightTaxCodeID { get; set; }

	[JsonProperty("appGlFiscalYearID", Order = 23)]
	[Required(ErrorMessage = "appGlFiscalYearID is required.")]
	public short appGlFiscalYearID { get; set; }

	[JsonProperty("appGlFiscalYearPeriodID", Order = 24)]
	[Required(ErrorMessage = "appGlFiscalYearPeriodID is required.")]
	public byte appGlFiscalYearPeriodID { get; set; }

	[JsonProperty("appInvoiceBalanceBase", Order = 25)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appInvoiceBalanceBase { get; set; }

	[JsonProperty("appInvoiceBalanceForeign", Order = 26)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appInvoiceBalanceForeign { get; set; }

	[JsonProperty("appInvoiceCommentsRTF", Order = 27)]
	[MaxLength(50)]
	public string appInvoiceCommentsRTF { get; set; }

	[JsonProperty("appInvoiceCommentsText", Order = 28)]
	[MaxLength(50)]
	public string appInvoiceCommentsText { get; set; }

	[JsonProperty("appInvoiceDate", Order = 29)]
	[Required(ErrorMessage = "appInvoiceDate is required.")]
	public DateTime? appInvoiceDate { get; set; }

	[JsonProperty("appInvoiceDescription", Order = 30)]
	[MaxLength(50)]
	public string appInvoiceDescription { get; set; }

	[JsonProperty("appInvoiceSubtotalBase", Order = 31)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appInvoiceSubtotalBase { get; set; }

	[JsonProperty("appInvoiceSubtotalForeign", Order = 32)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appInvoiceSubtotalForeign { get; set; }

	[JsonProperty("appInvoiceTaxAmountBase", Order = 33)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appInvoiceTaxAmountBase { get; set; }

	[JsonProperty("appInvoiceTaxAmountForeign", Order = 34)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appInvoiceTaxAmountForeign { get; set; }

	[JsonProperty("appInvoiceTotalBase", Order = 35)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appInvoiceTotalBase { get; set; }

	[JsonProperty("appInvoiceTotalForeign", Order = 36)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appInvoiceTotalForeign { get; set; }

	[JsonProperty("appInvoiceType", Order = 37)]
	[Required(ErrorMessage = "appInvoiceType is required.")]
	public byte appInvoiceType { get; set; }

	[JsonProperty("appCustomRate", Order = 38)]
	public bool appCustomRate { get; set; }

	[JsonProperty("appOnHold", Order = 39)]
	public bool appOnHold { get; set; }

	[JsonProperty("appOpenInvoiceLoad", Order = 40)]
	public bool appOpenInvoiceLoad { get; set; }

	[JsonProperty("appOverpayment", Order = 41)]
	public bool appOverpayment { get; set; }

	[JsonProperty("appPaidComplete", Order = 42)]
	public bool appPaidComplete { get; set; }

	[JsonProperty("appPostedToGl", Order = 43)]
	public bool appPostedToGl { get; set; }

	[JsonProperty("appTaxReportable", Order = 44)]
	public bool appTaxReportable { get; set; }

	[JsonProperty("appOriginalExchangeRate", Order = 45)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appOriginalExchangeRate { get; set; }

	[JsonProperty("appOverPaymentHeaderID", Order = 46)]
	public int appOverPaymentHeaderID { get; set; }

	[JsonProperty("appOverPaymentSessionID", Order = 47)]
	public int appOverPaymentSessionID { get; set; }

	[JsonProperty("appPaidDate", Order = 48)]
	public DateTime? appPaidDate { get; set; }

	[JsonProperty("appPaymentTermID", Order = 49)]
	[MaxLength(5)]
	public string appPaymentTermID { get; set; }

	[JsonProperty("appPlantDepartmentID", Order = 50)]
	[MaxLength(5)]
	public string appPlantDepartmentID { get; set; }

	[JsonProperty("appPlantID", Order = 51)]
	[MaxLength(5)]
	public string appPlantID { get; set; }

	[JsonProperty("appPostedDate", Order = 52)]
	public DateTime? appPostedDate { get; set; }

	[JsonProperty("appProjectID", Order = 53)]
	[MaxLength(10)]
	public string appProjectID { get; set; }

	[JsonProperty("appRetentionBalanceBase", Order = 54)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appRetentionBalanceBase { get; set; }

	[JsonProperty("appRetentionBalanceForeign", Order = 55)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appRetentionBalanceForeign { get; set; }

	[JsonProperty("appRetentionTotalBase", Order = 56)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appRetentionTotalBase { get; set; }

	[JsonProperty("appRetentionTotalForeign", Order = 57)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appRetentionTotalForeign { get; set; }

	[JsonProperty("appRowVersion", Order = 58)]
	public byte[] appRowVersion { get; set; }

	[JsonProperty("appSecondFreightTaxAmtBase", Order = 59)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appSecondFreightTaxAmtBase { get; set; }

	[JsonProperty("appSecondFreightTaxAmtForeign", Order = 60)]
	[Range(0.0, 9999999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal appSecondFreightTaxAmtForeign { get; set; }

	[JsonProperty("appSecondFreightTaxCodeID", Order = 61)]
	[MaxLength(5)]
	public string appSecondFreightTaxCodeID { get; set; }

	[JsonProperty("appSupplierInvoiceNumber", Order = 62)]
	[MaxLength(30)]
	public string appSupplierInvoiceNumber { get; set; }

	[JsonProperty("appSupplierOrganizationID", Order = 63)]
	[Required(ErrorMessage = "appSupplierOrganizationID is required.")]
	[MaxLength(10)]
	public string appSupplierOrganizationID { get; set; }

	[JsonProperty("customFields", Order = 64)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
