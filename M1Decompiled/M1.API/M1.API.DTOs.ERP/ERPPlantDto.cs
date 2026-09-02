using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPlantDto
{
	[JsonProperty("xauAccruedCreditorsGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string xauAccruedCreditorsGlAccountID { get; set; }

	[JsonProperty("xauAddressLine1", Order = 2)]
	[MaxLength(50)]
	public string xauAddressLine1 { get; set; }

	[JsonProperty("xauAddressLine2", Order = 3)]
	[MaxLength(50)]
	public string xauAddressLine2 { get; set; }

	[JsonProperty("xauAddressLine3", Order = 4)]
	[MaxLength(50)]
	public string xauAddressLine3 { get; set; }

	[JsonProperty("xauApApGlAccountID", Order = 5)]
	[MaxLength(11)]
	public string xauApApGlAccountID { get; set; }

	[JsonProperty("xauApBankAccountID", Order = 6)]
	[MaxLength(5)]
	public string xauApBankAccountID { get; set; }

	[JsonProperty("xauApCashGlAccountID", Order = 7)]
	[MaxLength(11)]
	public string xauApCashGlAccountID { get; set; }

	[JsonProperty("xauApDiscountGlAccountID", Order = 8)]
	[MaxLength(11)]
	public string xauApDiscountGlAccountID { get; set; }

	[JsonProperty("xauApFreightGlAccountID", Order = 9)]
	[MaxLength(11)]
	public string xauApFreightGlAccountID { get; set; }

	[JsonProperty("xauArArGlAccountID", Order = 10)]
	[MaxLength(11)]
	public string xauArArGlAccountID { get; set; }

	[JsonProperty("xauArBankAccountID", Order = 11)]
	[MaxLength(5)]
	public string xauArBankAccountID { get; set; }

	[JsonProperty("xauArCashGlAccountID", Order = 12)]
	[MaxLength(11)]
	public string xauArCashGlAccountID { get; set; }

	[JsonProperty("xauArDepositGlAccountID", Order = 13)]
	[MaxLength(11)]
	public string xauArDepositGlAccountID { get; set; }

	[JsonProperty("xauArDiscountGlAccountID", Order = 14)]
	[MaxLength(11)]
	public string xauArDiscountGlAccountID { get; set; }

	[JsonProperty("xauArFreightGlAccountID", Order = 15)]
	[MaxLength(11)]
	public string xauArFreightGlAccountID { get; set; }

	[JsonProperty("xauArSalesGlAccountID", Order = 16)]
	[MaxLength(11)]
	public string xauArSalesGlAccountID { get; set; }

	[JsonProperty("xauCity", Order = 17)]
	[MaxLength(30)]
	public string xauCity { get; set; }

	[JsonProperty("xauPlantID", Order = 18)]
	[Required(ErrorMessage = "xauPlantID is required.")]
	[MaxLength(5)]
	public string xauPlantID { get; set; }

	[JsonProperty("xauCountry", Order = 19)]
	[MaxLength(20)]
	public string xauCountry { get; set; }

	[JsonProperty("xauCountryCode", Order = 20)]
	[MaxLength(5)]
	public string xauCountryCode { get; set; }

	[JsonProperty("xauCreatedBy", Order = 21)]
	[MaxLength(20)]
	public string xauCreatedBy { get; set; }

	[JsonProperty("xauCreatedDate", Order = 22)]
	public DateTime? xauCreatedDate { get; set; }

	[JsonProperty("xauDayStartTimeFri", Order = 23)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauDayStartTimeFri { get; set; }

	[JsonProperty("xauDayStartTimeMon", Order = 24)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauDayStartTimeMon { get; set; }

	[JsonProperty("xauDayStartTimeSat", Order = 25)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauDayStartTimeSat { get; set; }

	[JsonProperty("xauDayStartTimeSun", Order = 26)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauDayStartTimeSun { get; set; }

	[JsonProperty("xauDayStartTimeThu", Order = 27)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauDayStartTimeThu { get; set; }

	[JsonProperty("xauDayStartTimeTue", Order = 28)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauDayStartTimeTue { get; set; }

	[JsonProperty("xauDayStartTimeWed", Order = 29)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauDayStartTimeWed { get; set; }

	[JsonProperty("xauEmailAddress", Order = 30)]
	[MaxLength(50)]
	public string xauEmailAddress { get; set; }

	[JsonProperty("xauUniqueID", Order = 31)]
	public Guid xauUniqueID { get; set; }

	[JsonProperty("xauEstablishedDate", Order = 32)]
	public DateTime? xauEstablishedDate { get; set; }

	[JsonProperty("xauFaxNumber", Order = 33)]
	[MaxLength(20)]
	public string xauFaxNumber { get; set; }

	[JsonProperty("xauFederalID", Order = 34)]
	[MaxLength(20)]
	public string xauFederalID { get; set; }

	[JsonProperty("xauHoursFri", Order = 35)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauHoursFri { get; set; }

	[JsonProperty("xauHoursMon", Order = 36)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauHoursMon { get; set; }

	[JsonProperty("xauHoursSat", Order = 37)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauHoursSat { get; set; }

	[JsonProperty("xauHoursSun", Order = 38)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauHoursSun { get; set; }

	[JsonProperty("xauHoursThu", Order = 39)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauHoursThu { get; set; }

	[JsonProperty("xauHoursTue", Order = 40)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauHoursTue { get; set; }

	[JsonProperty("xauHoursWed", Order = 41)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xauHoursWed { get; set; }

	[JsonProperty("xauInactiveDate", Order = 42)]
	public DateTime? xauInactiveDate { get; set; }

	[JsonProperty("xauInactive", Order = 43)]
	public bool xauInactive { get; set; }

	[JsonProperty("xauAvalaraAddressValidated", Order = 44)]
	public bool xauAvalaraAddressValidated { get; set; }

	[JsonProperty("xauUseProperties", Order = 45)]
	public bool xauUseProperties { get; set; }

	[JsonProperty("xauLaborClearingGlAccountID", Order = 46)]
	[MaxLength(11)]
	public string xauLaborClearingGlAccountID { get; set; }

	[JsonProperty("xauName", Order = 47)]
	[Required(ErrorMessage = "xauName is required.")]
	[MaxLength(50)]
	public string xauName { get; set; }

	[JsonProperty("xauOverheadClearingGlAccountID", Order = 48)]
	[MaxLength(11)]
	public string xauOverheadClearingGlAccountID { get; set; }

	[JsonProperty("xauPhoneNumber", Order = 49)]
	[MaxLength(20)]
	public string xauPhoneNumber { get; set; }

	[JsonProperty("xauPostCode", Order = 50)]
	[MaxLength(10)]
	public string xauPostCode { get; set; }

	[JsonProperty("xauPurchaseVarianceGlAccountID", Order = 51)]
	[MaxLength(11)]
	public string xauPurchaseVarianceGlAccountID { get; set; }

	[JsonProperty("xauRowVersion", Order = 52)]
	public byte[] xauRowVersion { get; set; }

	[JsonProperty("xauShipAwaitInvoiceGlAccountID", Order = 53)]
	[MaxLength(11)]
	public string xauShipAwaitInvoiceGlAccountID { get; set; }

	[JsonProperty("xauState", Order = 54)]
	[MaxLength(3)]
	public string xauState { get; set; }

	[JsonProperty("xauStockInTransitGlAccountID", Order = 55)]
	[MaxLength(11)]
	public string xauStockInTransitGlAccountID { get; set; }

	[JsonProperty("xauStockRevaluationGlAccountID", Order = 56)]
	[MaxLength(11)]
	public string xauStockRevaluationGlAccountID { get; set; }

	[JsonProperty("xauSVarLaborGlAccountID", Order = 57)]
	[MaxLength(11)]
	public string xauSVarLaborGlAccountID { get; set; }

	[JsonProperty("xauSVarMaterialGlAccountID", Order = 58)]
	[MaxLength(11)]
	public string xauSVarMaterialGlAccountID { get; set; }

	[JsonProperty("xauSVarOverheadGlAccountID", Order = 59)]
	[MaxLength(11)]
	public string xauSVarOverheadGlAccountID { get; set; }

	[JsonProperty("xauSVarSubcontractGlAccountID", Order = 60)]
	[MaxLength(11)]
	public string xauSVarSubcontractGlAccountID { get; set; }

	[JsonProperty("xauWipLaborGlAccountID", Order = 61)]
	[MaxLength(11)]
	public string xauWipLaborGlAccountID { get; set; }

	[JsonProperty("xauWipMaterialGlAccountID", Order = 62)]
	[MaxLength(11)]
	public string xauWipMaterialGlAccountID { get; set; }

	[JsonProperty("xauWipoverheadGlAccountID", Order = 63)]
	[MaxLength(11)]
	public string xauWipoverheadGlAccountID { get; set; }

	[JsonProperty("xauWipSubcontractGlAccountID", Order = 64)]
	[MaxLength(11)]
	public string xauWipSubcontractGlAccountID { get; set; }

	[JsonProperty("customFields", Order = 65)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
