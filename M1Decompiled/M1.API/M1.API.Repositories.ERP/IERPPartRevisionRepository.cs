using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartRevisionRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartRevision with the specified Unique Id exists.
	/// </summary>
	/// <param name="partRevisionId">The Unique Id of the PartRevision to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartRevision exists or not.</returns>
	Task<bool> DoesPartRevisionExist(Guid partRevisionId);

	/// <summary>
	/// Retrieves all PartRevisions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartRevisions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartRevisions DTOs.</returns>
	Task<ICollection<ERPPartRevisionInformationDto>> GetAllPartRevisions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartRevision.
	/// </summary>
	/// <param name="partRevisionId">The Unique Id of the PartRevision to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartRevision DTO.</returns>
	Task<ERPPartRevisionInformationDto> GetPartRevision(Guid partRevisionId);

	/// <summary>
	/// Saves the provided ERP partRevision.
	/// </summary>
	/// <param name="partRevision">The ERP partRevision to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartRevision(ERPPartRevisionDto partRevision);
}
