using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPToolMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ToolMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="toolMemoId">The Unique Id of the ToolMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ToolMemo exists or not.</returns>
	Task<bool> DoesToolMemoExist(Guid toolMemoId);

	/// <summary>
	/// Retrieves all ToolMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ToolMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ToolMemos DTOs.</returns>
	Task<ICollection<ERPToolMemoInformationDto>> GetAllToolMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ToolMemo.
	/// </summary>
	/// <param name="toolMemoId">The Unique Id of the ToolMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ToolMemo DTO.</returns>
	Task<ERPToolMemoInformationDto> GetToolMemo(Guid toolMemoId);

	/// <summary>
	/// Saves the provided ERP toolMemo.
	/// </summary>
	/// <param name="toolMemo">The ERP toolMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveToolMemo(ERPToolMemoDto toolMemo);
}
