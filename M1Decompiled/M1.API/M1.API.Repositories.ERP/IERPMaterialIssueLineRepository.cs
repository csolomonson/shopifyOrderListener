using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPMaterialIssueLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a MaterialIssueLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="materialIssueLineId">The Unique Id of the MaterialIssueLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the MaterialIssueLine exists or not.</returns>
	Task<bool> DoesMaterialIssueLineExist(Guid materialIssueLineId);

	/// <summary>
	/// Retrieves all MaterialIssueLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of MaterialIssueLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of MaterialIssueLines DTOs.</returns>
	Task<ICollection<ERPMaterialIssueLineInformationDto>> GetAllMaterialIssueLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific MaterialIssueLine.
	/// </summary>
	/// <param name="materialIssueLineId">The Unique Id of the MaterialIssueLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the MaterialIssueLine DTO.</returns>
	Task<ERPMaterialIssueLineInformationDto> GetMaterialIssueLine(Guid materialIssueLineId);

	/// <summary>
	/// Saves the provided ERP materialIssueLine.
	/// </summary>
	/// <param name="materialIssueLine">The ERP materialIssueLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveMaterialIssueLine(ERPMaterialIssueLineDto materialIssueLine);
}
