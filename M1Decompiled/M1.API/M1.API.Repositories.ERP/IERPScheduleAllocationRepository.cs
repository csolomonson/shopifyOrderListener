using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPScheduleAllocationRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ScheduleAllocation with the specified Unique Id exists.
	/// </summary>
	/// <param name="scheduleAllocationId">The Unique Id of the ScheduleAllocation to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ScheduleAllocation exists or not.</returns>
	Task<bool> DoesScheduleAllocationExist(Guid scheduleAllocationId);

	/// <summary>
	/// Retrieves all ScheduleAllocations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleAllocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleAllocations DTOs.</returns>
	Task<ICollection<ERPScheduleAllocationInformationDto>> GetAllScheduleAllocations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ScheduleAllocation.
	/// </summary>
	/// <param name="scheduleAllocationId">The Unique Id of the ScheduleAllocation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ScheduleAllocation DTO.</returns>
	Task<ERPScheduleAllocationInformationDto> GetScheduleAllocation(Guid scheduleAllocationId);
}
