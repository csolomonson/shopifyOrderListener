using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRFQMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RFQMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="rFQMemoId">The Unique Id of the RFQMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RFQMemo exists or not.</returns>
	Task<bool> DoesRFQMemoExist(Guid rFQMemoId);

	/// <summary>
	/// Retrieves all RFQMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RFQMemos DTOs.</returns>
	Task<ICollection<ERPRFQMemoInformationDto>> GetAllRFQMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RFQMemo.
	/// </summary>
	/// <param name="rFQMemoId">The Unique Id of the RFQMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RFQMemo DTO.</returns>
	Task<ERPRFQMemoInformationDto> GetRFQMemo(Guid rFQMemoId);

	/// <summary>
	/// Saves the provided ERP rFQMemo.
	/// </summary>
	/// <param name="rFQMemo">The ERP rFQMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRFQMemo(ERPRFQMemoDto rFQMemo);
}
