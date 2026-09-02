using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPARPaymentSessionDto
{
	[JsonProperty("arsApDiscountGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string arsApDiscountGlAccountID { get; set; }

	[JsonProperty("arsApGlAccountID", Order = 2)]
	[MaxLength(11)]
	public string arsApGlAccountID { get; set; }

	[JsonProperty("arsArGlAccountID", Order = 3)]
	[Required(ErrorMessage = "arsArGlAccountID is required.")]
	[MaxLength(11)]
	public string arsArGlAccountID { get; set; }

	[JsonProperty("arsBankAccountID", Order = 4)]
	[MaxLength(5)]
	public string arsBankAccountID { get; set; }

	[JsonProperty("arsCashGlAccountID", Order = 5)]
	[MaxLength(11)]
	public string arsCashGlAccountID { get; set; }

	[JsonProperty("arsCreatedBy", Order = 6)]
	[MaxLength(20)]
	public string arsCreatedBy { get; set; }

	[JsonProperty("arsCreatedDate", Order = 7)]
	public DateTime? arsCreatedDate { get; set; }

	[JsonProperty("arsCurrencyRateID", Order = 8)]
	[MaxLength(5)]
	public string arsCurrencyRateID { get; set; }

	[JsonProperty("arsDepositAmount", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arsDepositAmount { get; set; }

	[JsonProperty("arsDepositAmountForeign", Order = 10)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arsDepositAmountForeign { get; set; }

	[JsonProperty("arsDiscountGlAccountID", Order = 11)]
	[MaxLength(11)]
	public string arsDiscountGlAccountID { get; set; }

	[JsonProperty("arsUniqueID", Order = 12)]
	public Guid arsUniqueID { get; set; }

	[JsonProperty("arsExchangeRate", Order = 13)]
	[Range(0.0, 9999999.999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arsExchangeRate { get; set; }

	[JsonProperty("arsGlFiscalYearID", Order = 14)]
	[Required(ErrorMessage = "arsGlFiscalYearID is required.")]
	public short arsGlFiscalYearID { get; set; }

	[JsonProperty("arsGlFiscalYearPeriodID", Order = 15)]
	[Required(ErrorMessage = "arsGlFiscalYearPeriodID is required.")]
	public byte arsGlFiscalYearPeriodID { get; set; }

	[JsonProperty("arsAvalaraTaxCalculated", Order = 16)]
	public bool arsAvalaraTaxCalculated { get; set; }

	[JsonProperty("arsCustomRate", Order = 17)]
	public bool arsCustomRate { get; set; }

	[JsonProperty("arsGroupBySettlement", Order = 18)]
	public bool arsGroupBySettlement { get; set; }

	[JsonProperty("arsOpenPaymentLoad", Order = 19)]
	public bool arsOpenPaymentLoad { get; set; }

	[JsonProperty("arsPostedToGl", Order = 20)]
	public bool arsPostedToGl { get; set; }

	[JsonProperty("arsPlantDepartmentID", Order = 21)]
	[MaxLength(5)]
	public string arsPlantDepartmentID { get; set; }

	[JsonProperty("arsPlantID", Order = 22)]
	[MaxLength(5)]
	public string arsPlantID { get; set; }

	[JsonProperty("arsPostedDate", Order = 23)]
	public DateTime? arsPostedDate { get; set; }

	[JsonProperty("arsReceiptDate", Order = 24)]
	[Required(ErrorMessage = "arsReceiptDate is required.")]
	public DateTime? arsReceiptDate { get; set; }

	[JsonProperty("arsRowVersion", Order = 25)]
	public byte[] arsRowVersion { get; set; }

	[JsonProperty("arsArPaymentSessionID", Order = 26)]
	[Required(ErrorMessage = "arsArPaymentSessionID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int arsArPaymentSessionID { get; set; }

	[JsonProperty("arsSettlementEndTime", Order = 27)]
	public DateTime? arsSettlementEndTime { get; set; }

	[JsonProperty("arsSettlementStartTime", Order = 28)]
	public DateTime? arsSettlementStartTime { get; set; }

	[JsonProperty("customFields", Order = 29)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
