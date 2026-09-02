using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPChangeLogRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ChangeLog with the specified Unique Id exists.
	/// </summary>
	/// <param name="changeLogId">The Unique Id of the ChangeLog to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ChangeLog exists or not.</returns>
	Task<bool> DoesChangeLogExist(Guid changeLogId);

	/// <summary>
	/// Retrieves all ChangeLog with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeLog to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ChangeLog DTOs.</returns>
	Task<ICollection<ERPChangeLogInformationDto>> GetAllChangeLog(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ChangeLog.
	/// </summary>
	/// <param name="changeLogId">The Unique Id of the ChangeLog to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ChangeLog DTO.</returns>
	Task<ERPChangeLogInformationDto> GetChangeLog(Guid changeLogId);
}
