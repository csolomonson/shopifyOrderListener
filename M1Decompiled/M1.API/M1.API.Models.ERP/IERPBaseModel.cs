using System;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public interface IERPBaseModel : IAPIBaseModel, IDisposable
{
	IERPCustomTableRepository ERPCustomTableRepository { get; set; }

	IERPAgingBucketRepository ERPAgingBucketRepository { get; set; }

	IERPAPInvoiceExpenseAccountRepository ERPAPInvoiceExpenseAccountRepository { get; set; }

	IERPAPInvoiceLineRepository ERPAPInvoiceLineRepository { get; set; }

	IERPAPInvoiceMemoRepository ERPAPInvoiceMemoRepository { get; set; }

	IERPAPInvoiceRepository ERPAPInvoiceRepository { get; set; }

	IERPAPPaymentHeaderRepository ERPAPPaymentHeaderRepository { get; set; }

	IERPAPPaymentLineRepository ERPAPPaymentLineRepository { get; set; }

	IERPAPPaymentSessionRepository ERPAPPaymentSessionRepository { get; set; }

	IERPARInvoiceLineRepository ERPARInvoiceLineRepository { get; set; }

	IERPARInvoiceMemoRepository ERPARInvoiceMemoRepository { get; set; }

	IERPARInvoiceRepository ERPARInvoiceRepository { get; set; }

	IERPARInvoiceSalesPersonRepository ERPARInvoiceSalesPersonRepository { get; set; }

	IERPARPaymentHeaderRepository ERPARPaymentHeaderRepository { get; set; }

	IERPARPaymentLineRepository ERPARPaymentLineRepository { get; set; }

	IERPARPaymentSessionRepository ERPARPaymentSessionRepository { get; set; }

	IERPAssetAdjustmentRepository ERPAssetAdjustmentRepository { get; set; }

	IERPAssetLowValuePoolRepository ERPAssetLowValuePoolRepository { get; set; }

	IERPAssetMemoRepository ERPAssetMemoRepository { get; set; }

	IERPAssetPoolTransactionRepository ERPAssetPoolTransactionRepository { get; set; }

	IERPAssetRepository ERPAssetRepository { get; set; }

	IERPAssetScheduleRepository ERPAssetScheduleRepository { get; set; }

	IERPAssetTypeMethodRepository ERPAssetTypeMethodRepository { get; set; }

	IERPAssetTypePlantRepository ERPAssetTypePlantRepository { get; set; }

	IERPAssetTypeRepository ERPAssetTypeRepository { get; set; }

	IERPAttachmentMemoRepository ERPAttachmentMemoRepository { get; set; }

	IERPAttachmentRepository ERPAttachmentRepository { get; set; }

	IERPAttachmentTypeRepository ERPAttachmentTypeRepository { get; set; }

	IERPBankAccountRepository ERPBankAccountRepository { get; set; }

	IERPBankEntryRepository ERPBankEntryRepository { get; set; }

	IERPBankStatementRepository ERPBankStatementRepository { get; set; }

	IERPCallLineRepository ERPCallLineRepository { get; set; }

	IERPCallMemoRepository ERPCallMemoRepository { get; set; }

	IERPCallRepository ERPCallRepository { get; set; }

	IERPCallTypeRepository ERPCallTypeRepository { get; set; }

	IERPChangeLogRepository ERPChangeLogRepository { get; set; }

	IERPChangeRequestGroupLinkRepository ERPChangeRequestGroupLinkRepository { get; set; }

	IERPChangeRequestGroupRepository ERPChangeRequestGroupRepository { get; set; }

	IERPChangeRequestRepository ERPChangeRequestRepository { get; set; }

	IERPChangeRequestTypeRepository ERPChangeRequestTypeRepository { get; set; }

	IERPContactGroupRepository ERPContactGroupRepository { get; set; }

	IERPContactMethodRepository ERPContactMethodRepository { get; set; }

	IERPContactTitleRepository ERPContactTitleRepository { get; set; }

	IERPCorrectiveActionCategoryRepository ERPCorrectiveActionCategoryRepository { get; set; }

	IERPCorrectiveActionCodeRepository ERPCorrectiveActionCodeRepository { get; set; }

	IERPCountyCodeRepository ERPCountyCodeRepository { get; set; }

	IERPCurrencyRateLineRepository ERPCurrencyRateLineRepository { get; set; }

	IERPCurrencyRateRepository ERPCurrencyRateRepository { get; set; }

	IERPCustomerGroupRepository ERPCustomerGroupRepository { get; set; }

	IERPCustomerPackageRepository ERPCustomerPackageRepository { get; set; }

	IERPCycleCodeRepository ERPCycleCodeRepository { get; set; }

	IERPDatasetPropertyRepository ERPDatasetPropertyRepository { get; set; }

	IERPDMRClaimComponentRepository ERPDMRClaimComponentRepository { get; set; }

	IERPDMRClaimLineRepository ERPDMRClaimLineRepository { get; set; }

	IERPDMRClaimRepository ERPDMRClaimRepository { get; set; }

	IERPDMRShipmentComponentRepository ERPDMRShipmentComponentRepository { get; set; }

	IERPDMRShipmentLineRepository ERPDMRShipmentLineRepository { get; set; }

	IERPDMRShipmentRepository ERPDMRShipmentRepository { get; set; }

	IERPDocumentLinkRepository ERPDocumentLinkRepository { get; set; }

	IERPEmployeeAttachmentRepository ERPEmployeeAttachmentRepository { get; set; }

	IERPEmployeeMemoRepository ERPEmployeeMemoRepository { get; set; }

	IERPEmployeePersonalDatumRepository ERPEmployeePersonalDatumRepository { get; set; }

	IERPEmployeePOApprovalRepository ERPEmployeePOApprovalRepository { get; set; }

	IERPEmployeeQAApprovalRepository ERPEmployeeQAApprovalRepository { get; set; }

	IERPEmployeeRepository ERPEmployeeRepository { get; set; }

	IERPEmployeeSalesBudgetLineRepository ERPEmployeeSalesBudgetLineRepository { get; set; }

	IERPEmployeeSalesBudgetRepository ERPEmployeeSalesBudgetRepository { get; set; }

	IERPEmployeeSkillCompetencyRepository ERPEmployeeSkillCompetencyRepository { get; set; }

	IERPEmployeeSkillRepository ERPEmployeeSkillRepository { get; set; }

	IERPEmployeeSOApprovalRepository ERPEmployeeSOApprovalRepository { get; set; }

	IERPExpenseAccountSplitRepository ERPExpenseAccountSplitRepository { get; set; }

	IERPExpenseRepository ERPExpenseRepository { get; set; }

	IERPFinancialPropertyRepository ERPFinancialPropertyRepository { get; set; }

	IERPFollowupRepository ERPFollowupRepository { get; set; }

	IERPFreightPackageLinkRepository ERPFreightPackageLinkRepository { get; set; }

	IERPFreightPackageRateRepository ERPFreightPackageRateRepository { get; set; }

	IERPFreightPackageRepository ERPFreightPackageRepository { get; set; }

	IERPFreightReferenceRepository ERPFreightReferenceRepository { get; set; }

	IERPFreightShipmentRepository ERPFreightShipmentRepository { get; set; }

	IERPGLAccountRepository ERPGLAccountRepository { get; set; }

	IERPGLCategoryRepository ERPGLCategoryRepository { get; set; }

	IERPGLChartRepository ERPGLChartRepository { get; set; }

	IERPGLDepartmentRepository ERPGLDepartmentRepository { get; set; }

	IERPGLDivisionRepository ERPGLDivisionRepository { get; set; }

	IERPGLFiscalYearBudgetAmountRepository ERPGLFiscalYearBudgetAmountRepository { get; set; }

	IERPGLFiscalYearBudgetHeaderRepository ERPGLFiscalYearBudgetHeaderRepository { get; set; }

	IERPGLFiscalYearBudgetLineRepository ERPGLFiscalYearBudgetLineRepository { get; set; }

	IERPGLFiscalYearOpeningBalanceRepository ERPGLFiscalYearOpeningBalanceRepository { get; set; }

	IERPGLFiscalYearPeriodMovementRepository ERPGLFiscalYearPeriodMovementRepository { get; set; }

	IERPGLFiscalYearPeriodRepository ERPGLFiscalYearPeriodRepository { get; set; }

	IERPGLFiscalYearRepository ERPGLFiscalYearRepository { get; set; }

	IERPGLJournalLineRepository ERPGLJournalLineRepository { get; set; }

	IERPGLJournalMemoRepository ERPGLJournalMemoRepository { get; set; }

	IERPGLJournalRepository ERPGLJournalRepository { get; set; }

	IERPIndirectLaborCodeRepository ERPIndirectLaborCodeRepository { get; set; }

	IERPIndustryTypeRepository ERPIndustryTypeRepository { get; set; }

	IERPInspectionComponentRepository ERPInspectionComponentRepository { get; set; }

	IERPInspectionLineApprovalRepository ERPInspectionLineApprovalRepository { get; set; }

	IERPInspectionLineRepository ERPInspectionLineRepository { get; set; }

	IERPInspectionRepository ERPInspectionRepository { get; set; }

	IERPInventoryCountLineRepository ERPInventoryCountLineRepository { get; set; }

	IERPInventoryCountRepository ERPInventoryCountRepository { get; set; }

	IERPJobAssemblyRepository ERPJobAssemblyRepository { get; set; }

	IERPJobCostRepository ERPJobCostRepository { get; set; }

	IERPJobMaterialComponentRepository ERPJobMaterialComponentRepository { get; set; }

	IERPJobMaterialRepository ERPJobMaterialRepository { get; set; }

	IERPJobMemoRepository ERPJobMemoRepository { get; set; }

	IERPJobOperationRepository ERPJobOperationRepository { get; set; }

	IERPJobPriorityRepository ERPJobPriorityRepository { get; set; }

	IERPJobRepository ERPJobRepository { get; set; }

	IERPJobScenarioRepository ERPJobScenarioRepository { get; set; }

	IERPKnowledgeBasePageRepository ERPKnowledgeBasePageRepository { get; set; }

	IERPLandedCostCategoryRepository ERPLandedCostCategoryRepository { get; set; }

	IERPLandedCostChargeDetailRepository ERPLandedCostChargeDetailRepository { get; set; }

	IERPLandedCostChargeRepository ERPLandedCostChargeRepository { get; set; }

	IERPLandedCostRepository ERPLandedCostRepository { get; set; }

	IERPLaserCalculatorLineRepository ERPLaserCalculatorLineRepository { get; set; }

	IERPLaserCalculatorRepository ERPLaserCalculatorRepository { get; set; }

	IERPLeadCompetitorRepository ERPLeadCompetitorRepository { get; set; }

	IERPLeadLineRepository ERPLeadLineRepository { get; set; }

	IERPLeadMemoRepository ERPLeadMemoRepository { get; set; }

	IERPLeadRepository ERPLeadRepository { get; set; }

	IERPLeadSalesPersonRepository ERPLeadSalesPersonRepository { get; set; }

	IERPLotNumberRepository ERPLotNumberRepository { get; set; }

	IERPLotNumberStatusRepository ERPLotNumberStatusRepository { get; set; }

	IERPLotNumberTransactionRepository ERPLotNumberTransactionRepository { get; set; }

	IERPMarketingProgramRepository ERPMarketingProgramRepository { get; set; }

	IERPMaterialIssueComponentRepository ERPMaterialIssueComponentRepository { get; set; }

	IERPMaterialIssueLineRepository ERPMaterialIssueLineRepository { get; set; }

	IERPMaterialIssueRepository ERPMaterialIssueRepository { get; set; }

	IERPMfgReceiptComponentRepository ERPMfgReceiptComponentRepository { get; set; }

	IERPMfgReceiptRepository ERPMfgReceiptRepository { get; set; }

	IERPMilestoneRepository ERPMilestoneRepository { get; set; }

	IERPMRPDemandRepository ERPMRPDemandRepository { get; set; }

	IERPMRPJobDetailRepository ERPMRPJobDetailRepository { get; set; }

	IERPMRPLineRepository ERPMRPLineRepository { get; set; }

	IERPMRPSessionRepository ERPMRPSessionRepository { get; set; }

	IERPMRPSupplyRepository ERPMRPSupplyRepository { get; set; }

	IERPNextIDRepository ERPNextIDRepository { get; set; }

	IERPNonConformanceCategoryRepository ERPNonConformanceCategoryRepository { get; set; }

	IERPNonConformanceCauseRepository ERPNonConformanceCauseRepository { get; set; }

	IERPNonConformanceCodeRepository ERPNonConformanceCodeRepository { get; set; }

	IERPNonConformanceRepository ERPNonConformanceRepository { get; set; }

	IERPOrganizationContactGroupLinkRepository ERPOrganizationContactGroupLinkRepository { get; set; }

	IERPOrganizationContactRepository ERPOrganizationContactRepository { get; set; }

	IERPOrganizationIndustryTypeLinkRepository ERPOrganizationIndustryTypeLinkRepository { get; set; }

	IERPOrganizationLocationRepository ERPOrganizationLocationRepository { get; set; }

	IERPOrganizationLocSalesPersonRepository ERPOrganizationLocSalesPersonRepository { get; set; }

	IERPOrganizationMemoRepository ERPOrganizationMemoRepository { get; set; }

	IERPOrganizationRepository ERPOrganizationRepository { get; set; }

	IERPPartAlternateRepository ERPPartAlternateRepository { get; set; }

	IERPPartAssemblyRepository ERPPartAssemblyRepository { get; set; }

	IERPPartBinDetailRepository ERPPartBinDetailRepository { get; set; }

	IERPPartBinRepository ERPPartBinRepository { get; set; }

	IERPPartClassRepository ERPPartClassRepository { get; set; }

	IERPPartClassPlantRepository ERPPartClassPlantRepository { get; set; }

	IERPPartCrossReferenceRepository ERPPartCrossReferenceRepository { get; set; }

	IERPPartForecastLineRepository ERPPartForecastLineRepository { get; set; }

	IERPPartForecastRepository ERPPartForecastRepository { get; set; }

	IERPPartGroupPlantRepository ERPPartGroupPlantRepository { get; set; }

	IERPPartGroupRepository ERPPartGroupRepository { get; set; }

	IERPPartMaterialRepository ERPPartMaterialRepository { get; set; }

	IERPPartMemoRepository ERPPartMemoRepository { get; set; }

	IERPPartOperationRepository ERPPartOperationRepository { get; set; }

	IERPPartOrgReferenceRepository ERPPartOrgReferenceRepository { get; set; }

	IERPPartPriceBreakRepository ERPPartPriceBreakRepository { get; set; }

	IERPPartPriceRepository ERPPartPriceRepository { get; set; }

	IERPPartRevisionRepository ERPPartRevisionRepository { get; set; }

	IERPPartRuleRepository ERPPartRuleRepository { get; set; }

	IERPPartRepository ERPPartRepository { get; set; }

	IERPPartTransactionCostRepository ERPPartTransactionCostRepository { get; set; }

	IERPPartTransactionRepository ERPPartTransactionRepository { get; set; }

	IERPPartUnitSalePriceRepository ERPPartUnitSalePriceRepository { get; set; }

	IERPPartWarehouseLocationRepository ERPPartWarehouseLocationRepository { get; set; }

	IERPPaymentMethodRepository ERPPaymentMethodRepository { get; set; }

	IERPPlantDepartmentRepository ERPPlantDepartmentRepository { get; set; }

	IERPPlantRepository ERPPlantRepository { get; set; }

	IERPPriorityRepository ERPPriorityRepository { get; set; }

	IERPProcessRepository ERPProcessRepository { get; set; }

	IERPProductCategoryRepository ERPProductCategoryRepository { get; set; }

	IERPProductCategoryLineRepository ERPProductCategoryLineRepository { get; set; }

	IERPProductionCalendarDayRepository ERPProductionCalendarDayRepository { get; set; }

	IERPProductionCalendarRepository ERPProductionCalendarRepository { get; set; }

	IERPProductionCalendarWorkCenterRepository ERPProductionCalendarWorkCenterRepository { get; set; }

	IERPProductionDepartmentRepository ERPProductionDepartmentRepository { get; set; }

	IERPProductionPropertyRepository ERPProductionPropertyRepository { get; set; }

	IERPProjectAreaRepository ERPProjectAreaRepository { get; set; }

	IERPProjectContactRepository ERPProjectContactRepository { get; set; }

	IERPProjectedPaymentRepository ERPProjectedPaymentRepository { get; set; }

	IERPProjectRepository ERPProjectRepository { get; set; }

	IERPProjectTypeRepository ERPProjectTypeRepository { get; set; }

	IERPPunchCalculatorRepository ERPPunchCalculatorRepository { get; set; }

	IERPPurchaseOrderAccountRepository ERPPurchaseOrderAccountRepository { get; set; }

	IERPPurchaseOrderApprovalRepository ERPPurchaseOrderApprovalRepository { get; set; }

	IERPPurchaseOrderComponentRepository ERPPurchaseOrderComponentRepository { get; set; }

	IERPPurchaseOrderDeliveryRepository ERPPurchaseOrderDeliveryRepository { get; set; }

	IERPPurchaseOrderLineRepository ERPPurchaseOrderLineRepository { get; set; }

	IERPPurchaseOrderMemoRepository ERPPurchaseOrderMemoRepository { get; set; }

	IERPPurchaseOrderRepository ERPPurchaseOrderRepository { get; set; }

	IERPPurchasePlannerLineRepository ERPPurchasePlannerLineRepository { get; set; }

	IERPPurchasePlannerOrderDetailRepository ERPPurchasePlannerOrderDetailRepository { get; set; }

	IERPPurchasePlannerRequirementRepository ERPPurchasePlannerRequirementRepository { get; set; }

	IERPPurchasePlannerSessionRepository ERPPurchasePlannerSessionRepository { get; set; }

	IERPQuantityAdjustmentRepository ERPQuantityAdjustmentRepository { get; set; }

	IERPQuoteAssemblyRepository ERPQuoteAssemblyRepository { get; set; }

	IERPQuoteLineRepository ERPQuoteLineRepository { get; set; }

	IERPQuoteMaterialRepository ERPQuoteMaterialRepository { get; set; }

	IERPQuoteMemoRepository ERPQuoteMemoRepository { get; set; }

	IERPQuoteOperationRepository ERPQuoteOperationRepository { get; set; }

	IERPQuoteQuantityRepository ERPQuoteQuantityRepository { get; set; }

	IERPQuoteRepository ERPQuoteRepository { get; set; }

	IERPQuoteSalesPersonRepository ERPQuoteSalesPersonRepository { get; set; }

	IERPReasonPlantRepository ERPReasonPlantRepository { get; set; }

	IERPReasonRepository ERPReasonRepository { get; set; }

	IERPReceiptComponentRepository ERPReceiptComponentRepository { get; set; }

	IERPReceiptLineRepository ERPReceiptLineRepository { get; set; }

	IERPReceiptRepository ERPReceiptRepository { get; set; }

	IERPRecentActivitiesLogRepository ERPRecentActivitiesLogRepository { get; set; }

	IERPRFQLineRepository ERPRFQLineRepository { get; set; }

	IERPRFQMemoRepository ERPRFQMemoRepository { get; set; }

	IERPRFQQuantityRepository ERPRFQQuantityRepository { get; set; }

	IERPRFQRepository ERPRFQRepository { get; set; }

	IERPRFQSupplierRepository ERPRFQSupplierRepository { get; set; }

	IERPRMAActionTypeRepository ERPRMAActionTypeRepository { get; set; }

	IERPRMAClaimComponentRepository ERPRMAClaimComponentRepository { get; set; }

	IERPRMAClaimLineRepository ERPRMAClaimLineRepository { get; set; }

	IERPRMAClaimRepository ERPRMAClaimRepository { get; set; }

	IERPRMAReceiptComponentRepository ERPRMAReceiptComponentRepository { get; set; }

	IERPRMAReceiptLineRepository ERPRMAReceiptLineRepository { get; set; }

	IERPRMAReceiptRepository ERPRMAReceiptRepository { get; set; }

	IERPSalesOrderApprovalRepository ERPSalesOrderApprovalRepository { get; set; }

	IERPSalesOrderComponentRepository ERPSalesOrderComponentRepository { get; set; }

	IERPSalesOrderDeliveryRepository ERPSalesOrderDeliveryRepository { get; set; }

	IERPSalesOrderJobLinkRepository ERPSalesOrderJobLinkRepository { get; set; }

	IERPSalesOrderLineRepository ERPSalesOrderLineRepository { get; set; }

	IERPSalesOrderMemoRepository ERPSalesOrderMemoRepository { get; set; }

	IERPSalesOrderPickListLineRepository ERPSalesOrderPickListLineRepository { get; set; }

	IERPSalesOrderPickListSessionRepository ERPSalesOrderPickListSessionRepository { get; set; }

	IERPSalesOrderRepository ERPSalesOrderRepository { get; set; }

	IERPSalesOrderSalesPersonRepository ERPSalesOrderSalesPersonRepository { get; set; }

	IERPScheduleAllocationRepository ERPScheduleAllocationRepository { get; set; }

	IERPScheduleBranchRepository ERPScheduleBranchRepository { get; set; }

	IERPScheduleResourceLaneRepository ERPScheduleResourceLaneRepository { get; set; }

	IERPScheduleTaskBucketRepository ERPScheduleTaskBucketRepository { get; set; }

	IERPScheduleTaskRepository ERPScheduleTaskRepository { get; set; }

	IERPScheduleTreeRepository ERPScheduleTreeRepository { get; set; }

	IERPSerialNumberRepository ERPSerialNumberRepository { get; set; }

	IERPSerialNumberStatusRepository ERPSerialNumberStatusRepository { get; set; }

	IERPSerialNumberTransactionRepository ERPSerialNumberTransactionRepository { get; set; }

	IERPServiceContractLineRepository ERPServiceContractLineRepository { get; set; }

	IERPServiceContractMemoRepository ERPServiceContractMemoRepository { get; set; }

	IERPServiceContractOwnerRepository ERPServiceContractOwnerRepository { get; set; }

	IERPServiceContractRepository ERPServiceContractRepository { get; set; }

	IERPServiceContractTypeRepository ERPServiceContractTypeRepository { get; set; }

	IERPSheetCalculatorRepository ERPSheetCalculatorRepository { get; set; }

	IERPShiftBreakRepository ERPShiftBreakRepository { get; set; }

	IERPShiftRepository ERPShiftRepository { get; set; }

	IERPShipmentComponentRepository ERPShipmentComponentRepository { get; set; }

	IERPShipmentFreightLinkRepository ERPShipmentFreightLinkRepository { get; set; }

	IERPShipmentFreightReferenceRepository ERPShipmentFreightReferenceRepository { get; set; }

	IERPShipmentLineRepository ERPShipmentLineRepository { get; set; }

	IERPShipmentPackageDetailRepository ERPShipmentPackageDetailRepository { get; set; }

	IERPShipmentPackageRepository ERPShipmentPackageRepository { get; set; }

	IERPShipmentRepository ERPShipmentRepository { get; set; }

	IERPShippingMethodRepository ERPShippingMethodRepository { get; set; }

	IERPShippingPaymentTypeRepository ERPShippingPaymentTypeRepository { get; set; }

	IERPShippingPropertyRepository ERPShippingPropertyRepository { get; set; }

	IERPSkillCompetencyRepository ERPSkillCompetencyRepository { get; set; }

	IERPSkillRepository ERPSkillRepository { get; set; }

	IERPStandardMessageRepository ERPStandardMessageRepository { get; set; }

	IERPSupplierRatingRepository ERPSupplierRatingRepository { get; set; }

	IERPTaxCodeLineRepository ERPTaxCodeLineRepository { get; set; }

	IERPTaxCodePlantRepository ERPTaxCodePlantRepository { get; set; }

	IERPTaxCodeRepository ERPTaxCodeRepository { get; set; }

	IERPTimecardLineRepository ERPTimecardLineRepository { get; set; }

	IERPTimecardRepository ERPTimecardRepository { get; set; }

	IERPToolCategoryRepository ERPToolCategoryRepository { get; set; }

	IERPToolMemoRepository ERPToolMemoRepository { get; set; }

	IERPToolMovementRepository ERPToolMovementRepository { get; set; }

	IERPToolRepository ERPToolRepository { get; set; }

	IERPTopActivitiesLogRepository ERPTopActivitiesLogRepository { get; set; }

	IERPWarehouseBinRepository ERPWarehouseBinRepository { get; set; }

	IERPWarehouseReceiptComponentRepository ERPWarehouseReceiptComponentRepository { get; set; }

	IERPWarehouseReceiptLineRepository ERPWarehouseReceiptLineRepository { get; set; }

	IERPWarehouseReceiptRepository ERPWarehouseReceiptRepository { get; set; }

	IERPWarehouseRequisitionComponentRepository ERPWarehouseRequisitionComponentRepository { get; set; }

	IERPWarehouseRequisitionLineRepository ERPWarehouseRequisitionLineRepository { get; set; }

	IERPWarehouseRequisitionRepository ERPWarehouseRequisitionRepository { get; set; }

	IERPWarehouseRepository ERPWarehouseRepository { get; set; }

	IERPWarehouseTransferComponentRepository ERPWarehouseTransferComponentRepository { get; set; }

	IERPWarehouseTransferLineRepository ERPWarehouseTransferLineRepository { get; set; }

	IERPWarehouseTransferRepository ERPWarehouseTransferRepository { get; set; }

	IERPWorkCenterMachineRepository ERPWorkCenterMachineRepository { get; set; }

	IERPWorkCenterMemoRepository ERPWorkCenterMemoRepository { get; set; }

	IERPWorkCenterRepository ERPWorkCenterRepository { get; set; }

	IERPWorkCenterSkillCompetencyRepository ERPWorkCenterSkillCompetencyRepository { get; set; }

	IERPWorkCenterSkillRepository ERPWorkCenterSkillRepository { get; set; }
}
