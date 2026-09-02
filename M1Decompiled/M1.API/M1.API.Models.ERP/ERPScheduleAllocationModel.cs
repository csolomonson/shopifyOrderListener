using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPScheduleAllocationModel : ERPBaseModel, IERPScheduleAllocationModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleAllocations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPScheduleAllocationRepository iERPScheduleAllocationRepository = (base.ERPScheduleAllocationRepository = new ERPScheduleAllocationRepository(base.ApiClientContext));
		using (iERPScheduleAllocationRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPScheduleAllocationRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPScheduleAllocationRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPScheduleAllocationRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPScheduleAllocationRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetScheduleAllocation(Guid scheduleAllocationId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPScheduleAllocationRepository iERPScheduleAllocationRepository = (base.ERPScheduleAllocationRepository = new ERPScheduleAllocationRepository(base.ApiClientContext));
		using (iERPScheduleAllocationRepository)
		{
			if (!(await base.ERPScheduleAllocationRepository.DoesScheduleAllocationExist(scheduleAllocationId)))
			{
				errorsList.Add($"ScheduleAllocation [{scheduleAllocationId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPScheduleAllocationDto>>> Process_GetAllScheduleAllocations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPScheduleAllocationDto> allScheduleAllocationsDto = new List<ERPScheduleAllocationDto>();
		ERPResponseMessageDto<IList<ERPScheduleAllocationDto>> result;
		try
		{
			IERPScheduleAllocationRepository iERPScheduleAllocationRepository = (base.ERPScheduleAllocationRepository = new ERPScheduleAllocationRepository(base.ApiClientContext));
			using (iERPScheduleAllocationRepository)
			{
				foreach (ERPScheduleAllocationInformationDto item2 in await base.ERPScheduleAllocationRepository.GetAllScheduleAllocations(pageSize, pageNumber, filter, orderBy))
				{
					ERPScheduleAllocationDto item = new ERPScheduleAllocationDto
					{
						sxdDateType = item2.sxdDateType,
						sxdEndActualDateTime = item2.sxdEndActualDateTime,
						sxdEndDate = item2.sxdEndDate,
						sxdEndMinute = item2.sxdEndMinute,
						sxdUniqueID = item2.sxdUniqueID,
						sxdGroupUniqueID = item2.sxdGroupUniqueID,
						sxdMinutes = item2.sxdMinutes,
						sxdResourceUniqueID = item2.sxdResourceUniqueID,
						sxdRowVersion = item2.sxdRowVersion,
						sxdScheduleBranchID = item2.sxdScheduleBranchID,
						sxdScheduleResourceLaneID = item2.sxdScheduleResourceLaneID,
						sxdScheduleTaskID = item2.sxdScheduleTaskID,
						sxdScheduleTreeID = item2.sxdScheduleTreeID,
						sxdScheduleAllocationID = item2.sxdScheduleAllocationID,
						sxdStartActualDateTime = item2.sxdStartActualDateTime,
						sxdStartDate = item2.sxdStartDate,
						sxdStartMinute = item2.sxdStartMinute,
						CustomFields = item2.CustomFields
					};
					allScheduleAllocationsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ScheduleAllocations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPScheduleAllocationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allScheduleAllocationsDto,
				RecordCount = allScheduleAllocationsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPScheduleAllocationDto>> Process_GetScheduleAllocation(Guid scheduleAllocationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPScheduleAllocationDto scheduleAllocationDto = null;
		ERPResponseMessageDto<ERPScheduleAllocationDto> result;
		try
		{
			IERPScheduleAllocationRepository iERPScheduleAllocationRepository = (base.ERPScheduleAllocationRepository = new ERPScheduleAllocationRepository(base.ApiClientContext));
			using (iERPScheduleAllocationRepository)
			{
				ERPScheduleAllocationInformationDto eRPScheduleAllocationInformationDto = await base.ERPScheduleAllocationRepository.GetScheduleAllocation(scheduleAllocationId);
				scheduleAllocationDto = new ERPScheduleAllocationDto
				{
					sxdDateType = eRPScheduleAllocationInformationDto.sxdDateType,
					sxdEndActualDateTime = eRPScheduleAllocationInformationDto.sxdEndActualDateTime,
					sxdEndDate = eRPScheduleAllocationInformationDto.sxdEndDate,
					sxdEndMinute = eRPScheduleAllocationInformationDto.sxdEndMinute,
					sxdUniqueID = eRPScheduleAllocationInformationDto.sxdUniqueID,
					sxdGroupUniqueID = eRPScheduleAllocationInformationDto.sxdGroupUniqueID,
					sxdMinutes = eRPScheduleAllocationInformationDto.sxdMinutes,
					sxdResourceUniqueID = eRPScheduleAllocationInformationDto.sxdResourceUniqueID,
					sxdRowVersion = eRPScheduleAllocationInformationDto.sxdRowVersion,
					sxdScheduleBranchID = eRPScheduleAllocationInformationDto.sxdScheduleBranchID,
					sxdScheduleResourceLaneID = eRPScheduleAllocationInformationDto.sxdScheduleResourceLaneID,
					sxdScheduleTaskID = eRPScheduleAllocationInformationDto.sxdScheduleTaskID,
					sxdScheduleTreeID = eRPScheduleAllocationInformationDto.sxdScheduleTreeID,
					sxdScheduleAllocationID = eRPScheduleAllocationInformationDto.sxdScheduleAllocationID,
					sxdStartActualDateTime = eRPScheduleAllocationInformationDto.sxdStartActualDateTime,
					sxdStartDate = eRPScheduleAllocationInformationDto.sxdStartDate,
					sxdStartMinute = eRPScheduleAllocationInformationDto.sxdStartMinute,
					CustomFields = eRPScheduleAllocationInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ScheduleAllocations []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPScheduleAllocationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = scheduleAllocationDto
			};
		}
		return result;
	}
}
