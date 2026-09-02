using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPScheduleTaskRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ScheduleTask with the specified Unique Id exists.
	/// </summary>
	/// <param name="scheduleTaskId">The Unique Id of the ScheduleTask to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ScheduleTask exists or not.</returns>
	Task<bool> DoesScheduleTaskExist(Guid scheduleTaskId);

	/// <summary>
	/// Retrieves all ScheduleTasks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleTasks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleTasks DTOs.</returns>
	Task<ICollection<ERPScheduleTaskInformationDto>> GetAllScheduleTasks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ScheduleTask.
	/// </summary>
	/// <param name="scheduleTaskId">The Unique Id of the ScheduleTask to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ScheduleTask DTO.</returns>
	Task<ERPScheduleTaskInformationDto> GetScheduleTask(Guid scheduleTaskId);
}
