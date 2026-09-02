using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRecentActivitiesLogRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RecentActivitiesLog with the specified Unique Id exists.
	/// </summary>
	/// <param name="recentActivitiesLogId">The Unique Id of the RecentActivitiesLog to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RecentActivitiesLog exists or not.</returns>
	Task<bool> DoesRecentActivitiesLogExist(Guid recentActivitiesLogId);

	/// <summary>
	/// Retrieves all RecentActivitiesLog with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RecentActivitiesLog to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RecentActivitiesLog DTOs.</returns>
	Task<ICollection<ERPRecentActivitiesLogInformationDto>> GetAllRecentActivitiesLog(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RecentActivitiesLog.
	/// </summary>
	/// <param name="recentActivitiesLogId">The Unique Id of the RecentActivitiesLog to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RecentActivitiesLog DTO.</returns>
	Task<ERPRecentActivitiesLogInformationDto> GetRecentActivitiesLog(Guid recentActivitiesLogId);
}
