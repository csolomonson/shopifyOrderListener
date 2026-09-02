using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPQuoteLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuoteLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="quoteLineId">The Unique Id of the QuoteLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteLine exists or not.</returns>
	Task<bool> DoesQuoteLineExist(Guid quoteLineId);

	/// <summary>
	/// Retrieves all QuoteLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteLines DTOs.</returns>
	Task<ICollection<ERPQuoteLineInformationDto>> GetAllQuoteLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific QuoteLine.
	/// </summary>
	/// <param name="quoteLineId">The Unique Id of the QuoteLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the QuoteLine DTO.</returns>
	Task<ERPQuoteLineInformationDto> GetQuoteLine(Guid quoteLineId);

	/// <summary>
	/// Saves the provided ERP quoteLine.
	/// </summary>
	/// <param name="quoteLine">The ERP quoteLine to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuoteLine(ERPQuoteLineDto quoteLine);
}
