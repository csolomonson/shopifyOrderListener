using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAPPaymentSessionDto
{
	[JsonProperty("apsApGlAccountID", Order = 1)]
	[Required(ErrorMessage = "apsApGlAccountID is required.")]
	[MaxLength(11)]
	public string apsApGlAccountID { get; set; }

	[JsonProperty("apsArGlAccountID", Order = 2)]
	[MaxLength(11)]
	public string apsArGlAccountID { get; set; }

	[JsonProperty("apsBankAccountID", Order = 3)]
	[MaxLength(5)]
	public string apsBankAccountID { get; set; }

	[JsonProperty("apsCashGlAccountID", Order = 4)]
	[MaxLength(11)]
	public string apsCashGlAccountID { get; set; }

	[JsonProperty("apsCompletedDate", Order = 5)]
	public DateTime? apsCompletedDate { get; set; }

	[JsonProperty("apsCreatedBy", Order = 6)]
	[MaxLength(20)]
	public string apsCreatedBy { get; set; }

	[JsonProperty("apsCreatedDate", Order = 7)]
	public DateTime? apsCreatedDate { get; set; }

	[JsonProperty("apsCurrencyRateID", Order = 8)]
	[MaxLength(5)]
	public string apsCurrencyRateID { get; set; }

	[JsonProperty("apsEftDescription", Order = 9)]
	[MaxLength(20)]
	public string apsEftDescription { get; set; }

	[JsonProperty("apsEftReferenceNumber", Order = 10)]
	[MaxLength(16)]
	public string apsEftReferenceNumber { get; set; }

	[JsonProperty("apsEftSettlementDate", Order = 11)]
	public DateTime? apsEftSettlementDate { get; set; }

	[JsonProperty("apsUniqueID", Order = 12)]
	public Guid apsUniqueID { get; set; }

	[JsonProperty("apsExchangeRate", Order = 13)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apsExchangeRate { get; set; }

	[JsonProperty("apsGlFiscalYearID", Order = 14)]
	[Required(ErrorMessage = "apsGlFiscalYearID is required.")]
	public short apsGlFiscalYearID { get; set; }

	[JsonProperty("apsGlFiscalYearPeriodID", Order = 15)]
	[Required(ErrorMessage = "apsGlFiscalYearPeriodID is required.")]
	public byte apsGlFiscalYearPeriodID { get; set; }

	[JsonProperty("apsCompleted", Order = 16)]
	public bool apsCompleted { get; set; }

	[JsonProperty("apsCustomRate", Order = 17)]
	public bool apsCustomRate { get; set; }

	[JsonProperty("apsOpenPaymentLoad", Order = 18)]
	public bool apsOpenPaymentLoad { get; set; }

	[JsonProperty("apsPaymentsPrinted", Order = 19)]
	public bool apsPaymentsPrinted { get; set; }

	[JsonProperty("apsPostedToGl", Order = 20)]
	public bool apsPostedToGl { get; set; }

	[JsonProperty("apsPaymentAmount", Order = 21)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apsPaymentAmount { get; set; }

	[JsonProperty("apsPaymentAmountForeign", Order = 22)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal apsPaymentAmountForeign { get; set; }

	[JsonProperty("apsPaymentDate", Order = 23)]
	[Required(ErrorMessage = "apsPaymentDate is required.")]
	public DateTime? apsPaymentDate { get; set; }

	[JsonProperty("apsPlantDepartmentID", Order = 24)]
	[MaxLength(5)]
	public string apsPlantDepartmentID { get; set; }

	[JsonProperty("apsPlantID", Order = 25)]
	[MaxLength(5)]
	public string apsPlantID { get; set; }

	[JsonProperty("apsPostedDate", Order = 26)]
	public DateTime? apsPostedDate { get; set; }

	[JsonProperty("apsRowVersion", Order = 27)]
	public byte[] apsRowVersion { get; set; }

	[JsonProperty("apsApPaymentSessionID", Order = 28)]
	[Required(ErrorMessage = "apsApPaymentSessionID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int apsApPaymentSessionID { get; set; }

	[JsonProperty("apsSessionType", Order = 29)]
	[Required(ErrorMessage = "apsSessionType is required.")]
	public byte apsSessionType { get; set; }

	[JsonProperty("customFields", Order = 30)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
