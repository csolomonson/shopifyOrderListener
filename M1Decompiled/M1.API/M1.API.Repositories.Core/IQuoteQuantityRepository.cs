using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;

namespace M1.API.Repositories.Core;

public interface IQuoteQuantityRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuoteQuantity with the specified ID exists.
	/// </summary>
	/// <param name="quoteQuantityId">The ID of the QuoteQuantity to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteQuantity exists or not.</returns>
	Task<bool> DoesQuoteQuantityExists(string quoteQuantityId);

	/// <summary>
	/// Retrieves all QuoteQuantity with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteQuantities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteQuantities DTOs.</returns>
	Task<ICollection<BOMQuoteQuantityDto>> GetAllQuoteQuantities(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about QuoteQuantities.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to retrieve information for.</param>
	/// <param name="quoteLineId">The ID of the QuoteLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteQuantities DTOs for a giving Quote and QuoteLine Id.</returns>
	Task<IList<BOMQuoteQuantityDto>> GetQuoteQuantitiesInfo(string quoteId, string quoteLineId);
}
