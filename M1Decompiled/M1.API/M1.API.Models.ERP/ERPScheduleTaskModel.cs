using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPScheduleTaskModel : ERPBaseModel, IERPScheduleTaskModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleTasks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPScheduleTaskRepository iERPScheduleTaskRepository = (base.ERPScheduleTaskRepository = new ERPScheduleTaskRepository(base.ApiClientContext));
		using (iERPScheduleTaskRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPScheduleTaskRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPScheduleTaskRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPScheduleTaskRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPScheduleTaskRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetScheduleTask(Guid scheduleTaskId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPScheduleTaskRepository iERPScheduleTaskRepository = (base.ERPScheduleTaskRepository = new ERPScheduleTaskRepository(base.ApiClientContext));
		using (iERPScheduleTaskRepository)
		{
			if (!(await base.ERPScheduleTaskRepository.DoesScheduleTaskExist(scheduleTaskId)))
			{
				errorsList.Add($"ScheduleTask [{scheduleTaskId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPScheduleTaskDto>>> Process_GetAllScheduleTasks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPScheduleTaskDto> allScheduleTasksDto = new List<ERPScheduleTaskDto>();
		ERPResponseMessageDto<IList<ERPScheduleTaskDto>> result;
		try
		{
			IERPScheduleTaskRepository iERPScheduleTaskRepository = (base.ERPScheduleTaskRepository = new ERPScheduleTaskRepository(base.ApiClientContext));
			using (iERPScheduleTaskRepository)
			{
				foreach (ERPScheduleTaskInformationDto item2 in await base.ERPScheduleTaskRepository.GetAllScheduleTasks(pageSize, pageNumber, filter, orderBy))
				{
					ERPScheduleTaskDto item = new ERPScheduleTaskDto
					{
						sxkCreatedBy = item2.sxkCreatedBy,
						sxkCreatedDate = item2.sxkCreatedDate,
						sxkCurrentTaskDateType = item2.sxkCurrentTaskDateType,
						sxkEndActualDateTime = item2.sxkEndActualDateTime,
						sxkEndDate = item2.sxkEndDate,
						sxkEndMinute = item2.sxkEndMinute,
						sxkUniqueID = item2.sxkUniqueID,
						sxkExchangeID = item2.sxkExchangeID,
						sxkLinkedTaskDateType = item2.sxkLinkedTaskDateType,
						sxkLinkedTaskID = item2.sxkLinkedTaskID,
						sxkMinutes = item2.sxkMinutes,
						sxkOffsetMinutes = item2.sxkOffsetMinutes,
						sxkPlantDepartmentID = item2.sxkPlantDepartmentID,
						sxkPlantID = item2.sxkPlantID,
						sxkProcessID = item2.sxkProcessID,
						sxkRowVersion = item2.sxkRowVersion,
						sxkScheduleBranchID = item2.sxkScheduleBranchID,
						sxkScheduleTreeID = item2.sxkScheduleTreeID,
						sxkScheduleTypeID = item2.sxkScheduleTypeID,
						sxkScheduleTaskID = item2.sxkScheduleTaskID,
						sxkStartActualDateTime = item2.sxkStartActualDateTime,
						sxkStartDate = item2.sxkStartDate,
						sxkStartMinute = item2.sxkStartMinute,
						CustomFields = item2.CustomFields
					};
					allScheduleTasksDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ScheduleTasks]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPScheduleTaskDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allScheduleTasksDto,
				RecordCount = allScheduleTasksDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPScheduleTaskDto>> Process_GetScheduleTask(Guid scheduleTaskId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPScheduleTaskDto scheduleTaskDto = null;
		ERPResponseMessageDto<ERPScheduleTaskDto> result;
		try
		{
			IERPScheduleTaskRepository iERPScheduleTaskRepository = (base.ERPScheduleTaskRepository = new ERPScheduleTaskRepository(base.ApiClientContext));
			using (iERPScheduleTaskRepository)
			{
				ERPScheduleTaskInformationDto eRPScheduleTaskInformationDto = await base.ERPScheduleTaskRepository.GetScheduleTask(scheduleTaskId);
				scheduleTaskDto = new ERPScheduleTaskDto
				{
					sxkCreatedBy = eRPScheduleTaskInformationDto.sxkCreatedBy,
					sxkCreatedDate = eRPScheduleTaskInformationDto.sxkCreatedDate,
					sxkCurrentTaskDateType = eRPScheduleTaskInformationDto.sxkCurrentTaskDateType,
					sxkEndActualDateTime = eRPScheduleTaskInformationDto.sxkEndActualDateTime,
					sxkEndDate = eRPScheduleTaskInformationDto.sxkEndDate,
					sxkEndMinute = eRPScheduleTaskInformationDto.sxkEndMinute,
					sxkUniqueID = eRPScheduleTaskInformationDto.sxkUniqueID,
					sxkExchangeID = eRPScheduleTaskInformationDto.sxkExchangeID,
					sxkLinkedTaskDateType = eRPScheduleTaskInformationDto.sxkLinkedTaskDateType,
					sxkLinkedTaskID = eRPScheduleTaskInformationDto.sxkLinkedTaskID,
					sxkMinutes = eRPScheduleTaskInformationDto.sxkMinutes,
					sxkOffsetMinutes = eRPScheduleTaskInformationDto.sxkOffsetMinutes,
					sxkPlantDepartmentID = eRPScheduleTaskInformationDto.sxkPlantDepartmentID,
					sxkPlantID = eRPScheduleTaskInformationDto.sxkPlantID,
					sxkProcessID = eRPScheduleTaskInformationDto.sxkProcessID,
					sxkRowVersion = eRPScheduleTaskInformationDto.sxkRowVersion,
					sxkScheduleBranchID = eRPScheduleTaskInformationDto.sxkScheduleBranchID,
					sxkScheduleTreeID = eRPScheduleTaskInformationDto.sxkScheduleTreeID,
					sxkScheduleTypeID = eRPScheduleTaskInformationDto.sxkScheduleTypeID,
					sxkScheduleTaskID = eRPScheduleTaskInformationDto.sxkScheduleTaskID,
					sxkStartActualDateTime = eRPScheduleTaskInformationDto.sxkStartActualDateTime,
					sxkStartDate = eRPScheduleTaskInformationDto.sxkStartDate,
					sxkStartMinute = eRPScheduleTaskInformationDto.sxkStartMinute,
					CustomFields = eRPScheduleTaskInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ScheduleTasks []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPScheduleTaskDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = scheduleTaskDto
			};
		}
		return result;
	}
}
