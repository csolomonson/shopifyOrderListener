using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPQuoteMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuoteMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="quoteMemoId">The Unique Id of the QuoteMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteMemo exists or not.</returns>
	Task<bool> DoesQuoteMemoExist(Guid quoteMemoId);

	/// <summary>
	/// Retrieves all QuoteMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteMemos DTOs.</returns>
	Task<ICollection<ERPQuoteMemoInformationDto>> GetAllQuoteMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific QuoteMemo.
	/// </summary>
	/// <param name="quoteMemoId">The Unique Id of the QuoteMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the QuoteMemo DTO.</returns>
	Task<ERPQuoteMemoInformationDto> GetQuoteMemo(Guid quoteMemoId);

	/// <summary>
	/// Saves the provided ERP quoteMemo.
	/// </summary>
	/// <param name="quoteMemo">The ERP quoteMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuoteMemo(ERPQuoteMemoDto quoteMemo);
}
