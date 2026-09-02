using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;

namespace M1.API.Repositories.Core.Sales;

public interface IQuoteLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuoteLine with the specified ID exists.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to check.</param>
	/// <param name="quoteLineId">The ID of the QuoteLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteLine exists or not.</returns>
	Task<bool> DoesQuoteLineExists(string quoteId, string quoteLineId);

	/// <summary>
	/// Retrieves all QuoteLine with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteLines DTOs.</returns>
	Task<ICollection<BOMQuoteLineDto>> GetAllQuoteLines(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific QuoteLine.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to retrieve information for.</param>
	/// <param name="quoteLineId">The ID of the QuoteLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the QuoteLine DTO.</returns>
	Task<BOMQuoteLineDto> GetQuoteLine(string quoteId, string quoteLineId);

	/// <summary>
	/// Saves the provided BOM quoteLine.
	/// </summary>
	/// <param name="quoteLine">The BOM quote line to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuoteLineAsync(BOMCreateQuoteLineDto quoteLine);
}
