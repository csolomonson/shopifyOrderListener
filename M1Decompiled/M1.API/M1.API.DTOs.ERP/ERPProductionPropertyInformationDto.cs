using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProductionPropertyInformationDto
{
	public string xapAllowNegQtyOnHandHistory { get; set; }

	public string xapAnonymousCustomerID { get; set; }

	public string xapChChangeRequestTypeID { get; set; }

	public byte xapCmArPaymentCreditMessage { get; set; }

	public byte xapCmArPaymentHoldMessage { get; set; }

	public byte xapCmFieldServiceCreditMessage { get; set; }

	public byte xapCmFieldServiceHoldMessage { get; set; }

	public string xapCmNonTaxReasonID { get; set; }

	public byte xapCmOrderCreditMessage { get; set; }

	public byte xapCmOrderHoldMessage { get; set; }

	public byte xapCmSalesPersonDefaultLoc { get; set; }

	public byte xapCmShipmentCreditMessage { get; set; }

	public byte xapCmShipmentHoldMessage { get; set; }

	public string xapCostingMethodHistory { get; set; }

	public string xapCreatedBy { get; set; }

	public DateTime? xapCreatedDate { get; set; }

	public DateTime? xapDateToSchedule { get; set; }

	public string xapDcAutoClockOutComputer { get; set; }

	public byte xapDcIdleTimeThreshhold { get; set; }

	public byte xapDcLaborCalculationMethod { get; set; }

	public byte xapDcPayCalculationMethod { get; set; }

	public byte xapDcRefreshInterval { get; set; }

	public string xapDcSfeInspectionPassword { get; set; }

	public string xapDcSfeShutdownPassword { get; set; }

	public byte xapDcTimeFormat { get; set; }

	public string xapDMDefaultFolder { get; set; }

	public string xapEdi810ServiceUrl { get; set; }

	public string xapEdi856ServiceUrl { get; set; }

	public string xapEdiPassword { get; set; }

	public string xapEdiUserName { get; set; }

	public Guid xapUniqueID { get; set; }

	public string xapHdAttachmentFilePath { get; set; }

	public short xapHdcallDueDateDays { get; set; }

	public string xapHdcallTypeID { get; set; }

	public string xapHdcontactMethodID { get; set; }

	public string xapHdMailMergeCallTypeID { get; set; }

	public string xapHdMailMergeContactMethodID { get; set; }

	public string xapHdNewCallSoundFile { get; set; }

	public string xapHdSalesCallTypeID { get; set; }

	public string xapImAutoCreateRevisionID { get; set; }

	public byte xapImCostingMethod { get; set; }

	public byte xapImMfgDefaultCostType { get; set; }

	public bool xapCmCreateJobOnly { get; set; }

	public bool xapCmCreditLimitSourceInv { get; set; }

	public bool xapCmCreditLimitSourceOrder { get; set; }

	public bool xapCmCreditLimitSourceShip { get; set; }

	public bool xapCmCustomerTaxable { get; set; }

	public bool xapCmEnableResellers { get; set; }

	public bool xapCmIncludeFreightInPrice { get; set; }

	public bool xapDcAllowNegativeQty { get; set; }

	public bool xapDcAllowProductionComplete { get; set; }

	public bool xapDcAutoClockOutLocked { get; set; }

	public bool xapDcEnableCreateSequence { get; set; }

	public bool xapDcEnableIssueMaterial { get; set; }

	public bool xapDcEnableJobTraveler { get; set; }

	public bool xapDcEnableMinimizeButtonInSfe { get; set; }

	public bool xapDcEnableTimecardAudit { get; set; }

	public bool xapDcEnableWorkQueue { get; set; }

	public bool xapDcPromptForActivityPassword { get; set; }

	public bool xapDcPromptForAuditPassword { get; set; }

	public bool xapDcPromptForClockInPassword { get; set; }

	public bool xapDcPromptForLaborDescription { get; set; }

	public bool xapDcPromptForMessagePassword { get; set; }

	public bool xapDcPromptForReason { get; set; }

	public bool xapDcShowCurrentJobsOnly { get; set; }

	public bool xapDcSplitDirectLaborHours { get; set; }

	public bool xapDcSplitIndirectLaborHours { get; set; }

	public bool xapDcUseServerTime { get; set; }

	public bool xapDcWarnOnOutsideOperation { get; set; }

	public bool xapDcWarnOnOverProduction { get; set; }

	public bool xapGlCreateStockJournals { get; set; }

	public bool xapHdcreateCallForEmails { get; set; }

	public bool xapImAllowNegativeQtyOnHand { get; set; }

	public bool xapImAutoCreateRevision { get; set; }

	public bool xapImCopyAlternates { get; set; }

	public bool xapImCopyPartMemos { get; set; }

	public bool xapImCopyPartOrgReferences { get; set; }

	public bool xapImCopyPartPrices { get; set; }

	public bool xapImCopyPartRules { get; set; }

	public bool xapImEnableOrgPartCustomer { get; set; }

	public bool xapImEnableOrgPartSupplier { get; set; }

	public bool xapImEnableWarningWhenNegative { get; set; }

	public bool xapImForceConfiguratorScreens { get; set; }

	public bool xapImHideUseMethodInTree { get; set; }

	public bool xapImIgnoreLCInStdCostRollup { get; set; }

	public bool xapImOnlyAllowExistingBins { get; set; }

	public bool xapImOverwriteDescription { get; set; }

	public bool xapImOverwriteDocuments { get; set; }

	public bool xapImOverwriteMethod { get; set; }

	public bool xapImRefreshMaterial { get; set; }

	public bool xapImRefreshMaterialCosts { get; set; }

	public bool xapImScrapRoundUp { get; set; }

	public bool xapImSetUseMethod { get; set; }

	public bool xapImTransferCustomer { get; set; }

	public bool xapImTransferDescriptions { get; set; }

	public bool xapImTransferMaterial { get; set; }

	public bool xapImUseStdForStdCostRollUp { get; set; }

	public bool xapJmExcessQuantity { get; set; }

	public bool xapJmIgnoreEmployees { get; set; }

	public bool xapJmIgnoreMachines { get; set; }

	public bool xapJmLoadLevelFinite { get; set; }

	public bool xapJmMinimizeGaps { get; set; }

	public bool xapJmMRPForecastFirmJob { get; set; }

	public bool xapJmOverwriteDescription { get; set; }

	public bool xapJmOverwriteDocuments { get; set; }

	public bool xapJmOverwriteMethod { get; set; }

	public bool xapJmRefreshHours { get; set; }

	public bool xapJmRefreshMaterial { get; set; }

	public bool xapJmRefreshMaterialCosts { get; set; }

	public bool xapJmScheduleShowActualTimes { get; set; }

	public bool xapJmScheduleUseActuals { get; set; }

	public bool xapJmShopLoadShowFutureLoad { get; set; }

	public bool xapJmShopLoadShowPastLoad { get; set; }

	public bool xapLmUpdateActualWithRounded { get; set; }

	public bool xapNextSerialNumberPerGroup { get; set; }

	public bool xapOmAutoCreateDelivery { get; set; }

	public bool xapOmEnableDiscountFields { get; set; }

	public bool xapOmEnableFreightFields { get; set; }

	public bool xapOmIncludeOrderDeliveryInJob { get; set; }

	public bool xapOmIncludeOrderLineInJob { get; set; }

	public bool xapOmMarkCreateJobForMto { get; set; }

	public bool xapOmMarkPullQuoteMethodForMto { get; set; }

	public bool xapOmShowDeliveriesInTree { get; set; }

	public bool xapOmUseQuotingMarkupTM { get; set; }

	public bool xapPmPTOUsesDeliveryCost { get; set; }

	public bool xapPmPurPlannerIncWhsQties { get; set; }

	public bool xapPmPurPlannerUseBestPrice { get; set; }

	public bool xapPmShowFirmOnlyPoWiz { get; set; }

	public bool xapPoWizardShowQtyToInspect { get; set; }

	public bool xapPRUseFirmQuotesOnly { get; set; }

	public bool xapQArmaRequiresInspection { get; set; }

	public bool xapQAShowRmaOtherInfo { get; set; }

	public bool xapQmMultipleQuantities { get; set; }

	public bool xapQmMUseDefHeaderFooterText { get; set; }

	public bool xapQmOverwriteDescription { get; set; }

	public bool xapQmOverwriteDocuments { get; set; }

	public bool xapQmOverwriteMethod { get; set; }

	public bool xapQmRefreshMaterial { get; set; }

	public bool xapQmRefreshMaterialCosts { get; set; }

	public bool xapQmRefreshRateInfo { get; set; }

	public bool xapRQGroupPobyRfq { get; set; }

	public bool xapRQIncludeAlternateParts { get; set; }

	public bool xapSfeAllowSuspend { get; set; }

	public bool xapSfeBarcodeScanner { get; set; }

	public bool xapSfeTouchScreen { get; set; }

	public bool xapSmDeleteZeroShipmentLines { get; set; }

	public string xapJmCalendarExportFields { get; set; }

	public string xapJmInitialExtension { get; set; }

	public string xapJmInsideInspectionLineRTF { get; set; }

	public string xapJmInsideInspectionLineText { get; set; }

	public byte xapJmJobMaterialSource { get; set; }

	public byte xapJmLoadReliefMethod { get; set; }

	public string xapJmOutsideInspectionLineRTF { get; set; }

	public string xapJmOutsideInspectionLineText { get; set; }

	public string xapJmScheduleBoardFields { get; set; }

	public byte xapJmScheduleType { get; set; }

	public byte xapJmShopLoadBuckets { get; set; }

	public byte xapJmShopLoadDays { get; set; }

	public string xapJmShopLoadDepartmentID { get; set; }

	public string xapJmShopLoadFields { get; set; }

	public string xapJmShopLoadPlantID { get; set; }

	public string xapJmShopLoadTimeType { get; set; }

	public byte xapJmSplitCosts { get; set; }

	public string xapJmStandardFactor { get; set; }

	public byte xapLmCalculateEndTime { get; set; }

	public string xapLmLeaveBoardFields { get; set; }

	public string xapLOResponseMethodID { get; set; }

	public string xapNextSerialNumberIDFormula { get; set; }

	public string xapOmAddlChargePartID { get; set; }

	public string xapOmAddlChargePartRevisionID { get; set; }

	public byte xapOmDeliveryType { get; set; }

	public string xapOmFreeOnBoardDescription { get; set; }

	public byte xapOmLineQuantityValidation { get; set; }

	public byte xapOmOrderDeliveryDigits { get; set; }

	public byte xapOmOrderLineDigits { get; set; }

	public string xapOmSalesGlAccountID { get; set; }

	public string xapOmUnitOfMeasure { get; set; }

	public string xapPACalendarExportFields { get; set; }

	public string xapPAExportFormat { get; set; }

	public string xapPAExportLocation { get; set; }

	public byte xapPmCostingMethod { get; set; }

	public DateTime? xapPmDefaultDueDate { get; set; }

	public short xapPmFollowUpDays { get; set; }

	public byte xapPmPoWizardDisplayType { get; set; }

	public byte xapPmPurchaseType { get; set; }

	public string xapPmTaxExemptNumber { get; set; }

	public byte xapPRLaborMethod { get; set; }

	public short xapQAInspQueueRefreshInterval { get; set; }

	public string xapQmAdditionalChargeText { get; set; }

	public short xapQmExpirationDays { get; set; }

	public short xapQmFollowUpDays { get; set; }

	public byte xapQmFollowUpType { get; set; }

	public decimal xapQmLaborMarkup { get; set; }

	public decimal xapQmMaterialMarkup { get; set; }

	public string xapQmMQuoteFooterMessageRTF { get; set; }

	public string xapQmMQuoteFooterMessageText { get; set; }

	public string xapQmMQuoteHeaderMessageRTF { get; set; }

	public string xapQmMQuoteHeaderMessageText { get; set; }

	public decimal xapQmOverheadMarkup { get; set; }

	public decimal xapQmPurchaseToOrderMarkup { get; set; }

	public string xapQmQuoteFooterMessageRTF { get; set; }

	public string xapQmQuoteFooterMessageText { get; set; }

	public string xapQmQuoteHeaderMessageRTF { get; set; }

	public string xapQmQuoteHeaderMessageText { get; set; }

	public byte xapQmQuoteMarkupType { get; set; }

	public decimal xapQmQuotingMarkup { get; set; }

	public byte xapQmQuotingMethod { get; set; }

	public decimal xapQmSubcontractMarkup { get; set; }

	public byte[] xapRowVersion { get; set; }

	public string xapSfeActiveJobQueueFields { get; set; }

	public string xapSfeAddPartSelect { get; set; }

	public string xapSfeAsmSearchFields { get; set; }

	public decimal xapSfeEndJobCompletionCode { get; set; }

	public string xapSfeEndJobGoodQty { get; set; }

	public string xapSfeEndJobScrapQty { get; set; }

	public string xapSfeIssueMaterialQty { get; set; }

	public string xapSfeJobSearchSelect { get; set; }

	public string xapSfeJobTraveller { get; set; }

	public string xapSfeOprSearchFields { get; set; }

	public string xapSfeSetupPercentage { get; set; }

	public decimal xapSfeStartJobWorkCode { get; set; }

	public string xapSfeTCAuditReport { get; set; }

	public string xapSfeWorkQueueFields { get; set; }

	public string xapSfeWorkQueueSort { get; set; }

	public bool xapShowQtyOnHandMobInv { get; set; }

	public string xapSmEdi856CustomLabel { get; set; }

	public byte xapSmLineQuantityValidation { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
