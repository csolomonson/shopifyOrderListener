using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPFinancialPropertyInformationDto
{
	public string xafAccruedCreditorsGlAccountID { get; set; }

	public byte xafAgingMethod { get; set; }

	public string xafApAgingBucketID { get; set; }

	public DateTime? xafApApCostStartDate { get; set; }

	public string xafApApGlAccountID { get; set; }

	public string xafApCashGlAccountID { get; set; }

	public string xafApDiscountGlAccountID { get; set; }

	public string xafApFreightGlAccountID { get; set; }

	public byte xafApGroupReceiptsBySupplier { get; set; }

	public byte xafApPaymentMaxLinesPerPage { get; set; }

	public string xafArAgingBucketID { get; set; }

	public string xafArArGlAccountID { get; set; }

	public string xafArCashGlAccountID { get; set; }

	public string xafArDefaultLaborPartGroupID { get; set; }

	public string xafArDepositGlAccountID { get; set; }

	public string xafArDepositPartID { get; set; }

	public string xafArDepositPartRevisionID { get; set; }

	public string xafArDiscountGlAccountID { get; set; }

	public string xafArFinanceChargeGlAccountID { get; set; }

	public short xafArFinanceChargeGraceDays { get; set; }

	public DateTime? xafArFinanceChargeLastRunDate { get; set; }

	public decimal xafArFinanceChargePercent { get; set; }

	public byte xafArFinanceShowCreditBalance { get; set; }

	public string xafArFreightGlAccountID { get; set; }

	public byte xafArGroupShipmentsByCustomer { get; set; }

	public string xafArLaborPartID { get; set; }

	public string xafArLaborPartRevisionID { get; set; }

	public string xafArNET1GatewayID { get; set; }

	public string xafArNET1MerchantKey { get; set; }

	public int xafArNET1Port { get; set; }

	public short xafArNET1TimeoutSeconds { get; set; }

	public byte xafArShowDeposits { get; set; }

	public string xafAvalaraAccountID { get; set; }

	public byte xafAvalaraArInvoicePostOption { get; set; }

	public string xafAvalaraCanadaGstTaxCodeID { get; set; }

	public string xafAvalaraCanadaHSTTaxCodeID { get; set; }

	public string xafAvalaraCanadaPSTTaxCodeID { get; set; }

	public string xafAvalaraCanadaQSTTaxCodeID { get; set; }

	public string xafAvalaraCompanyCode { get; set; }

	public byte xafAvalaraFilterCountry { get; set; }

	public string xafAvalaraLicenseKey { get; set; }

	public string xafAvalaraTaxCodeID { get; set; }

	public short xafAvalaraTimeoutSeconds { get; set; }

	public string xafAvalaraURL { get; set; }

	public byte xafCAEmployerDentalBenefits { get; set; }

	public string xafCogsStatusHistory { get; set; }

	public byte xafCogsUseAccounts { get; set; }

	public string xafCreatedBy { get; set; }

	public DateTime? xafCreatedDate { get; set; }

	public byte xafCreditCardMethod { get; set; }

	public string xafDrawerCashGlAccountID { get; set; }

	public decimal xafDrawerCashStartAmount { get; set; }

	public Guid xafUniqueID { get; set; }

	public short xafGlFiscalYearID { get; set; }

	public byte xafGlFiscalYearPeriodID { get; set; }

	public string xafGlRetainedEarningsAccountID { get; set; }

	public bool xafAgeByDaysInMonth { get; set; }

	public bool xafApAllowParentAccountPost { get; set; }

	public bool xafApAlwaysTakeDiscount { get; set; }

	public bool xafApAssignNumbersToEft { get; set; }

	public bool xafApCreditUpdatesReceipt { get; set; }

	public bool xafApDisableTaxFields { get; set; }

	public bool xafApDiscountOnFreight { get; set; }

	public bool xafApDiscountOnTax { get; set; }

	public bool xafApExpressPost { get; set; }

	public bool xafApIncludeTaxInExpAmt { get; set; }

	public bool xafApPaymentFilterPlant { get; set; }

	public bool xafApTaxOnFreight { get; set; }

	public bool xafApUpdateJobCosts { get; set; }

	public bool xafArAllowParentAccountPost { get; set; }

	public bool xafArCalculateTaxOnDeposit { get; set; }

	public bool xafArCreateDiscountJournals { get; set; }

	public bool xafArCreditUpdatesShipment { get; set; }

	public bool xafArDisableTaxFields { get; set; }

	public bool xafArDiscountOnFreight { get; set; }

	public bool xafArExpressPost { get; set; }

	public bool xafArIncludeFrgtInDepositCalc { get; set; }

	public bool xafArIncludeTaxInDepositCalc { get; set; }

	public bool xafArPaymentFilterPlant { get; set; }

	public bool xafArTaxOnFreight { get; set; }

	public bool xafAvalaraDisableAddrValidate { get; set; }

	public bool xafAvalaraDisableIgnoreLine { get; set; }

	public bool xafAvalaraForceAddressValidate { get; set; }

	public bool xafCreateBankEntries { get; set; }

	public bool xafDisableMultiplePlants { get; set; }

	public bool xafExactDaysInPaymentTerms { get; set; }

	public bool xafFAroundToNearestDollar { get; set; }

	public bool xafGlCreateStockJournals { get; set; }

	public bool xafGlExpressPost { get; set; }

	public bool xafIncludeLLInTermination { get; set; }

	public bool xafPAAllowParentAccountPost { get; set; }

	public bool xafPAAssignNumbersToEft { get; set; }

	public bool xafPADeleteZeroPayHeaders { get; set; }

	public bool xafPAExpressPost { get; set; }

	public bool xafPartsMustExist { get; set; }

	public bool xafPAShowHolidaysForSalary { get; set; }

	public bool xafProductionExpressPost { get; set; }

	public bool xafRecalcSalarySacrifice { get; set; }

	public bool xafStpSetGrossPayAsETP { get; set; }

	public string xafLaborClearingGlAccountID { get; set; }

	public byte xafMiscReceiptVarianceAccount { get; set; }

	public string xafOverheadClearingGlAccountID { get; set; }

	public byte xafPALeaveBalanceCheck { get; set; }

	public byte xafPAUseDate { get; set; }

	public string xafPurchaseVarianceGlAccountID { get; set; }

	public string xafRoundingGlAccountID { get; set; }

	public byte[] xafRowVersion { get; set; }

	public string xafShipAwaitInvoiceGlAccountID { get; set; }

	public string xafStockInTransitGlAccountID { get; set; }

	public string xafStockRevaluationGlAccountID { get; set; }

	public string xafStoreCreditGlAccountID { get; set; }

	public string xafSuperEmployerID { get; set; }

	public DateTime? xafSuperEndDate { get; set; }

	public string xafSuperExportDateFormat { get; set; }

	public string xafSuperExportFilePath { get; set; }

	public DateTime? xafSuperStartDate { get; set; }

	public string xafSVarLaborGlAccountID { get; set; }

	public string xafSVarMaterialGlAccountID { get; set; }

	public string xafSVarOverheadGlAccountID { get; set; }

	public string xafSVarSubcontractGlAccountID { get; set; }

	public string xafTaxOnReportMethod { get; set; }

	public string xafTestFileCode { get; set; }

	public string xafTransmitterControlCode { get; set; }

	public string xafUS1094FileLocation { get; set; }

	public string xafWipLaborGlAccountID { get; set; }

	public string xafWipMaterialGlAccountID { get; set; }

	public string xafWipoverheadGlAccountID { get; set; }

	public string xafWipSubcontractGlAccountID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
