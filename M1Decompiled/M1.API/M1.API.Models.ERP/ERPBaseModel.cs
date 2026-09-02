using System;
using System.Collections.Generic;
using M1.API.Repositories.ERP;
using M1.API.Utilities;

namespace M1.API.Models.ERP;

public abstract class ERPBaseModel : APIBaseModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public IERPCustomTableRepository ERPCustomTableRepository { get; set; }

	public IERPAgingBucketRepository ERPAgingBucketRepository { get; set; }

	public IERPAPInvoiceExpenseAccountRepository ERPAPInvoiceExpenseAccountRepository { get; set; }

	public IERPAPInvoiceLineRepository ERPAPInvoiceLineRepository { get; set; }

	public IERPAPInvoiceMemoRepository ERPAPInvoiceMemoRepository { get; set; }

	public IERPAPInvoiceRepository ERPAPInvoiceRepository { get; set; }

	public IERPAPPaymentHeaderRepository ERPAPPaymentHeaderRepository { get; set; }

	public IERPAPPaymentLineRepository ERPAPPaymentLineRepository { get; set; }

	public IERPAPPaymentSessionRepository ERPAPPaymentSessionRepository { get; set; }

	public IERPARInvoiceLineRepository ERPARInvoiceLineRepository { get; set; }

	public IERPARInvoiceMemoRepository ERPARInvoiceMemoRepository { get; set; }

	public IERPARInvoiceRepository ERPARInvoiceRepository { get; set; }

	public IERPARInvoiceSalesPersonRepository ERPARInvoiceSalesPersonRepository { get; set; }

	public IERPARPaymentHeaderRepository ERPARPaymentHeaderRepository { get; set; }

	public IERPARPaymentLineRepository ERPARPaymentLineRepository { get; set; }

	public IERPARPaymentSessionRepository ERPARPaymentSessionRepository { get; set; }

	public IERPAssetAdjustmentRepository ERPAssetAdjustmentRepository { get; set; }

	public IERPAssetLowValuePoolRepository ERPAssetLowValuePoolRepository { get; set; }

	public IERPAssetMemoRepository ERPAssetMemoRepository { get; set; }

	public IERPAssetPoolTransactionRepository ERPAssetPoolTransactionRepository { get; set; }

	public IERPAssetRepository ERPAssetRepository { get; set; }

	public IERPAssetScheduleRepository ERPAssetScheduleRepository { get; set; }

	public IERPAssetTypeMethodRepository ERPAssetTypeMethodRepository { get; set; }

	public IERPAssetTypePlantRepository ERPAssetTypePlantRepository { get; set; }

	public IERPAssetTypeRepository ERPAssetTypeRepository { get; set; }

	public IERPAttachmentMemoRepository ERPAttachmentMemoRepository { get; set; }

	public IERPAttachmentRepository ERPAttachmentRepository { get; set; }

	public IERPAttachmentTypeRepository ERPAttachmentTypeRepository { get; set; }

	public IERPBankAccountRepository ERPBankAccountRepository { get; set; }

	public IERPBankEntryRepository ERPBankEntryRepository { get; set; }

	public IERPBankStatementRepository ERPBankStatementRepository { get; set; }

	public IERPCallLineRepository ERPCallLineRepository { get; set; }

	public IERPCallMemoRepository ERPCallMemoRepository { get; set; }

	public IERPCallRepository ERPCallRepository { get; set; }

	public IERPCallTypeRepository ERPCallTypeRepository { get; set; }

	public IERPChangeLogRepository ERPChangeLogRepository { get; set; }

	public IERPChangeRequestGroupLinkRepository ERPChangeRequestGroupLinkRepository { get; set; }

	public IERPChangeRequestGroupRepository ERPChangeRequestGroupRepository { get; set; }

	public IERPChangeRequestRepository ERPChangeRequestRepository { get; set; }

	public IERPChangeRequestTypeRepository ERPChangeRequestTypeRepository { get; set; }

	public IERPContactGroupRepository ERPContactGroupRepository { get; set; }

	public IERPContactMethodRepository ERPContactMethodRepository { get; set; }

	public IERPContactTitleRepository ERPContactTitleRepository { get; set; }

	public IERPCorrectiveActionCategoryRepository ERPCorrectiveActionCategoryRepository { get; set; }

	public IERPCorrectiveActionCodeRepository ERPCorrectiveActionCodeRepository { get; set; }

	public IERPCountyCodeRepository ERPCountyCodeRepository { get; set; }

	public IERPCurrencyRateLineRepository ERPCurrencyRateLineRepository { get; set; }

	public IERPCurrencyRateRepository ERPCurrencyRateRepository { get; set; }

	public IERPCustomerGroupRepository ERPCustomerGroupRepository { get; set; }

	public IERPCustomerPackageRepository ERPCustomerPackageRepository { get; set; }

	public IERPCycleCodeRepository ERPCycleCodeRepository { get; set; }

	public IERPDatasetPropertyRepository ERPDatasetPropertyRepository { get; set; }

	public IERPDMRClaimComponentRepository ERPDMRClaimComponentRepository { get; set; }

	public IERPDMRClaimLineRepository ERPDMRClaimLineRepository { get; set; }

	public IERPDMRClaimRepository ERPDMRClaimRepository { get; set; }

	public IERPDMRShipmentComponentRepository ERPDMRShipmentComponentRepository { get; set; }

	public IERPDMRShipmentLineRepository ERPDMRShipmentLineRepository { get; set; }

	public IERPDMRShipmentRepository ERPDMRShipmentRepository { get; set; }

	public IERPDocumentLinkRepository ERPDocumentLinkRepository { get; set; }

	public IERPEmployeeAttachmentRepository ERPEmployeeAttachmentRepository { get; set; }

	public IERPEmployeeMemoRepository ERPEmployeeMemoRepository { get; set; }

	public IERPEmployeePersonalDatumRepository ERPEmployeePersonalDatumRepository { get; set; }

	public IERPEmployeePOApprovalRepository ERPEmployeePOApprovalRepository { get; set; }

	public IERPEmployeeQAApprovalRepository ERPEmployeeQAApprovalRepository { get; set; }

	public IERPEmployeeRepository ERPEmployeeRepository { get; set; }

	public IERPEmployeeSalesBudgetLineRepository ERPEmployeeSalesBudgetLineRepository { get; set; }

	public IERPEmployeeSalesBudgetRepository ERPEmployeeSalesBudgetRepository { get; set; }

	public IERPEmployeeSkillCompetencyRepository ERPEmployeeSkillCompetencyRepository { get; set; }

	public IERPEmployeeSkillRepository ERPEmployeeSkillRepository { get; set; }

	public IERPEmployeeSOApprovalRepository ERPEmployeeSOApprovalRepository { get; set; }

	public IERPExpenseAccountSplitRepository ERPExpenseAccountSplitRepository { get; set; }

	public IERPExpenseRepository ERPExpenseRepository { get; set; }

	public IERPFinancialPropertyRepository ERPFinancialPropertyRepository { get; set; }

	public IERPFollowupRepository ERPFollowupRepository { get; set; }

	public IERPFreightPackageLinkRepository ERPFreightPackageLinkRepository { get; set; }

	public IERPFreightPackageRateRepository ERPFreightPackageRateRepository { get; set; }

	public IERPFreightPackageRepository ERPFreightPackageRepository { get; set; }

	public IERPFreightReferenceRepository ERPFreightReferenceRepository { get; set; }

	public IERPFreightShipmentRepository ERPFreightShipmentRepository { get; set; }

	public IERPGLAccountRepository ERPGLAccountRepository { get; set; }

	public IERPGLCategoryRepository ERPGLCategoryRepository { get; set; }

	public IERPGLChartRepository ERPGLChartRepository { get; set; }

	public IERPGLDepartmentRepository ERPGLDepartmentRepository { get; set; }

	public IERPGLDivisionRepository ERPGLDivisionRepository { get; set; }

	public IERPGLFiscalYearBudgetAmountRepository ERPGLFiscalYearBudgetAmountRepository { get; set; }

	public IERPGLFiscalYearBudgetHeaderRepository ERPGLFiscalYearBudgetHeaderRepository { get; set; }

	public IERPGLFiscalYearBudgetLineRepository ERPGLFiscalYearBudgetLineRepository { get; set; }

	public IERPGLFiscalYearOpeningBalanceRepository ERPGLFiscalYearOpeningBalanceRepository { get; set; }

	public IERPGLFiscalYearPeriodMovementRepository ERPGLFiscalYearPeriodMovementRepository { get; set; }

	public IERPGLFiscalYearPeriodRepository ERPGLFiscalYearPeriodRepository { get; set; }

	public IERPGLFiscalYearRepository ERPGLFiscalYearRepository { get; set; }

	public IERPGLJournalLineRepository ERPGLJournalLineRepository { get; set; }

	public IERPGLJournalMemoRepository ERPGLJournalMemoRepository { get; set; }

	public IERPGLJournalRepository ERPGLJournalRepository { get; set; }

	public IERPIndirectLaborCodeRepository ERPIndirectLaborCodeRepository { get; set; }

	public IERPIndustryTypeRepository ERPIndustryTypeRepository { get; set; }

	public IERPInspectionComponentRepository ERPInspectionComponentRepository { get; set; }

	public IERPInspectionLineApprovalRepository ERPInspectionLineApprovalRepository { get; set; }

	public IERPInspectionLineRepository ERPInspectionLineRepository { get; set; }

	public IERPInspectionRepository ERPInspectionRepository { get; set; }

	public IERPInventoryCountLineRepository ERPInventoryCountLineRepository { get; set; }

	public IERPInventoryCountRepository ERPInventoryCountRepository { get; set; }

	public IERPJobAssemblyRepository ERPJobAssemblyRepository { get; set; }

	public IERPJobCostRepository ERPJobCostRepository { get; set; }

	public IERPJobMaterialComponentRepository ERPJobMaterialComponentRepository { get; set; }

	public IERPJobMaterialRepository ERPJobMaterialRepository { get; set; }

	public IERPJobMemoRepository ERPJobMemoRepository { get; set; }

	public IERPJobOperationRepository ERPJobOperationRepository { get; set; }

	public IERPJobPriorityRepository ERPJobPriorityRepository { get; set; }

	public IERPJobRepository ERPJobRepository { get; set; }

	public IERPJobScenarioRepository ERPJobScenarioRepository { get; set; }

	public IERPKnowledgeBasePageRepository ERPKnowledgeBasePageRepository { get; set; }

	public IERPLandedCostCategoryRepository ERPLandedCostCategoryRepository { get; set; }

	public IERPLandedCostChargeDetailRepository ERPLandedCostChargeDetailRepository { get; set; }

	public IERPLandedCostChargeRepository ERPLandedCostChargeRepository { get; set; }

	public IERPLandedCostRepository ERPLandedCostRepository { get; set; }

	public IERPLaserCalculatorLineRepository ERPLaserCalculatorLineRepository { get; set; }

	public IERPLaserCalculatorRepository ERPLaserCalculatorRepository { get; set; }

	public IERPLeadCompetitorRepository ERPLeadCompetitorRepository { get; set; }

	public IERPLeadLineRepository ERPLeadLineRepository { get; set; }

	public IERPLeadMemoRepository ERPLeadMemoRepository { get; set; }

	public IERPLeadRepository ERPLeadRepository { get; set; }

	public IERPLeadSalesPersonRepository ERPLeadSalesPersonRepository { get; set; }

	public IERPLotNumberRepository ERPLotNumberRepository { get; set; }

	public IERPLotNumberStatusRepository ERPLotNumberStatusRepository { get; set; }

	public IERPLotNumberTransactionRepository ERPLotNumberTransactionRepository { get; set; }

	public IERPMarketingProgramRepository ERPMarketingProgramRepository { get; set; }

	public IERPMaterialIssueComponentRepository ERPMaterialIssueComponentRepository { get; set; }

	public IERPMaterialIssueLineRepository ERPMaterialIssueLineRepository { get; set; }

	public IERPMaterialIssueRepository ERPMaterialIssueRepository { get; set; }

	public IERPMfgReceiptComponentRepository ERPMfgReceiptComponentRepository { get; set; }

	public IERPMfgReceiptRepository ERPMfgReceiptRepository { get; set; }

	public IERPMilestoneRepository ERPMilestoneRepository { get; set; }

	public IERPMRPDemandRepository ERPMRPDemandRepository { get; set; }

	public IERPMRPJobDetailRepository ERPMRPJobDetailRepository { get; set; }

	public IERPMRPLineRepository ERPMRPLineRepository { get; set; }

	public IERPMRPSessionRepository ERPMRPSessionRepository { get; set; }

	public IERPMRPSupplyRepository ERPMRPSupplyRepository { get; set; }

	public IERPNextIDRepository ERPNextIDRepository { get; set; }

	public IERPNonConformanceCategoryRepository ERPNonConformanceCategoryRepository { get; set; }

	public IERPNonConformanceCauseRepository ERPNonConformanceCauseRepository { get; set; }

	public IERPNonConformanceCodeRepository ERPNonConformanceCodeRepository { get; set; }

	public IERPNonConformanceRepository ERPNonConformanceRepository { get; set; }

	public IERPOrganizationContactGroupLinkRepository ERPOrganizationContactGroupLinkRepository { get; set; }

	public IERPOrganizationContactRepository ERPOrganizationContactRepository { get; set; }

	public IERPOrganizationIndustryTypeLinkRepository ERPOrganizationIndustryTypeLinkRepository { get; set; }

	public IERPOrganizationLocationRepository ERPOrganizationLocationRepository { get; set; }

	public IERPOrganizationLocSalesPersonRepository ERPOrganizationLocSalesPersonRepository { get; set; }

	public IERPOrganizationMemoRepository ERPOrganizationMemoRepository { get; set; }

	public IERPOrganizationRepository ERPOrganizationRepository { get; set; }

	public IERPPartAlternateRepository ERPPartAlternateRepository { get; set; }

	public IERPPartAssemblyRepository ERPPartAssemblyRepository { get; set; }

	public IERPPartBinDetailRepository ERPPartBinDetailRepository { get; set; }

	public IERPPartBinRepository ERPPartBinRepository { get; set; }

	public IERPPartClassRepository ERPPartClassRepository { get; set; }

	public IERPPartClassPlantRepository ERPPartClassPlantRepository { get; set; }

	public IERPPartCrossReferenceRepository ERPPartCrossReferenceRepository { get; set; }

	public IERPPartForecastLineRepository ERPPartForecastLineRepository { get; set; }

	public IERPPartForecastRepository ERPPartForecastRepository { get; set; }

	public IERPPartGroupPlantRepository ERPPartGroupPlantRepository { get; set; }

	public IERPPartGroupRepository ERPPartGroupRepository { get; set; }

	public IERPPartMaterialRepository ERPPartMaterialRepository { get; set; }

	public IERPPartMemoRepository ERPPartMemoRepository { get; set; }

	public IERPPartOperationRepository ERPPartOperationRepository { get; set; }

	public IERPPartOrgReferenceRepository ERPPartOrgReferenceRepository { get; set; }

	public IERPPartPriceBreakRepository ERPPartPriceBreakRepository { get; set; }

	public IERPPartPriceRepository ERPPartPriceRepository { get; set; }

	public IERPPartRevisionRepository ERPPartRevisionRepository { get; set; }

	public IERPPartRuleRepository ERPPartRuleRepository { get; set; }

	public IERPPartRepository ERPPartRepository { get; set; }

	public IERPPartTransactionCostRepository ERPPartTransactionCostRepository { get; set; }

	public IERPPartTransactionRepository ERPPartTransactionRepository { get; set; }

	public IERPPartUnitSalePriceRepository ERPPartUnitSalePriceRepository { get; set; }

	public IERPPartWarehouseLocationRepository ERPPartWarehouseLocationRepository { get; set; }

	public IERPPaymentMethodRepository ERPPaymentMethodRepository { get; set; }

	public IERPPlantDepartmentRepository ERPPlantDepartmentRepository { get; set; }

	public IERPPlantRepository ERPPlantRepository { get; set; }

	public IERPPriorityRepository ERPPriorityRepository { get; set; }

	public IERPProcessRepository ERPProcessRepository { get; set; }

	public IERPProductCategoryRepository ERPProductCategoryRepository { get; set; }

	public IERPProductCategoryLineRepository ERPProductCategoryLineRepository { get; set; }

	public IERPProductionCalendarDayRepository ERPProductionCalendarDayRepository { get; set; }

	public IERPProductionCalendarRepository ERPProductionCalendarRepository { get; set; }

	public IERPProductionCalendarWorkCenterRepository ERPProductionCalendarWorkCenterRepository { get; set; }

	public IERPProductionDepartmentRepository ERPProductionDepartmentRepository { get; set; }

	public IERPProductionPropertyRepository ERPProductionPropertyRepository { get; set; }

	public IERPProjectAreaRepository ERPProjectAreaRepository { get; set; }

	public IERPProjectContactRepository ERPProjectContactRepository { get; set; }

	public IERPProjectedPaymentRepository ERPProjectedPaymentRepository { get; set; }

	public IERPProjectRepository ERPProjectRepository { get; set; }

	public IERPProjectTypeRepository ERPProjectTypeRepository { get; set; }

	public IERPPunchCalculatorRepository ERPPunchCalculatorRepository { get; set; }

	public IERPPurchaseOrderAccountRepository ERPPurchaseOrderAccountRepository { get; set; }

	public IERPPurchaseOrderApprovalRepository ERPPurchaseOrderApprovalRepository { get; set; }

	public IERPPurchaseOrderComponentRepository ERPPurchaseOrderComponentRepository { get; set; }

	public IERPPurchaseOrderDeliveryRepository ERPPurchaseOrderDeliveryRepository { get; set; }

	public IERPPurchaseOrderLineRepository ERPPurchaseOrderLineRepository { get; set; }

	public IERPPurchaseOrderMemoRepository ERPPurchaseOrderMemoRepository { get; set; }

	public IERPPurchaseOrderRepository ERPPurchaseOrderRepository { get; set; }

	public IERPPurchasePlannerLineRepository ERPPurchasePlannerLineRepository { get; set; }

	public IERPPurchasePlannerOrderDetailRepository ERPPurchasePlannerOrderDetailRepository { get; set; }

	public IERPPurchasePlannerRequirementRepository ERPPurchasePlannerRequirementRepository { get; set; }

	public IERPPurchasePlannerSessionRepository ERPPurchasePlannerSessionRepository { get; set; }

	public IERPQuantityAdjustmentRepository ERPQuantityAdjustmentRepository { get; set; }

	public IERPQuoteAssemblyRepository ERPQuoteAssemblyRepository { get; set; }

	public IERPQuoteLineRepository ERPQuoteLineRepository { get; set; }

	public IERPQuoteMaterialRepository ERPQuoteMaterialRepository { get; set; }

	public IERPQuoteMemoRepository ERPQuoteMemoRepository { get; set; }

	public IERPQuoteOperationRepository ERPQuoteOperationRepository { get; set; }

	public IERPQuoteQuantityRepository ERPQuoteQuantityRepository { get; set; }

	public IERPQuoteRepository ERPQuoteRepository { get; set; }

	public IERPQuoteSalesPersonRepository ERPQuoteSalesPersonRepository { get; set; }

	public IERPReasonPlantRepository ERPReasonPlantRepository { get; set; }

	public IERPReasonRepository ERPReasonRepository { get; set; }

	public IERPReceiptComponentRepository ERPReceiptComponentRepository { get; set; }

	public IERPReceiptLineRepository ERPReceiptLineRepository { get; set; }

	public IERPReceiptRepository ERPReceiptRepository { get; set; }

	public IERPRecentActivitiesLogRepository ERPRecentActivitiesLogRepository { get; set; }

	public IERPRFQLineRepository ERPRFQLineRepository { get; set; }

	public IERPRFQMemoRepository ERPRFQMemoRepository { get; set; }

	public IERPRFQQuantityRepository ERPRFQQuantityRepository { get; set; }

	public IERPRFQRepository ERPRFQRepository { get; set; }

	public IERPRFQSupplierRepository ERPRFQSupplierRepository { get; set; }

	public IERPRMAActionTypeRepository ERPRMAActionTypeRepository { get; set; }

	public IERPRMAClaimComponentRepository ERPRMAClaimComponentRepository { get; set; }

	public IERPRMAClaimLineRepository ERPRMAClaimLineRepository { get; set; }

	public IERPRMAClaimRepository ERPRMAClaimRepository { get; set; }

	public IERPRMAReceiptComponentRepository ERPRMAReceiptComponentRepository { get; set; }

	public IERPRMAReceiptLineRepository ERPRMAReceiptLineRepository { get; set; }

	public IERPRMAReceiptRepository ERPRMAReceiptRepository { get; set; }

	public IERPSalesOrderApprovalRepository ERPSalesOrderApprovalRepository { get; set; }

	public IERPSalesOrderComponentRepository ERPSalesOrderComponentRepository { get; set; }

	public IERPSalesOrderDeliveryRepository ERPSalesOrderDeliveryRepository { get; set; }

	public IERPSalesOrderJobLinkRepository ERPSalesOrderJobLinkRepository { get; set; }

	public IERPSalesOrderLineRepository ERPSalesOrderLineRepository { get; set; }

	public IERPSalesOrderMemoRepository ERPSalesOrderMemoRepository { get; set; }

	public IERPSalesOrderPickListLineRepository ERPSalesOrderPickListLineRepository { get; set; }

	public IERPSalesOrderPickListSessionRepository ERPSalesOrderPickListSessionRepository { get; set; }

	public IERPSalesOrderRepository ERPSalesOrderRepository { get; set; }

	public IERPSalesOrderSalesPersonRepository ERPSalesOrderSalesPersonRepository { get; set; }

	public IERPScheduleAllocationRepository ERPScheduleAllocationRepository { get; set; }

	public IERPScheduleBranchRepository ERPScheduleBranchRepository { get; set; }

	public IERPScheduleResourceLaneRepository ERPScheduleResourceLaneRepository { get; set; }

	public IERPScheduleTaskBucketRepository ERPScheduleTaskBucketRepository { get; set; }

	public IERPScheduleTaskRepository ERPScheduleTaskRepository { get; set; }

	public IERPScheduleTreeRepository ERPScheduleTreeRepository { get; set; }

	public IERPSerialNumberRepository ERPSerialNumberRepository { get; set; }

	public IERPSerialNumberStatusRepository ERPSerialNumberStatusRepository { get; set; }

	public IERPSerialNumberTransactionRepository ERPSerialNumberTransactionRepository { get; set; }

	public IERPServiceContractLineRepository ERPServiceContractLineRepository { get; set; }

	public IERPServiceContractMemoRepository ERPServiceContractMemoRepository { get; set; }

	public IERPServiceContractOwnerRepository ERPServiceContractOwnerRepository { get; set; }

	public IERPServiceContractRepository ERPServiceContractRepository { get; set; }

	public IERPServiceContractTypeRepository ERPServiceContractTypeRepository { get; set; }

	public IERPSheetCalculatorRepository ERPSheetCalculatorRepository { get; set; }

	public IERPShiftBreakRepository ERPShiftBreakRepository { get; set; }

	public IERPShiftRepository ERPShiftRepository { get; set; }

	public IERPShipmentComponentRepository ERPShipmentComponentRepository { get; set; }

	public IERPShipmentFreightLinkRepository ERPShipmentFreightLinkRepository { get; set; }

	public IERPShipmentFreightReferenceRepository ERPShipmentFreightReferenceRepository { get; set; }

	public IERPShipmentLineRepository ERPShipmentLineRepository { get; set; }

	public IERPShipmentPackageDetailRepository ERPShipmentPackageDetailRepository { get; set; }

	public IERPShipmentPackageRepository ERPShipmentPackageRepository { get; set; }

	public IERPShipmentRepository ERPShipmentRepository { get; set; }

	public IERPShippingMethodRepository ERPShippingMethodRepository { get; set; }

	public IERPShippingPaymentTypeRepository ERPShippingPaymentTypeRepository { get; set; }

	public IERPShippingPropertyRepository ERPShippingPropertyRepository { get; set; }

	public IERPSkillCompetencyRepository ERPSkillCompetencyRepository { get; set; }

	public IERPSkillRepository ERPSkillRepository { get; set; }

	public IERPStandardMessageRepository ERPStandardMessageRepository { get; set; }

	public IERPSupplierRatingRepository ERPSupplierRatingRepository { get; set; }

	public IERPTaxCodeLineRepository ERPTaxCodeLineRepository { get; set; }

	public IERPTaxCodePlantRepository ERPTaxCodePlantRepository { get; set; }

	public IERPTaxCodeRepository ERPTaxCodeRepository { get; set; }

	public IERPTimecardLineRepository ERPTimecardLineRepository { get; set; }

	public IERPTimecardRepository ERPTimecardRepository { get; set; }

	public IERPToolCategoryRepository ERPToolCategoryRepository { get; set; }

	public IERPToolMemoRepository ERPToolMemoRepository { get; set; }

	public IERPToolMovementRepository ERPToolMovementRepository { get; set; }

	public IERPToolRepository ERPToolRepository { get; set; }

	public IERPTopActivitiesLogRepository ERPTopActivitiesLogRepository { get; set; }

	public IERPWarehouseBinRepository ERPWarehouseBinRepository { get; set; }

	public IERPWarehouseReceiptComponentRepository ERPWarehouseReceiptComponentRepository { get; set; }

	public IERPWarehouseReceiptLineRepository ERPWarehouseReceiptLineRepository { get; set; }

	public IERPWarehouseReceiptRepository ERPWarehouseReceiptRepository { get; set; }

	public IERPWarehouseRequisitionComponentRepository ERPWarehouseRequisitionComponentRepository { get; set; }

	public IERPWarehouseRequisitionLineRepository ERPWarehouseRequisitionLineRepository { get; set; }

	public IERPWarehouseRequisitionRepository ERPWarehouseRequisitionRepository { get; set; }

	public IERPWarehouseRepository ERPWarehouseRepository { get; set; }

	public IERPWarehouseTransferComponentRepository ERPWarehouseTransferComponentRepository { get; set; }

	public IERPWarehouseTransferLineRepository ERPWarehouseTransferLineRepository { get; set; }

	public IERPWarehouseTransferRepository ERPWarehouseTransferRepository { get; set; }

	public IERPWorkCenterMachineRepository ERPWorkCenterMachineRepository { get; set; }

	public IERPWorkCenterMemoRepository ERPWorkCenterMemoRepository { get; set; }

	public IERPWorkCenterRepository ERPWorkCenterRepository { get; set; }

	public IERPWorkCenterSkillCompetencyRepository ERPWorkCenterSkillCompetencyRepository { get; set; }

	public IERPWorkCenterSkillRepository ERPWorkCenterSkillRepository { get; set; }

	public ERPBaseModel(APIClientContext clientContext)
	{
		base.ApiClientContext = clientContext;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
	}

	public ERPBaseModel()
	{
		base.ApiClientContext = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
	}
}
