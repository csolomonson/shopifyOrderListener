using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPScheduleResourceLaneModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ScheduleResourceLanes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleResourceLanes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleResourceLanes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ScheduleResourceLane information based on the specified ScheduleResourceLane Unique Id.
	/// </summary>
	/// <param name="scheduleResourceLaneId">The Unique Id of the ScheduleResourceLane.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetScheduleResourceLane(Guid scheduleResourceLaneId);

	/// <summary>
	/// Processes the request to retrieve all ScheduleResourceLanes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleResourceLanes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleResourceLanes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPScheduleResourceLaneDto>>> Process_GetAllScheduleResourceLanes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ScheduleResourceLane.
	/// </summary>
	/// <param name="scheduleResourceLaneId">The Unique Id of the ScheduleResourceLane to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ScheduleResourceLane DTO.</returns>
	Task<ERPResponseMessageDto<ERPScheduleResourceLaneDto>> Process_GetScheduleResourceLane(Guid scheduleResourceLaneId);
}
