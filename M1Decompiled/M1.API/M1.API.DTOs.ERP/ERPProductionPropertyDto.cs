using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProductionPropertyDto
{
	[JsonProperty("xapAllowNegQtyOnHandHistory", Order = 1)]
	[MaxLength(50)]
	public string xapAllowNegQtyOnHandHistory { get; set; }

	[JsonProperty("xapAnonymousCustomerID", Order = 2)]
	[MaxLength(10)]
	public string xapAnonymousCustomerID { get; set; }

	[JsonProperty("xapChChangeRequestTypeID", Order = 3)]
	[MaxLength(5)]
	public string xapChChangeRequestTypeID { get; set; }

	[JsonProperty("xapCmArPaymentCreditMessage", Order = 4)]
	public byte xapCmArPaymentCreditMessage { get; set; }

	[JsonProperty("xapCmArPaymentHoldMessage", Order = 5)]
	public byte xapCmArPaymentHoldMessage { get; set; }

	[JsonProperty("xapCmFieldServiceCreditMessage", Order = 6)]
	public byte xapCmFieldServiceCreditMessage { get; set; }

	[JsonProperty("xapCmFieldServiceHoldMessage", Order = 7)]
	public byte xapCmFieldServiceHoldMessage { get; set; }

	[JsonProperty("xapCmNonTaxReasonID", Order = 8)]
	[MaxLength(5)]
	public string xapCmNonTaxReasonID { get; set; }

	[JsonProperty("xapCmOrderCreditMessage", Order = 9)]
	public byte xapCmOrderCreditMessage { get; set; }

	[JsonProperty("xapCmOrderHoldMessage", Order = 10)]
	public byte xapCmOrderHoldMessage { get; set; }

	[JsonProperty("xapCmSalesPersonDefaultLoc", Order = 11)]
	[Required(ErrorMessage = "xapCmSalesPersonDefaultLoc is required.")]
	public byte xapCmSalesPersonDefaultLoc { get; set; }

	[JsonProperty("xapCmShipmentCreditMessage", Order = 12)]
	public byte xapCmShipmentCreditMessage { get; set; }

	[JsonProperty("xapCmShipmentHoldMessage", Order = 13)]
	public byte xapCmShipmentHoldMessage { get; set; }

	[JsonProperty("xapCostingMethodHistory", Order = 14)]
	[MaxLength(50)]
	public string xapCostingMethodHistory { get; set; }

	[JsonProperty("xapCreatedBy", Order = 15)]
	[MaxLength(20)]
	public string xapCreatedBy { get; set; }

	[JsonProperty("xapCreatedDate", Order = 16)]
	public DateTime? xapCreatedDate { get; set; }

	[JsonProperty("xapDateToSchedule", Order = 17)]
	public DateTime? xapDateToSchedule { get; set; }

	[JsonProperty("xapDcAutoClockOutComputer", Order = 18)]
	[MaxLength(50)]
	public string xapDcAutoClockOutComputer { get; set; }

	[JsonProperty("xapDcIdleTimeThreshhold", Order = 19)]
	public byte xapDcIdleTimeThreshhold { get; set; }

	[JsonProperty("xapDcLaborCalculationMethod", Order = 20)]
	[Required(ErrorMessage = "xapDcLaborCalculationMethod is required.")]
	public byte xapDcLaborCalculationMethod { get; set; }

	[JsonProperty("xapDcPayCalculationMethod", Order = 21)]
	[Required(ErrorMessage = "xapDcPayCalculationMethod is required.")]
	public byte xapDcPayCalculationMethod { get; set; }

	[JsonProperty("xapDcRefreshInterval", Order = 22)]
	public byte xapDcRefreshInterval { get; set; }

	[JsonProperty("xapDcSfeInspectionPassword", Order = 23)]
	[MaxLength(10)]
	public string xapDcSfeInspectionPassword { get; set; }

	[JsonProperty("xapDcSfeShutdownPassword", Order = 24)]
	[MaxLength(10)]
	public string xapDcSfeShutdownPassword { get; set; }

	[JsonProperty("xapDcTimeFormat", Order = 25)]
	[Required(ErrorMessage = "xapDcTimeFormat is required.")]
	public byte xapDcTimeFormat { get; set; }

	[JsonProperty("xapDMDefaultFolder", Order = 26)]
	[MaxLength(50)]
	public string xapDMDefaultFolder { get; set; }

	[JsonProperty("xapEdi810ServiceUrl", Order = 27)]
	[MaxLength(200)]
	public string xapEdi810ServiceUrl { get; set; }

	[JsonProperty("xapEdi856ServiceUrl", Order = 28)]
	[MaxLength(200)]
	public string xapEdi856ServiceUrl { get; set; }

	[JsonProperty("xapEdiPassword", Order = 29)]
	[MaxLength(50)]
	public string xapEdiPassword { get; set; }

	[JsonProperty("xapEdiUserName", Order = 30)]
	[MaxLength(10)]
	public string xapEdiUserName { get; set; }

	[JsonProperty("xapUniqueID", Order = 31)]
	public Guid xapUniqueID { get; set; }

	[JsonProperty("xapHdAttachmentFilePath", Order = 32)]
	[MaxLength(50)]
	public string xapHdAttachmentFilePath { get; set; }

	[JsonProperty("xapHdcallDueDateDays", Order = 33)]
	public short xapHdcallDueDateDays { get; set; }

	[JsonProperty("xapHdcallTypeID", Order = 34)]
	[MaxLength(5)]
	public string xapHdcallTypeID { get; set; }

	[JsonProperty("xapHdcontactMethodID", Order = 35)]
	[MaxLength(5)]
	public string xapHdcontactMethodID { get; set; }

	[JsonProperty("xapHdMailMergeCallTypeID", Order = 36)]
	[MaxLength(5)]
	public string xapHdMailMergeCallTypeID { get; set; }

	[JsonProperty("xapHdMailMergeContactMethodID", Order = 37)]
	[MaxLength(5)]
	public string xapHdMailMergeContactMethodID { get; set; }

	[JsonProperty("xapHdNewCallSoundFile", Order = 38)]
	[MaxLength(50)]
	public string xapHdNewCallSoundFile { get; set; }

	[JsonProperty("xapHdSalesCallTypeID", Order = 39)]
	[MaxLength(5)]
	public string xapHdSalesCallTypeID { get; set; }

	[JsonProperty("xapImAutoCreateRevisionID", Order = 40)]
	[MaxLength(15)]
	public string xapImAutoCreateRevisionID { get; set; }

	[JsonProperty("xapImCostingMethod", Order = 41)]
	[Required(ErrorMessage = "xapImCostingMethod is required.")]
	public byte xapImCostingMethod { get; set; }

	[JsonProperty("xapImMfgDefaultCostType", Order = 42)]
	[Required(ErrorMessage = "xapImMfgDefaultCostType is required.")]
	public byte xapImMfgDefaultCostType { get; set; }

	[JsonProperty("xapCmCreateJobOnly", Order = 43)]
	public bool xapCmCreateJobOnly { get; set; }

	[JsonProperty("xapCmCreditLimitSourceInv", Order = 44)]
	public bool xapCmCreditLimitSourceInv { get; set; }

	[JsonProperty("xapCmCreditLimitSourceOrder", Order = 45)]
	public bool xapCmCreditLimitSourceOrder { get; set; }

	[JsonProperty("xapCmCreditLimitSourceShip", Order = 46)]
	public bool xapCmCreditLimitSourceShip { get; set; }

	[JsonProperty("xapCmCustomerTaxable", Order = 47)]
	public bool xapCmCustomerTaxable { get; set; }

	[JsonProperty("xapCmEnableResellers", Order = 48)]
	public bool xapCmEnableResellers { get; set; }

	[JsonProperty("xapCmIncludeFreightInPrice", Order = 49)]
	public bool xapCmIncludeFreightInPrice { get; set; }

	[JsonProperty("xapDcAllowNegativeQty", Order = 50)]
	public bool xapDcAllowNegativeQty { get; set; }

	[JsonProperty("xapDcAllowProductionComplete", Order = 51)]
	public bool xapDcAllowProductionComplete { get; set; }

	[JsonProperty("xapDcAutoClockOutLocked", Order = 52)]
	public bool xapDcAutoClockOutLocked { get; set; }

	[JsonProperty("xapDcEnableCreateSequence", Order = 53)]
	public bool xapDcEnableCreateSequence { get; set; }

	[JsonProperty("xapDcEnableIssueMaterial", Order = 54)]
	public bool xapDcEnableIssueMaterial { get; set; }

	[JsonProperty("xapDcEnableJobTraveler", Order = 55)]
	public bool xapDcEnableJobTraveler { get; set; }

	[JsonProperty("xapDcEnableMinimizeButtonInSfe", Order = 56)]
	public bool xapDcEnableMinimizeButtonInSfe { get; set; }

	[JsonProperty("xapDcEnableTimecardAudit", Order = 57)]
	public bool xapDcEnableTimecardAudit { get; set; }

	[JsonProperty("xapDcEnableWorkQueue", Order = 58)]
	public bool xapDcEnableWorkQueue { get; set; }

	[JsonProperty("xapDcPromptForActivityPassword", Order = 59)]
	public bool xapDcPromptForActivityPassword { get; set; }

	[JsonProperty("xapDcPromptForAuditPassword", Order = 60)]
	public bool xapDcPromptForAuditPassword { get; set; }

	[JsonProperty("xapDcPromptForClockInPassword", Order = 61)]
	public bool xapDcPromptForClockInPassword { get; set; }

	[JsonProperty("xapDcPromptForLaborDescription", Order = 62)]
	public bool xapDcPromptForLaborDescription { get; set; }

	[JsonProperty("xapDcPromptForMessagePassword", Order = 63)]
	public bool xapDcPromptForMessagePassword { get; set; }

	[JsonProperty("xapDcPromptForReason", Order = 64)]
	public bool xapDcPromptForReason { get; set; }

	[JsonProperty("xapDcShowCurrentJobsOnly", Order = 65)]
	public bool xapDcShowCurrentJobsOnly { get; set; }

	[JsonProperty("xapDcSplitDirectLaborHours", Order = 66)]
	public bool xapDcSplitDirectLaborHours { get; set; }

	[JsonProperty("xapDcSplitIndirectLaborHours", Order = 67)]
	public bool xapDcSplitIndirectLaborHours { get; set; }

	[JsonProperty("xapDcUseServerTime", Order = 68)]
	public bool xapDcUseServerTime { get; set; }

	[JsonProperty("xapDcWarnOnOutsideOperation", Order = 69)]
	public bool xapDcWarnOnOutsideOperation { get; set; }

	[JsonProperty("xapDcWarnOnOverProduction", Order = 70)]
	public bool xapDcWarnOnOverProduction { get; set; }

	[JsonProperty("xapGlCreateStockJournals", Order = 71)]
	public bool xapGlCreateStockJournals { get; set; }

	[JsonProperty("xapHdcreateCallForEmails", Order = 72)]
	public bool xapHdcreateCallForEmails { get; set; }

	[JsonProperty("xapImAllowNegativeQtyOnHand", Order = 73)]
	public bool xapImAllowNegativeQtyOnHand { get; set; }

	[JsonProperty("xapImAutoCreateRevision", Order = 74)]
	public bool xapImAutoCreateRevision { get; set; }

	[JsonProperty("xapImCopyAlternates", Order = 75)]
	public bool xapImCopyAlternates { get; set; }

	[JsonProperty("xapImCopyPartMemos", Order = 76)]
	public bool xapImCopyPartMemos { get; set; }

	[JsonProperty("xapImCopyPartOrgReferences", Order = 77)]
	public bool xapImCopyPartOrgReferences { get; set; }

	[JsonProperty("xapImCopyPartPrices", Order = 78)]
	public bool xapImCopyPartPrices { get; set; }

	[JsonProperty("xapImCopyPartRules", Order = 79)]
	public bool xapImCopyPartRules { get; set; }

	[JsonProperty("xapImEnableOrgPartCustomer", Order = 80)]
	public bool xapImEnableOrgPartCustomer { get; set; }

	[JsonProperty("xapImEnableOrgPartSupplier", Order = 81)]
	public bool xapImEnableOrgPartSupplier { get; set; }

	[JsonProperty("xapImEnableWarningWhenNegative", Order = 82)]
	public bool xapImEnableWarningWhenNegative { get; set; }

	[JsonProperty("xapImForceConfiguratorScreens", Order = 83)]
	public bool xapImForceConfiguratorScreens { get; set; }

	[JsonProperty("xapImHideUseMethodInTree", Order = 84)]
	public bool xapImHideUseMethodInTree { get; set; }

	[JsonProperty("xapImIgnoreLCInStdCostRollup", Order = 85)]
	public bool xapImIgnoreLCInStdCostRollup { get; set; }

	[JsonProperty("xapImOnlyAllowExistingBins", Order = 86)]
	public bool xapImOnlyAllowExistingBins { get; set; }

	[JsonProperty("xapImOverwriteDescription", Order = 87)]
	public bool xapImOverwriteDescription { get; set; }

	[JsonProperty("xapImOverwriteDocuments", Order = 88)]
	public bool xapImOverwriteDocuments { get; set; }

	[JsonProperty("xapImOverwriteMethod", Order = 89)]
	public bool xapImOverwriteMethod { get; set; }

	[JsonProperty("xapImRefreshMaterial", Order = 90)]
	public bool xapImRefreshMaterial { get; set; }

	[JsonProperty("xapImRefreshMaterialCosts", Order = 91)]
	public bool xapImRefreshMaterialCosts { get; set; }

	[JsonProperty("xapImScrapRoundUp", Order = 92)]
	public bool xapImScrapRoundUp { get; set; }

	[JsonProperty("xapImSetUseMethod", Order = 93)]
	public bool xapImSetUseMethod { get; set; }

	[JsonProperty("xapImTransferCustomer", Order = 94)]
	public bool xapImTransferCustomer { get; set; }

	[JsonProperty("xapImTransferDescriptions", Order = 95)]
	public bool xapImTransferDescriptions { get; set; }

	[JsonProperty("xapImTransferMaterial", Order = 96)]
	public bool xapImTransferMaterial { get; set; }

	[JsonProperty("xapImUseStdForStdCostRollUp", Order = 97)]
	public bool xapImUseStdForStdCostRollUp { get; set; }

	[JsonProperty("xapJmExcessQuantity", Order = 98)]
	public bool xapJmExcessQuantity { get; set; }

	[JsonProperty("xapJmIgnoreEmployees", Order = 99)]
	public bool xapJmIgnoreEmployees { get; set; }

	[JsonProperty("xapJmIgnoreMachines", Order = 100)]
	public bool xapJmIgnoreMachines { get; set; }

	[JsonProperty("xapJmLoadLevelFinite", Order = 101)]
	public bool xapJmLoadLevelFinite { get; set; }

	[JsonProperty("xapJmMinimizeGaps", Order = 102)]
	public bool xapJmMinimizeGaps { get; set; }

	[JsonProperty("xapJmMRPForecastFirmJob", Order = 103)]
	public bool xapJmMRPForecastFirmJob { get; set; }

	[JsonProperty("xapJmOverwriteDescription", Order = 104)]
	public bool xapJmOverwriteDescription { get; set; }

	[JsonProperty("xapJmOverwriteDocuments", Order = 105)]
	public bool xapJmOverwriteDocuments { get; set; }

	[JsonProperty("xapJmOverwriteMethod", Order = 106)]
	public bool xapJmOverwriteMethod { get; set; }

	[JsonProperty("xapJmRefreshHours", Order = 107)]
	public bool xapJmRefreshHours { get; set; }

	[JsonProperty("xapJmRefreshMaterial", Order = 108)]
	public bool xapJmRefreshMaterial { get; set; }

	[JsonProperty("xapJmRefreshMaterialCosts", Order = 109)]
	public bool xapJmRefreshMaterialCosts { get; set; }

	[JsonProperty("xapJmScheduleShowActualTimes", Order = 110)]
	public bool xapJmScheduleShowActualTimes { get; set; }

	[JsonProperty("xapJmScheduleUseActuals", Order = 111)]
	public bool xapJmScheduleUseActuals { get; set; }

	[JsonProperty("xapJmShopLoadShowFutureLoad", Order = 112)]
	public bool xapJmShopLoadShowFutureLoad { get; set; }

	[JsonProperty("xapJmShopLoadShowPastLoad", Order = 113)]
	public bool xapJmShopLoadShowPastLoad { get; set; }

	[JsonProperty("xapLmUpdateActualWithRounded", Order = 114)]
	public bool xapLmUpdateActualWithRounded { get; set; }

	[JsonProperty("xapNextSerialNumberPerGroup", Order = 115)]
	public bool xapNextSerialNumberPerGroup { get; set; }

	[JsonProperty("xapOmAutoCreateDelivery", Order = 116)]
	public bool xapOmAutoCreateDelivery { get; set; }

	[JsonProperty("xapOmEnableDiscountFields", Order = 117)]
	public bool xapOmEnableDiscountFields { get; set; }

	[JsonProperty("xapOmEnableFreightFields", Order = 118)]
	public bool xapOmEnableFreightFields { get; set; }

	[JsonProperty("xapOmIncludeOrderDeliveryInJob", Order = 119)]
	public bool xapOmIncludeOrderDeliveryInJob { get; set; }

	[JsonProperty("xapOmIncludeOrderLineInJob", Order = 120)]
	public bool xapOmIncludeOrderLineInJob { get; set; }

	[JsonProperty("xapOmMarkCreateJobForMto", Order = 121)]
	public bool xapOmMarkCreateJobForMto { get; set; }

	[JsonProperty("xapOmMarkPullQuoteMethodForMto", Order = 122)]
	public bool xapOmMarkPullQuoteMethodForMto { get; set; }

	[JsonProperty("xapOmShowDeliveriesInTree", Order = 123)]
	public bool xapOmShowDeliveriesInTree { get; set; }

	[JsonProperty("xapOmUseQuotingMarkupTM", Order = 124)]
	public bool xapOmUseQuotingMarkupTM { get; set; }

	[JsonProperty("xapPmPTOUsesDeliveryCost", Order = 125)]
	public bool xapPmPTOUsesDeliveryCost { get; set; }

	[JsonProperty("xapPmPurPlannerIncWhsQties", Order = 126)]
	public bool xapPmPurPlannerIncWhsQties { get; set; }

	[JsonProperty("xapPmPurPlannerUseBestPrice", Order = 127)]
	public bool xapPmPurPlannerUseBestPrice { get; set; }

	[JsonProperty("xapPmShowFirmOnlyPoWiz", Order = 128)]
	public bool xapPmShowFirmOnlyPoWiz { get; set; }

	[JsonProperty("xapPoWizardShowQtyToInspect", Order = 129)]
	public bool xapPoWizardShowQtyToInspect { get; set; }

	[JsonProperty("xapPRUseFirmQuotesOnly", Order = 130)]
	public bool xapPRUseFirmQuotesOnly { get; set; }

	[JsonProperty("xapQArmaRequiresInspection", Order = 131)]
	public bool xapQArmaRequiresInspection { get; set; }

	[JsonProperty("xapQAShowRmaOtherInfo", Order = 132)]
	public bool xapQAShowRmaOtherInfo { get; set; }

	[JsonProperty("xapQmMultipleQuantities", Order = 133)]
	public bool xapQmMultipleQuantities { get; set; }

	[JsonProperty("xapQmMUseDefHeaderFooterText", Order = 134)]
	public bool xapQmMUseDefHeaderFooterText { get; set; }

	[JsonProperty("xapQmOverwriteDescription", Order = 135)]
	public bool xapQmOverwriteDescription { get; set; }

	[JsonProperty("xapQmOverwriteDocuments", Order = 136)]
	public bool xapQmOverwriteDocuments { get; set; }

	[JsonProperty("xapQmOverwriteMethod", Order = 137)]
	public bool xapQmOverwriteMethod { get; set; }

	[JsonProperty("xapQmRefreshMaterial", Order = 138)]
	public bool xapQmRefreshMaterial { get; set; }

	[JsonProperty("xapQmRefreshMaterialCosts", Order = 139)]
	public bool xapQmRefreshMaterialCosts { get; set; }

	[JsonProperty("xapQmRefreshRateInfo", Order = 140)]
	public bool xapQmRefreshRateInfo { get; set; }

	[JsonProperty("xapRQGroupPobyRfq", Order = 141)]
	public bool xapRQGroupPobyRfq { get; set; }

	[JsonProperty("xapRQIncludeAlternateParts", Order = 142)]
	public bool xapRQIncludeAlternateParts { get; set; }

	[JsonProperty("xapSfeAllowSuspend", Order = 143)]
	public bool xapSfeAllowSuspend { get; set; }

	[JsonProperty("xapSfeBarcodeScanner", Order = 144)]
	public bool xapSfeBarcodeScanner { get; set; }

	[JsonProperty("xapSfeTouchScreen", Order = 145)]
	public bool xapSfeTouchScreen { get; set; }

	[JsonProperty("xapSmDeleteZeroShipmentLines", Order = 146)]
	public bool xapSmDeleteZeroShipmentLines { get; set; }

	[JsonProperty("xapJmCalendarExportFields", Order = 147)]
	[MaxLength(50)]
	public string xapJmCalendarExportFields { get; set; }

	[JsonProperty("xapJmInitialExtension", Order = 148)]
	[MaxLength(4)]
	public string xapJmInitialExtension { get; set; }

	[JsonProperty("xapJmInsideInspectionLineRTF", Order = 149)]
	[MaxLength(50)]
	public string xapJmInsideInspectionLineRTF { get; set; }

	[JsonProperty("xapJmInsideInspectionLineText", Order = 150)]
	[MaxLength(50)]
	public string xapJmInsideInspectionLineText { get; set; }

	[JsonProperty("xapJmJobMaterialSource", Order = 151)]
	[Required(ErrorMessage = "xapJmJobMaterialSource is required.")]
	public byte xapJmJobMaterialSource { get; set; }

	[JsonProperty("xapJmLoadReliefMethod", Order = 152)]
	[Required(ErrorMessage = "xapJmLoadReliefMethod is required.")]
	public byte xapJmLoadReliefMethod { get; set; }

	[JsonProperty("xapJmOutsideInspectionLineRTF", Order = 153)]
	[MaxLength(50)]
	public string xapJmOutsideInspectionLineRTF { get; set; }

	[JsonProperty("xapJmOutsideInspectionLineText", Order = 154)]
	[MaxLength(50)]
	public string xapJmOutsideInspectionLineText { get; set; }

	[JsonProperty("xapJmScheduleBoardFields", Order = 155)]
	[MaxLength(50)]
	public string xapJmScheduleBoardFields { get; set; }

	[JsonProperty("xapJmScheduleType", Order = 156)]
	public byte xapJmScheduleType { get; set; }

	[JsonProperty("xapJmShopLoadBuckets", Order = 157)]
	public byte xapJmShopLoadBuckets { get; set; }

	[JsonProperty("xapJmShopLoadDays", Order = 158)]
	public byte xapJmShopLoadDays { get; set; }

	[JsonProperty("xapJmShopLoadDepartmentID", Order = 159)]
	[MaxLength(5)]
	public string xapJmShopLoadDepartmentID { get; set; }

	[JsonProperty("xapJmShopLoadFields", Order = 160)]
	[MaxLength(10)]
	public string xapJmShopLoadFields { get; set; }

	[JsonProperty("xapJmShopLoadPlantID", Order = 161)]
	[MaxLength(5)]
	public string xapJmShopLoadPlantID { get; set; }

	[JsonProperty("xapJmShopLoadTimeType", Order = 162)]
	[MaxLength(2)]
	public string xapJmShopLoadTimeType { get; set; }

	[JsonProperty("xapJmSplitCosts", Order = 163)]
	public byte xapJmSplitCosts { get; set; }

	[JsonProperty("xapJmStandardFactor", Order = 164)]
	[Required(ErrorMessage = "xapJmStandardFactor is required.")]
	[MaxLength(2)]
	public string xapJmStandardFactor { get; set; }

	[JsonProperty("xapLmCalculateEndTime", Order = 165)]
	public byte xapLmCalculateEndTime { get; set; }

	[JsonProperty("xapLmLeaveBoardFields", Order = 166)]
	[MaxLength(50)]
	public string xapLmLeaveBoardFields { get; set; }

	[JsonProperty("xapLOResponseMethodID", Order = 167)]
	[MaxLength(5)]
	public string xapLOResponseMethodID { get; set; }

	[JsonProperty("xapNextSerialNumberIDFormula", Order = 168)]
	[MaxLength(50)]
	public string xapNextSerialNumberIDFormula { get; set; }

	[JsonProperty("xapOmAddlChargePartID", Order = 169)]
	[MaxLength(30)]
	public string xapOmAddlChargePartID { get; set; }

	[JsonProperty("xapOmAddlChargePartRevisionID", Order = 170)]
	[MaxLength(15)]
	public string xapOmAddlChargePartRevisionID { get; set; }

	[JsonProperty("xapOmDeliveryType", Order = 171)]
	public byte xapOmDeliveryType { get; set; }

	[JsonProperty("xapOmFreeOnBoardDescription", Order = 172)]
	[MaxLength(15)]
	public string xapOmFreeOnBoardDescription { get; set; }

	[JsonProperty("xapOmLineQuantityValidation", Order = 173)]
	public byte xapOmLineQuantityValidation { get; set; }

	[JsonProperty("xapOmOrderDeliveryDigits", Order = 174)]
	public byte xapOmOrderDeliveryDigits { get; set; }

	[JsonProperty("xapOmOrderLineDigits", Order = 175)]
	public byte xapOmOrderLineDigits { get; set; }

	[JsonProperty("xapOmSalesGlAccountID", Order = 176)]
	[MaxLength(11)]
	public string xapOmSalesGlAccountID { get; set; }

	[JsonProperty("xapOmUnitOfMeasure", Order = 177)]
	[MaxLength(2)]
	public string xapOmUnitOfMeasure { get; set; }

	[JsonProperty("xapPACalendarExportFields", Order = 178)]
	[MaxLength(50)]
	public string xapPACalendarExportFields { get; set; }

	[JsonProperty("xapPAExportFormat", Order = 179)]
	[MaxLength(10)]
	public string xapPAExportFormat { get; set; }

	[JsonProperty("xapPAExportLocation", Order = 180)]
	[MaxLength(50)]
	public string xapPAExportLocation { get; set; }

	[JsonProperty("xapPmCostingMethod", Order = 181)]
	[Required(ErrorMessage = "xapPmCostingMethod is required.")]
	public byte xapPmCostingMethod { get; set; }

	[JsonProperty("xapPmDefaultDueDate", Order = 182)]
	[Required(ErrorMessage = "xapPmDefaultDueDate is required.")]
	public DateTime? xapPmDefaultDueDate { get; set; }

	[JsonProperty("xapPmFollowUpDays", Order = 183)]
	public short xapPmFollowUpDays { get; set; }

	[JsonProperty("xapPmPoWizardDisplayType", Order = 184)]
	[Required(ErrorMessage = "xapPmPoWizardDisplayType is required.")]
	public byte xapPmPoWizardDisplayType { get; set; }

	[JsonProperty("xapPmPurchaseType", Order = 185)]
	[Required(ErrorMessage = "xapPmPurchaseType is required.")]
	public byte xapPmPurchaseType { get; set; }

	[JsonProperty("xapPmTaxExemptNumber", Order = 186)]
	[MaxLength(16)]
	public string xapPmTaxExemptNumber { get; set; }

	[JsonProperty("xapPRLaborMethod", Order = 187)]
	[Required(ErrorMessage = "xapPRLaborMethod is required.")]
	public byte xapPRLaborMethod { get; set; }

	[JsonProperty("xapQAInspQueueRefreshInterval", Order = 188)]
	public short xapQAInspQueueRefreshInterval { get; set; }

	[JsonProperty("xapQmAdditionalChargeText", Order = 189)]
	[MaxLength(50)]
	public string xapQmAdditionalChargeText { get; set; }

	[JsonProperty("xapQmExpirationDays", Order = 190)]
	public short xapQmExpirationDays { get; set; }

	[JsonProperty("xapQmFollowUpDays", Order = 191)]
	public short xapQmFollowUpDays { get; set; }

	[JsonProperty("xapQmFollowUpType", Order = 192)]
	[Required(ErrorMessage = "xapQmFollowUpType is required.")]
	public byte xapQmFollowUpType { get; set; }

	[JsonProperty("xapQmLaborMarkup", Order = 193)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xapQmLaborMarkup { get; set; }

	[JsonProperty("xapQmMaterialMarkup", Order = 194)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xapQmMaterialMarkup { get; set; }

	[JsonProperty("xapQmMQuoteFooterMessageRTF", Order = 195)]
	[MaxLength(50)]
	public string xapQmMQuoteFooterMessageRTF { get; set; }

	[JsonProperty("xapQmMQuoteFooterMessageText", Order = 196)]
	[MaxLength(50)]
	public string xapQmMQuoteFooterMessageText { get; set; }

	[JsonProperty("xapQmMQuoteHeaderMessageRTF", Order = 197)]
	[MaxLength(50)]
	public string xapQmMQuoteHeaderMessageRTF { get; set; }

	[JsonProperty("xapQmMQuoteHeaderMessageText", Order = 198)]
	[MaxLength(50)]
	public string xapQmMQuoteHeaderMessageText { get; set; }

	[JsonProperty("xapQmOverheadMarkup", Order = 199)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xapQmOverheadMarkup { get; set; }

	[JsonProperty("xapQmPurchaseToOrderMarkup", Order = 200)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xapQmPurchaseToOrderMarkup { get; set; }

	[JsonProperty("xapQmQuoteFooterMessageRTF", Order = 201)]
	[MaxLength(50)]
	public string xapQmQuoteFooterMessageRTF { get; set; }

	[JsonProperty("xapQmQuoteFooterMessageText", Order = 202)]
	[MaxLength(50)]
	public string xapQmQuoteFooterMessageText { get; set; }

	[JsonProperty("xapQmQuoteHeaderMessageRTF", Order = 203)]
	[MaxLength(50)]
	public string xapQmQuoteHeaderMessageRTF { get; set; }

	[JsonProperty("xapQmQuoteHeaderMessageText", Order = 204)]
	[MaxLength(50)]
	public string xapQmQuoteHeaderMessageText { get; set; }

	[JsonProperty("xapQmQuoteMarkupType", Order = 205)]
	[Required(ErrorMessage = "xapQmQuoteMarkupType is required.")]
	public byte xapQmQuoteMarkupType { get; set; }

	[JsonProperty("xapQmQuotingMarkup", Order = 206)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xapQmQuotingMarkup { get; set; }

	[JsonProperty("xapQmQuotingMethod", Order = 207)]
	[Required(ErrorMessage = "xapQmQuotingMethod is required.")]
	public byte xapQmQuotingMethod { get; set; }

	[JsonProperty("xapQmSubcontractMarkup", Order = 208)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xapQmSubcontractMarkup { get; set; }

	[JsonProperty("xapRowVersion", Order = 209)]
	public byte[] xapRowVersion { get; set; }

	[JsonProperty("xapSfeActiveJobQueueFields", Order = 210)]
	[MaxLength(50)]
	public string xapSfeActiveJobQueueFields { get; set; }

	[JsonProperty("xapSfeAddPartSelect", Order = 211)]
	[MaxLength(50)]
	public string xapSfeAddPartSelect { get; set; }

	[JsonProperty("xapSfeAsmSearchFields", Order = 212)]
	[MaxLength(50)]
	public string xapSfeAsmSearchFields { get; set; }

	[JsonProperty("xapSfeEndJobCompletionCode", Order = 213)]
	[Range(0, 9, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xapSfeEndJobCompletionCode { get; set; }

	[JsonProperty("xapSfeEndJobGoodQty", Order = 214)]
	[MaxLength(50)]
	public string xapSfeEndJobGoodQty { get; set; }

	[JsonProperty("xapSfeEndJobScrapQty", Order = 215)]
	[MaxLength(50)]
	public string xapSfeEndJobScrapQty { get; set; }

	[JsonProperty("xapSfeIssueMaterialQty", Order = 216)]
	[MaxLength(50)]
	public string xapSfeIssueMaterialQty { get; set; }

	[JsonProperty("xapSfeJobSearchSelect", Order = 217)]
	[MaxLength(50)]
	public string xapSfeJobSearchSelect { get; set; }

	[JsonProperty("xapSfeJobTraveller", Order = 218)]
	[MaxLength(50)]
	public string xapSfeJobTraveller { get; set; }

	[JsonProperty("xapSfeOprSearchFields", Order = 219)]
	[MaxLength(50)]
	public string xapSfeOprSearchFields { get; set; }

	[JsonProperty("xapSfeSetupPercentage", Order = 220)]
	[MaxLength(3)]
	public string xapSfeSetupPercentage { get; set; }

	[JsonProperty("xapSfeStartJobWorkCode", Order = 221)]
	[Range(0, 9, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xapSfeStartJobWorkCode { get; set; }

	[JsonProperty("xapSfeTCAuditReport", Order = 222)]
	[MaxLength(50)]
	public string xapSfeTCAuditReport { get; set; }

	[JsonProperty("xapSfeWorkQueueFields", Order = 223)]
	[MaxLength(50)]
	public string xapSfeWorkQueueFields { get; set; }

	[JsonProperty("xapSfeWorkQueueSort", Order = 224)]
	[MaxLength(50)]
	public string xapSfeWorkQueueSort { get; set; }

	[JsonProperty("xapShowQtyOnHandMobInv", Order = 225)]
	public bool xapShowQtyOnHandMobInv { get; set; }

	[JsonProperty("xapSmEdi856CustomLabel", Order = 226)]
	[MaxLength(35)]
	public string xapSmEdi856CustomLabel { get; set; }

	[JsonProperty("xapSmLineQuantityValidation", Order = 227)]
	public byte xapSmLineQuantityValidation { get; set; }

	[JsonProperty("customFields", Order = 228)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
