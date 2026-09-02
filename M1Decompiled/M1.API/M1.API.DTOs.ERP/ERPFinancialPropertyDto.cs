using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPFinancialPropertyDto
{
	[JsonProperty("xafAccruedCreditorsGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string xafAccruedCreditorsGlAccountID { get; set; }

	[JsonProperty("xafAgingMethod", Order = 2)]
	[Required(ErrorMessage = "xafAgingMethod is required.")]
	public byte xafAgingMethod { get; set; }

	[JsonProperty("xafApAgingBucketID", Order = 3)]
	[MaxLength(5)]
	public string xafApAgingBucketID { get; set; }

	[JsonProperty("xafApApCostStartDate", Order = 4)]
	public DateTime? xafApApCostStartDate { get; set; }

	[JsonProperty("xafApApGlAccountID", Order = 5)]
	[MaxLength(11)]
	public string xafApApGlAccountID { get; set; }

	[JsonProperty("xafApCashGlAccountID", Order = 6)]
	[MaxLength(11)]
	public string xafApCashGlAccountID { get; set; }

	[JsonProperty("xafApDiscountGlAccountID", Order = 7)]
	[MaxLength(11)]
	public string xafApDiscountGlAccountID { get; set; }

	[JsonProperty("xafApFreightGlAccountID", Order = 8)]
	[MaxLength(11)]
	public string xafApFreightGlAccountID { get; set; }

	[JsonProperty("xafApGroupReceiptsBySupplier", Order = 9)]
	public byte xafApGroupReceiptsBySupplier { get; set; }

	[JsonProperty("xafApPaymentMaxLinesPerPage", Order = 10)]
	public byte xafApPaymentMaxLinesPerPage { get; set; }

	[JsonProperty("xafArAgingBucketID", Order = 11)]
	[MaxLength(5)]
	public string xafArAgingBucketID { get; set; }

	[JsonProperty("xafArArGlAccountID", Order = 12)]
	[MaxLength(11)]
	public string xafArArGlAccountID { get; set; }

	[JsonProperty("xafArCashGlAccountID", Order = 13)]
	[MaxLength(11)]
	public string xafArCashGlAccountID { get; set; }

	[JsonProperty("xafArDefaultLaborPartGroupID", Order = 14)]
	[MaxLength(5)]
	public string xafArDefaultLaborPartGroupID { get; set; }

	[JsonProperty("xafArDepositGlAccountID", Order = 15)]
	[MaxLength(11)]
	public string xafArDepositGlAccountID { get; set; }

	[JsonProperty("xafArDepositPartID", Order = 16)]
	[MaxLength(30)]
	public string xafArDepositPartID { get; set; }

	[JsonProperty("xafArDepositPartRevisionID", Order = 17)]
	[MaxLength(15)]
	public string xafArDepositPartRevisionID { get; set; }

	[JsonProperty("xafArDiscountGlAccountID", Order = 18)]
	[MaxLength(11)]
	public string xafArDiscountGlAccountID { get; set; }

	[JsonProperty("xafArFinanceChargeGlAccountID", Order = 19)]
	[MaxLength(11)]
	public string xafArFinanceChargeGlAccountID { get; set; }

	[JsonProperty("xafArFinanceChargeGraceDays", Order = 20)]
	public short xafArFinanceChargeGraceDays { get; set; }

	[JsonProperty("xafArFinanceChargeLastRunDate", Order = 21)]
	public DateTime? xafArFinanceChargeLastRunDate { get; set; }

	[JsonProperty("xafArFinanceChargePercent", Order = 22)]
	[Range(0.0, 999.999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xafArFinanceChargePercent { get; set; }

	[JsonProperty("xafArFinanceShowCreditBalance", Order = 23)]
	[Required(ErrorMessage = "xafArFinanceShowCreditBalance is required.")]
	public byte xafArFinanceShowCreditBalance { get; set; }

	[JsonProperty("xafArFreightGlAccountID", Order = 24)]
	[MaxLength(11)]
	public string xafArFreightGlAccountID { get; set; }

	[JsonProperty("xafArGroupShipmentsByCustomer", Order = 25)]
	public byte xafArGroupShipmentsByCustomer { get; set; }

	[JsonProperty("xafArLaborPartID", Order = 26)]
	[MaxLength(30)]
	public string xafArLaborPartID { get; set; }

	[JsonProperty("xafArLaborPartRevisionID", Order = 27)]
	[MaxLength(15)]
	public string xafArLaborPartRevisionID { get; set; }

	[JsonProperty("xafArNET1GatewayID", Order = 28)]
	[MaxLength(20)]
	public string xafArNET1GatewayID { get; set; }

	[JsonProperty("xafArNET1MerchantKey", Order = 29)]
	[MaxLength(20)]
	public string xafArNET1MerchantKey { get; set; }

	[JsonProperty("xafArNET1Port", Order = 30)]
	[Range(0, 999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xafArNET1Port { get; set; }

	[JsonProperty("xafArNET1TimeoutSeconds", Order = 31)]
	public short xafArNET1TimeoutSeconds { get; set; }

	[JsonProperty("xafArShowDeposits", Order = 32)]
	public byte xafArShowDeposits { get; set; }

	[JsonProperty("xafAvalaraAccountID", Order = 33)]
	[MaxLength(50)]
	public string xafAvalaraAccountID { get; set; }

	[JsonProperty("xafAvalaraArInvoicePostOption", Order = 34)]
	public byte xafAvalaraArInvoicePostOption { get; set; }

	[JsonProperty("xafAvalaraCanadaGstTaxCodeID", Order = 35)]
	[MaxLength(5)]
	public string xafAvalaraCanadaGstTaxCodeID { get; set; }

	[JsonProperty("xafAvalaraCanadaHSTTaxCodeID", Order = 36)]
	[MaxLength(5)]
	public string xafAvalaraCanadaHSTTaxCodeID { get; set; }

	[JsonProperty("xafAvalaraCanadaPSTTaxCodeID", Order = 37)]
	[MaxLength(5)]
	public string xafAvalaraCanadaPSTTaxCodeID { get; set; }

	[JsonProperty("xafAvalaraCanadaQSTTaxCodeID", Order = 38)]
	[MaxLength(5)]
	public string xafAvalaraCanadaQSTTaxCodeID { get; set; }

	[JsonProperty("xafAvalaraCompanyCode", Order = 39)]
	[MaxLength(50)]
	public string xafAvalaraCompanyCode { get; set; }

	[JsonProperty("xafAvalaraFilterCountry", Order = 40)]
	public byte xafAvalaraFilterCountry { get; set; }

	[JsonProperty("xafAvalaraLicenseKey", Order = 41)]
	[MaxLength(50)]
	public string xafAvalaraLicenseKey { get; set; }

	[JsonProperty("xafAvalaraTaxCodeID", Order = 42)]
	[MaxLength(5)]
	public string xafAvalaraTaxCodeID { get; set; }

	[JsonProperty("xafAvalaraTimeoutSeconds", Order = 43)]
	public short xafAvalaraTimeoutSeconds { get; set; }

	[JsonProperty("xafAvalaraURL", Order = 44)]
	[MaxLength(120)]
	public string xafAvalaraURL { get; set; }

	[JsonProperty("xafCAEmployerDentalBenefits", Order = 45)]
	public byte xafCAEmployerDentalBenefits { get; set; }

	[JsonProperty("xafCogsStatusHistory", Order = 46)]
	[MaxLength(50)]
	public string xafCogsStatusHistory { get; set; }

	[JsonProperty("xafCogsUseAccounts", Order = 47)]
	public byte xafCogsUseAccounts { get; set; }

	[JsonProperty("xafCreatedBy", Order = 48)]
	[MaxLength(20)]
	public string xafCreatedBy { get; set; }

	[JsonProperty("xafCreatedDate", Order = 49)]
	public DateTime? xafCreatedDate { get; set; }

	[JsonProperty("xafCreditCardMethod", Order = 50)]
	public byte xafCreditCardMethod { get; set; }

	[JsonProperty("xafDrawerCashGlAccountID", Order = 51)]
	[MaxLength(11)]
	public string xafDrawerCashGlAccountID { get; set; }

	[JsonProperty("xafDrawerCashStartAmount", Order = 52)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xafDrawerCashStartAmount { get; set; }

	[JsonProperty("xafUniqueID", Order = 53)]
	public Guid xafUniqueID { get; set; }

	[JsonProperty("xafGlFiscalYearID", Order = 54)]
	public short xafGlFiscalYearID { get; set; }

	[JsonProperty("xafGlFiscalYearPeriodID", Order = 55)]
	public byte xafGlFiscalYearPeriodID { get; set; }

	[JsonProperty("xafGlRetainedEarningsAccountID", Order = 56)]
	[MaxLength(11)]
	public string xafGlRetainedEarningsAccountID { get; set; }

	[JsonProperty("xafAgeByDaysInMonth", Order = 57)]
	public bool xafAgeByDaysInMonth { get; set; }

	[JsonProperty("xafApAllowParentAccountPost", Order = 58)]
	public bool xafApAllowParentAccountPost { get; set; }

	[JsonProperty("xafApAlwaysTakeDiscount", Order = 59)]
	public bool xafApAlwaysTakeDiscount { get; set; }

	[JsonProperty("xafApAssignNumbersToEft", Order = 60)]
	public bool xafApAssignNumbersToEft { get; set; }

	[JsonProperty("xafApCreditUpdatesReceipt", Order = 61)]
	public bool xafApCreditUpdatesReceipt { get; set; }

	[JsonProperty("xafApDisableTaxFields", Order = 62)]
	public bool xafApDisableTaxFields { get; set; }

	[JsonProperty("xafApDiscountOnFreight", Order = 63)]
	public bool xafApDiscountOnFreight { get; set; }

	[JsonProperty("xafApDiscountOnTax", Order = 64)]
	public bool xafApDiscountOnTax { get; set; }

	[JsonProperty("xafApExpressPost", Order = 65)]
	public bool xafApExpressPost { get; set; }

	[JsonProperty("xafApIncludeTaxInExpAmt", Order = 66)]
	public bool xafApIncludeTaxInExpAmt { get; set; }

	[JsonProperty("xafApPaymentFilterPlant", Order = 67)]
	public bool xafApPaymentFilterPlant { get; set; }

	[JsonProperty("xafApTaxOnFreight", Order = 68)]
	public bool xafApTaxOnFreight { get; set; }

	[JsonProperty("xafApUpdateJobCosts", Order = 69)]
	public bool xafApUpdateJobCosts { get; set; }

	[JsonProperty("xafArAllowParentAccountPost", Order = 70)]
	public bool xafArAllowParentAccountPost { get; set; }

	[JsonProperty("xafArCalculateTaxOnDeposit", Order = 71)]
	public bool xafArCalculateTaxOnDeposit { get; set; }

	[JsonProperty("xafArCreateDiscountJournals", Order = 72)]
	public bool xafArCreateDiscountJournals { get; set; }

	[JsonProperty("xafArCreditUpdatesShipment", Order = 73)]
	public bool xafArCreditUpdatesShipment { get; set; }

	[JsonProperty("xafArDisableTaxFields", Order = 74)]
	public bool xafArDisableTaxFields { get; set; }

	[JsonProperty("xafArDiscountOnFreight", Order = 75)]
	public bool xafArDiscountOnFreight { get; set; }

	[JsonProperty("xafArExpressPost", Order = 76)]
	public bool xafArExpressPost { get; set; }

	[JsonProperty("xafArIncludeFrgtInDepositCalc", Order = 77)]
	public bool xafArIncludeFrgtInDepositCalc { get; set; }

	[JsonProperty("xafArIncludeTaxInDepositCalc", Order = 78)]
	public bool xafArIncludeTaxInDepositCalc { get; set; }

	[JsonProperty("xafArPaymentFilterPlant", Order = 79)]
	public bool xafArPaymentFilterPlant { get; set; }

	[JsonProperty("xafArTaxOnFreight", Order = 80)]
	public bool xafArTaxOnFreight { get; set; }

	[JsonProperty("xafAvalaraDisableAddrValidate", Order = 81)]
	public bool xafAvalaraDisableAddrValidate { get; set; }

	[JsonProperty("xafAvalaraDisableIgnoreLine", Order = 82)]
	public bool xafAvalaraDisableIgnoreLine { get; set; }

	[JsonProperty("xafAvalaraForceAddressValidate", Order = 83)]
	public bool xafAvalaraForceAddressValidate { get; set; }

	[JsonProperty("xafCreateBankEntries", Order = 84)]
	public bool xafCreateBankEntries { get; set; }

	[JsonProperty("xafDisableMultiplePlants", Order = 85)]
	public bool xafDisableMultiplePlants { get; set; }

	[JsonProperty("xafExactDaysInPaymentTerms", Order = 86)]
	public bool xafExactDaysInPaymentTerms { get; set; }

	[JsonProperty("xafFAroundToNearestDollar", Order = 87)]
	public bool xafFAroundToNearestDollar { get; set; }

	[JsonProperty("xafGlCreateStockJournals", Order = 88)]
	public bool xafGlCreateStockJournals { get; set; }

	[JsonProperty("xafGlExpressPost", Order = 89)]
	public bool xafGlExpressPost { get; set; }

	[JsonProperty("xafIncludeLLInTermination", Order = 90)]
	public bool xafIncludeLLInTermination { get; set; }

	[JsonProperty("xafPAAllowParentAccountPost", Order = 91)]
	public bool xafPAAllowParentAccountPost { get; set; }

	[JsonProperty("xafPAAssignNumbersToEft", Order = 92)]
	public bool xafPAAssignNumbersToEft { get; set; }

	[JsonProperty("xafPADeleteZeroPayHeaders", Order = 93)]
	public bool xafPADeleteZeroPayHeaders { get; set; }

	[JsonProperty("xafPAExpressPost", Order = 94)]
	public bool xafPAExpressPost { get; set; }

	[JsonProperty("xafPartsMustExist", Order = 95)]
	public bool xafPartsMustExist { get; set; }

	[JsonProperty("xafPAShowHolidaysForSalary", Order = 96)]
	public bool xafPAShowHolidaysForSalary { get; set; }

	[JsonProperty("xafProductionExpressPost", Order = 97)]
	public bool xafProductionExpressPost { get; set; }

	[JsonProperty("xafRecalcSalarySacrifice", Order = 98)]
	public bool xafRecalcSalarySacrifice { get; set; }

	[JsonProperty("xafStpSetGrossPayAsETP", Order = 99)]
	public bool xafStpSetGrossPayAsETP { get; set; }

	[JsonProperty("xafLaborClearingGlAccountID", Order = 100)]
	[MaxLength(11)]
	public string xafLaborClearingGlAccountID { get; set; }

	[JsonProperty("xafMiscReceiptVarianceAccount", Order = 101)]
	public byte xafMiscReceiptVarianceAccount { get; set; }

	[JsonProperty("xafOverheadClearingGlAccountID", Order = 102)]
	[MaxLength(11)]
	public string xafOverheadClearingGlAccountID { get; set; }

	[JsonProperty("xafPALeaveBalanceCheck", Order = 103)]
	public byte xafPALeaveBalanceCheck { get; set; }

	[JsonProperty("xafPAUseDate", Order = 104)]
	public byte xafPAUseDate { get; set; }

	[JsonProperty("xafPurchaseVarianceGlAccountID", Order = 105)]
	[MaxLength(11)]
	public string xafPurchaseVarianceGlAccountID { get; set; }

	[JsonProperty("xafRoundingGlAccountID", Order = 106)]
	[MaxLength(11)]
	public string xafRoundingGlAccountID { get; set; }

	[JsonProperty("xafRowVersion", Order = 107)]
	public byte[] xafRowVersion { get; set; }

	[JsonProperty("xafShipAwaitInvoiceGlAccountID", Order = 108)]
	[MaxLength(11)]
	public string xafShipAwaitInvoiceGlAccountID { get; set; }

	[JsonProperty("xafStockInTransitGlAccountID", Order = 109)]
	[MaxLength(11)]
	public string xafStockInTransitGlAccountID { get; set; }

	[JsonProperty("xafStockRevaluationGlAccountID", Order = 110)]
	[MaxLength(11)]
	public string xafStockRevaluationGlAccountID { get; set; }

	[JsonProperty("xafStoreCreditGlAccountID", Order = 111)]
	[MaxLength(11)]
	public string xafStoreCreditGlAccountID { get; set; }

	[JsonProperty("xafSuperEmployerID", Order = 112)]
	[MaxLength(30)]
	public string xafSuperEmployerID { get; set; }

	[JsonProperty("xafSuperEndDate", Order = 113)]
	public DateTime? xafSuperEndDate { get; set; }

	[JsonProperty("xafSuperExportDateFormat", Order = 114)]
	[MaxLength(11)]
	public string xafSuperExportDateFormat { get; set; }

	[JsonProperty("xafSuperExportFilePath", Order = 115)]
	[MaxLength(200)]
	public string xafSuperExportFilePath { get; set; }

	[JsonProperty("xafSuperStartDate", Order = 116)]
	public DateTime? xafSuperStartDate { get; set; }

	[JsonProperty("xafSVarLaborGlAccountID", Order = 117)]
	[MaxLength(11)]
	public string xafSVarLaborGlAccountID { get; set; }

	[JsonProperty("xafSVarMaterialGlAccountID", Order = 118)]
	[MaxLength(11)]
	public string xafSVarMaterialGlAccountID { get; set; }

	[JsonProperty("xafSVarOverheadGlAccountID", Order = 119)]
	[MaxLength(11)]
	public string xafSVarOverheadGlAccountID { get; set; }

	[JsonProperty("xafSVarSubcontractGlAccountID", Order = 120)]
	[MaxLength(11)]
	public string xafSVarSubcontractGlAccountID { get; set; }

	[JsonProperty("xafTaxOnReportMethod", Order = 121)]
	[Required(ErrorMessage = "xafTaxOnReportMethod is required.")]
	[MaxLength(1)]
	public string xafTaxOnReportMethod { get; set; }

	[JsonProperty("xafTestFileCode", Order = 122)]
	[MaxLength(1)]
	public string xafTestFileCode { get; set; }

	[JsonProperty("xafTransmitterControlCode", Order = 123)]
	[MaxLength(10)]
	public string xafTransmitterControlCode { get; set; }

	[JsonProperty("xafUS1094FileLocation", Order = 124)]
	[MaxLength(250)]
	public string xafUS1094FileLocation { get; set; }

	[JsonProperty("xafWipLaborGlAccountID", Order = 125)]
	[MaxLength(11)]
	public string xafWipLaborGlAccountID { get; set; }

	[JsonProperty("xafWipMaterialGlAccountID", Order = 126)]
	[MaxLength(11)]
	public string xafWipMaterialGlAccountID { get; set; }

	[JsonProperty("xafWipoverheadGlAccountID", Order = 127)]
	[MaxLength(11)]
	public string xafWipoverheadGlAccountID { get; set; }

	[JsonProperty("xafWipSubcontractGlAccountID", Order = 128)]
	[MaxLength(11)]
	public string xafWipSubcontractGlAccountID { get; set; }

	[JsonProperty("customFields", Order = 129)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
