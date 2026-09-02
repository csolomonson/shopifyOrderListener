using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProductionPropertyModel : ERPBaseModel, IERPProductionPropertyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProductionProperties(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProductionPropertyRepository iERPProductionPropertyRepository = (base.ERPProductionPropertyRepository = new ERPProductionPropertyRepository(base.ApiClientContext));
		using (iERPProductionPropertyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProductionPropertyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProductionPropertyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProductionPropertyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProductionPropertyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProductionProperty(Guid productionPropertyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProductionPropertyRepository iERPProductionPropertyRepository = (base.ERPProductionPropertyRepository = new ERPProductionPropertyRepository(base.ApiClientContext));
		using (iERPProductionPropertyRepository)
		{
			if (!(await base.ERPProductionPropertyRepository.DoesProductionPropertyExist(productionPropertyId)))
			{
				errorsList.Add($"ProductionProperty [{productionPropertyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProductionPropertyDto>>> Process_GetAllProductionProperties(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProductionPropertyDto> allProductionPropertiesDto = new List<ERPProductionPropertyDto>();
		ERPResponseMessageDto<IList<ERPProductionPropertyDto>> result;
		try
		{
			IERPProductionPropertyRepository iERPProductionPropertyRepository = (base.ERPProductionPropertyRepository = new ERPProductionPropertyRepository(base.ApiClientContext));
			using (iERPProductionPropertyRepository)
			{
				foreach (ERPProductionPropertyInformationDto item2 in await base.ERPProductionPropertyRepository.GetAllProductionProperties(pageSize, pageNumber, filter, orderBy))
				{
					ERPProductionPropertyDto item = new ERPProductionPropertyDto
					{
						xapAllowNegQtyOnHandHistory = item2.xapAllowNegQtyOnHandHistory,
						xapAnonymousCustomerID = item2.xapAnonymousCustomerID,
						xapChChangeRequestTypeID = item2.xapChChangeRequestTypeID,
						xapCmArPaymentCreditMessage = item2.xapCmArPaymentCreditMessage,
						xapCmArPaymentHoldMessage = item2.xapCmArPaymentHoldMessage,
						xapCmFieldServiceCreditMessage = item2.xapCmFieldServiceCreditMessage,
						xapCmFieldServiceHoldMessage = item2.xapCmFieldServiceHoldMessage,
						xapCmNonTaxReasonID = item2.xapCmNonTaxReasonID,
						xapCmOrderCreditMessage = item2.xapCmOrderCreditMessage,
						xapCmOrderHoldMessage = item2.xapCmOrderHoldMessage,
						xapCmSalesPersonDefaultLoc = item2.xapCmSalesPersonDefaultLoc,
						xapCmShipmentCreditMessage = item2.xapCmShipmentCreditMessage,
						xapCmShipmentHoldMessage = item2.xapCmShipmentHoldMessage,
						xapCostingMethodHistory = item2.xapCostingMethodHistory,
						xapCreatedBy = item2.xapCreatedBy,
						xapCreatedDate = item2.xapCreatedDate,
						xapDateToSchedule = item2.xapDateToSchedule,
						xapDcAutoClockOutComputer = item2.xapDcAutoClockOutComputer,
						xapDcIdleTimeThreshhold = item2.xapDcIdleTimeThreshhold,
						xapDcLaborCalculationMethod = item2.xapDcLaborCalculationMethod,
						xapDcPayCalculationMethod = item2.xapDcPayCalculationMethod,
						xapDcRefreshInterval = item2.xapDcRefreshInterval,
						xapDcSfeInspectionPassword = item2.xapDcSfeInspectionPassword,
						xapDcSfeShutdownPassword = item2.xapDcSfeShutdownPassword,
						xapDcTimeFormat = item2.xapDcTimeFormat,
						xapDMDefaultFolder = item2.xapDMDefaultFolder,
						xapEdi810ServiceUrl = item2.xapEdi810ServiceUrl,
						xapEdi856ServiceUrl = item2.xapEdi856ServiceUrl,
						xapEdiPassword = item2.xapEdiPassword,
						xapEdiUserName = item2.xapEdiUserName,
						xapUniqueID = item2.xapUniqueID,
						xapHdAttachmentFilePath = item2.xapHdAttachmentFilePath,
						xapHdcallDueDateDays = item2.xapHdcallDueDateDays,
						xapHdcallTypeID = item2.xapHdcallTypeID,
						xapHdcontactMethodID = item2.xapHdcontactMethodID,
						xapHdMailMergeCallTypeID = item2.xapHdMailMergeCallTypeID,
						xapHdMailMergeContactMethodID = item2.xapHdMailMergeContactMethodID,
						xapHdNewCallSoundFile = item2.xapHdNewCallSoundFile,
						xapHdSalesCallTypeID = item2.xapHdSalesCallTypeID,
						xapImAutoCreateRevisionID = item2.xapImAutoCreateRevisionID,
						xapImCostingMethod = item2.xapImCostingMethod,
						xapImMfgDefaultCostType = item2.xapImMfgDefaultCostType,
						xapCmCreateJobOnly = item2.xapCmCreateJobOnly,
						xapCmCreditLimitSourceInv = item2.xapCmCreditLimitSourceInv,
						xapCmCreditLimitSourceOrder = item2.xapCmCreditLimitSourceOrder,
						xapCmCreditLimitSourceShip = item2.xapCmCreditLimitSourceShip,
						xapCmCustomerTaxable = item2.xapCmCustomerTaxable,
						xapCmEnableResellers = item2.xapCmEnableResellers,
						xapCmIncludeFreightInPrice = item2.xapCmIncludeFreightInPrice,
						xapDcAllowNegativeQty = item2.xapDcAllowNegativeQty,
						xapDcAllowProductionComplete = item2.xapDcAllowProductionComplete,
						xapDcAutoClockOutLocked = item2.xapDcAutoClockOutLocked,
						xapDcEnableCreateSequence = item2.xapDcEnableCreateSequence,
						xapDcEnableIssueMaterial = item2.xapDcEnableIssueMaterial,
						xapDcEnableJobTraveler = item2.xapDcEnableJobTraveler,
						xapDcEnableMinimizeButtonInSfe = item2.xapDcEnableMinimizeButtonInSfe,
						xapDcEnableTimecardAudit = item2.xapDcEnableTimecardAudit,
						xapDcEnableWorkQueue = item2.xapDcEnableWorkQueue,
						xapDcPromptForActivityPassword = item2.xapDcPromptForActivityPassword,
						xapDcPromptForAuditPassword = item2.xapDcPromptForAuditPassword,
						xapDcPromptForClockInPassword = item2.xapDcPromptForClockInPassword,
						xapDcPromptForLaborDescription = item2.xapDcPromptForLaborDescription,
						xapDcPromptForMessagePassword = item2.xapDcPromptForMessagePassword,
						xapDcPromptForReason = item2.xapDcPromptForReason,
						xapDcShowCurrentJobsOnly = item2.xapDcShowCurrentJobsOnly,
						xapDcSplitDirectLaborHours = item2.xapDcSplitDirectLaborHours,
						xapDcSplitIndirectLaborHours = item2.xapDcSplitIndirectLaborHours,
						xapDcUseServerTime = item2.xapDcUseServerTime,
						xapDcWarnOnOutsideOperation = item2.xapDcWarnOnOutsideOperation,
						xapDcWarnOnOverProduction = item2.xapDcWarnOnOverProduction,
						xapGlCreateStockJournals = item2.xapGlCreateStockJournals,
						xapHdcreateCallForEmails = item2.xapHdcreateCallForEmails,
						xapImAllowNegativeQtyOnHand = item2.xapImAllowNegativeQtyOnHand,
						xapImAutoCreateRevision = item2.xapImAutoCreateRevision,
						xapImCopyAlternates = item2.xapImCopyAlternates,
						xapImCopyPartMemos = item2.xapImCopyPartMemos,
						xapImCopyPartOrgReferences = item2.xapImCopyPartOrgReferences,
						xapImCopyPartPrices = item2.xapImCopyPartPrices,
						xapImCopyPartRules = item2.xapImCopyPartRules,
						xapImEnableOrgPartCustomer = item2.xapImEnableOrgPartCustomer,
						xapImEnableOrgPartSupplier = item2.xapImEnableOrgPartSupplier,
						xapImEnableWarningWhenNegative = item2.xapImEnableWarningWhenNegative,
						xapImForceConfiguratorScreens = item2.xapImForceConfiguratorScreens,
						xapImHideUseMethodInTree = item2.xapImHideUseMethodInTree,
						xapImIgnoreLCInStdCostRollup = item2.xapImIgnoreLCInStdCostRollup,
						xapImOnlyAllowExistingBins = item2.xapImOnlyAllowExistingBins,
						xapImOverwriteDescription = item2.xapImOverwriteDescription,
						xapImOverwriteDocuments = item2.xapImOverwriteDocuments,
						xapImOverwriteMethod = item2.xapImOverwriteMethod,
						xapImRefreshMaterial = item2.xapImRefreshMaterial,
						xapImRefreshMaterialCosts = item2.xapImRefreshMaterialCosts,
						xapImScrapRoundUp = item2.xapImScrapRoundUp,
						xapImSetUseMethod = item2.xapImSetUseMethod,
						xapImTransferCustomer = item2.xapImTransferCustomer,
						xapImTransferDescriptions = item2.xapImTransferDescriptions,
						xapImTransferMaterial = item2.xapImTransferMaterial,
						xapImUseStdForStdCostRollUp = item2.xapImUseStdForStdCostRollUp,
						xapJmExcessQuantity = item2.xapJmExcessQuantity,
						xapJmIgnoreEmployees = item2.xapJmIgnoreEmployees,
						xapJmIgnoreMachines = item2.xapJmIgnoreMachines,
						xapJmLoadLevelFinite = item2.xapJmLoadLevelFinite,
						xapJmMinimizeGaps = item2.xapJmMinimizeGaps,
						xapJmMRPForecastFirmJob = item2.xapJmMRPForecastFirmJob,
						xapJmOverwriteDescription = item2.xapJmOverwriteDescription,
						xapJmOverwriteDocuments = item2.xapJmOverwriteDocuments,
						xapJmOverwriteMethod = item2.xapJmOverwriteMethod,
						xapJmRefreshHours = item2.xapJmRefreshHours,
						xapJmRefreshMaterial = item2.xapJmRefreshMaterial,
						xapJmRefreshMaterialCosts = item2.xapJmRefreshMaterialCosts,
						xapJmScheduleShowActualTimes = item2.xapJmScheduleShowActualTimes,
						xapJmScheduleUseActuals = item2.xapJmScheduleUseActuals,
						xapJmShopLoadShowFutureLoad = item2.xapJmShopLoadShowFutureLoad,
						xapJmShopLoadShowPastLoad = item2.xapJmShopLoadShowPastLoad,
						xapLmUpdateActualWithRounded = item2.xapLmUpdateActualWithRounded,
						xapNextSerialNumberPerGroup = item2.xapNextSerialNumberPerGroup,
						xapOmAutoCreateDelivery = item2.xapOmAutoCreateDelivery,
						xapOmEnableDiscountFields = item2.xapOmEnableDiscountFields,
						xapOmEnableFreightFields = item2.xapOmEnableFreightFields,
						xapOmIncludeOrderDeliveryInJob = item2.xapOmIncludeOrderDeliveryInJob,
						xapOmIncludeOrderLineInJob = item2.xapOmIncludeOrderLineInJob,
						xapOmMarkCreateJobForMto = item2.xapOmMarkCreateJobForMto,
						xapOmMarkPullQuoteMethodForMto = item2.xapOmMarkPullQuoteMethodForMto,
						xapOmShowDeliveriesInTree = item2.xapOmShowDeliveriesInTree,
						xapOmUseQuotingMarkupTM = item2.xapOmUseQuotingMarkupTM,
						xapPmPTOUsesDeliveryCost = item2.xapPmPTOUsesDeliveryCost,
						xapPmPurPlannerIncWhsQties = item2.xapPmPurPlannerIncWhsQties,
						xapPmPurPlannerUseBestPrice = item2.xapPmPurPlannerUseBestPrice,
						xapPmShowFirmOnlyPoWiz = item2.xapPmShowFirmOnlyPoWiz,
						xapPoWizardShowQtyToInspect = item2.xapPoWizardShowQtyToInspect,
						xapPRUseFirmQuotesOnly = item2.xapPRUseFirmQuotesOnly,
						xapQArmaRequiresInspection = item2.xapQArmaRequiresInspection,
						xapQAShowRmaOtherInfo = item2.xapQAShowRmaOtherInfo,
						xapQmMultipleQuantities = item2.xapQmMultipleQuantities,
						xapQmMUseDefHeaderFooterText = item2.xapQmMUseDefHeaderFooterText,
						xapQmOverwriteDescription = item2.xapQmOverwriteDescription,
						xapQmOverwriteDocuments = item2.xapQmOverwriteDocuments,
						xapQmOverwriteMethod = item2.xapQmOverwriteMethod,
						xapQmRefreshMaterial = item2.xapQmRefreshMaterial,
						xapQmRefreshMaterialCosts = item2.xapQmRefreshMaterialCosts,
						xapQmRefreshRateInfo = item2.xapQmRefreshRateInfo,
						xapRQGroupPobyRfq = item2.xapRQGroupPobyRfq,
						xapRQIncludeAlternateParts = item2.xapRQIncludeAlternateParts,
						xapSfeAllowSuspend = item2.xapSfeAllowSuspend,
						xapSfeBarcodeScanner = item2.xapSfeBarcodeScanner,
						xapSfeTouchScreen = item2.xapSfeTouchScreen,
						xapSmDeleteZeroShipmentLines = item2.xapSmDeleteZeroShipmentLines,
						xapJmCalendarExportFields = item2.xapJmCalendarExportFields,
						xapJmInitialExtension = item2.xapJmInitialExtension,
						xapJmInsideInspectionLineRTF = item2.xapJmInsideInspectionLineRTF,
						xapJmInsideInspectionLineText = item2.xapJmInsideInspectionLineText,
						xapJmJobMaterialSource = item2.xapJmJobMaterialSource,
						xapJmLoadReliefMethod = item2.xapJmLoadReliefMethod,
						xapJmOutsideInspectionLineRTF = item2.xapJmOutsideInspectionLineRTF,
						xapJmOutsideInspectionLineText = item2.xapJmOutsideInspectionLineText,
						xapJmScheduleBoardFields = item2.xapJmScheduleBoardFields,
						xapJmScheduleType = item2.xapJmScheduleType,
						xapJmShopLoadBuckets = item2.xapJmShopLoadBuckets,
						xapJmShopLoadDays = item2.xapJmShopLoadDays,
						xapJmShopLoadDepartmentID = item2.xapJmShopLoadDepartmentID,
						xapJmShopLoadFields = item2.xapJmShopLoadFields,
						xapJmShopLoadPlantID = item2.xapJmShopLoadPlantID,
						xapJmShopLoadTimeType = item2.xapJmShopLoadTimeType,
						xapJmSplitCosts = item2.xapJmSplitCosts,
						xapJmStandardFactor = item2.xapJmStandardFactor,
						xapLmCalculateEndTime = item2.xapLmCalculateEndTime,
						xapLmLeaveBoardFields = item2.xapLmLeaveBoardFields,
						xapLOResponseMethodID = item2.xapLOResponseMethodID,
						xapNextSerialNumberIDFormula = item2.xapNextSerialNumberIDFormula,
						xapOmAddlChargePartID = item2.xapOmAddlChargePartID,
						xapOmAddlChargePartRevisionID = item2.xapOmAddlChargePartRevisionID,
						xapOmDeliveryType = item2.xapOmDeliveryType,
						xapOmFreeOnBoardDescription = item2.xapOmFreeOnBoardDescription,
						xapOmLineQuantityValidation = item2.xapOmLineQuantityValidation,
						xapOmOrderDeliveryDigits = item2.xapOmOrderDeliveryDigits,
						xapOmOrderLineDigits = item2.xapOmOrderLineDigits,
						xapOmSalesGlAccountID = item2.xapOmSalesGlAccountID,
						xapOmUnitOfMeasure = item2.xapOmUnitOfMeasure,
						xapPACalendarExportFields = item2.xapPACalendarExportFields,
						xapPAExportFormat = item2.xapPAExportFormat,
						xapPAExportLocation = item2.xapPAExportLocation,
						xapPmCostingMethod = item2.xapPmCostingMethod,
						xapPmDefaultDueDate = item2.xapPmDefaultDueDate,
						xapPmFollowUpDays = item2.xapPmFollowUpDays,
						xapPmPoWizardDisplayType = item2.xapPmPoWizardDisplayType,
						xapPmPurchaseType = item2.xapPmPurchaseType,
						xapPmTaxExemptNumber = item2.xapPmTaxExemptNumber,
						xapPRLaborMethod = item2.xapPRLaborMethod,
						xapQAInspQueueRefreshInterval = item2.xapQAInspQueueRefreshInterval,
						xapQmAdditionalChargeText = item2.xapQmAdditionalChargeText,
						xapQmExpirationDays = item2.xapQmExpirationDays,
						xapQmFollowUpDays = item2.xapQmFollowUpDays,
						xapQmFollowUpType = item2.xapQmFollowUpType,
						xapQmLaborMarkup = item2.xapQmLaborMarkup,
						xapQmMaterialMarkup = item2.xapQmMaterialMarkup,
						xapQmMQuoteFooterMessageRTF = item2.xapQmMQuoteFooterMessageRTF,
						xapQmMQuoteFooterMessageText = item2.xapQmMQuoteFooterMessageText,
						xapQmMQuoteHeaderMessageRTF = item2.xapQmMQuoteHeaderMessageRTF,
						xapQmMQuoteHeaderMessageText = item2.xapQmMQuoteHeaderMessageText,
						xapQmOverheadMarkup = item2.xapQmOverheadMarkup,
						xapQmPurchaseToOrderMarkup = item2.xapQmPurchaseToOrderMarkup,
						xapQmQuoteFooterMessageRTF = item2.xapQmQuoteFooterMessageRTF,
						xapQmQuoteFooterMessageText = item2.xapQmQuoteFooterMessageText,
						xapQmQuoteHeaderMessageRTF = item2.xapQmQuoteHeaderMessageRTF,
						xapQmQuoteHeaderMessageText = item2.xapQmQuoteHeaderMessageText,
						xapQmQuoteMarkupType = item2.xapQmQuoteMarkupType,
						xapQmQuotingMarkup = item2.xapQmQuotingMarkup,
						xapQmQuotingMethod = item2.xapQmQuotingMethod,
						xapQmSubcontractMarkup = item2.xapQmSubcontractMarkup,
						xapRowVersion = item2.xapRowVersion,
						xapSfeActiveJobQueueFields = item2.xapSfeActiveJobQueueFields,
						xapSfeAddPartSelect = item2.xapSfeAddPartSelect,
						xapSfeAsmSearchFields = item2.xapSfeAsmSearchFields,
						xapSfeEndJobCompletionCode = item2.xapSfeEndJobCompletionCode,
						xapSfeEndJobGoodQty = item2.xapSfeEndJobGoodQty,
						xapSfeEndJobScrapQty = item2.xapSfeEndJobScrapQty,
						xapSfeIssueMaterialQty = item2.xapSfeIssueMaterialQty,
						xapSfeJobSearchSelect = item2.xapSfeJobSearchSelect,
						xapSfeJobTraveller = item2.xapSfeJobTraveller,
						xapSfeOprSearchFields = item2.xapSfeOprSearchFields,
						xapSfeSetupPercentage = item2.xapSfeSetupPercentage,
						xapSfeStartJobWorkCode = item2.xapSfeStartJobWorkCode,
						xapSfeTCAuditReport = item2.xapSfeTCAuditReport,
						xapSfeWorkQueueFields = item2.xapSfeWorkQueueFields,
						xapSfeWorkQueueSort = item2.xapSfeWorkQueueSort,
						xapShowQtyOnHandMobInv = item2.xapShowQtyOnHandMobInv,
						xapSmEdi856CustomLabel = item2.xapSmEdi856CustomLabel,
						xapSmLineQuantityValidation = item2.xapSmLineQuantityValidation,
						CustomFields = item2.CustomFields
					};
					allProductionPropertiesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProductionProperties]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProductionPropertyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProductionPropertiesDto,
				RecordCount = allProductionPropertiesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProductionPropertyDto>> Process_GetProductionProperty(Guid productionPropertyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProductionPropertyDto productionPropertyDto = null;
		ERPResponseMessageDto<ERPProductionPropertyDto> result;
		try
		{
			IERPProductionPropertyRepository iERPProductionPropertyRepository = (base.ERPProductionPropertyRepository = new ERPProductionPropertyRepository(base.ApiClientContext));
			using (iERPProductionPropertyRepository)
			{
				ERPProductionPropertyInformationDto eRPProductionPropertyInformationDto = await base.ERPProductionPropertyRepository.GetProductionProperty(productionPropertyId);
				productionPropertyDto = new ERPProductionPropertyDto
				{
					xapAllowNegQtyOnHandHistory = eRPProductionPropertyInformationDto.xapAllowNegQtyOnHandHistory,
					xapAnonymousCustomerID = eRPProductionPropertyInformationDto.xapAnonymousCustomerID,
					xapChChangeRequestTypeID = eRPProductionPropertyInformationDto.xapChChangeRequestTypeID,
					xapCmArPaymentCreditMessage = eRPProductionPropertyInformationDto.xapCmArPaymentCreditMessage,
					xapCmArPaymentHoldMessage = eRPProductionPropertyInformationDto.xapCmArPaymentHoldMessage,
					xapCmFieldServiceCreditMessage = eRPProductionPropertyInformationDto.xapCmFieldServiceCreditMessage,
					xapCmFieldServiceHoldMessage = eRPProductionPropertyInformationDto.xapCmFieldServiceHoldMessage,
					xapCmNonTaxReasonID = eRPProductionPropertyInformationDto.xapCmNonTaxReasonID,
					xapCmOrderCreditMessage = eRPProductionPropertyInformationDto.xapCmOrderCreditMessage,
					xapCmOrderHoldMessage = eRPProductionPropertyInformationDto.xapCmOrderHoldMessage,
					xapCmSalesPersonDefaultLoc = eRPProductionPropertyInformationDto.xapCmSalesPersonDefaultLoc,
					xapCmShipmentCreditMessage = eRPProductionPropertyInformationDto.xapCmShipmentCreditMessage,
					xapCmShipmentHoldMessage = eRPProductionPropertyInformationDto.xapCmShipmentHoldMessage,
					xapCostingMethodHistory = eRPProductionPropertyInformationDto.xapCostingMethodHistory,
					xapCreatedBy = eRPProductionPropertyInformationDto.xapCreatedBy,
					xapCreatedDate = eRPProductionPropertyInformationDto.xapCreatedDate,
					xapDateToSchedule = eRPProductionPropertyInformationDto.xapDateToSchedule,
					xapDcAutoClockOutComputer = eRPProductionPropertyInformationDto.xapDcAutoClockOutComputer,
					xapDcIdleTimeThreshhold = eRPProductionPropertyInformationDto.xapDcIdleTimeThreshhold,
					xapDcLaborCalculationMethod = eRPProductionPropertyInformationDto.xapDcLaborCalculationMethod,
					xapDcPayCalculationMethod = eRPProductionPropertyInformationDto.xapDcPayCalculationMethod,
					xapDcRefreshInterval = eRPProductionPropertyInformationDto.xapDcRefreshInterval,
					xapDcSfeInspectionPassword = eRPProductionPropertyInformationDto.xapDcSfeInspectionPassword,
					xapDcSfeShutdownPassword = eRPProductionPropertyInformationDto.xapDcSfeShutdownPassword,
					xapDcTimeFormat = eRPProductionPropertyInformationDto.xapDcTimeFormat,
					xapDMDefaultFolder = eRPProductionPropertyInformationDto.xapDMDefaultFolder,
					xapEdi810ServiceUrl = eRPProductionPropertyInformationDto.xapEdi810ServiceUrl,
					xapEdi856ServiceUrl = eRPProductionPropertyInformationDto.xapEdi856ServiceUrl,
					xapEdiPassword = eRPProductionPropertyInformationDto.xapEdiPassword,
					xapEdiUserName = eRPProductionPropertyInformationDto.xapEdiUserName,
					xapUniqueID = eRPProductionPropertyInformationDto.xapUniqueID,
					xapHdAttachmentFilePath = eRPProductionPropertyInformationDto.xapHdAttachmentFilePath,
					xapHdcallDueDateDays = eRPProductionPropertyInformationDto.xapHdcallDueDateDays,
					xapHdcallTypeID = eRPProductionPropertyInformationDto.xapHdcallTypeID,
					xapHdcontactMethodID = eRPProductionPropertyInformationDto.xapHdcontactMethodID,
					xapHdMailMergeCallTypeID = eRPProductionPropertyInformationDto.xapHdMailMergeCallTypeID,
					xapHdMailMergeContactMethodID = eRPProductionPropertyInformationDto.xapHdMailMergeContactMethodID,
					xapHdNewCallSoundFile = eRPProductionPropertyInformationDto.xapHdNewCallSoundFile,
					xapHdSalesCallTypeID = eRPProductionPropertyInformationDto.xapHdSalesCallTypeID,
					xapImAutoCreateRevisionID = eRPProductionPropertyInformationDto.xapImAutoCreateRevisionID,
					xapImCostingMethod = eRPProductionPropertyInformationDto.xapImCostingMethod,
					xapImMfgDefaultCostType = eRPProductionPropertyInformationDto.xapImMfgDefaultCostType,
					xapCmCreateJobOnly = eRPProductionPropertyInformationDto.xapCmCreateJobOnly,
					xapCmCreditLimitSourceInv = eRPProductionPropertyInformationDto.xapCmCreditLimitSourceInv,
					xapCmCreditLimitSourceOrder = eRPProductionPropertyInformationDto.xapCmCreditLimitSourceOrder,
					xapCmCreditLimitSourceShip = eRPProductionPropertyInformationDto.xapCmCreditLimitSourceShip,
					xapCmCustomerTaxable = eRPProductionPropertyInformationDto.xapCmCustomerTaxable,
					xapCmEnableResellers = eRPProductionPropertyInformationDto.xapCmEnableResellers,
					xapCmIncludeFreightInPrice = eRPProductionPropertyInformationDto.xapCmIncludeFreightInPrice,
					xapDcAllowNegativeQty = eRPProductionPropertyInformationDto.xapDcAllowNegativeQty,
					xapDcAllowProductionComplete = eRPProductionPropertyInformationDto.xapDcAllowProductionComplete,
					xapDcAutoClockOutLocked = eRPProductionPropertyInformationDto.xapDcAutoClockOutLocked,
					xapDcEnableCreateSequence = eRPProductionPropertyInformationDto.xapDcEnableCreateSequence,
					xapDcEnableIssueMaterial = eRPProductionPropertyInformationDto.xapDcEnableIssueMaterial,
					xapDcEnableJobTraveler = eRPProductionPropertyInformationDto.xapDcEnableJobTraveler,
					xapDcEnableMinimizeButtonInSfe = eRPProductionPropertyInformationDto.xapDcEnableMinimizeButtonInSfe,
					xapDcEnableTimecardAudit = eRPProductionPropertyInformationDto.xapDcEnableTimecardAudit,
					xapDcEnableWorkQueue = eRPProductionPropertyInformationDto.xapDcEnableWorkQueue,
					xapDcPromptForActivityPassword = eRPProductionPropertyInformationDto.xapDcPromptForActivityPassword,
					xapDcPromptForAuditPassword = eRPProductionPropertyInformationDto.xapDcPromptForAuditPassword,
					xapDcPromptForClockInPassword = eRPProductionPropertyInformationDto.xapDcPromptForClockInPassword,
					xapDcPromptForLaborDescription = eRPProductionPropertyInformationDto.xapDcPromptForLaborDescription,
					xapDcPromptForMessagePassword = eRPProductionPropertyInformationDto.xapDcPromptForMessagePassword,
					xapDcPromptForReason = eRPProductionPropertyInformationDto.xapDcPromptForReason,
					xapDcShowCurrentJobsOnly = eRPProductionPropertyInformationDto.xapDcShowCurrentJobsOnly,
					xapDcSplitDirectLaborHours = eRPProductionPropertyInformationDto.xapDcSplitDirectLaborHours,
					xapDcSplitIndirectLaborHours = eRPProductionPropertyInformationDto.xapDcSplitIndirectLaborHours,
					xapDcUseServerTime = eRPProductionPropertyInformationDto.xapDcUseServerTime,
					xapDcWarnOnOutsideOperation = eRPProductionPropertyInformationDto.xapDcWarnOnOutsideOperation,
					xapDcWarnOnOverProduction = eRPProductionPropertyInformationDto.xapDcWarnOnOverProduction,
					xapGlCreateStockJournals = eRPProductionPropertyInformationDto.xapGlCreateStockJournals,
					xapHdcreateCallForEmails = eRPProductionPropertyInformationDto.xapHdcreateCallForEmails,
					xapImAllowNegativeQtyOnHand = eRPProductionPropertyInformationDto.xapImAllowNegativeQtyOnHand,
					xapImAutoCreateRevision = eRPProductionPropertyInformationDto.xapImAutoCreateRevision,
					xapImCopyAlternates = eRPProductionPropertyInformationDto.xapImCopyAlternates,
					xapImCopyPartMemos = eRPProductionPropertyInformationDto.xapImCopyPartMemos,
					xapImCopyPartOrgReferences = eRPProductionPropertyInformationDto.xapImCopyPartOrgReferences,
					xapImCopyPartPrices = eRPProductionPropertyInformationDto.xapImCopyPartPrices,
					xapImCopyPartRules = eRPProductionPropertyInformationDto.xapImCopyPartRules,
					xapImEnableOrgPartCustomer = eRPProductionPropertyInformationDto.xapImEnableOrgPartCustomer,
					xapImEnableOrgPartSupplier = eRPProductionPropertyInformationDto.xapImEnableOrgPartSupplier,
					xapImEnableWarningWhenNegative = eRPProductionPropertyInformationDto.xapImEnableWarningWhenNegative,
					xapImForceConfiguratorScreens = eRPProductionPropertyInformationDto.xapImForceConfiguratorScreens,
					xapImHideUseMethodInTree = eRPProductionPropertyInformationDto.xapImHideUseMethodInTree,
					xapImIgnoreLCInStdCostRollup = eRPProductionPropertyInformationDto.xapImIgnoreLCInStdCostRollup,
					xapImOnlyAllowExistingBins = eRPProductionPropertyInformationDto.xapImOnlyAllowExistingBins,
					xapImOverwriteDescription = eRPProductionPropertyInformationDto.xapImOverwriteDescription,
					xapImOverwriteDocuments = eRPProductionPropertyInformationDto.xapImOverwriteDocuments,
					xapImOverwriteMethod = eRPProductionPropertyInformationDto.xapImOverwriteMethod,
					xapImRefreshMaterial = eRPProductionPropertyInformationDto.xapImRefreshMaterial,
					xapImRefreshMaterialCosts = eRPProductionPropertyInformationDto.xapImRefreshMaterialCosts,
					xapImScrapRoundUp = eRPProductionPropertyInformationDto.xapImScrapRoundUp,
					xapImSetUseMethod = eRPProductionPropertyInformationDto.xapImSetUseMethod,
					xapImTransferCustomer = eRPProductionPropertyInformationDto.xapImTransferCustomer,
					xapImTransferDescriptions = eRPProductionPropertyInformationDto.xapImTransferDescriptions,
					xapImTransferMaterial = eRPProductionPropertyInformationDto.xapImTransferMaterial,
					xapImUseStdForStdCostRollUp = eRPProductionPropertyInformationDto.xapImUseStdForStdCostRollUp,
					xapJmExcessQuantity = eRPProductionPropertyInformationDto.xapJmExcessQuantity,
					xapJmIgnoreEmployees = eRPProductionPropertyInformationDto.xapJmIgnoreEmployees,
					xapJmIgnoreMachines = eRPProductionPropertyInformationDto.xapJmIgnoreMachines,
					xapJmLoadLevelFinite = eRPProductionPropertyInformationDto.xapJmLoadLevelFinite,
					xapJmMinimizeGaps = eRPProductionPropertyInformationDto.xapJmMinimizeGaps,
					xapJmMRPForecastFirmJob = eRPProductionPropertyInformationDto.xapJmMRPForecastFirmJob,
					xapJmOverwriteDescription = eRPProductionPropertyInformationDto.xapJmOverwriteDescription,
					xapJmOverwriteDocuments = eRPProductionPropertyInformationDto.xapJmOverwriteDocuments,
					xapJmOverwriteMethod = eRPProductionPropertyInformationDto.xapJmOverwriteMethod,
					xapJmRefreshHours = eRPProductionPropertyInformationDto.xapJmRefreshHours,
					xapJmRefreshMaterial = eRPProductionPropertyInformationDto.xapJmRefreshMaterial,
					xapJmRefreshMaterialCosts = eRPProductionPropertyInformationDto.xapJmRefreshMaterialCosts,
					xapJmScheduleShowActualTimes = eRPProductionPropertyInformationDto.xapJmScheduleShowActualTimes,
					xapJmScheduleUseActuals = eRPProductionPropertyInformationDto.xapJmScheduleUseActuals,
					xapJmShopLoadShowFutureLoad = eRPProductionPropertyInformationDto.xapJmShopLoadShowFutureLoad,
					xapJmShopLoadShowPastLoad = eRPProductionPropertyInformationDto.xapJmShopLoadShowPastLoad,
					xapLmUpdateActualWithRounded = eRPProductionPropertyInformationDto.xapLmUpdateActualWithRounded,
					xapNextSerialNumberPerGroup = eRPProductionPropertyInformationDto.xapNextSerialNumberPerGroup,
					xapOmAutoCreateDelivery = eRPProductionPropertyInformationDto.xapOmAutoCreateDelivery,
					xapOmEnableDiscountFields = eRPProductionPropertyInformationDto.xapOmEnableDiscountFields,
					xapOmEnableFreightFields = eRPProductionPropertyInformationDto.xapOmEnableFreightFields,
					xapOmIncludeOrderDeliveryInJob = eRPProductionPropertyInformationDto.xapOmIncludeOrderDeliveryInJob,
					xapOmIncludeOrderLineInJob = eRPProductionPropertyInformationDto.xapOmIncludeOrderLineInJob,
					xapOmMarkCreateJobForMto = eRPProductionPropertyInformationDto.xapOmMarkCreateJobForMto,
					xapOmMarkPullQuoteMethodForMto = eRPProductionPropertyInformationDto.xapOmMarkPullQuoteMethodForMto,
					xapOmShowDeliveriesInTree = eRPProductionPropertyInformationDto.xapOmShowDeliveriesInTree,
					xapOmUseQuotingMarkupTM = eRPProductionPropertyInformationDto.xapOmUseQuotingMarkupTM,
					xapPmPTOUsesDeliveryCost = eRPProductionPropertyInformationDto.xapPmPTOUsesDeliveryCost,
					xapPmPurPlannerIncWhsQties = eRPProductionPropertyInformationDto.xapPmPurPlannerIncWhsQties,
					xapPmPurPlannerUseBestPrice = eRPProductionPropertyInformationDto.xapPmPurPlannerUseBestPrice,
					xapPmShowFirmOnlyPoWiz = eRPProductionPropertyInformationDto.xapPmShowFirmOnlyPoWiz,
					xapPoWizardShowQtyToInspect = eRPProductionPropertyInformationDto.xapPoWizardShowQtyToInspect,
					xapPRUseFirmQuotesOnly = eRPProductionPropertyInformationDto.xapPRUseFirmQuotesOnly,
					xapQArmaRequiresInspection = eRPProductionPropertyInformationDto.xapQArmaRequiresInspection,
					xapQAShowRmaOtherInfo = eRPProductionPropertyInformationDto.xapQAShowRmaOtherInfo,
					xapQmMultipleQuantities = eRPProductionPropertyInformationDto.xapQmMultipleQuantities,
					xapQmMUseDefHeaderFooterText = eRPProductionPropertyInformationDto.xapQmMUseDefHeaderFooterText,
					xapQmOverwriteDescription = eRPProductionPropertyInformationDto.xapQmOverwriteDescription,
					xapQmOverwriteDocuments = eRPProductionPropertyInformationDto.xapQmOverwriteDocuments,
					xapQmOverwriteMethod = eRPProductionPropertyInformationDto.xapQmOverwriteMethod,
					xapQmRefreshMaterial = eRPProductionPropertyInformationDto.xapQmRefreshMaterial,
					xapQmRefreshMaterialCosts = eRPProductionPropertyInformationDto.xapQmRefreshMaterialCosts,
					xapQmRefreshRateInfo = eRPProductionPropertyInformationDto.xapQmRefreshRateInfo,
					xapRQGroupPobyRfq = eRPProductionPropertyInformationDto.xapRQGroupPobyRfq,
					xapRQIncludeAlternateParts = eRPProductionPropertyInformationDto.xapRQIncludeAlternateParts,
					xapSfeAllowSuspend = eRPProductionPropertyInformationDto.xapSfeAllowSuspend,
					xapSfeBarcodeScanner = eRPProductionPropertyInformationDto.xapSfeBarcodeScanner,
					xapSfeTouchScreen = eRPProductionPropertyInformationDto.xapSfeTouchScreen,
					xapSmDeleteZeroShipmentLines = eRPProductionPropertyInformationDto.xapSmDeleteZeroShipmentLines,
					xapJmCalendarExportFields = eRPProductionPropertyInformationDto.xapJmCalendarExportFields,
					xapJmInitialExtension = eRPProductionPropertyInformationDto.xapJmInitialExtension,
					xapJmInsideInspectionLineRTF = eRPProductionPropertyInformationDto.xapJmInsideInspectionLineRTF,
					xapJmInsideInspectionLineText = eRPProductionPropertyInformationDto.xapJmInsideInspectionLineText,
					xapJmJobMaterialSource = eRPProductionPropertyInformationDto.xapJmJobMaterialSource,
					xapJmLoadReliefMethod = eRPProductionPropertyInformationDto.xapJmLoadReliefMethod,
					xapJmOutsideInspectionLineRTF = eRPProductionPropertyInformationDto.xapJmOutsideInspectionLineRTF,
					xapJmOutsideInspectionLineText = eRPProductionPropertyInformationDto.xapJmOutsideInspectionLineText,
					xapJmScheduleBoardFields = eRPProductionPropertyInformationDto.xapJmScheduleBoardFields,
					xapJmScheduleType = eRPProductionPropertyInformationDto.xapJmScheduleType,
					xapJmShopLoadBuckets = eRPProductionPropertyInformationDto.xapJmShopLoadBuckets,
					xapJmShopLoadDays = eRPProductionPropertyInformationDto.xapJmShopLoadDays,
					xapJmShopLoadDepartmentID = eRPProductionPropertyInformationDto.xapJmShopLoadDepartmentID,
					xapJmShopLoadFields = eRPProductionPropertyInformationDto.xapJmShopLoadFields,
					xapJmShopLoadPlantID = eRPProductionPropertyInformationDto.xapJmShopLoadPlantID,
					xapJmShopLoadTimeType = eRPProductionPropertyInformationDto.xapJmShopLoadTimeType,
					xapJmSplitCosts = eRPProductionPropertyInformationDto.xapJmSplitCosts,
					xapJmStandardFactor = eRPProductionPropertyInformationDto.xapJmStandardFactor,
					xapLmCalculateEndTime = eRPProductionPropertyInformationDto.xapLmCalculateEndTime,
					xapLmLeaveBoardFields = eRPProductionPropertyInformationDto.xapLmLeaveBoardFields,
					xapLOResponseMethodID = eRPProductionPropertyInformationDto.xapLOResponseMethodID,
					xapNextSerialNumberIDFormula = eRPProductionPropertyInformationDto.xapNextSerialNumberIDFormula,
					xapOmAddlChargePartID = eRPProductionPropertyInformationDto.xapOmAddlChargePartID,
					xapOmAddlChargePartRevisionID = eRPProductionPropertyInformationDto.xapOmAddlChargePartRevisionID,
					xapOmDeliveryType = eRPProductionPropertyInformationDto.xapOmDeliveryType,
					xapOmFreeOnBoardDescription = eRPProductionPropertyInformationDto.xapOmFreeOnBoardDescription,
					xapOmLineQuantityValidation = eRPProductionPropertyInformationDto.xapOmLineQuantityValidation,
					xapOmOrderDeliveryDigits = eRPProductionPropertyInformationDto.xapOmOrderDeliveryDigits,
					xapOmOrderLineDigits = eRPProductionPropertyInformationDto.xapOmOrderLineDigits,
					xapOmSalesGlAccountID = eRPProductionPropertyInformationDto.xapOmSalesGlAccountID,
					xapOmUnitOfMeasure = eRPProductionPropertyInformationDto.xapOmUnitOfMeasure,
					xapPACalendarExportFields = eRPProductionPropertyInformationDto.xapPACalendarExportFields,
					xapPAExportFormat = eRPProductionPropertyInformationDto.xapPAExportFormat,
					xapPAExportLocation = eRPProductionPropertyInformationDto.xapPAExportLocation,
					xapPmCostingMethod = eRPProductionPropertyInformationDto.xapPmCostingMethod,
					xapPmDefaultDueDate = eRPProductionPropertyInformationDto.xapPmDefaultDueDate,
					xapPmFollowUpDays = eRPProductionPropertyInformationDto.xapPmFollowUpDays,
					xapPmPoWizardDisplayType = eRPProductionPropertyInformationDto.xapPmPoWizardDisplayType,
					xapPmPurchaseType = eRPProductionPropertyInformationDto.xapPmPurchaseType,
					xapPmTaxExemptNumber = eRPProductionPropertyInformationDto.xapPmTaxExemptNumber,
					xapPRLaborMethod = eRPProductionPropertyInformationDto.xapPRLaborMethod,
					xapQAInspQueueRefreshInterval = eRPProductionPropertyInformationDto.xapQAInspQueueRefreshInterval,
					xapQmAdditionalChargeText = eRPProductionPropertyInformationDto.xapQmAdditionalChargeText,
					xapQmExpirationDays = eRPProductionPropertyInformationDto.xapQmExpirationDays,
					xapQmFollowUpDays = eRPProductionPropertyInformationDto.xapQmFollowUpDays,
					xapQmFollowUpType = eRPProductionPropertyInformationDto.xapQmFollowUpType,
					xapQmLaborMarkup = eRPProductionPropertyInformationDto.xapQmLaborMarkup,
					xapQmMaterialMarkup = eRPProductionPropertyInformationDto.xapQmMaterialMarkup,
					xapQmMQuoteFooterMessageRTF = eRPProductionPropertyInformationDto.xapQmMQuoteFooterMessageRTF,
					xapQmMQuoteFooterMessageText = eRPProductionPropertyInformationDto.xapQmMQuoteFooterMessageText,
					xapQmMQuoteHeaderMessageRTF = eRPProductionPropertyInformationDto.xapQmMQuoteHeaderMessageRTF,
					xapQmMQuoteHeaderMessageText = eRPProductionPropertyInformationDto.xapQmMQuoteHeaderMessageText,
					xapQmOverheadMarkup = eRPProductionPropertyInformationDto.xapQmOverheadMarkup,
					xapQmPurchaseToOrderMarkup = eRPProductionPropertyInformationDto.xapQmPurchaseToOrderMarkup,
					xapQmQuoteFooterMessageRTF = eRPProductionPropertyInformationDto.xapQmQuoteFooterMessageRTF,
					xapQmQuoteFooterMessageText = eRPProductionPropertyInformationDto.xapQmQuoteFooterMessageText,
					xapQmQuoteHeaderMessageRTF = eRPProductionPropertyInformationDto.xapQmQuoteHeaderMessageRTF,
					xapQmQuoteHeaderMessageText = eRPProductionPropertyInformationDto.xapQmQuoteHeaderMessageText,
					xapQmQuoteMarkupType = eRPProductionPropertyInformationDto.xapQmQuoteMarkupType,
					xapQmQuotingMarkup = eRPProductionPropertyInformationDto.xapQmQuotingMarkup,
					xapQmQuotingMethod = eRPProductionPropertyInformationDto.xapQmQuotingMethod,
					xapQmSubcontractMarkup = eRPProductionPropertyInformationDto.xapQmSubcontractMarkup,
					xapRowVersion = eRPProductionPropertyInformationDto.xapRowVersion,
					xapSfeActiveJobQueueFields = eRPProductionPropertyInformationDto.xapSfeActiveJobQueueFields,
					xapSfeAddPartSelect = eRPProductionPropertyInformationDto.xapSfeAddPartSelect,
					xapSfeAsmSearchFields = eRPProductionPropertyInformationDto.xapSfeAsmSearchFields,
					xapSfeEndJobCompletionCode = eRPProductionPropertyInformationDto.xapSfeEndJobCompletionCode,
					xapSfeEndJobGoodQty = eRPProductionPropertyInformationDto.xapSfeEndJobGoodQty,
					xapSfeEndJobScrapQty = eRPProductionPropertyInformationDto.xapSfeEndJobScrapQty,
					xapSfeIssueMaterialQty = eRPProductionPropertyInformationDto.xapSfeIssueMaterialQty,
					xapSfeJobSearchSelect = eRPProductionPropertyInformationDto.xapSfeJobSearchSelect,
					xapSfeJobTraveller = eRPProductionPropertyInformationDto.xapSfeJobTraveller,
					xapSfeOprSearchFields = eRPProductionPropertyInformationDto.xapSfeOprSearchFields,
					xapSfeSetupPercentage = eRPProductionPropertyInformationDto.xapSfeSetupPercentage,
					xapSfeStartJobWorkCode = eRPProductionPropertyInformationDto.xapSfeStartJobWorkCode,
					xapSfeTCAuditReport = eRPProductionPropertyInformationDto.xapSfeTCAuditReport,
					xapSfeWorkQueueFields = eRPProductionPropertyInformationDto.xapSfeWorkQueueFields,
					xapSfeWorkQueueSort = eRPProductionPropertyInformationDto.xapSfeWorkQueueSort,
					xapShowQtyOnHandMobInv = eRPProductionPropertyInformationDto.xapShowQtyOnHandMobInv,
					xapSmEdi856CustomLabel = eRPProductionPropertyInformationDto.xapSmEdi856CustomLabel,
					xapSmLineQuantityValidation = eRPProductionPropertyInformationDto.xapSmLineQuantityValidation,
					CustomFields = eRPProductionPropertyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProductionProperties []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProductionPropertyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = productionPropertyDto
			};
		}
		return result;
	}
}
