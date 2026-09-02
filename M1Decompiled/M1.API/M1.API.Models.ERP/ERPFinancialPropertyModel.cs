using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPFinancialPropertyModel : ERPBaseModel, IERPFinancialPropertyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllFinancialProperties(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPFinancialPropertyRepository iERPFinancialPropertyRepository = (base.ERPFinancialPropertyRepository = new ERPFinancialPropertyRepository(base.ApiClientContext));
		using (iERPFinancialPropertyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPFinancialPropertyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPFinancialPropertyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPFinancialPropertyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPFinancialPropertyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetFinancialProperty(Guid financialPropertyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFinancialPropertyRepository iERPFinancialPropertyRepository = (base.ERPFinancialPropertyRepository = new ERPFinancialPropertyRepository(base.ApiClientContext));
		using (iERPFinancialPropertyRepository)
		{
			if (!(await base.ERPFinancialPropertyRepository.DoesFinancialPropertyExist(financialPropertyId)))
			{
				errorsList.Add($"FinancialProperty [{financialPropertyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPFinancialPropertyDto>>> Process_GetAllFinancialProperties(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPFinancialPropertyDto> allFinancialPropertiesDto = new List<ERPFinancialPropertyDto>();
		ERPResponseMessageDto<IList<ERPFinancialPropertyDto>> result;
		try
		{
			IERPFinancialPropertyRepository iERPFinancialPropertyRepository = (base.ERPFinancialPropertyRepository = new ERPFinancialPropertyRepository(base.ApiClientContext));
			using (iERPFinancialPropertyRepository)
			{
				foreach (ERPFinancialPropertyInformationDto item2 in await base.ERPFinancialPropertyRepository.GetAllFinancialProperties(pageSize, pageNumber, filter, orderBy))
				{
					ERPFinancialPropertyDto item = new ERPFinancialPropertyDto
					{
						xafAccruedCreditorsGlAccountID = item2.xafAccruedCreditorsGlAccountID,
						xafAgingMethod = item2.xafAgingMethod,
						xafApAgingBucketID = item2.xafApAgingBucketID,
						xafApApCostStartDate = item2.xafApApCostStartDate,
						xafApApGlAccountID = item2.xafApApGlAccountID,
						xafApCashGlAccountID = item2.xafApCashGlAccountID,
						xafApDiscountGlAccountID = item2.xafApDiscountGlAccountID,
						xafApFreightGlAccountID = item2.xafApFreightGlAccountID,
						xafApGroupReceiptsBySupplier = item2.xafApGroupReceiptsBySupplier,
						xafApPaymentMaxLinesPerPage = item2.xafApPaymentMaxLinesPerPage,
						xafArAgingBucketID = item2.xafArAgingBucketID,
						xafArArGlAccountID = item2.xafArArGlAccountID,
						xafArCashGlAccountID = item2.xafArCashGlAccountID,
						xafArDefaultLaborPartGroupID = item2.xafArDefaultLaborPartGroupID,
						xafArDepositGlAccountID = item2.xafArDepositGlAccountID,
						xafArDepositPartID = item2.xafArDepositPartID,
						xafArDepositPartRevisionID = item2.xafArDepositPartRevisionID,
						xafArDiscountGlAccountID = item2.xafArDiscountGlAccountID,
						xafArFinanceChargeGlAccountID = item2.xafArFinanceChargeGlAccountID,
						xafArFinanceChargeGraceDays = item2.xafArFinanceChargeGraceDays,
						xafArFinanceChargeLastRunDate = item2.xafArFinanceChargeLastRunDate,
						xafArFinanceChargePercent = item2.xafArFinanceChargePercent,
						xafArFinanceShowCreditBalance = item2.xafArFinanceShowCreditBalance,
						xafArFreightGlAccountID = item2.xafArFreightGlAccountID,
						xafArGroupShipmentsByCustomer = item2.xafArGroupShipmentsByCustomer,
						xafArLaborPartID = item2.xafArLaborPartID,
						xafArLaborPartRevisionID = item2.xafArLaborPartRevisionID,
						xafArNET1GatewayID = item2.xafArNET1GatewayID,
						xafArNET1MerchantKey = item2.xafArNET1MerchantKey,
						xafArNET1Port = item2.xafArNET1Port,
						xafArNET1TimeoutSeconds = item2.xafArNET1TimeoutSeconds,
						xafArShowDeposits = item2.xafArShowDeposits,
						xafAvalaraAccountID = item2.xafAvalaraAccountID,
						xafAvalaraArInvoicePostOption = item2.xafAvalaraArInvoicePostOption,
						xafAvalaraCanadaGstTaxCodeID = item2.xafAvalaraCanadaGstTaxCodeID,
						xafAvalaraCanadaHSTTaxCodeID = item2.xafAvalaraCanadaHSTTaxCodeID,
						xafAvalaraCanadaPSTTaxCodeID = item2.xafAvalaraCanadaPSTTaxCodeID,
						xafAvalaraCanadaQSTTaxCodeID = item2.xafAvalaraCanadaQSTTaxCodeID,
						xafAvalaraCompanyCode = item2.xafAvalaraCompanyCode,
						xafAvalaraFilterCountry = item2.xafAvalaraFilterCountry,
						xafAvalaraLicenseKey = item2.xafAvalaraLicenseKey,
						xafAvalaraTaxCodeID = item2.xafAvalaraTaxCodeID,
						xafAvalaraTimeoutSeconds = item2.xafAvalaraTimeoutSeconds,
						xafAvalaraURL = item2.xafAvalaraURL,
						xafCAEmployerDentalBenefits = item2.xafCAEmployerDentalBenefits,
						xafCogsStatusHistory = item2.xafCogsStatusHistory,
						xafCogsUseAccounts = item2.xafCogsUseAccounts,
						xafCreatedBy = item2.xafCreatedBy,
						xafCreatedDate = item2.xafCreatedDate,
						xafCreditCardMethod = item2.xafCreditCardMethod,
						xafDrawerCashGlAccountID = item2.xafDrawerCashGlAccountID,
						xafDrawerCashStartAmount = item2.xafDrawerCashStartAmount,
						xafUniqueID = item2.xafUniqueID,
						xafGlFiscalYearID = item2.xafGlFiscalYearID,
						xafGlFiscalYearPeriodID = item2.xafGlFiscalYearPeriodID,
						xafGlRetainedEarningsAccountID = item2.xafGlRetainedEarningsAccountID,
						xafAgeByDaysInMonth = item2.xafAgeByDaysInMonth,
						xafApAllowParentAccountPost = item2.xafApAllowParentAccountPost,
						xafApAlwaysTakeDiscount = item2.xafApAlwaysTakeDiscount,
						xafApAssignNumbersToEft = item2.xafApAssignNumbersToEft,
						xafApCreditUpdatesReceipt = item2.xafApCreditUpdatesReceipt,
						xafApDisableTaxFields = item2.xafApDisableTaxFields,
						xafApDiscountOnFreight = item2.xafApDiscountOnFreight,
						xafApDiscountOnTax = item2.xafApDiscountOnTax,
						xafApExpressPost = item2.xafApExpressPost,
						xafApIncludeTaxInExpAmt = item2.xafApIncludeTaxInExpAmt,
						xafApPaymentFilterPlant = item2.xafApPaymentFilterPlant,
						xafApTaxOnFreight = item2.xafApTaxOnFreight,
						xafApUpdateJobCosts = item2.xafApUpdateJobCosts,
						xafArAllowParentAccountPost = item2.xafArAllowParentAccountPost,
						xafArCalculateTaxOnDeposit = item2.xafArCalculateTaxOnDeposit,
						xafArCreateDiscountJournals = item2.xafArCreateDiscountJournals,
						xafArCreditUpdatesShipment = item2.xafArCreditUpdatesShipment,
						xafArDisableTaxFields = item2.xafArDisableTaxFields,
						xafArDiscountOnFreight = item2.xafArDiscountOnFreight,
						xafArExpressPost = item2.xafArExpressPost,
						xafArIncludeFrgtInDepositCalc = item2.xafArIncludeFrgtInDepositCalc,
						xafArIncludeTaxInDepositCalc = item2.xafArIncludeTaxInDepositCalc,
						xafArPaymentFilterPlant = item2.xafArPaymentFilterPlant,
						xafArTaxOnFreight = item2.xafArTaxOnFreight,
						xafAvalaraDisableAddrValidate = item2.xafAvalaraDisableAddrValidate,
						xafAvalaraDisableIgnoreLine = item2.xafAvalaraDisableIgnoreLine,
						xafAvalaraForceAddressValidate = item2.xafAvalaraForceAddressValidate,
						xafCreateBankEntries = item2.xafCreateBankEntries,
						xafDisableMultiplePlants = item2.xafDisableMultiplePlants,
						xafExactDaysInPaymentTerms = item2.xafExactDaysInPaymentTerms,
						xafFAroundToNearestDollar = item2.xafFAroundToNearestDollar,
						xafGlCreateStockJournals = item2.xafGlCreateStockJournals,
						xafGlExpressPost = item2.xafGlExpressPost,
						xafIncludeLLInTermination = item2.xafIncludeLLInTermination,
						xafPAAllowParentAccountPost = item2.xafPAAllowParentAccountPost,
						xafPAAssignNumbersToEft = item2.xafPAAssignNumbersToEft,
						xafPADeleteZeroPayHeaders = item2.xafPADeleteZeroPayHeaders,
						xafPAExpressPost = item2.xafPAExpressPost,
						xafPartsMustExist = item2.xafPartsMustExist,
						xafPAShowHolidaysForSalary = item2.xafPAShowHolidaysForSalary,
						xafProductionExpressPost = item2.xafProductionExpressPost,
						xafRecalcSalarySacrifice = item2.xafRecalcSalarySacrifice,
						xafStpSetGrossPayAsETP = item2.xafStpSetGrossPayAsETP,
						xafLaborClearingGlAccountID = item2.xafLaborClearingGlAccountID,
						xafMiscReceiptVarianceAccount = item2.xafMiscReceiptVarianceAccount,
						xafOverheadClearingGlAccountID = item2.xafOverheadClearingGlAccountID,
						xafPALeaveBalanceCheck = item2.xafPALeaveBalanceCheck,
						xafPAUseDate = item2.xafPAUseDate,
						xafPurchaseVarianceGlAccountID = item2.xafPurchaseVarianceGlAccountID,
						xafRoundingGlAccountID = item2.xafRoundingGlAccountID,
						xafRowVersion = item2.xafRowVersion,
						xafShipAwaitInvoiceGlAccountID = item2.xafShipAwaitInvoiceGlAccountID,
						xafStockInTransitGlAccountID = item2.xafStockInTransitGlAccountID,
						xafStockRevaluationGlAccountID = item2.xafStockRevaluationGlAccountID,
						xafStoreCreditGlAccountID = item2.xafStoreCreditGlAccountID,
						xafSuperEmployerID = item2.xafSuperEmployerID,
						xafSuperEndDate = item2.xafSuperEndDate,
						xafSuperExportDateFormat = item2.xafSuperExportDateFormat,
						xafSuperExportFilePath = item2.xafSuperExportFilePath,
						xafSuperStartDate = item2.xafSuperStartDate,
						xafSVarLaborGlAccountID = item2.xafSVarLaborGlAccountID,
						xafSVarMaterialGlAccountID = item2.xafSVarMaterialGlAccountID,
						xafSVarOverheadGlAccountID = item2.xafSVarOverheadGlAccountID,
						xafSVarSubcontractGlAccountID = item2.xafSVarSubcontractGlAccountID,
						xafTaxOnReportMethod = item2.xafTaxOnReportMethod,
						xafTestFileCode = item2.xafTestFileCode,
						xafTransmitterControlCode = item2.xafTransmitterControlCode,
						xafUS1094FileLocation = item2.xafUS1094FileLocation,
						xafWipLaborGlAccountID = item2.xafWipLaborGlAccountID,
						xafWipMaterialGlAccountID = item2.xafWipMaterialGlAccountID,
						xafWipoverheadGlAccountID = item2.xafWipoverheadGlAccountID,
						xafWipSubcontractGlAccountID = item2.xafWipSubcontractGlAccountID,
						CustomFields = item2.CustomFields
					};
					allFinancialPropertiesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all FinancialProperties]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPFinancialPropertyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allFinancialPropertiesDto,
				RecordCount = allFinancialPropertiesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFinancialPropertyDto>> Process_GetFinancialProperty(Guid financialPropertyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPFinancialPropertyDto financialPropertyDto = null;
		ERPResponseMessageDto<ERPFinancialPropertyDto> result;
		try
		{
			IERPFinancialPropertyRepository iERPFinancialPropertyRepository = (base.ERPFinancialPropertyRepository = new ERPFinancialPropertyRepository(base.ApiClientContext));
			using (iERPFinancialPropertyRepository)
			{
				ERPFinancialPropertyInformationDto eRPFinancialPropertyInformationDto = await base.ERPFinancialPropertyRepository.GetFinancialProperty(financialPropertyId);
				financialPropertyDto = new ERPFinancialPropertyDto
				{
					xafAccruedCreditorsGlAccountID = eRPFinancialPropertyInformationDto.xafAccruedCreditorsGlAccountID,
					xafAgingMethod = eRPFinancialPropertyInformationDto.xafAgingMethod,
					xafApAgingBucketID = eRPFinancialPropertyInformationDto.xafApAgingBucketID,
					xafApApCostStartDate = eRPFinancialPropertyInformationDto.xafApApCostStartDate,
					xafApApGlAccountID = eRPFinancialPropertyInformationDto.xafApApGlAccountID,
					xafApCashGlAccountID = eRPFinancialPropertyInformationDto.xafApCashGlAccountID,
					xafApDiscountGlAccountID = eRPFinancialPropertyInformationDto.xafApDiscountGlAccountID,
					xafApFreightGlAccountID = eRPFinancialPropertyInformationDto.xafApFreightGlAccountID,
					xafApGroupReceiptsBySupplier = eRPFinancialPropertyInformationDto.xafApGroupReceiptsBySupplier,
					xafApPaymentMaxLinesPerPage = eRPFinancialPropertyInformationDto.xafApPaymentMaxLinesPerPage,
					xafArAgingBucketID = eRPFinancialPropertyInformationDto.xafArAgingBucketID,
					xafArArGlAccountID = eRPFinancialPropertyInformationDto.xafArArGlAccountID,
					xafArCashGlAccountID = eRPFinancialPropertyInformationDto.xafArCashGlAccountID,
					xafArDefaultLaborPartGroupID = eRPFinancialPropertyInformationDto.xafArDefaultLaborPartGroupID,
					xafArDepositGlAccountID = eRPFinancialPropertyInformationDto.xafArDepositGlAccountID,
					xafArDepositPartID = eRPFinancialPropertyInformationDto.xafArDepositPartID,
					xafArDepositPartRevisionID = eRPFinancialPropertyInformationDto.xafArDepositPartRevisionID,
					xafArDiscountGlAccountID = eRPFinancialPropertyInformationDto.xafArDiscountGlAccountID,
					xafArFinanceChargeGlAccountID = eRPFinancialPropertyInformationDto.xafArFinanceChargeGlAccountID,
					xafArFinanceChargeGraceDays = eRPFinancialPropertyInformationDto.xafArFinanceChargeGraceDays,
					xafArFinanceChargeLastRunDate = eRPFinancialPropertyInformationDto.xafArFinanceChargeLastRunDate,
					xafArFinanceChargePercent = eRPFinancialPropertyInformationDto.xafArFinanceChargePercent,
					xafArFinanceShowCreditBalance = eRPFinancialPropertyInformationDto.xafArFinanceShowCreditBalance,
					xafArFreightGlAccountID = eRPFinancialPropertyInformationDto.xafArFreightGlAccountID,
					xafArGroupShipmentsByCustomer = eRPFinancialPropertyInformationDto.xafArGroupShipmentsByCustomer,
					xafArLaborPartID = eRPFinancialPropertyInformationDto.xafArLaborPartID,
					xafArLaborPartRevisionID = eRPFinancialPropertyInformationDto.xafArLaborPartRevisionID,
					xafArNET1GatewayID = eRPFinancialPropertyInformationDto.xafArNET1GatewayID,
					xafArNET1MerchantKey = eRPFinancialPropertyInformationDto.xafArNET1MerchantKey,
					xafArNET1Port = eRPFinancialPropertyInformationDto.xafArNET1Port,
					xafArNET1TimeoutSeconds = eRPFinancialPropertyInformationDto.xafArNET1TimeoutSeconds,
					xafArShowDeposits = eRPFinancialPropertyInformationDto.xafArShowDeposits,
					xafAvalaraAccountID = eRPFinancialPropertyInformationDto.xafAvalaraAccountID,
					xafAvalaraArInvoicePostOption = eRPFinancialPropertyInformationDto.xafAvalaraArInvoicePostOption,
					xafAvalaraCanadaGstTaxCodeID = eRPFinancialPropertyInformationDto.xafAvalaraCanadaGstTaxCodeID,
					xafAvalaraCanadaHSTTaxCodeID = eRPFinancialPropertyInformationDto.xafAvalaraCanadaHSTTaxCodeID,
					xafAvalaraCanadaPSTTaxCodeID = eRPFinancialPropertyInformationDto.xafAvalaraCanadaPSTTaxCodeID,
					xafAvalaraCanadaQSTTaxCodeID = eRPFinancialPropertyInformationDto.xafAvalaraCanadaQSTTaxCodeID,
					xafAvalaraCompanyCode = eRPFinancialPropertyInformationDto.xafAvalaraCompanyCode,
					xafAvalaraFilterCountry = eRPFinancialPropertyInformationDto.xafAvalaraFilterCountry,
					xafAvalaraLicenseKey = eRPFinancialPropertyInformationDto.xafAvalaraLicenseKey,
					xafAvalaraTaxCodeID = eRPFinancialPropertyInformationDto.xafAvalaraTaxCodeID,
					xafAvalaraTimeoutSeconds = eRPFinancialPropertyInformationDto.xafAvalaraTimeoutSeconds,
					xafAvalaraURL = eRPFinancialPropertyInformationDto.xafAvalaraURL,
					xafCAEmployerDentalBenefits = eRPFinancialPropertyInformationDto.xafCAEmployerDentalBenefits,
					xafCogsStatusHistory = eRPFinancialPropertyInformationDto.xafCogsStatusHistory,
					xafCogsUseAccounts = eRPFinancialPropertyInformationDto.xafCogsUseAccounts,
					xafCreatedBy = eRPFinancialPropertyInformationDto.xafCreatedBy,
					xafCreatedDate = eRPFinancialPropertyInformationDto.xafCreatedDate,
					xafCreditCardMethod = eRPFinancialPropertyInformationDto.xafCreditCardMethod,
					xafDrawerCashGlAccountID = eRPFinancialPropertyInformationDto.xafDrawerCashGlAccountID,
					xafDrawerCashStartAmount = eRPFinancialPropertyInformationDto.xafDrawerCashStartAmount,
					xafUniqueID = eRPFinancialPropertyInformationDto.xafUniqueID,
					xafGlFiscalYearID = eRPFinancialPropertyInformationDto.xafGlFiscalYearID,
					xafGlFiscalYearPeriodID = eRPFinancialPropertyInformationDto.xafGlFiscalYearPeriodID,
					xafGlRetainedEarningsAccountID = eRPFinancialPropertyInformationDto.xafGlRetainedEarningsAccountID,
					xafAgeByDaysInMonth = eRPFinancialPropertyInformationDto.xafAgeByDaysInMonth,
					xafApAllowParentAccountPost = eRPFinancialPropertyInformationDto.xafApAllowParentAccountPost,
					xafApAlwaysTakeDiscount = eRPFinancialPropertyInformationDto.xafApAlwaysTakeDiscount,
					xafApAssignNumbersToEft = eRPFinancialPropertyInformationDto.xafApAssignNumbersToEft,
					xafApCreditUpdatesReceipt = eRPFinancialPropertyInformationDto.xafApCreditUpdatesReceipt,
					xafApDisableTaxFields = eRPFinancialPropertyInformationDto.xafApDisableTaxFields,
					xafApDiscountOnFreight = eRPFinancialPropertyInformationDto.xafApDiscountOnFreight,
					xafApDiscountOnTax = eRPFinancialPropertyInformationDto.xafApDiscountOnTax,
					xafApExpressPost = eRPFinancialPropertyInformationDto.xafApExpressPost,
					xafApIncludeTaxInExpAmt = eRPFinancialPropertyInformationDto.xafApIncludeTaxInExpAmt,
					xafApPaymentFilterPlant = eRPFinancialPropertyInformationDto.xafApPaymentFilterPlant,
					xafApTaxOnFreight = eRPFinancialPropertyInformationDto.xafApTaxOnFreight,
					xafApUpdateJobCosts = eRPFinancialPropertyInformationDto.xafApUpdateJobCosts,
					xafArAllowParentAccountPost = eRPFinancialPropertyInformationDto.xafArAllowParentAccountPost,
					xafArCalculateTaxOnDeposit = eRPFinancialPropertyInformationDto.xafArCalculateTaxOnDeposit,
					xafArCreateDiscountJournals = eRPFinancialPropertyInformationDto.xafArCreateDiscountJournals,
					xafArCreditUpdatesShipment = eRPFinancialPropertyInformationDto.xafArCreditUpdatesShipment,
					xafArDisableTaxFields = eRPFinancialPropertyInformationDto.xafArDisableTaxFields,
					xafArDiscountOnFreight = eRPFinancialPropertyInformationDto.xafArDiscountOnFreight,
					xafArExpressPost = eRPFinancialPropertyInformationDto.xafArExpressPost,
					xafArIncludeFrgtInDepositCalc = eRPFinancialPropertyInformationDto.xafArIncludeFrgtInDepositCalc,
					xafArIncludeTaxInDepositCalc = eRPFinancialPropertyInformationDto.xafArIncludeTaxInDepositCalc,
					xafArPaymentFilterPlant = eRPFinancialPropertyInformationDto.xafArPaymentFilterPlant,
					xafArTaxOnFreight = eRPFinancialPropertyInformationDto.xafArTaxOnFreight,
					xafAvalaraDisableAddrValidate = eRPFinancialPropertyInformationDto.xafAvalaraDisableAddrValidate,
					xafAvalaraDisableIgnoreLine = eRPFinancialPropertyInformationDto.xafAvalaraDisableIgnoreLine,
					xafAvalaraForceAddressValidate = eRPFinancialPropertyInformationDto.xafAvalaraForceAddressValidate,
					xafCreateBankEntries = eRPFinancialPropertyInformationDto.xafCreateBankEntries,
					xafDisableMultiplePlants = eRPFinancialPropertyInformationDto.xafDisableMultiplePlants,
					xafExactDaysInPaymentTerms = eRPFinancialPropertyInformationDto.xafExactDaysInPaymentTerms,
					xafFAroundToNearestDollar = eRPFinancialPropertyInformationDto.xafFAroundToNearestDollar,
					xafGlCreateStockJournals = eRPFinancialPropertyInformationDto.xafGlCreateStockJournals,
					xafGlExpressPost = eRPFinancialPropertyInformationDto.xafGlExpressPost,
					xafIncludeLLInTermination = eRPFinancialPropertyInformationDto.xafIncludeLLInTermination,
					xafPAAllowParentAccountPost = eRPFinancialPropertyInformationDto.xafPAAllowParentAccountPost,
					xafPAAssignNumbersToEft = eRPFinancialPropertyInformationDto.xafPAAssignNumbersToEft,
					xafPADeleteZeroPayHeaders = eRPFinancialPropertyInformationDto.xafPADeleteZeroPayHeaders,
					xafPAExpressPost = eRPFinancialPropertyInformationDto.xafPAExpressPost,
					xafPartsMustExist = eRPFinancialPropertyInformationDto.xafPartsMustExist,
					xafPAShowHolidaysForSalary = eRPFinancialPropertyInformationDto.xafPAShowHolidaysForSalary,
					xafProductionExpressPost = eRPFinancialPropertyInformationDto.xafProductionExpressPost,
					xafRecalcSalarySacrifice = eRPFinancialPropertyInformationDto.xafRecalcSalarySacrifice,
					xafStpSetGrossPayAsETP = eRPFinancialPropertyInformationDto.xafStpSetGrossPayAsETP,
					xafLaborClearingGlAccountID = eRPFinancialPropertyInformationDto.xafLaborClearingGlAccountID,
					xafMiscReceiptVarianceAccount = eRPFinancialPropertyInformationDto.xafMiscReceiptVarianceAccount,
					xafOverheadClearingGlAccountID = eRPFinancialPropertyInformationDto.xafOverheadClearingGlAccountID,
					xafPALeaveBalanceCheck = eRPFinancialPropertyInformationDto.xafPALeaveBalanceCheck,
					xafPAUseDate = eRPFinancialPropertyInformationDto.xafPAUseDate,
					xafPurchaseVarianceGlAccountID = eRPFinancialPropertyInformationDto.xafPurchaseVarianceGlAccountID,
					xafRoundingGlAccountID = eRPFinancialPropertyInformationDto.xafRoundingGlAccountID,
					xafRowVersion = eRPFinancialPropertyInformationDto.xafRowVersion,
					xafShipAwaitInvoiceGlAccountID = eRPFinancialPropertyInformationDto.xafShipAwaitInvoiceGlAccountID,
					xafStockInTransitGlAccountID = eRPFinancialPropertyInformationDto.xafStockInTransitGlAccountID,
					xafStockRevaluationGlAccountID = eRPFinancialPropertyInformationDto.xafStockRevaluationGlAccountID,
					xafStoreCreditGlAccountID = eRPFinancialPropertyInformationDto.xafStoreCreditGlAccountID,
					xafSuperEmployerID = eRPFinancialPropertyInformationDto.xafSuperEmployerID,
					xafSuperEndDate = eRPFinancialPropertyInformationDto.xafSuperEndDate,
					xafSuperExportDateFormat = eRPFinancialPropertyInformationDto.xafSuperExportDateFormat,
					xafSuperExportFilePath = eRPFinancialPropertyInformationDto.xafSuperExportFilePath,
					xafSuperStartDate = eRPFinancialPropertyInformationDto.xafSuperStartDate,
					xafSVarLaborGlAccountID = eRPFinancialPropertyInformationDto.xafSVarLaborGlAccountID,
					xafSVarMaterialGlAccountID = eRPFinancialPropertyInformationDto.xafSVarMaterialGlAccountID,
					xafSVarOverheadGlAccountID = eRPFinancialPropertyInformationDto.xafSVarOverheadGlAccountID,
					xafSVarSubcontractGlAccountID = eRPFinancialPropertyInformationDto.xafSVarSubcontractGlAccountID,
					xafTaxOnReportMethod = eRPFinancialPropertyInformationDto.xafTaxOnReportMethod,
					xafTestFileCode = eRPFinancialPropertyInformationDto.xafTestFileCode,
					xafTransmitterControlCode = eRPFinancialPropertyInformationDto.xafTransmitterControlCode,
					xafUS1094FileLocation = eRPFinancialPropertyInformationDto.xafUS1094FileLocation,
					xafWipLaborGlAccountID = eRPFinancialPropertyInformationDto.xafWipLaborGlAccountID,
					xafWipMaterialGlAccountID = eRPFinancialPropertyInformationDto.xafWipMaterialGlAccountID,
					xafWipoverheadGlAccountID = eRPFinancialPropertyInformationDto.xafWipoverheadGlAccountID,
					xafWipSubcontractGlAccountID = eRPFinancialPropertyInformationDto.xafWipSubcontractGlAccountID,
					CustomFields = eRPFinancialPropertyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the FinancialProperties []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFinancialPropertyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = financialPropertyDto
			};
		}
		return result;
	}
}
