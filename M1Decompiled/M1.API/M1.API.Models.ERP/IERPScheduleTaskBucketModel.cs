using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPScheduleTaskBucketModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ScheduleTaskBuckets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleTaskBuckets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleTaskBuckets(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ScheduleTaskBucket information based on the specified ScheduleTaskBucket Unique Id.
	/// </summary>
	/// <param name="scheduleTaskBucketId">The Unique Id of the ScheduleTaskBucket.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetScheduleTaskBucket(Guid scheduleTaskBucketId);

	/// <summary>
	/// Processes the request to retrieve all ScheduleTaskBuckets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleTaskBuckets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleTaskBuckets DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPScheduleTaskBucketDto>>> Process_GetAllScheduleTaskBuckets(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ScheduleTaskBucket.
	/// </summary>
	/// <param name="scheduleTaskBucketId">The Unique Id of the ScheduleTaskBucket to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ScheduleTaskBucket DTO.</returns>
	Task<ERPResponseMessageDto<ERPScheduleTaskBucketDto>> Process_GetScheduleTaskBucket(Guid scheduleTaskBucketId);
}
