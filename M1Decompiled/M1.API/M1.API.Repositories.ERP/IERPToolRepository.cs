using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPToolRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Tool with the specified Unique Id exists.
	/// </summary>
	/// <param name="toolId">The Unique Id of the Tool to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Tool exists or not.</returns>
	Task<bool> DoesToolExist(Guid toolId);

	/// <summary>
	/// Retrieves all Tools with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Tools to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Tools DTOs.</returns>
	Task<ICollection<ERPToolInformationDto>> GetAllTools(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Tool.
	/// </summary>
	/// <param name="toolId">The Unique Id of the Tool to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Tool DTO.</returns>
	Task<ERPToolInformationDto> GetTool(Guid toolId);

	/// <summary>
	/// Saves the provided ERP tool.
	/// </summary>
	/// <param name="tool">The ERP tool to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveTool(ERPToolDto tool);
}
