using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPReasonRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Reason with the specified Unique Id exists.
	/// </summary>
	/// <param name="reasonId">The Unique Id of the Reason to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Reason exists or not.</returns>
	Task<bool> DoesReasonExist(Guid reasonId);

	/// <summary>
	/// Retrieves all Reasons with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Reasons to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Reasons DTOs.</returns>
	Task<ICollection<ERPReasonInformationDto>> GetAllReasons(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Reason.
	/// </summary>
	/// <param name="reasonId">The Unique Id of the Reason to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Reason DTO.</returns>
	Task<ERPReasonInformationDto> GetReason(Guid reasonId);
}
