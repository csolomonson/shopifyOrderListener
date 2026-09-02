using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPChangeRequestGroupLinkRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ChangeRequestGroupLink with the specified Unique Id exists.
	/// </summary>
	/// <param name="changeRequestGroupLinkId">The Unique Id of the ChangeRequestGroupLink to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ChangeRequestGroupLink exists or not.</returns>
	Task<bool> DoesChangeRequestGroupLinkExist(Guid changeRequestGroupLinkId);

	/// <summary>
	/// Retrieves all ChangeRequestGroupLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeRequestGroupLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ChangeRequestGroupLinks DTOs.</returns>
	Task<ICollection<ERPChangeRequestGroupLinkInformationDto>> GetAllChangeRequestGroupLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ChangeRequestGroupLink.
	/// </summary>
	/// <param name="changeRequestGroupLinkId">The Unique Id of the ChangeRequestGroupLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ChangeRequestGroupLink DTO.</returns>
	Task<ERPChangeRequestGroupLinkInformationDto> GetChangeRequestGroupLink(Guid changeRequestGroupLinkId);

	/// <summary>
	/// Saves the provided ERP changeRequestGroupLink.
	/// </summary>
	/// <param name="changeRequestGroupLink">The ERP changeRequestGroupLink to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveChangeRequestGroupLink(ERPChangeRequestGroupLinkDto changeRequestGroupLink);
}
