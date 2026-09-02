using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPScheduleAllocationModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ScheduleAllocations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleAllocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleAllocations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ScheduleAllocation information based on the specified ScheduleAllocation Unique Id.
	/// </summary>
	/// <param name="scheduleAllocationId">The Unique Id of the ScheduleAllocation.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetScheduleAllocation(Guid scheduleAllocationId);

	/// <summary>
	/// Processes the request to retrieve all ScheduleAllocations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleAllocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleAllocations DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPScheduleAllocationDto>>> Process_GetAllScheduleAllocations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ScheduleAllocation.
	/// </summary>
	/// <param name="scheduleAllocationId">The Unique Id of the ScheduleAllocation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ScheduleAllocation DTO.</returns>
	Task<ERPResponseMessageDto<ERPScheduleAllocationDto>> Process_GetScheduleAllocation(Guid scheduleAllocationId);
}
