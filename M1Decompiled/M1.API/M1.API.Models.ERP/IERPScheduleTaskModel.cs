using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPScheduleTaskModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ScheduleTasks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleTasks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleTasks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ScheduleTask information based on the specified ScheduleTask Unique Id.
	/// </summary>
	/// <param name="scheduleTaskId">The Unique Id of the ScheduleTask.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetScheduleTask(Guid scheduleTaskId);

	/// <summary>
	/// Processes the request to retrieve all ScheduleTasks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleTasks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleTasks DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPScheduleTaskDto>>> Process_GetAllScheduleTasks(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ScheduleTask.
	/// </summary>
	/// <param name="scheduleTaskId">The Unique Id of the ScheduleTask to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ScheduleTask DTO.</returns>
	Task<ERPResponseMessageDto<ERPScheduleTaskDto>> Process_GetScheduleTask(Guid scheduleTaskId);
}
