using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPARPaymentSessionModel : ERPBaseModel, IERPARPaymentSessionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllARPaymentSessions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPARPaymentSessionRepository iERPARPaymentSessionRepository = (base.ERPARPaymentSessionRepository = new ERPARPaymentSessionRepository(base.ApiClientContext));
		using (iERPARPaymentSessionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPARPaymentSessionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPARPaymentSessionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPARPaymentSessionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPARPaymentSessionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetARPaymentSession(Guid aRPaymentSessionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARPaymentSessionRepository iERPARPaymentSessionRepository = (base.ERPARPaymentSessionRepository = new ERPARPaymentSessionRepository(base.ApiClientContext));
		using (iERPARPaymentSessionRepository)
		{
			if (!(await base.ERPARPaymentSessionRepository.DoesARPaymentSessionExist(aRPaymentSessionId)))
			{
				errorsList.Add($"ARPaymentSession [{aRPaymentSessionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutARPaymentSession(ERPARPaymentSessionDto aRPaymentSession)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARPaymentSessionRepository iERPARPaymentSessionRepository = (base.ERPARPaymentSessionRepository = new ERPARPaymentSessionRepository(base.ApiClientContext));
		using (iERPARPaymentSessionRepository)
		{
			if (!string.IsNullOrWhiteSpace(aRPaymentSession.arsPlantDepartmentID) && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { aRPaymentSession.arsPlantID, aRPaymentSession.arsPlantDepartmentID })))
			{
				errorsList.Add("arsPlantDepartmentID [" + aRPaymentSession.arsPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentSession.arsPlantID) && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { aRPaymentSession.arsPlantID })))
			{
				errorsList.Add("arsPlantID [" + aRPaymentSession.arsPlantID + "] not found.");
			}
			if (aRPaymentSession.arsGlFiscalYearID > 0 && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { aRPaymentSession.arsGlFiscalYearID })))
			{
				errorsList.Add($"arsGlFiscalYearID [{aRPaymentSession.arsGlFiscalYearID}] not found.");
			}
			if (aRPaymentSession.arsGlFiscalYearPeriodID > 0 && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { aRPaymentSession.arsGlFiscalYearID, aRPaymentSession.arsGlFiscalYearPeriodID })))
			{
				errorsList.Add($"arsGlFiscalYearPeriodID [{aRPaymentSession.arsGlFiscalYearPeriodID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentSession.arsArGlAccountID) && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentSession.arsArGlAccountID })))
			{
				errorsList.Add("arsArGlAccountID [" + aRPaymentSession.arsArGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentSession.arsCashGlAccountID) && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentSession.arsCashGlAccountID })))
			{
				errorsList.Add("arsCashGlAccountID [" + aRPaymentSession.arsCashGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentSession.arsBankAccountID) && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("BankAccounts", new object[1] { "GLNBANKACCOUNTID" }, new object[1] { aRPaymentSession.arsBankAccountID })))
			{
				errorsList.Add("arsBankAccountID [" + aRPaymentSession.arsBankAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentSession.arsDiscountGlAccountID) && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentSession.arsDiscountGlAccountID })))
			{
				errorsList.Add("arsDiscountGlAccountID [" + aRPaymentSession.arsDiscountGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentSession.arsApGlAccountID) && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentSession.arsApGlAccountID })))
			{
				errorsList.Add("arsApGlAccountID [" + aRPaymentSession.arsApGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentSession.arsApDiscountGlAccountID) && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentSession.arsApDiscountGlAccountID })))
			{
				errorsList.Add("arsApDiscountGlAccountID [" + aRPaymentSession.arsApDiscountGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentSession.arsCurrencyRateID) && !(await base.ERPARPaymentSessionRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { aRPaymentSession.arsCurrencyRateID })))
			{
				errorsList.Add("arsCurrencyRateID [" + aRPaymentSession.arsCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPARPaymentSessionDto>>> Process_GetAllARPaymentSessions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPARPaymentSessionDto> allARPaymentSessionsDto = new List<ERPARPaymentSessionDto>();
		ERPResponseMessageDto<IList<ERPARPaymentSessionDto>> result;
		try
		{
			IERPARPaymentSessionRepository iERPARPaymentSessionRepository = (base.ERPARPaymentSessionRepository = new ERPARPaymentSessionRepository(base.ApiClientContext));
			using (iERPARPaymentSessionRepository)
			{
				foreach (ERPARPaymentSessionInformationDto item2 in await base.ERPARPaymentSessionRepository.GetAllARPaymentSessions(pageSize, pageNumber, filter, orderBy))
				{
					ERPARPaymentSessionDto item = new ERPARPaymentSessionDto
					{
						arsApDiscountGlAccountID = item2.arsApDiscountGlAccountID,
						arsApGlAccountID = item2.arsApGlAccountID,
						arsArGlAccountID = item2.arsArGlAccountID,
						arsBankAccountID = item2.arsBankAccountID,
						arsCashGlAccountID = item2.arsCashGlAccountID,
						arsCreatedBy = item2.arsCreatedBy,
						arsCreatedDate = item2.arsCreatedDate,
						arsCurrencyRateID = item2.arsCurrencyRateID,
						arsDepositAmount = item2.arsDepositAmount,
						arsDepositAmountForeign = item2.arsDepositAmountForeign,
						arsDiscountGlAccountID = item2.arsDiscountGlAccountID,
						arsUniqueID = item2.arsUniqueID,
						arsExchangeRate = item2.arsExchangeRate,
						arsGlFiscalYearID = item2.arsGlFiscalYearID,
						arsGlFiscalYearPeriodID = item2.arsGlFiscalYearPeriodID,
						arsAvalaraTaxCalculated = item2.arsAvalaraTaxCalculated,
						arsCustomRate = item2.arsCustomRate,
						arsGroupBySettlement = item2.arsGroupBySettlement,
						arsOpenPaymentLoad = item2.arsOpenPaymentLoad,
						arsPostedToGl = item2.arsPostedToGl,
						arsPlantDepartmentID = item2.arsPlantDepartmentID,
						arsPlantID = item2.arsPlantID,
						arsPostedDate = item2.arsPostedDate,
						arsReceiptDate = item2.arsReceiptDate,
						arsRowVersion = item2.arsRowVersion,
						arsArPaymentSessionID = item2.arsArPaymentSessionID,
						arsSettlementEndTime = item2.arsSettlementEndTime,
						arsSettlementStartTime = item2.arsSettlementStartTime,
						CustomFields = item2.CustomFields
					};
					allARPaymentSessionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ARPaymentSessions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPARPaymentSessionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allARPaymentSessionsDto,
				RecordCount = allARPaymentSessionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARPaymentSessionDto>> Process_GetARPaymentSession(Guid aRPaymentSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPARPaymentSessionDto aRPaymentSessionDto = null;
		ERPResponseMessageDto<ERPARPaymentSessionDto> result;
		try
		{
			IERPARPaymentSessionRepository iERPARPaymentSessionRepository = (base.ERPARPaymentSessionRepository = new ERPARPaymentSessionRepository(base.ApiClientContext));
			using (iERPARPaymentSessionRepository)
			{
				ERPARPaymentSessionInformationDto eRPARPaymentSessionInformationDto = await base.ERPARPaymentSessionRepository.GetARPaymentSession(aRPaymentSessionId);
				aRPaymentSessionDto = new ERPARPaymentSessionDto
				{
					arsApDiscountGlAccountID = eRPARPaymentSessionInformationDto.arsApDiscountGlAccountID,
					arsApGlAccountID = eRPARPaymentSessionInformationDto.arsApGlAccountID,
					arsArGlAccountID = eRPARPaymentSessionInformationDto.arsArGlAccountID,
					arsBankAccountID = eRPARPaymentSessionInformationDto.arsBankAccountID,
					arsCashGlAccountID = eRPARPaymentSessionInformationDto.arsCashGlAccountID,
					arsCreatedBy = eRPARPaymentSessionInformationDto.arsCreatedBy,
					arsCreatedDate = eRPARPaymentSessionInformationDto.arsCreatedDate,
					arsCurrencyRateID = eRPARPaymentSessionInformationDto.arsCurrencyRateID,
					arsDepositAmount = eRPARPaymentSessionInformationDto.arsDepositAmount,
					arsDepositAmountForeign = eRPARPaymentSessionInformationDto.arsDepositAmountForeign,
					arsDiscountGlAccountID = eRPARPaymentSessionInformationDto.arsDiscountGlAccountID,
					arsUniqueID = eRPARPaymentSessionInformationDto.arsUniqueID,
					arsExchangeRate = eRPARPaymentSessionInformationDto.arsExchangeRate,
					arsGlFiscalYearID = eRPARPaymentSessionInformationDto.arsGlFiscalYearID,
					arsGlFiscalYearPeriodID = eRPARPaymentSessionInformationDto.arsGlFiscalYearPeriodID,
					arsAvalaraTaxCalculated = eRPARPaymentSessionInformationDto.arsAvalaraTaxCalculated,
					arsCustomRate = eRPARPaymentSessionInformationDto.arsCustomRate,
					arsGroupBySettlement = eRPARPaymentSessionInformationDto.arsGroupBySettlement,
					arsOpenPaymentLoad = eRPARPaymentSessionInformationDto.arsOpenPaymentLoad,
					arsPostedToGl = eRPARPaymentSessionInformationDto.arsPostedToGl,
					arsPlantDepartmentID = eRPARPaymentSessionInformationDto.arsPlantDepartmentID,
					arsPlantID = eRPARPaymentSessionInformationDto.arsPlantID,
					arsPostedDate = eRPARPaymentSessionInformationDto.arsPostedDate,
					arsReceiptDate = eRPARPaymentSessionInformationDto.arsReceiptDate,
					arsRowVersion = eRPARPaymentSessionInformationDto.arsRowVersion,
					arsArPaymentSessionID = eRPARPaymentSessionInformationDto.arsArPaymentSessionID,
					arsSettlementEndTime = eRPARPaymentSessionInformationDto.arsSettlementEndTime,
					arsSettlementStartTime = eRPARPaymentSessionInformationDto.arsSettlementStartTime,
					CustomFields = eRPARPaymentSessionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ARPaymentSessions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARPaymentSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aRPaymentSessionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARPaymentSessionDto>> Process_PutARPaymentSession(ERPARPaymentSessionDto aRPaymentSession)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPARPaymentSessionDto createdObject = null;
		ERPResponseMessageDto<ERPARPaymentSessionDto> result;
		try
		{
			IERPARPaymentSessionRepository iERPARPaymentSessionRepository = (base.ERPARPaymentSessionRepository = new ERPARPaymentSessionRepository(base.ApiClientContext));
			using (iERPARPaymentSessionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPARPaymentSessionRepository.SaveARPaymentSession(aRPaymentSession);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPARPaymentSessionInformationDto eRPARPaymentSessionInformationDto = await base.ERPARPaymentSessionRepository.GetARPaymentSession(aRPaymentSession.arsUniqueID);
					createdObject = new ERPARPaymentSessionDto
					{
						arsApDiscountGlAccountID = eRPARPaymentSessionInformationDto.arsApDiscountGlAccountID,
						arsApGlAccountID = eRPARPaymentSessionInformationDto.arsApGlAccountID,
						arsArGlAccountID = eRPARPaymentSessionInformationDto.arsArGlAccountID,
						arsBankAccountID = eRPARPaymentSessionInformationDto.arsBankAccountID,
						arsCashGlAccountID = eRPARPaymentSessionInformationDto.arsCashGlAccountID,
						arsCreatedBy = eRPARPaymentSessionInformationDto.arsCreatedBy,
						arsCreatedDate = eRPARPaymentSessionInformationDto.arsCreatedDate,
						arsCurrencyRateID = eRPARPaymentSessionInformationDto.arsCurrencyRateID,
						arsDepositAmount = eRPARPaymentSessionInformationDto.arsDepositAmount,
						arsDepositAmountForeign = eRPARPaymentSessionInformationDto.arsDepositAmountForeign,
						arsDiscountGlAccountID = eRPARPaymentSessionInformationDto.arsDiscountGlAccountID,
						arsUniqueID = eRPARPaymentSessionInformationDto.arsUniqueID,
						arsExchangeRate = eRPARPaymentSessionInformationDto.arsExchangeRate,
						arsGlFiscalYearID = eRPARPaymentSessionInformationDto.arsGlFiscalYearID,
						arsGlFiscalYearPeriodID = eRPARPaymentSessionInformationDto.arsGlFiscalYearPeriodID,
						arsAvalaraTaxCalculated = eRPARPaymentSessionInformationDto.arsAvalaraTaxCalculated,
						arsCustomRate = eRPARPaymentSessionInformationDto.arsCustomRate,
						arsGroupBySettlement = eRPARPaymentSessionInformationDto.arsGroupBySettlement,
						arsOpenPaymentLoad = eRPARPaymentSessionInformationDto.arsOpenPaymentLoad,
						arsPostedToGl = eRPARPaymentSessionInformationDto.arsPostedToGl,
						arsPlantDepartmentID = eRPARPaymentSessionInformationDto.arsPlantDepartmentID,
						arsPlantID = eRPARPaymentSessionInformationDto.arsPlantID,
						arsPostedDate = eRPARPaymentSessionInformationDto.arsPostedDate,
						arsReceiptDate = eRPARPaymentSessionInformationDto.arsReceiptDate,
						arsRowVersion = eRPARPaymentSessionInformationDto.arsRowVersion,
						arsArPaymentSessionID = eRPARPaymentSessionInformationDto.arsArPaymentSessionID,
						arsSettlementEndTime = eRPARPaymentSessionInformationDto.arsSettlementEndTime,
						arsSettlementStartTime = eRPARPaymentSessionInformationDto.arsSettlementStartTime,
						CustomFields = eRPARPaymentSessionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ARPaymentSession [{aRPaymentSession.arsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARPaymentSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteARPaymentSession(Guid aRPaymentSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARPaymentSessionRepository iERPARPaymentSessionRepository = (base.ERPARPaymentSessionRepository = new ERPARPaymentSessionRepository(base.ApiClientContext));
		using (iERPARPaymentSessionRepository)
		{
			if (!(await base.ERPARPaymentSessionRepository.DoesARPaymentSessionExist(aRPaymentSessionId)))
			{
				base.ErrorsList.Add($"ARPaymentSession [{aRPaymentSessionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPARPaymentSessionInformationDto eRPARPaymentSessionInformationDto = await base.ERPARPaymentSessionRepository.GetARPaymentSession(aRPaymentSessionId);
				string text = await base.ERPARPaymentSessionRepository.WhereUsed("ARPaymentSessions", new object[1] { eRPARPaymentSessionInformationDto.arsArPaymentSessionID }, new object[1] { "arsArPaymentSessionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ARPaymentSession cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPARPaymentSessionDto>> Process_DeleteARPaymentSession(Guid aRPaymentSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPARPaymentSessionDto> result;
		try
		{
			IERPARPaymentSessionRepository iERPARPaymentSessionRepository = (base.ERPARPaymentSessionRepository = new ERPARPaymentSessionRepository(base.ApiClientContext));
			using (iERPARPaymentSessionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPARPaymentSessionRepository.DeleteRowFromTable("ARPaymentSessions", "ars", aRPaymentSessionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ARPaymentSession [{aRPaymentSessionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARPaymentSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPARPaymentSessionDto()
			};
		}
		return result;
	}
}
