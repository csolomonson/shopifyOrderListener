using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPScheduleTreeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ScheduleTree with the specified Unique Id exists.
	/// </summary>
	/// <param name="scheduleTreeId">The Unique Id of the ScheduleTree to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ScheduleTree exists or not.</returns>
	Task<bool> DoesScheduleTreeExist(Guid scheduleTreeId);

	/// <summary>
	/// Retrieves all ScheduleTrees with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleTrees to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleTrees DTOs.</returns>
	Task<ICollection<ERPScheduleTreeInformationDto>> GetAllScheduleTrees(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ScheduleTree.
	/// </summary>
	/// <param name="scheduleTreeId">The Unique Id of the ScheduleTree to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ScheduleTree DTO.</returns>
	Task<ERPScheduleTreeInformationDto> GetScheduleTree(Guid scheduleTreeId);
}
