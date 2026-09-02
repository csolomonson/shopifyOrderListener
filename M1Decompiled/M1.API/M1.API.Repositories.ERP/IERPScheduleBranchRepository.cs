using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPScheduleBranchRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ScheduleBranch with the specified Unique Id exists.
	/// </summary>
	/// <param name="scheduleBranchId">The Unique Id of the ScheduleBranch to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ScheduleBranch exists or not.</returns>
	Task<bool> DoesScheduleBranchExist(Guid scheduleBranchId);

	/// <summary>
	/// Retrieves all ScheduleBranches with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ScheduleBranches to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ScheduleBranches DTOs.</returns>
	Task<ICollection<ERPScheduleBranchInformationDto>> GetAllScheduleBranches(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ScheduleBranch.
	/// </summary>
	/// <param name="scheduleBranchId">The Unique Id of the ScheduleBranch to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ScheduleBranch DTO.</returns>
	Task<ERPScheduleBranchInformationDto> GetScheduleBranch(Guid scheduleBranchId);
}
