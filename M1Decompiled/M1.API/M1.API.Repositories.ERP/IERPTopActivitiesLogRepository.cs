using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPTopActivitiesLogRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a TopActivitiesLog with the specified Unique Id exists.
	/// </summary>
	/// <param name="topActivitiesLogId">The Unique Id of the TopActivitiesLog to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the TopActivitiesLog exists or not.</returns>
	Task<bool> DoesTopActivitiesLogExist(Guid topActivitiesLogId);

	/// <summary>
	/// Retrieves all TopActivitiesLog with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TopActivitiesLog to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of TopActivitiesLog DTOs.</returns>
	Task<ICollection<ERPTopActivitiesLogInformationDto>> GetAllTopActivitiesLog(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific TopActivitiesLog.
	/// </summary>
	/// <param name="topActivitiesLogId">The Unique Id of the TopActivitiesLog to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the TopActivitiesLog DTO.</returns>
	Task<ERPTopActivitiesLogInformationDto> GetTopActivitiesLog(Guid topActivitiesLogId);
}
