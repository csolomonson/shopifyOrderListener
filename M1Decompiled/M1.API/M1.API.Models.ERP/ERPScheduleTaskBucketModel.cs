using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPScheduleTaskBucketModel : ERPBaseModel, IERPScheduleTaskBucketModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleTaskBuckets(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPScheduleTaskBucketRepository iERPScheduleTaskBucketRepository = (base.ERPScheduleTaskBucketRepository = new ERPScheduleTaskBucketRepository(base.ApiClientContext));
		using (iERPScheduleTaskBucketRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPScheduleTaskBucketRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPScheduleTaskBucketRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPScheduleTaskBucketRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPScheduleTaskBucketRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetScheduleTaskBucket(Guid scheduleTaskBucketId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPScheduleTaskBucketRepository iERPScheduleTaskBucketRepository = (base.ERPScheduleTaskBucketRepository = new ERPScheduleTaskBucketRepository(base.ApiClientContext));
		using (iERPScheduleTaskBucketRepository)
		{
			if (!(await base.ERPScheduleTaskBucketRepository.DoesScheduleTaskBucketExist(scheduleTaskBucketId)))
			{
				errorsList.Add($"ScheduleTaskBucket [{scheduleTaskBucketId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPScheduleTaskBucketDto>>> Process_GetAllScheduleTaskBuckets(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPScheduleTaskBucketDto> allScheduleTaskBucketsDto = new List<ERPScheduleTaskBucketDto>();
		ERPResponseMessageDto<IList<ERPScheduleTaskBucketDto>> result;
		try
		{
			IERPScheduleTaskBucketRepository iERPScheduleTaskBucketRepository = (base.ERPScheduleTaskBucketRepository = new ERPScheduleTaskBucketRepository(base.ApiClientContext));
			using (iERPScheduleTaskBucketRepository)
			{
				foreach (ERPScheduleTaskBucketInformationDto item2 in await base.ERPScheduleTaskBucketRepository.GetAllScheduleTaskBuckets(pageSize, pageNumber, filter, orderBy))
				{
					ERPScheduleTaskBucketDto item = new ERPScheduleTaskBucketDto
					{
						sxeCompletedMinutes = item2.sxeCompletedMinutes,
						sxeUniqueID = item2.sxeUniqueID,
						sxeCompleted = item2.sxeCompleted,
						sxeMinutes = item2.sxeMinutes,
						sxePercentComplete = item2.sxePercentComplete,
						sxeRowVersion = item2.sxeRowVersion,
						sxeScheduleBranchID = item2.sxeScheduleBranchID,
						sxeScheduleTaskID = item2.sxeScheduleTaskID,
						sxeScheduleTreeID = item2.sxeScheduleTreeID,
						sxeScheduleTypeBucketID = item2.sxeScheduleTypeBucketID,
						sxeScheduleTypeID = item2.sxeScheduleTypeID,
						sxeScheduleTaskBucketID = item2.sxeScheduleTaskBucketID,
						CustomFields = item2.CustomFields
					};
					allScheduleTaskBucketsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ScheduleTaskBuckets]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPScheduleTaskBucketDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allScheduleTaskBucketsDto,
				RecordCount = allScheduleTaskBucketsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPScheduleTaskBucketDto>> Process_GetScheduleTaskBucket(Guid scheduleTaskBucketId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPScheduleTaskBucketDto scheduleTaskBucketDto = null;
		ERPResponseMessageDto<ERPScheduleTaskBucketDto> result;
		try
		{
			IERPScheduleTaskBucketRepository iERPScheduleTaskBucketRepository = (base.ERPScheduleTaskBucketRepository = new ERPScheduleTaskBucketRepository(base.ApiClientContext));
			using (iERPScheduleTaskBucketRepository)
			{
				ERPScheduleTaskBucketInformationDto eRPScheduleTaskBucketInformationDto = await base.ERPScheduleTaskBucketRepository.GetScheduleTaskBucket(scheduleTaskBucketId);
				scheduleTaskBucketDto = new ERPScheduleTaskBucketDto
				{
					sxeCompletedMinutes = eRPScheduleTaskBucketInformationDto.sxeCompletedMinutes,
					sxeUniqueID = eRPScheduleTaskBucketInformationDto.sxeUniqueID,
					sxeCompleted = eRPScheduleTaskBucketInformationDto.sxeCompleted,
					sxeMinutes = eRPScheduleTaskBucketInformationDto.sxeMinutes,
					sxePercentComplete = eRPScheduleTaskBucketInformationDto.sxePercentComplete,
					sxeRowVersion = eRPScheduleTaskBucketInformationDto.sxeRowVersion,
					sxeScheduleBranchID = eRPScheduleTaskBucketInformationDto.sxeScheduleBranchID,
					sxeScheduleTaskID = eRPScheduleTaskBucketInformationDto.sxeScheduleTaskID,
					sxeScheduleTreeID = eRPScheduleTaskBucketInformationDto.sxeScheduleTreeID,
					sxeScheduleTypeBucketID = eRPScheduleTaskBucketInformationDto.sxeScheduleTypeBucketID,
					sxeScheduleTypeID = eRPScheduleTaskBucketInformationDto.sxeScheduleTypeID,
					sxeScheduleTaskBucketID = eRPScheduleTaskBucketInformationDto.sxeScheduleTaskBucketID,
					CustomFields = eRPScheduleTaskBucketInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ScheduleTaskBuckets []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPScheduleTaskBucketDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = scheduleTaskBucketDto
			};
		}
		return result;
	}
}
