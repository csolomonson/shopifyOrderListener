using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPDocumentLinkRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a DocumentLink with the specified Unique Id exists.
	/// </summary>
	/// <param name="documentLinkId">The Unique Id of the DocumentLink to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the DocumentLink exists or not.</returns>
	Task<bool> DoesDocumentLinkExist(Guid documentLinkId);

	/// <summary>
	/// Retrieves all DocumentLinks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of DocumentLinks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of DocumentLinks DTOs.</returns>
	Task<ICollection<ERPDocumentLinkInformationDto>> GetAllDocumentLinks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific DocumentLink.
	/// </summary>
	/// <param name="documentLinkId">The Unique Id of the DocumentLink to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the DocumentLink DTO.</returns>
	Task<ERPDocumentLinkInformationDto> GetDocumentLink(Guid documentLinkId);

	/// <summary>
	/// Saves the provided ERP documentLink.
	/// </summary>
	/// <param name="documentLink">The ERP documentLink to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveDocumentLink(ERPDocumentLinkDto documentLink);
}
