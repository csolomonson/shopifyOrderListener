using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPProductionPropertyRepository : APIBaseRepository, IERPProductionPropertyRepository, IAPIBaseRepository, IDisposable
{
	public ERPProductionPropertyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProductionPropertyExist(Guid productionPropertyId)
	{
		InitializeParameterLists();
		base.filterList.Add("xapUniqueID|C", productionPropertyId);
		base.selectList.Add("xapUniqueID");
		return Task.FromResult(GetAsObject("ProductionProperties", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProductionPropertyInformationDto>> GetAllProductionProperties(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProductionPropertyInformationDto> collection = new List<ERPProductionPropertyInformationDto>();
		InitializeParameterLists();
		string[] array = new string[227]
		{
			"xapAllowNegQtyOnHandHistory", "xapAnonymousCustomerID", "xapChChangeRequestTypeID", "xapCmArPaymentCreditMessage", "xapCmArPaymentHoldMessage", "xapCmFieldServiceCreditMessage", "xapCmFieldServiceHoldMessage", "xapCmNonTaxReasonID", "xapCmOrderCreditMessage", "xapCmOrderHoldMessage",
			"xapCmSalesPersonDefaultLoc", "xapCmShipmentCreditMessage", "xapCmShipmentHoldMessage", "xapCostingMethodHistory", "xapCreatedBy", "xapCreatedDate", "xapDateToSchedule", "xapDcAutoClockOutComputer", "xapDcIdleTimeThreshhold", "xapDcLaborCalculationMethod",
			"xapDcPayCalculationMethod", "xapDcRefreshInterval", "xapDcSfeInspectionPassword", "xapDcSfeShutdownPassword", "xapDcTimeFormat", "xapDMDefaultFolder", "xapEdi810ServiceUrl", "xapEdi856ServiceUrl", "xapEdiPassword", "xapEdiUserName",
			"xapUniqueID", "xapHdAttachmentFilePath", "xapHdcallDueDateDays", "xapHdcallTypeID", "xapHdcontactMethodID", "xapHdMailMergeCallTypeID", "xapHdMailMergeContactMethodID", "xapHdNewCallSoundFile", "xapHdSalesCallTypeID", "xapImAutoCreateRevisionID",
			"xapImCostingMethod", "xapImMfgDefaultCostType", "xapCmCreateJobOnly", "xapCmCreditLimitSourceInv", "xapCmCreditLimitSourceOrder", "xapCmCreditLimitSourceShip", "xapCmCustomerTaxable", "xapCmEnableResellers", "xapCmIncludeFreightInPrice", "xapDcAllowNegativeQty",
			"xapDcAllowProductionComplete", "xapDcAutoClockOutLocked", "xapDcEnableCreateSequence", "xapDcEnableIssueMaterial", "xapDcEnableJobTraveler", "xapDcEnableMinimizeButtonInSfe", "xapDcEnableTimecardAudit", "xapDcEnableWorkQueue", "xapDcPromptForActivityPassword", "xapDcPromptForAuditPassword",
			"xapDcPromptForClockInPassword", "xapDcPromptForLaborDescription", "xapDcPromptForMessagePassword", "xapDcPromptForReason", "xapDcShowCurrentJobsOnly", "xapDcSplitDirectLaborHours", "xapDcSplitIndirectLaborHours", "xapDcUseServerTime", "xapDcWarnOnOutsideOperation", "xapDcWarnOnOverProduction",
			"xapGlCreateStockJournals", "xapHdcreateCallForEmails", "xapImAllowNegativeQtyOnHand", "xapImAutoCreateRevision", "xapImCopyAlternates", "xapImCopyPartMemos", "xapImCopyPartOrgReferences", "xapImCopyPartPrices", "xapImCopyPartRules", "xapImEnableOrgPartCustomer",
			"xapImEnableOrgPartSupplier", "xapImEnableWarningWhenNegative", "xapImForceConfiguratorScreens", "xapImHideUseMethodInTree", "xapImIgnoreLCInStdCostRollup", "xapImOnlyAllowExistingBins", "xapImOverwriteDescription", "xapImOverwriteDocuments", "xapImOverwriteMethod", "xapImRefreshMaterial",
			"xapImRefreshMaterialCosts", "xapImScrapRoundUp", "xapImSetUseMethod", "xapImTransferCustomer", "xapImTransferDescriptions", "xapImTransferMaterial", "xapImUseStdForStdCostRollUp", "xapJmExcessQuantity", "xapJmIgnoreEmployees", "xapJmIgnoreMachines",
			"xapJmLoadLevelFinite", "xapJmMinimizeGaps", "xapJmMRPForecastFirmJob", "xapJmOverwriteDescription", "xapJmOverwriteDocuments", "xapJmOverwriteMethod", "xapJmRefreshHours", "xapJmRefreshMaterial", "xapJmRefreshMaterialCosts", "xapJmScheduleShowActualTimes",
			"xapJmScheduleUseActuals", "xapJmShopLoadShowFutureLoad", "xapJmShopLoadShowPastLoad", "xapLmUpdateActualWithRounded", "xapNextSerialNumberPerGroup", "xapOmAutoCreateDelivery", "xapOmEnableDiscountFields", "xapOmEnableFreightFields", "xapOmIncludeOrderDeliveryInJob", "xapOmIncludeOrderLineInJob",
			"xapOmMarkCreateJobForMto", "xapOmMarkPullQuoteMethodForMto", "xapOmShowDeliveriesInTree", "xapOmUseQuotingMarkupTM", "xapPmPTOUsesDeliveryCost", "xapPmPurPlannerIncWhsQties", "xapPmPurPlannerUseBestPrice", "xapPmShowFirmOnlyPoWiz", "xapPoWizardShowQtyToInspect", "xapPRUseFirmQuotesOnly",
			"xapQArmaRequiresInspection", "xapQAShowRmaOtherInfo", "xapQmMultipleQuantities", "xapQmMUseDefHeaderFooterText", "xapQmOverwriteDescription", "xapQmOverwriteDocuments", "xapQmOverwriteMethod", "xapQmRefreshMaterial", "xapQmRefreshMaterialCosts", "xapQmRefreshRateInfo",
			"xapRQGroupPobyRfq", "xapRQIncludeAlternateParts", "xapSfeAllowSuspend", "xapSfeBarcodeScanner", "xapSfeTouchScreen", "xapSmDeleteZeroShipmentLines", "xapJmCalendarExportFields", "xapJmInitialExtension", "xapJmInsideInspectionLineRTF", "xapJmInsideInspectionLineText",
			"xapJmJobMaterialSource", "xapJmLoadReliefMethod", "xapJmOutsideInspectionLineRTF", "xapJmOutsideInspectionLineText", "xapJmScheduleBoardFields", "xapJmScheduleType", "xapJmShopLoadBuckets", "xapJmShopLoadDays", "xapJmShopLoadDepartmentID", "xapJmShopLoadFields",
			"xapJmShopLoadPlantID", "xapJmShopLoadTimeType", "xapJmSplitCosts", "xapJmStandardFactor", "xapLmCalculateEndTime", "xapLmLeaveBoardFields", "xapLOResponseMethodID", "xapNextSerialNumberIDFormula", "xapOmAddlChargePartID", "xapOmAddlChargePartRevisionID",
			"xapOmDeliveryType", "xapOmFreeOnBoardDescription", "xapOmLineQuantityValidation", "xapOmOrderDeliveryDigits", "xapOmOrderLineDigits", "xapOmSalesGlAccountID", "xapOmUnitOfMeasure", "xapPACalendarExportFields", "xapPAExportFormat", "xapPAExportLocation",
			"xapPmCostingMethod", "xapPmDefaultDueDate", "xapPmFollowUpDays", "xapPmPoWizardDisplayType", "xapPmPurchaseType", "xapPmTaxExemptNumber", "xapPRLaborMethod", "xapQAInspQueueRefreshInterval", "xapQmAdditionalChargeText", "xapQmExpirationDays",
			"xapQmFollowUpDays", "xapQmFollowUpType", "xapQmLaborMarkup", "xapQmMaterialMarkup", "xapQmMQuoteFooterMessageRTF", "xapQmMQuoteFooterMessageText", "xapQmMQuoteHeaderMessageRTF", "xapQmMQuoteHeaderMessageText", "xapQmOverheadMarkup", "xapQmPurchaseToOrderMarkup",
			"xapQmQuoteFooterMessageRTF", "xapQmQuoteFooterMessageText", "xapQmQuoteHeaderMessageRTF", "xapQmQuoteHeaderMessageText", "xapQmQuoteMarkupType", "xapQmQuotingMarkup", "xapQmQuotingMethod", "xapQmSubcontractMarkup", "xapRowVersion", "xapSfeActiveJobQueueFields",
			"xapSfeAddPartSelect", "xapSfeAsmSearchFields", "xapSfeEndJobCompletionCode", "xapSfeEndJobGoodQty", "xapSfeEndJobScrapQty", "xapSfeIssueMaterialQty", "xapSfeJobSearchSelect", "xapSfeJobTraveller", "xapSfeOprSearchFields", "xapSfeSetupPercentage",
			"xapSfeStartJobWorkCode", "xapSfeTCAuditReport", "xapSfeWorkQueueFields", "xapSfeWorkQueueSort", "xapShowQtyOnHandMobInv", "xapSmEdi856CustomLabel", "xapSmLineQuantityValidation"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProductionProperties");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("ProductionProperties", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProductionPropertyInformationDto eRPProductionPropertyInformationDto = new ERPProductionPropertyInformationDto();
				eRPProductionPropertyInformationDto.xapAllowNegQtyOnHandHistory = dataTable.Rows[i].Field<string>("xapAllowNegQtyOnHandHistory");
				eRPProductionPropertyInformationDto.xapAnonymousCustomerID = dataTable.Rows[i].Field<string>("xapAnonymousCustomerID");
				eRPProductionPropertyInformationDto.xapChChangeRequestTypeID = dataTable.Rows[i].Field<string>("xapChChangeRequestTypeID");
				eRPProductionPropertyInformationDto.xapCmArPaymentCreditMessage = dataTable.Rows[i].Field<byte>("xapCmArPaymentCreditMessage");
				eRPProductionPropertyInformationDto.xapCmArPaymentHoldMessage = dataTable.Rows[i].Field<byte>("xapCmArPaymentHoldMessage");
				eRPProductionPropertyInformationDto.xapCmFieldServiceCreditMessage = dataTable.Rows[i].Field<byte>("xapCmFieldServiceCreditMessage");
				eRPProductionPropertyInformationDto.xapCmFieldServiceHoldMessage = dataTable.Rows[i].Field<byte>("xapCmFieldServiceHoldMessage");
				eRPProductionPropertyInformationDto.xapCmNonTaxReasonID = dataTable.Rows[i].Field<string>("xapCmNonTaxReasonID");
				eRPProductionPropertyInformationDto.xapCmOrderCreditMessage = dataTable.Rows[i].Field<byte>("xapCmOrderCreditMessage");
				eRPProductionPropertyInformationDto.xapCmOrderHoldMessage = dataTable.Rows[i].Field<byte>("xapCmOrderHoldMessage");
				eRPProductionPropertyInformationDto.xapCmSalesPersonDefaultLoc = dataTable.Rows[i].Field<byte>("xapCmSalesPersonDefaultLoc");
				eRPProductionPropertyInformationDto.xapCmShipmentCreditMessage = dataTable.Rows[i].Field<byte>("xapCmShipmentCreditMessage");
				eRPProductionPropertyInformationDto.xapCmShipmentHoldMessage = dataTable.Rows[i].Field<byte>("xapCmShipmentHoldMessage");
				eRPProductionPropertyInformationDto.xapCostingMethodHistory = dataTable.Rows[i].Field<string>("xapCostingMethodHistory");
				eRPProductionPropertyInformationDto.xapCreatedBy = dataTable.Rows[i].Field<string>("xapCreatedBy");
				eRPProductionPropertyInformationDto.xapCreatedDate = dataTable.Rows[i].Field<DateTime?>("xapCreatedDate");
				eRPProductionPropertyInformationDto.xapDateToSchedule = dataTable.Rows[i].Field<DateTime?>("xapDateToSchedule");
				eRPProductionPropertyInformationDto.xapDcAutoClockOutComputer = dataTable.Rows[i].Field<string>("xapDcAutoClockOutComputer");
				eRPProductionPropertyInformationDto.xapDcIdleTimeThreshhold = dataTable.Rows[i].Field<byte>("xapDcIdleTimeThreshhold");
				eRPProductionPropertyInformationDto.xapDcLaborCalculationMethod = dataTable.Rows[i].Field<byte>("xapDcLaborCalculationMethod");
				eRPProductionPropertyInformationDto.xapDcPayCalculationMethod = dataTable.Rows[i].Field<byte>("xapDcPayCalculationMethod");
				eRPProductionPropertyInformationDto.xapDcRefreshInterval = dataTable.Rows[i].Field<byte>("xapDcRefreshInterval");
				eRPProductionPropertyInformationDto.xapDcSfeInspectionPassword = dataTable.Rows[i].Field<string>("xapDcSfeInspectionPassword");
				eRPProductionPropertyInformationDto.xapDcSfeShutdownPassword = dataTable.Rows[i].Field<string>("xapDcSfeShutdownPassword");
				eRPProductionPropertyInformationDto.xapDcTimeFormat = dataTable.Rows[i].Field<byte>("xapDcTimeFormat");
				eRPProductionPropertyInformationDto.xapDMDefaultFolder = dataTable.Rows[i].Field<string>("xapDMDefaultFolder");
				eRPProductionPropertyInformationDto.xapEdi810ServiceUrl = dataTable.Rows[i].Field<string>("xapEdi810ServiceUrl");
				eRPProductionPropertyInformationDto.xapEdi856ServiceUrl = dataTable.Rows[i].Field<string>("xapEdi856ServiceUrl");
				eRPProductionPropertyInformationDto.xapEdiPassword = dataTable.Rows[i].Field<string>("xapEdiPassword");
				eRPProductionPropertyInformationDto.xapEdiUserName = dataTable.Rows[i].Field<string>("xapEdiUserName");
				eRPProductionPropertyInformationDto.xapUniqueID = dataTable.Rows[i].Field<Guid>("xapUniqueID");
				eRPProductionPropertyInformationDto.xapHdAttachmentFilePath = dataTable.Rows[i].Field<string>("xapHdAttachmentFilePath");
				eRPProductionPropertyInformationDto.xapHdcallDueDateDays = dataTable.Rows[i].Field<short>("xapHdcallDueDateDays");
				eRPProductionPropertyInformationDto.xapHdcallTypeID = dataTable.Rows[i].Field<string>("xapHdcallTypeID");
				eRPProductionPropertyInformationDto.xapHdcontactMethodID = dataTable.Rows[i].Field<string>("xapHdcontactMethodID");
				eRPProductionPropertyInformationDto.xapHdMailMergeCallTypeID = dataTable.Rows[i].Field<string>("xapHdMailMergeCallTypeID");
				eRPProductionPropertyInformationDto.xapHdMailMergeContactMethodID = dataTable.Rows[i].Field<string>("xapHdMailMergeContactMethodID");
				eRPProductionPropertyInformationDto.xapHdNewCallSoundFile = dataTable.Rows[i].Field<string>("xapHdNewCallSoundFile");
				eRPProductionPropertyInformationDto.xapHdSalesCallTypeID = dataTable.Rows[i].Field<string>("xapHdSalesCallTypeID");
				eRPProductionPropertyInformationDto.xapImAutoCreateRevisionID = dataTable.Rows[i].Field<string>("xapImAutoCreateRevisionID");
				eRPProductionPropertyInformationDto.xapImCostingMethod = dataTable.Rows[i].Field<byte>("xapImCostingMethod");
				eRPProductionPropertyInformationDto.xapImMfgDefaultCostType = dataTable.Rows[i].Field<byte>("xapImMfgDefaultCostType");
				eRPProductionPropertyInformationDto.xapCmCreateJobOnly = dataTable.Rows[i].Field<bool>("xapCmCreateJobOnly");
				eRPProductionPropertyInformationDto.xapCmCreditLimitSourceInv = dataTable.Rows[i].Field<bool>("xapCmCreditLimitSourceInv");
				eRPProductionPropertyInformationDto.xapCmCreditLimitSourceOrder = dataTable.Rows[i].Field<bool>("xapCmCreditLimitSourceOrder");
				eRPProductionPropertyInformationDto.xapCmCreditLimitSourceShip = dataTable.Rows[i].Field<bool>("xapCmCreditLimitSourceShip");
				eRPProductionPropertyInformationDto.xapCmCustomerTaxable = dataTable.Rows[i].Field<bool>("xapCmCustomerTaxable");
				eRPProductionPropertyInformationDto.xapCmEnableResellers = dataTable.Rows[i].Field<bool>("xapCmEnableResellers");
				eRPProductionPropertyInformationDto.xapCmIncludeFreightInPrice = dataTable.Rows[i].Field<bool>("xapCmIncludeFreightInPrice");
				eRPProductionPropertyInformationDto.xapDcAllowNegativeQty = dataTable.Rows[i].Field<bool>("xapDcAllowNegativeQty");
				eRPProductionPropertyInformationDto.xapDcAllowProductionComplete = dataTable.Rows[i].Field<bool>("xapDcAllowProductionComplete");
				eRPProductionPropertyInformationDto.xapDcAutoClockOutLocked = dataTable.Rows[i].Field<bool>("xapDcAutoClockOutLocked");
				eRPProductionPropertyInformationDto.xapDcEnableCreateSequence = dataTable.Rows[i].Field<bool>("xapDcEnableCreateSequence");
				eRPProductionPropertyInformationDto.xapDcEnableIssueMaterial = dataTable.Rows[i].Field<bool>("xapDcEnableIssueMaterial");
				eRPProductionPropertyInformationDto.xapDcEnableJobTraveler = dataTable.Rows[i].Field<bool>("xapDcEnableJobTraveler");
				eRPProductionPropertyInformationDto.xapDcEnableMinimizeButtonInSfe = dataTable.Rows[i].Field<bool>("xapDcEnableMinimizeButtonInSfe");
				eRPProductionPropertyInformationDto.xapDcEnableTimecardAudit = dataTable.Rows[i].Field<bool>("xapDcEnableTimecardAudit");
				eRPProductionPropertyInformationDto.xapDcEnableWorkQueue = dataTable.Rows[i].Field<bool>("xapDcEnableWorkQueue");
				eRPProductionPropertyInformationDto.xapDcPromptForActivityPassword = dataTable.Rows[i].Field<bool>("xapDcPromptForActivityPassword");
				eRPProductionPropertyInformationDto.xapDcPromptForAuditPassword = dataTable.Rows[i].Field<bool>("xapDcPromptForAuditPassword");
				eRPProductionPropertyInformationDto.xapDcPromptForClockInPassword = dataTable.Rows[i].Field<bool>("xapDcPromptForClockInPassword");
				eRPProductionPropertyInformationDto.xapDcPromptForLaborDescription = dataTable.Rows[i].Field<bool>("xapDcPromptForLaborDescription");
				eRPProductionPropertyInformationDto.xapDcPromptForMessagePassword = dataTable.Rows[i].Field<bool>("xapDcPromptForMessagePassword");
				eRPProductionPropertyInformationDto.xapDcPromptForReason = dataTable.Rows[i].Field<bool>("xapDcPromptForReason");
				eRPProductionPropertyInformationDto.xapDcShowCurrentJobsOnly = dataTable.Rows[i].Field<bool>("xapDcShowCurrentJobsOnly");
				eRPProductionPropertyInformationDto.xapDcSplitDirectLaborHours = dataTable.Rows[i].Field<bool>("xapDcSplitDirectLaborHours");
				eRPProductionPropertyInformationDto.xapDcSplitIndirectLaborHours = dataTable.Rows[i].Field<bool>("xapDcSplitIndirectLaborHours");
				eRPProductionPropertyInformationDto.xapDcUseServerTime = dataTable.Rows[i].Field<bool>("xapDcUseServerTime");
				eRPProductionPropertyInformationDto.xapDcWarnOnOutsideOperation = dataTable.Rows[i].Field<bool>("xapDcWarnOnOutsideOperation");
				eRPProductionPropertyInformationDto.xapDcWarnOnOverProduction = dataTable.Rows[i].Field<bool>("xapDcWarnOnOverProduction");
				eRPProductionPropertyInformationDto.xapGlCreateStockJournals = dataTable.Rows[i].Field<bool>("xapGlCreateStockJournals");
				eRPProductionPropertyInformationDto.xapHdcreateCallForEmails = dataTable.Rows[i].Field<bool>("xapHdcreateCallForEmails");
				eRPProductionPropertyInformationDto.xapImAllowNegativeQtyOnHand = dataTable.Rows[i].Field<bool>("xapImAllowNegativeQtyOnHand");
				eRPProductionPropertyInformationDto.xapImAutoCreateRevision = dataTable.Rows[i].Field<bool>("xapImAutoCreateRevision");
				eRPProductionPropertyInformationDto.xapImCopyAlternates = dataTable.Rows[i].Field<bool>("xapImCopyAlternates");
				eRPProductionPropertyInformationDto.xapImCopyPartMemos = dataTable.Rows[i].Field<bool>("xapImCopyPartMemos");
				eRPProductionPropertyInformationDto.xapImCopyPartOrgReferences = dataTable.Rows[i].Field<bool>("xapImCopyPartOrgReferences");
				eRPProductionPropertyInformationDto.xapImCopyPartPrices = dataTable.Rows[i].Field<bool>("xapImCopyPartPrices");
				eRPProductionPropertyInformationDto.xapImCopyPartRules = dataTable.Rows[i].Field<bool>("xapImCopyPartRules");
				eRPProductionPropertyInformationDto.xapImEnableOrgPartCustomer = dataTable.Rows[i].Field<bool>("xapImEnableOrgPartCustomer");
				eRPProductionPropertyInformationDto.xapImEnableOrgPartSupplier = dataTable.Rows[i].Field<bool>("xapImEnableOrgPartSupplier");
				eRPProductionPropertyInformationDto.xapImEnableWarningWhenNegative = dataTable.Rows[i].Field<bool>("xapImEnableWarningWhenNegative");
				eRPProductionPropertyInformationDto.xapImForceConfiguratorScreens = dataTable.Rows[i].Field<bool>("xapImForceConfiguratorScreens");
				eRPProductionPropertyInformationDto.xapImHideUseMethodInTree = dataTable.Rows[i].Field<bool>("xapImHideUseMethodInTree");
				eRPProductionPropertyInformationDto.xapImIgnoreLCInStdCostRollup = dataTable.Rows[i].Field<bool>("xapImIgnoreLCInStdCostRollup");
				eRPProductionPropertyInformationDto.xapImOnlyAllowExistingBins = dataTable.Rows[i].Field<bool>("xapImOnlyAllowExistingBins");
				eRPProductionPropertyInformationDto.xapImOverwriteDescription = dataTable.Rows[i].Field<bool>("xapImOverwriteDescription");
				eRPProductionPropertyInformationDto.xapImOverwriteDocuments = dataTable.Rows[i].Field<bool>("xapImOverwriteDocuments");
				eRPProductionPropertyInformationDto.xapImOverwriteMethod = dataTable.Rows[i].Field<bool>("xapImOverwriteMethod");
				eRPProductionPropertyInformationDto.xapImRefreshMaterial = dataTable.Rows[i].Field<bool>("xapImRefreshMaterial");
				eRPProductionPropertyInformationDto.xapImRefreshMaterialCosts = dataTable.Rows[i].Field<bool>("xapImRefreshMaterialCosts");
				eRPProductionPropertyInformationDto.xapImScrapRoundUp = dataTable.Rows[i].Field<bool>("xapImScrapRoundUp");
				eRPProductionPropertyInformationDto.xapImSetUseMethod = dataTable.Rows[i].Field<bool>("xapImSetUseMethod");
				eRPProductionPropertyInformationDto.xapImTransferCustomer = dataTable.Rows[i].Field<bool>("xapImTransferCustomer");
				eRPProductionPropertyInformationDto.xapImTransferDescriptions = dataTable.Rows[i].Field<bool>("xapImTransferDescriptions");
				eRPProductionPropertyInformationDto.xapImTransferMaterial = dataTable.Rows[i].Field<bool>("xapImTransferMaterial");
				eRPProductionPropertyInformationDto.xapImUseStdForStdCostRollUp = dataTable.Rows[i].Field<bool>("xapImUseStdForStdCostRollUp");
				eRPProductionPropertyInformationDto.xapJmExcessQuantity = dataTable.Rows[i].Field<bool>("xapJmExcessQuantity");
				eRPProductionPropertyInformationDto.xapJmIgnoreEmployees = dataTable.Rows[i].Field<bool>("xapJmIgnoreEmployees");
				eRPProductionPropertyInformationDto.xapJmIgnoreMachines = dataTable.Rows[i].Field<bool>("xapJmIgnoreMachines");
				eRPProductionPropertyInformationDto.xapJmLoadLevelFinite = dataTable.Rows[i].Field<bool>("xapJmLoadLevelFinite");
				eRPProductionPropertyInformationDto.xapJmMinimizeGaps = dataTable.Rows[i].Field<bool>("xapJmMinimizeGaps");
				eRPProductionPropertyInformationDto.xapJmMRPForecastFirmJob = dataTable.Rows[i].Field<bool>("xapJmMRPForecastFirmJob");
				eRPProductionPropertyInformationDto.xapJmOverwriteDescription = dataTable.Rows[i].Field<bool>("xapJmOverwriteDescription");
				eRPProductionPropertyInformationDto.xapJmOverwriteDocuments = dataTable.Rows[i].Field<bool>("xapJmOverwriteDocuments");
				eRPProductionPropertyInformationDto.xapJmOverwriteMethod = dataTable.Rows[i].Field<bool>("xapJmOverwriteMethod");
				eRPProductionPropertyInformationDto.xapJmRefreshHours = dataTable.Rows[i].Field<bool>("xapJmRefreshHours");
				eRPProductionPropertyInformationDto.xapJmRefreshMaterial = dataTable.Rows[i].Field<bool>("xapJmRefreshMaterial");
				eRPProductionPropertyInformationDto.xapJmRefreshMaterialCosts = dataTable.Rows[i].Field<bool>("xapJmRefreshMaterialCosts");
				eRPProductionPropertyInformationDto.xapJmScheduleShowActualTimes = dataTable.Rows[i].Field<bool>("xapJmScheduleShowActualTimes");
				eRPProductionPropertyInformationDto.xapJmScheduleUseActuals = dataTable.Rows[i].Field<bool>("xapJmScheduleUseActuals");
				eRPProductionPropertyInformationDto.xapJmShopLoadShowFutureLoad = dataTable.Rows[i].Field<bool>("xapJmShopLoadShowFutureLoad");
				eRPProductionPropertyInformationDto.xapJmShopLoadShowPastLoad = dataTable.Rows[i].Field<bool>("xapJmShopLoadShowPastLoad");
				eRPProductionPropertyInformationDto.xapLmUpdateActualWithRounded = dataTable.Rows[i].Field<bool>("xapLmUpdateActualWithRounded");
				eRPProductionPropertyInformationDto.xapNextSerialNumberPerGroup = dataTable.Rows[i].Field<bool>("xapNextSerialNumberPerGroup");
				eRPProductionPropertyInformationDto.xapOmAutoCreateDelivery = dataTable.Rows[i].Field<bool>("xapOmAutoCreateDelivery");
				eRPProductionPropertyInformationDto.xapOmEnableDiscountFields = dataTable.Rows[i].Field<bool>("xapOmEnableDiscountFields");
				eRPProductionPropertyInformationDto.xapOmEnableFreightFields = dataTable.Rows[i].Field<bool>("xapOmEnableFreightFields");
				eRPProductionPropertyInformationDto.xapOmIncludeOrderDeliveryInJob = dataTable.Rows[i].Field<bool>("xapOmIncludeOrderDeliveryInJob");
				eRPProductionPropertyInformationDto.xapOmIncludeOrderLineInJob = dataTable.Rows[i].Field<bool>("xapOmIncludeOrderLineInJob");
				eRPProductionPropertyInformationDto.xapOmMarkCreateJobForMto = dataTable.Rows[i].Field<bool>("xapOmMarkCreateJobForMto");
				eRPProductionPropertyInformationDto.xapOmMarkPullQuoteMethodForMto = dataTable.Rows[i].Field<bool>("xapOmMarkPullQuoteMethodForMto");
				eRPProductionPropertyInformationDto.xapOmShowDeliveriesInTree = dataTable.Rows[i].Field<bool>("xapOmShowDeliveriesInTree");
				eRPProductionPropertyInformationDto.xapOmUseQuotingMarkupTM = dataTable.Rows[i].Field<bool>("xapOmUseQuotingMarkupTM");
				eRPProductionPropertyInformationDto.xapPmPTOUsesDeliveryCost = dataTable.Rows[i].Field<bool>("xapPmPTOUsesDeliveryCost");
				eRPProductionPropertyInformationDto.xapPmPurPlannerIncWhsQties = dataTable.Rows[i].Field<bool>("xapPmPurPlannerIncWhsQties");
				eRPProductionPropertyInformationDto.xapPmPurPlannerUseBestPrice = dataTable.Rows[i].Field<bool>("xapPmPurPlannerUseBestPrice");
				eRPProductionPropertyInformationDto.xapPmShowFirmOnlyPoWiz = dataTable.Rows[i].Field<bool>("xapPmShowFirmOnlyPoWiz");
				eRPProductionPropertyInformationDto.xapPoWizardShowQtyToInspect = dataTable.Rows[i].Field<bool>("xapPoWizardShowQtyToInspect");
				eRPProductionPropertyInformationDto.xapPRUseFirmQuotesOnly = dataTable.Rows[i].Field<bool>("xapPRUseFirmQuotesOnly");
				eRPProductionPropertyInformationDto.xapQArmaRequiresInspection = dataTable.Rows[i].Field<bool>("xapQArmaRequiresInspection");
				eRPProductionPropertyInformationDto.xapQAShowRmaOtherInfo = dataTable.Rows[i].Field<bool>("xapQAShowRmaOtherInfo");
				eRPProductionPropertyInformationDto.xapQmMultipleQuantities = dataTable.Rows[i].Field<bool>("xapQmMultipleQuantities");
				eRPProductionPropertyInformationDto.xapQmMUseDefHeaderFooterText = dataTable.Rows[i].Field<bool>("xapQmMUseDefHeaderFooterText");
				eRPProductionPropertyInformationDto.xapQmOverwriteDescription = dataTable.Rows[i].Field<bool>("xapQmOverwriteDescription");
				eRPProductionPropertyInformationDto.xapQmOverwriteDocuments = dataTable.Rows[i].Field<bool>("xapQmOverwriteDocuments");
				eRPProductionPropertyInformationDto.xapQmOverwriteMethod = dataTable.Rows[i].Field<bool>("xapQmOverwriteMethod");
				eRPProductionPropertyInformationDto.xapQmRefreshMaterial = dataTable.Rows[i].Field<bool>("xapQmRefreshMaterial");
				eRPProductionPropertyInformationDto.xapQmRefreshMaterialCosts = dataTable.Rows[i].Field<bool>("xapQmRefreshMaterialCosts");
				eRPProductionPropertyInformationDto.xapQmRefreshRateInfo = dataTable.Rows[i].Field<bool>("xapQmRefreshRateInfo");
				eRPProductionPropertyInformationDto.xapRQGroupPobyRfq = dataTable.Rows[i].Field<bool>("xapRQGroupPobyRfq");
				eRPProductionPropertyInformationDto.xapRQIncludeAlternateParts = dataTable.Rows[i].Field<bool>("xapRQIncludeAlternateParts");
				eRPProductionPropertyInformationDto.xapSfeAllowSuspend = dataTable.Rows[i].Field<bool>("xapSfeAllowSuspend");
				eRPProductionPropertyInformationDto.xapSfeBarcodeScanner = dataTable.Rows[i].Field<bool>("xapSfeBarcodeScanner");
				eRPProductionPropertyInformationDto.xapSfeTouchScreen = dataTable.Rows[i].Field<bool>("xapSfeTouchScreen");
				eRPProductionPropertyInformationDto.xapSmDeleteZeroShipmentLines = dataTable.Rows[i].Field<bool>("xapSmDeleteZeroShipmentLines");
				eRPProductionPropertyInformationDto.xapJmCalendarExportFields = dataTable.Rows[i].Field<string>("xapJmCalendarExportFields");
				eRPProductionPropertyInformationDto.xapJmInitialExtension = dataTable.Rows[i].Field<string>("xapJmInitialExtension");
				eRPProductionPropertyInformationDto.xapJmInsideInspectionLineRTF = dataTable.Rows[i].Field<string>("xapJmInsideInspectionLineRTF");
				eRPProductionPropertyInformationDto.xapJmInsideInspectionLineText = dataTable.Rows[i].Field<string>("xapJmInsideInspectionLineText");
				eRPProductionPropertyInformationDto.xapJmJobMaterialSource = dataTable.Rows[i].Field<byte>("xapJmJobMaterialSource");
				eRPProductionPropertyInformationDto.xapJmLoadReliefMethod = dataTable.Rows[i].Field<byte>("xapJmLoadReliefMethod");
				eRPProductionPropertyInformationDto.xapJmOutsideInspectionLineRTF = dataTable.Rows[i].Field<string>("xapJmOutsideInspectionLineRTF");
				eRPProductionPropertyInformationDto.xapJmOutsideInspectionLineText = dataTable.Rows[i].Field<string>("xapJmOutsideInspectionLineText");
				eRPProductionPropertyInformationDto.xapJmScheduleBoardFields = dataTable.Rows[i].Field<string>("xapJmScheduleBoardFields");
				eRPProductionPropertyInformationDto.xapJmScheduleType = dataTable.Rows[i].Field<byte>("xapJmScheduleType");
				eRPProductionPropertyInformationDto.xapJmShopLoadBuckets = dataTable.Rows[i].Field<byte>("xapJmShopLoadBuckets");
				eRPProductionPropertyInformationDto.xapJmShopLoadDays = dataTable.Rows[i].Field<byte>("xapJmShopLoadDays");
				eRPProductionPropertyInformationDto.xapJmShopLoadDepartmentID = dataTable.Rows[i].Field<string>("xapJmShopLoadDepartmentID");
				eRPProductionPropertyInformationDto.xapJmShopLoadFields = dataTable.Rows[i].Field<string>("xapJmShopLoadFields");
				eRPProductionPropertyInformationDto.xapJmShopLoadPlantID = dataTable.Rows[i].Field<string>("xapJmShopLoadPlantID");
				eRPProductionPropertyInformationDto.xapJmShopLoadTimeType = dataTable.Rows[i].Field<string>("xapJmShopLoadTimeType");
				eRPProductionPropertyInformationDto.xapJmSplitCosts = dataTable.Rows[i].Field<byte>("xapJmSplitCosts");
				eRPProductionPropertyInformationDto.xapJmStandardFactor = dataTable.Rows[i].Field<string>("xapJmStandardFactor");
				eRPProductionPropertyInformationDto.xapLmCalculateEndTime = dataTable.Rows[i].Field<byte>("xapLmCalculateEndTime");
				eRPProductionPropertyInformationDto.xapLmLeaveBoardFields = dataTable.Rows[i].Field<string>("xapLmLeaveBoardFields");
				eRPProductionPropertyInformationDto.xapLOResponseMethodID = dataTable.Rows[i].Field<string>("xapLOResponseMethodID");
				eRPProductionPropertyInformationDto.xapNextSerialNumberIDFormula = dataTable.Rows[i].Field<string>("xapNextSerialNumberIDFormula");
				eRPProductionPropertyInformationDto.xapOmAddlChargePartID = dataTable.Rows[i].Field<string>("xapOmAddlChargePartID");
				eRPProductionPropertyInformationDto.xapOmAddlChargePartRevisionID = dataTable.Rows[i].Field<string>("xapOmAddlChargePartRevisionID");
				eRPProductionPropertyInformationDto.xapOmDeliveryType = dataTable.Rows[i].Field<byte>("xapOmDeliveryType");
				eRPProductionPropertyInformationDto.xapOmFreeOnBoardDescription = dataTable.Rows[i].Field<string>("xapOmFreeOnBoardDescription");
				eRPProductionPropertyInformationDto.xapOmLineQuantityValidation = dataTable.Rows[i].Field<byte>("xapOmLineQuantityValidation");
				eRPProductionPropertyInformationDto.xapOmOrderDeliveryDigits = dataTable.Rows[i].Field<byte>("xapOmOrderDeliveryDigits");
				eRPProductionPropertyInformationDto.xapOmOrderLineDigits = dataTable.Rows[i].Field<byte>("xapOmOrderLineDigits");
				eRPProductionPropertyInformationDto.xapOmSalesGlAccountID = dataTable.Rows[i].Field<string>("xapOmSalesGlAccountID");
				eRPProductionPropertyInformationDto.xapOmUnitOfMeasure = dataTable.Rows[i].Field<string>("xapOmUnitOfMeasure");
				eRPProductionPropertyInformationDto.xapPACalendarExportFields = dataTable.Rows[i].Field<string>("xapPACalendarExportFields");
				eRPProductionPropertyInformationDto.xapPAExportFormat = dataTable.Rows[i].Field<string>("xapPAExportFormat");
				eRPProductionPropertyInformationDto.xapPAExportLocation = dataTable.Rows[i].Field<string>("xapPAExportLocation");
				eRPProductionPropertyInformationDto.xapPmCostingMethod = dataTable.Rows[i].Field<byte>("xapPmCostingMethod");
				eRPProductionPropertyInformationDto.xapPmDefaultDueDate = dataTable.Rows[i].Field<DateTime?>("xapPmDefaultDueDate");
				eRPProductionPropertyInformationDto.xapPmFollowUpDays = dataTable.Rows[i].Field<short>("xapPmFollowUpDays");
				eRPProductionPropertyInformationDto.xapPmPoWizardDisplayType = dataTable.Rows[i].Field<byte>("xapPmPoWizardDisplayType");
				eRPProductionPropertyInformationDto.xapPmPurchaseType = dataTable.Rows[i].Field<byte>("xapPmPurchaseType");
				eRPProductionPropertyInformationDto.xapPmTaxExemptNumber = dataTable.Rows[i].Field<string>("xapPmTaxExemptNumber");
				eRPProductionPropertyInformationDto.xapPRLaborMethod = dataTable.Rows[i].Field<byte>("xapPRLaborMethod");
				eRPProductionPropertyInformationDto.xapQAInspQueueRefreshInterval = dataTable.Rows[i].Field<short>("xapQAInspQueueRefreshInterval");
				eRPProductionPropertyInformationDto.xapQmAdditionalChargeText = dataTable.Rows[i].Field<string>("xapQmAdditionalChargeText");
				eRPProductionPropertyInformationDto.xapQmExpirationDays = dataTable.Rows[i].Field<short>("xapQmExpirationDays");
				eRPProductionPropertyInformationDto.xapQmFollowUpDays = dataTable.Rows[i].Field<short>("xapQmFollowUpDays");
				eRPProductionPropertyInformationDto.xapQmFollowUpType = dataTable.Rows[i].Field<byte>("xapQmFollowUpType");
				eRPProductionPropertyInformationDto.xapQmLaborMarkup = dataTable.Rows[i].Field<decimal>("xapQmLaborMarkup");
				eRPProductionPropertyInformationDto.xapQmMaterialMarkup = dataTable.Rows[i].Field<decimal>("xapQmMaterialMarkup");
				eRPProductionPropertyInformationDto.xapQmMQuoteFooterMessageRTF = dataTable.Rows[i].Field<string>("xapQmMQuoteFooterMessageRTF");
				eRPProductionPropertyInformationDto.xapQmMQuoteFooterMessageText = dataTable.Rows[i].Field<string>("xapQmMQuoteFooterMessageText");
				eRPProductionPropertyInformationDto.xapQmMQuoteHeaderMessageRTF = dataTable.Rows[i].Field<string>("xapQmMQuoteHeaderMessageRTF");
				eRPProductionPropertyInformationDto.xapQmMQuoteHeaderMessageText = dataTable.Rows[i].Field<string>("xapQmMQuoteHeaderMessageText");
				eRPProductionPropertyInformationDto.xapQmOverheadMarkup = dataTable.Rows[i].Field<decimal>("xapQmOverheadMarkup");
				eRPProductionPropertyInformationDto.xapQmPurchaseToOrderMarkup = dataTable.Rows[i].Field<decimal>("xapQmPurchaseToOrderMarkup");
				eRPProductionPropertyInformationDto.xapQmQuoteFooterMessageRTF = dataTable.Rows[i].Field<string>("xapQmQuoteFooterMessageRTF");
				eRPProductionPropertyInformationDto.xapQmQuoteFooterMessageText = dataTable.Rows[i].Field<string>("xapQmQuoteFooterMessageText");
				eRPProductionPropertyInformationDto.xapQmQuoteHeaderMessageRTF = dataTable.Rows[i].Field<string>("xapQmQuoteHeaderMessageRTF");
				eRPProductionPropertyInformationDto.xapQmQuoteHeaderMessageText = dataTable.Rows[i].Field<string>("xapQmQuoteHeaderMessageText");
				eRPProductionPropertyInformationDto.xapQmQuoteMarkupType = dataTable.Rows[i].Field<byte>("xapQmQuoteMarkupType");
				eRPProductionPropertyInformationDto.xapQmQuotingMarkup = dataTable.Rows[i].Field<decimal>("xapQmQuotingMarkup");
				eRPProductionPropertyInformationDto.xapQmQuotingMethod = dataTable.Rows[i].Field<byte>("xapQmQuotingMethod");
				eRPProductionPropertyInformationDto.xapQmSubcontractMarkup = dataTable.Rows[i].Field<decimal>("xapQmSubcontractMarkup");
				eRPProductionPropertyInformationDto.xapRowVersion = dataTable.Rows[i].Field<byte[]>("xapRowVersion");
				eRPProductionPropertyInformationDto.xapSfeActiveJobQueueFields = dataTable.Rows[i].Field<string>("xapSfeActiveJobQueueFields");
				eRPProductionPropertyInformationDto.xapSfeAddPartSelect = dataTable.Rows[i].Field<string>("xapSfeAddPartSelect");
				eRPProductionPropertyInformationDto.xapSfeAsmSearchFields = dataTable.Rows[i].Field<string>("xapSfeAsmSearchFields");
				eRPProductionPropertyInformationDto.xapSfeEndJobCompletionCode = dataTable.Rows[i].Field<decimal>("xapSfeEndJobCompletionCode");
				eRPProductionPropertyInformationDto.xapSfeEndJobGoodQty = dataTable.Rows[i].Field<string>("xapSfeEndJobGoodQty");
				eRPProductionPropertyInformationDto.xapSfeEndJobScrapQty = dataTable.Rows[i].Field<string>("xapSfeEndJobScrapQty");
				eRPProductionPropertyInformationDto.xapSfeIssueMaterialQty = dataTable.Rows[i].Field<string>("xapSfeIssueMaterialQty");
				eRPProductionPropertyInformationDto.xapSfeJobSearchSelect = dataTable.Rows[i].Field<string>("xapSfeJobSearchSelect");
				eRPProductionPropertyInformationDto.xapSfeJobTraveller = dataTable.Rows[i].Field<string>("xapSfeJobTraveller");
				eRPProductionPropertyInformationDto.xapSfeOprSearchFields = dataTable.Rows[i].Field<string>("xapSfeOprSearchFields");
				eRPProductionPropertyInformationDto.xapSfeSetupPercentage = dataTable.Rows[i].Field<string>("xapSfeSetupPercentage");
				eRPProductionPropertyInformationDto.xapSfeStartJobWorkCode = dataTable.Rows[i].Field<decimal>("xapSfeStartJobWorkCode");
				eRPProductionPropertyInformationDto.xapSfeTCAuditReport = dataTable.Rows[i].Field<string>("xapSfeTCAuditReport");
				eRPProductionPropertyInformationDto.xapSfeWorkQueueFields = dataTable.Rows[i].Field<string>("xapSfeWorkQueueFields");
				eRPProductionPropertyInformationDto.xapSfeWorkQueueSort = dataTable.Rows[i].Field<string>("xapSfeWorkQueueSort");
				eRPProductionPropertyInformationDto.xapShowQtyOnHandMobInv = dataTable.Rows[i].Field<bool>("xapShowQtyOnHandMobInv");
				eRPProductionPropertyInformationDto.xapSmEdi856CustomLabel = dataTable.Rows[i].Field<string>("xapSmEdi856CustomLabel");
				eRPProductionPropertyInformationDto.xapSmLineQuantityValidation = dataTable.Rows[i].Field<byte>("xapSmLineQuantityValidation");
				eRPProductionPropertyInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProductionPropertyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProductionPropertyInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProductionPropertyInformationDto> GetProductionProperty(Guid productionPropertyId)
	{
		ERPProductionPropertyInformationDto eRPProductionPropertyInformationDto = new ERPProductionPropertyInformationDto();
		InitializeParameterLists();
		string[] collection = new string[227]
		{
			"xapAllowNegQtyOnHandHistory", "xapAnonymousCustomerID", "xapChChangeRequestTypeID", "xapCmArPaymentCreditMessage", "xapCmArPaymentHoldMessage", "xapCmFieldServiceCreditMessage", "xapCmFieldServiceHoldMessage", "xapCmNonTaxReasonID", "xapCmOrderCreditMessage", "xapCmOrderHoldMessage",
			"xapCmSalesPersonDefaultLoc", "xapCmShipmentCreditMessage", "xapCmShipmentHoldMessage", "xapCostingMethodHistory", "xapCreatedBy", "xapCreatedDate", "xapDateToSchedule", "xapDcAutoClockOutComputer", "xapDcIdleTimeThreshhold", "xapDcLaborCalculationMethod",
			"xapDcPayCalculationMethod", "xapDcRefreshInterval", "xapDcSfeInspectionPassword", "xapDcSfeShutdownPassword", "xapDcTimeFormat", "xapDMDefaultFolder", "xapEdi810ServiceUrl", "xapEdi856ServiceUrl", "xapEdiPassword", "xapEdiUserName",
			"xapUniqueID", "xapHdAttachmentFilePath", "xapHdcallDueDateDays", "xapHdcallTypeID", "xapHdcontactMethodID", "xapHdMailMergeCallTypeID", "xapHdMailMergeContactMethodID", "xapHdNewCallSoundFile", "xapHdSalesCallTypeID", "xapImAutoCreateRevisionID",
			"xapImCostingMethod", "xapImMfgDefaultCostType", "xapCmCreateJobOnly", "xapCmCreditLimitSourceInv", "xapCmCreditLimitSourceOrder", "xapCmCreditLimitSourceShip", "xapCmCustomerTaxable", "xapCmEnableResellers", "xapCmIncludeFreightInPrice", "xapDcAllowNegativeQty",
			"xapDcAllowProductionComplete", "xapDcAutoClockOutLocked", "xapDcEnableCreateSequence", "xapDcEnableIssueMaterial", "xapDcEnableJobTraveler", "xapDcEnableMinimizeButtonInSfe", "xapDcEnableTimecardAudit", "xapDcEnableWorkQueue", "xapDcPromptForActivityPassword", "xapDcPromptForAuditPassword",
			"xapDcPromptForClockInPassword", "xapDcPromptForLaborDescription", "xapDcPromptForMessagePassword", "xapDcPromptForReason", "xapDcShowCurrentJobsOnly", "xapDcSplitDirectLaborHours", "xapDcSplitIndirectLaborHours", "xapDcUseServerTime", "xapDcWarnOnOutsideOperation", "xapDcWarnOnOverProduction",
			"xapGlCreateStockJournals", "xapHdcreateCallForEmails", "xapImAllowNegativeQtyOnHand", "xapImAutoCreateRevision", "xapImCopyAlternates", "xapImCopyPartMemos", "xapImCopyPartOrgReferences", "xapImCopyPartPrices", "xapImCopyPartRules", "xapImEnableOrgPartCustomer",
			"xapImEnableOrgPartSupplier", "xapImEnableWarningWhenNegative", "xapImForceConfiguratorScreens", "xapImHideUseMethodInTree", "xapImIgnoreLCInStdCostRollup", "xapImOnlyAllowExistingBins", "xapImOverwriteDescription", "xapImOverwriteDocuments", "xapImOverwriteMethod", "xapImRefreshMaterial",
			"xapImRefreshMaterialCosts", "xapImScrapRoundUp", "xapImSetUseMethod", "xapImTransferCustomer", "xapImTransferDescriptions", "xapImTransferMaterial", "xapImUseStdForStdCostRollUp", "xapJmExcessQuantity", "xapJmIgnoreEmployees", "xapJmIgnoreMachines",
			"xapJmLoadLevelFinite", "xapJmMinimizeGaps", "xapJmMRPForecastFirmJob", "xapJmOverwriteDescription", "xapJmOverwriteDocuments", "xapJmOverwriteMethod", "xapJmRefreshHours", "xapJmRefreshMaterial", "xapJmRefreshMaterialCosts", "xapJmScheduleShowActualTimes",
			"xapJmScheduleUseActuals", "xapJmShopLoadShowFutureLoad", "xapJmShopLoadShowPastLoad", "xapLmUpdateActualWithRounded", "xapNextSerialNumberPerGroup", "xapOmAutoCreateDelivery", "xapOmEnableDiscountFields", "xapOmEnableFreightFields", "xapOmIncludeOrderDeliveryInJob", "xapOmIncludeOrderLineInJob",
			"xapOmMarkCreateJobForMto", "xapOmMarkPullQuoteMethodForMto", "xapOmShowDeliveriesInTree", "xapOmUseQuotingMarkupTM", "xapPmPTOUsesDeliveryCost", "xapPmPurPlannerIncWhsQties", "xapPmPurPlannerUseBestPrice", "xapPmShowFirmOnlyPoWiz", "xapPoWizardShowQtyToInspect", "xapPRUseFirmQuotesOnly",
			"xapQArmaRequiresInspection", "xapQAShowRmaOtherInfo", "xapQmMultipleQuantities", "xapQmMUseDefHeaderFooterText", "xapQmOverwriteDescription", "xapQmOverwriteDocuments", "xapQmOverwriteMethod", "xapQmRefreshMaterial", "xapQmRefreshMaterialCosts", "xapQmRefreshRateInfo",
			"xapRQGroupPobyRfq", "xapRQIncludeAlternateParts", "xapSfeAllowSuspend", "xapSfeBarcodeScanner", "xapSfeTouchScreen", "xapSmDeleteZeroShipmentLines", "xapJmCalendarExportFields", "xapJmInitialExtension", "xapJmInsideInspectionLineRTF", "xapJmInsideInspectionLineText",
			"xapJmJobMaterialSource", "xapJmLoadReliefMethod", "xapJmOutsideInspectionLineRTF", "xapJmOutsideInspectionLineText", "xapJmScheduleBoardFields", "xapJmScheduleType", "xapJmShopLoadBuckets", "xapJmShopLoadDays", "xapJmShopLoadDepartmentID", "xapJmShopLoadFields",
			"xapJmShopLoadPlantID", "xapJmShopLoadTimeType", "xapJmSplitCosts", "xapJmStandardFactor", "xapLmCalculateEndTime", "xapLmLeaveBoardFields", "xapLOResponseMethodID", "xapNextSerialNumberIDFormula", "xapOmAddlChargePartID", "xapOmAddlChargePartRevisionID",
			"xapOmDeliveryType", "xapOmFreeOnBoardDescription", "xapOmLineQuantityValidation", "xapOmOrderDeliveryDigits", "xapOmOrderLineDigits", "xapOmSalesGlAccountID", "xapOmUnitOfMeasure", "xapPACalendarExportFields", "xapPAExportFormat", "xapPAExportLocation",
			"xapPmCostingMethod", "xapPmDefaultDueDate", "xapPmFollowUpDays", "xapPmPoWizardDisplayType", "xapPmPurchaseType", "xapPmTaxExemptNumber", "xapPRLaborMethod", "xapQAInspQueueRefreshInterval", "xapQmAdditionalChargeText", "xapQmExpirationDays",
			"xapQmFollowUpDays", "xapQmFollowUpType", "xapQmLaborMarkup", "xapQmMaterialMarkup", "xapQmMQuoteFooterMessageRTF", "xapQmMQuoteFooterMessageText", "xapQmMQuoteHeaderMessageRTF", "xapQmMQuoteHeaderMessageText", "xapQmOverheadMarkup", "xapQmPurchaseToOrderMarkup",
			"xapQmQuoteFooterMessageRTF", "xapQmQuoteFooterMessageText", "xapQmQuoteHeaderMessageRTF", "xapQmQuoteHeaderMessageText", "xapQmQuoteMarkupType", "xapQmQuotingMarkup", "xapQmQuotingMethod", "xapQmSubcontractMarkup", "xapRowVersion", "xapSfeActiveJobQueueFields",
			"xapSfeAddPartSelect", "xapSfeAsmSearchFields", "xapSfeEndJobCompletionCode", "xapSfeEndJobGoodQty", "xapSfeEndJobScrapQty", "xapSfeIssueMaterialQty", "xapSfeJobSearchSelect", "xapSfeJobTraveller", "xapSfeOprSearchFields", "xapSfeSetupPercentage",
			"xapSfeStartJobWorkCode", "xapSfeTCAuditReport", "xapSfeWorkQueueFields", "xapSfeWorkQueueSort", "xapShowQtyOnHandMobInv", "xapSmEdi856CustomLabel", "xapSmLineQuantityValidation"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xapUniqueID|C", productionPropertyId);
		AddCustomFieldsToSelectList("ProductionProperties");
		using (DataTable dataTable = GetAsDataTable("ProductionProperties", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProductionPropertyInformationDto);
			}
			eRPProductionPropertyInformationDto.xapAllowNegQtyOnHandHistory = dataTable.Rows[0].Field<string>("xapAllowNegQtyOnHandHistory");
			eRPProductionPropertyInformationDto.xapAnonymousCustomerID = dataTable.Rows[0].Field<string>("xapAnonymousCustomerID");
			eRPProductionPropertyInformationDto.xapChChangeRequestTypeID = dataTable.Rows[0].Field<string>("xapChChangeRequestTypeID");
			eRPProductionPropertyInformationDto.xapCmArPaymentCreditMessage = dataTable.Rows[0].Field<byte>("xapCmArPaymentCreditMessage");
			eRPProductionPropertyInformationDto.xapCmArPaymentHoldMessage = dataTable.Rows[0].Field<byte>("xapCmArPaymentHoldMessage");
			eRPProductionPropertyInformationDto.xapCmFieldServiceCreditMessage = dataTable.Rows[0].Field<byte>("xapCmFieldServiceCreditMessage");
			eRPProductionPropertyInformationDto.xapCmFieldServiceHoldMessage = dataTable.Rows[0].Field<byte>("xapCmFieldServiceHoldMessage");
			eRPProductionPropertyInformationDto.xapCmNonTaxReasonID = dataTable.Rows[0].Field<string>("xapCmNonTaxReasonID");
			eRPProductionPropertyInformationDto.xapCmOrderCreditMessage = dataTable.Rows[0].Field<byte>("xapCmOrderCreditMessage");
			eRPProductionPropertyInformationDto.xapCmOrderHoldMessage = dataTable.Rows[0].Field<byte>("xapCmOrderHoldMessage");
			eRPProductionPropertyInformationDto.xapCmSalesPersonDefaultLoc = dataTable.Rows[0].Field<byte>("xapCmSalesPersonDefaultLoc");
			eRPProductionPropertyInformationDto.xapCmShipmentCreditMessage = dataTable.Rows[0].Field<byte>("xapCmShipmentCreditMessage");
			eRPProductionPropertyInformationDto.xapCmShipmentHoldMessage = dataTable.Rows[0].Field<byte>("xapCmShipmentHoldMessage");
			eRPProductionPropertyInformationDto.xapCostingMethodHistory = dataTable.Rows[0].Field<string>("xapCostingMethodHistory");
			eRPProductionPropertyInformationDto.xapCreatedBy = dataTable.Rows[0].Field<string>("xapCreatedBy");
			eRPProductionPropertyInformationDto.xapCreatedDate = dataTable.Rows[0].Field<DateTime?>("xapCreatedDate");
			eRPProductionPropertyInformationDto.xapDateToSchedule = dataTable.Rows[0].Field<DateTime?>("xapDateToSchedule");
			eRPProductionPropertyInformationDto.xapDcAutoClockOutComputer = dataTable.Rows[0].Field<string>("xapDcAutoClockOutComputer");
			eRPProductionPropertyInformationDto.xapDcIdleTimeThreshhold = dataTable.Rows[0].Field<byte>("xapDcIdleTimeThreshhold");
			eRPProductionPropertyInformationDto.xapDcLaborCalculationMethod = dataTable.Rows[0].Field<byte>("xapDcLaborCalculationMethod");
			eRPProductionPropertyInformationDto.xapDcPayCalculationMethod = dataTable.Rows[0].Field<byte>("xapDcPayCalculationMethod");
			eRPProductionPropertyInformationDto.xapDcRefreshInterval = dataTable.Rows[0].Field<byte>("xapDcRefreshInterval");
			eRPProductionPropertyInformationDto.xapDcSfeInspectionPassword = dataTable.Rows[0].Field<string>("xapDcSfeInspectionPassword");
			eRPProductionPropertyInformationDto.xapDcSfeShutdownPassword = dataTable.Rows[0].Field<string>("xapDcSfeShutdownPassword");
			eRPProductionPropertyInformationDto.xapDcTimeFormat = dataTable.Rows[0].Field<byte>("xapDcTimeFormat");
			eRPProductionPropertyInformationDto.xapDMDefaultFolder = dataTable.Rows[0].Field<string>("xapDMDefaultFolder");
			eRPProductionPropertyInformationDto.xapEdi810ServiceUrl = dataTable.Rows[0].Field<string>("xapEdi810ServiceUrl");
			eRPProductionPropertyInformationDto.xapEdi856ServiceUrl = dataTable.Rows[0].Field<string>("xapEdi856ServiceUrl");
			eRPProductionPropertyInformationDto.xapEdiPassword = dataTable.Rows[0].Field<string>("xapEdiPassword");
			eRPProductionPropertyInformationDto.xapEdiUserName = dataTable.Rows[0].Field<string>("xapEdiUserName");
			eRPProductionPropertyInformationDto.xapUniqueID = dataTable.Rows[0].Field<Guid>("xapUniqueID");
			eRPProductionPropertyInformationDto.xapHdAttachmentFilePath = dataTable.Rows[0].Field<string>("xapHdAttachmentFilePath");
			eRPProductionPropertyInformationDto.xapHdcallDueDateDays = dataTable.Rows[0].Field<short>("xapHdcallDueDateDays");
			eRPProductionPropertyInformationDto.xapHdcallTypeID = dataTable.Rows[0].Field<string>("xapHdcallTypeID");
			eRPProductionPropertyInformationDto.xapHdcontactMethodID = dataTable.Rows[0].Field<string>("xapHdcontactMethodID");
			eRPProductionPropertyInformationDto.xapHdMailMergeCallTypeID = dataTable.Rows[0].Field<string>("xapHdMailMergeCallTypeID");
			eRPProductionPropertyInformationDto.xapHdMailMergeContactMethodID = dataTable.Rows[0].Field<string>("xapHdMailMergeContactMethodID");
			eRPProductionPropertyInformationDto.xapHdNewCallSoundFile = dataTable.Rows[0].Field<string>("xapHdNewCallSoundFile");
			eRPProductionPropertyInformationDto.xapHdSalesCallTypeID = dataTable.Rows[0].Field<string>("xapHdSalesCallTypeID");
			eRPProductionPropertyInformationDto.xapImAutoCreateRevisionID = dataTable.Rows[0].Field<string>("xapImAutoCreateRevisionID");
			eRPProductionPropertyInformationDto.xapImCostingMethod = dataTable.Rows[0].Field<byte>("xapImCostingMethod");
			eRPProductionPropertyInformationDto.xapImMfgDefaultCostType = dataTable.Rows[0].Field<byte>("xapImMfgDefaultCostType");
			eRPProductionPropertyInformationDto.xapCmCreateJobOnly = dataTable.Rows[0].Field<bool>("xapCmCreateJobOnly");
			eRPProductionPropertyInformationDto.xapCmCreditLimitSourceInv = dataTable.Rows[0].Field<bool>("xapCmCreditLimitSourceInv");
			eRPProductionPropertyInformationDto.xapCmCreditLimitSourceOrder = dataTable.Rows[0].Field<bool>("xapCmCreditLimitSourceOrder");
			eRPProductionPropertyInformationDto.xapCmCreditLimitSourceShip = dataTable.Rows[0].Field<bool>("xapCmCreditLimitSourceShip");
			eRPProductionPropertyInformationDto.xapCmCustomerTaxable = dataTable.Rows[0].Field<bool>("xapCmCustomerTaxable");
			eRPProductionPropertyInformationDto.xapCmEnableResellers = dataTable.Rows[0].Field<bool>("xapCmEnableResellers");
			eRPProductionPropertyInformationDto.xapCmIncludeFreightInPrice = dataTable.Rows[0].Field<bool>("xapCmIncludeFreightInPrice");
			eRPProductionPropertyInformationDto.xapDcAllowNegativeQty = dataTable.Rows[0].Field<bool>("xapDcAllowNegativeQty");
			eRPProductionPropertyInformationDto.xapDcAllowProductionComplete = dataTable.Rows[0].Field<bool>("xapDcAllowProductionComplete");
			eRPProductionPropertyInformationDto.xapDcAutoClockOutLocked = dataTable.Rows[0].Field<bool>("xapDcAutoClockOutLocked");
			eRPProductionPropertyInformationDto.xapDcEnableCreateSequence = dataTable.Rows[0].Field<bool>("xapDcEnableCreateSequence");
			eRPProductionPropertyInformationDto.xapDcEnableIssueMaterial = dataTable.Rows[0].Field<bool>("xapDcEnableIssueMaterial");
			eRPProductionPropertyInformationDto.xapDcEnableJobTraveler = dataTable.Rows[0].Field<bool>("xapDcEnableJobTraveler");
			eRPProductionPropertyInformationDto.xapDcEnableMinimizeButtonInSfe = dataTable.Rows[0].Field<bool>("xapDcEnableMinimizeButtonInSfe");
			eRPProductionPropertyInformationDto.xapDcEnableTimecardAudit = dataTable.Rows[0].Field<bool>("xapDcEnableTimecardAudit");
			eRPProductionPropertyInformationDto.xapDcEnableWorkQueue = dataTable.Rows[0].Field<bool>("xapDcEnableWorkQueue");
			eRPProductionPropertyInformationDto.xapDcPromptForActivityPassword = dataTable.Rows[0].Field<bool>("xapDcPromptForActivityPassword");
			eRPProductionPropertyInformationDto.xapDcPromptForAuditPassword = dataTable.Rows[0].Field<bool>("xapDcPromptForAuditPassword");
			eRPProductionPropertyInformationDto.xapDcPromptForClockInPassword = dataTable.Rows[0].Field<bool>("xapDcPromptForClockInPassword");
			eRPProductionPropertyInformationDto.xapDcPromptForLaborDescription = dataTable.Rows[0].Field<bool>("xapDcPromptForLaborDescription");
			eRPProductionPropertyInformationDto.xapDcPromptForMessagePassword = dataTable.Rows[0].Field<bool>("xapDcPromptForMessagePassword");
			eRPProductionPropertyInformationDto.xapDcPromptForReason = dataTable.Rows[0].Field<bool>("xapDcPromptForReason");
			eRPProductionPropertyInformationDto.xapDcShowCurrentJobsOnly = dataTable.Rows[0].Field<bool>("xapDcShowCurrentJobsOnly");
			eRPProductionPropertyInformationDto.xapDcSplitDirectLaborHours = dataTable.Rows[0].Field<bool>("xapDcSplitDirectLaborHours");
			eRPProductionPropertyInformationDto.xapDcSplitIndirectLaborHours = dataTable.Rows[0].Field<bool>("xapDcSplitIndirectLaborHours");
			eRPProductionPropertyInformationDto.xapDcUseServerTime = dataTable.Rows[0].Field<bool>("xapDcUseServerTime");
			eRPProductionPropertyInformationDto.xapDcWarnOnOutsideOperation = dataTable.Rows[0].Field<bool>("xapDcWarnOnOutsideOperation");
			eRPProductionPropertyInformationDto.xapDcWarnOnOverProduction = dataTable.Rows[0].Field<bool>("xapDcWarnOnOverProduction");
			eRPProductionPropertyInformationDto.xapGlCreateStockJournals = dataTable.Rows[0].Field<bool>("xapGlCreateStockJournals");
			eRPProductionPropertyInformationDto.xapHdcreateCallForEmails = dataTable.Rows[0].Field<bool>("xapHdcreateCallForEmails");
			eRPProductionPropertyInformationDto.xapImAllowNegativeQtyOnHand = dataTable.Rows[0].Field<bool>("xapImAllowNegativeQtyOnHand");
			eRPProductionPropertyInformationDto.xapImAutoCreateRevision = dataTable.Rows[0].Field<bool>("xapImAutoCreateRevision");
			eRPProductionPropertyInformationDto.xapImCopyAlternates = dataTable.Rows[0].Field<bool>("xapImCopyAlternates");
			eRPProductionPropertyInformationDto.xapImCopyPartMemos = dataTable.Rows[0].Field<bool>("xapImCopyPartMemos");
			eRPProductionPropertyInformationDto.xapImCopyPartOrgReferences = dataTable.Rows[0].Field<bool>("xapImCopyPartOrgReferences");
			eRPProductionPropertyInformationDto.xapImCopyPartPrices = dataTable.Rows[0].Field<bool>("xapImCopyPartPrices");
			eRPProductionPropertyInformationDto.xapImCopyPartRules = dataTable.Rows[0].Field<bool>("xapImCopyPartRules");
			eRPProductionPropertyInformationDto.xapImEnableOrgPartCustomer = dataTable.Rows[0].Field<bool>("xapImEnableOrgPartCustomer");
			eRPProductionPropertyInformationDto.xapImEnableOrgPartSupplier = dataTable.Rows[0].Field<bool>("xapImEnableOrgPartSupplier");
			eRPProductionPropertyInformationDto.xapImEnableWarningWhenNegative = dataTable.Rows[0].Field<bool>("xapImEnableWarningWhenNegative");
			eRPProductionPropertyInformationDto.xapImForceConfiguratorScreens = dataTable.Rows[0].Field<bool>("xapImForceConfiguratorScreens");
			eRPProductionPropertyInformationDto.xapImHideUseMethodInTree = dataTable.Rows[0].Field<bool>("xapImHideUseMethodInTree");
			eRPProductionPropertyInformationDto.xapImIgnoreLCInStdCostRollup = dataTable.Rows[0].Field<bool>("xapImIgnoreLCInStdCostRollup");
			eRPProductionPropertyInformationDto.xapImOnlyAllowExistingBins = dataTable.Rows[0].Field<bool>("xapImOnlyAllowExistingBins");
			eRPProductionPropertyInformationDto.xapImOverwriteDescription = dataTable.Rows[0].Field<bool>("xapImOverwriteDescription");
			eRPProductionPropertyInformationDto.xapImOverwriteDocuments = dataTable.Rows[0].Field<bool>("xapImOverwriteDocuments");
			eRPProductionPropertyInformationDto.xapImOverwriteMethod = dataTable.Rows[0].Field<bool>("xapImOverwriteMethod");
			eRPProductionPropertyInformationDto.xapImRefreshMaterial = dataTable.Rows[0].Field<bool>("xapImRefreshMaterial");
			eRPProductionPropertyInformationDto.xapImRefreshMaterialCosts = dataTable.Rows[0].Field<bool>("xapImRefreshMaterialCosts");
			eRPProductionPropertyInformationDto.xapImScrapRoundUp = dataTable.Rows[0].Field<bool>("xapImScrapRoundUp");
			eRPProductionPropertyInformationDto.xapImSetUseMethod = dataTable.Rows[0].Field<bool>("xapImSetUseMethod");
			eRPProductionPropertyInformationDto.xapImTransferCustomer = dataTable.Rows[0].Field<bool>("xapImTransferCustomer");
			eRPProductionPropertyInformationDto.xapImTransferDescriptions = dataTable.Rows[0].Field<bool>("xapImTransferDescriptions");
			eRPProductionPropertyInformationDto.xapImTransferMaterial = dataTable.Rows[0].Field<bool>("xapImTransferMaterial");
			eRPProductionPropertyInformationDto.xapImUseStdForStdCostRollUp = dataTable.Rows[0].Field<bool>("xapImUseStdForStdCostRollUp");
			eRPProductionPropertyInformationDto.xapJmExcessQuantity = dataTable.Rows[0].Field<bool>("xapJmExcessQuantity");
			eRPProductionPropertyInformationDto.xapJmIgnoreEmployees = dataTable.Rows[0].Field<bool>("xapJmIgnoreEmployees");
			eRPProductionPropertyInformationDto.xapJmIgnoreMachines = dataTable.Rows[0].Field<bool>("xapJmIgnoreMachines");
			eRPProductionPropertyInformationDto.xapJmLoadLevelFinite = dataTable.Rows[0].Field<bool>("xapJmLoadLevelFinite");
			eRPProductionPropertyInformationDto.xapJmMinimizeGaps = dataTable.Rows[0].Field<bool>("xapJmMinimizeGaps");
			eRPProductionPropertyInformationDto.xapJmMRPForecastFirmJob = dataTable.Rows[0].Field<bool>("xapJmMRPForecastFirmJob");
			eRPProductionPropertyInformationDto.xapJmOverwriteDescription = dataTable.Rows[0].Field<bool>("xapJmOverwriteDescription");
			eRPProductionPropertyInformationDto.xapJmOverwriteDocuments = dataTable.Rows[0].Field<bool>("xapJmOverwriteDocuments");
			eRPProductionPropertyInformationDto.xapJmOverwriteMethod = dataTable.Rows[0].Field<bool>("xapJmOverwriteMethod");
			eRPProductionPropertyInformationDto.xapJmRefreshHours = dataTable.Rows[0].Field<bool>("xapJmRefreshHours");
			eRPProductionPropertyInformationDto.xapJmRefreshMaterial = dataTable.Rows[0].Field<bool>("xapJmRefreshMaterial");
			eRPProductionPropertyInformationDto.xapJmRefreshMaterialCosts = dataTable.Rows[0].Field<bool>("xapJmRefreshMaterialCosts");
			eRPProductionPropertyInformationDto.xapJmScheduleShowActualTimes = dataTable.Rows[0].Field<bool>("xapJmScheduleShowActualTimes");
			eRPProductionPropertyInformationDto.xapJmScheduleUseActuals = dataTable.Rows[0].Field<bool>("xapJmScheduleUseActuals");
			eRPProductionPropertyInformationDto.xapJmShopLoadShowFutureLoad = dataTable.Rows[0].Field<bool>("xapJmShopLoadShowFutureLoad");
			eRPProductionPropertyInformationDto.xapJmShopLoadShowPastLoad = dataTable.Rows[0].Field<bool>("xapJmShopLoadShowPastLoad");
			eRPProductionPropertyInformationDto.xapLmUpdateActualWithRounded = dataTable.Rows[0].Field<bool>("xapLmUpdateActualWithRounded");
			eRPProductionPropertyInformationDto.xapNextSerialNumberPerGroup = dataTable.Rows[0].Field<bool>("xapNextSerialNumberPerGroup");
			eRPProductionPropertyInformationDto.xapOmAutoCreateDelivery = dataTable.Rows[0].Field<bool>("xapOmAutoCreateDelivery");
			eRPProductionPropertyInformationDto.xapOmEnableDiscountFields = dataTable.Rows[0].Field<bool>("xapOmEnableDiscountFields");
			eRPProductionPropertyInformationDto.xapOmEnableFreightFields = dataTable.Rows[0].Field<bool>("xapOmEnableFreightFields");
			eRPProductionPropertyInformationDto.xapOmIncludeOrderDeliveryInJob = dataTable.Rows[0].Field<bool>("xapOmIncludeOrderDeliveryInJob");
			eRPProductionPropertyInformationDto.xapOmIncludeOrderLineInJob = dataTable.Rows[0].Field<bool>("xapOmIncludeOrderLineInJob");
			eRPProductionPropertyInformationDto.xapOmMarkCreateJobForMto = dataTable.Rows[0].Field<bool>("xapOmMarkCreateJobForMto");
			eRPProductionPropertyInformationDto.xapOmMarkPullQuoteMethodForMto = dataTable.Rows[0].Field<bool>("xapOmMarkPullQuoteMethodForMto");
			eRPProductionPropertyInformationDto.xapOmShowDeliveriesInTree = dataTable.Rows[0].Field<bool>("xapOmShowDeliveriesInTree");
			eRPProductionPropertyInformationDto.xapOmUseQuotingMarkupTM = dataTable.Rows[0].Field<bool>("xapOmUseQuotingMarkupTM");
			eRPProductionPropertyInformationDto.xapPmPTOUsesDeliveryCost = dataTable.Rows[0].Field<bool>("xapPmPTOUsesDeliveryCost");
			eRPProductionPropertyInformationDto.xapPmPurPlannerIncWhsQties = dataTable.Rows[0].Field<bool>("xapPmPurPlannerIncWhsQties");
			eRPProductionPropertyInformationDto.xapPmPurPlannerUseBestPrice = dataTable.Rows[0].Field<bool>("xapPmPurPlannerUseBestPrice");
			eRPProductionPropertyInformationDto.xapPmShowFirmOnlyPoWiz = dataTable.Rows[0].Field<bool>("xapPmShowFirmOnlyPoWiz");
			eRPProductionPropertyInformationDto.xapPoWizardShowQtyToInspect = dataTable.Rows[0].Field<bool>("xapPoWizardShowQtyToInspect");
			eRPProductionPropertyInformationDto.xapPRUseFirmQuotesOnly = dataTable.Rows[0].Field<bool>("xapPRUseFirmQuotesOnly");
			eRPProductionPropertyInformationDto.xapQArmaRequiresInspection = dataTable.Rows[0].Field<bool>("xapQArmaRequiresInspection");
			eRPProductionPropertyInformationDto.xapQAShowRmaOtherInfo = dataTable.Rows[0].Field<bool>("xapQAShowRmaOtherInfo");
			eRPProductionPropertyInformationDto.xapQmMultipleQuantities = dataTable.Rows[0].Field<bool>("xapQmMultipleQuantities");
			eRPProductionPropertyInformationDto.xapQmMUseDefHeaderFooterText = dataTable.Rows[0].Field<bool>("xapQmMUseDefHeaderFooterText");
			eRPProductionPropertyInformationDto.xapQmOverwriteDescription = dataTable.Rows[0].Field<bool>("xapQmOverwriteDescription");
			eRPProductionPropertyInformationDto.xapQmOverwriteDocuments = dataTable.Rows[0].Field<bool>("xapQmOverwriteDocuments");
			eRPProductionPropertyInformationDto.xapQmOverwriteMethod = dataTable.Rows[0].Field<bool>("xapQmOverwriteMethod");
			eRPProductionPropertyInformationDto.xapQmRefreshMaterial = dataTable.Rows[0].Field<bool>("xapQmRefreshMaterial");
			eRPProductionPropertyInformationDto.xapQmRefreshMaterialCosts = dataTable.Rows[0].Field<bool>("xapQmRefreshMaterialCosts");
			eRPProductionPropertyInformationDto.xapQmRefreshRateInfo = dataTable.Rows[0].Field<bool>("xapQmRefreshRateInfo");
			eRPProductionPropertyInformationDto.xapRQGroupPobyRfq = dataTable.Rows[0].Field<bool>("xapRQGroupPobyRfq");
			eRPProductionPropertyInformationDto.xapRQIncludeAlternateParts = dataTable.Rows[0].Field<bool>("xapRQIncludeAlternateParts");
			eRPProductionPropertyInformationDto.xapSfeAllowSuspend = dataTable.Rows[0].Field<bool>("xapSfeAllowSuspend");
			eRPProductionPropertyInformationDto.xapSfeBarcodeScanner = dataTable.Rows[0].Field<bool>("xapSfeBarcodeScanner");
			eRPProductionPropertyInformationDto.xapSfeTouchScreen = dataTable.Rows[0].Field<bool>("xapSfeTouchScreen");
			eRPProductionPropertyInformationDto.xapSmDeleteZeroShipmentLines = dataTable.Rows[0].Field<bool>("xapSmDeleteZeroShipmentLines");
			eRPProductionPropertyInformationDto.xapJmCalendarExportFields = dataTable.Rows[0].Field<string>("xapJmCalendarExportFields");
			eRPProductionPropertyInformationDto.xapJmInitialExtension = dataTable.Rows[0].Field<string>("xapJmInitialExtension");
			eRPProductionPropertyInformationDto.xapJmInsideInspectionLineRTF = dataTable.Rows[0].Field<string>("xapJmInsideInspectionLineRTF");
			eRPProductionPropertyInformationDto.xapJmInsideInspectionLineText = dataTable.Rows[0].Field<string>("xapJmInsideInspectionLineText");
			eRPProductionPropertyInformationDto.xapJmJobMaterialSource = dataTable.Rows[0].Field<byte>("xapJmJobMaterialSource");
			eRPProductionPropertyInformationDto.xapJmLoadReliefMethod = dataTable.Rows[0].Field<byte>("xapJmLoadReliefMethod");
			eRPProductionPropertyInformationDto.xapJmOutsideInspectionLineRTF = dataTable.Rows[0].Field<string>("xapJmOutsideInspectionLineRTF");
			eRPProductionPropertyInformationDto.xapJmOutsideInspectionLineText = dataTable.Rows[0].Field<string>("xapJmOutsideInspectionLineText");
			eRPProductionPropertyInformationDto.xapJmScheduleBoardFields = dataTable.Rows[0].Field<string>("xapJmScheduleBoardFields");
			eRPProductionPropertyInformationDto.xapJmScheduleType = dataTable.Rows[0].Field<byte>("xapJmScheduleType");
			eRPProductionPropertyInformationDto.xapJmShopLoadBuckets = dataTable.Rows[0].Field<byte>("xapJmShopLoadBuckets");
			eRPProductionPropertyInformationDto.xapJmShopLoadDays = dataTable.Rows[0].Field<byte>("xapJmShopLoadDays");
			eRPProductionPropertyInformationDto.xapJmShopLoadDepartmentID = dataTable.Rows[0].Field<string>("xapJmShopLoadDepartmentID");
			eRPProductionPropertyInformationDto.xapJmShopLoadFields = dataTable.Rows[0].Field<string>("xapJmShopLoadFields");
			eRPProductionPropertyInformationDto.xapJmShopLoadPlantID = dataTable.Rows[0].Field<string>("xapJmShopLoadPlantID");
			eRPProductionPropertyInformationDto.xapJmShopLoadTimeType = dataTable.Rows[0].Field<string>("xapJmShopLoadTimeType");
			eRPProductionPropertyInformationDto.xapJmSplitCosts = dataTable.Rows[0].Field<byte>("xapJmSplitCosts");
			eRPProductionPropertyInformationDto.xapJmStandardFactor = dataTable.Rows[0].Field<string>("xapJmStandardFactor");
			eRPProductionPropertyInformationDto.xapLmCalculateEndTime = dataTable.Rows[0].Field<byte>("xapLmCalculateEndTime");
			eRPProductionPropertyInformationDto.xapLmLeaveBoardFields = dataTable.Rows[0].Field<string>("xapLmLeaveBoardFields");
			eRPProductionPropertyInformationDto.xapLOResponseMethodID = dataTable.Rows[0].Field<string>("xapLOResponseMethodID");
			eRPProductionPropertyInformationDto.xapNextSerialNumberIDFormula = dataTable.Rows[0].Field<string>("xapNextSerialNumberIDFormula");
			eRPProductionPropertyInformationDto.xapOmAddlChargePartID = dataTable.Rows[0].Field<string>("xapOmAddlChargePartID");
			eRPProductionPropertyInformationDto.xapOmAddlChargePartRevisionID = dataTable.Rows[0].Field<string>("xapOmAddlChargePartRevisionID");
			eRPProductionPropertyInformationDto.xapOmDeliveryType = dataTable.Rows[0].Field<byte>("xapOmDeliveryType");
			eRPProductionPropertyInformationDto.xapOmFreeOnBoardDescription = dataTable.Rows[0].Field<string>("xapOmFreeOnBoardDescription");
			eRPProductionPropertyInformationDto.xapOmLineQuantityValidation = dataTable.Rows[0].Field<byte>("xapOmLineQuantityValidation");
			eRPProductionPropertyInformationDto.xapOmOrderDeliveryDigits = dataTable.Rows[0].Field<byte>("xapOmOrderDeliveryDigits");
			eRPProductionPropertyInformationDto.xapOmOrderLineDigits = dataTable.Rows[0].Field<byte>("xapOmOrderLineDigits");
			eRPProductionPropertyInformationDto.xapOmSalesGlAccountID = dataTable.Rows[0].Field<string>("xapOmSalesGlAccountID");
			eRPProductionPropertyInformationDto.xapOmUnitOfMeasure = dataTable.Rows[0].Field<string>("xapOmUnitOfMeasure");
			eRPProductionPropertyInformationDto.xapPACalendarExportFields = dataTable.Rows[0].Field<string>("xapPACalendarExportFields");
			eRPProductionPropertyInformationDto.xapPAExportFormat = dataTable.Rows[0].Field<string>("xapPAExportFormat");
			eRPProductionPropertyInformationDto.xapPAExportLocation = dataTable.Rows[0].Field<string>("xapPAExportLocation");
			eRPProductionPropertyInformationDto.xapPmCostingMethod = dataTable.Rows[0].Field<byte>("xapPmCostingMethod");
			eRPProductionPropertyInformationDto.xapPmDefaultDueDate = dataTable.Rows[0].Field<DateTime?>("xapPmDefaultDueDate");
			eRPProductionPropertyInformationDto.xapPmFollowUpDays = dataTable.Rows[0].Field<short>("xapPmFollowUpDays");
			eRPProductionPropertyInformationDto.xapPmPoWizardDisplayType = dataTable.Rows[0].Field<byte>("xapPmPoWizardDisplayType");
			eRPProductionPropertyInformationDto.xapPmPurchaseType = dataTable.Rows[0].Field<byte>("xapPmPurchaseType");
			eRPProductionPropertyInformationDto.xapPmTaxExemptNumber = dataTable.Rows[0].Field<string>("xapPmTaxExemptNumber");
			eRPProductionPropertyInformationDto.xapPRLaborMethod = dataTable.Rows[0].Field<byte>("xapPRLaborMethod");
			eRPProductionPropertyInformationDto.xapQAInspQueueRefreshInterval = dataTable.Rows[0].Field<short>("xapQAInspQueueRefreshInterval");
			eRPProductionPropertyInformationDto.xapQmAdditionalChargeText = dataTable.Rows[0].Field<string>("xapQmAdditionalChargeText");
			eRPProductionPropertyInformationDto.xapQmExpirationDays = dataTable.Rows[0].Field<short>("xapQmExpirationDays");
			eRPProductionPropertyInformationDto.xapQmFollowUpDays = dataTable.Rows[0].Field<short>("xapQmFollowUpDays");
			eRPProductionPropertyInformationDto.xapQmFollowUpType = dataTable.Rows[0].Field<byte>("xapQmFollowUpType");
			eRPProductionPropertyInformationDto.xapQmLaborMarkup = dataTable.Rows[0].Field<decimal>("xapQmLaborMarkup");
			eRPProductionPropertyInformationDto.xapQmMaterialMarkup = dataTable.Rows[0].Field<decimal>("xapQmMaterialMarkup");
			eRPProductionPropertyInformationDto.xapQmMQuoteFooterMessageRTF = dataTable.Rows[0].Field<string>("xapQmMQuoteFooterMessageRTF");
			eRPProductionPropertyInformationDto.xapQmMQuoteFooterMessageText = dataTable.Rows[0].Field<string>("xapQmMQuoteFooterMessageText");
			eRPProductionPropertyInformationDto.xapQmMQuoteHeaderMessageRTF = dataTable.Rows[0].Field<string>("xapQmMQuoteHeaderMessageRTF");
			eRPProductionPropertyInformationDto.xapQmMQuoteHeaderMessageText = dataTable.Rows[0].Field<string>("xapQmMQuoteHeaderMessageText");
			eRPProductionPropertyInformationDto.xapQmOverheadMarkup = dataTable.Rows[0].Field<decimal>("xapQmOverheadMarkup");
			eRPProductionPropertyInformationDto.xapQmPurchaseToOrderMarkup = dataTable.Rows[0].Field<decimal>("xapQmPurchaseToOrderMarkup");
			eRPProductionPropertyInformationDto.xapQmQuoteFooterMessageRTF = dataTable.Rows[0].Field<string>("xapQmQuoteFooterMessageRTF");
			eRPProductionPropertyInformationDto.xapQmQuoteFooterMessageText = dataTable.Rows[0].Field<string>("xapQmQuoteFooterMessageText");
			eRPProductionPropertyInformationDto.xapQmQuoteHeaderMessageRTF = dataTable.Rows[0].Field<string>("xapQmQuoteHeaderMessageRTF");
			eRPProductionPropertyInformationDto.xapQmQuoteHeaderMessageText = dataTable.Rows[0].Field<string>("xapQmQuoteHeaderMessageText");
			eRPProductionPropertyInformationDto.xapQmQuoteMarkupType = dataTable.Rows[0].Field<byte>("xapQmQuoteMarkupType");
			eRPProductionPropertyInformationDto.xapQmQuotingMarkup = dataTable.Rows[0].Field<decimal>("xapQmQuotingMarkup");
			eRPProductionPropertyInformationDto.xapQmQuotingMethod = dataTable.Rows[0].Field<byte>("xapQmQuotingMethod");
			eRPProductionPropertyInformationDto.xapQmSubcontractMarkup = dataTable.Rows[0].Field<decimal>("xapQmSubcontractMarkup");
			eRPProductionPropertyInformationDto.xapRowVersion = dataTable.Rows[0].Field<byte[]>("xapRowVersion");
			eRPProductionPropertyInformationDto.xapSfeActiveJobQueueFields = dataTable.Rows[0].Field<string>("xapSfeActiveJobQueueFields");
			eRPProductionPropertyInformationDto.xapSfeAddPartSelect = dataTable.Rows[0].Field<string>("xapSfeAddPartSelect");
			eRPProductionPropertyInformationDto.xapSfeAsmSearchFields = dataTable.Rows[0].Field<string>("xapSfeAsmSearchFields");
			eRPProductionPropertyInformationDto.xapSfeEndJobCompletionCode = dataTable.Rows[0].Field<decimal>("xapSfeEndJobCompletionCode");
			eRPProductionPropertyInformationDto.xapSfeEndJobGoodQty = dataTable.Rows[0].Field<string>("xapSfeEndJobGoodQty");
			eRPProductionPropertyInformationDto.xapSfeEndJobScrapQty = dataTable.Rows[0].Field<string>("xapSfeEndJobScrapQty");
			eRPProductionPropertyInformationDto.xapSfeIssueMaterialQty = dataTable.Rows[0].Field<string>("xapSfeIssueMaterialQty");
			eRPProductionPropertyInformationDto.xapSfeJobSearchSelect = dataTable.Rows[0].Field<string>("xapSfeJobSearchSelect");
			eRPProductionPropertyInformationDto.xapSfeJobTraveller = dataTable.Rows[0].Field<string>("xapSfeJobTraveller");
			eRPProductionPropertyInformationDto.xapSfeOprSearchFields = dataTable.Rows[0].Field<string>("xapSfeOprSearchFields");
			eRPProductionPropertyInformationDto.xapSfeSetupPercentage = dataTable.Rows[0].Field<string>("xapSfeSetupPercentage");
			eRPProductionPropertyInformationDto.xapSfeStartJobWorkCode = dataTable.Rows[0].Field<decimal>("xapSfeStartJobWorkCode");
			eRPProductionPropertyInformationDto.xapSfeTCAuditReport = dataTable.Rows[0].Field<string>("xapSfeTCAuditReport");
			eRPProductionPropertyInformationDto.xapSfeWorkQueueFields = dataTable.Rows[0].Field<string>("xapSfeWorkQueueFields");
			eRPProductionPropertyInformationDto.xapSfeWorkQueueSort = dataTable.Rows[0].Field<string>("xapSfeWorkQueueSort");
			eRPProductionPropertyInformationDto.xapShowQtyOnHandMobInv = dataTable.Rows[0].Field<bool>("xapShowQtyOnHandMobInv");
			eRPProductionPropertyInformationDto.xapSmEdi856CustomLabel = dataTable.Rows[0].Field<string>("xapSmEdi856CustomLabel");
			eRPProductionPropertyInformationDto.xapSmLineQuantityValidation = dataTable.Rows[0].Field<byte>("xapSmLineQuantityValidation");
			eRPProductionPropertyInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProductionPropertyInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProductionPropertyInformationDto);
	}
}
