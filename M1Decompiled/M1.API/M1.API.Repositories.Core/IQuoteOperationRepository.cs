using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;

namespace M1.API.Repositories.Core;

public interface IQuoteOperationRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuoteOperation with the specified ID exists.
	/// </summary>
	/// <param name="quoteOperationId">The ID of the QuoteOperation to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteOperation exists or not.</returns>
	Task<bool> DoesQuoteOperationExists(string quoteOperationId);

	/// <summary>
	/// Retrieves all QuoteOperation with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteOperations DTOs.</returns>
	Task<ICollection<BOMQuoteOperationDto>> GetAllQuoteOperations(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves a list of <see cref="T:M1.API.DTOs.BOM.BOMQuoteOperationDto" /> objects related to a specific quote.
	/// </summary>
	/// <param name="quoteId">The unique identifier for the quote.</param>
	/// <param name="quoteLineId">Optional. The unique identifier for the quote line associated with the quote.</param>
	/// <param name="quoteAssemblyId">Optional. The unique identifier for the quote assembly associated with the quote.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteOperations DTOs.</returns>
	Task<ICollection<BOMQuoteOperationDto>> GetQuoteOperationsAsync(string quoteId, string quoteLineId, string quoteAssemblyId);
}
