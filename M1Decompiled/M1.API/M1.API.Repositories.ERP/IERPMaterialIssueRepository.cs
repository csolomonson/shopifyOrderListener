using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPMaterialIssueRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a MaterialIssue with the specified Unique Id exists.
	/// </summary>
	/// <param name="materialIssueId">The Unique Id of the MaterialIssue to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the MaterialIssue exists or not.</returns>
	Task<bool> DoesMaterialIssueExist(Guid materialIssueId);

	/// <summary>
	/// Retrieves all MaterialIssues with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MaterialIssues to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MaterialIssues DTOs.</returns>
	Task<ICollection<ERPMaterialIssueInformationDto>> GetAllMaterialIssues(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific MaterialIssue.
	/// </summary>
	/// <param name="materialIssueId">The Unique Id of the MaterialIssue to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the MaterialIssue DTO.</returns>
	Task<ERPMaterialIssueInformationDto> GetMaterialIssue(Guid materialIssueId);

	/// <summary>
	/// Saves the provided ERP materialIssue.
	/// </summary>
	/// <param name="materialIssue">The ERP materialIssue to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveMaterialIssue(ERPMaterialIssueDto materialIssue);
}
