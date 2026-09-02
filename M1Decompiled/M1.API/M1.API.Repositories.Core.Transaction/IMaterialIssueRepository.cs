using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;

namespace M1.API.Repositories.Core.Transaction;

public interface IMaterialIssueRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a material issue with the specified ID exists.
	/// </summary>
	/// <param name="materialIssueId">The ID of the material issue to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the material issue exists or not.</returns>
	Task<bool> DoesMaterialIssueExists(string materialIssueId);

	/// <summary>
	/// Retrieves detailed information about a specific material issue.
	/// </summary>
	/// <param name="materialIssueId">The ID of the material issue to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the material issue DTO.</returns>
	Task<MaterialIssueDto> GetMaterialIssue(string materialIssueId);

	/// <summary>
	/// Retrieves all material issues with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of material issues to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a collection of material issue DTOs.</returns>
	Task<ICollection<MaterialIssueDto>> GetAllMaterialIssues(int? pageSize = null, int? pageNumber = null);
}
