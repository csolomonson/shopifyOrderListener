using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLeadMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LeadMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="leadMemoId">The Unique Id of the LeadMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LeadMemo exists or not.</returns>
	Task<bool> DoesLeadMemoExist(Guid leadMemoId);

	/// <summary>
	/// Retrieves all LeadMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LeadMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LeadMemos DTOs.</returns>
	Task<ICollection<ERPLeadMemoInformationDto>> GetAllLeadMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LeadMemo.
	/// </summary>
	/// <param name="leadMemoId">The Unique Id of the LeadMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LeadMemo DTO.</returns>
	Task<ERPLeadMemoInformationDto> GetLeadMemo(Guid leadMemoId);

	/// <summary>
	/// Saves the provided ERP leadMemo.
	/// </summary>
	/// <param name="leadMemo">The ERP leadMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLeadMemo(ERPLeadMemoDto leadMemo);
}
