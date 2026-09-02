using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRecentActivitiesLogModel : ERPBaseModel, IERPRecentActivitiesLogModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRecentActivitiesLog(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRecentActivitiesLogRepository iERPRecentActivitiesLogRepository = (base.ERPRecentActivitiesLogRepository = new ERPRecentActivitiesLogRepository(base.ApiClientContext));
		using (iERPRecentActivitiesLogRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRecentActivitiesLogRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRecentActivitiesLogRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRecentActivitiesLogRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRecentActivitiesLogRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRecentActivitiesLog(Guid recentActivitiesLogId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRecentActivitiesLogRepository iERPRecentActivitiesLogRepository = (base.ERPRecentActivitiesLogRepository = new ERPRecentActivitiesLogRepository(base.ApiClientContext));
		using (iERPRecentActivitiesLogRepository)
		{
			if (!(await base.ERPRecentActivitiesLogRepository.DoesRecentActivitiesLogExist(recentActivitiesLogId)))
			{
				errorsList.Add($"RecentActivitiesLog [{recentActivitiesLogId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRecentActivitiesLogDto>>> Process_GetAllRecentActivitiesLog(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRecentActivitiesLogDto> allRecentActivitiesLogDto = new List<ERPRecentActivitiesLogDto>();
		ERPResponseMessageDto<IList<ERPRecentActivitiesLogDto>> result;
		try
		{
			IERPRecentActivitiesLogRepository iERPRecentActivitiesLogRepository = (base.ERPRecentActivitiesLogRepository = new ERPRecentActivitiesLogRepository(base.ApiClientContext));
			using (iERPRecentActivitiesLogRepository)
			{
				foreach (ERPRecentActivitiesLogInformationDto item2 in await base.ERPRecentActivitiesLogRepository.GetAllRecentActivitiesLog(pageSize, pageNumber, filter, orderBy))
				{
					ERPRecentActivitiesLogDto item = new ERPRecentActivitiesLogDto
					{
						rtlCount = item2.rtlCount,
						rtlExplorerType = item2.rtlExplorerType,
						rtlLastOpenedDateTime = item2.rtlLastOpenedDateTime,
						rtlObjectDataRun = item2.rtlObjectDataRun,
						rtlObjectID = item2.rtlObjectID,
						rtlObjectName = item2.rtlObjectName,
						rtlParentKey = item2.rtlParentKey,
						rtlRecentActivityID = item2.rtlRecentActivityID,
						rtlRowVersion = item2.rtlRowVersion,
						rtlUserID = item2.rtlUserID,
						CustomFields = item2.CustomFields
					};
					allRecentActivitiesLogDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RecentActivitiesLog]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRecentActivitiesLogDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRecentActivitiesLogDto,
				RecordCount = allRecentActivitiesLogDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRecentActivitiesLogDto>> Process_GetRecentActivitiesLog(Guid recentActivitiesLogId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRecentActivitiesLogDto recentActivitiesLogDto = null;
		ERPResponseMessageDto<ERPRecentActivitiesLogDto> result;
		try
		{
			IERPRecentActivitiesLogRepository iERPRecentActivitiesLogRepository = (base.ERPRecentActivitiesLogRepository = new ERPRecentActivitiesLogRepository(base.ApiClientContext));
			using (iERPRecentActivitiesLogRepository)
			{
				ERPRecentActivitiesLogInformationDto eRPRecentActivitiesLogInformationDto = await base.ERPRecentActivitiesLogRepository.GetRecentActivitiesLog(recentActivitiesLogId);
				recentActivitiesLogDto = new ERPRecentActivitiesLogDto
				{
					rtlCount = eRPRecentActivitiesLogInformationDto.rtlCount,
					rtlExplorerType = eRPRecentActivitiesLogInformationDto.rtlExplorerType,
					rtlLastOpenedDateTime = eRPRecentActivitiesLogInformationDto.rtlLastOpenedDateTime,
					rtlObjectDataRun = eRPRecentActivitiesLogInformationDto.rtlObjectDataRun,
					rtlObjectID = eRPRecentActivitiesLogInformationDto.rtlObjectID,
					rtlObjectName = eRPRecentActivitiesLogInformationDto.rtlObjectName,
					rtlParentKey = eRPRecentActivitiesLogInformationDto.rtlParentKey,
					rtlRecentActivityID = eRPRecentActivitiesLogInformationDto.rtlRecentActivityID,
					rtlRowVersion = eRPRecentActivitiesLogInformationDto.rtlRowVersion,
					rtlUserID = eRPRecentActivitiesLogInformationDto.rtlUserID,
					CustomFields = eRPRecentActivitiesLogInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RecentActivitiesLog []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRecentActivitiesLogDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = recentActivitiesLogDto
			};
		}
		return result;
	}
}
