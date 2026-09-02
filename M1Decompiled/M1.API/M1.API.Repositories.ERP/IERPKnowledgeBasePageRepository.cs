using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPKnowledgeBasePageRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a KnowledgeBasePage with the specified Unique Id exists.
	/// </summary>
	/// <param name="knowledgeBasePageId">The Unique Id of the KnowledgeBasePage to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the KnowledgeBasePage exists or not.</returns>
	Task<bool> DoesKnowledgeBasePageExist(Guid knowledgeBasePageId);

	/// <summary>
	/// Retrieves all KnowledgeBasePages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of KnowledgeBasePages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of KnowledgeBasePages DTOs.</returns>
	Task<ICollection<ERPKnowledgeBasePageInformationDto>> GetAllKnowledgeBasePages(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific KnowledgeBasePage.
	/// </summary>
	/// <param name="knowledgeBasePageId">The Unique Id of the KnowledgeBasePage to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the KnowledgeBasePage DTO.</returns>
	Task<ERPKnowledgeBasePageInformationDto> GetKnowledgeBasePage(Guid knowledgeBasePageId);

	/// <summary>
	/// Saves the provided ERP knowledgeBasePage.
	/// </summary>
	/// <param name="knowledgeBasePage">The ERP knowledgeBasePage to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveKnowledgeBasePage(ERPKnowledgeBasePageDto knowledgeBasePage);
}
