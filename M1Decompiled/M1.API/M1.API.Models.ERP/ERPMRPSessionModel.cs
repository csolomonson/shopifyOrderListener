using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMRPSessionModel : ERPBaseModel, IERPMRPSessionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMRPSessions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMRPSessionRepository iERPMRPSessionRepository = (base.ERPMRPSessionRepository = new ERPMRPSessionRepository(base.ApiClientContext));
		using (iERPMRPSessionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMRPSessionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMRPSessionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMRPSessionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMRPSessionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMRPSession(Guid mRPSessionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPSessionRepository iERPMRPSessionRepository = (base.ERPMRPSessionRepository = new ERPMRPSessionRepository(base.ApiClientContext));
		using (iERPMRPSessionRepository)
		{
			if (!(await base.ERPMRPSessionRepository.DoesMRPSessionExist(mRPSessionId)))
			{
				errorsList.Add($"MRPSession [{mRPSessionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMRPSession(ERPMRPSessionDto mRPSession)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMRPSessionRepository iERPMRPSessionRepository = (base.ERPMRPSessionRepository = new ERPMRPSessionRepository(base.ApiClientContext));
		using (iERPMRPSessionRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMRPSessionDto>>> Process_GetAllMRPSessions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMRPSessionDto> allMRPSessionsDto = new List<ERPMRPSessionDto>();
		ERPResponseMessageDto<IList<ERPMRPSessionDto>> result;
		try
		{
			IERPMRPSessionRepository iERPMRPSessionRepository = (base.ERPMRPSessionRepository = new ERPMRPSessionRepository(base.ApiClientContext));
			using (iERPMRPSessionRepository)
			{
				foreach (ERPMRPSessionInformationDto item2 in await base.ERPMRPSessionRepository.GetAllMRPSessions(pageSize, pageNumber, filter, orderBy))
				{
					ERPMRPSessionDto item = new ERPMRPSessionDto
					{
						mrpCompletedDate = item2.mrpCompletedDate,
						mrpCreatedBy = item2.mrpCreatedBy,
						mrpCreatedDate = item2.mrpCreatedDate,
						mrpCustomerIDs = item2.mrpCustomerIDs,
						mrpCutoffDate = item2.mrpCutoffDate,
						mrpUniqueID = item2.mrpUniqueID,
						mrpCompleted = item2.mrpCompleted,
						mrpConsolidatePartForecastJobs = item2.mrpConsolidatePartForecastJobs,
						mrpGenerated = item2.mrpGenerated,
						mrpIncludePartForecasts = item2.mrpIncludePartForecasts,
						mrpPartClassIDs = item2.mrpPartClassIDs,
						mrpPartGroupIDs = item2.mrpPartGroupIDs,
						mrpPartIDs = item2.mrpPartIDs,
						mrpPlantIDs = item2.mrpPlantIDs,
						mrpRowVersion = item2.mrpRowVersion,
						mrpSessionID = item2.mrpSessionID,
						mrpWarehouseIDs = item2.mrpWarehouseIDs,
						CustomFields = item2.CustomFields
					};
					allMRPSessionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MRPSessions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMRPSessionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMRPSessionsDto,
				RecordCount = allMRPSessionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMRPSessionDto>> Process_GetMRPSession(Guid mRPSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMRPSessionDto mRPSessionDto = null;
		ERPResponseMessageDto<ERPMRPSessionDto> result;
		try
		{
			IERPMRPSessionRepository iERPMRPSessionRepository = (base.ERPMRPSessionRepository = new ERPMRPSessionRepository(base.ApiClientContext));
			using (iERPMRPSessionRepository)
			{
				ERPMRPSessionInformationDto eRPMRPSessionInformationDto = await base.ERPMRPSessionRepository.GetMRPSession(mRPSessionId);
				mRPSessionDto = new ERPMRPSessionDto
				{
					mrpCompletedDate = eRPMRPSessionInformationDto.mrpCompletedDate,
					mrpCreatedBy = eRPMRPSessionInformationDto.mrpCreatedBy,
					mrpCreatedDate = eRPMRPSessionInformationDto.mrpCreatedDate,
					mrpCustomerIDs = eRPMRPSessionInformationDto.mrpCustomerIDs,
					mrpCutoffDate = eRPMRPSessionInformationDto.mrpCutoffDate,
					mrpUniqueID = eRPMRPSessionInformationDto.mrpUniqueID,
					mrpCompleted = eRPMRPSessionInformationDto.mrpCompleted,
					mrpConsolidatePartForecastJobs = eRPMRPSessionInformationDto.mrpConsolidatePartForecastJobs,
					mrpGenerated = eRPMRPSessionInformationDto.mrpGenerated,
					mrpIncludePartForecasts = eRPMRPSessionInformationDto.mrpIncludePartForecasts,
					mrpPartClassIDs = eRPMRPSessionInformationDto.mrpPartClassIDs,
					mrpPartGroupIDs = eRPMRPSessionInformationDto.mrpPartGroupIDs,
					mrpPartIDs = eRPMRPSessionInformationDto.mrpPartIDs,
					mrpPlantIDs = eRPMRPSessionInformationDto.mrpPlantIDs,
					mrpRowVersion = eRPMRPSessionInformationDto.mrpRowVersion,
					mrpSessionID = eRPMRPSessionInformationDto.mrpSessionID,
					mrpWarehouseIDs = eRPMRPSessionInformationDto.mrpWarehouseIDs,
					CustomFields = eRPMRPSessionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MRPSessions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = mRPSessionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMRPSessionDto>> Process_PutMRPSession(ERPMRPSessionDto mRPSession)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMRPSessionDto createdObject = null;
		ERPResponseMessageDto<ERPMRPSessionDto> result;
		try
		{
			IERPMRPSessionRepository iERPMRPSessionRepository = (base.ERPMRPSessionRepository = new ERPMRPSessionRepository(base.ApiClientContext));
			using (iERPMRPSessionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMRPSessionRepository.SaveMRPSession(mRPSession);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMRPSessionInformationDto eRPMRPSessionInformationDto = await base.ERPMRPSessionRepository.GetMRPSession(mRPSession.mrpUniqueID);
					createdObject = new ERPMRPSessionDto
					{
						mrpCompletedDate = eRPMRPSessionInformationDto.mrpCompletedDate,
						mrpCreatedBy = eRPMRPSessionInformationDto.mrpCreatedBy,
						mrpCreatedDate = eRPMRPSessionInformationDto.mrpCreatedDate,
						mrpCustomerIDs = eRPMRPSessionInformationDto.mrpCustomerIDs,
						mrpCutoffDate = eRPMRPSessionInformationDto.mrpCutoffDate,
						mrpUniqueID = eRPMRPSessionInformationDto.mrpUniqueID,
						mrpCompleted = eRPMRPSessionInformationDto.mrpCompleted,
						mrpConsolidatePartForecastJobs = eRPMRPSessionInformationDto.mrpConsolidatePartForecastJobs,
						mrpGenerated = eRPMRPSessionInformationDto.mrpGenerated,
						mrpIncludePartForecasts = eRPMRPSessionInformationDto.mrpIncludePartForecasts,
						mrpPartClassIDs = eRPMRPSessionInformationDto.mrpPartClassIDs,
						mrpPartGroupIDs = eRPMRPSessionInformationDto.mrpPartGroupIDs,
						mrpPartIDs = eRPMRPSessionInformationDto.mrpPartIDs,
						mrpPlantIDs = eRPMRPSessionInformationDto.mrpPlantIDs,
						mrpRowVersion = eRPMRPSessionInformationDto.mrpRowVersion,
						mrpSessionID = eRPMRPSessionInformationDto.mrpSessionID,
						mrpWarehouseIDs = eRPMRPSessionInformationDto.mrpWarehouseIDs,
						CustomFields = eRPMRPSessionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MRPSession [{mRPSession.mrpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMRPSession(Guid mRPSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPSessionRepository iERPMRPSessionRepository = (base.ERPMRPSessionRepository = new ERPMRPSessionRepository(base.ApiClientContext));
		using (iERPMRPSessionRepository)
		{
			if (!(await base.ERPMRPSessionRepository.DoesMRPSessionExist(mRPSessionId)))
			{
				base.ErrorsList.Add($"MRPSession [{mRPSessionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMRPSessionInformationDto eRPMRPSessionInformationDto = await base.ERPMRPSessionRepository.GetMRPSession(mRPSessionId);
				string text = await base.ERPMRPSessionRepository.WhereUsed("MRPSessions", new object[1] { eRPMRPSessionInformationDto.mrpSessionID }, new object[1] { "mrpSessionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MRPSession cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMRPSessionDto>> Process_DeleteMRPSession(Guid mRPSessionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMRPSessionDto> result;
		try
		{
			IERPMRPSessionRepository iERPMRPSessionRepository = (base.ERPMRPSessionRepository = new ERPMRPSessionRepository(base.ApiClientContext));
			using (iERPMRPSessionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMRPSessionRepository.DeleteRowFromTable("MRPSessions", "mrp", mRPSessionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MRPSession [{mRPSessionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPSessionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMRPSessionDto()
			};
		}
		return result;
	}
}
