using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPTopActivitiesLogModel : ERPBaseModel, IERPTopActivitiesLogModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllTopActivitiesLog(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPTopActivitiesLogRepository iERPTopActivitiesLogRepository = (base.ERPTopActivitiesLogRepository = new ERPTopActivitiesLogRepository(base.ApiClientContext));
		using (iERPTopActivitiesLogRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPTopActivitiesLogRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPTopActivitiesLogRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPTopActivitiesLogRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPTopActivitiesLogRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetTopActivitiesLog(Guid topActivitiesLogId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTopActivitiesLogRepository iERPTopActivitiesLogRepository = (base.ERPTopActivitiesLogRepository = new ERPTopActivitiesLogRepository(base.ApiClientContext));
		using (iERPTopActivitiesLogRepository)
		{
			if (!(await base.ERPTopActivitiesLogRepository.DoesTopActivitiesLogExist(topActivitiesLogId)))
			{
				errorsList.Add($"TopActivitiesLog [{topActivitiesLogId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPTopActivitiesLogDto>>> Process_GetAllTopActivitiesLog(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPTopActivitiesLogDto> allTopActivitiesLogDto = new List<ERPTopActivitiesLogDto>();
		ERPResponseMessageDto<IList<ERPTopActivitiesLogDto>> result;
		try
		{
			IERPTopActivitiesLogRepository iERPTopActivitiesLogRepository = (base.ERPTopActivitiesLogRepository = new ERPTopActivitiesLogRepository(base.ApiClientContext));
			using (iERPTopActivitiesLogRepository)
			{
				foreach (ERPTopActivitiesLogInformationDto item2 in await base.ERPTopActivitiesLogRepository.GetAllTopActivitiesLog(pageSize, pageNumber, filter, orderBy))
				{
					ERPTopActivitiesLogDto item = new ERPTopActivitiesLogDto
					{
						rxlCount = item2.rxlCount,
						rxlExplorerType = item2.rxlExplorerType,
						rxlGridID = item2.rxlGridID,
						rxlObjectDataRun = item2.rxlObjectDataRun,
						rxlObjectName = item2.rxlObjectName,
						rxlProcessedDateTime = item2.rxlProcessedDateTime,
						rxlRowVersion = item2.rxlRowVersion,
						rxlTopActivityID = item2.rxlTopActivityID,
						rxlUserID = item2.rxlUserID,
						rxlVisualizerID = item2.rxlVisualizerID,
						rxlVisualizerType = item2.rxlVisualizerType,
						CustomFields = item2.CustomFields
					};
					allTopActivitiesLogDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all TopActivitiesLog]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPTopActivitiesLogDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allTopActivitiesLogDto,
				RecordCount = allTopActivitiesLogDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTopActivitiesLogDto>> Process_GetTopActivitiesLog(Guid topActivitiesLogId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPTopActivitiesLogDto topActivitiesLogDto = null;
		ERPResponseMessageDto<ERPTopActivitiesLogDto> result;
		try
		{
			IERPTopActivitiesLogRepository iERPTopActivitiesLogRepository = (base.ERPTopActivitiesLogRepository = new ERPTopActivitiesLogRepository(base.ApiClientContext));
			using (iERPTopActivitiesLogRepository)
			{
				ERPTopActivitiesLogInformationDto eRPTopActivitiesLogInformationDto = await base.ERPTopActivitiesLogRepository.GetTopActivitiesLog(topActivitiesLogId);
				topActivitiesLogDto = new ERPTopActivitiesLogDto
				{
					rxlCount = eRPTopActivitiesLogInformationDto.rxlCount,
					rxlExplorerType = eRPTopActivitiesLogInformationDto.rxlExplorerType,
					rxlGridID = eRPTopActivitiesLogInformationDto.rxlGridID,
					rxlObjectDataRun = eRPTopActivitiesLogInformationDto.rxlObjectDataRun,
					rxlObjectName = eRPTopActivitiesLogInformationDto.rxlObjectName,
					rxlProcessedDateTime = eRPTopActivitiesLogInformationDto.rxlProcessedDateTime,
					rxlRowVersion = eRPTopActivitiesLogInformationDto.rxlRowVersion,
					rxlTopActivityID = eRPTopActivitiesLogInformationDto.rxlTopActivityID,
					rxlUserID = eRPTopActivitiesLogInformationDto.rxlUserID,
					rxlVisualizerID = eRPTopActivitiesLogInformationDto.rxlVisualizerID,
					rxlVisualizerType = eRPTopActivitiesLogInformationDto.rxlVisualizerType,
					CustomFields = eRPTopActivitiesLogInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the TopActivitiesLog []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTopActivitiesLogDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = topActivitiesLogDto
			};
		}
		return result;
	}
}
