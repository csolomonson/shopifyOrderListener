using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPScheduleResourceLaneModel : ERPBaseModel, IERPScheduleResourceLaneModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleResourceLanes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPScheduleResourceLaneRepository iERPScheduleResourceLaneRepository = (base.ERPScheduleResourceLaneRepository = new ERPScheduleResourceLaneRepository(base.ApiClientContext));
		using (iERPScheduleResourceLaneRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPScheduleResourceLaneRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPScheduleResourceLaneRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPScheduleResourceLaneRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPScheduleResourceLaneRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetScheduleResourceLane(Guid scheduleResourceLaneId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPScheduleResourceLaneRepository iERPScheduleResourceLaneRepository = (base.ERPScheduleResourceLaneRepository = new ERPScheduleResourceLaneRepository(base.ApiClientContext));
		using (iERPScheduleResourceLaneRepository)
		{
			if (!(await base.ERPScheduleResourceLaneRepository.DoesScheduleResourceLaneExist(scheduleResourceLaneId)))
			{
				errorsList.Add($"ScheduleResourceLane [{scheduleResourceLaneId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPScheduleResourceLaneDto>>> Process_GetAllScheduleResourceLanes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPScheduleResourceLaneDto> allScheduleResourceLanesDto = new List<ERPScheduleResourceLaneDto>();
		ERPResponseMessageDto<IList<ERPScheduleResourceLaneDto>> result;
		try
		{
			IERPScheduleResourceLaneRepository iERPScheduleResourceLaneRepository = (base.ERPScheduleResourceLaneRepository = new ERPScheduleResourceLaneRepository(base.ApiClientContext));
			using (iERPScheduleResourceLaneRepository)
			{
				foreach (ERPScheduleResourceLaneInformationDto item2 in await base.ERPScheduleResourceLaneRepository.GetAllScheduleResourceLanes(pageSize, pageNumber, filter, orderBy))
				{
					ERPScheduleResourceLaneDto item = new ERPScheduleResourceLaneDto
					{
						sxrUniqueID = item2.sxrUniqueID,
						sxrGroupUniqueID = item2.sxrGroupUniqueID,
						sxrLockedResourceUniqueID = item2.sxrLockedResourceUniqueID,
						sxrResourceType = item2.sxrResourceType,
						sxrRowVersion = item2.sxrRowVersion,
						sxrScheduleBranchID = item2.sxrScheduleBranchID,
						sxrScheduleTaskID = item2.sxrScheduleTaskID,
						sxrScheduleTreeID = item2.sxrScheduleTreeID,
						sxrScheduleResourceLaneID = item2.sxrScheduleResourceLaneID,
						CustomFields = item2.CustomFields
					};
					allScheduleResourceLanesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ScheduleResourceLanes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPScheduleResourceLaneDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allScheduleResourceLanesDto,
				RecordCount = allScheduleResourceLanesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPScheduleResourceLaneDto>> Process_GetScheduleResourceLane(Guid scheduleResourceLaneId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPScheduleResourceLaneDto scheduleResourceLaneDto = null;
		ERPResponseMessageDto<ERPScheduleResourceLaneDto> result;
		try
		{
			IERPScheduleResourceLaneRepository iERPScheduleResourceLaneRepository = (base.ERPScheduleResourceLaneRepository = new ERPScheduleResourceLaneRepository(base.ApiClientContext));
			using (iERPScheduleResourceLaneRepository)
			{
				ERPScheduleResourceLaneInformationDto eRPScheduleResourceLaneInformationDto = await base.ERPScheduleResourceLaneRepository.GetScheduleResourceLane(scheduleResourceLaneId);
				scheduleResourceLaneDto = new ERPScheduleResourceLaneDto
				{
					sxrUniqueID = eRPScheduleResourceLaneInformationDto.sxrUniqueID,
					sxrGroupUniqueID = eRPScheduleResourceLaneInformationDto.sxrGroupUniqueID,
					sxrLockedResourceUniqueID = eRPScheduleResourceLaneInformationDto.sxrLockedResourceUniqueID,
					sxrResourceType = eRPScheduleResourceLaneInformationDto.sxrResourceType,
					sxrRowVersion = eRPScheduleResourceLaneInformationDto.sxrRowVersion,
					sxrScheduleBranchID = eRPScheduleResourceLaneInformationDto.sxrScheduleBranchID,
					sxrScheduleTaskID = eRPScheduleResourceLaneInformationDto.sxrScheduleTaskID,
					sxrScheduleTreeID = eRPScheduleResourceLaneInformationDto.sxrScheduleTreeID,
					sxrScheduleResourceLaneID = eRPScheduleResourceLaneInformationDto.sxrScheduleResourceLaneID,
					CustomFields = eRPScheduleResourceLaneInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ScheduleResourceLanes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPScheduleResourceLaneDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = scheduleResourceLaneDto
			};
		}
		return result;
	}
}
