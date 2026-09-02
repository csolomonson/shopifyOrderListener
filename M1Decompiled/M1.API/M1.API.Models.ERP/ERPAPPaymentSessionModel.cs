using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAPPaymentSessionModel : ERPBaseModel, IERPAPPaymentSessionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAPPaymentSessions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAPPaymentSessionRepository iERPAPPaymentSessionRepository = (base.ERPAPPaymentSessionRepository = new ERPAPPaymentSessionRepository(base.ApiClientContext));
		using (iERPAPPaymentSessionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAPPaymentSessionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAPPaymentSessionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAPPaymentSessionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAPPaymentSessionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAPPaymentSession(Guid aPPaymentSessionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPPaymentSessionRepository iERPAPPaymentSessionRepository = (base.ERPAPPaymentSessionRepository = new ERPAPPaymentSessionRepository(base.ApiClientContext));
		using (iERPAPPaymentSessionRepository)
		{
			if (!(await base.ERPAPPaymentSessionRepository.DoesAPPaymentSessionExist(aPPaymentSessionId)))
			{
				errorsList.Add($"APPaymentSession [{aPPaymentSessionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAPPaymentSession(ERPAPPaymentSessionDto aPPaymentSession)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPPaymentSessionRepository iERPAPPaymentSessionRepository = (base.ERPAPPaymentSessionRepository = new ERPAPPaymentSessionRepository(base.ApiClientContext));
		using (iERPAPPaymentSessionRepository)
		{
			if (!string.IsNullOrWhiteSpace(aPPaymentSession.apsPlantDepartmentID) && !(await base.ERPAPPaymentSessionRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { aPPaymentSession.apsPlantID, aPPaymentSession.apsPlantDepartmentID })))
			{
				errorsList.Add("apsPlantDepartmentID [" + aPPaymentSession.apsPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentSession.apsPlantID) && !(await base.ERPAPPaymentSessionRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { aPPaymentSession.apsPlantID })))
			{
				errorsList.Add("apsPlantID [" + aPPaymentSession.apsPlantID + "] not found.");
			}
			if (aPPaymentSession.apsGlFiscalYearID > 0 && !(await base.ERPAPPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { aPPaymentSession.apsGlFiscalYearID })))
			{
				errorsList.Add($"apsGlFiscalYearID [{aPPaymentSession.apsGlFiscalYearID}] not found.");
			}
			if (aPPaymentSession.apsGlFiscalYearPeriodID > 0 && !(await base.ERPAPPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { aPPaymentSession.apsGlFiscalYearID, aPPaymentSession.apsGlFiscalYearPeriodID })))
			{
				errorsList.Add($"apsGlFiscalYearPeriodID [{aPPaymentSession.apsGlFiscalYearPeriodID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentSession.apsBankAccountID) && !(await base.ERPAPPaymentSessionRepository.DoesRecordExistInTableUsingKeys("BankAccounts", new object[1] { "GLNBANKACCOUNTID" }, new object[1] { aPPaymentSession.apsBankAccountID })))
			{
				errorsList.Add("apsBankAccountID [" + aPPaymentSession.apsBankAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentSession.apsCashGlAccountID) && !(await base.ERPAPPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPPaymentSession.apsCashGlAccountID })))
			{
				errorsList.Add("apsCashGlAccountID [" + aPPaymentSession.apsCashGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentSession.apsApGlAccountID) && !(await base.ERPAPPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPPaymentSession.apsApGlAccountID })))
			{
				errorsList.Add("apsApGlAccountID [" + aPPaymentSession.apsApGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentSession.apsArGlAccountID) && !(await base.ERPAPPaymentSessionRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPPaymentSession.apsArGlAccountID })))
			{
				errorsList.Add("apsArGlAccountID [" + aPPaymentSession.apsArGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentSession.apsCurrencyRateID) && !(await base.ERPAPPaymentSessionRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { aPPaymentSession.apsCurrencyRateID })))
			{
				errorsList.Add("apsCurrencyRateID [" + aPPaymentSession.apsCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAPPaymentSessionDto>>> Process_GetAllAPPaymentSessions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAPPaymentSessionDto> allAPPaymentSessionsDto = new List<ERPAPPaymentSessionDto>();
		ERPResponseMessageDto<IList<ERPAPPaymentSessionDto>> result;
		try
		{
			IERPAPPaymentSessionRepository iERPAPPaymentSessionRepository = (base.ERPAPPaymentSessionRepository = new ERPAPPaymentSessionRepository(base.ApiClientContext));
			using (iERPAPPaymentSessionRepository)
			{
				foreach (ERPAPPaymentSessionInformationDto item2 in await base.ERPAPPaymentSessionRepository.GetAllAPPaymentSessions(pageSize, pageNumber, filter, orderBy))
				{
					ERPAPPaymentSessionDto item = new ERPAPPaymentSessionDto
					{
						apsApGlAccountID = item2.apsApGlAccountID,
						apsArGlAccountID = item2.apsArGlAccountID,
						apsBankAccountID = item2.apsBankAccountID,
						apsCashGlAccountID = item2.apsCashGlAccountID,
						apsCompletedDate = item2.apsCompletedDate,
						apsCreatedBy = item2.apsCreatedBy,
						apsCreatedDate = item2.apsCreatedDate,
						apsCurrencyRateID = item2.apsCurrencyRateID,
						apsEftDescription = item2.apsEftDescription,
						apsEftReferenceNumber = item2.apsEftReferenceNumber,
						apsEftSettlementDate = item2.apsEftSettlementDate,
						apsUniqueID = item2.apsUniqueID,
						apsExchangeRate = item2.apsExchangeRate,
						apsGlFiscalYearID = item2.apsGlFiscalYearID,
						apsGlFiscalYearPeriodID = item2.apsGlFiscalYearPeriodID,
						apsCompleted = item2.apsCompleted,
						apsCustomRate = item2.apsCustomRate,
						apsOpenPaymentLoad = item2.apsOpenPaymentLoad,
						apsPaymentsPrinted = item2.apsPaymentsPrinted,
						apsPostedToGl = item2.apsPostedToGl,
						apsPaymentAmount = item2.apsPaymentAmount,
						apsPaymentAmountForeign = item2.apsPaymentAmountForeign,
						apsPaymentDate = item2.apsPaymentDate,
						apsPlantDepartmentID = item2.apsPlantDepartmentID,
						apsPlantID = item2.apsPlantID,
						apsPostedDate = item2.apsPostedDate,
						apsRowVersion = item2.apsRowVersion,
						apsApPaymentSessionID = item2.apsApPaymentSessionID,
						apsSessionType = item2.apsSessionType,
						CustomFields = item2.CustomFields
					};
					allAPPaymentSessionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all APPaymentSessions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAPPaymentSessionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAPPaymentSessionsDto,
				RecordCount = allAPPaymentSessionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPPaymentSessionDto>> Process_GetAPPaymentSession(Guid aPPaymentSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAPPaymentSessionDto aPPaymentSessionDto = null;
		ERPResponseMessageDto<ERPAPPaymentSessionDto> result;
		try
		{
			IERPAPPaymentSessionRepository iERPAPPaymentSessionRepository = (base.ERPAPPaymentSessionRepository = new ERPAPPaymentSessionRepository(base.ApiClientContext));
			using (iERPAPPaymentSessionRepository)
			{
				ERPAPPaymentSessionInformationDto eRPAPPaymentSessionInformationDto = await base.ERPAPPaymentSessionRepository.GetAPPaymentSession(aPPaymentSessionId);
				aPPaymentSessionDto = new ERPAPPaymentSessionDto
				{
					apsApGlAccountID = eRPAPPaymentSessionInformationDto.apsApGlAccountID,
					apsArGlAccountID = eRPAPPaymentSessionInformationDto.apsArGlAccountID,
					apsBankAccountID = eRPAPPaymentSessionInformationDto.apsBankAccountID,
					apsCashGlAccountID = eRPAPPaymentSessionInformationDto.apsCashGlAccountID,
					apsCompletedDate = eRPAPPaymentSessionInformationDto.apsCompletedDate,
					apsCreatedBy = eRPAPPaymentSessionInformationDto.apsCreatedBy,
					apsCreatedDate = eRPAPPaymentSessionInformationDto.apsCreatedDate,
					apsCurrencyRateID = eRPAPPaymentSessionInformationDto.apsCurrencyRateID,
					apsEftDescription = eRPAPPaymentSessionInformationDto.apsEftDescription,
					apsEftReferenceNumber = eRPAPPaymentSessionInformationDto.apsEftReferenceNumber,
					apsEftSettlementDate = eRPAPPaymentSessionInformationDto.apsEftSettlementDate,
					apsUniqueID = eRPAPPaymentSessionInformationDto.apsUniqueID,
					apsExchangeRate = eRPAPPaymentSessionInformationDto.apsExchangeRate,
					apsGlFiscalYearID = eRPAPPaymentSessionInformationDto.apsGlFiscalYearID,
					apsGlFiscalYearPeriodID = eRPAPPaymentSessionInformationDto.apsGlFiscalYearPeriodID,
					apsCompleted = eRPAPPaymentSessionInformationDto.apsCompleted,
					apsCustomRate = eRPAPPaymentSessionInformationDto.apsCustomRate,
					apsOpenPaymentLoad = eRPAPPaymentSessionInformationDto.apsOpenPaymentLoad,
					apsPaymentsPrinted = eRPAPPaymentSessionInformationDto.apsPaymentsPrinted,
					apsPostedToGl = eRPAPPaymentSessionInformationDto.apsPostedToGl,
					apsPaymentAmount = eRPAPPaymentSessionInformationDto.apsPaymentAmount,
					apsPaymentAmountForeign = eRPAPPaymentSessionInformationDto.apsPaymentAmountForeign,
					apsPaymentDate = eRPAPPaymentSessionInformationDto.apsPaymentDate,
					apsPlantDepartmentID = eRPAPPaymentSessionInformationDto.apsPlantDepartmentID,
					apsPlantID = eRPAPPaymentSessionInformationDto.apsPlantID,
					apsPostedDate = eRPAPPaymentSessionInformationDto.apsPostedDate,
					apsRowVersion = eRPAPPaymentSessionInformationDto.apsRowVersion,
					apsApPaymentSessionID = eRPAPPaymentSessionInformationDto.apsApPaymentSessionID,
					apsSessionType = eRPAPPaymentSessionInformationDto.apsSessionType,
					CustomFields = eRPAPPaymentSessionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the APPaymentSessions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPPaymentSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aPPaymentSessionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPPaymentSessionDto>> Process_PutAPPaymentSession(ERPAPPaymentSessionDto aPPaymentSession)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAPPaymentSessionDto createdObject = null;
		ERPResponseMessageDto<ERPAPPaymentSessionDto> result;
		try
		{
			IERPAPPaymentSessionRepository iERPAPPaymentSessionRepository = (base.ERPAPPaymentSessionRepository = new ERPAPPaymentSessionRepository(base.ApiClientContext));
			using (iERPAPPaymentSessionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAPPaymentSessionRepository.SaveAPPaymentSession(aPPaymentSession);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAPPaymentSessionInformationDto eRPAPPaymentSessionInformationDto = await base.ERPAPPaymentSessionRepository.GetAPPaymentSession(aPPaymentSession.apsUniqueID);
					createdObject = new ERPAPPaymentSessionDto
					{
						apsApGlAccountID = eRPAPPaymentSessionInformationDto.apsApGlAccountID,
						apsArGlAccountID = eRPAPPaymentSessionInformationDto.apsArGlAccountID,
						apsBankAccountID = eRPAPPaymentSessionInformationDto.apsBankAccountID,
						apsCashGlAccountID = eRPAPPaymentSessionInformationDto.apsCashGlAccountID,
						apsCompletedDate = eRPAPPaymentSessionInformationDto.apsCompletedDate,
						apsCreatedBy = eRPAPPaymentSessionInformationDto.apsCreatedBy,
						apsCreatedDate = eRPAPPaymentSessionInformationDto.apsCreatedDate,
						apsCurrencyRateID = eRPAPPaymentSessionInformationDto.apsCurrencyRateID,
						apsEftDescription = eRPAPPaymentSessionInformationDto.apsEftDescription,
						apsEftReferenceNumber = eRPAPPaymentSessionInformationDto.apsEftReferenceNumber,
						apsEftSettlementDate = eRPAPPaymentSessionInformationDto.apsEftSettlementDate,
						apsUniqueID = eRPAPPaymentSessionInformationDto.apsUniqueID,
						apsExchangeRate = eRPAPPaymentSessionInformationDto.apsExchangeRate,
						apsGlFiscalYearID = eRPAPPaymentSessionInformationDto.apsGlFiscalYearID,
						apsGlFiscalYearPeriodID = eRPAPPaymentSessionInformationDto.apsGlFiscalYearPeriodID,
						apsCompleted = eRPAPPaymentSessionInformationDto.apsCompleted,
						apsCustomRate = eRPAPPaymentSessionInformationDto.apsCustomRate,
						apsOpenPaymentLoad = eRPAPPaymentSessionInformationDto.apsOpenPaymentLoad,
						apsPaymentsPrinted = eRPAPPaymentSessionInformationDto.apsPaymentsPrinted,
						apsPostedToGl = eRPAPPaymentSessionInformationDto.apsPostedToGl,
						apsPaymentAmount = eRPAPPaymentSessionInformationDto.apsPaymentAmount,
						apsPaymentAmountForeign = eRPAPPaymentSessionInformationDto.apsPaymentAmountForeign,
						apsPaymentDate = eRPAPPaymentSessionInformationDto.apsPaymentDate,
						apsPlantDepartmentID = eRPAPPaymentSessionInformationDto.apsPlantDepartmentID,
						apsPlantID = eRPAPPaymentSessionInformationDto.apsPlantID,
						apsPostedDate = eRPAPPaymentSessionInformationDto.apsPostedDate,
						apsRowVersion = eRPAPPaymentSessionInformationDto.apsRowVersion,
						apsApPaymentSessionID = eRPAPPaymentSessionInformationDto.apsApPaymentSessionID,
						apsSessionType = eRPAPPaymentSessionInformationDto.apsSessionType,
						CustomFields = eRPAPPaymentSessionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing APPaymentSession [{aPPaymentSession.apsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPPaymentSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAPPaymentSession(Guid aPPaymentSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPPaymentSessionRepository iERPAPPaymentSessionRepository = (base.ERPAPPaymentSessionRepository = new ERPAPPaymentSessionRepository(base.ApiClientContext));
		using (iERPAPPaymentSessionRepository)
		{
			if (!(await base.ERPAPPaymentSessionRepository.DoesAPPaymentSessionExist(aPPaymentSessionId)))
			{
				base.ErrorsList.Add($"APPaymentSession [{aPPaymentSessionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAPPaymentSessionInformationDto eRPAPPaymentSessionInformationDto = await base.ERPAPPaymentSessionRepository.GetAPPaymentSession(aPPaymentSessionId);
				string text = await base.ERPAPPaymentSessionRepository.WhereUsed("APPaymentSessions", new object[1] { eRPAPPaymentSessionInformationDto.apsApPaymentSessionID }, new object[1] { "apsApPaymentSessionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("APPaymentSession cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAPPaymentSessionDto>> Process_DeleteAPPaymentSession(Guid aPPaymentSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAPPaymentSessionDto> result;
		try
		{
			IERPAPPaymentSessionRepository iERPAPPaymentSessionRepository = (base.ERPAPPaymentSessionRepository = new ERPAPPaymentSessionRepository(base.ApiClientContext));
			using (iERPAPPaymentSessionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAPPaymentSessionRepository.DeleteRowFromTable("APPaymentSessions", "aps", aPPaymentSessionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of APPaymentSession [{aPPaymentSessionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPPaymentSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAPPaymentSessionDto()
			};
		}
		return result;
	}
}
