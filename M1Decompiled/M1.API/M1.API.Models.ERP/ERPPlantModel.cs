using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPlantModel : ERPBaseModel, IERPPlantModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPlantRepository iERPPlantRepository = (base.ERPPlantRepository = new ERPPlantRepository(base.ApiClientContext));
		using (iERPPlantRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPlantRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPlantRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPlantRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPlantRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPlant(Guid plantId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPlantRepository iERPPlantRepository = (base.ERPPlantRepository = new ERPPlantRepository(base.ApiClientContext));
		using (iERPPlantRepository)
		{
			if (!(await base.ERPPlantRepository.DoesPlantExist(plantId)))
			{
				errorsList.Add($"Plant [{plantId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPlant(ERPPlantDto plant)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPlantRepository iERPPlantRepository = (base.ERPPlantRepository = new ERPPlantRepository(base.ApiClientContext));
		using (iERPPlantRepository)
		{
			if (!string.IsNullOrWhiteSpace(plant.xauArArGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauArArGlAccountID })))
			{
				errorsList.Add("xauArArGlAccountID [" + plant.xauArArGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauArCashGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauArCashGlAccountID })))
			{
				errorsList.Add("xauArCashGlAccountID [" + plant.xauArCashGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauArFreightGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauArFreightGlAccountID })))
			{
				errorsList.Add("xauArFreightGlAccountID [" + plant.xauArFreightGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauArDiscountGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauArDiscountGlAccountID })))
			{
				errorsList.Add("xauArDiscountGlAccountID [" + plant.xauArDiscountGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauArSalesGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauArSalesGlAccountID })))
			{
				errorsList.Add("xauArSalesGlAccountID [" + plant.xauArSalesGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauArBankAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("BankAccounts", new object[1] { "GLNBANKACCOUNTID" }, new object[1] { plant.xauArBankAccountID })))
			{
				errorsList.Add("xauArBankAccountID [" + plant.xauArBankAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauApApGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauApApGlAccountID })))
			{
				errorsList.Add("xauApApGlAccountID [" + plant.xauApApGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauApCashGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauApCashGlAccountID })))
			{
				errorsList.Add("xauApCashGlAccountID [" + plant.xauApCashGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauApFreightGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauApFreightGlAccountID })))
			{
				errorsList.Add("xauApFreightGlAccountID [" + plant.xauApFreightGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauApDiscountGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauApDiscountGlAccountID })))
			{
				errorsList.Add("xauApDiscountGlAccountID [" + plant.xauApDiscountGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauApBankAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("BankAccounts", new object[1] { "GLNBANKACCOUNTID" }, new object[1] { plant.xauApBankAccountID })))
			{
				errorsList.Add("xauApBankAccountID [" + plant.xauApBankAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauSVarLaborGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauSVarLaborGlAccountID })))
			{
				errorsList.Add("xauSVarLaborGlAccountID [" + plant.xauSVarLaborGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauSVarMaterialGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauSVarMaterialGlAccountID })))
			{
				errorsList.Add("xauSVarMaterialGlAccountID [" + plant.xauSVarMaterialGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauSVarSubcontractGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauSVarSubcontractGlAccountID })))
			{
				errorsList.Add("xauSVarSubcontractGlAccountID [" + plant.xauSVarSubcontractGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauSVarOverheadGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauSVarOverheadGlAccountID })))
			{
				errorsList.Add("xauSVarOverheadGlAccountID [" + plant.xauSVarOverheadGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauPurchaseVarianceGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauPurchaseVarianceGlAccountID })))
			{
				errorsList.Add("xauPurchaseVarianceGlAccountID [" + plant.xauPurchaseVarianceGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauWipLaborGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauWipLaborGlAccountID })))
			{
				errorsList.Add("xauWipLaborGlAccountID [" + plant.xauWipLaborGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauWipMaterialGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauWipMaterialGlAccountID })))
			{
				errorsList.Add("xauWipMaterialGlAccountID [" + plant.xauWipMaterialGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauWipSubcontractGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauWipSubcontractGlAccountID })))
			{
				errorsList.Add("xauWipSubcontractGlAccountID [" + plant.xauWipSubcontractGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauWipoverheadGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauWipoverheadGlAccountID })))
			{
				errorsList.Add("xauWipoverheadGlAccountID [" + plant.xauWipoverheadGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauAccruedCreditorsGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauAccruedCreditorsGlAccountID })))
			{
				errorsList.Add("xauAccruedCreditorsGlAccountID [" + plant.xauAccruedCreditorsGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauLaborClearingGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauLaborClearingGlAccountID })))
			{
				errorsList.Add("xauLaborClearingGlAccountID [" + plant.xauLaborClearingGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauOverheadClearingGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauOverheadClearingGlAccountID })))
			{
				errorsList.Add("xauOverheadClearingGlAccountID [" + plant.xauOverheadClearingGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauStockRevaluationGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauStockRevaluationGlAccountID })))
			{
				errorsList.Add("xauStockRevaluationGlAccountID [" + plant.xauStockRevaluationGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauArDepositGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauArDepositGlAccountID })))
			{
				errorsList.Add("xauArDepositGlAccountID [" + plant.xauArDepositGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauShipAwaitInvoiceGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauShipAwaitInvoiceGlAccountID })))
			{
				errorsList.Add("xauShipAwaitInvoiceGlAccountID [" + plant.xauShipAwaitInvoiceGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plant.xauStockInTransitGlAccountID) && !(await base.ERPPlantRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plant.xauStockInTransitGlAccountID })))
			{
				errorsList.Add("xauStockInTransitGlAccountID [" + plant.xauStockInTransitGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPlantDto>>> Process_GetAllPlants(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPlantDto> allPlantsDto = new List<ERPPlantDto>();
		ERPResponseMessageDto<IList<ERPPlantDto>> result;
		try
		{
			IERPPlantRepository iERPPlantRepository = (base.ERPPlantRepository = new ERPPlantRepository(base.ApiClientContext));
			using (iERPPlantRepository)
			{
				foreach (ERPPlantInformationDto item2 in await base.ERPPlantRepository.GetAllPlants(pageSize, pageNumber, filter, orderBy))
				{
					ERPPlantDto item = new ERPPlantDto
					{
						xauAccruedCreditorsGlAccountID = item2.xauAccruedCreditorsGlAccountID,
						xauAddressLine1 = item2.xauAddressLine1,
						xauAddressLine2 = item2.xauAddressLine2,
						xauAddressLine3 = item2.xauAddressLine3,
						xauApApGlAccountID = item2.xauApApGlAccountID,
						xauApBankAccountID = item2.xauApBankAccountID,
						xauApCashGlAccountID = item2.xauApCashGlAccountID,
						xauApDiscountGlAccountID = item2.xauApDiscountGlAccountID,
						xauApFreightGlAccountID = item2.xauApFreightGlAccountID,
						xauArArGlAccountID = item2.xauArArGlAccountID,
						xauArBankAccountID = item2.xauArBankAccountID,
						xauArCashGlAccountID = item2.xauArCashGlAccountID,
						xauArDepositGlAccountID = item2.xauArDepositGlAccountID,
						xauArDiscountGlAccountID = item2.xauArDiscountGlAccountID,
						xauArFreightGlAccountID = item2.xauArFreightGlAccountID,
						xauArSalesGlAccountID = item2.xauArSalesGlAccountID,
						xauCity = item2.xauCity,
						xauPlantID = item2.xauPlantID,
						xauCountry = item2.xauCountry,
						xauCountryCode = item2.xauCountryCode,
						xauCreatedBy = item2.xauCreatedBy,
						xauCreatedDate = item2.xauCreatedDate,
						xauDayStartTimeFri = item2.xauDayStartTimeFri,
						xauDayStartTimeMon = item2.xauDayStartTimeMon,
						xauDayStartTimeSat = item2.xauDayStartTimeSat,
						xauDayStartTimeSun = item2.xauDayStartTimeSun,
						xauDayStartTimeThu = item2.xauDayStartTimeThu,
						xauDayStartTimeTue = item2.xauDayStartTimeTue,
						xauDayStartTimeWed = item2.xauDayStartTimeWed,
						xauEmailAddress = item2.xauEmailAddress,
						xauUniqueID = item2.xauUniqueID,
						xauEstablishedDate = item2.xauEstablishedDate,
						xauFaxNumber = item2.xauFaxNumber,
						xauFederalID = item2.xauFederalID,
						xauHoursFri = item2.xauHoursFri,
						xauHoursMon = item2.xauHoursMon,
						xauHoursSat = item2.xauHoursSat,
						xauHoursSun = item2.xauHoursSun,
						xauHoursThu = item2.xauHoursThu,
						xauHoursTue = item2.xauHoursTue,
						xauHoursWed = item2.xauHoursWed,
						xauInactiveDate = item2.xauInactiveDate,
						xauInactive = item2.xauInactive,
						xauAvalaraAddressValidated = item2.xauAvalaraAddressValidated,
						xauUseProperties = item2.xauUseProperties,
						xauLaborClearingGlAccountID = item2.xauLaborClearingGlAccountID,
						xauName = item2.xauName,
						xauOverheadClearingGlAccountID = item2.xauOverheadClearingGlAccountID,
						xauPhoneNumber = item2.xauPhoneNumber,
						xauPostCode = item2.xauPostCode,
						xauPurchaseVarianceGlAccountID = item2.xauPurchaseVarianceGlAccountID,
						xauRowVersion = item2.xauRowVersion,
						xauShipAwaitInvoiceGlAccountID = item2.xauShipAwaitInvoiceGlAccountID,
						xauState = item2.xauState,
						xauStockInTransitGlAccountID = item2.xauStockInTransitGlAccountID,
						xauStockRevaluationGlAccountID = item2.xauStockRevaluationGlAccountID,
						xauSVarLaborGlAccountID = item2.xauSVarLaborGlAccountID,
						xauSVarMaterialGlAccountID = item2.xauSVarMaterialGlAccountID,
						xauSVarOverheadGlAccountID = item2.xauSVarOverheadGlAccountID,
						xauSVarSubcontractGlAccountID = item2.xauSVarSubcontractGlAccountID,
						xauWipLaborGlAccountID = item2.xauWipLaborGlAccountID,
						xauWipMaterialGlAccountID = item2.xauWipMaterialGlAccountID,
						xauWipoverheadGlAccountID = item2.xauWipoverheadGlAccountID,
						xauWipSubcontractGlAccountID = item2.xauWipSubcontractGlAccountID,
						CustomFields = item2.CustomFields
					};
					allPlantsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Plants]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPlantDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPlantsDto,
				RecordCount = allPlantsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPlantDto>> Process_GetPlant(Guid plantId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPlantDto plantDto = null;
		ERPResponseMessageDto<ERPPlantDto> result;
		try
		{
			IERPPlantRepository iERPPlantRepository = (base.ERPPlantRepository = new ERPPlantRepository(base.ApiClientContext));
			using (iERPPlantRepository)
			{
				ERPPlantInformationDto eRPPlantInformationDto = await base.ERPPlantRepository.GetPlant(plantId);
				plantDto = new ERPPlantDto
				{
					xauAccruedCreditorsGlAccountID = eRPPlantInformationDto.xauAccruedCreditorsGlAccountID,
					xauAddressLine1 = eRPPlantInformationDto.xauAddressLine1,
					xauAddressLine2 = eRPPlantInformationDto.xauAddressLine2,
					xauAddressLine3 = eRPPlantInformationDto.xauAddressLine3,
					xauApApGlAccountID = eRPPlantInformationDto.xauApApGlAccountID,
					xauApBankAccountID = eRPPlantInformationDto.xauApBankAccountID,
					xauApCashGlAccountID = eRPPlantInformationDto.xauApCashGlAccountID,
					xauApDiscountGlAccountID = eRPPlantInformationDto.xauApDiscountGlAccountID,
					xauApFreightGlAccountID = eRPPlantInformationDto.xauApFreightGlAccountID,
					xauArArGlAccountID = eRPPlantInformationDto.xauArArGlAccountID,
					xauArBankAccountID = eRPPlantInformationDto.xauArBankAccountID,
					xauArCashGlAccountID = eRPPlantInformationDto.xauArCashGlAccountID,
					xauArDepositGlAccountID = eRPPlantInformationDto.xauArDepositGlAccountID,
					xauArDiscountGlAccountID = eRPPlantInformationDto.xauArDiscountGlAccountID,
					xauArFreightGlAccountID = eRPPlantInformationDto.xauArFreightGlAccountID,
					xauArSalesGlAccountID = eRPPlantInformationDto.xauArSalesGlAccountID,
					xauCity = eRPPlantInformationDto.xauCity,
					xauPlantID = eRPPlantInformationDto.xauPlantID,
					xauCountry = eRPPlantInformationDto.xauCountry,
					xauCountryCode = eRPPlantInformationDto.xauCountryCode,
					xauCreatedBy = eRPPlantInformationDto.xauCreatedBy,
					xauCreatedDate = eRPPlantInformationDto.xauCreatedDate,
					xauDayStartTimeFri = eRPPlantInformationDto.xauDayStartTimeFri,
					xauDayStartTimeMon = eRPPlantInformationDto.xauDayStartTimeMon,
					xauDayStartTimeSat = eRPPlantInformationDto.xauDayStartTimeSat,
					xauDayStartTimeSun = eRPPlantInformationDto.xauDayStartTimeSun,
					xauDayStartTimeThu = eRPPlantInformationDto.xauDayStartTimeThu,
					xauDayStartTimeTue = eRPPlantInformationDto.xauDayStartTimeTue,
					xauDayStartTimeWed = eRPPlantInformationDto.xauDayStartTimeWed,
					xauEmailAddress = eRPPlantInformationDto.xauEmailAddress,
					xauUniqueID = eRPPlantInformationDto.xauUniqueID,
					xauEstablishedDate = eRPPlantInformationDto.xauEstablishedDate,
					xauFaxNumber = eRPPlantInformationDto.xauFaxNumber,
					xauFederalID = eRPPlantInformationDto.xauFederalID,
					xauHoursFri = eRPPlantInformationDto.xauHoursFri,
					xauHoursMon = eRPPlantInformationDto.xauHoursMon,
					xauHoursSat = eRPPlantInformationDto.xauHoursSat,
					xauHoursSun = eRPPlantInformationDto.xauHoursSun,
					xauHoursThu = eRPPlantInformationDto.xauHoursThu,
					xauHoursTue = eRPPlantInformationDto.xauHoursTue,
					xauHoursWed = eRPPlantInformationDto.xauHoursWed,
					xauInactiveDate = eRPPlantInformationDto.xauInactiveDate,
					xauInactive = eRPPlantInformationDto.xauInactive,
					xauAvalaraAddressValidated = eRPPlantInformationDto.xauAvalaraAddressValidated,
					xauUseProperties = eRPPlantInformationDto.xauUseProperties,
					xauLaborClearingGlAccountID = eRPPlantInformationDto.xauLaborClearingGlAccountID,
					xauName = eRPPlantInformationDto.xauName,
					xauOverheadClearingGlAccountID = eRPPlantInformationDto.xauOverheadClearingGlAccountID,
					xauPhoneNumber = eRPPlantInformationDto.xauPhoneNumber,
					xauPostCode = eRPPlantInformationDto.xauPostCode,
					xauPurchaseVarianceGlAccountID = eRPPlantInformationDto.xauPurchaseVarianceGlAccountID,
					xauRowVersion = eRPPlantInformationDto.xauRowVersion,
					xauShipAwaitInvoiceGlAccountID = eRPPlantInformationDto.xauShipAwaitInvoiceGlAccountID,
					xauState = eRPPlantInformationDto.xauState,
					xauStockInTransitGlAccountID = eRPPlantInformationDto.xauStockInTransitGlAccountID,
					xauStockRevaluationGlAccountID = eRPPlantInformationDto.xauStockRevaluationGlAccountID,
					xauSVarLaborGlAccountID = eRPPlantInformationDto.xauSVarLaborGlAccountID,
					xauSVarMaterialGlAccountID = eRPPlantInformationDto.xauSVarMaterialGlAccountID,
					xauSVarOverheadGlAccountID = eRPPlantInformationDto.xauSVarOverheadGlAccountID,
					xauSVarSubcontractGlAccountID = eRPPlantInformationDto.xauSVarSubcontractGlAccountID,
					xauWipLaborGlAccountID = eRPPlantInformationDto.xauWipLaborGlAccountID,
					xauWipMaterialGlAccountID = eRPPlantInformationDto.xauWipMaterialGlAccountID,
					xauWipoverheadGlAccountID = eRPPlantInformationDto.xauWipoverheadGlAccountID,
					xauWipSubcontractGlAccountID = eRPPlantInformationDto.xauWipSubcontractGlAccountID,
					CustomFields = eRPPlantInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Plants []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPlantDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = plantDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPlantDto>> Process_PutPlant(ERPPlantDto plant)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPlantDto createdObject = null;
		ERPResponseMessageDto<ERPPlantDto> result;
		try
		{
			IERPPlantRepository iERPPlantRepository = (base.ERPPlantRepository = new ERPPlantRepository(base.ApiClientContext));
			using (iERPPlantRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPlantRepository.SavePlant(plant);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPlantInformationDto eRPPlantInformationDto = await base.ERPPlantRepository.GetPlant(plant.xauUniqueID);
					createdObject = new ERPPlantDto
					{
						xauAccruedCreditorsGlAccountID = eRPPlantInformationDto.xauAccruedCreditorsGlAccountID,
						xauAddressLine1 = eRPPlantInformationDto.xauAddressLine1,
						xauAddressLine2 = eRPPlantInformationDto.xauAddressLine2,
						xauAddressLine3 = eRPPlantInformationDto.xauAddressLine3,
						xauApApGlAccountID = eRPPlantInformationDto.xauApApGlAccountID,
						xauApBankAccountID = eRPPlantInformationDto.xauApBankAccountID,
						xauApCashGlAccountID = eRPPlantInformationDto.xauApCashGlAccountID,
						xauApDiscountGlAccountID = eRPPlantInformationDto.xauApDiscountGlAccountID,
						xauApFreightGlAccountID = eRPPlantInformationDto.xauApFreightGlAccountID,
						xauArArGlAccountID = eRPPlantInformationDto.xauArArGlAccountID,
						xauArBankAccountID = eRPPlantInformationDto.xauArBankAccountID,
						xauArCashGlAccountID = eRPPlantInformationDto.xauArCashGlAccountID,
						xauArDepositGlAccountID = eRPPlantInformationDto.xauArDepositGlAccountID,
						xauArDiscountGlAccountID = eRPPlantInformationDto.xauArDiscountGlAccountID,
						xauArFreightGlAccountID = eRPPlantInformationDto.xauArFreightGlAccountID,
						xauArSalesGlAccountID = eRPPlantInformationDto.xauArSalesGlAccountID,
						xauCity = eRPPlantInformationDto.xauCity,
						xauPlantID = eRPPlantInformationDto.xauPlantID,
						xauCountry = eRPPlantInformationDto.xauCountry,
						xauCountryCode = eRPPlantInformationDto.xauCountryCode,
						xauCreatedBy = eRPPlantInformationDto.xauCreatedBy,
						xauCreatedDate = eRPPlantInformationDto.xauCreatedDate,
						xauDayStartTimeFri = eRPPlantInformationDto.xauDayStartTimeFri,
						xauDayStartTimeMon = eRPPlantInformationDto.xauDayStartTimeMon,
						xauDayStartTimeSat = eRPPlantInformationDto.xauDayStartTimeSat,
						xauDayStartTimeSun = eRPPlantInformationDto.xauDayStartTimeSun,
						xauDayStartTimeThu = eRPPlantInformationDto.xauDayStartTimeThu,
						xauDayStartTimeTue = eRPPlantInformationDto.xauDayStartTimeTue,
						xauDayStartTimeWed = eRPPlantInformationDto.xauDayStartTimeWed,
						xauEmailAddress = eRPPlantInformationDto.xauEmailAddress,
						xauUniqueID = eRPPlantInformationDto.xauUniqueID,
						xauEstablishedDate = eRPPlantInformationDto.xauEstablishedDate,
						xauFaxNumber = eRPPlantInformationDto.xauFaxNumber,
						xauFederalID = eRPPlantInformationDto.xauFederalID,
						xauHoursFri = eRPPlantInformationDto.xauHoursFri,
						xauHoursMon = eRPPlantInformationDto.xauHoursMon,
						xauHoursSat = eRPPlantInformationDto.xauHoursSat,
						xauHoursSun = eRPPlantInformationDto.xauHoursSun,
						xauHoursThu = eRPPlantInformationDto.xauHoursThu,
						xauHoursTue = eRPPlantInformationDto.xauHoursTue,
						xauHoursWed = eRPPlantInformationDto.xauHoursWed,
						xauInactiveDate = eRPPlantInformationDto.xauInactiveDate,
						xauInactive = eRPPlantInformationDto.xauInactive,
						xauAvalaraAddressValidated = eRPPlantInformationDto.xauAvalaraAddressValidated,
						xauUseProperties = eRPPlantInformationDto.xauUseProperties,
						xauLaborClearingGlAccountID = eRPPlantInformationDto.xauLaborClearingGlAccountID,
						xauName = eRPPlantInformationDto.xauName,
						xauOverheadClearingGlAccountID = eRPPlantInformationDto.xauOverheadClearingGlAccountID,
						xauPhoneNumber = eRPPlantInformationDto.xauPhoneNumber,
						xauPostCode = eRPPlantInformationDto.xauPostCode,
						xauPurchaseVarianceGlAccountID = eRPPlantInformationDto.xauPurchaseVarianceGlAccountID,
						xauRowVersion = eRPPlantInformationDto.xauRowVersion,
						xauShipAwaitInvoiceGlAccountID = eRPPlantInformationDto.xauShipAwaitInvoiceGlAccountID,
						xauState = eRPPlantInformationDto.xauState,
						xauStockInTransitGlAccountID = eRPPlantInformationDto.xauStockInTransitGlAccountID,
						xauStockRevaluationGlAccountID = eRPPlantInformationDto.xauStockRevaluationGlAccountID,
						xauSVarLaborGlAccountID = eRPPlantInformationDto.xauSVarLaborGlAccountID,
						xauSVarMaterialGlAccountID = eRPPlantInformationDto.xauSVarMaterialGlAccountID,
						xauSVarOverheadGlAccountID = eRPPlantInformationDto.xauSVarOverheadGlAccountID,
						xauSVarSubcontractGlAccountID = eRPPlantInformationDto.xauSVarSubcontractGlAccountID,
						xauWipLaborGlAccountID = eRPPlantInformationDto.xauWipLaborGlAccountID,
						xauWipMaterialGlAccountID = eRPPlantInformationDto.xauWipMaterialGlAccountID,
						xauWipoverheadGlAccountID = eRPPlantInformationDto.xauWipoverheadGlAccountID,
						xauWipSubcontractGlAccountID = eRPPlantInformationDto.xauWipSubcontractGlAccountID,
						CustomFields = eRPPlantInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Plant [{plant.xauUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPlantDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePlant(Guid plantId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPlantRepository iERPPlantRepository = (base.ERPPlantRepository = new ERPPlantRepository(base.ApiClientContext));
		using (iERPPlantRepository)
		{
			if (!(await base.ERPPlantRepository.DoesPlantExist(plantId)))
			{
				base.ErrorsList.Add($"Plant [{plantId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPlantInformationDto eRPPlantInformationDto = await base.ERPPlantRepository.GetPlant(plantId);
				string text = await base.ERPPlantRepository.WhereUsed("Plants", new object[1] { eRPPlantInformationDto.xauPlantID }, new object[1] { "xauPlantID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Plant cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPlantDto>> Process_DeletePlant(Guid plantId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPlantDto> result;
		try
		{
			IERPPlantRepository iERPPlantRepository = (base.ERPPlantRepository = new ERPPlantRepository(base.ApiClientContext));
			using (iERPPlantRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPlantRepository.DeleteRowFromTable("Plants", "xau", plantId);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
				IList<string> errorsList = base.ErrorsList;
				if (errorsList != null && errorsList.Count > 0)
				{
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Plant [{plantId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPlantDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPlantDto()
			};
		}
		return result;
	}
}
