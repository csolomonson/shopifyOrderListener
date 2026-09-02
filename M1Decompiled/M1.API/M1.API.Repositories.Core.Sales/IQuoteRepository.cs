using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;

namespace M1.API.Repositories.Core.Sales;

public interface IQuoteRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Quote with the specified ID exists.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Quote exists or not.</returns>
	Task<bool> DoesQuoteExistsAsync(string quoteId);

	/// <summary>
	/// Retrieves the quote ID associated with the specified GUID.
	/// </summary>
	/// <param name="guidOut">The GUID used to find the corresponding quote ID.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the quote ID as a string.
	/// </returns>
	Task<string> GetQuoteIdFromGuidAsync(Guid guidOut);

	/// <summary>
	/// Retrieves all Quote with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Quotes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of Quotes DTOs.</returns>
	Task<ICollection<BOMQuoteDto>> GetAllQuotesAsync(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific Quote.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Quote DTO.</returns>
	Task<BOMQuoteDto> GetQuoteAsync(string quoteId);

	/// <summary>
	/// Retrieves the information of quote lines based on the provided quote ID.
	/// </summary>
	/// <param name="quoteId">The ID of the quote for which to retrieve line information.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a list of 
	/// <see cref="T:M1.API.DTOs.BOM.Sales.BOMQuoteLineDto" /> objects.
	/// </returns>
	Task<IList<BOMQuoteLineDto>> GetQuoteLinesInfoAsync(string quoteId);

	/// <summary>
	/// Saves the provided BOM quote.
	/// </summary>
	/// <param name="quote">The BOM quote to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuoteAsync(BOMCreateQuoteDto quote);
}
