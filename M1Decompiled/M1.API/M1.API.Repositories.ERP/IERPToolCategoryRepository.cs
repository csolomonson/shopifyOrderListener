using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPToolCategoryRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ToolCategory with the specified Unique Id exists.
	/// </summary>
	/// <param name="toolCategoryId">The Unique Id of the ToolCategory to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ToolCategory exists or not.</returns>
	Task<bool> DoesToolCategoryExist(Guid toolCategoryId);

	/// <summary>
	/// Retrieves all ToolCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ToolCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ToolCategories DTOs.</returns>
	Task<ICollection<ERPToolCategoryInformationDto>> GetAllToolCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ToolCategory.
	/// </summary>
	/// <param name="toolCategoryId">The Unique Id of the ToolCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ToolCategory DTO.</returns>
	Task<ERPToolCategoryInformationDto> GetToolCategory(Guid toolCategoryId);
}
