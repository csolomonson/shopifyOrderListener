using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;

namespace M1.API.Repositories.Core.Transaction;

public interface IMaterialIssueLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Retrieves all material issue lines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of material issue lines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a collection of material issue line information DTOs.</returns>
	Task<ICollection<MaterialIssueLineInformationDto>> GetAllMaterialIssueLines(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific material issue.
	/// </summary>
	/// <param name="materialIssueId">The ID of the material issue to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the material issue information DTO.</returns>
	Task<MaterialIssueDto> GetMaterialIssueInfo(string materialIssueId);

	/// <summary>
	/// Retrieves detailed line information about a specific material issue.
	/// </summary>
	/// <param name="materialIssueId">The ID of the material issue to retrieve line information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a list of material issue line information DTOs.</returns>
	Task<IList<MaterialIssueLineInformationDto>> GetMaterialIssueLineInfo(string materialIssueId);
}
