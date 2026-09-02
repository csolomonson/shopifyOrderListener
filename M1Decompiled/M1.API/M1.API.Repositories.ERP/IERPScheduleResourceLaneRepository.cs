using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPScheduleResourceLaneRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ScheduleResourceLane with the specified Unique Id exists.
	/// </summary>
	/// <param name="scheduleResourceLaneId">The Unique Id of the ScheduleResourceLane to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ScheduleResourceLane exists or not.</returns>
	Task<bool> DoesScheduleResourceLaneExist(Guid scheduleResourceLaneId);

	/// <summary>
	/// Retrieves all ScheduleResourceLanes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleResourceLanes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleResourceLanes DTOs.</returns>
	Task<ICollection<ERPScheduleResourceLaneInformationDto>> GetAllScheduleResourceLanes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ScheduleResourceLane.
	/// </summary>
	/// <param name="scheduleResourceLaneId">The Unique Id of the ScheduleResourceLane to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ScheduleResourceLane DTO.</returns>
	Task<ERPScheduleResourceLaneInformationDto> GetScheduleResourceLane(Guid scheduleResourceLaneId);
}
