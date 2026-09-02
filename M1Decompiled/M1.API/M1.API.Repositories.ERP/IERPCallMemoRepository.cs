using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPCallMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a CallMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="callMemoId">The Unique Id of the CallMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the CallMemo exists or not.</returns>
	Task<bool> DoesCallMemoExist(Guid callMemoId);

	/// <summary>
	/// Retrieves all CallMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CallMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CallMemos DTOs.</returns>
	Task<ICollection<ERPCallMemoInformationDto>> GetAllCallMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific CallMemo.
	/// </summary>
	/// <param name="callMemoId">The Unique Id of the CallMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the CallMemo DTO.</returns>
	Task<ERPCallMemoInformationDto> GetCallMemo(Guid callMemoId);

	/// <summary>
	/// Saves the provided ERP callMemo.
	/// </summary>
	/// <param name="callMemo">The ERP callMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveCallMemo(ERPCallMemoDto callMemo);
}
