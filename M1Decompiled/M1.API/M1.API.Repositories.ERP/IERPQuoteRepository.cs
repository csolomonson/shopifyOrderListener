using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPQuoteRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Quote with the specified Unique Id exists.
	/// </summary>
	/// <param name="quoteId">The Unique Id of the Quote to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Quote exists or not.</returns>
	Task<bool> DoesQuoteExist(Guid quoteId);

	/// <summary>
	/// Retrieves all Quotes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Quotes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Quotes DTOs.</returns>
	Task<ICollection<ERPQuoteInformationDto>> GetAllQuotes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Quote.
	/// </summary>
	/// <param name="quoteId">The Unique Id of the Quote to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Quote DTO.</returns>
	Task<ERPQuoteInformationDto> GetQuote(Guid quoteId);

	/// <summary>
	/// Saves the provided ERP quote.
	/// </summary>
	/// <param name="quote">The ERP quote to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuote(ERPQuoteDto quote);
}
