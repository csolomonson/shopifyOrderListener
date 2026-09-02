using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="partMemoId">The Unique Id of the PartMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartMemo exists or not.</returns>
	Task<bool> DoesPartMemoExist(Guid partMemoId);

	/// <summary>
	/// Retrieves all PartMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartMemos DTOs.</returns>
	Task<ICollection<ERPPartMemoInformationDto>> GetAllPartMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartMemo.
	/// </summary>
	/// <param name="partMemoId">The Unique Id of the PartMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartMemo DTO.</returns>
	Task<ERPPartMemoInformationDto> GetPartMemo(Guid partMemoId);

	/// <summary>
	/// Saves the provided ERP partMemo.
	/// </summary>
	/// <param name="partMemo">The ERP partMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartMemo(ERPPartMemoDto partMemo);
}
